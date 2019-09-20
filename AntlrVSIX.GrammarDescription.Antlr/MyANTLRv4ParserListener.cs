using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Text;
using Symtab;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Misc;

namespace AntlrVSIX.GrammarDescription.Antlr
{
    class Pass1Listener : ANTLRv4ParserBaseListener
    {
        public Dictionary<IParseTree, CombinedScopeSymbol> _attributes = new Dictionary<IParseTree, CombinedScopeSymbol>();
        public Scope _root;

        public Pass1Listener() { }

        public IParseTree NearestScope(IParseTree node)
        {
            for (; node != null; node = node.Parent)
            {
                if (_attributes.TryGetValue(node, out CombinedScopeSymbol value) && value is Scope)
                    return node;
            }
            return null;
        }

        public Scope GetScope(IParseTree node)
        {
            _attributes.TryGetValue(node, out CombinedScopeSymbol value);
            return value as Scope;
        }

        public override void EnterGrammarSpec([NotNull] ANTLRv4Parser.GrammarSpecContext context)
        {
            var scope = (CombinedScopeSymbol)new SymbolTable().GLOBALS;
            _attributes[context] = scope;
            _root = (Scope)scope;
            base.EnterGrammarSpec(context);
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
            var id = context.GetChild(i).GetText();
            Symbol sym = new Symtab.ClassSymbol(id);
            _root.define(ref sym);
            var s = (CombinedScopeSymbol)sym;
            _attributes[context] = s;
            _attributes[context.GetChild(i)] = s;
            base.EnterParserRuleSpec(context);
        }
    }

    class Pass2Listener : ANTLRv4ParserBaseListener
    {
        public Dictionary<IParseTree, CombinedScopeSymbol> _attributes;
        private Scope _root;

        public Pass2Listener(Dictionary<IParseTree, CombinedScopeSymbol> attributes, Scope root)
        {
            _attributes = attributes;
            _root = root;
        }

        public override void EnterTerminal([NotNull] ANTLRv4Parser.TerminalContext context)
        {
            var first = context.GetChild(0) as TerminalNodeImpl;
            if (first.Symbol.Type == ANTLRv4Parser.TOKEN_REF)
            {
                var id = first.GetText();
                Symbol sym = _root.LookupType(id);
                if (sym == null)
                {
                    sym = (Symbol)new Literal(id, id, 0);
                    _root.define(ref sym);
                }
                var s = (CombinedScopeSymbol)new RefSymbol(sym);
                _attributes[context] = s;
                _attributes[context.GetChild(0)] = s;
            }
        }

        public override void EnterRuleref([NotNull] ANTLRv4Parser.RulerefContext context)
        {
            var id = context.GetChild(0).GetText();
            Symbol sym = _root.LookupType(id);
            if (sym == null)
            {
                sym = (Symbol)new ClassSymbol(id);
                _root.define(ref sym);
            }
            var s = (CombinedScopeSymbol)new RefSymbol(sym);
            _attributes[context] = s;
            _attributes[context.GetChild(0)] = s;
        }


    }
}
