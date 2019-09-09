namespace AntlrVSIX.GrammarDescription.Rust
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
    using Symtab;

    class GrammarDescription : IGrammarDescription
    {
        public System.Type Parser { get; } = typeof(RustParser);
        public System.Type Lexer { get; } = typeof(RustLexer);
        public void Parse(string ffn, string code, out IParseTree parse_tree, out Dictionary<IParseTree, Symbol> symbols)
        {
            IParseTree pt = null;

            // Set up Antlr to parse input grammar.
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            CommonTokenStream cts = new CommonTokenStream(
                new RustLexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())));
            var parser = new RustParser(cts);


            try
            {
                pt = parser.crate();
            }
            catch (Exception e)
            {
                // Parsing error.
            }

            //StringBuilder sb = new StringBuilder();
            //Foobar.ParenthesizedAST(_tree, sb, "", cts);
            //string fn = System.IO.Path.GetFileName(ffn);
            //fn = "c:\\temp\\" + fn;
            //System.IO.File.WriteAllText(fn, sb.ToString());

            parse_tree = pt;
            symbols = new Dictionary<IParseTree, Symbol>();
        }

        public Dictionary<IToken, int> ExtractComments(string code)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            CommonTokenStream cts_off_channel = new CommonTokenStream(
                new RustLexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())),
                RustLexer.Hidden);
            var new_list = new Dictionary<IToken, int>();
            var type = InverseMap[ClassificationNameComment];
            while (cts_off_channel.LA(1) != RustLexer.Eof)
            {
                IToken token = cts_off_channel.LT(1);
                if (token.Type == RustLexer.LineComment ||
                    token.Type == RustLexer.BlockComment
                    )
                {
                    new_list[token] = type;
                }
                cts_off_channel.Consume();
            }
            return new_list;
        }

        public const string FileExtension = ".rs";

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

        /* Tagging and classification types. */
        private const string ClassificationNameVariable = "Rust - variable";
        private const string ClassificationNameFunction = "Rust - function";
        private const string ClassificationNameComment = "Rust - comment";
        private const string ClassificationNameKeyword = "Rust - keyword";
        private const string ClassificationNameLiteral = "Rust - literal";
        private const string ClassificationNameMacro = "Rust - macro";
        private const string ClassificationNameType = "Rust - type";

        public string[] Map { get; } = new string[]
        {
            ClassificationNameVariable,
            ClassificationNameFunction,
            ClassificationNameComment,
            ClassificationNameKeyword,
            ClassificationNameLiteral,
            ClassificationNameMacro,
            ClassificationNameType,
        };

        public Dictionary<string, int> InverseMap { get; } = new Dictionary<string, int>()
        {
            { ClassificationNameVariable, 0 },
            { ClassificationNameFunction, 1 },
            { ClassificationNameComment, 2 },
            { ClassificationNameKeyword, 3 },
            { ClassificationNameLiteral, 4 },
            { ClassificationNameMacro, 5 },
            { ClassificationNameType, 6 },
        };

        /* Color scheme for the tagging. */
        public List<System.Windows.Media.Color> MapColor { get; } = new List<System.Windows.Media.Color>()
        {
            Colors.Purple,
            Colors.Orange,
            Color.FromRgb(0, 128, 0), //ClassificationNameComment
            Color.FromRgb(0, 0, 255), //ClassificationNameKeyword
            Color.FromRgb(163, 21, 21), //ClassificationNameLiteral
            Colors.Red,
            Color.FromRgb(43, 145, 175), //ClassificationNameType
        };

        public List<System.Windows.Media.Color> MapInvertedColor { get; } = new List<System.Windows.Media.Color>()
        {
            Colors.LightPink,
            Colors.LightYellow,
            Colors.LightGreen,
            Colors.LightBlue,
            Colors.Red,
            Colors.Red,
            Colors.Red,
        };

        public List<bool> CanFindAllRefs { get; } = new List<bool>()
        {
            true, // variable
            true, // method
            false, // comment
            false, // keyword
            true, // literal
            false, // macro
            true, // type
        };

        public List<bool> CanRename { get; } = new List<bool>()
        {
            true, // variable
            true, // method
            false, // comment
            false, // keyword
            false, // literal
            false, // macro
            false, // type
       };

        public List<bool> CanGotodef { get; } = new List<bool>()
        {
            true, // variable
            true, // method
            false, // comment
            false, // keyword
            false, // literal
            false, // macro
            false, // type
       };

        public List<bool> CanGotovisitor { get; } = new List<bool>()
        {
            false, // variable
            false, // method
            false, // comment
            false, // keyword
            false, // literal
            false, // macro
            false, // type
        };

        private static List<string> _keywords = new List<string>()
        {
            "fn",
            "use",
            "let",
            "mut",
            "pub",
            "struct",
            "self",
            "bool",
        };

        public List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.Symbol>, IParseTree, bool>> Identify { get; } = new List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.Symbol>, IParseTree, bool>>()
        {
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.Symbol> st, IParseTree t) => // variable = 0
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    // Make sure it's not a def.
                    var is_def = gd.IdentifyDefinition[0](gd, st, term);
                    if (is_def) return false;
                    if (_keywords.Contains(text)) return false;
                    //if (term.Parent as RustParser.IdentContext == null) return false;
                    //if (term.Parent.Parent is RustParser.MethodInvocation_lfno_primaryContext) return false;
                    //for (var p = term.Parent; p != null; p = p.Parent)
                    //{
                    //    if (p is RustParser.ExpressionContext) return true;
                    //}
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.Symbol> st, IParseTree t) => // method = 1
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    var is_def = gd.IdentifyDefinition[1](gd, st, term);
                    if (is_def) return false;
                    if (_keywords.Contains(text)) return false;
                    //if (term.Parent as RustParser.IdentifierContext == null) return false;
                    //for (var p = term.Parent; p != null; p = p.Parent)
                    //{
                    //    if (p is RustParser.ExpressionContext) return false; // already counted.
                    //    if (p is RustParser.TypeNameContext) return false; // already counted as variable.
                    //    if (p is RustParser.MethodInvocationContext) return true;
                    //}
                    return false;
                },
            null, // comment = 2
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.Symbol> st, IParseTree t) => // keyword = 3
                {
                    TerminalNodeImpl nonterm = t as TerminalNodeImpl;
                    if (nonterm == null) return false;
                    var text = nonterm.GetText();
                    if (!_keywords.Contains(text)) return false;
                    return true;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.Symbol> st, IParseTree t) => // literal = 4
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    // Chicken/egg problem. Assume that literals are marked
                    // with the appropriate token type.
                    if (term.Symbol == null) return false;
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.Symbol> st, IParseTree t) => // macro = 5
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    return false;
                },
        };

        public List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.Symbol>, IParseTree, bool>> IdentifyDefinition { get; } = new List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.Symbol>, IParseTree, bool>>()
        {
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.Symbol> st, IParseTree t) => // variable
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_keywords.Contains(text)) return false;
                    //if (term.Parent as RustParser.IdentifierContext == null) return false;
                    //if (term.Parent.Parent is RustParser.VariableDeclaratorIdContext) return true;
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.Symbol> st, IParseTree t) => // method
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_keywords.Contains(text)) return false;
                    //if (term.Parent as RustParser.IdentifierContext == null) return false;
                    //if (term.Parent.Parent is RustParser.MethodDeclaratorContext) return true;
                    return false;
                },
            null, // comment
            null, // keyword
            null, // literal
            (IGrammarDescription gd, Dictionary<IParseTree, Symtab.Symbol> st, IParseTree t) => // macro = 5
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    return false;
                },
        };

        public bool CanNextRule { get { return false; } }

        public bool DoErrorSquiggles { get { return false; } }
    }
}
