namespace LanguageServer.Java
{
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Collections.Generic;

    class JavaParserDetails : ParserDetails
    {
        static Dictionary<string, IScope> _scopes = new Dictionary<string, IScope>();
        public static Graphs.Utils.MultiMap<string, string> _dependent_grammars = new Graphs.Utils.MultiMap<string, string>();

        public JavaParserDetails(Workspaces.Document item)
            : base(item)
        {
            // Passes executed in order for all files.
            Passes.Add(() =>
            {
                // Gather Imports from grammars.
                // Gather _dependent_grammars map.
                ParseTreeWalker.Default.Walk(new Pass3Listener(this), ParseTree);
                return false;
            });
            Passes.Add(() =>
            {
                // For all imported grammars across the entire universe,
                // make sure all are loaded in the workspace,
                // then restart.
                foreach (KeyValuePair<string, List<string>> dep in _dependent_grammars)
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

                // The workspace is completely loaded. Create scopes for all files in workspace
                // if they don't already exist.
                foreach (KeyValuePair<string, List<string>> dep in _dependent_grammars)
                {
                    var name = dep.Key;
                    _scopes.TryGetValue(name, out IScope file_scope);
                    if (file_scope != null) continue;
                    _scopes[name] = new FileScope(name, null);
                }

                // Set up search path scopes for Imports relationship.
                var root = _scopes[this.FullFileName];
                foreach (var dep in this.Imports)
                {
                    var import = new SearchPathScope(root);
                    _scopes.TryGetValue(dep, out IScope dep_scope);
                    if (dep_scope == null)
                    {
                        _scopes[dep] = new FileScope(dep, null);
                    }
                    dep_scope = _scopes[dep];
                    import.nest(dep_scope);
                    root.nest(import);
                }
                root.empty();
                this.RootScope = root;
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
