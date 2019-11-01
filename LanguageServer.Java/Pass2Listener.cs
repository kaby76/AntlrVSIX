namespace LanguageServer.Java
{
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Collections.Generic;
    using System.Linq;

    class Pass2Listener : Java9ParserBaseListener
    {
        private JavaParserDetails _pd;

        public Pass2Listener(JavaParserDetails pd)
        {
            _pd = pd;
        }

        public IParseTree NearestScope(IParseTree node)
        {
            for (; node != null; node = node.Parent)
            {
                _pd.Attributes.TryGetValue(node, out IList<CombinedScopeSymbol> list);
                if (list != null)
                {
                    if (list.Count == 1 && list[0] is IScope)
                        return node;
                }
            }
            return null;
        }

        public IScope GetScope(IParseTree node)
        {
            if (node == null)
                return null;
            _pd.Attributes.TryGetValue(node, out IList<CombinedScopeSymbol> list);
            if (list != null)
            {
                if (list.Count == 1 && list[0] is IScope)
                    return list[0] as IScope;
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
            var scope = GetScope(NearestScope(context));
            var s = scope.getSymbol(id_name);
            if (s != null)
                _pd.Attributes[node] = new List<CombinedScopeSymbol>() { (CombinedScopeSymbol)s };
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
                    var scope = GetScope(NearestScope(context));
                    var list_s = scope.LookupType(id_name);
                    if (list_s.Any())
                    {
                        var ref_list = new List<CombinedScopeSymbol>();
                        foreach (var s in list_s)
                        {
                            ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                        }
                        if (ref_list.Any())
                            _pd.Attributes[first] = ref_list;
                    }
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
                        var scope = GetScope(NearestScope(context));
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _pd.Attributes[first] = new List<CombinedScopeSymbol>() { (CombinedScopeSymbol)sc };
                    }
                    else
                    {
                        var scope = GetScope(NearestScope(context));
                        var list_s = scope.LookupType(id_name);
                        var ref_list = new List<CombinedScopeSymbol>();
                        foreach (var s in list_s)
                        {
                            ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                        }
                        if (ref_list.Any())
                            _pd.Attributes[first] = ref_list;
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
                    _pd.Attributes.TryGetValue(first, out IList<CombinedScopeSymbol> list_v);
                    var ref_list = new List<CombinedScopeSymbol>();
                    foreach (var v in list_v)
                    {
                        if (v is SymbolWithScope)
                        {
                            var sy = context.GetChild(2) as TerminalNodeImpl;
                            var id_name = context.GetChild(2).GetText();
                            var w = v as SymbolWithScope;
                            var list_s = w.LookupType(id_name);
                            if (list_s == null || !list_s.Any()) continue;
                            foreach (var s in list_s)
                            {
                                ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                            }
                        }
                    }
                    if (ref_list.Any())
                    {
                        _pd.Attributes[context.GetChild(0)] = ref_list;
                        _pd.Attributes[context] = ref_list;
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
                        var scope = GetScope(NearestScope(context));
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _pd.Attributes[first] = new List<CombinedScopeSymbol>() { (CombinedScopeSymbol)sc };
                    }
                    else
                    {
                        var scope = GetScope(NearestScope(context));
                        var list_s = scope.LookupType(id_name);
                        var ref_list = new List<CombinedScopeSymbol>();
                        foreach (var s in list_s)
                        {
                            ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                        }
                        if (ref_list.Any())
                            _pd.Attributes[first] = ref_list;
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
                    _pd.Attributes.TryGetValue(first, out IList<CombinedScopeSymbol> list_v);
                    var ref_list = new List<CombinedScopeSymbol>();
                    foreach (var v in list_v)
                    {
                        if (v is SymbolWithScope)
                        {
                            var sy = context.GetChild(2).GetChild(0) as TerminalNodeImpl;
                            var id_name = context.GetChild(2).GetText();
                            var w = v as SymbolWithScope;
                            var list_s = w.LookupType(id_name);
                            if (list_s == null || !list_s.Any()) continue;
                            foreach (var s in list_s)
                            {
                                ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                            }
                        }
                    }
                    if (ref_list.Any())
                    {
                        _pd.Attributes[context] = ref_list;
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
                        var scope = GetScope(NearestScope(context));
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _pd.Attributes[first] = new List<CombinedScopeSymbol>() { (CombinedScopeSymbol)sc };
                    }
                    else
                    {
                        var scope = GetScope(NearestScope(context));
                        var list_s = scope.LookupType(id_name);
                        var ref_list = new List<CombinedScopeSymbol>();
                        foreach (var s in list_s)
                        {
                            ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                        }
                        if (ref_list.Any())
                            _pd.Attributes[first] = ref_list;
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
                    _pd.Attributes.TryGetValue(first, out IList<CombinedScopeSymbol> list_v);
                    var ref_list = new List<CombinedScopeSymbol>();
                    foreach (var v in list_v)
                    {
                        if (v is SymbolWithScope)
                        {
                            var sy = context.GetChild(2).GetChild(0) as TerminalNodeImpl;
                            var id_name = context.GetChild(2).GetText();
                            var w = v as SymbolWithScope;
                            var list_s = w.LookupType(id_name);
                            if (list_s == null || !list_s.Any()) continue;
                            foreach (var s in list_s)
                            {
                                ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                            }
                        }
                    }
                    if (ref_list.Any())
                    {
                        _pd.Attributes[context.GetChild(0)] = ref_list;
                        _pd.Attributes[context] = ref_list;
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
                        var scope = GetScope(NearestScope(context));
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _pd.Attributes[first] = new List<CombinedScopeSymbol>() { (CombinedScopeSymbol)sc };
                    }
                    else
                    {
                        var scope = GetScope(NearestScope(context));
                        var list_s = scope.LookupType(id_name);
                        var ref_list = new List<CombinedScopeSymbol>();
                        foreach (var s in list_s)
                        {
                            ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                        }
                        if (ref_list.Any())
                            _pd.Attributes[first] = ref_list;
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
                    _pd.Attributes.TryGetValue(first, out IList<CombinedScopeSymbol> list_v);
                    var ref_list = new List<CombinedScopeSymbol>();
                    foreach (var v in list_v)
                    {
                        if (v is SymbolWithScope)
                        {
                            var sy = context.GetChild(2) as TerminalNodeImpl;
                            var id_name = context.GetChild(2).GetText();
                            var w = v as SymbolWithScope;
                            var list_s = w.LookupType(id_name);
                            if (list_s == null || !list_s.Any()) continue;
                            foreach (var s in list_s)
                            {
                                ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                            }
                        }
                    }
                    if (ref_list.Any())
                    {
                        _pd.Attributes[context.GetChild(0)] = ref_list;
                        _pd.Attributes[context] = ref_list;
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
                        var scope = GetScope(NearestScope(context));
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _pd.Attributes[first] = new List<CombinedScopeSymbol>() { (CombinedScopeSymbol)sc };
                    }
                    else
                    {
                        var scope = GetScope(NearestScope(context));
                        var list_s = scope.LookupType(id_name);
                        var ref_list = new List<CombinedScopeSymbol>();
                        foreach (var s in list_s)
                        {
                            ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                        }
                        if (ref_list.Any())
                        {
                            _pd.Attributes[first] = ref_list;
                        }
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
                    _pd.Attributes.TryGetValue(first, out IList<CombinedScopeSymbol> list_v);
                    var ref_list = new List<CombinedScopeSymbol>();
                    foreach (var v in list_v)
                    {
                        if (v is SymbolWithScope)
                        {
                            var sy = context.GetChild(2) as TerminalNodeImpl;
                            var id_name = context.GetChild(2).GetText();
                            var w = v as SymbolWithScope;
                            var list_s = w.LookupType(id_name);
                            if (list_s == null || !list_s.Any()) continue;
                            foreach (var s in list_s)
                            {
                                ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                            }
                        }
                    }
                    if (ref_list.Any())
                    {
                        _pd.Attributes[context.GetChild(0)] = ref_list;
                        _pd.Attributes[context] = ref_list;
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
                        var scope = GetScope(NearestScope(context));
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null) _pd.Attributes[first] = new List<CombinedScopeSymbol>() { (CombinedScopeSymbol)sc };
                    }
                    else
                    {
                        var scope = GetScope(NearestScope(context));
                        var sy = first.GetChild(0) as TerminalNodeImpl;
                        var list_s = scope.LookupType(id_name);
                        var ref_list = new List<CombinedScopeSymbol>();
                        foreach (var s in list_s)
                        {
                            ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                        }
                        if (ref_list.Any())
                        {
                            _pd.Attributes[first] = ref_list;
                        }
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
                    _pd.Attributes.TryGetValue(first, out IList<CombinedScopeSymbol> list_v);
                    var ref_list = new List<CombinedScopeSymbol>();
                    foreach (var v in list_v)
                    {
                        if (v is SymbolWithScope)
                        {
                            var sy = context.GetChild(2) as TerminalNodeImpl;
                            var id_name = context.GetChild(2).GetText();
                            var w = v as SymbolWithScope;
                            var list_s = w.LookupType(id_name);
                            if (list_s == null || !list_s.Any()) continue;
                            foreach (var s in list_s)
                            {
                                ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                            }
                        }
                    }
                    if (ref_list.Any())
                    {
                        _pd.Attributes[context.GetChild(0)] = ref_list;
                        _pd.Attributes[context] = ref_list;
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
                        var scope = GetScope(NearestScope(context));
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null)
                        {
                            _pd.Attributes[first] = new List<CombinedScopeSymbol>() { (CombinedScopeSymbol)sc };
                            _pd.Attributes[context] = new List<CombinedScopeSymbol>() { (CombinedScopeSymbol)sc };
                        }
                    }
                    else
                    {
                        var scope = GetScope(NearestScope(context));
                        var list_s = scope.LookupType(id_name);
                        var ref_list = new List<CombinedScopeSymbol>();
                        foreach (var s in list_s)
                        {
                            ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                        }
                        if (ref_list.Any())
                        {
                            _pd.Attributes[first] = ref_list;
                            _pd.Attributes[context] = ref_list;
                        }
                    }
                }
            }
        }

        public override void EnterPrimaryNoNewArray_lfno_primary([NotNull] Java9Parser.PrimaryNoNewArray_lfno_primaryContext context)
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
                        var scope = GetScope(NearestScope(context));
                        var sc = scope;
                        for (; sc != null; sc = sc.EnclosingScope)
                        {
                            if (sc is ClassSymbol) break;
                        }
                        if (sc != null)
                        {
                            _pd.Attributes[first] = new List<CombinedScopeSymbol>() { (CombinedScopeSymbol)sc };
                            _pd.Attributes[context] = new List<CombinedScopeSymbol>() { (CombinedScopeSymbol)sc };
                        }
                    }
                    else
                    {
                        var scope = GetScope(NearestScope(context));
                        var list_s = scope.LookupType(id_name);
                        var ref_list = new List<CombinedScopeSymbol>();
                        foreach (var s in list_s)
                        {
                            ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                        }
                        if (ref_list.Any())
                        {
                            _pd.Attributes[first] = ref_list;
                            _pd.Attributes[context] = ref_list;
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
                    var super_class = sc.EnclosingScope;
                    if (super_class == null)
                    {
                        return;
                    }
                    var id = context.GetText();
                    var sy = context.GetChild(0) as TerminalNodeImpl;
                    var list_s = super_class.LookupType(id);
                    if (list_s == null || !list_s.Any()) return;
                    var ref_list = new List<CombinedScopeSymbol>();
                    foreach (var s in list_s)
                    {
                        ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                    }
                    if (ref_list.Any())
                    {
                        _pd.Attributes[context.GetChild(0)] = ref_list;
                        _pd.Attributes[context] = ref_list;
                    }
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
                        var scope = GetScope(NearestScope(context));
                        var list_s = scope.LookupType(id);
                        var ref_list = new List<CombinedScopeSymbol>();
                        foreach (var s in list_s)
                        {
                            ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                        }
                        if (ref_list.Any())
                        {
                            _pd.Attributes[context.GetChild(0)] = ref_list;
                            _pd.Attributes[context] = ref_list;
                        }
                    }
                    else
                    {
                        var c = _pd.Attributes[p.GetChild(i)];
                        if (c != null && c.Count == 1)
                        {
                            var sc = c.First() as IScope;
                            var list_s = sc.LookupType(id);
                            if (list_s == null || !list_s.Any()) return;
                            var ref_list = new List<CombinedScopeSymbol>();
                            foreach (var s in list_s)
                            {
                                ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                            }
                            if (ref_list.Any())
                            {
                                _pd.Attributes[context.GetChild(0)] = ref_list;
                                _pd.Attributes[context] = ref_list;
                            }
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
                        var scope = GetScope(NearestScope(context));
                        var sc = scope;
                        var list_s = sc.LookupType(id);
                        if (list_s == null || !list_s.Any()) return;
                        var ref_list = new List<CombinedScopeSymbol>();
                        foreach (var s in list_s)
                        {
                            ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                        }
                        if (ref_list.Any())
                        {
                            _pd.Attributes[context.GetChild(0)] = ref_list;
                            _pd.Attributes[context] = ref_list;
                        }
                    }
                    else
                    {
                        var c = _pd.Attributes[p.GetChild(i)];
                        if (c != null && c.Count == 1)
                        {
                            var sc = c.First() as IScope;
                            var list_s = sc.LookupType(id);
                            if (list_s == null || !list_s.Any()) return;
                            var ref_list = new List<CombinedScopeSymbol>();
                            foreach (var s in list_s)
                            {
                                ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                            }
                            if (ref_list.Any())
                            {
                                _pd.Attributes[context.GetChild(0)] = ref_list;
                                _pd.Attributes[context] = ref_list;
                            }
                        }
                    }
                }
            } else if (parent is Java9Parser.FieldAccessContext)
            {
                var p = (Antlr4.Runtime.ParserRuleContext)parent;
                var index = p.children.IndexOf(context);
                var id_name = context.GetText();
                var sc = parent.GetChild(index-2);
                _pd.Attributes.TryGetValue(sc, out IList<CombinedScopeSymbol> ss);
                if (ss != null && ss.Count == 1)
                {
                    var list_s = ((IScope)ss.First()).LookupType(id_name);
                    if (list_s == null || !list_s.Any()) return;
                    var sy = context.GetChild(0) as TerminalNodeImpl;
                    var ref_list = new List<CombinedScopeSymbol>();
                    foreach (var s in list_s)
                    {
                        ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                    }
                    if (ref_list.Any())
                    {
                        _pd.Attributes[parent] = ref_list;
                        _pd.Attributes[context.GetChild(0)] = ref_list;
                        _pd.Attributes[context] = ref_list;
                    }
                }
            }
            else if (parent is Java9Parser.ExpressionNameContext)
            {
                var scope = GetScope(NearestScope(context));
                var sc = scope;
                var id = context.GetText();
                IList<ISymbol> list_s = sc.LookupType(id);
                if (list_s == null || !list_s.Any()) return;
                // Note we need to eliminate ambiguity here. It could be
                // a field or parameter or local variable. Just pick the first.
                list_s = new List<ISymbol>() { list_s.First() };
                var sy = context.GetChild(0) as TerminalNodeImpl;
                var ref_list = new List<CombinedScopeSymbol>();
                foreach (var s in list_s)
                {
                    ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                }
                if (ref_list.Any())
                {
                    _pd.Attributes[context.GetChild(0)] = ref_list;
                    _pd.Attributes[context] = ref_list;
                }
            }
            else if (parent is Java9Parser.TypeNameContext && parent.ChildCount == 1)
            {
                var scope = GetScope(NearestScope(context));
                var sc = scope;
                var id = context.GetText();
                IList<ISymbol> list_s = sc.LookupType(id);
                if (list_s == null || !list_s.Any()) return;
                var sy = context.GetChild(0) as TerminalNodeImpl;
                var ref_list = new List<CombinedScopeSymbol>();
                foreach (var s in list_s)
                {
                    ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                }
                if (ref_list.Any())
                {
                    _pd.Attributes[context.GetChild(0)] = ref_list;
                    _pd.Attributes[context] = ref_list;
                }
            }
            else if (parent is Java9Parser.UnannClassType_lfno_unannClassOrInterfaceTypeContext)
            {
                var scope = GetScope(NearestScope(context));
                var sc = scope;
                var id = context.GetText();
                IList<ISymbol> list_s = sc.LookupType(id);
                if (list_s == null || !list_s.Any()) return;
                var sy = context.GetChild(0) as TerminalNodeImpl;
                var ref_list = new List<CombinedScopeSymbol>();
                foreach (var s in list_s)
                {
                    ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                }
                if (ref_list.Any())
                {
                    _pd.Attributes[context.GetChild(0)] = ref_list;
                    _pd.Attributes[context] = ref_list;
                }
            }
            else if (parent is Java9Parser.MethodInvocation_lf_primaryContext)
            {
                var scope = GetScope(NearestScope(context));
                var sc = scope;
                var id = context.GetText();
                IList<ISymbol> list_s = sc.LookupType(id);
                if (list_s == null || !list_s.Any()) return;
                var sy = context.GetChild(0) as TerminalNodeImpl;
                var ref_list = new List<CombinedScopeSymbol>();
                foreach (var s in list_s)
                {
                    ref_list.Add((CombinedScopeSymbol)new RefSymbol(sy.Symbol, s));
                }
                if (ref_list.Any())
                {
                    _pd.Attributes[context.GetChild(0)] = ref_list;
                    _pd.Attributes[context] = ref_list;
                }
            }
        }

        public override void ExitPrimary(Java9Parser.PrimaryContext context)
        {
            var last = context.GetChild(context.ChildCount - 1);
            _pd.Attributes.TryGetValue(last, out IList<CombinedScopeSymbol> list_v);
            if (list_v != null && list_v.Any())
            {
                _pd.Attributes[context] = list_v;
            }
        }
    }
}
