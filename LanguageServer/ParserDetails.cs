namespace LanguageServer
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ParserDetails : ICloneable
    {
        public virtual Workspaces.Document Item { get; set; }
        public virtual string FullFileName => Item?.FullPath;
        public virtual string Code => Item?.Code;
        public virtual bool Changed => Item == null ? true : Item.Changed;
        public virtual void Cleanup() { }
        public virtual IGrammarDescription Gd { get; set; }

        public virtual Dictionary<TerminalNodeImpl, int> Refs { get; set; } = new Dictionary<TerminalNodeImpl, int>();
        public virtual HashSet<string> PropagateChangesTo { get; set; } = new HashSet<string>();
        public virtual Dictionary<TerminalNodeImpl, int> Defs { get; set; } = new Dictionary<TerminalNodeImpl, int>();
        public virtual Dictionary<TerminalNodeImpl, int> Tags { get; set; } = new Dictionary<TerminalNodeImpl, int>();
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

        public ParserDetails(Workspaces.Document item)
        {
            Item = item;
            Item.Changed = true;
        }


        public virtual void Parse()
        {
            Workspaces.Document item = Item;
            string code = item.Code;
            string ffn = item.FullPath;
            bool has_changed = item.Changed;
            item.Changed = false;
            if (!has_changed)
            {
                return;
            }

            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null)
            {
                throw new Exception();
            }

            gd.Parse(this);

            AllNodes = DFSVisitor.DFS(ParseTree as ParserRuleContext);
            Comments = gd.ExtractComments(code);
            Defs = new Dictionary<TerminalNodeImpl, int>();
            Refs = new Dictionary<TerminalNodeImpl, int>();
            Tags = new Dictionary<TerminalNodeImpl, int>();
            Errors = new HashSet<IParseTree>();
            Imports = new HashSet<string>();
            Attributes = new Dictionary<IParseTree, IList<CombinedScopeSymbol>>();
            Cleanup();
        }

        public virtual List<Func<bool>> Passes { get; } = new List<Func<bool>>();

        public bool Pass(int pass_number)
        {
            return Passes[pass_number]();
        }

        public virtual void GatherDefs()
        {
            Workspaces.Document item = Item;
            string ffn = item.FullPath;
            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null)
            {
                throw new Exception();
            }

            for (int classification = 0; classification < gd.IdentifyDefinition.Count; ++classification)
            {
                Func<IGrammarDescription, Dictionary<IParseTree, IList<CombinedScopeSymbol>>, IParseTree, bool> fun = gd.IdentifyDefinition[classification];
                if (fun == null)
                {
                    continue;
                }

                IEnumerable<IParseTree> it = AllNodes.Where(t => fun(gd, Attributes, t));
                foreach (IParseTree t in it)
                {
                    TerminalNodeImpl x = (t as TerminalNodeImpl);
                    if (x == null)
                    {
                        continue;
                    }

                    if (x.Symbol == null)
                    {
                        continue;
                    }

                    try
                    {
                        Defs.Add(x, classification);
                        Tags.Add(x, classification);
                    }
                    catch (ArgumentException)
                    {
                        // Duplicate
                    }
                }
            }
        }

        public virtual void GatherRefs()
        {
            Workspaces.Document item = Item;
            string ffn = item.FullPath;
            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null)
            {
                throw new Exception();
            }

            for (int classification = 0; classification < gd.Identify.Count; ++classification)
            {
                Func<IGrammarDescription, Dictionary<IParseTree, IList<CombinedScopeSymbol>>, IParseTree, bool> fun = gd.Identify[classification];
                if (fun == null)
                {
                    continue;
                }

                IEnumerable<IParseTree> it = AllNodes.Where(t => fun(gd, Attributes, t));
                foreach (IParseTree t in it)
                {
                    TerminalNodeImpl x = (t as TerminalNodeImpl);
                    if (x == null)
                    {
                        continue;
                    }

                    if (x.Symbol == null)
                    {
                        continue;
                    }

                    try
                    {
                        Attributes.TryGetValue(x, out IList<CombinedScopeSymbol> attr_list);
                        if (attr_list == null)
                        {
                            continue;
                        }

                        foreach (CombinedScopeSymbol attr in attr_list)
                        {
                            Tags.Add(x, classification);
                            if (attr == null)
                            {
                                continue;
                            }

                            ISymbol sym = attr as Symtab.ISymbol;
                            if (sym == null)
                            {
                                continue;
                            }

                            List<ISymbol> defs = sym.resolve();
                            if (defs == null) continue;
                            foreach (var def in defs)
                            {
                                if (def != null && def.file != null && def.file != ""
                                    && def.file != ffn)
                                {
                                    Workspaces.Document def_item = Workspaces.Workspace.Instance.FindDocument(def.file);
                                    ParserDetails def_pd = ParserDetailsFactory.Create(def_item);
                                    def_pd.PropagateChangesTo.Add(ffn);
                                }
                            }
                            Refs.Add(x, classification);
                        }
                    }
                    catch (ArgumentException)
                    {
                        // Duplicate
                    }
                }
            }
        }

        public virtual void GatherErrors()
        {
            Workspaces.Document item = Item;
            string ffn = item.FullPath;
            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
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
            Workspaces.Document item = Item;
            string ffn = item.FullPath;
            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
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

    }
}
