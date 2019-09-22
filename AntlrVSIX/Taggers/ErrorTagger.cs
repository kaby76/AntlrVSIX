﻿
namespace AntlrVSIX.ErrorTagger
{
    using Antlr4.Runtime;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text.Adornments;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Threading;
    using System;

    class ErrorTagger : ITagger<IErrorTag>
    {
        private ITextBuffer _buffer;

        public ErrorTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
            var doc = _buffer.GetFilePath();
            var gd = GrammarDescriptionFactory.Create(doc);
            if (gd == null) return;
            if (!gd.DoErrorSquiggles) return;

            ThreadHelper.Generic.BeginInvoke(DispatcherPriority.ApplicationIdle, () =>
            {
                var span = new SnapshotSpan(buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length);
                TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(span));
            });
        }

        private void DocumentValidated(object sender, EventArgs e)
        {
            var span = new SnapshotSpan(_buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length);
            TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(span));
        }

        public IEnumerable<ITagSpan<IErrorTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            var doc = _buffer.GetFilePath();
            var gd = GrammarDescriptionFactory.Create(doc);
            if (gd == null) yield break;
            if (!gd.DoErrorSquiggles) yield break;

            foreach (SnapshotSpan curSpan in spans)
            {
                ITextSnapshotLine containingLine = curSpan.Start.GetContainingLine();
                int curLoc = containingLine.Start.Position;
                string text = curSpan.GetText();
                ITextBuffer buf = curSpan.Snapshot.TextBuffer;
                var dd = buf.GetTextDocument();
                string ffn = dd.FilePath;
                string path_containing_applied_occurrence = Path.GetDirectoryName(ffn);
                ParserDetails details = null;
                bool found = ParserDetails._per_file_parser_details.TryGetValue(ffn, out details);
                if (!found) continue;
                SnapshotPoint start = curSpan.Start;
                int curLocStart = start.Position;
                SnapshotPoint end = curSpan.End;
                int curLocEnd = end.Position;
                List<Antlr4.Runtime.Tree.TerminalNodeImpl> where = new List<Antlr4.Runtime.Tree.TerminalNodeImpl>();
                List<ParserDetails> where_details = new List<ParserDetails>();
                IEnumerable<Antlr4.Runtime.Tree.TerminalNodeImpl> combined_tokens = new List<Antlr4.Runtime.Tree.TerminalNodeImpl>();
                List<Antlr4.Runtime.Tree.TerminalNodeImpl> all_term_tokens = new List<Antlr4.Runtime.Tree.TerminalNodeImpl>();
                List<Antlr4.Runtime.Tree.TerminalNodeImpl> all_nonterm_tokens = new List<Antlr4.Runtime.Tree.TerminalNodeImpl>();

                all_nonterm_tokens = details._refs.Where((pair) =>
                {
                    var token = pair.Key;
                    var classification = pair.Value;
                    if (classification != 0) return false;
                    int start_token_start = token.Symbol.StartIndex;
                    int end_token_end = token.Symbol.StopIndex;
                    if (start_token_start >= curLocEnd) return false;
                    if (end_token_end < curLocStart) return false;
                    var is_any_definer = ParserDetails._per_file_parser_details
                        .Where(pd =>
                            (Path.GetDirectoryName(pd.Value.FullFileName) == path_containing_applied_occurrence)
                            && pd.Value._defs.Where(
                                p =>
                                {
                                    var d = p.Key;
                                    var c = p.Value;
                                    if (c != 0) return false;
                                    return d.Symbol.Text == token.Symbol.Text;
                                }).Any()).Any();
                    return !is_any_definer;
                }).Select(t => t.Key).ToList();

                combined_tokens = combined_tokens.Concat(all_nonterm_tokens);

                all_nonterm_tokens = details._refs.Where((pair) =>
                {
                    var token = pair.Key;
                    var classification = pair.Value;
                    if (classification != 1) return false;
                    int start_token_start = token.Symbol.StartIndex;
                    int end_token_end = token.Symbol.StopIndex;
                    if (start_token_start >= curLocEnd) return false;
                    if (end_token_end < curLocStart) return false;
                    var is_any_definer = ParserDetails._per_file_parser_details
                        .Where(pd =>
                            (Path.GetDirectoryName(pd.Value.FullFileName) == path_containing_applied_occurrence)
                            && pd.Value._defs.Where(
                                p =>
                                {
                                    var d = p.Key;
                                    var c = p.Value;
                                    if (c != 1) return false;
                                    return d.Symbol.Text == token.Symbol.Text;
                                }).Any()).Any();
                    return !is_any_definer;
                }).Select(t => t.Key).ToList();

                combined_tokens = combined_tokens.Concat(all_term_tokens);

                // Sort the list.
                var sorted_combined_tokens = combined_tokens.OrderBy((t) => t.Symbol.StartIndex).ToList();

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

                    ErrorCategory type = ErrorCategory.Error;

                    if (tokenSpan.IntersectsWith(curSpan))
                    {
                        string errorType = GetErrorType(type);
                        TagSpan<IErrorTag> t = new TagSpan<IErrorTag>(tokenSpan, new ErrorTag(errorType));
                        yield return t;
                    }
                }
            }
        }

        public static string GetErrorType(ErrorCategory errorType)
        {
            switch (errorType)
            {
                case ErrorCategory.Error:
                    return PredefinedErrorTypeNames.SyntaxError;
                case ErrorCategory.Warning:
                    return PredefinedErrorTypeNames.Warning;
            }
            return ErrorFormatDefinition.Suggestion;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
    }
}
