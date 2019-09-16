using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Symtab;
using System.Collections.Generic;

namespace AntlrVSIX.GrammarDescription.Java
{
    class Pass2Listener : Java9ParserBaseListener
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
            var m = (Symtab.MethodSymbol)_scopes[context];
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
            var e = (Symtab.EnumSymbol)_scopes[context];
            base.EnterEnumDeclaration(context);
        }

        public override void EnterBlock(Java9Parser.BlockContext context)
        {
            var b = (Symtab.LocalScope)_scopes[context];
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
            var m = (Symtab.MethodSymbol)_scopes[context];
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
            var m = (Symtab.ClassSymbol)_scopes[context];
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
            var m = (Symtab.InterfaceSymbol)_scopes[context];
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

        public override void EnterLocalVariableDeclaration(Java9Parser.LocalVariableDeclarationContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
                if (context.GetChild(i) as Java9Parser.UnannTypeContext != null)
                    break;
            var unanntype = context.GetChild(i) as Java9Parser.UnannTypeContext;
            var c1 = unanntype.GetChild(0);
            if (c1 is Java9Parser.UnannPrimitiveTypeContext)
            {
                var type_name = c1.GetText();
                var type = new Symtab.PrimitiveType(type_name);
                type.MonoType = new Mono.Cecil.TypeReference(null, type_name, null, null, true);
            } else if (c1 is Java9Parser.UnannReferenceTypeContext)
            {

            }

            base.EnterLocalVariableDeclaration(context);
        }

        public override void EnterMethodInvocation_lfno_primary([NotNull] Java9Parser.MethodInvocation_lfno_primaryContext context)
        {
            var name = context.GetText();
            var s_ref = new Symtab.MethodSymbol(name);
            var s_def = _current_scope.Peek().resolve(name);
            if (s_def != null)
            {
                s_ref.definition = s_def;
            }
            _symbols[context] = s_ref;
            base.EnterMethodInvocation_lfno_primary(context);
        }

        public override void EnterBasicForStatement([NotNull] Java9Parser.BasicForStatementContext context)
        {
            var b = (Symtab.LocalScope)_scopes[context];
            _current_scope.Push(b);
            base.EnterBasicForStatement(context);
        }

        public override void ExitBasicForStatement([NotNull] Java9Parser.BasicForStatementContext context)
        {
            _current_scope.Pop();
            base.ExitBasicForStatement(context);
        }

        public override void EnterBasicForStatementNoShortIf([NotNull] Java9Parser.BasicForStatementNoShortIfContext context)
        {
            var b = (Symtab.LocalScope)_scopes[context];
            _current_scope.Push(b);
            base.EnterBasicForStatementNoShortIf(context);
        }

        public override void ExitBasicForStatementNoShortIf([NotNull] Java9Parser.BasicForStatementNoShortIfContext context)
        {
            _current_scope.Pop();
            base.ExitBasicForStatementNoShortIf(context);
        }

        public override void EnterEnhancedForStatement([NotNull] Java9Parser.EnhancedForStatementContext context)
        {
            var b = (Symtab.LocalScope)_scopes[context];
            _current_scope.Push(b);
            base.EnterEnhancedForStatement(context);
        }

        public override void ExitEnhancedForStatement([NotNull] Java9Parser.EnhancedForStatementContext context)
        {
            _current_scope.Pop();
            base.ExitEnhancedForStatement(context);
        }

        public override void EnterEnhancedForStatementNoShortIf([NotNull] Java9Parser.EnhancedForStatementNoShortIfContext context)
        {
            var b = (Symtab.LocalScope)_scopes[context];
            _current_scope.Push(b);
            base.EnterEnhancedForStatementNoShortIf(context);
        }

        public override void ExitEnhancedForStatementNoShortIf([NotNull] Java9Parser.EnhancedForStatementNoShortIfContext context)
        {
            _current_scope.Pop();
            base.ExitEnhancedForStatementNoShortIf(context);
        }

        public override void EnterExpressionName([NotNull] Java9Parser.ExpressionNameContext context)
        {
            var first = context.GetChild(0);
            if (first is Java9Parser.IdentifierContext)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    var id_name = first.GetText();
                    var s = _current_scope.Peek().resolve(id_name);
                    if (s != null) _symbols[first] = s;
                }
            }
            else if (first is Java9Parser.AmbiguousNameContext)
            {
                //bool is_statement = false;
                //for (var p = first.Parent; p != null; p = p.Parent)
                //{
                //    if (p is Java9Parser.StatementContext)
                //    {
                //        is_statement = true;
                //        break;
                //    }
                //}
                //if (is_statement)
                //{
                //    var id_name = first.GetText();
                //    var s = _current_scope.Peek().resolve(id_name);
                //    if (s != null) _symbols[first] = s;
                //}
            }
            base.EnterExpressionName(context);
        }

        public override void ExitAmbiguousName([NotNull] Java9Parser.AmbiguousNameContext context)
        {
            // Synthesize attributes.
            var first = context.GetChild(0);
            if (first is Java9Parser.IdentifierContext)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    var id_name = first.GetText();
                    if (id_name == "this")
                    {
                        var sc = _current_scope.Peek();
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _symbols[first] = (Symbol)sc;
                    }
                    else
                    {
                        var s = _current_scope.Peek().resolve(id_name);
                        if (s != null) _symbols[first] = s;
                    }
                }
            }
            else if (first is Java9Parser.AmbiguousNameContext)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    _symbols.TryGetValue(first, out Symbol v);
                    if (v != null && v is SymbolWithScope)
                    {
                        var id_name = context.GetChild(2).GetText();
                        var w = v as SymbolWithScope;
                        var s = w.resolve(id_name);
                        if (s != null) _symbols[context] = s;
                    }
                }
            }
            base.ExitAmbiguousName(context);
        }

        public override void ExitModuleName([NotNull] Java9Parser.ModuleNameContext context)
        {
            // Synthesize attributes.
            var first = context.GetChild(0);
            if (first is Java9Parser.IdentifierContext)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    var id_name = first.GetText();
                    if (id_name == "this")
                    {
                        var sc = _current_scope.Peek();
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _symbols[first] = (Symbol)sc;
                    }
                    else
                    {
                        var s = _current_scope.Peek().resolve(id_name);
                        if (s != null) _symbols[first] = s;
                    }
                }
            }
            else if (first is Java9Parser.ModuleNameContext)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    _symbols.TryGetValue(first, out Symbol v);
                    if (v != null && v is SymbolWithScope)
                    {
                        var id_name = context.GetChild(2).GetText();
                        var w = v as SymbolWithScope;
                        var s = w.resolve(id_name);
                        if (s != null) _symbols[context] = s;
                    }
                }
            }
            base.ExitModuleName(context);
        }

        public override void ExitPackageName([NotNull] Java9Parser.PackageNameContext context)
        {
            // Synthesize attributes.
            var first = context.GetChild(0);
            if (first is Java9Parser.IdentifierContext)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    var id_name = first.GetText();
                    if (id_name == "this")
                    {
                        var sc = _current_scope.Peek();
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _symbols[first] = (Symbol)sc;
                    }
                    else
                    {
                        var s = _current_scope.Peek().resolve(id_name);
                        if (s != null) _symbols[first] = s;
                    }
                }
            }
            else if (first is Java9Parser.PackageNameContext)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    _symbols.TryGetValue(first, out Symbol v);
                    if (v != null && v is SymbolWithScope)
                    {
                        var id_name = context.GetChild(2).GetText();
                        var w = v as SymbolWithScope;
                        var s = w.resolve(id_name);
                        if (s != null) _symbols[context] = s;
                    }
                }
            }
            base.ExitPackageName(context);
        }

        public override void ExitTypeName([NotNull] Java9Parser.TypeNameContext context)
        {
            // Synthesize attributes.
            var first = context.GetChild(0);
            if (first is Java9Parser.IdentifierContext)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    var id_name = first.GetText();
                    if (id_name == "this")
                    {
                        var sc = _current_scope.Peek();
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _symbols[first] = (Symbol)sc;
                    }
                    else
                    {
                        var s = _current_scope.Peek().resolve(id_name);
                        if (s != null) _symbols[first] = s;
                    }
                }
            }
            else if (first is Java9Parser.TypeNameContext)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    _symbols.TryGetValue(first, out Symbol v);
                    if (v != null && v is SymbolWithScope)
                    {
                        var id_name = context.GetChild(2).GetText();
                        var w = v as SymbolWithScope;
                        var s = w.resolve(id_name);
                        if (s != null) _symbols[context] = s;
                    }
                }
            }
            base.ExitTypeName(context);
        }

        public override void ExitPackageOrTypeName([NotNull] Java9Parser.PackageOrTypeNameContext context)
        {
            // Synthesize attributes.
            var first = context.GetChild(0);
            if (first is Java9Parser.IdentifierContext)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    var id_name = first.GetText();
                    if (id_name == "this")
                    {
                        var sc = _current_scope.Peek();
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _symbols[first] = (Symbol)sc;
                    }
                    else
                    {
                        var s = _current_scope.Peek().resolve(id_name);
                        if (s != null) _symbols[first] = s;
                    }
                }
            }
            else if (first is Java9Parser.PackageOrTypeNameContext)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    _symbols.TryGetValue(first, out Symbol v);
                    if (v != null && v is SymbolWithScope)
                    {
                        var id_name = context.GetChild(2).GetText();
                        var w = v as SymbolWithScope;
                        var s = w.resolve(id_name);
                        if (s != null) _symbols[context] = s;
                    }
                }
            }
            base.ExitPackageOrTypeName(context);
        }

        public override void ExitExpressionName([NotNull] Java9Parser.ExpressionNameContext context)
        {
            // Synthesize attributes.
            var first = context.GetChild(0);
            if (first is Java9Parser.IdentifierContext)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                    if (p is Java9Parser.BlockStatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    var id_name = first.GetText();
                    if (id_name == "this")
                    {
                        var sc = _current_scope.Peek();
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _symbols[first] = (Symbol)sc;
                    }
                    else
                    {
                        var s = _current_scope.Peek().resolve(id_name);
                        if (s != null) _symbols[first] = s;
                    }
                }
            }
            else if (first is Java9Parser.ExpressionNameContext)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                    if (p is Java9Parser.BlockStatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    _symbols.TryGetValue(first, out Symbol v);
                    if (v != null && v is SymbolWithScope)
                    {
                        var id_name = context.GetChild(2).GetText();
                        var w = v as SymbolWithScope;
                        var s = w.resolve(id_name);
                        if (s != null) _symbols[context] = s;
                    }
                }
            }
            base.ExitExpressionName(context);
        }

        public override void ExitPrimaryNoNewArray_lfno_primary([NotNull] Java9Parser.PrimaryNoNewArray_lfno_primaryContext context)
        {
            // Synthesize attributes.
            var first = context.GetChild(0);
            if (first is TerminalNodeImpl)
            {
                bool is_statement = false;
                for (var p = first.Parent; p != null; p = p.Parent)
                {
                    if (p is Java9Parser.StatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                    if (p is Java9Parser.BlockStatementContext)
                    {
                        is_statement = true;
                        break;
                    }
                }
                if (is_statement)
                {
                    var id_name = first.GetText();
                    if (id_name == "this")
                    {
                        var sc = _current_scope.Peek();
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _symbols[first] = (Symbol)sc;
                    } else
                    {
                        var s = _current_scope.Peek().resolve(id_name);
                        if (s != null) _symbols[first] = s;
                    }
                }
            }
            base.ExitPrimaryNoNewArray_lfno_primary(context);
        }

        public override void ExitFieldAccess([NotNull] Java9Parser.FieldAccessContext context)
        {
            var first = context.GetChild(0);
            if (first is Java9Parser.PrimaryContext)
            {
                var c = context.GetChild(2);
                var id_name = c.GetText();
                _symbols.TryGetValue(first, out Symbol p);
                if (p != null)
                {
                    var s = ((Scope)p).resolve(id_name);
                    if (s != null)
                    {
                        _symbols[c] = s;
                        _symbols[context] = s;
                    }
                }
            }
            base.ExitFieldAccess(context);
        }
    }
}
