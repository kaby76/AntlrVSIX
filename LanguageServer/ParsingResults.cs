namespace LanguageServer
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ParsingResults : ICloneable
    {
        public virtual Workspaces.Document Item { get; set; }
        public virtual string FullFileName => Item?.FullPath;
        public virtual string Code => Item?.Code;
        public virtual bool Changed => Item == null ? true : Item.Changed;
        public virtual void Cleanup() {
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
        public virtual IParserDescription Gd { get; set; }
        public virtual int QuietAfter { get; set; }

        public virtual Dictionary<TerminalNodeImpl, int> Refs { get; set; } = new Dictionary<TerminalNodeImpl, int>();
        public virtual HashSet<string> PropagateChangesTo { get; set; } = new HashSet<string>();
        public virtual Dictionary<TerminalNodeImpl, int> Defs { get; set; } = new Dictionary<TerminalNodeImpl, int>();
        public virtual Dictionary<TerminalNodeImpl, int> PopupList { get; set; } = new Dictionary<TerminalNodeImpl, int>();
        public virtual Dictionary<IToken, int> ColorizedList { get; set; } = new Dictionary<IToken, int>();
        public virtual HashSet<string> Imports { get; set; } = new HashSet<string>();

        public virtual HashSet<IParseTree> Errors { get; set; } = new HashSet<IParseTree>();

        public virtual Dictionary<IToken, int> Comments { get; set; } = new Dictionary<IToken, int>();

        public virtual Dictionary<IParseTree, IList<CombinedScopeSymbol>> Attributes { get; set; } = new Dictionary<IParseTree, IList<CombinedScopeSymbol>>();

        public virtual IScope RootScope { get; set; }

        public virtual IParseTree ParseTree { get; set; } = null;

        public virtual IEnumerable<IParseTree> AllNodes { get; set; } = null;
        public virtual Antlr4.Runtime.Parser Parser { get; set; } = null;
        public virtual Antlr4.Runtime.Lexer Lexer { get; set; } = null;
        public virtual CommonTokenStream TokStream { get; set; } = null;

        public ParsingResults(Workspaces.Document item)
        {
            Item = item;
            Item.Changed = true;
            // Passes executed in order for all files.
            Passes.Add(() =>
            {
                // Gather Imports from grammars.
                // Gather _dependent_grammars map.
                int before_count = 0;
                foreach (KeyValuePair<string, List<string>> x in ParsingResults._dependent_grammars)
                {
                    before_count++;
                    before_count = before_count + x.Value.Count;
                }
                if (ParseTree == null) return false;
                ParseTreeWalker.Default.Walk(new Pass0Listener(this), ParseTree);
                int after_count = 0;
                foreach (KeyValuePair<string, List<string>> dep in ParsingResults._dependent_grammars)
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
                foreach (KeyValuePair<string, List<string>> dep in ParsingResults._dependent_grammars)
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


        public virtual void Parse()
        {
            Workspaces.Document document = Item;
            string code = document.Code;
            string ffn = document.FullPath;
            bool has_changed = document.Changed;
            document.Changed = false;
            if (!has_changed)
            {
                return;
            }

            IParserDescription gd = ParserDescriptionFactory.Create(document);
            if (gd == null)
            {
                throw new Exception();
            }

            gd.Parse(this);

            AllNodes = DFSVisitor.DFS(ParseTree as ParserRuleContext);
            Comments = gd.ExtractComments(code);
            Defs = new Dictionary<TerminalNodeImpl, int>();
            Refs = new Dictionary<TerminalNodeImpl, int>();
            PopupList = new Dictionary<TerminalNodeImpl, int>();
            Errors = new HashSet<IParseTree>();
            Imports = new HashSet<string>();
            Attributes = new Dictionary<IParseTree, IList<CombinedScopeSymbol>>();
            ColorizedList = new Dictionary<Antlr4.Runtime.IToken, int>();
            Cleanup();
        }

        public virtual List<Func<bool>> Passes { get; } = new List<Func<bool>>();

        public bool Pass(int pass_number)
        {
            return Passes[pass_number]();
        }

        public virtual void GatherRefsDefsAndOthers()
        {
            try
            {
                Workspaces.Document document = Item;
                string ffn = document.FullPath;
                IParserDescription gd = ParserDescriptionFactory.Create(document);
                if (gd == null)
                {
                    throw new Exception();
                }

                if (AllNodes != null)
                {
                    Func<IParserDescription, Dictionary<IParseTree, IList<CombinedScopeSymbol>>, IParseTree, int> fun = gd.Classify;
                    IEnumerable<IParseTree> it = AllNodes.Where(n => n is TerminalNodeImpl);
                    foreach (var n in it)
                    {
                        var t = n as TerminalNodeImpl;
                        int i = -1;
                        try
                        {
                            i = gd.Classify(gd, Attributes, t);
                            if (i >= 0)
                            {
                                ColorizedList.Add(t.Symbol, i);
                            }
                        }
                        catch (Exception) { }
                        try
                        {
                            if (i == (int)LanguageServer.Antlr4ParsingResults.AntlrClassifications.ClassificationNonterminalRef
                                || i == (int)LanguageServer.Antlr4ParsingResults.AntlrClassifications.ClassificationTerminalRef
                                || i == (int)LanguageServer.Antlr4ParsingResults.AntlrClassifications.ClassificationModeRef
                                || i == (int)LanguageServer.Antlr4ParsingResults.AntlrClassifications.ClassificationChannelRef
                                )
                            {
                                Refs.Add(t, i);
                                PopupList.Add(t, i);
                            }
                        }
                        catch (Exception) { }
                        try
                        {
                            if (i == (int)LanguageServer.Antlr4ParsingResults.AntlrClassifications.ClassificationNonterminalDef
                                || i == (int)LanguageServer.Antlr4ParsingResults.AntlrClassifications.ClassificationTerminalDef
                                || i == (int)LanguageServer.Antlr4ParsingResults.AntlrClassifications.ClassificationModeDef
                                || i == (int)LanguageServer.Antlr4ParsingResults.AntlrClassifications.ClassificationChannelDef
                                )
                            {
                                Defs.Add(t, i);
                                PopupList.Add(t, i);
                            }
                        }
                        catch (Exception) { }
                    }
                }

                if (Comments != null)
                {
                    foreach (KeyValuePair<Antlr4.Runtime.IToken, int> p in Comments)
                    {
                        IToken t = p.Key;
                        ColorizedList.Add(t, (int)LanguageServer.Antlr4ParsingResults.AntlrClassifications.ClassificationComment);
                    }
                }
            }
#pragma warning disable 0168
            catch (Exception eeks) { }
#pragma warning restore 0168
        }

        public virtual void GatherErrors()
        {
            Workspaces.Document document = Item;
            string ffn = document.FullPath;
            IParserDescription gd = ParserDescriptionFactory.Create(document);
            if (gd == null)
            {
                throw new Exception();
            }

            {
                IEnumerable<IParseTree> it = AllNodes.Where(t => t as Antlr4.Runtime.Tree.ErrorNodeImpl != null);
                foreach (IParseTree t in it)
                {
                    Errors.Add(t);
                }
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public virtual List<string> Candidates(int char_index)
        {
            Workspaces.Document document = Item;
            string ffn = document.FullPath;
            IParserDescription gd = ParserDescriptionFactory.Create(document);
            if (gd == null)
            {
                throw new Exception();
            }

            string code = Code.Substring(0, char_index);
            gd.Parse(code, out CommonTokenStream tok_stream, out Parser parser, out Lexer lexer, out IParseTree pt);
            LASets la_sets = new LASets();
            IntervalSet int_set = la_sets.Compute(parser, tok_stream);
            List<string> result = new List<string>();
            foreach (int r in int_set.ToList())
            {
                string rule_name = Lexer.RuleNames[r];
                result.Add(rule_name);
            }
            return result;
        }

        private static readonly Dictionary<string, IScope> _scopes = new Dictionary<string, IScope>();
        public static Algorithms.Utils.MultiMap<string, string> _dependent_grammars = new Algorithms.Utils.MultiMap<string, string>();

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
    }
}
