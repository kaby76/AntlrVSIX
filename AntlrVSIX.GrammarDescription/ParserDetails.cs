
namespace AntlrVSIX.GrammarDescription
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using AntlrVSIX.GrammarDescription;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ParserDetails
    {
        public virtual Document Item { get; set; }
        public virtual string FullFileName { get; set; }
        public virtual string Code { get; set; }

        public virtual IGrammarDescription Gd { get; set; }

        public virtual Dictionary<TerminalNodeImpl, int> Refs { get; set; } = new Dictionary<TerminalNodeImpl, int>();
        
        public virtual Dictionary<TerminalNodeImpl, int> Defs { get; set; } = new Dictionary<TerminalNodeImpl, int>();

        public virtual Dictionary<IToken, int> Comments { get; set; } = new Dictionary<IToken, int>();

        public virtual Dictionary<IParseTree, Symtab.CombinedScopeSymbol> Attributes { get; set; } = new Dictionary<IParseTree, Symtab.CombinedScopeSymbol>();

        public virtual Symtab.Scope RootScope { get; set; }

        public virtual IParseTree ParseTree { get; set; } = null;

        public virtual IEnumerable<IParseTree> AllNodes { get; set; } = null;

        public virtual IParseTreeListener P1Listener { get; set; } = null;

        public virtual IParseTreeListener P2Listener { get; set; } = null;

        public virtual void Parse(Document item)
        {
            Item = item;
            var code = item.Code;
            var ffn = item.FullPath;
            ParserDetails pd = ParserDetailsFactory.Create(item);
            bool has_changed = item.Changed;
            item.Changed = false;
            if (!has_changed) return;

            pd.Code = code;
            pd.FullFileName = ffn;

            //if (item.GetProperty("BuildAction") == "prjBuildActionNone")
            //    return null;

            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null) throw new Exception();
            gd.Parse(pd);

            pd.AllNodes = DFSVisitor.DFS(pd.ParseTree as ParserRuleContext);
            pd.Comments = gd.ExtractComments(code);
            pd.Defs = new Dictionary<TerminalNodeImpl, int>();
            pd.Refs = new Dictionary<TerminalNodeImpl, int>();
        }

        public void Pass1(ParserDetails pd)
        {
            var pass1 = pd.P1Listener;
            ParseTreeWalker.Default.Walk(pass1, pd.ParseTree);
        }

        public void Pass2(ParserDetails pd)
        {
            var pass1 = pd.P1Listener;
            var pass2 = pd.P2Listener;
            ParseTreeWalker.Default.Walk(pass2, pd.ParseTree);
        }

        public virtual void GatherDefs(Document item)
        {
            var ffn = item.FullPath;
            ParserDetails pd = ParserDetailsFactory.Create(item);
            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null) throw new Exception();
            for (int classification = 0; classification < gd.IdentifyDefinition.Count; ++classification)
            {
                var fun = gd.IdentifyDefinition[classification];
                if (fun == null) continue;
                var it = pd.AllNodes.Where(t => fun(gd, pd.Attributes, t));
                foreach (var t in it)
                {
                    var x = (t as TerminalNodeImpl);
                    try
                    {
                        pd.Defs.Add(x, classification);
                    }
#pragma warning disable CS0168 // Variable is declared but never used
                    catch (ArgumentException _)
#pragma warning restore CS0168 // Variable is declared but never used
                    {
                        // Duplicate
                    }
                }
            }
        }

        public virtual void GatherRefs(Document item)
        {
            var ffn = item.FullPath;
            ParserDetails pd = ParserDetailsFactory.Create(item);
            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null) throw new Exception();
            for (int classification = 0; classification < gd.Identify.Count; ++classification)
            {
                var fun = gd.Identify[classification];
                if (fun == null) continue;
                var it = pd.AllNodes.Where(t => fun(gd, pd.Attributes, t));
                foreach (var t in it)
                {
                    var x = (t as TerminalNodeImpl);
                    try
                    {
                        pd.Refs.Add(x, classification);
                    }
#pragma warning disable CS0168 // Variable is declared but never used
                    catch (ArgumentException _)
#pragma warning restore CS0168 // Variable is declared but never used
                    {
                        // Duplicate
                    }
                }
            }
        }
    }
}
