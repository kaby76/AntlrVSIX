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
        public virtual void Cleanup() { }
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
                            if (i == (int)LanguageServer.Antlr4ParserDescription.AntlrClassifications.ClassificationNonterminalRef
                                || i == (int)LanguageServer.Antlr4ParserDescription.AntlrClassifications.ClassificationTerminalRef
                                || i == (int)LanguageServer.Antlr4ParserDescription.AntlrClassifications.ClassificationModeRef
                                || i == (int)LanguageServer.Antlr4ParserDescription.AntlrClassifications.ClassificationChannelRef
                                )
                            {
                                Refs.Add(t, i);
                                PopupList.Add(t, i);
                            }
                        }
                        catch (Exception) { }
                        try
                        {
                            if (i == (int)LanguageServer.Antlr4ParserDescription.AntlrClassifications.ClassificationNonterminalDef
                                || i == (int)LanguageServer.Antlr4ParserDescription.AntlrClassifications.ClassificationTerminalDef
                                || i == (int)LanguageServer.Antlr4ParserDescription.AntlrClassifications.ClassificationModeDef
                                || i == (int)LanguageServer.Antlr4ParserDescription.AntlrClassifications.ClassificationChannelDef
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
                        ColorizedList.Add(t, (int)LanguageServer.Antlr4ParserDescription.AntlrClassifications.ClassificationComment);
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

    }
}
