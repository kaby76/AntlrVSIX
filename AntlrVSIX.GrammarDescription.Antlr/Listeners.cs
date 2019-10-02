using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Symtab;
using System.Collections.Generic;

namespace AntlrVSIX.GrammarDescription.Antlr
{
    public class Pass1Listener : ANTLRv4ParserBaseListener
    {
        private AntlrParserDetails _pd;

        public Pass1Listener(AntlrParserDetails pd)
        {
            _pd = pd;
        }

        public IParseTree NearestScope(IParseTree node)
        {
            for (; node != null; node = node.Parent)
            {
                if (_pd.Attributes.TryGetValue(node, out CombinedScopeSymbol value) && value is Scope)
                    return node;
            }
            return null;
        }

        public Scope GetScope(IParseTree node)
        {
            _pd.Attributes.TryGetValue(node, out CombinedScopeSymbol value);
            return value as Scope;
        }

        public override void EnterGrammarSpec([NotNull] ANTLRv4Parser.GrammarSpecContext context)
        {
            _pd.Attributes[context] = (CombinedScopeSymbol)_pd.RootScope;
        }

        public override void EnterParserRuleSpec([NotNull] ANTLRv4Parser.ParserRuleSpecContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
            {
                if (!(context.GetChild(i) is TerminalNodeImpl)) continue;
                var c = context.GetChild(i) as TerminalNodeImpl;
                if (c.Symbol.Type == ANTLRv4Lexer.RULE_REF) break;
            }
            if (i == context.ChildCount) return;
            var rule_ref = context.GetChild(i) as TerminalNodeImpl;
            var id = rule_ref.GetText();
            Symbol sym = new NonterminalSymbol(id, rule_ref.Symbol.Line, rule_ref.Symbol.Column, rule_ref.Symbol.InputStream.SourceName);
            _pd.RootScope.define(ref sym);
            var s = (CombinedScopeSymbol)sym;
            _pd.Attributes[context] = s;
            _pd.Attributes[context.GetChild(i)] = s;
        }

        public override void EnterLexerRuleSpec([NotNull] ANTLRv4Parser.LexerRuleSpecContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
            {
                if (!(context.GetChild(i) is TerminalNodeImpl)) continue;
                var c = context.GetChild(i) as TerminalNodeImpl;
                if (c.Symbol.Type == ANTLRv4Lexer.TOKEN_REF) break;
            }
            if (i == context.ChildCount) return;
            var token_ref = context.GetChild(i) as TerminalNodeImpl;
            var id = token_ref.GetText();
            Symbol sym = new TerminalSymbol(id, token_ref.Symbol.Line, token_ref.Symbol.Column, token_ref.Symbol.InputStream.SourceName);
            _pd.RootScope.define(ref sym);
            var s = (CombinedScopeSymbol)sym;
            _pd.Attributes[context] = s;
            _pd.Attributes[context.GetChild(i)] = s;
        }

        public override void EnterId([NotNull] ANTLRv4Parser.IdContext context)
        {
            if (context.Parent is ANTLRv4Parser.ModeSpecContext)
            {
                var term = context.GetChild(0) as TerminalNodeImpl;
                var id = term.GetText();
                Symbol sym = new ModeSymbol(id, term.Symbol.Line, term.Symbol.Column, term.Symbol.InputStream.SourceName);
                _pd.RootScope.define(ref sym);
                var s = (CombinedScopeSymbol)sym;
                _pd.Attributes[context] = s;
                _pd.Attributes[context.GetChild(0)] = s;
            } else if (context.Parent is ANTLRv4Parser.IdListContext && context.Parent?.Parent is ANTLRv4Parser.ChannelsSpecContext)
            {
                var term = context.GetChild(0) as TerminalNodeImpl;
                var id = term.GetText();
                Symbol sym = new ChannelSymbol(id, term.Symbol.Line, term.Symbol.Column, term.Symbol.InputStream.SourceName);
                _pd.RootScope.define(ref sym);
                var s = (CombinedScopeSymbol)sym;
                _pd.Attributes[context] = s;
                _pd.Attributes[term] = s;
            }
        }
    }

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
                Symbol sym = _pd.RootScope.LookupType(id);
                if (sym == null)
                {
                    sym = (Symbol)new TerminalSymbol(id, first.Symbol.Line, first.Symbol.Column, first.Symbol.InputStream.SourceName);
                    _pd.RootScope.define(ref sym);
                }
                var s = (CombinedScopeSymbol)new RefSymbol(sym);
                _pd.Attributes[context] = s;
                _pd.Attributes[context.GetChild(0)] = s;
            }
        }

        public override void EnterRuleref([NotNull] ANTLRv4Parser.RulerefContext context)
        {
            var id = context.GetChild(0).GetText();
            Symbol sym = _pd.RootScope.LookupType(id);
            if (sym == null)
            {
                sym = (Symbol)new NonterminalSymbol(id, 0, 0, null);
                _pd.RootScope.define(ref sym);
            }
            var s = (CombinedScopeSymbol)new RefSymbol(sym);
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
                    Symbol sym = _pd.RootScope.LookupType(id);
                    if (sym == null)
                    {
                        sym = (Symbol)new ModeSymbol(id, 0, 0, null);
                        _pd.RootScope.define(ref sym);
                    }
                    var s = (CombinedScopeSymbol)new RefSymbol(sym);
                    _pd.Attributes[context] = s;
                    _pd.Attributes[context.GetChild(0)] = s;
                }
                else if (lc.GetChild(0)?.GetChild(0)?.GetText() == "channel")
                {
                    var term = context.GetChild(0) as TerminalNodeImpl;
                    var id = term.GetText();
                    Symbol sym = _pd.RootScope.LookupType(id);
                    if (sym == null)
                    {
                        sym = (Symbol)new ChannelSymbol(id, 0, 0, null);
                        _pd.RootScope.define(ref sym);
                    }
                    var s = (CombinedScopeSymbol)new RefSymbol(sym);
                    _pd.Attributes[context] = s;
                    _pd.Attributes[context.GetChild(0)] = s;
                }
            }
        }
    }
}
