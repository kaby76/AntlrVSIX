
using System;
using System.Collections.Generic;
using System.Text;
using Symtab;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

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
            var m = new Symtab.MethodSymbol(name);
            _current_scope.Peek().define(m);
            _symbols[node] = m;
            _scopes[context] = m;
            _current_scope.Push(m);
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
                var f = new Symtab.LocalSymbol(name);
                _symbols[node] = f;
                _current_scope.Peek().define(f);
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
            var e = new Symtab.EnumSymbol(name);
            _current_scope.Peek().define(e);
            _symbols[node] = e;
            base.EnterEnumDeclaration(context);
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
            var v = new Symtab.MethodSymbol(name);
            _current_scope.Peek().define(v);
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
            var m = new Symtab.MethodSymbol(name);
            _current_scope.Peek().define(m);
            _symbols[node] = m;
            _scopes[context] = m;
            _current_scope.Push(m);
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
                var f = new Symtab.FieldSymbol(name);
                _current_scope.Peek().define(f);
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
            var cs = new Symtab.ClassSymbol(id_name);
            cs.MonoType = new Mono.Cecil.TypeDefinition(null, id_name, default(Mono.Cecil.TypeAttributes));
            _current_scope.Peek().define(cs);
            _symbols[node] = cs;
            _scopes[context] = cs;
            _current_scope.Push(cs);
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
            var cs = new Symtab.InterfaceSymbol(id_name);
            _current_scope.Peek().define(cs);
            _symbols[node] = cs;
            _scopes[context] = cs;
            _current_scope.Push(cs);
            base.EnterNormalInterfaceDeclaration(context);
        }

        public override void ExitNormalInterfaceDeclaration(Java9Parser.NormalInterfaceDeclarationContext context)
        {
            _current_scope.Pop();
            base.ExitNormalInterfaceDeclaration(context);
        }
    }
}
