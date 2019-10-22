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

        private List<string> _dependent_grammars = new List<string>();

        public AntlrParserDetails(Workspaces.Document item)
            : base(item)
        {
            // Passes executed in order for all files.
            Passes.Add(() =>
            {
                // Gather dependent grammars.
                ParseTreeWalker.Default.Walk(new Pass3Listener(this), ParseTree);
            });
            Passes.Add(() =>
            {
                // Set up symbol table.
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
            });
            Passes.Add(() =>
            {
                ParseTreeWalker.Default.Walk(new Pass1Listener(this), ParseTree);
            });
            Passes.Add(() =>
            {
                ParseTreeWalker.Default.Walk(new Pass2Listener(this), ParseTree);
            });
        }

        private void Nuke(IScope scope)
        {
            if (scope == null) return;
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
            if (value == null) return;
            Nuke(value);
            foreach (var s in value.NestedScopes)
            {
                // Nuke all symbols in this scope.
                Nuke(s);
            }
        }
    }
}
