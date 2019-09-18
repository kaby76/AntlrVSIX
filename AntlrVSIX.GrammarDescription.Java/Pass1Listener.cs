
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
        public Stack<Scope> _current_scope = new Stack<Scope>();
        public Dictionary<IParseTree, Symbol> _symbols = new Dictionary<IParseTree, Symbol>();
        public Dictionary<IParseTree, Scope> _scopes = new Dictionary<IParseTree, Scope>();

        public Pass1Listener()
        {
            _current_scope.Push(new SymbolTable().GLOBALS);
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
            _current_scope.Peek().define(ref m);
            _symbols[node] = m;
            Scope mm = (Scope)m;
            _scopes[context] = mm;
            _current_scope.Push(mm);
            base.EnterInterfaceMethodDeclaration(context);
        }

        public override void ExitInterfaceMethodDeclaration(Java9Parser.InterfaceMethodDeclarationContext context)
        {
            _current_scope.Pop();
            base.ExitInterfaceMethodDeclaration(context);
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
                var node = vd.GetChild(0);
                var name = node.GetText();
                Symbol f = new Symtab.LocalSymbol(name);
                _symbols[node] = f;
                _current_scope.Peek().define(ref f);
            }
            base.EnterLocalVariableDeclaration(context);
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
            _current_scope.Peek().define(ref e);
            _symbols[node] = e;
            Scope mm = (Scope)e;
            _scopes[context] = mm;
            _current_scope.Push(mm);
            base.EnterEnumDeclaration(context);
        }

        public override void ExitEnumDeclaration([NotNull] Java9Parser.EnumDeclarationContext context)
        {
            _current_scope.Pop();
            base.ExitEnumDeclaration(context);
        }

        public override void EnterBlock(Java9Parser.BlockContext context)
        {
            var e = _current_scope.Peek();
            var b = new Symtab.LocalScope(e);
            _current_scope.Push(b);
            _scopes[context] = b;
            base.EnterBlock(context);
        }

        public override void ExitBlock(Java9Parser.BlockContext context)
        {
            _current_scope.Pop();
            base.ExitBlock(context);
        }

        public override void EnterFormalParameter(Java9Parser.FormalParameterContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
                if (context.GetChild(i) as Java9Parser.VariableDeclaratorIdContext != null)
                    break;
            var vdi = context.GetChild(i) as Java9Parser.VariableDeclaratorIdContext;
            var node = vdi.GetChild(0);
            var name = node.GetText();
            Symbol v = new Symtab.MethodSymbol(name);
            _current_scope.Peek().define(ref v);
            _symbols[node] = v;
            base.EnterFormalParameter(context);
        }

        public override void ExitMethodDeclaration(Java9Parser.MethodDeclarationContext context)
        {
            _current_scope.Pop();
            base.ExitMethodDeclaration(context);
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
            _current_scope.Peek().define(ref m);
            _symbols[node] = m;
            Scope mm = (Scope)m;
            _scopes[context] = mm;
            _current_scope.Push(mm);
            base.EnterMethodDeclaration(context);
        }

        public override void EnterFieldDeclaration(Java9Parser.FieldDeclarationContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
                if (context.GetChild(i) as Java9Parser.UnannTypeContext != null)
                    break;
            int vdli = i + 1;
            var vdl = context.GetChild(vdli) as Java9Parser.VariableDeclaratorListContext;
            for (int j = 0; j < vdl.ChildCount; j += 2)
            {
                var vd = vdl.GetChild(j) as Java9Parser.VariableDeclaratorContext;
                var node = vd.GetChild(0);
                var name = node.GetText();
                Symbol f = new Symtab.FieldSymbol(name);
                _current_scope.Peek().define(ref f);
                _symbols[node] = f;
            }
            base.EnterFieldDeclaration(context);
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
            //cs.MonoType = new Mono.Cecil.TypeDefinition(null, id_name, default(Mono.Cecil.TypeAttributes));
            _current_scope.Peek().define(ref cs);
            _symbols[node] = cs;
            Scope s = (Scope)cs;
            _scopes[context] = s;
            _current_scope.Push(s);
            base.EnterNormalClassDeclaration(context);
        }

        public override void ExitNormalClassDeclaration(Java9Parser.NormalClassDeclarationContext context)
        {
            _current_scope.Pop();
            base.ExitNormalClassDeclaration(context);
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
            _current_scope.Peek().define(ref cs);
            _symbols[node] = cs;
            Scope s = (Scope)cs;
            _scopes[context] = s;
            _current_scope.Push(s);
            base.EnterNormalInterfaceDeclaration(context);
        }

        public override void ExitNormalInterfaceDeclaration(Java9Parser.NormalInterfaceDeclarationContext context)
        {
            _current_scope.Pop();
            base.ExitNormalInterfaceDeclaration(context);
        }

        public override void EnterBasicForStatement([NotNull] Java9Parser.BasicForStatementContext context)
        {
            var e = _current_scope.Peek();
            var b = new Symtab.LocalScope(e);
            _current_scope.Push(b);
            _scopes[context] = b;
            base.EnterBasicForStatement(context);
        }

        public override void ExitBasicForStatement([NotNull] Java9Parser.BasicForStatementContext context)
        {
            _current_scope.Pop();
            base.ExitBasicForStatement(context);
        }

        public override void EnterBasicForStatementNoShortIf([NotNull] Java9Parser.BasicForStatementNoShortIfContext context)
        {
            var e = _current_scope.Peek();
            var b = new Symtab.LocalScope(e);
            _current_scope.Push(b);
            _scopes[context] = b;
            base.EnterBasicForStatementNoShortIf(context);
        }

        public override void ExitBasicForStatementNoShortIf([NotNull] Java9Parser.BasicForStatementNoShortIfContext context)
        {
            _current_scope.Pop();
            base.ExitBasicForStatementNoShortIf(context);
        }

        public override void EnterEnhancedForStatement([NotNull] Java9Parser.EnhancedForStatementContext context)
        {
            var e = _current_scope.Peek();
            var b = new Symtab.LocalScope(e);
            _current_scope.Push(b);
            _scopes[context] = b;
            base.EnterEnhancedForStatement(context);
        }

        public override void ExitEnhancedForStatement([NotNull] Java9Parser.EnhancedForStatementContext context)
        {
            _current_scope.Pop();
            base.ExitEnhancedForStatement(context);
        }

        public override void EnterEnhancedForStatementNoShortIf([NotNull] Java9Parser.EnhancedForStatementNoShortIfContext context)
        {
            var e = _current_scope.Peek();
            var b = new Symtab.LocalScope(e);
            _current_scope.Push(b);
            _scopes[context] = b;
            base.EnterEnhancedForStatementNoShortIf(context);
        }

        public override void ExitEnhancedForStatementNoShortIf([NotNull] Java9Parser.EnhancedForStatementNoShortIfContext context)
        {
            _current_scope.Pop();
            base.ExitEnhancedForStatementNoShortIf(context);
        }

        public override void EnterTryWithResourcesStatement([NotNull] Java9Parser.TryWithResourcesStatementContext context)
        {
            var e = _current_scope.Peek();
            var b = new Symtab.LocalScope(e);
            _current_scope.Push(b);
            _scopes[context] = b;
            base.EnterTryWithResourcesStatement(context);
        }

        public override void ExitTryWithResourcesStatement([NotNull] Java9Parser.TryWithResourcesStatementContext context)
        {
            _current_scope.Pop();
            base.ExitTryWithResourcesStatement(context);
        }

        public override void ExitResource([NotNull] Java9Parser.ResourceContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
                if (context.GetChild(i) as Java9Parser.VariableDeclaratorIdContext != null)
                    break;
            var vdi = context.GetChild(i) as Java9Parser.VariableDeclaratorIdContext;
            var node = vdi;
            var name = vdi.GetChild(0).GetText();
            Symbol f = new Symtab.LocalSymbol(name);
            _symbols[node] = f;
            _current_scope.Peek().define(ref f);
            base.ExitResource(context);
        }

        public override void ExitLiteral([NotNull] Java9Parser.LiteralContext context)
        {
            var literal = context.GetText();
            var literal_symbol = new Symtab.Symtab.Literal(literal);
            _symbols[context] = literal_symbol;
            base.ExitLiteral(context);
        }
    }
}
