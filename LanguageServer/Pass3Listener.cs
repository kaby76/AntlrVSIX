namespace LanguageServer
{
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Collections.Generic;
    using System.Linq;

    public class Pass3Listener : ANTLRv4ParserBaseListener
    {
        private readonly AntlrGrammarDetails _pd;

        public Pass3Listener(AntlrGrammarDetails pd)
        {
            _pd = pd;
        }

        public override void EnterTerminal([NotNull] ANTLRv4Parser.TerminalContext context)
        {
            TerminalNodeImpl first = context.GetChild(0) as TerminalNodeImpl;
            if (first.Symbol.Type == ANTLRv4Parser.TOKEN_REF)
            {
                string id = first.GetText();
                List<ISymbol> list = _pd.RootScope.LookupType(id).ToList();
                if (!list.Any())
                {
                    ISymbol sym = new TerminalSymbol(id, first.Symbol);
                    _pd.RootScope.define(ref sym);
                    list = _pd.RootScope.LookupType(id).ToList();
                }
                List<CombinedScopeSymbol> new_attrs = new List<CombinedScopeSymbol>();
                CombinedScopeSymbol s = new RefSymbol(first.Symbol, list);
                new_attrs.Add(s);
                _pd.Attributes[context] = new_attrs;
                _pd.Attributes[context.GetChild(0)] = new_attrs;
            }
        }

        public override void EnterRuleref([NotNull] ANTLRv4Parser.RulerefContext context)
        {
            TerminalNodeImpl first = context.GetChild(0) as TerminalNodeImpl;
            string id = context.GetChild(0).GetText();
            List<ISymbol> list = _pd.RootScope.LookupType(id).ToList();
            if (!list.Any())
            {
                ISymbol sym = new NonterminalSymbol(id, first.Symbol);
                _pd.RootScope.define(ref sym);
                list = _pd.RootScope.LookupType(id).ToList();
            }
            List<CombinedScopeSymbol> new_attrs = new List<CombinedScopeSymbol>();
            CombinedScopeSymbol s = new RefSymbol(first.Symbol, list);
            new_attrs.Add(s);
            _pd.Attributes[context] = new_attrs;
            _pd.Attributes[context.GetChild(0)] = new_attrs;
        }

        public override void EnterIdentifier([NotNull] ANTLRv4Parser.IdentifierContext context)
        {
            if (context.Parent is ANTLRv4Parser.LexerCommandExprContext && context.Parent.Parent is ANTLRv4Parser.LexerCommandContext)
            {
                ANTLRv4Parser.LexerCommandContext lc = context.Parent.Parent as ANTLRv4Parser.LexerCommandContext;
                if (lc.GetChild(0)?.GetChild(0)?.GetText() == "pushMode")
                {
                    TerminalNodeImpl term = context.GetChild(0) as TerminalNodeImpl;
                    string id = term.GetText();
                    List<ISymbol> sym_list = _pd.RootScope.LookupType(id).ToList();
                    if (!sym_list.Any())
                    {
                        ISymbol sym = new ModeSymbol(id, null);
                        _pd.RootScope.define(ref sym);
                        sym_list = _pd.RootScope.LookupType(id).ToList();
                    }
                    List<CombinedScopeSymbol> ref_list = new List<CombinedScopeSymbol>();
                    CombinedScopeSymbol s = new RefSymbol(term.Symbol, sym_list);
                    ref_list.Add(s);
                    _pd.Attributes[context] = ref_list;
                    _pd.Attributes[context.GetChild(0)] = ref_list;
                }
                else if (lc.GetChild(0)?.GetChild(0)?.GetText() == "channel")
                {
                    TerminalNodeImpl term = context.GetChild(0) as TerminalNodeImpl;
                    string id = term.GetText();
                    List<ISymbol> sym_list = _pd.RootScope.LookupType(id).ToList();
                    if (!sym_list.Any())
                    {
                        ISymbol sym = new ChannelSymbol(id, null);
                        _pd.RootScope.define(ref sym);
                        sym_list = _pd.RootScope.LookupType(id).ToList();
                    }
                    List<CombinedScopeSymbol> ref_list = new List<CombinedScopeSymbol>();
                    CombinedScopeSymbol s = new RefSymbol(term.Symbol, sym_list);
                    ref_list.Add(s);
                    _pd.Attributes[context] = ref_list;
                    _pd.Attributes[context.GetChild(0)] = ref_list;
                }
                else if (lc.GetChild(0)?.GetChild(0)?.GetText() == "type")
                {
                    TerminalNodeImpl term = context.GetChild(0) as TerminalNodeImpl;
                    string id = term.GetText();
                    List<ISymbol> sym_list = _pd.RootScope.LookupType(id).ToList();
                    if (!sym_list.Any())
                    {
                        ISymbol sym = new TerminalSymbol(id, null);
                        _pd.RootScope.define(ref sym);
                        sym_list = _pd.RootScope.LookupType(id).ToList();
                    }
                    List<CombinedScopeSymbol> ref_list = new List<CombinedScopeSymbol>();
                    CombinedScopeSymbol s = new RefSymbol(term.Symbol, sym_list);
                    ref_list.Add(s);
                    _pd.Attributes[context] = ref_list;
                    _pd.Attributes[context.GetChild(0)] = ref_list;
                }
            }
        }
    }
}
