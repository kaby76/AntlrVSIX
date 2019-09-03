
using System;
using System.Collections.Generic;
using System.Text;
using org.antlr.symtab;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace AntlrVSIX.GrammarDescription.Java
{
    class MyJava9Listener : Java9BaseListener
    {
        SymbolTable st = new SymbolTable();
        Stack<Scope> current_scope = new Stack<Scope>();
        public Dictionary<IParseTree, Symbol> symbols = new Dictionary<IParseTree, Symbol>();

        public override void ExitInterfaceMethodDeclaration(Java9Parser.InterfaceMethodDeclarationContext context)
        {
            current_scope.Pop();
            base.ExitInterfaceMethodDeclaration(context);
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
            var m = new org.antlr.symtab.MethodSymbol(name);
            current_scope.Peek().define(m);
            symbols[node] = m;
            current_scope.Push(m);
            base.EnterInterfaceMethodDeclaration(context);
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
                var f = new org.antlr.symtab.FieldSymbol(name);
                symbols[node] = f;
                current_scope.Peek().define(f);
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
            var e = new org.antlr.symtab.EnumSymbol(name);
            current_scope.Peek().define(e);
            symbols[node] = e;
            base.EnterEnumDeclaration(context);
        }

        public override void EnterBlock(Java9Parser.BlockContext context)
        {
            var e = current_scope.Peek();
            var b = new org.antlr.symtab.LocalScope(e);
            current_scope.Push(b);
            base.EnterBlock(context);
        }

        public override void ExitBlock(Java9Parser.BlockContext context)
        {
            current_scope.Pop();
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
            var v = new org.antlr.symtab.MethodSymbol(name);
            current_scope.Peek().define(v);
            symbols[node] = v;
            base.EnterFormalParameter(context);
        }

        public override void ExitMethodDeclaration(Java9Parser.MethodDeclarationContext context)
        {
            current_scope.Pop();
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
            var m = new org.antlr.symtab.MethodSymbol(name);
            current_scope.Peek().define(m);
            symbols[node] = m;
            current_scope.Push(m);
            base.EnterMethodDeclaration(context);
        }


        public MyJava9Listener()
        {
            current_scope.Push(st.GLOBALS);
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
                var f = new org.antlr.symtab.FieldSymbol(name);
                current_scope.Peek().define(f);
                symbols[node] = f;
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
            var cs = new org.antlr.symtab.ClassSymbol(id_name);
            current_scope.Peek().define(cs);
            symbols[node] = cs;
            current_scope.Push(cs);
            base.EnterNormalClassDeclaration(context);
        }

        public override void ExitNormalClassDeclaration(Java9Parser.NormalClassDeclarationContext context)
        {
            current_scope.Pop();
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
            var cs = new org.antlr.symtab.InterfaceSymbol(id_name);
            current_scope.Peek().define(cs);
            symbols[node] = cs;
            current_scope.Push(cs);
            base.EnterNormalInterfaceDeclaration(context);
        }

        public override void ExitNormalInterfaceDeclaration(Java9Parser.NormalInterfaceDeclarationContext context)
        {
            current_scope.Pop();
            base.ExitNormalInterfaceDeclaration(context);
        }

    }
}