using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using AntlrLanguage.Grammar;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

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
        ITextBuffer _buffer;

        // List of all nonterminals and terminals in the given grammar.
        private IList<string> _ant_nonterminals_names;
        private IList<string> _ant_terminals_names;

        // List of all comments, terminal, nonterminal, and keyword tokens in the grammar.
        private IList<IToken> _ant_terminals;
        private IList<IToken> _ant_nonterminals;
        private IList<IToken> _ant_comments;
        private IList<IToken> _ant_keywords;

        // Parser and parse tree.
        private ANTLRv4Parser _ant_parser = null;
        IParseTree _ant_tree = null;
        IParseTree[] _all_nodes = null;

        IDictionary<string, AntlrTokenTypes> _antlrTypes;

        private string GetAntText()
        {
            return _buffer.CurrentSnapshot.GetText();
        }

        internal AntlrTokenTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
            _antlrTypes = new Dictionary<string, AntlrTokenTypes>();
            _antlrTypes["nonterminal"] = AntlrTokenTypes.Nonterminal;
            _antlrTypes["terminal"] = AntlrTokenTypes.Terminal;
            _antlrTypes["acomment"] = AntlrTokenTypes.Comment;
            _antlrTypes["akeyword"] = AntlrTokenTypes.Keyword;
            _antlrTypes["other"] = AntlrTokenTypes.Other;

            var text = GetAntText();
            Parse(text);

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

        private void Parse(string plain_old_input_grammar)
        {
            // Set up Antlr to parse input grammar.
            byte[] byteArray = Encoding.UTF8.GetBytes(plain_old_input_grammar);
            CommonTokenStream cts = new CommonTokenStream(
                new ANTLRv4Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())));
            _ant_parser = new ANTLRv4Parser(cts);

            // Set up another token stream containing comments. This might be
            // problematic as the parser influences the lexer.
            CommonTokenStream cts_off_channel = new CommonTokenStream(
                new ANTLRv4Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())),
                ANTLRv4Lexer.OFF_CHANNEL);

            // Get all comments.
            _ant_comments = new List<IToken>();
            while (cts_off_channel.LA(1) != ANTLRv4Parser.Eof)
            {
                IToken token = cts_off_channel.LT(1);
                if (token.Type == ANTLRv4Parser.BLOCK_COMMENT
                    || token.Type == ANTLRv4Parser.LINE_COMMENT)
                {
                    _ant_comments.Add(token);
                }
                cts_off_channel.Consume();
            }

            _ant_tree = _ant_parser.grammarSpec();
            _all_nodes = DFSVisitor.DFS(_ant_tree as ParserRuleContext).ToArray();

            {
                // Get all nonterminal names from the grammar.
                IEnumerable<IParseTree> nonterm_nodes_iterator = _all_nodes.Where((IParseTree n) =>
                {
                    TerminalNodeImpl nonterm = n as TerminalNodeImpl;
                    return nonterm?.Symbol.Type == ANTLRv4Parser.RULE_REF;
                });
                _ant_nonterminals_names = nonterm_nodes_iterator.Select<IParseTree, string>(
                    (t) => (t as TerminalNodeImpl).Symbol.Text).ToArray();
            }

            {
                // Get all terminal names from the grammar.
                IEnumerable<IParseTree> term_nodes_iterator = _all_nodes.Where((IParseTree n) =>
                {
                    TerminalNodeImpl nonterm = n as TerminalNodeImpl;
                    return nonterm?.Symbol.Type == ANTLRv4Parser.TOKEN_REF;
                });
                _ant_terminals_names = term_nodes_iterator.Select<IParseTree, string>(
                    (t) => (t as TerminalNodeImpl).Symbol.Text).ToArray();
            }

            {
                // Get all defining and applied occurences of nonterminal names in grammar.
                IEnumerable<IParseTree> nonterm_nodes_iterator = _all_nodes.Where((IParseTree n) =>
                {
                    TerminalNodeImpl nonterm = n as TerminalNodeImpl;
                    if (nonterm == null) return false;
                    if (!_ant_nonterminals_names.Contains(nonterm.GetText())) return false;
                    // The token must be part of parserRuleSpec context.
                    for (var p = nonterm.Parent; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.ParserRuleSpecContext) return true;
                    }
                    return false;
                });
                _ant_nonterminals = nonterm_nodes_iterator.Select<IParseTree, IToken>(
                    (t) => (t as TerminalNodeImpl).Symbol).ToArray();
            }

            {
                // Get all defining and applied occurences of nonterminal names in grammar.
                IEnumerable<IParseTree> term_nodes_iterator = _all_nodes.Where((IParseTree n) =>
                {
                    TerminalNodeImpl term = n as TerminalNodeImpl;
                    if (term == null) return false;
                    if (!_ant_terminals_names.Contains(term.GetText())) return false;
                    // The token must be part of parserRuleSpec context.
                    for (var p = term.Parent; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.ParserRuleSpecContext ||
                            p is ANTLRv4Parser.LexerRuleSpecContext) return true;
                    }
                    return false;
                });
                _ant_terminals = term_nodes_iterator.Select<IParseTree, IToken>(
                    (t) => (t as TerminalNodeImpl).Symbol).ToArray();
            }

            {
                // Get all keyword tokens in grammar.
                IEnumerable<IParseTree> keywords_interator = _all_nodes.Where((IParseTree n) =>
                {
                    TerminalNodeImpl nonterm = n as TerminalNodeImpl;
                    if (nonterm == null) return false;
                    for (var p = nonterm.Parent; p != null; p = p.Parent)
                    {
                        // "parser grammar" "lexer grammar" etc.
                        if (p is ANTLRv4Parser.GrammarTypeContext) return true;
                        if (p is ANTLRv4Parser.OptionsSpecContext) return true;
                        // "options ..."
                        if (p is ANTLRv4Parser.OptionContext) return false;
                        // "import ..."
                        if (p is ANTLRv4Parser.DelegateGrammarsContext) return true;
                        if (p is ANTLRv4Parser.DelegateGrammarContext) return false;
                        // "tokens ..."
                        if (p is ANTLRv4Parser.TokensSpecContext) return true;
                        if (p is ANTLRv4Parser.IdListContext) return false;
                        // "channels ..."
                        if (p is ANTLRv4Parser.ChannelsSpecContext) return true;
                        if (p is ANTLRv4Parser.ModeSpecContext) return true;
                    }
                    return false;
                });
                _ant_keywords = keywords_interator.Select<IParseTree, IToken>(
                    (t) => (t as TerminalNodeImpl).Symbol).ToArray();
            }

            //pp.ErrorHandler = new MyErrorStrategy();
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

                var text = curSpan.GetText();

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

                all_nonterm_tokens = _ant_nonterminals.Where((token) =>
                {
                    int start_token_start = token.StartIndex;
                    int end_token_end = token.StopIndex;
                    if (start_token_start >= curLocEnd) return false;
                    if (end_token_end < curLocStart) return false;
                    return true;
                }).ToList();
                combined_tokens = combined_tokens.Concat(all_nonterm_tokens);
                all_term_tokens = _ant_terminals.Where((token) =>
                {
                    int start_token_start = token.StartIndex;
                    int end_token_end = token.StopIndex;
                    if (start_token_start >= curLocEnd) return false;
                    if (end_token_end < curLocStart) return false;
                    return true;
                }).ToList();
                combined_tokens = combined_tokens.Concat(all_term_tokens);
                all_comment_tokens = _ant_comments.Where((token) =>
                {
                    int start_token_start = token.StartIndex;
                    int end_token_end = token.StopIndex;
                    if (start_token_start >= curLocEnd) return false;
                    if (end_token_end < curLocStart) return false;
                    return true;
                }).ToList();
                combined_tokens = combined_tokens.Concat(all_comment_tokens);
                all_keyword_tokens = _ant_keywords.Where((token) =>
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
                        yield return new TagSpan<AntlrTokenTag>(tokenSpan, new AntlrTokenTag(type));
                }
            }
        }
    }
}
