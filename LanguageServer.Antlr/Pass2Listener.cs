namespace LanguageServer.Antlr
{
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Symtab;

    public class Pass2Listener : ANTLRv4ParserBaseListener
    {
        AntlrParserDetails _pd;

        public Pass2Listener(AntlrParserDetails pd)
        {
            _pd = pd;
        }

        public override void EnterTerminal([NotNull] ANTLRv4Parser.TerminalContext context)
        {
            var first = context.GetChild(0) as TerminalNodeImpl;
            if (first.Symbol.Type == ANTLRv4Parser.TOKEN_REF)
            {
                var id = first.GetText();
                ISymbol sym = _pd.RootScope.LookupType(id);
                if (sym == null)
                {
                    sym = (ISymbol)new TerminalSymbol(id, first.Symbol);
                    _pd.RootScope.define(ref sym);
                }
                var s = (CombinedScopeSymbol)new RefSymbol(first.Symbol, sym);
                _pd.Attributes[context] = s;
                _pd.Attributes[context.GetChild(0)] = s;
            }
        }

        public override void EnterRuleref([NotNull] ANTLRv4Parser.RulerefContext context)
        {
            var sy = context.GetChild(0) as TerminalNodeImpl;
            var id = context.GetChild(0).GetText();
            ISymbol sym = _pd.RootScope.LookupType(id);
            if (sym == null)
            {
                sym = (ISymbol)new NonterminalSymbol(id, null);
                _pd.RootScope.define(ref sym);
            }
            var s = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, sym);
            _pd.Attributes[context] = s;
            _pd.Attributes[context.GetChild(0)] = s;
        }

        public override void EnterId([NotNull] ANTLRv4Parser.IdContext context)
        {
            if (context.Parent is ANTLRv4Parser.LexerCommandExprContext && context.Parent.Parent is ANTLRv4Parser.LexerCommandContext)
            {
                var lc = context.Parent.Parent as ANTLRv4Parser.LexerCommandContext;
                if (lc.GetChild(0)?.GetChild(0)?.GetText() == "pushMode")
                {
                    var term = context.GetChild(0) as TerminalNodeImpl;
                    var id = term.GetText();
                    ISymbol sym = _pd.RootScope.LookupType(id);
                    if (sym == null)
                    {
                        sym = (ISymbol)new ModeSymbol(id, null);
                        _pd.RootScope.define(ref sym);
                    }
                    var s = (CombinedScopeSymbol)new RefSymbol(term.Symbol, sym);
                    _pd.Attributes[context] = s;
                    _pd.Attributes[context.GetChild(0)] = s;
                }
                else if (lc.GetChild(0)?.GetChild(0)?.GetText() == "channel")
                {
                    var term = context.GetChild(0) as TerminalNodeImpl;
                    var id = term.GetText();
                    ISymbol sym = _pd.RootScope.LookupType(id);
                    if (sym == null)
                    {
                        sym = (ISymbol)new ChannelSymbol(id, null);
                        _pd.RootScope.define(ref sym);
                    }
                    var s = (CombinedScopeSymbol)new RefSymbol(term.Symbol, sym);
                    _pd.Attributes[context] = s;
                    _pd.Attributes[context.GetChild(0)] = s;
                }
            }
        }
    }
}
