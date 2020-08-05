namespace LanguageServer
{
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Collections.Generic;
    using System.Linq;

    public class AntlrGrammarDetails : ParsingResults
    {
        private static readonly Dictionary<string, IScope> _scopes = new Dictionary<string, IScope>();
        public static Algorithms.Utils.MultiMap<string, string> _dependent_grammars = new Algorithms.Utils.MultiMap<string, string>();

        public AntlrGrammarDetails(Workspaces.Document item)
            : base(item)
        {
            // Passes executed in order for all files.
            Passes.Add(() =>
            {
                // Gather Imports from grammars.
                // Gather _dependent_grammars map.
                int before_count = 0;
                foreach (KeyValuePair<string, List<string>> x in AntlrGrammarDetails._dependent_grammars)
                {
                    before_count++;
                    before_count = before_count + x.Value.Count;
                }
                if (ParseTree == null) return false;
                ParseTreeWalker.Default.Walk(new Pass0Listener(this), ParseTree);
                int after_count = 0;
                foreach (KeyValuePair<string, List<string>> dep in AntlrGrammarDetails._dependent_grammars)
                {
                    string name = dep.Key;
                    Workspaces.Document x = Workspaces.Workspace.Instance.FindDocument(name);
                    if (x == null)
                    {
                        // Add document.
                        Workspaces.Container proj = Item.Parent;
                        Workspaces.Document new_doc = new Workspaces.Document(name);
                        proj.AddChild(new_doc);
                        after_count++;
                    }
                    after_count++;
                    after_count = after_count + dep.Value.Count;
                }
                return before_count != after_count;
            });
            Passes.Add(() =>
            {
                // For all imported grammars across the entire universe,
                // make sure all are loaded in the workspace,
                // then restart.
                foreach (KeyValuePair<string, List<string>> dep in AntlrGrammarDetails._dependent_grammars)
                {
                    string name = dep.Key;
                    Workspaces.Document x = Workspaces.Workspace.Instance.FindDocument(name);
                    if (x == null)
                    {
                        // Add document.
                        Workspaces.Container proj = Item.Parent;
                        Workspaces.Document new_doc = new Workspaces.Document(name);
                        proj.AddChild(new_doc);
                        return true;
                    }
                    foreach (string y in dep.Value)
                    {
                        Workspaces.Document z = Workspaces.Workspace.Instance.FindDocument(y);
                        if (z == null)
                        {
                            // Add document.
                            Workspaces.Container proj = Item.Parent;
                            Workspaces.Document new_doc = new Workspaces.Document(y);
                            proj.AddChild(new_doc);
                            return true;
                        }
                    }
                }

                // The workspace is completely loaded. Create scopes for all files in workspace
                // if they don't already exist.
                foreach (KeyValuePair<string, List<string>> dep in _dependent_grammars)
                {
                    string name = dep.Key;
                    _scopes.TryGetValue(name, out IScope file_scope);
                    if (file_scope != null)
                    {
                        continue;
                    }

                    _scopes[name] = new FileScope(name, null);
                }

                // Set up search path scopes for Imports relationship.
                IScope root = _scopes[FullFileName];
                foreach (string dep in Imports)
                {
                    // Don't add if already have this search path.
                    IScope dep_scope = _scopes[dep];
                    bool found = false;
                    foreach (IScope scope in root.NestedScopes)
                    {
                        if (scope is SearchPathScope)
                        {
                            SearchPathScope spc = scope as SearchPathScope;
                            if (spc.NestedScopes.First() == dep_scope)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        SearchPathScope import = new SearchPathScope(root);
                        import.nest(dep_scope);
                        root.nest(import);
                    }
                }
                root.empty();
                RootScope = root;
                return false;
            });
            Passes.Add(() =>
            {
                if (ParseTree == null) return false;
                ParseTreeWalker.Default.Walk(new Pass2Listener(this), ParseTree);
                return false;
            });
            Passes.Add(() =>
            {
                if (ParseTree == null) return false;
                ParseTreeWalker.Default.Walk(new Pass3Listener(this), ParseTree);
                return false;
            });
        }

        private void Nuke(IScope scope)
        {
            if (scope == null)
            {
                return;
            }

            foreach (IScope c in scope.NestedScopes)
            {
                Nuke(c);
            }
            IList<ISymbol> copy = scope.Symbols;
            foreach (ISymbol s in copy)
            {
                if (s.file == Item.FullPath)
                {
                    scope.remove(s);
                }
            }
        }

        public override void Cleanup()
        {
            string dir = Item.FullPath;
            dir = System.IO.Path.GetDirectoryName(dir);
            _scopes.TryGetValue(dir, out IScope value);
            if (value == null)
            {
                return;
            }

            Nuke(value);
            foreach (IScope s in value.NestedScopes)
            {
                // Nuke all symbols in this scope.
                Nuke(s);
            }
        }
    }
}
