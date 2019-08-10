
namespace AntlrVSIX.Grammar
{
    using Antlr4.Runtime.Tree;
    using Antlr4.Runtime;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System;

    public class ParserDetails : IObservable<ParserDetails>
    {
        public static Dictionary<string, ParserDetails> _per_file_parser_details =
            new Dictionary<string, ParserDetails>();

        public string FullFileName;
        public string Code;
        private List<IObserver<ParserDetails>> _observers = new List<IObserver<ParserDetails>>();

        // Parser and parse tree.
        private ANTLRv4Parser _ant_parser = null;
        private IParseTree _ant_tree = null;
        private IEnumerable<IParseTree>_all_nodes = null;

        // List of all comments, terminal, nonterminal, and keyword tokens in the grammar.
        public IEnumerable<IToken> _ant_terminals;
        public IEnumerable<IToken> _ant_terminals_defining;
        public IEnumerable<IToken> _ant_nonterminals;
        public IEnumerable<IToken> _ant_nonterminals_defining;
        public IEnumerable<IToken> _ant_comments;
        public IEnumerable<IToken> _ant_keywords;
        public IEnumerable<IToken> _ant_literals;

        public static ParserDetails Parse(string code, string ffn)
        {
            bool has_entry = _per_file_parser_details.ContainsKey(ffn);
            ParserDetails pd;
            if (!has_entry)
            {
                pd = new ParserDetails();
                _per_file_parser_details[ffn] = pd;
            }
            else
            {
                pd = _per_file_parser_details[ffn];
            }

            bool has_changed = false;
            if (pd.Code == code) return pd;
            else if (pd.Code != null) has_changed = true;

            pd.Code = code;
            pd.FullFileName = ffn;

            // Set up Antlr to parse input grammar.
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            CommonTokenStream cts = new CommonTokenStream(
                new ANTLRv4Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())));
            pd._ant_parser = new ANTLRv4Parser(cts);

