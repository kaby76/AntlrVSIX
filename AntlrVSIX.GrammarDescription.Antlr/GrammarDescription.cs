﻿namespace AntlrVSIX.GrammarDescription.Antlr
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
            CommonTokenStream cts = new CommonTokenStream(
                new ANTLRv4Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())));
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
                    if (value != null)
                    {
                        if (value is Symtab.RefSymbol && ((Symtab.RefSymbol)value).Def is Symtab.ClassSymbol)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // terminal = 1
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
                    if (value != null)
                    {
                        if (value is Symtab.RefSymbol && ((Symtab.RefSymbol)value).Def is Symtab.Literal)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
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
                    if (value != null && value is Symtab.RefSymbol && ((Symtab.RefSymbol)value).Def is Symtab.FieldSymbol)
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
                    if (term.Symbol.Type != ANTLRv4Parser.RULE_REF)
                        return false;
                    var text = term.GetText();
                    if (_antlr_keywords.Contains(text)) return false;
                    IRuleNode parent = term.Parent;
                    for (int i = 0; i < parent.ChildCount; ++i)
                    {
                        if (parent.GetChild(i) == term &&
                            i + 1 < parent.ChildCount &&
                            parent.GetChild(i + 1).GetText() == ":")
                            return true;
                    }
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree n) => // terminal
                {
                    TerminalNodeImpl term = n as TerminalNodeImpl;
                    if (term == null) return false;
                    if (term.Symbol.Type != ANTLRv4Parser.TOKEN_REF)
                        return false;
                    var text = term.GetText();
                    if (_antlr_keywords.Contains(text)) return false;
                    IRuleNode parent = term.Parent;
                    for (int i = 0; i < parent.ChildCount; ++i)
                    {
                        if (parent.GetChild(i) == term &&
                            i + 1 < parent.ChildCount &&
                            parent.GetChild(i + 1).GetText() == ":")
                            return true;
                    }
                    if (term.Parent != null
                        && term.Parent is ANTLRv4Parser.IdContext
                        && term.Parent.Parent != null
                        && term.Parent.Parent is ANTLRv4Parser.IdListContext
                        && term.Parent.Parent.Parent != null
                        && term.Parent.Parent.Parent is ANTLRv4Parser.TokensSpecContext)
                        return true;
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
                    if (value != null && value is Symtab.FieldSymbol)
                    {
                        return true;
                    }
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // channel
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_antlr_keywords.Contains(text)) return false;
                    if (!Char.IsUpper(text[0])) return false;
                    IRuleNode parent = term.Parent;
                    while (parent != null)
                    {
                        if (parent as ANTLRv4Parser.LexerRuleSpecContext != null)
                            return false;
                        if (parent as ANTLRv4Parser.ChannelsSpecContext != null)
                            return true;
                        parent = parent.Parent;
                    }
                    return false;
                }
        };

        public bool CanNextRule { get { return true; } }

        public bool DoErrorSquiggles { get { return true; } }

        public bool CanReformat { get { return true; } }

    }
}
