
namespace AntlrVSIX.Tagger
{
    using AntlrVSIX.Extensions;
    using LanguageServer;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class AntlrTokenTagger : ITagger<AntlrTokenTag>
    {
        private ITextBuffer _buffer;
        private IGrammarDescription _grammar_description;

        internal AntlrTokenTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
            buffer.Changed += new EventHandler<TextContentChangedEventArgs>(OnTextChanged);
        }

        static CancellationTokenSource source;
        static Task<int> task;

        async void OnTextChanged(object sender, TextContentChangedEventArgs args)
        {
            var s = source;
            var t = task;
            if (s == null)
            {
                source = new CancellationTokenSource();
                task = Task.Run(async delegate
                {
                    await Task.Delay(TimeSpan.FromSeconds(3), source.Token);
                    return 42;
                });
                try
                {
                    await task;
                }
                catch (Exception)
                {
                }
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    ReparseFile(sender, args);
                }
                s = source;
                t = task;
                if (s != null) s.Dispose();
                if (t != null) t.Dispose();
                source = null;
                task = null;
            }
            else
            {
                s.Cancel();
                try
                {
                    await t;
                }
                catch (Exception)
                {
                }
                OnTextChanged(sender, args);
            }
        }

        void ReparseFile(object sender, TextContentChangedEventArgs args)
        {
            ITextBuffer doc = sender as ITextBuffer;
            ITextSnapshot snapshot = doc.CurrentSnapshot;
            string code = doc.GetBufferText();
            string ffn = doc.GetFFN().Result;
            var item = Workspaces.Workspace.Instance.FindDocument(ffn);
            if (item == null) return;
            item.Code = code;
            var to_do = LanguageServer.Module.Compile();
            SnapshotSpan span = new SnapshotSpan(_buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length);
            EventHandler<SnapshotSpanEventArgs> temp = TagsChanged;
            if (temp == null)
                return;
            temp(this, new SnapshotSpanEventArgs(span));
        }

        public void Raise()
        {
            SnapshotSpan span = new SnapshotSpan(_buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length);
            EventHandler<SnapshotSpanEventArgs> temp = TagsChanged;
            if (temp == null)
                return;
            temp(this, new SnapshotSpanEventArgs(span));
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public IEnumerable<ITagSpan<AntlrTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            var ffn = _buffer.GetFFN().Result;
            if (ffn == null) yield break;
            _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
            if (_grammar_description == null) yield break;
            var item = Workspaces.Workspace.Instance.FindDocument(ffn);
            if (item == null) yield break;
            foreach (SnapshotSpan curSpan in spans)
            {
                ITextSnapshotLine containingLine = curSpan.Start.GetContainingLine();
                int curLoc = containingLine.Start.Position;
                string text = curSpan.GetText();
                var details = ParserDetailsFactory.Create(item);
                SnapshotPoint start = curSpan.Start;
                int curLocStart = start.Position;
                SnapshotPoint end = curSpan.End;
                int curLocEnd = end.Position;
                var dpc_sym = LanguageServer.Module.GetDocumentSymbol(curLocStart, item);
                var sorted_combined_tokens = LanguageServer.Module.Get(
                    new Range(start.Position, end.Position),
                    item);
                foreach (DocumentSymbol p in sorted_combined_tokens)
                {
                    int start_token_start = p.range.Start.Value;
                    int end_token_end = p.range.End.Value;
                    int type = p.kind;
                    int length = end_token_end - start_token_start + 1;

                    // Make sure the length doesn't go past the end of the current span.
                    if (start_token_start + length > curLocEnd)
                        length = curLocEnd - start_token_start;

                    var tokenSpan = new SnapshotSpan(
                        curSpan.Snapshot,
                        new Span(start_token_start, length));

                    if (tokenSpan.IntersectsWith(curSpan))
                    {
                        if (type >= 0)
                        {
                            TagSpan<AntlrTokenTag> t = new TagSpan<AntlrTokenTag>(tokenSpan, new AntlrTokenTag(type));
                            yield return t;
                        }
                    }
                }
            }
        }
    }
}
