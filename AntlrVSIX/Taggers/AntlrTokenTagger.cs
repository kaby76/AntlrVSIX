
namespace AntlrVSIX.Tagger
{
    using AntlrVSIX.Extensions;
    using LanguageServer;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class AntlrTokenTagger : ITagger<AntlrTokenTag>
    {
        private ITextBuffer _buffer;

        internal AntlrTokenTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public IEnumerable<ITagSpan<AntlrTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            // Note that this buffer is likely an ElisionBuffer.
            var ffn = _buffer.GetFFN().Result;
            if (ffn == null) yield break;
            var grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
            if (grammar_description == null) yield break;
            var document = Workspaces.Workspace.Instance.FindDocument(ffn);
            if (document == null) yield break;
            var details = ParserDetailsFactory.Create(document);
            foreach (SnapshotSpan curSpan in spans)
            {
                SnapshotPoint start = curSpan.Start;
                SnapshotPoint end = curSpan.End;
                var text = curSpan.GetText();
                int curLocStart = start.Position;
                int curLocEnd = end.Position;
                var dpc_sym = LanguageServer.Module.GetDocumentSymbol(curLocStart, document);
                var sorted_combined_tokens = LanguageServer.Module.Get(
                    new Range(curLocStart, curLocEnd - 1),
                    document);
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
