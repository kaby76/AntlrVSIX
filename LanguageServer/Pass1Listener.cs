namespace LanguageServer
{
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Collections.Generic;

    public class Pass1Listener : ANTLRv4ParserBaseListener
    {
        private readonly AntlrGrammarDetails _pd;

        public Pass1Listener(AntlrGrammarDetails pd)
        {
            _pd = pd;
        }

        public IParseTree NearestScope(IParseTree node)
        {
            for (; node != null; node = node.Parent)
            {
                _pd.Attributes.TryGetValue(node, out IList<CombinedScopeSymbol> list);
                if (list != null)
                {
                    if (list.Count == 1 && list[0] is IScope)
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        public IScope GetScope(IParseTree node)
        {
            if (node == null)
            {
                return null;
            }

            _pd.Attributes.TryGetValue(node, out IList<CombinedScopeSymbol> list);
            if (list != null)
            {
                if (list.Count == 1 && list[0] is IScope)
                {
                    return list[0] as IScope;
                }
            }
            return null;
        }

        public override void EnterGrammarSpec([NotNull] ANTLRv4Parser.GrammarSpecContext context)
        {
            _pd.Attributes[context] = new List<CombinedScopeSymbol>() { (CombinedScopeSymbol)_pd.RootScope };
        }

        public override void EnterParserRuleSpec([NotNull] ANTLRv4Parser.ParserRuleSpecContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
            {
                if (!(context.GetChild(i) is TerminalNodeImpl))
                {
                    continue;
                }

                TerminalNodeImpl c = context.GetChild(i) as TerminalNodeImpl;
                if (c.Symbol.Type == ANTLRv4Lexer.RULE_REF)
                {
                    break;
                }
            }
            if (i == context.ChildCount)
            {
                return;
            }

            TerminalNodeImpl rule_ref = context.GetChild(i) as TerminalNodeImpl;
            string id = rule_ref.GetText();
            ISymbol sym = new NonterminalSymbol(id, rule_ref.Symbol);
            _pd.RootScope.define(ref sym);
            CombinedScopeSymbol s = (CombinedScopeSymbol)sym;
            _pd.Attributes[context] = new List<CombinedScopeSymbol>() { s };
            _pd.Attributes[context.GetChild(i)] = new List<CombinedScopeSymbol>() { s };
        }

        public override void EnterLexerRuleSpec([NotNull] ANTLRv4Parser.LexerRuleSpecContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
            {
                if (!(context.GetChild(i) is TerminalNodeImpl))
                {
                    continue;
                }

                TerminalNodeImpl c = context.GetChild(i) as TerminalNodeImpl;
                if (c.Symbol.Type == ANTLRv4Lexer.TOKEN_REF)
                {
                    break;
                }
            }
            if (i == context.ChildCount)
            {
                return;
            }

            TerminalNodeImpl token_ref = context.GetChild(i) as TerminalNodeImpl;
            string id = token_ref.GetText();
            ISymbol sym = new TerminalSymbol(id, token_ref.Symbol);
            _pd.RootScope.define(ref sym);
            CombinedScopeSymbol s = (CombinedScopeSymbol)sym;
            _pd.Attributes[context] = new List<CombinedScopeSymbol>() { s };
            _pd.Attributes[context.GetChild(i)] = new List<CombinedScopeSymbol>() { s };
        }

        public override void EnterId([NotNull] ANTLRv4Parser.IdContext context)
        {
            if (context.Parent is ANTLRv4Parser.ModeSpecContext)
            {
                TerminalNodeImpl term = context.GetChild(0) as TerminalNodeImpl;
                string id = term.GetText();
                ISymbol sym = new ModeSymbol(id, term.Symbol);
                _pd.RootScope.define(ref sym);
                CombinedScopeSymbol s = (CombinedScopeSymbol)sym;
                _pd.Attributes[context] = new List<CombinedScopeSymbol>() { s };
                _pd.Attributes[context.GetChild(0)] = new List<CombinedScopeSymbol>() { s };
            }
            else if (context.Parent is ANTLRv4Parser.IdListContext && context.Parent?.Parent is ANTLRv4Parser.ChannelsSpecContext)
            {
                TerminalNodeImpl term = context.GetChild(0) as TerminalNodeImpl;
                string id = term.GetText();
                ISymbol sym = new ChannelSymbol(id, term.Symbol);
                _pd.RootScope.define(ref sym);
                CombinedScopeSymbol s = (CombinedScopeSymbol)sym;
                _pd.Attributes[context] = new List<CombinedScopeSymbol>() { s };
                _pd.Attributes[term] = new List<CombinedScopeSymbol>() { s };
            }
        }
    }
}
