
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
        private IDictionary<string, int> _antlr_tag_types;

        internal AntlrTokenTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
            var doc = _buffer.GetTextDocument();
            if (doc == null) return;
            var ffn = doc.FilePath;
            _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
            if (_grammar_description == null) return;
            var item = Workspaces.Workspace.Instance.FindDocument(ffn);
            if (item == null) return;

            _antlr_tag_types = new Dictionary<string, int>();
            for (int i = 0; i < _grammar_description.Map.Length; ++i)
            {
                string name = _grammar_description.Map[i];
                _antlr_tag_types[name] = i;
            }
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
                    await Task.Delay(TimeSpan.FromSeconds(5), source.Token);
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
            ITextDocument document = doc.GetTextDocument();
            string ffn = document.FilePath;
            var item = Workspaces.Workspace.Instance.FindDocument(ffn);
            if (item == null) return;
            item.Code = code;
            LanguageServer.Module.Compile();
            SnapshotSpan span = new SnapshotSpan(_buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length);
            var temp = TagsChanged;
            if (temp == null)
                return;
            temp(this, new SnapshotSpanEventArgs(span));
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public IEnumerable<ITagSpan<AntlrTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (_grammar_description == null) yield break;
            if (_grammar_description == null) throw new Exception();
            foreach (SnapshotSpan curSpan in spans)
            {
                ITextSnapshotLine containingLine = curSpan.Start.GetContainingLine();
                int curLoc = containingLine.Start.Position;
                string text = curSpan.GetText();
                ITextBuffer buf = curSpan.Snapshot.TextBuffer;
                var doc = buf.GetTextDocument();
                string file_name = doc.FilePath;
                var item = Workspaces.Workspace.Instance.FindDocument(file_name);
                if (item == null) continue;
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
