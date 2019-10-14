namespace LanguageServer.Java
{
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Collections.Generic;

    class Pass2Listener : Java9ParserBaseListener
    {
        private JavaParserDetails _pd;

        public Pass2Listener(JavaParserDetails pd)
        {
            _pd = pd;
        }

        public IScope NearestScope(IParseTree node)
        {
            for (; node != null; node = node.Parent)
            {
                if (_pd.Attributes.TryGetValue(node, out CombinedScopeSymbol value) && value is IScope)
                    return (IScope)value;
            }
            return null;
        }

        public override void EnterTypeVariable(Java9Parser.TypeVariableContext context)
        {
            int i;
            for (i = 0; i < context.ChildCount; ++i)
                if (context.GetChild(i) as Java9Parser.IdentifierContext != null)
                    break;
            var node = context.GetChild(i) as Java9Parser.IdentifierContext;
            var id_name = node.GetText();
            var scope = NearestScope(context);
            var s = scope.getSymbol(id_name);
            if (s != null)
                _pd.Attributes[node] = (CombinedScopeSymbol)s;
        }

        public override void EnterMethodInvocation_lfno_primary([NotNull] Java9Parser.MethodInvocation_lfno_primaryContext context)
        {
            //var name = context.GetText();
            //var scope = NearestScope(context);
            //var s_def = scope.LookupType(name);
            //if (s_def != null)
            //{
            //    var s_ref = new Symtab.RefSymbol(s_def);
            //    _attributes[context] = s_ref;
            //}
        }

        public override void EnterExpressionName([NotNull] Java9Parser.ExpressionNameContext context)
        {
            var first = context.GetChild(0);
            if (first is Java9Parser.IdentifierContext)
            {
                var sy = first.GetChild(0) as TerminalNodeImpl;
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
                    var id_name = sy.GetText();
                    var scope = NearestScope(context);
                    var s = scope.LookupType(id_name);
                    if (s != null) _pd.Attributes[first] = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
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
        }

        public override void ExitAmbiguousName([NotNull] Java9Parser.AmbiguousNameContext context)
        {
            // Synthesize attributes.
            var first = context.GetChild(0);
            if (first is Java9Parser.IdentifierContext)
            {
                var sy = first.GetChild(0) as TerminalNodeImpl;
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
                        var scope = NearestScope(context);
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _pd.Attributes[first] = (CombinedScopeSymbol)sc;
                    }
                    else
                    {
                        var scope = NearestScope(context);
                        var s = scope.LookupType(id_name);
                        if (s != null) _pd.Attributes[first] = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
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
                    _pd.Attributes.TryGetValue(first, out CombinedScopeSymbol v);
                    if (v != null && v is SymbolWithScope)
                    {
                        var sy = context.GetChild(2) as TerminalNodeImpl;
                        var id_name = context.GetChild(2).GetText();
                        var w = v as SymbolWithScope;
                        var s = w.LookupType(id_name);
                        if (s == null) return;
                        var x = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                        _pd.Attributes[context.GetChild(0)] = (CombinedScopeSymbol)x;
                        _pd.Attributes[context] = x;
                    }
                }
            }
        }

        public override void ExitModuleName([NotNull] Java9Parser.ModuleNameContext context)
        {
            // Synthesize attributes.
            var first = context.GetChild(0);
            if (first is Java9Parser.IdentifierContext)
            {
                var sy = first.GetChild(0) as TerminalNodeImpl;
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
                        var scope = NearestScope(context);
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _pd.Attributes[first] = (CombinedScopeSymbol)sc;
                    }
                    else
                    {
                        var scope = NearestScope(context);
                        var s = scope.LookupType(id_name);
                        if (s != null) _pd.Attributes[first] = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
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
                    _pd.Attributes.TryGetValue(first, out CombinedScopeSymbol v);
                    if (v != null && v is SymbolWithScope)
                    {
                        var sy = context.GetChild(2).GetChild(0) as TerminalNodeImpl;
                        var id_name = context.GetChild(2).GetText();
                        var w = v as SymbolWithScope;
                        var s = w.LookupType(id_name);
                        if (s != null) _pd.Attributes[context] = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                    }
                }
            }
        }

        public override void ExitPackageName([NotNull] Java9Parser.PackageNameContext context)
        {
            // Synthesize attributes.
            var first = context.GetChild(0);
            if (first is Java9Parser.IdentifierContext)
            {
                var sy = first.GetChild(0) as TerminalNodeImpl;
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
                        var scope = NearestScope(context);
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _pd.Attributes[first] = (CombinedScopeSymbol)sc;
                    }
                    else
                    {
                        var scope = NearestScope(context);
                        var s = scope.LookupType(id_name);
                        if (s != null) _pd.Attributes[first] = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
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
                    _pd.Attributes.TryGetValue(first, out CombinedScopeSymbol v);
                    if (v != null && v is SymbolWithScope)
                    {
                        var sy = context.GetChild(2).GetChild(0) as TerminalNodeImpl;
                        var id_name = context.GetChild(2).GetText();
                        var w = v as SymbolWithScope;
                        var s = w.LookupType(id_name);
                        if (s == null) return;
                        var x = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                        _pd.Attributes[context.GetChild(0)] = (CombinedScopeSymbol)x;
                        _pd.Attributes[context] = x;
                    }
                }
            }
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
                    var sy = first.GetChild(0) as TerminalNodeImpl;
                    if (id_name == "this")
                    {
                        var scope = NearestScope(context);
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _pd.Attributes[first] = (CombinedScopeSymbol)sc;
                    }
                    else
                    {
                        var scope = NearestScope(context);
                        var s = scope.LookupType(id_name);
                        if (s != null) _pd.Attributes[first] = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
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
                    _pd.Attributes.TryGetValue(first, out CombinedScopeSymbol v);
                    if (v != null && v is SymbolWithScope)
                    {
                        var sy = context.GetChild(2) as TerminalNodeImpl;
                        var id_name = context.GetChild(2).GetText();
                        var w = v as SymbolWithScope;
                        var s = w.LookupType(id_name);
                        if (s == null) return;
                        var x = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                        _pd.Attributes[context.GetChild(0)] = (CombinedScopeSymbol)x;
                        _pd.Attributes[context] = x;
                    }
                }
            }
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
                    var sy = first.GetChild(0) as TerminalNodeImpl;
                    if (id_name == "this")
                    {
                        var scope = NearestScope(context);
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _pd.Attributes[first] = (CombinedScopeSymbol)sc;
                    }
                    else
                    {
                        var scope = NearestScope(context);
                        var s = scope.LookupType(id_name);
                        if (s != null) _pd.Attributes[first] = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
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
                    _pd.Attributes.TryGetValue(first, out CombinedScopeSymbol v);
                    if (v != null && v is SymbolWithScope)
                    {
                        var sy = context.GetChild(2) as TerminalNodeImpl;
                        var id_name = context.GetChild(2).GetText();
                        var w = v as SymbolWithScope;
                        var s = w.LookupType(id_name);
                        if (s == null) return;
                        var x = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                        _pd.Attributes[context.GetChild(0)] = (CombinedScopeSymbol)x;
                        _pd.Attributes[context] = x;
                    }
                }
            }
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
                        var scope = NearestScope(context);
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _pd.Attributes[first] = (CombinedScopeSymbol)sc;
                    }
                    else
                    {
                        var scope = NearestScope(context);
                        var s = scope.LookupType(id_name);
                        var sy = first.GetChild(0) as TerminalNodeImpl;
                        if (s != null) _pd.Attributes[first] = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
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
                    _pd.Attributes.TryGetValue(first, out CombinedScopeSymbol v);
                    if (v != null && v is SymbolWithScope)
                    {
                        var sy = context.GetChild(2) as TerminalNodeImpl;
                        var id_name = context.GetChild(2).GetText();
                        var w = v as SymbolWithScope;
                        var s = w.LookupType(id_name);
                        if (s == null) return;
                        var x = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                        _pd.Attributes[context.GetChild(0)] = (CombinedScopeSymbol)x;
                        _pd.Attributes[context] = x;
                    }
                }
            }
        }

        public override void ExitPrimaryNoNewArray_lfno_primary([NotNull] Java9Parser.PrimaryNoNewArray_lfno_primaryContext context)
        {
            // Synthesize attributes.
            var first = context.GetChild(0);
            if (first is TerminalNodeImpl)
            {
                var sy = first as TerminalNodeImpl;
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
                        var scope = NearestScope(context);
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null)
                        {
                            _pd.Attributes[first] = (CombinedScopeSymbol)sc;
                            _pd.Attributes[context] = (CombinedScopeSymbol)sc;
                        }
                    }
                    else
                    {
                        var scope = NearestScope(context);
                        var s = scope.LookupType(id_name);
                        if (s != null)
                        {
                            _pd.Attributes[first] = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                            _pd.Attributes[context] = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                        }
                    }
                }
            }
        }

        public override void EnterIdentifier([NotNull] Java9Parser.IdentifierContext context)
        {
            var parent = context.Parent;
            if (parent is Java9Parser.FieldAccess_lfno_primaryContext)
            {
                var super = parent.GetChild(0);
                if (super is TerminalNodeImpl && super.GetText() == "super")
                {
                    var scope = NearestScope(context);
                    var sc = scope;
                    for (; sc != null; sc = sc.EnclosingScope)
                    {
                        if (sc is ClassSymbol) break;
                    }
                    if (sc == null)
                    {
                        return;
                    }
                    var super_class = sc.EnclosingScope;
                    if (super_class == null)
                    {
                        return;
                    }
                    var id = context.GetText();
                    var sy = context.GetChild(0) as TerminalNodeImpl;
                    var s = super_class.LookupType(id);
                    if (s == null) return;
                    var x = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                    _pd.Attributes[context.GetChild(0)] = (CombinedScopeSymbol)x;
                    _pd.Attributes[context] = x;
                }
            } else if (parent is Java9Parser.ClassInstanceCreationExpressionContext)
            {
                var p = (Antlr4.Runtime.ParserRuleContext)parent;
                var index = p.children.IndexOf(context);
                int rule_index = parent.RuleIndex;
                var first = p.GetChild(0);
                if ((first as TerminalNodeImpl)?.Symbol.Type == Java9Lexer.NEW)
                {
                    var id = context.GetText();
                    var sy = context.GetChild(0) as TerminalNodeImpl;
                    int i;
                    for (i = index - 1; i >= 0; i--)
                    {
                        if (p.GetChild(i) is Java9Parser.IdentifierContext)
                            break;
                    }
                    if (i < 0)
                    {
                        var scope = NearestScope(context);
                        var sc = scope;
                        var s = sc.LookupType(id);
                        if (s == null) return;
                        var x = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                        _pd.Attributes[context.GetChild(0)] = (CombinedScopeSymbol)x;
                        _pd.Attributes[context] = x;
                    }
                    else
                    {
                        var c = _pd.Attributes[p.GetChild(i)];
                        var sc = c as IScope;
                        if (sc != null)
                        {
                            var s = sc.LookupType(id);
                            if (s == null) return;
                            var x = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                            _pd.Attributes[context.GetChild(0)] = (CombinedScopeSymbol)x;
                            _pd.Attributes[context] = x;
                        }
                    }
                }
            } else if (parent is Java9Parser.ClassInstanceCreationExpression_lfno_primaryContext)
            {
                var p = (Antlr4.Runtime.ParserRuleContext)parent;
                var index = p.children.IndexOf(context);
                int rule_index = parent.RuleIndex;
                var first = p.GetChild(0);
                if ((first as TerminalNodeImpl)?.Symbol.Type == Java9Lexer.NEW)
                {
                    var id = context.GetText();
                    var sy = context.GetChild(0) as TerminalNodeImpl;
                    int i;
                    for (i = index - 1; i >= 0; i--)
                    {
                        if (p.GetChild(i) is Java9Parser.IdentifierContext)
                            break;
                    }
                    if (i < 0)
                    {
                        var scope = NearestScope(context);
                        var sc = scope;
                        var s = sc.LookupType(id);
                        if (s == null) return;
                        var x = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                        _pd.Attributes[context.GetChild(0)] = (CombinedScopeSymbol)x;
                        _pd.Attributes[context] = x;
                    }
                    else
                    {
                        var c = _pd.Attributes[p.GetChild(i)];
                        var sc = c as IScope;
                        if (sc != null)
                        {
                            var s = sc.LookupType(id);
                            if (s == null) return;
                            var x = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                            _pd.Attributes[context.GetChild(0)] = (CombinedScopeSymbol)x;
                            _pd.Attributes[context] = x;
                        }
                    }
                }
            } else if (parent is Java9Parser.FieldAccessContext)
            {
                var p = (Antlr4.Runtime.ParserRuleContext)parent;
                var index = p.children.IndexOf(context);
                var id_name = context.GetText();
                var sc = parent.GetChild(index-2);
                _pd.Attributes.TryGetValue(sc, out CombinedScopeSymbol ss);
                if (ss != null)
                {
                    var s = ((IScope)ss).LookupType(id_name);
                    if (s == null) return;
                    var sy = context.GetChild(0) as TerminalNodeImpl;
                    var x = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                    _pd.Attributes[parent] = (CombinedScopeSymbol)x;
                    _pd.Attributes[context.GetChild(0)] = (CombinedScopeSymbol)x;
                    _pd.Attributes[context] = x;
                }
            } else if (parent is Java9Parser.ExpressionNameContext)
            {
                var scope = NearestScope(context);
                var sc = scope;
                var id = context.GetText();
                var s = sc.LookupType(id);
                if (s == null) return;
                var sy = context.GetChild(0) as TerminalNodeImpl;
                var x = (CombinedScopeSymbol)new RefSymbol(sy.Symbol, s);
                _pd.Attributes[context.GetChild(0)] = (CombinedScopeSymbol)x;
                _pd.Attributes[context] = x;
            }
        }

        public override void ExitPrimary([NotNull] Java9Parser.PrimaryContext context)
        {
            var last = context.GetChild(context.ChildCount - 1);
            _pd.Attributes.TryGetValue(last, out CombinedScopeSymbol v);
            if (v != null)
            {
                _pd.Attributes[context] = v;
            }
        }
    }
}
