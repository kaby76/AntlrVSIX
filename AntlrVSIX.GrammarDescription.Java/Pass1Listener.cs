
using System;
using System.Collections.Generic;
using System.Text;
using Symtab;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Misc;

namespace AntlrVSIX.GrammarDescription.Java
{
    class Pass1Listener : Java9ParserBaseListener
    {
        public Dictionary<IParseTree, CombinedScopeSymbol> _attributes = new Dictionary<IParseTree, CombinedScopeSymbol>();

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

        public Pass1Listener()
        {
        }

        public override void EnterCompilationUnit([NotNull] Java9Parser.CompilationUnitContext context)
        {
            _attributes[context] = (CombinedScopeSymbol)new SymbolTable().GLOBALS;
        }

        public override void EnterInterfaceMethodDeclaration(Java9Parser.InterfaceMethodDeclarationContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
                if (context.GetChild(i) as Java9Parser.MethodHeaderContext != null)
                    break;
            var mh = context.GetChild(i) as Java9Parser.MethodHeaderContext;
            int j;
            for (j = 0; j < mh.ChildCount; ++j)
                if (mh.GetChild(j) as Java9Parser.MethodDeclaratorContext != null)
                    break;
            var md = mh.GetChild(j) as Java9Parser.MethodDeclaratorContext;
            var node = md.GetChild(0);
            var name = node.GetText();
            Symbol m = new Symtab.MethodSymbol(name);
            var scope = GetScope(NearestScope(context));
            scope.define(ref m);
            _attributes[node] = (CombinedScopeSymbol)m;
            _attributes[context.GetChild(0)] = (CombinedScopeSymbol)m;
            _attributes[context] = (CombinedScopeSymbol)m;
        }

        public override void EnterLocalVariableDeclaration(Java9Parser.LocalVariableDeclarationContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
                if (context.GetChild(i) as Java9Parser.VariableDeclaratorListContext != null)
                    break;
            var vdl = context.GetChild(i) as Java9Parser.VariableDeclaratorListContext;
            for (int j = 0; j < vdl.ChildCount; j += 2)
            {
                var vd = vdl.GetChild(j) as Java9Parser.VariableDeclaratorContext;
                var vdid = vd.GetChild(0);
                var id = vdid.GetChild(0);
                var term = id.GetChild(0) as TerminalNodeImpl;
                var name = term.GetText();
                Symbol f = new Symtab.LocalSymbol(name, term.Symbol);
                var scope = GetScope(NearestScope(context));
                scope.define(ref f);
                _attributes[term] = (CombinedScopeSymbol)f;
                _attributes[id] = (CombinedScopeSymbol)f;
            }
        }

        public override void EnterEnumDeclaration(Java9Parser.EnumDeclarationContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
                if (context.GetChild(i).GetText() == "enum")
                    break;
            var node = context.GetChild(i+1) as Java9Parser.IdentifierContext;
            var name = node.GetText();
            Symbol e = new Symtab.EnumSymbol(name);
            var scope = GetScope(NearestScope(context));
            scope.define(ref e);
            _attributes[node] =(CombinedScopeSymbol) e;
            _attributes[context.GetChild(0)] = (CombinedScopeSymbol)e;
            _attributes[context] = (CombinedScopeSymbol)e;
        }

        public override void EnterBlock(Java9Parser.BlockContext context)
        {
            var e = GetScope(NearestScope(context));
            var b = new Symtab.LocalScope(e);
            _attributes[context] = (CombinedScopeSymbol) b;
        }

        public override void EnterMethodDeclaration(Java9Parser.MethodDeclarationContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
                if (context.GetChild(i) as Java9Parser.MethodHeaderContext != null)
                    break;
            var mh = context.GetChild(i) as Java9Parser.MethodHeaderContext;
            int j;
            for (j = 0; j < mh.ChildCount; ++j)
                if (mh.GetChild(j) as Java9Parser.MethodDeclaratorContext != null)
                    break;
            var md = mh.GetChild(j) as Java9Parser.MethodDeclaratorContext;
            var node = md.GetChild(0);
            var name = node.GetText();
            Symbol m = new Symtab.MethodSymbol(name);
            var scope = GetScope(NearestScope(context));
            scope.define(ref m);
            _attributes[node] = (CombinedScopeSymbol)m;
            _attributes[context.GetChild(0)] = (CombinedScopeSymbol)m;
            _attributes[context] = (CombinedScopeSymbol)m;
        }

