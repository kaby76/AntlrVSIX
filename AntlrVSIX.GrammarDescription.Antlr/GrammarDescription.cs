namespace AntlrVSIX.GrammarDescription.Antlr
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using AntlrVSIX.GrammarDescription;
    using Symtab;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Media;
    using EnvDTE;
    using Microsoft.VisualStudio;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class GrammarDescription : IGrammarDescription
    {
        IParseTree _parse_tree;

        public string Name { get; } = "Antlr";
        public System.Type Parser { get; } = typeof(ANTLRv4Parser);
        public System.Type Lexer { get; } = typeof(ANTLRv4Lexer);

        public void Parse(string ffn, string code, out IParseTree parse_tree, out Dictionary<IParseTree, CombinedScopeSymbol> symbols)
        {
            IParseTree pt = null;

            // Set up Antlr to parse input grammar.
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            var ais = new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd());
            ais.name = ffn;
            CommonTokenStream cts = new CommonTokenStream(new ANTLRv4Lexer(ais));
            var parser = new ANTLRv4Parser(cts);

            try
            {
                pt = parser.grammarSpec();
            }
            catch (Exception e)
            {
                // Parsing error.
            }

            //StringBuilder sb = new StringBuilder();
            //TreeSerializer.ParenthesizedAST(pt, sb, "", cts);
            //string fn = System.IO.Path.GetFileName(ffn);
            //fn = "c:\\temp\\" + fn;
            //System.IO.File.WriteAllText(fn, sb.ToString());

            parse_tree = pt;
            var pass1 = new Pass1Listener();
            ParseTreeWalker.Default.Walk(pass1, parse_tree);
            var pass2 = new Pass2Listener(pass1._attributes, pass1._root);
            ParseTreeWalker.Default.Walk(pass2, parse_tree);
            symbols = pass2._attributes;
        }

        public Dictionary<IToken, int> ExtractComments(string code)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            CommonTokenStream cts_off_channel = new CommonTokenStream(
                new ANTLRv4Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())),
                ANTLRv4Lexer.OFF_CHANNEL);
            var new_list = new Dictionary<IToken, int>();
            var type = InverseMap[ClassificationNameComment];
            while (cts_off_channel.LA(1) != ANTLRv4Parser.Eof)
            {
                IToken token = cts_off_channel.LT(1);
                if (token.Type == ANTLRv4Lexer.BLOCK_COMMENT
                    || token.Type == ANTLRv4Lexer.LINE_COMMENT
                    || token.Type == ANTLRv4Lexer.DOC_COMMENT)
                {
                    new_list[token] = type;
                }
                cts_off_channel.Consume();
            }
            return new_list;
        }

        public string FileExtension { get; } = ".g4;.g";
        public string StartRule { get; } = "grammarSpec";

        public bool IsFileType(string ffn)
        {
            if (ffn == null) return false;
            var allowable_suffices = FileExtension.Split(';').ToList<string>();
            var suffix = Path.GetExtension(ffn).ToLower();
            foreach (var s in allowable_suffices)
                if (suffix == s)
                    return true;
            return false;
        }

        public Dictionary<IParseTree, Symbol> GetSymbolTable()
        {
            return new Dictionary<IParseTree, Symbol>();
        }


        /* Tagging and classification types. */
        private const string ClassificationNameTerminal = "Antlr - terminal";
        private const string ClassificationNameNonterminal = "Antlr - nonterminal";
        private const string ClassificationNameComment = "Antlr - comment";
        private const string ClassificationNameKeyword = "Antlr - keyword";
        private const string ClassificationNameLiteral = "Antlr - literal";
        private const string ClassificationNameMode = "Antlr - mode";
        private const string ClassificationNameChannel = "Antlr - channel";

        public string[] Map { get; } = new string[]
        {
            ClassificationNameNonterminal,
            ClassificationNameTerminal,
            ClassificationNameComment,
            ClassificationNameKeyword,
            ClassificationNameLiteral,
            ClassificationNameMode,
            ClassificationNameChannel,
        };

        public Dictionary<string, int> InverseMap { get; } = new Dictionary<string, int>()
        {
            { ClassificationNameNonterminal, 0 },
            { ClassificationNameTerminal, 1 },
            { ClassificationNameComment, 2 },
            { ClassificationNameKeyword, 3 },
            { ClassificationNameLiteral, 4 },
            { ClassificationNameMode, 5 },
            { ClassificationNameChannel, 6 },
        };

        /* Color scheme for the tagging. */
        public List<System.Windows.Media.Color> MapColor { get; } = new List<System.Windows.Media.Color>()
        {
            Color.FromRgb(43, 145, 175), //ClassificationNameNonterminal
            Colors.Purple, //ClassificationNameTerminal
            Color.FromRgb(0, 128, 0), //ClassificationNameComment
            Color.FromRgb(0, 0, 255), //ClassificationNameKeyword
            Color.FromRgb(163, 21, 21), //ClassificationNameLiteral
            Colors.Salmon, //ClassificationNameMode
            Colors.Coral, //ClassificationNameChannel
        };

        public List<System.Windows.Media.Color> MapInvertedColor { get; } = new List<System.Windows.Media.Color>()
        {
            Colors.LightPink,
            Colors.LightYellow,
            Colors.LightGreen,
            Colors.LightBlue,
            Colors.Red,
            Colors.LightSalmon,
            Colors.LightCoral,
        };

        public List<bool> CanFindAllRefs { get; } = new List<bool>()
        {
            true, // nonterminal
            true, // Terminal
            false, // comment
            false, // keyword
            true, // literal
            true, // mode
            true, // channel
        };

        public List<bool> CanRename { get; } = new List<bool>()
        {
            true, // nonterminal
            true, // Terminal
            false, // comment
            false, // keyword
            false, // literal
            true, // mode
            true, // channel
        };

        public List<bool> CanGotodef { get; } = new List<bool>()
        {
            true, // nonterminal
            true, // Terminal
            false, // comment
            false, // keyword
            false, // literal
            true, // mode
            true, // channel
        };

        public List<bool> CanGotovisitor { get; } = new List<bool>()
        {
            true, // nonterminal
            false, // Terminal
            false, // comment
            false, // keyword
            false, // literal
            false, // mode
            false, // channel
        };

        private static List<string> _antlr_keywords = new List<string>()
        {
            "options",
            "tokens",
            "channels",
            "import",
            "fragment",
            "lexer",
            "parser",
            "grammar",
            "protected",
            "public",
            "returns",
            "locals",
            "throws",
            "catch",
            "finally",
            "mode",
            "pushMode",
            "popMode",
            "type",
            "skip",
            "channel"
        };

        public List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>, IParseTree, bool>> Identify { get; } = new List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>, IParseTree, bool>>()
        {
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // nonterminal = 0
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
                    if (value != null && value is Symtab.RefSymbol && ((Symtab.RefSymbol)value).Def is Symtab.NonterminalSymbol)
                    {
                        return true;
                    }
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // terminal = 1
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
                    if (value != null && value is Symtab.RefSymbol && ((Symtab.RefSymbol)value).Def is Symtab.TerminalSymbol)
                    {
                        return true;
                    }
                    return false;
                },
            null, // comment = 2
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // keyword = 3
                {
                    TerminalNodeImpl nonterm = t as TerminalNodeImpl;
                    if (nonterm == null) return false;
                    var text = nonterm.GetText();
                    if (!_antlr_keywords.Contains(text)) return false;
                    return true;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // literal = 4
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    // Chicken/egg problem. Assume that literals are marked
                    // with the appropriate token type.
                    if (term.Symbol == null) return false;
                    if (!(term.Symbol.Type == ANTLRv4Parser.STRING_LITERAL
                        || term.Symbol.Type == ANTLRv4Parser.INT
                        || term.Symbol.Type == ANTLRv4Parser.LEXER_CHAR_SET))
                        return false;

                    // The token must be part of parserRuleSpec context.
                    for (var p = term.Parent; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.ParserRuleSpecContext ||
                            p is ANTLRv4Parser.LexerRuleSpecContext) return true;
                    }
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // mode = 5
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
                    if (value != null && value is Symtab.RefSymbol && ((Symtab.RefSymbol)value).Def is Symtab.ModeSymbol)
                    {
                        return true;
                    }
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // channel = 6
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_antlr_keywords.Contains(text)) return false;
                    if (! (term.Parent is ANTLRv4Parser.IdContext))
                        return false;
                    if (! (term.Parent.Parent is ANTLRv4Parser.LexerCommandExprContext))
                        return false;
                    if (! (term.Parent.Parent.Parent is ANTLRv4Parser.LexerCommandContext))
                        return false;
                    var p = term.Parent.Parent.Parent;
                    var id = p.GetChild(0).GetText();
                    if (id != "channel") return false;
                    return true;
                }
        };

        public List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>, IParseTree, bool>> IdentifyDefinition { get; } = new List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>, IParseTree, bool>>()
        {
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // nonterminal
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
                    if (value != null && value is Symtab.NonterminalSymbol)
                    {
                        return true;
                    }
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // terminal
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
                    if (value != null && value is Symtab.TerminalSymbol)
                    {
                        return true;
                    }
                    return false;
                },
            null, // comment
            null, // keyword
            null, // literal
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // mode
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
                    if (value != null && value is Symtab.ModeSymbol)
                    {
                        return true;
                    }
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // channel
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
                    if (value != null && value is Symtab.ChannelSymbol)
                    {
                        return true;
                    }
                    return false;
                }
        };

        public bool CanNextRule { get { return true; } }

        public bool DoErrorSquiggles { get { return true; } }

        public bool CanReformat { get { return true; } }

        public List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>, IParseTree, string>> PopUpDefinition { get; } = new List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>, IParseTree, string>>()
        {
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // nonterminal
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return null;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
                    if (value == null) return null;
                    var sym = value as Symbol;
                    if (sym == null) return null;
                    if (sym is Symtab.RefSymbol)
                    {
                        sym = sym.resolve();
                    }
                    StringBuilder sb = new StringBuilder();
                    if (sym is Symtab.TerminalSymbol)
                        sb.Append("Terminal ");
                    else if (sym is Symtab.NonterminalSymbol)
                        sb.Append("Nonterminal ");
                    else return null;
                    var fod = st.Where(kvp => kvp.Value == sym).Select(kvp => kvp.Key).FirstOrDefault();
                    if (fod == null) return sb.ToString();
                    sb.Append("defined in ");
                    sb.Append(sym.file);
                    sb.AppendLine();
                    var node = fod;
                    for (; node != null; node = node.Parent) if (node is ANTLRv4Parser.RuleSpecContext) break;
                    if (node == null) return null;
                    Reconstruct.Doit(sb, node);
                    return sb.ToString();
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // terminal
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return null;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
                    if (value == null) return null;
                    var sym = value as Symbol;
                    if (sym == null) return null;
                    if (sym is Symtab.RefSymbol)
                    {
                        sym = sym.resolve();
                    }
                    StringBuilder sb = new StringBuilder();
                    if (sym is Symtab.TerminalSymbol)
                        sb.Append("Terminal ");
                    else if (sym is Symtab.NonterminalSymbol)
                        sb.Append("Nonterminal ");
                    else return null;
                    var fod = st.Where(kvp => kvp.Value == sym).Select(kvp => kvp.Key).FirstOrDefault();
                    if (fod == null) return sb.ToString();
                    sb.Append("defined in ");
                    sb.Append(sym.file);
                    sb.AppendLine();
                    var node = fod;
                    for (; node != null; node = node.Parent) if (node is ANTLRv4Parser.RuleSpecContext) break;
                    if (node == null) return null;
                    Reconstruct.Doit(sb, node);
                    return sb.ToString();
                },
            null, // comment
            null, // keyword
            null, // literal
            null,
            null,
        };

        public void ProcessFile(ProjectItem item)
        {
            var r = item.ProjectItems;
            var c = r != null ? r.Count : 0;
            bool do_not_parse = false;
            try
            {
                if (item?.Properties == null) return;
                object prop = item?.Properties?.Item("FullPath")?.Value;
                string ffn = (string)prop;
                bool has_build_action = false;

                try
                {
                    object p2 = item?.Properties?.Item("BuildAction")?.Value;
                    has_build_action = true;
                }
                catch (Exception _) { }
                try
                {
                    object p3 = item?.Properties?.Item("CustomToolNamespace")?.Value;
                }
                catch (Exception _) { }
                try
                {
                    object p4 = item?.Properties?.Item("CustomTool")?.Value;
                }
                catch (Exception _) { }
                if (!ParserDetails._per_file_parser_details.ContainsKey(ffn))
                {
                    StreamReader sr = new StreamReader(ffn);
                    var xx = AntlrVSIX.GrammarDescription.Workspace.Instance.FindProjectFullName(ffn);
                    xx.Code = sr.ReadToEnd();
                    var pd = ParserDetails.Parse(xx);
                }
            }
            catch (Exception eeks)
            {
            }
        }

    }
}
