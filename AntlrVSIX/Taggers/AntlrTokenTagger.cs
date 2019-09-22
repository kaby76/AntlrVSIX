
namespace AntlrVSIX.Tagger
{
    using Antlr4.Runtime;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.GrammarDescription;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            _grammar_description = GrammarDescriptionFactory.Create(ffn);
            if (_grammar_description == null) return;

            _antlr_tag_types = new Dictionary<string, int>();
            for (int i = 0; i < _grammar_description.Map.Length; ++i)
            {
                string name = _grammar_description.Map[i];
                _antlr_tag_types[name] = i;
            }

            ITextDocument document = _buffer.GetTextDocument();
            string file_name = document.FilePath;
            string code = _buffer.GetBufferText();
            var pd = ParserDetails.Parse(code, file_name);
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

                ParserDetails details = null;
                bool found = ParserDetails._per_file_parser_details.TryGetValue(file_name, out details);
                if (!found) continue;

                SnapshotPoint start = curSpan.Start;
                int curLocStart = start.Position;
                SnapshotPoint end = curSpan.End;
                int curLocEnd = end.Position;

                var combined_tokens = new List<KeyValuePair<IToken, int>>();

                combined_tokens.AddRange(
                    details._refs.Where((pair) =>
                    {
                        var token = pair.Key;
                        int start_token_start = token.Symbol.StartIndex;
                        int end_token_end = token.Symbol.StopIndex;
                        if (start_token_start >= curLocEnd) return false;
                        if (end_token_end < curLocStart) return false;
                        return true;
                    }).Select( (pair) => new KeyValuePair<IToken, int>(pair.Key.Symbol, pair.Value)));

                combined_tokens.AddRange(details._defs.Where((pair) =>
                    {
                        var token = pair.Key;
                        int start_token_start = token.Symbol.StartIndex;
                        int end_token_end = token.Symbol.StopIndex;
                        if (start_token_start >= curLocEnd) return false;
                        if (end_token_end < curLocStart) return false;
                        return true;
                    }).Select((pair) => new KeyValuePair<IToken, int>(pair.Key.Symbol, pair.Value)));

                combined_tokens.AddRange(details._ant_comments.Where((pair) =>
                    {
                        var token = pair.Key;
                        int start_token_start = token.StartIndex;
                        int end_token_end = token.StopIndex;
                        if (start_token_start >= curLocEnd) return false;
                        if (end_token_end < curLocStart) return false;
                        return true;
                    }));

                // Sort the list.
                var sorted_combined_tokens = combined_tokens.OrderBy((t) => t.Key.StartIndex);

                foreach (var p in sorted_combined_tokens)
                {
                    var token = p.Key;
                    var type = p.Value;
                    int start_token_start = token.StartIndex;
                    int end_token_end = token.StopIndex;
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
