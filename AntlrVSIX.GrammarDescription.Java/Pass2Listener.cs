using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using org.antlr.symtab;
using System.Collections.Generic;

namespace AntlrVSIX.GrammarDescription.Java
{
    class Pass2Listener : Java9BaseListener
    {
        Stack<Scope> _current_scope;
        public Dictionary<IParseTree, Symbol> _symbols;
        Dictionary<IParseTree, Scope> _scopes;

        public Pass2Listener(Stack<Scope> current_scope, Dictionary<IParseTree, Symbol> symbols, Dictionary<IParseTree, Scope> scopes)
        {
            _current_scope = current_scope;
            _symbols = symbols;
            _scopes = scopes;
        }

        public override void EnterInterfaceMethodDeclaration(Java9Parser.InterfaceMethodDeclarationContext context)
        {
            var m = (org.antlr.symtab.MethodSymbol)_scopes[context];
            _current_scope.Push(m);
            base.EnterInterfaceMethodDeclaration(context);
        }

        public override void ExitInterfaceMethodDeclaration(Java9Parser.InterfaceMethodDeclarationContext context)
        {
            _current_scope.Pop();
            base.ExitInterfaceMethodDeclaration(context);
        }

        public override void EnterEnumDeclaration(Java9Parser.EnumDeclarationContext context)
        {
            var e = (org.antlr.symtab.EnumSymbol)_scopes[context];
            base.EnterEnumDeclaration(context);
        }

        public override void EnterBlock(Java9Parser.BlockContext context)
        {
            var b = (org.antlr.symtab.LocalScope)_scopes[context];
            _current_scope.Push(b);
            base.EnterBlock(context);
        }

        public override void ExitBlock(Java9Parser.BlockContext context)
        {
            _current_scope.Pop();
            base.ExitBlock(context);
        }

        public override void EnterMethodDeclaration(Java9Parser.MethodDeclarationContext context)
        {
            var m = (org.antlr.symtab.MethodSymbol)_scopes[context];
            _current_scope.Push(m);
            base.EnterMethodDeclaration(context);
        }

        public override void ExitMethodDeclaration(Java9Parser.MethodDeclarationContext context)
        {
            _current_scope.Pop();
            base.ExitMethodDeclaration(context);
        }

        public override void EnterNormalClassDeclaration(Java9Parser.NormalClassDeclarationContext context)
        {
            var m = (org.antlr.symtab.ClassSymbol)_scopes[context];
            _current_scope.Push(m);
            base.EnterNormalClassDeclaration(context);
        }

        public override void ExitNormalClassDeclaration(Java9Parser.NormalClassDeclarationContext context)
        {
            _current_scope.Pop();
            base.ExitNormalClassDeclaration(context);
        }

        public override void EnterNormalInterfaceDeclaration(Java9Parser.NormalInterfaceDeclarationContext context)
        {
            var m = (org.antlr.symtab.InterfaceSymbol)_scopes[context];
            _current_scope.Push(m);
            base.EnterNormalInterfaceDeclaration(context);
        }

        public override void ExitNormalInterfaceDeclaration(Java9Parser.NormalInterfaceDeclarationContext context)
        {
            _current_scope.Pop();
            base.ExitNormalInterfaceDeclaration(context);
        }

        public override void EnterTypeVariable(Java9Parser.TypeVariableContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
                if (context.GetChild(i) as Java9Parser.IdentifierContext != null)
                    break;
            var node = context.GetChild(i) as Java9Parser.IdentifierContext;
            var id_name = node.GetText();
            var s = _current_scope.Peek().getSymbol(id_name);
            if (s != null)
                _symbols[node] = s;
            base.EnterTypeVariable(context);
        }
    }
}