            // Set up another token stream containing comments. This might be
            // problematic as the parser influences the lexer.
            CommonTokenStream cts_off_channel = new CommonTokenStream(
                new ANTLRv4Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())),
                ANTLRv4Lexer.OFF_CHANNEL);

            // Get all comments.
            var new_list = new List<IToken>();
            while (cts_off_channel.LA(1) != ANTLRv4Parser.Eof)
            {
                IToken token = cts_off_channel.LT(1);
                if (token.Type == ANTLRv4Parser.BLOCK_COMMENT
                    || token.Type == ANTLRv4Parser.LINE_COMMENT)
                {
                    new_list.Add(token);
                }
                cts_off_channel.Consume();
            }
            pd._ant_comments = new_list;

            try
            {
                pd._ant_tree = pd._ant_parser.grammarSpec();
            }
            catch (Exception e)
            {
                // Parsing error.
            }

            pd._all_nodes = DFSVisitor.DFS(pd._ant_tree as ParserRuleContext);

            //StringBuilder sb = new StringBuilder();
            //Class1.ParenthesizedAST(pd._ant_tree, sb, "", cts);
            //string fn = System.IO.Path.GetFileName(pd.FullFileName);
            //fn = "c:\\temp\\" + fn;
            //System.IO.File.WriteAllText(fn, sb.ToString());

            {
                // Get all defining and applied occurences of nonterminal names in grammar.
                IEnumerable<IParseTree> nonterm_nodes_iterator = pd._all_nodes.Where((IParseTree n) =>
                {
                    TerminalNodeImpl nonterm = n as TerminalNodeImpl;
                    if (n.Parent as ANTLRv4Parser.RulerefContext != null &&
                        nonterm?.Symbol.Type == ANTLRv4Parser.RULE_REF)
                        return true;
                    if (n.Parent as ANTLRv4Parser.ActionBlockContext != null)
                        return false;
                    if (n.Parent as ANTLRv4Parser.ParserRuleSpecContext != null &&
                        nonterm?.Symbol.Type == ANTLRv4Parser.RULE_REF)
                        return true;
                    return false;
                });
                pd._ant_nonterminals = nonterm_nodes_iterator.Select<IParseTree, IToken>(
                    (t) => (t as TerminalNodeImpl).Symbol).ToArray();
                // Get all defining and applied occurences of nonterminal names in grammar.
                var iterator = nonterm_nodes_iterator.Where((IParseTree n) =>
                {
                    TerminalNodeImpl term = n as TerminalNodeImpl;
                    if (term == null) return false;
                    IRuleNode parent = term.Parent;
                    for (int i = 0; i < parent.ChildCount; ++i)
                    {
                        if (parent.GetChild(i) == term &&
                            i + 1 < parent.ChildCount &&
                            parent.GetChild(i + 1).GetText() == ":")
                            return true;
                    }
                    return false;
                });
                pd._ant_nonterminals_defining = iterator.Select<IParseTree, IToken>(
                    (t) => (t as TerminalNodeImpl).Symbol).ToArray();
            }

            {
                // Get all defining and applied occurences of nonterminal names in grammar.
                IEnumerable<IParseTree> term_nodes_iterator = pd._all_nodes.Where((IParseTree n) =>
                {
                    TerminalNodeImpl term = n as TerminalNodeImpl;
                    if (term == null) return false;
                    if (!Char.IsUpper(term.GetText()[0])) return false;
                    if (term.Parent as ANTLRv4Parser.TerminalContext != null)
                        return true;
                    if (term.Parent as ANTLRv4Parser.LexerRuleSpecContext != null)
                        return true;
                    if (term.Parent?.Parent as ANTLRv4Parser.LexerCommandExprContext != null)
                        return true;
                    return false;
                });
                pd._ant_terminals = term_nodes_iterator.Select<IParseTree, IToken>(
                    (t) => (t as TerminalNodeImpl).Symbol).ToArray();
                // Get all defining nonterminal names in grammar.
                var iterator = term_nodes_iterator.Where((IParseTree n) =>
                {
                    TerminalNodeImpl term = n as TerminalNodeImpl;
                    if (term == null) return false;
                    IRuleNode parent = term.Parent;
                    for (int i = 0; i < parent.ChildCount; ++i)
                    {
                        if (parent.GetChild(i) == term &&
                            i + 1 < parent.ChildCount &&
                            parent.GetChild(i + 1).GetText() == ":")
                            return true;
                    }
                    return false;
                });
                pd._ant_terminals_defining = iterator.Select<IParseTree, IToken>(
                    (t) => (t as TerminalNodeImpl).Symbol).ToArray();
            }

            {
                // Get all keyword tokens in grammar.
                IEnumerable<IParseTree> keywords_interator = pd._all_nodes.Where((IParseTree n) =>
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
                pd._ant_keywords = keywords_interator.Select<IParseTree, IToken>(
                    (t) => (t as TerminalNodeImpl).Symbol).ToArray();
            }

            {
                // Get all defining and applied occurences of nonterminal names in grammar.
                IEnumerable<IParseTree> lit_nodes_iterator = pd._all_nodes.Where((IParseTree n) =>
                {
                    TerminalNodeImpl term = n as TerminalNodeImpl;
                    if (term == null) return false;
                    // Chicken/egg problem. Assume that literals are marked
                    // with the appropriate token type.
                    if (term.Symbol == null) return false;
                    if (!(term.Symbol.Type == ANTLRv4Parser.STRING_LITERAL
                        || term.Symbol.Type == ANTLRv4Parser.INT
                        || term.Symbol.Type == ANTLRv4Parser.LEXER_CHAR_SET))
                        return false;

                    // The token must be part of parserRuleSpec context.
                    for (var p = term.Parent; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.ParserRuleSpecContext ||
                            p is ANTLRv4Parser.LexerRuleSpecContext) return true;
                    }
                    return false;
                });
                pd._ant_literals = lit_nodes_iterator.Select<IParseTree, IToken>(
                    (t) => (t as TerminalNodeImpl).Symbol).ToArray();
            }

            if (has_changed)
            {
                foreach (var observer in pd._observers)
                {
                    observer.OnNext(pd);
                }
            }

            return pd;
        }

        public IDisposable Subscribe(IObserver<ParserDetails> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<ParserDetails>> _observers;
            private IObserver<ParserDetails> _observer;

            public Unsubscriber(List<IObserver<ParserDetails>> observers, IObserver<ParserDetails> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