        public override void EnterNormalClassDeclaration(Java9Parser.NormalClassDeclarationContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
                if (context.GetChild(i) as Java9Parser.IdentifierContext != null)
                    break;
            var node = context.GetChild(i) as Java9Parser.IdentifierContext;
            var id_name = node.GetText();
            Symbol cs = new Symtab.ClassSymbol(id_name);
            var scope = GetScope(NearestScope(context));
            scope.define(ref cs);
            _attributes[node] = (CombinedScopeSymbol)cs;
            Scope s = (Scope)cs;
            _attributes[context.GetChild(0)] = (CombinedScopeSymbol)s;
            _attributes[context] = (CombinedScopeSymbol)s;
        }

        public override void EnterNormalInterfaceDeclaration(Java9Parser.NormalInterfaceDeclarationContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
                if (context.GetChild(i) as Java9Parser.IdentifierContext != null)
                    break;
            var node = context.GetChild(i) as Java9Parser.IdentifierContext;
            var id_name = node.GetText();
            Symbol cs = new Symtab.InterfaceSymbol(id_name);
            var scope = GetScope(NearestScope(context));
            scope.define(ref cs);
            _attributes[node] = (CombinedScopeSymbol)cs;
            Scope s = (Scope)cs;
            _attributes[context.GetChild(0)] = (CombinedScopeSymbol)s;
            _attributes[context] = (CombinedScopeSymbol)s;
        }

        public override void EnterBasicForStatement([NotNull] Java9Parser.BasicForStatementContext context)
        {
            var scope = GetScope(NearestScope(context));
            var e = scope;
            var b = new Symtab.LocalScope(e);
            _attributes[context] = b;
        }

        public override void EnterBasicForStatementNoShortIf([NotNull] Java9Parser.BasicForStatementNoShortIfContext context)
        {
            var scope = GetScope(NearestScope(context));
            var e = scope;
            var b = new Symtab.LocalScope(e);
            _attributes[context] = b;
        }

        public override void EnterEnhancedForStatement([NotNull] Java9Parser.EnhancedForStatementContext context)
        {
            var scope = GetScope(NearestScope(context));
            var e = scope;
            var b = new Symtab.LocalScope(e);
            _attributes[context] = b;
        }

        public override void EnterEnhancedForStatementNoShortIf([NotNull] Java9Parser.EnhancedForStatementNoShortIfContext context)
        {
            var scope = GetScope(NearestScope(context));
            var e = scope;
            var b = new Symtab.LocalScope(e);
            _attributes[context] = b;
        }

        public override void EnterTryWithResourcesStatement([NotNull] Java9Parser.TryWithResourcesStatementContext context)
        {
            var scope = GetScope(NearestScope(context));
            var e = scope;
            var b = new Symtab.LocalScope(e);
            _attributes[context] = b;
        }

        public override void ExitResource([NotNull] Java9Parser.ResourceContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
                if (context.GetChild(i) as Java9Parser.VariableDeclaratorIdContext != null)
                    break;
            var vdi = context.GetChild(i) as Java9Parser.VariableDeclaratorIdContext;
            var id = vdi.GetChild(0);
            var term = id.GetChild(0) as TerminalNodeImpl;
            var name = term.GetText();
            Symbol f = new Symtab.LocalSymbol(name, term.Symbol);
            _attributes[vdi] = (CombinedScopeSymbol)f;
            var scope = GetScope(NearestScope(context));
            scope.define(ref f);
            _attributes[context.GetChild(0)] = (CombinedScopeSymbol)f;
            _attributes[context] = (CombinedScopeSymbol)f;
        }

