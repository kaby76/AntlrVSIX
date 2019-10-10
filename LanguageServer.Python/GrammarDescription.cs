
namespace LanguageServer.Python
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using LanguageServer;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Media;
    using Symtab;

    class GrammarDescription : IGrammarDescription
    {
        public string Name { get; } = "Python";
        public System.Type Parser { get; } = typeof(Python3Parser);
        public System.Type Lexer { get; } = typeof(Python3Lexer);
        public void Parse(ParserDetails pd)
        {
            var code = pd.Code;
            var ffn = pd.FullFileName;
            IParseTree pt = null;

            // Set up Antlr to parse input grammar.
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            var ais = new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd());
            ais.name = ffn;
            CommonTokenStream cts = new CommonTokenStream(new Python3Lexer(ais));
            var parser = new Python3Parser(cts);

            try
            {
                pt = parser.file_input();
            }
            catch (Exception e)
            {
                // Parsing error.
            }

            //StringBuilder sb = new StringBuilder();
            //Foobar.ParenthesizedAST(_ant_tree, sb, "", cts);
            //string fn = System.IO.Path.GetFileName(ffn);
            //fn = "c:\\temp\\" + fn;
            //System.IO.File.WriteAllText(fn, sb.ToString());

            pd.ParseTree = pt;
        }

        public Dictionary<IToken, int> ExtractComments(string code)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            CommonTokenStream cts_off_channel = new CommonTokenStream(
                new Python3Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())),
                Python3Lexer.OFF_CHANNEL);
            var new_list = new Dictionary<IToken, int>();
            var type = InverseMap[ClassificationNameComment];
            while (cts_off_channel.LA(1) != Python3Lexer.Eof)
            {
                IToken token = cts_off_channel.LT(1);
                if (token.Type == Python3Lexer.LINE_COMMENT)
                {
                    new_list[token] = type;
                }
                cts_off_channel.Consume();
            }
            return new_list;
        }

        public string FileExtension { get; } = ".py";
        public string StartRule { get; } = "file_input";

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

        public ParserDetails CreateParserDetails(Workspaces.Document item)
        {
            throw new NotImplementedException();
        }

        /* Tagging and classification types. */
        private const string ClassificationNameVariable = "Python - variable";
        private const string ClassificationNameMethod = "Python - method";
        private const string ClassificationNameComment = "Python - comment";
        private const string ClassificationNameKeyword = "Python - keyword";
        private const string ClassificationNameLiteral = "Python - literal";

        public string[] Map { get; } = new string[]
        {
            ClassificationNameVariable,
            ClassificationNameMethod,
            ClassificationNameComment,
            ClassificationNameKeyword,
            ClassificationNameLiteral,
        };

        public Dictionary<string, int> InverseMap { get; } = new Dictionary<string, int>()
        {
            { ClassificationNameVariable, 0 },
            { ClassificationNameMethod, 1 },
            { ClassificationNameComment, 2 },
            { ClassificationNameKeyword, 3 },
            { ClassificationNameLiteral, 4 },
        };

        /* Color scheme for the tagging. */
        public List<System.Windows.Media.Color> MapColor { get; } = new List<System.Windows.Media.Color>()
        {
            Colors.Purple,
            Colors.Orange,
            Color.FromRgb(0, 128, 0), //ClassificationNameComment
            Color.FromRgb(0, 0, 255), //ClassificationNameKeyword
            Color.FromRgb(163, 21, 21), //ClassificationNameLiteral
        };

        public List<System.Windows.Media.Color> MapInvertedColor { get; } = new List<System.Windows.Media.Color>()
        {
            Colors.LightPink,
            Colors.LightYellow,
            Colors.LightGreen,
            Colors.LightBlue,
            Colors.Red,
        };

        public List<bool> CanFindAllRefs { get; } = new List<bool>()
        {
            true, // variable
            true, // method
            false, // comment
            false, // keyword
            true, // literal
        };

        public List<bool> CanRename { get; } = new List<bool>()
        {
            true, // variable
            true, // method
            false, // comment
            false, // keyword
            false, // literal
        };

        public List<bool> CanGotodef { get; } = new List<bool>()
        {
            true, // variable
            true, // method
            false, // comment
            false, // keyword
            false, // literal
        };

        public List<bool> CanGotovisitor { get; } = new List<bool>()
        {
            false, // variable
            false, // method
            false, // comment
            false, // keyword
            false, // literal
        };

        private static List<string> _keywords = new List<string>()
        {
            "def",
            "return",
            "raise",
            "from",
            "import",
            "as",
            "global",
            "nonlocal",
            "assert",
            "if",
            "elif",
            "else",
            "while",
            "for",
            "in",
            "try",
            "finally",
            "with",
            "except",
            "lambda",
            "or",
            "and",
            "not",
            "is",
            "None",
            "True",
            "False",
            "class",
            "yield",
            "del",
            "pass",
            "continue",
            "break",
            "async",
            "await",
        };

        public List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>, IParseTree, bool>> Identify { get; } = new List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>, IParseTree, bool>>()
        {
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // variable = 0
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    // Make sure it's not a def.
                    var fun = gd.IdentifyDefinition[0];
                    var is_def = fun != null ? fun(gd, st, term) : false;
                    if (is_def) return false;
                    if (_keywords.Contains(text)) return false;
                    if (term?.Symbol.Type == Python3Parser.NAME) return true;
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // method = 1
                {
                    return false;
                },
            null, // comment = 2
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // keyword = 3
                {
                    TerminalNodeImpl nonterm = t as TerminalNodeImpl;
                    if (nonterm == null) return false;
                    var text = nonterm.GetText();
                    if (!_keywords.Contains(text)) return false;
                    return true;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // literal = 4
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    // Chicken/egg problem. Assume that literals are marked
                    // with the appropriate token type.
                    if (term.Symbol == null) return false;
                    return false;
                },
        };

        public List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>, IParseTree, bool>> IdentifyDefinition { get; } = new List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>, IParseTree, bool>>()
        {
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // variable
                {
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st, IParseTree t) => // method
                {
                    return false;
                },
            null, // comment
            null, // keyword
            null, // literal
        };

        public List<Func<ParserDetails, IParseTree, string>> PopUpDefinition { get; } = new List<Func<ParserDetails, IParseTree, string>>()
        {
            null,
            null,
            null,
            null,
            null,
        };

        public bool CanNextRule { get { return false; } }

        public bool DoErrorSquiggles { get { return false; } }

        public bool CanReformat { get { return true; } }

    }
}
