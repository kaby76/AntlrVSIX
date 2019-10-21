
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
    using System.Linq;

    internal sealed class AntlrTokenTagger : ITagger<AntlrTokenTag>
    {
        private ITextBuffer _buffer;
        private IGrammarDescription _grammar_description;

        internal AntlrTokenTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
          //  buffer.Changed += new EventHandler<TextContentChangedEventArgs>(OnTextChanged);
        }

        static CancellationTokenSource source;
        static Task<int> task;

        async void OnTextChanged(object sender, TextContentChangedEventArgs args)
        {
            var s = source;
            var t = task;
            if (s != null)
            {
                s.Cancel();
            }
            {
                s = source = new CancellationTokenSource();
                t = task = Task.Run(async delegate
                {
                    await Task.Delay(TimeSpan.FromSeconds(3), s.Token);
                    return 42;
                });
                try
                {
                    await t;
                }
                catch (Exception)
                {
                }
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    ReparseFile(sender, args);
                }
                source = null;
                task = null;
            }
        }

        void ReparseFile(object sender, TextContentChangedEventArgs args)
        {
            ITextBuffer buffer = sender as ITextBuffer;
            ITextSnapshot snapshot = buffer.CurrentSnapshot;
            string code = buffer.GetBufferText();
            string ffn = buffer.GetFFN().Result;
            var item = Workspaces.Workspace.Instance.FindDocument(ffn);
            if (item == null) return;
            item.Code = code;
            var to_do = LanguageServer.Module.Compile();
            lock (updateLock)
            {
                SnapshotSpan span = new SnapshotSpan(_buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length);
                EventHandler<SnapshotSpanEventArgs> temp = TagsChanged;
                var i = _buffer as Microsoft.VisualStudio.Text.Projection.IElisionBuffer;
                if (i != null)
                {
                    var j = i.SourceBuffer;
                }
                else
                {

                }
                if (temp == null)
                    return;
                temp(this, new SnapshotSpanEventArgs(span));
            }
        }

        object updateLock = new object();

        public void Raise()
        {
            return;
            lock (updateLock)
            {
                SnapshotSpan span = new SnapshotSpan(_buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length);
                EventHandler<SnapshotSpanEventArgs> temp = TagsChanged;
                if (temp == null)
                    return;
                temp(this, new SnapshotSpanEventArgs(span));
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public IEnumerable<ITagSpan<AntlrTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            // Note that this buffer is likely an ElisionBuffer.
            var ffn = _buffer.GetFFN().Result;
            if (ffn == null) yield break;
            _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
            if (_grammar_description == null) yield break;
            var item = Workspaces.Workspace.Instance.FindDocument(ffn);
            if (item == null) yield break;
            var details = ParserDetailsFactory.Create(item);
            foreach (SnapshotSpan curSpan in spans)
            {
                SnapshotPoint start = curSpan.Start;
                SnapshotPoint end = curSpan.End;
                var text = curSpan.GetText();
                int curLocStart = start.Position;
                int curLocEnd = end.Position;
                var dpc_sym = LanguageServer.Module.GetDocumentSymbol(curLocStart, item);
                var sorted_combined_tokens = LanguageServer.Module.Get(
                    new Range(curLocStart, curLocEnd - 1),
                    item);
                var list = sorted_combined_tokens.ToList();
                foreach (DocumentSymbol p in sorted_combined_tokens)
                {
                    int start_token_start = p.range.Start.Value;
                    int end_token_end = p.range.End.Value;
                    int type = p.kind;
                    int length = end_token_end - start_token_start + 1;

                    // Make sure the length doesn't go past the end of the current span.
                    if (start_token_start + length > curLocEnd)
                        length = curLocEnd - start_token_start;

                    var a = curSpan.Snapshot.TextBuffer.CurrentSnapshot;
                    var b = curSpan.Snapshot;

                    var tokenSpan = new SnapshotSpan(
                        curSpan.Snapshot.TextBuffer.CurrentSnapshot,
                        //curSpan.Snapshot,
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