        public override void ExitLiteral([NotNull] Java9Parser.LiteralContext context)
        {
            var literal = context.GetText();
            var c = context.GetChild(0);
            var t = c as TerminalNodeImpl;
            var s = t.Symbol;
            string cleaned_up_literal = "";
            switch (s.Type)
            {
                case Java9Lexer.IntegerLiteral:
                case Java9Lexer.FloatingPointLiteral:
                    cleaned_up_literal = literal.Replace("_", "");
                    break;
                default:
                    cleaned_up_literal = literal;
                    break;
            }
            var literal_symbol = new Symtab.Literal(literal, cleaned_up_literal, s.Type);
            _attributes[context.GetChild(0)] = (CombinedScopeSymbol)literal_symbol;
            _attributes[context] = literal_symbol;
        }

        public override void EnterIdentifier([NotNull] Java9Parser.IdentifierContext context)
        {
            var parent = context.Parent;
            if (parent is Java9Parser.SimpleTypeNameContext && parent.Parent is Java9Parser.ConstructorDeclaratorContext)
            {
                var scope = GetScope(NearestScope(context));
                var sc = scope;
                for (; sc != null; sc = sc.EnclosingScope)
                {
                    if (sc is ClassSymbol) break;
                }
                if (sc == null)
                {
                    return;
                }
                var id = context.GetText();
                Symbol f = new MethodSymbol(id);
                sc.define(ref f);
                _attributes[context.GetChild(0)] = (CombinedScopeSymbol)f; 
                _attributes[context] = (CombinedScopeSymbol)f;
                _attributes[context.Parent] = (CombinedScopeSymbol)f;
                _attributes[context.Parent.Parent] = (CombinedScopeSymbol)f;
                _attributes[context.Parent.Parent.Parent] = (CombinedScopeSymbol)f;
            } else if (parent is Java9Parser.MethodDeclaratorContext)
            {
                var scope = GetScope(NearestScope(context));
                var sc = scope;
                if (sc == null)
                {
                    return;
                }
                var id = context.GetText();
                Symbol f = new MethodSymbol(id);
                sc.define(ref f);
                _attributes[context.GetChild(0)] = (CombinedScopeSymbol)f;
                _attributes[context] = (CombinedScopeSymbol)f;
                _attributes[parent] = (CombinedScopeSymbol)f;
            }
            else if (parent is Java9Parser.VariableDeclaratorIdContext && parent.Parent is Java9Parser.FormalParameterContext)
            {
                var term = context.GetChild(0) as TerminalNodeImpl;
                Symbol f = new Symtab.ParameterSymbol(term.GetText(), term.Symbol);
                var p = (ParserRuleContext)context;
                for (; p != null; p = (ParserRuleContext)p.Parent)
                {
                    if (p is Java9Parser.MethodDeclaratorContext ||
                        p is Java9Parser.ConstructorDeclaratorContext)
                        break;
                }
                if (p == null) return;
                {
                    var sc = _attributes[p];
                    ((Scope)sc).define(ref f);
                    _attributes[context.GetChild(0)] = (CombinedScopeSymbol)f;
                    _attributes[context] = (CombinedScopeSymbol)f;
                }
            }
            else if (parent is Java9Parser.VariableDeclaratorIdContext)
            {
                var term = context.GetChild(0) as TerminalNodeImpl;
                Symbol f = new Symtab.FieldSymbol(term.GetText(), term.Symbol);
                var p = (ParserRuleContext)context;
                for (; p != null; p = (ParserRuleContext)p.Parent)
                {
                    if (p is Java9Parser.FieldDeclarationContext)
                        break;
                }
                if (p == null) return;
                {
                    var scope = GetScope(NearestScope(p));
                    var sc = scope;
                    for (; sc != null; sc = sc.EnclosingScope)
                    {
                        if (sc is ClassSymbol) break;
                    }
                    if (sc == null)
                    {
                        return;
                    }
                    ((Scope)sc).define(ref f);
                    _attributes[context.GetChild(0)] = (CombinedScopeSymbol)f;
                    _attributes[context] = (CombinedScopeSymbol)f;
                }
            }
        }
    }
}
