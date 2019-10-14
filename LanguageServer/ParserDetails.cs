namespace LanguageServer
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ParserDetails : ICloneable
    {
        public virtual Workspaces.Document Item { get; set; }
        public virtual string FullFileName { get { return this.Item?.FullPath; } }
        public virtual string Code { get { return this.Item?.Code; } }
        public virtual bool Changed { get { return Item == null ? true : Item.Changed; } }

        public virtual IGrammarDescription Gd { get; set; }

        public virtual Dictionary<TerminalNodeImpl, int> Refs { get; set; } = new Dictionary<TerminalNodeImpl, int>();
        public virtual HashSet<string> Dependencies { get; set; } = new HashSet<string>();
        public virtual Dictionary<TerminalNodeImpl, int> Defs { get; set; } = new Dictionary<TerminalNodeImpl, int>();
        public virtual Dictionary<TerminalNodeImpl, int> Tags { get; set; } = new Dictionary<TerminalNodeImpl, int>();

        public virtual Dictionary<IToken, int> Comments { get; set; } = new Dictionary<IToken, int>();

        public virtual Dictionary<IParseTree, Symtab.CombinedScopeSymbol> Attributes { get; set; } = new Dictionary<IParseTree, Symtab.CombinedScopeSymbol>();

        public virtual Symtab.Scope RootScope { get; set; }

        public virtual IParseTree ParseTree { get; set; } = null;

        public virtual IEnumerable<IParseTree> AllNodes { get; set; } = null;

        public virtual IParseTreeListener P1Listener { get; set; } = null;

        public virtual IParseTreeListener P2Listener { get; set; } = null;

        public ParserDetails(Workspaces.Document item)
        {
            Item = item;
            Item.Changed = true;
        }


        public virtual void Parse()
        {
            var item = Item;
            var code = item.Code;
            var ffn = item.FullPath;
            bool has_changed = item.Changed;
            item.Changed = false;
            if (!has_changed) return;

            //if (item.GetProperty("BuildAction") == "prjBuildActionNone")
            //    return null;

            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null) throw new Exception();
            gd.Parse(this);

            this.AllNodes = DFSVisitor.DFS(this.ParseTree as ParserRuleContext);
            this.Comments = gd.ExtractComments(code);
            this.Defs = new Dictionary<TerminalNodeImpl, int>();
            this.Refs = new Dictionary<TerminalNodeImpl, int>();
        }

        public void Pass1()
        {
            var pass1 = P1Listener;
            ParseTreeWalker.Default.Walk(pass1, ParseTree);
        }

        public void Pass2()
        {
            var pass1 = P1Listener;
            var pass2 = P2Listener;
            ParseTreeWalker.Default.Walk(pass2, ParseTree);
        }

        public virtual void GatherDefs()
        {
            var item = Item;
            var ffn = item.FullPath;
            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null) throw new Exception();
            for (int classification = 0; classification < gd.IdentifyDefinition.Count; ++classification)
            {
                var fun = gd.IdentifyDefinition[classification];
                if (fun == null) continue;
                var it = this.AllNodes.Where(t => fun(gd, this.Attributes, t));
                foreach (var t in it)
                {
                    var x = (t as TerminalNodeImpl);
                    if (x == null) continue;
                    if (x.Symbol == null) continue;
                    try
                    {
                        this.Defs.Add(x, classification);
                        this.Tags.Add(x, classification);
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
            var item = Item;
            var ffn = item.FullPath;
            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null) throw new Exception();
            for (int classification = 0; classification < gd.Identify.Count; ++classification)
            {
                var fun = gd.Identify[classification];
                if (fun == null) continue;
                var it = this.AllNodes.Where(t => fun(gd, this.Attributes, t));
                foreach (var t in it)
                {
                    var x = (t as TerminalNodeImpl);
                    if (x == null) continue;
                    if (x.Symbol == null) continue;
                    try
                    {
                        var attr = this.Attributes[t];
                        var sym = attr as Symtab.Symbol;
                        var def = sym.resolve();
                        if (def != null && def.file != null && def.file != ""
                            && def.file != ffn)
                        {
                            var def_item = Workspaces.Workspace.Instance.FindDocument(def.file);
                            var def_pd = ParserDetailsFactory.Create(def_item);
                            def_pd.Dependencies.Add(ffn);
                        }
                        this.Refs.Add(x, classification);
                        this.Tags.Add(x, classification);
                    }
                    catch (ArgumentException)
                    {
                        // Duplicate
                    }
                }
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
