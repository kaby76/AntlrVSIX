using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using AntlrLanguage.Extensions;
using AntlrLanguage.Grammar;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.TextManager.Interop;

namespace AntlrLanguage.Tag
{
    /// <summary>
    /// AntlrTokenTagger is the basic tagging facility of this extension.
    /// The editor buffer is contained in _buffer. Please refer to
    /// https://msdn.microsoft.com/en-us/library/dd885240.aspx for more
    /// information on the editor and tagging.
    /// </summary>
    internal sealed class AntlrTokenTagger : ITagger<AntlrTokenTag>
    {
        private ITextBuffer _buffer;
        private ITextView _view;
        private SVsServiceProvider _service_provider;

        // Tagging information.
        IDictionary<string, AntlrTokenTypes> _antlrTypes;

        private string GetAntText()
        {
            return _buffer.CurrentSnapshot.GetText();
        }

        public IEnumerable<ProjectItem> Recurse(ProjectItems i)
        {
            if (i != null)
            {
                foreach (ProjectItem j in i)
                {
                    foreach (ProjectItem k in Recurse(j))
                    {
                        yield return k;
                    }
                }
            }
        }

        public IEnumerable<ProjectItem> Recurse(ProjectItem i)
        {
            yield return i;
            foreach (ProjectItem j in Recurse(i.ProjectItems))
            {
                yield return j;
            }
        }

        public IEnumerable<ProjectItem> SolutionFiles(DTE application)
        {
            Solution solution = (Solution)application.Solution;
            string solution_full_name = solution.FullName;
            string solution_file_name = solution.FileName;
            Properties solution_properties = solution.Properties;
            foreach (Project project in solution.Projects)
            {
                string project_full_name = project.FullName;
                string project_file_name = project.FileName;
                Properties project_properties = project.Properties;
                if (solution_properties != null)
                foreach (ProjectItem item in Recurse(project.ProjectItems))
                {
                    yield return item;
                }
            }
        }

        internal AntlrTokenTagger(ITextView view, ITextBuffer buffer, SVsServiceProvider service_provider)
        {
            _buffer = buffer;
            _view = view;
            var pp = _buffer.Properties;

            _antlrTypes = new Dictionary<string, AntlrTokenTypes>();
            _antlrTypes["nonterminal"] = AntlrTokenTypes.Nonterminal;
            _antlrTypes["terminal"] = AntlrTokenTypes.Terminal;
            _antlrTypes["acomment"] = AntlrTokenTypes.Comment;
            _antlrTypes["akeyword"] = AntlrTokenTypes.Keyword;
            _antlrTypes["other"] = AntlrTokenTypes.Other;

            // Get VS solution, if any, and parse all grammars
            DTE dte = null;
            if (service_provider != null)
                dte = (DTE)service_provider.GetService(typeof(DTE));
            DTE application = null;
            if (dte != null)
                application = dte.Application;
            if (application != null)
            {
                IEnumerable<ProjectItem> iterator = SolutionFiles(application);
                ProjectItem[] list = iterator.ToArray();
                foreach (var item in list)
                {
                    //var doc = item.Document; CRASHES!!!! DO NOT USE!
                    //var props = item.Properties;
                    string file_name = item.Name;
                    if (file_name != null)
                    {
                        string prefix = file_name.TrimSuffix(".g4");
                        if (prefix == file_name) continue;
                        IVsTextView w = file_name.GetIVsTextView();
                        // Window window = item.Open(); Yep this code doesn't work either--of course.
                        //if (w == null) file_name.OpenStupidFile(); works, but re-entrancy problem.
                    }
                }
            }

            var text = GetAntText();
            var doc2 = buffer.GetTextDocument();
            string file_name2 = doc2.FilePath;
            ParserDetails foo = new ParserDetails();
            ParserDetails._per_file_parser_details[file_name2] = foo;
            foo.Parse(text, file_name2);
            this._buffer.Changed += OnTextBufferChanged;
        }

        public event EventHandler ClassificationChanged;
        private void OnTextBufferChanged(object sender, TextContentChangedEventArgs e)
        {
            //ClassificationChanged(this, new ClassificationChangedEventArgs(
            // new SnapshotSpan(_buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length)));
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        // For each span of text given, perform a complete parse, and reclassify new spans with
        // the correct tag.
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

                    AntlrTokenTypes type;
                    if (all_term_tokens.Contains(token)) type = AntlrTokenTypes.Terminal;
                    else if (all_nonterm_tokens.Contains(token)) type = AntlrTokenTypes.Nonterminal;
                    else if (all_comment_tokens.Contains(token)) type = AntlrTokenTypes.Comment;
                    else if (all_keyword_tokens.Contains(token)) type = AntlrTokenTypes.Keyword;
                    else
                        type = AntlrTokenTypes.Other;

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
