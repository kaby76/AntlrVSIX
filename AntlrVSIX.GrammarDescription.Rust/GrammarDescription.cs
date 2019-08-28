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

    class GrammarDescription : IGrammarDescription
    {
        public IParseTree Parse(string ffn, string code)
        {
            IParseTree _tree = null;

            // Set up Antlr to parse input grammar.
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            CommonTokenStream cts = new CommonTokenStream(
                new AntlrVSIX.GrammarDescription.Rust.RustLexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())));
            var _parser = new AntlrVSIX.GrammarDescription.Rust.RustParser(cts);


            try
            {
                _tree = _parser.crate();
            }
            catch (Exception e)
            {
                // Parsing error.
            }

            StringBuilder sb = new StringBuilder();
            Foobar.ParenthesizedAST(_tree, sb, "", cts);
            string fn = System.IO.Path.GetFileName(ffn);
            fn = "c:\\temp\\" + fn;
            System.IO.File.WriteAllText(fn, sb.ToString());

            return _tree;
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
        private const string ClassificationNameVariable = "variable";
        private const string ClassificationNameFunction = "function";
        private const string ClassificationNameComment = "comment";
        private const string ClassificationNameKeyword = "keyword";
        private const string ClassificationNameLiteral = "literal";
        private const string ClassificationNameMacro = "macro";

        public string[] Map { get; } = new string[]
        {
            ClassificationNameVariable,
            ClassificationNameFunction,
            ClassificationNameComment,
            ClassificationNameKeyword,
            ClassificationNameLiteral,
            ClassificationNameMacro,
        };

        public Dictionary<string, int> InverseMap { get; } = new Dictionary<string, int>()
        {
            { ClassificationNameVariable, 0 },
            { ClassificationNameFunction, 1 },
            { ClassificationNameComment, 2 },
            { ClassificationNameKeyword, 3 },
            { ClassificationNameLiteral, 4 },
            { ClassificationNameMacro, 5 },
        };

        /* Color scheme for the tagging. */
        public List<System.Windows.Media.Color> MapColor { get; } = new List<System.Windows.Media.Color>()
        {
            Colors.Purple,
            Colors.Orange,
            Colors.Green,
            Colors.Blue,
            Colors.Red,
            Colors.Red,
        };

        public List<System.Windows.Media.Color> MapInvertedColor { get; } = new List<System.Windows.Media.Color>()
        {
            Colors.LightPink,
            Colors.LightYellow,
            Colors.LightGreen,
            Colors.LightBlue,
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
        };

        public List<bool> CanRename { get; } = new List<bool>()
        {
            true, // variable
            true, // method
            false, // comment
            false, // keyword
            false, // literal
            false, // macro
       };

        public List<bool> CanGotodef { get; } = new List<bool>()
        {
            true, // variable
            true, // method
            false, // comment
            false, // keyword
            false, // literal
            false, // macro
       };

        public List<bool> CanGotovisitor { get; } = new List<bool>()
        {
            false, // variable
            false, // method
            false, // comment
            false, // keyword
            false, // literal
            false, // macro
        };

        private static List<string> _keywords = new List<string>()
        {
            "fn",
            "use",
            "let",
            "mut",
        };

        public List<Func<IGrammarDescription, IParseTree, bool>> Identify { get; } = new List<Func<IGrammarDescription, IParseTree, bool>>()
        {
            (IGrammarDescription gd, IParseTree t) => // variable = 0
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    // Make sure it's not a def.
                    var is_def = gd.IdentifyDefinition[0](gd, term);
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
            (IGrammarDescription gd, IParseTree t) => // method = 1
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    var is_def = gd.IdentifyDefinition[1](gd, term);
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
            (IGrammarDescription gd, IParseTree t) => // keyword = 3
                {
                    TerminalNodeImpl nonterm = t as TerminalNodeImpl;
                    if (nonterm == null) return false;
                    var text = nonterm.GetText();
                    if (!_keywords.Contains(text)) return false;
                    return true;
                },
            (IGrammarDescription gd, IParseTree t) => // literal = 4
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    // Chicken/egg problem. Assume that literals are marked
                    // with the appropriate token type.
                    if (term.Symbol == null) return false;
                    return false;
                },
            (IGrammarDescription gd, IParseTree t) => // macro = 5
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
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
                    //if (term.Parent as RustParser.IdentifierContext == null) return false;
                    //if (term.Parent.Parent is RustParser.VariableDeclaratorIdContext) return true;
                    return false;
                },
            (IGrammarDescription gd, IParseTree t) => // method
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
            (IGrammarDescription gd, IParseTree t) => // macro = 5
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