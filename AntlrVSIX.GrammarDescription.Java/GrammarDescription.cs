
namespace AntlrVSIX.GrammarDescription.Java
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using AntlrVSIX.GrammarDescription;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Media;
    using org.antlr.symtab;

    class GrammarDescription : IGrammarDescription
    {
        public void Parse(string ffn, string code, out IParseTree parse_tree, out Dictionary<IParseTree, Symbol> symbols)
        {
            IParseTree pt = null;

            // Set up Antlr to parse input grammar.
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            CommonTokenStream cts = new CommonTokenStream(
                new Java9Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())));
            var parser = new Java9Parser(cts);

            try
            {
                pt = parser.ordinaryCompilation();
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

            parse_tree = pt;
            var st = new MyJava9Listener();
            ParseTreeWalker.Default.Walk(st, parse_tree);
            symbols = st.symbols;
        }

        public Dictionary<IToken, int> ExtractComments(string code)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            CommonTokenStream cts_off_channel = new CommonTokenStream(
                new Java9Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())),
                Lexer.Hidden);
            var new_list = new Dictionary<IToken, int>();
            var type = InverseMap[ClassificationNameComment];
            while (cts_off_channel.LA(1) != Java9Lexer.Eof)
            {
                IToken token = cts_off_channel.LT(1);
                if (token.Type == Java9Lexer.COMMENT
                    || token.Type == Java9Lexer.LINE_COMMENT)
                {
                    new_list[token] = type;
                }
                cts_off_channel.Consume();
            }
            return new_list;
        }

        public const string FileExtension = ".java";

        public bool IsFileType(string ffn)
        {
            if (ffn == null) return false;
            var allowable_suffices = FileExtension.Split(';').ToList();
            var suffix = Path.GetExtension(ffn).ToLower();
            foreach (var s in allowable_suffices)
                if (suffix == s)
                    return true;
            return false;
        }

        /* Tagging and classification types. */
        private const string ClassificationNameVariable = "Java - variable";
        private const string ClassificationNameMethod = "Java - method";
        private const string ClassificationNameComment = "Java - comment";
        private const string ClassificationNameKeyword = "Java - keyword";
        private const string ClassificationNameKeywordControl = "Java - keyword-control";
        private const string ClassificationNameLiteral = "Java - literal";
        private const string ClassificationNameType = "Java - type";
        private const string ClassificationNameClass = "Java - class";

        public string[] Map { get; } = new string[]
        {
            ClassificationNameVariable,
            ClassificationNameMethod,
            ClassificationNameComment,
            ClassificationNameKeyword,
            ClassificationNameKeywordControl,
            ClassificationNameLiteral,
            ClassificationNameType,
            ClassificationNameClass,
        };

        public Dictionary<string, int> InverseMap { get; } = new Dictionary<string, int>()
        {
            { ClassificationNameVariable, 0 },
            { ClassificationNameMethod, 1 },
            { ClassificationNameComment, 2 },
            { ClassificationNameKeyword, 3 },
            { ClassificationNameKeywordControl, 4 },
            { ClassificationNameLiteral, 5 },
            { ClassificationNameType, 6 },
            { ClassificationNameClass, 7 },
        };

        /* Color scheme for the tagging. */
        public List<Color> MapColor { get; } = new List<Color>()
        {
            Colors.Purple, //ClassificationNameVariable
            Colors.Orange, //ClassificationNameMethod
            Color.FromRgb(0, 128, 0), //ClassificationNameComment
            Color.FromRgb(0, 0, 255), //ClassificationNameKeyword
            Color.FromRgb(189, 8, 196), //ClassificationNameKeywordControl
            Color.FromRgb(163, 21, 21), //ClassificationNameLiteral
            Color.FromRgb(43, 145, 175), //ClassificationNameType
            Color.FromRgb(43, 145, 175), //ClassificationNameClass
        };

        public List<Color> MapInvertedColor { get; } = new List<Color>()
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
            false, // keyword-control
            true, // literal
            true, // type
            true, // class
        };

        public List<bool> CanRename { get; } = new List<bool>()
        {
            true, // variable
            true, // method
            false, // comment
            false, // keyword
            false, // keyword-control
            false, // literal
            true, // type
            true, // class
        };

        public List<bool> CanGotodef { get; } = new List<bool>()
        {
            true, // variable
            true, // method
            false, // comment
            false, // keyword
            false, // keyword-control
            false, // literal
            true, // type
            true, // class
        };

        public List<bool> CanGotovisitor { get; } = new List<bool>()
        {
            false, // variable
            false, // method
            false, // comment
            false, // keyword
            false, // keyword-control
            false, // literal
            false, // type
            false, // class
        };

        private static List<string> _keywords = new List<string>()
        {
            "abstract",
            "assert",
            "boolean",
            "byte",
            "char",
            "class",
            "const",
            "double",
            "enum",
            "extends",
            "final",
            "float",
            "implements",
            "import",
            "instanceof",
            "int",
            "interface",
            "long",
            "native",
            "new",
            "package",
            "private",
            "protected",
            "public",
            "short",
            "static",
            "strictfp",
            "super",
            "synchronized",
            "this",
            "transient",
            "void",
            "volatile",
        };
        private static List<string> _keywords_control = new List<string>()
        {
            "break",
            "case",
            "catch",
            "continue",
            "default",
            "do",
            "else",
            "finally",
            "for",
            "if",
            "goto",
            "return",
            "switch",
            "throws",
            "try",
            "while",
        };

        public List<Func<IGrammarDescription, IParseTree, bool>> Identify { get; } = new List<Func<IGrammarDescription, IParseTree, bool>>()
        {
            (IGrammarDescription gd, IParseTree t) => // variable = 0
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    // Make sure it's not a def.
                    var is_def = gd.IdentifyDefinition[0](gd, term);
                    if (is_def) return false;
                    var is_type = gd.Identify[6](gd, term);
                    if (is_type) return false;
                    var text = term.GetText();
                    if (_keywords.Contains(text)) return false;
                    if (_keywords_control.Contains(text)) return false;
                    if (term.Parent as Java9Parser.IdentifierContext == null) return false;
                    if (term.Parent.Parent is Java9Parser.MethodInvocation_lfno_primaryContext) return false;
                    for (var p = term.Parent; p != null; p = p.Parent)
                    {
                        if (p is Java9Parser.ExpressionContext) return true;
                    }
                    return false;
                },
            (IGrammarDescription gd, IParseTree t) => // method = 1
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    var is_def = gd.IdentifyDefinition[1](gd, term);
                    if (is_def) return false;
                    if (_keywords.Contains(text)) return false;
                    if (_keywords_control.Contains(text)) return false;
                    if (term.Parent as Java9Parser.IdentifierContext == null) return false;
                    for (var p = term.Parent; p != null; p = p.Parent)
                    {
                        if (p is Java9Parser.ExpressionContext) return false; // already counted.
                        if (p is Java9Parser.TypeNameContext) return false; // already counted as variable.
                        if (p is Java9Parser.MethodInvocationContext) return true;
                    }
                    return false;
                },
            null, // comment = 2
            (IGrammarDescription gd, IParseTree t) => // keyword = 3
                {
                    TerminalNodeImpl nonterm = t as TerminalNodeImpl;
                    if (nonterm == null) return false;
                    var text = nonterm.GetText();
                    if (!_keywords.Contains(text)) return false;
                    return true;
                },
            (IGrammarDescription gd, IParseTree t) => // keyword-control = 4
                {
                    TerminalNodeImpl nonterm = t as TerminalNodeImpl;
                    if (nonterm == null) return false;
                    var text = nonterm.GetText();
                    if (!_keywords_control.Contains(text)) return false;
                    return true;
                },
            (IGrammarDescription gd, IParseTree t) => // literal = 5
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    if (term.Symbol == null) return false;
                    return false;
                },
            (IGrammarDescription gd, IParseTree t) => // type = 6
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_keywords.Contains(text)) return false;
                    if (_keywords_control.Contains(text)) return false;
                    if (term.Parent as Java9Parser.IdentifierContext == null) return false;
                    if (term.Parent.Parent is Java9Parser.UnannClassType_lfno_unannClassOrInterfaceTypeContext) return true;
                    return false;
                },
        };

        public List<Func<IGrammarDescription, IParseTree, bool>> IdentifyDefinition { get; } = new List<Func<IGrammarDescription, IParseTree, bool>>()
        {
            (IGrammarDescription gd, IParseTree t) => // variable
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_keywords.Contains(text)) return false;
                    if (term.Parent as Java9Parser.IdentifierContext == null) return false;
                    if (term.Parent.Parent is Java9Parser.VariableDeclaratorIdContext) return true;
                    return false;
                },
            (IGrammarDescription gd, IParseTree t) => // method
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_keywords.Contains(text)) return false;
                    if (term.Parent as Java9Parser.IdentifierContext == null) return false;
                    if (term.Parent.Parent is Java9Parser.MethodDeclaratorContext) return true;
                    return false;
                },
            null, // comment
            null, // keyword
            null, // keyword-control
            null, // literal
            null, // type
        };

        public bool CanNextRule { get { return false; } }

        public bool DoErrorSquiggles { get { return false; } }
    }
}
