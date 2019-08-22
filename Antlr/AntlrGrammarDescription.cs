namespace AntlrVSIX.Antlr
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using AntlrVSIX.Grammar;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Media;

    class AntlrGrammarDescription : IGrammarDescription
    {
        private AntlrGrammarDescription() { }

        public IParseTree Parse(string ffn, string code)
        {
            IParseTree _ant_tree = null;

            // Set up Antlr to parse input grammar.
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            CommonTokenStream cts = new CommonTokenStream(
                new ANTLRv4Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())));
            var _ant_parser = new ANTLRv4Parser(cts);

            // Set up another token stream containing comments. This might be
            // problematic as the parser influences the lexer.
            CommonTokenStream cts_off_channel = new CommonTokenStream(
                new ANTLRv4Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())),
                ANTLRv4Lexer.OFF_CHANNEL);

            try
            {
                _ant_tree = _ant_parser.grammarSpec();
            }
            catch (Exception e)
            {
                // Parsing error.
            }

            StringBuilder sb = new StringBuilder();
            Class1.ParenthesizedAST(_ant_tree, sb, "", cts);
            string fn = System.IO.Path.GetFileName(ffn);
            fn = "c:\\temp\\" + fn;
            System.IO.File.WriteAllText(fn, sb.ToString());

            return _ant_tree;
        }

        public const string FileExtension = ".g4;.g";

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

        public static AntlrGrammarDescription Instance { get; private set; } = new AntlrGrammarDescription();


        /* Tagging and classification types. */
        private const string ClassificationNameTerminal = "terminal";
        private const string ClassificationNameNonterminal = "nonterminal";
        private const string ClassificationNameComment = "comment";
        private const string ClassificationNameKeyword = "keyword";
        private const string ClassificationNameLiteral = "literal";
        private const string ClassificationNameMode = "mode";
        private const string ClassificationNameChannel = "channel";

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
            Colors.Purple,
            Colors.Orange,
            Colors.Green,
            Colors.Blue,
            Colors.Red,
            Colors.Salmon,
            Colors.Coral,
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

        public List<Func<IGrammarDescription, IParseTree, bool>> Identify { get; } = new List<Func<IGrammarDescription, IParseTree, bool>>()
        {
            (IGrammarDescription gd, IParseTree n) => // nonterminal = 0
                {
                    TerminalNodeImpl term = n as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    // Make sure it's not a def.
                    var is_def = gd.IdentifyDefinition[0](gd, term);
                    if (is_def) return false;
                    if (_antlr_keywords.Contains(text)) return false;
                    if (n.Parent as ANTLRv4Parser.RulerefContext != null &&
                        term?.Symbol.Type == ANTLRv4Parser.RULE_REF)
                        return true;
                    if (n.Parent as ANTLRv4Parser.ActionBlockContext != null)
                        return false;
                    if (n.Parent as ANTLRv4Parser.ParserRuleSpecContext != null &&
                        term?.Symbol.Type == ANTLRv4Parser.RULE_REF)
                        return true;
                    return false;
                },
            (IGrammarDescription gd, IParseTree n) => // terminal = 1
                {
                    TerminalNodeImpl term = n as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (!Char.IsUpper(text[0])) return false;
                    // Make sure it's not a def.
                    var is_def = gd.IdentifyDefinition[1](gd, term);
                    if (is_def) return false;
                       // special case--channels get their own classification.
                    var is_channel = gd.IdentifyDefinition[6](gd, term);
                    if (is_channel) return false;
                    if (gd.Identify[5](gd, term)) return false;
                    if (gd.Identify[6](gd, term)) return false;
                    if (term.Parent as ANTLRv4Parser.TerminalContext != null)
                        return true;
                    if (term.Parent as ANTLRv4Parser.LexerRuleSpecContext != null)
                        return true;
                    if (term.Parent?.Parent as ANTLRv4Parser.LexerCommandExprContext != null)
                        return true;
                    return false;
                },
            null, // comment = 2
            (IGrammarDescription gd, IParseTree t) => // keyword = 3
                {
                    TerminalNodeImpl nonterm = t as TerminalNodeImpl;
                    if (nonterm == null) return false;
                    var text = nonterm.GetText();
                    if (!_antlr_keywords.Contains(text)) return false;
                    return true;
                },
            (IGrammarDescription gd, IParseTree t) => // literal = 4
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
            (IGrammarDescription gd, IParseTree t) => // mode = 5
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_antlr_keywords.Contains(text)) return false;
                    return false;
                },
            (IGrammarDescription gd, IParseTree t) => // channel = 6
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_antlr_keywords.Contains(text)) return false;
                    // The channel def must be part of channelsSpec context.
                    for (var p = term.Parent; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.ChannelsSpecContext) return true;
                    }
                    return false;
                }
        };

        public List<Func<IGrammarDescription, IParseTree, bool>> IdentifyDefinition { get; } = new List<Func<IGrammarDescription, IParseTree, bool>>()
        {
            (IGrammarDescription gd, IParseTree t) => // nonterminal
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
            (IGrammarDescription gd, IParseTree n) => // terminal
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
                    return false;
                },
            null, // comment
            null, // keyword
            null, // literal
            (IGrammarDescription gd, IParseTree n) => // mode
                {
                    TerminalNodeImpl term = n as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_antlr_keywords.Contains(text)) return false;
                    if (!Char.IsUpper(text[0])) return false;
                    IRuleNode parent = term.Parent;
                    while (parent != null)
                    {
                        if (parent as ANTLRv4Parser.LexerRuleSpecContext != null)
                            return false;
                        if (parent as ANTLRv4Parser.ModeSpecContext != null)
                            return true;
                        parent = parent.Parent;
                    }
                    return false;
                },
            (IGrammarDescription gd, IParseTree t) => // channel
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
    }
}
