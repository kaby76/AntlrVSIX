namespace LanguageServer.Java
{
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Collections.Generic;

    class JavaParserDetails : ParserDetails
    {
        static Dictionary<string, IScope> _scopes = new Dictionary<string, IScope>();
        static Dictionary<string, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>> _attributes = new Dictionary<string, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>>();
        static IScope _global_scope = new SymbolTable().GLOBALS;

        public static Graphs.Utils.MultiMap<string, string> imports = new Graphs.Utils.MultiMap<string, string>();

        public JavaParserDetails(Workspaces.Document item)
            : base(item)
        {
            // Passes executed in order for all files.
            Passes.Add(() =>
            {
                // Gather dependent grammars.
                ParseTreeWalker.Default.Walk(new Pass3Listener(this), ParseTree);
                return false;
            });
            Passes.Add(() =>
            {
                // For all imported grammars, add in these to do if not in the workspace, and restart.
                foreach (KeyValuePair<string, List<string>> dep in imports)
                {
                    var name = dep.Key;
                    var x = Workspaces.Workspace.Instance.FindDocument(name);
                    if (x == null)
                    {
                        // Add document.
                        var proj = this.Item.Parent;
                        var new_doc = new Workspaces.Document(name, name);
                        proj.AddChild(new_doc);
                        return true;
                    }
                    foreach (var y in dep.Value)
                    {
                        var z = Workspaces.Workspace.Instance.FindDocument(y);
                        if (z == null)
                        {
                            // Add document.
                            var proj = this.Item.Parent;
                            var new_doc = new Workspaces.Document(y, y);
                            proj.AddChild(new_doc);
                            return true;
                        }
                    }
                }

                var dir = item.FullPath;
                dir = System.IO.Path.GetDirectoryName(dir);
                _scopes.TryGetValue(dir, out IScope value);
                if (value == null)
                {
                    value = new LocalScope(_global_scope);
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
                return false;
            });
            Passes.Add(() =>
            {
                ParseTreeWalker.Default.Walk(new Pass1Listener(this), ParseTree);
                return false;
            });
            Passes.Add(() =>
            {
                ParseTreeWalker.Default.Walk(new Pass2Listener(this), ParseTree);
                return false;
            });
        }
    }
}
