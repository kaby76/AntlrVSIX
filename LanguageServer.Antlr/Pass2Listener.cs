namespace LanguageServer.Antlr
{
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Linq;
    using System.Collections.Generic;

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
                var list = _pd.RootScope.LookupType(id);
                if (! list.Any())
                {
                    var sym = (ISymbol)new TerminalSymbol(id, first.Symbol);
                    _pd.RootScope.define(ref sym);
                }
                var new_attrs = new List<CombinedScopeSymbol>();
                foreach (var sym in list)
                {
                    var s = (CombinedScopeSymbol)new RefSymbol(first.Symbol, sym);
                    new_attrs.Add(s);
                }
                _pd.Attributes[context] = new_attrs;
                _pd.Attributes[context.GetChild(0)] = new_attrs;
            }
        }

        public override void EnterRuleref([NotNull] ANTLRv4Parser.RulerefContext context)
        {
            var first = context.GetChild(0) as TerminalNodeImpl;
            var id = context.GetChild(0).GetText();
            var list = _pd.RootScope.LookupType(id);
            if (!list.Any())
            {
                var sym = (ISymbol)new NonterminalSymbol(id, first.Symbol);
                _pd.RootScope.define(ref sym);
            }
            var new_attrs = new List<CombinedScopeSymbol>();
            foreach (var sym in list)
            {
                var s = (CombinedScopeSymbol)new RefSymbol(first.Symbol, sym);
                new_attrs.Add(s);
            }
            _pd.Attributes[context] = new_attrs;
            _pd.Attributes[context.GetChild(0)] = new_attrs;
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
                    IList<ISymbol> sym_list = _pd.RootScope.LookupType(id);
                    if (! sym_list.Any())
                    {
                        var sym = (ISymbol)new ModeSymbol(id, null);
                        _pd.RootScope.define(ref sym);
                    }
                    var ref_list = new List<CombinedScopeSymbol>();
                    foreach (var sym in sym_list)
                    {
                        var s = (CombinedScopeSymbol)new RefSymbol(term.Symbol, sym);
                        ref_list.Add(s);
                    }
                    _pd.Attributes[context] = ref_list;
                    _pd.Attributes[context.GetChild(0)] = ref_list;
                }
                else if (lc.GetChild(0)?.GetChild(0)?.GetText() == "channel")
                {
                    var term = context.GetChild(0) as TerminalNodeImpl;
                    var id = term.GetText();
                    IList<ISymbol> sym_list = _pd.RootScope.LookupType(id);
                    if (!sym_list.Any())
                    {
                        var sym = (ISymbol)new ChannelSymbol(id, null);
                        _pd.RootScope.define(ref sym);
                    }
                    var ref_list = new List<CombinedScopeSymbol>();
                    foreach (var sym in sym_list)
                    {
                        var s = (CombinedScopeSymbol)new RefSymbol(term.Symbol, sym);
                        ref_list.Add(s);
                    }
                    _pd.Attributes[context] = ref_list;
                    _pd.Attributes[context.GetChild(0)] = ref_list;
                }
            }
        }
    }
}
