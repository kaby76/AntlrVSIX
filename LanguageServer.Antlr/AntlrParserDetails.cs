namespace LanguageServer.Antlr
{
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Collections.Generic;

    public class AntlrParserDetails : ParserDetails
    {
        static Dictionary<string, IScope> _scopes = new Dictionary<string, IScope>();
        static Dictionary<string, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>> _attributes = new Dictionary<string, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>>();
        static IScope _global_scope = new SymbolTable().GLOBALS;

        public AntlrParserDetails(Workspaces.Document item)
            : base(item)
        {
            P2Listener = new Pass2Listener(this);
            P1Listener = new Pass1Listener(this);
            var dir = item.FullPath;
            dir = System.IO.Path.GetDirectoryName(dir);
            _scopes.TryGetValue(dir, out IScope value);
            if (value == null)
            {
                value = new LocalScope(_global_scope);
                _global_scope.nest(value);
                _scopes[dir] = value;
            }
            this.RootScope = value;
            _attributes.TryGetValue(dir, out Dictionary<IParseTree, CombinedScopeSymbol> at);
            if (at == null)
            {
                at = new Dictionary<IParseTree, CombinedScopeSymbol>();
                _attributes[dir] = at;
            }
            this.Attributes = at;
        }

        private void Nuke(IScope scope)
        {
            foreach (var c in scope.NestedScopes)
            {
                Nuke(c);
            }
            var copy = scope.Symbols;
            foreach (var s in copy)
            {
                if (s.file == Item.FullPath)
                    scope.remove(s);
            }
        }

        public override void Cleanup()
        {
            var dir = Item.FullPath;
            dir = System.IO.Path.GetDirectoryName(dir);
            _scopes.TryGetValue(dir, out IScope value);
            Nuke(value);
            foreach (var s in value.NestedScopes)
            {
                // Nuke all symbols in this scope.
                Nuke(s);
            }
        }
    }
}
