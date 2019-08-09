
namespace AntlrVSIX.Tagger
{
    using Antlr4.Runtime;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;

    internal sealed class AntlrTokenTagger : ITagger<AntlrTokenTag>
    {
        private ITextBuffer _buffer;
        private IDictionary<string, AntlrTagTypes> _antlr_tag_types;

        internal AntlrTokenTagger(ITextBuffer buffer)
        {
            _buffer = buffer;

            _antlr_tag_types = new Dictionary<string, AntlrTagTypes>();
            _antlr_tag_types[Constants.ClassificationNameNonterminal] = AntlrTagTypes.Nonterminal;
            _antlr_tag_types[Constants.ClassificationNameTerminal] = AntlrTagTypes.Terminal;
            _antlr_tag_types[Constants.ClassificationNameComment] = AntlrTagTypes.Comment;
            _antlr_tag_types[Constants.ClassificationNameKeyword] = AntlrTagTypes.Keyword;
            _antlr_tag_types[Constants.ClassificationNameLiteral] = AntlrTagTypes.Literal;
            _antlr_tag_types["other"] = AntlrTagTypes.Other;

            ITextDocument document = _buffer.GetTextDocument();
            string file_name = document.FilePath;
            if (!file_name.IsAntlrSuffix()) return;

            string code = _buffer.GetBufferText();
            var pd = ParserDetails.Parse(code, file_name);
        }

        void ReparseFile(object sender, TextContentChangedEventArgs args)
        {
            ITextSnapshot snapshot = _buffer.CurrentSnapshot;
            string code = _buffer.GetBufferText();
            ITextDocument document = _buffer.GetTextDocument();
            string file_name = document.FilePath;
            if (!file_name.IsAntlrSuffix()) return;
            var pd = ParserDetails.Parse(code, file_name);
            SnapshotSpan span = new SnapshotSpan(_buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length);
            var temp = TagsChanged;
            if (temp == null)
                return;
            //temp(this, new SnapshotSpanEventArgs(span));
        }


        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        // For each span of text given, perform a complete parse, and reclassify and tag new spans.
        public IEnumerable<ITagSpan<AntlrTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            // Nothing graceful here in relating a span to a part in the
            // syntax tree. For now, get the bounds of the span, find tree nodes
            // that overlap the span. Find all nonterminals
            // and terminals to mark the span up. Likewise, for all comments,
            // find all that overlap span. Sort all terminals, nonterminals,
            // and comments into a list, and package it up as new TagSpan.

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

                // Collect all nonterminals, terminals, ..., in this span.
                IEnumerable<IToken> combined_tokens = new List<IToken>();
                List<IToken> all_term_tokens = new List<IToken>();
                List<IToken> all_nonterm_tokens = new List<IToken>();
                List<IToken> all_comment_tokens = new List<IToken>();
                List<IToken> all_keyword_tokens = new List<IToken>();
                List<IToken> all_literal_tokens = new List<IToken>();

                all_nonterm_tokens = details._ant_nonterminals.Where((token) =>
                {
                    int start_token_start = token.StartIndex;
                    int end_token_end = token.StopIndex;
                    if (start_token_start >= curLocEnd) return false;
                    if (end_token_end < curLocStart) return false;
                    return true;
                }).ToList();
                combined_tokens = combined_tokens.Concat(all_nonterm_tokens);
                all_term_tokens = details._ant_terminals.Where((token) =>
                {
                    int start_token_start = token.StartIndex;
                    int end_token_end = token.StopIndex;
                    if (start_token_start >= curLocEnd) return false;
                    if (end_token_end < curLocStart) return false;
                    return true;
                }).ToList();
                combined_tokens = combined_tokens.Concat(all_term_tokens);
                all_comment_tokens = details._ant_comments.Where((token) =>
                {
                    int start_token_start = token.StartIndex;
                    int end_token_end = token.StopIndex;
                    if (start_token_start >= curLocEnd) return false;
                    if (end_token_end < curLocStart) return false;
                    return true;
                }).ToList();
                combined_tokens = combined_tokens.Concat(all_comment_tokens);
                all_keyword_tokens = details._ant_keywords.Where((token) =>
                {
                    int start_token_start = token.StartIndex;
                    int end_token_end = token.StopIndex;
                    if (start_token_start >= curLocEnd) return false;
                    if (end_token_end < curLocStart) return false;
                    return true;
                }).ToList();
                combined_tokens = combined_tokens.Concat(all_keyword_tokens);
                all_literal_tokens = details._ant_literals.Where((token) =>
                {
                    int start_token_start = token.StartIndex;
                    int end_token_end = token.StopIndex;
                    if (start_token_start >= curLocEnd) return false;
                    if (end_token_end < curLocStart) return false;
                    return true;
                }).ToList();
                combined_tokens = combined_tokens.Concat(all_literal_tokens);

                // Sort the list.
                var sorted_combined_tokens = combined_tokens.OrderBy((t) => t.StartIndex).ToList();

                // Assumption: tokens do not overlap.

                foreach (IToken token in sorted_combined_tokens)
                {
                    int start_token_start = token.StartIndex;
                    int end_token_end = token.StopIndex;
                    int length = end_token_end - start_token_start + 1;

                    // Make sure the length doesn't go past the end of the current span.
                    if (start_token_start + length > curLocEnd)
                        length = curLocEnd - start_token_start;

                    var tokenSpan = new SnapshotSpan(
                        curSpan.Snapshot,
                        new Span(start_token_start, length));

                    AntlrTagTypes type;
                    if (all_term_tokens.Contains(token)) type = AntlrTagTypes.Terminal;
                    else if (all_nonterm_tokens.Contains(token)) type = AntlrTagTypes.Nonterminal;
                    else if (all_comment_tokens.Contains(token)) type = AntlrTagTypes.Comment;
                    else if (all_keyword_tokens.Contains(token)) type = AntlrTagTypes.Keyword;
                    else if (all_literal_tokens.Contains(token)) type = AntlrTagTypes.Literal;
                    else
                        type = AntlrTagTypes.Other;

                    if (tokenSpan.IntersectsWith(curSpan))
                    {
                        TagSpan<AntlrTokenTag> t = new TagSpan<AntlrTokenTag>(tokenSpan, new AntlrTokenTag(type));
                        yield return t;
                    }
                }
            }
        }
    }
}
