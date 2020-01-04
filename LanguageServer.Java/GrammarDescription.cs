
namespace LanguageServer.Java
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using LanguageServer;
    using Symtab;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    class GrammarDescription : IGrammarDescription
    {
        public string Name { get; } = "Java";
        public System.Type Parser { get; } = typeof(Java9Parser);
        public System.Type Lexer { get; } = typeof(Java9Lexer);
        public ParserDetails CreateParserDetails(Workspaces.Document item)
        {
            return new JavaParserDetails(item);
        }

        public void Parse(ParserDetails pd)
        {
            string ffn = pd.FullFileName;
            string code = pd.Code;
            IParseTree pt = null;

            // Set up Antlr to parse input grammar.
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            var ais = new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd());
            ais.name = ffn;
            CommonTokenStream cts = new CommonTokenStream(new Java9Lexer(ais));
            var parser = new Java9Parser(cts);

            try
            {
                pt = parser.compilationUnit();
            }
            catch (Exception)
            {
                // Parsing error.
            }

            //StringBuilder sb = new StringBuilder();
            //TreeSerializer.ParenthesizedAST(pt, sb, "", cts);
            //string fn = System.IO.Path.GetFileName(ffn);
            //fn = "c:\\temp\\" + fn;
            //System.IO.File.WriteAllText(fn, sb.ToString());

            pd.ParseTree = pt;
        }

        public Dictionary<IToken, int> ExtractComments(string code)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            CommonTokenStream cts_off_channel = new CommonTokenStream(
                new Java9Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())),
                Java9Lexer.COMMENTS_CHANNEL);
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

        public string FileExtension { get; } = ".java";
        public string StartRule { get; } = "compilationUnit";

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

        public void Parse(string code, out CommonTokenStream TokStream, out Parser Parser, out Lexer Lexer, out IParseTree ParseTree)
        {
            throw new NotImplementedException();
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
        private const string ClassificationNameField = "Java - field";

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
            ClassificationNameField,
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
            { ClassificationNameField, 8 },
        };

        /* Color scheme for the tagging. */
        public List<System.Drawing.Color> MapColor { get; } = new List<System.Drawing.Color>()
        {
            System.Drawing.Color.Purple, //ClassificationNameVariable
            System.Drawing.Color.Orange, //ClassificationNameMethod
            System.Drawing.Color.FromArgb(0, 128, 0), //ClassificationNameComment
            System.Drawing.Color.FromArgb(0, 0, 255), //ClassificationNameKeyword
            System.Drawing.Color.FromArgb(189, 8, 196), //ClassificationNameKeywordControl
            System.Drawing.Color.FromArgb(163, 21, 21), //ClassificationNameLiteral
            System.Drawing.Color.FromArgb(43, 145, 175), //ClassificationNameType
            System.Drawing.Color.FromArgb(43, 145, 175), //ClassificationNameClass
            System.Drawing.Color.FromArgb(43, 145, 175), //ClassificationNameField
        };

        public List<System.Drawing.Color> MapInvertedColor { get; } = new List<System.Drawing.Color>()
        {
            System.Drawing.Color.LightPink,
            System.Drawing.Color.LightYellow,
            System.Drawing.Color.LightGreen,
            System.Drawing.Color.LightBlue,
            System.Drawing.Color.Red,
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
            true, // field
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
            true, // field
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
            true, // field
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
            false, // field
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

        public List<Func<IGrammarDescription, Dictionary<IParseTree, IList<CombinedScopeSymbol>>, IParseTree, bool>> Identify { get; } = new List<Func<IGrammarDescription, Dictionary<IParseTree, IList<CombinedScopeSymbol>>, IParseTree, bool>>()
        {
            (IGrammarDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) => // variable = 0
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                    if (list_value == null) return false;
                    foreach (var value in list_value)
                    {
                        if (value != null && value is RefSymbol && ((RefSymbol)value).Def is VariableSymbol)
                        {
                            return true;
                        }
                    }
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) => // method = 1
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                    if (list_value == null) return false;
                    foreach (var value in list_value)
                    {
                        if (value != null && value is RefSymbol && ((RefSymbol)value).Def is MethodSymbol)
                        {
                            return true;
                        }
                    }
                    return false;
                },
            null, // comment = 2
            (IGrammarDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) => // keyword = 3
                {
                    TerminalNodeImpl nonterm = t as TerminalNodeImpl;
                    if (nonterm == null) return false;
                    var text = nonterm.GetText();
                    if (!_keywords.Contains(text)) return false;
                    return true;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) => // keyword-control = 4
                {
                    TerminalNodeImpl nonterm = t as TerminalNodeImpl;
                    if (nonterm == null) return false;
                    var text = nonterm.GetText();
                    if (!_keywords_control.Contains(text)) return false;
                    return true;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) => // literal = 5
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                    if (list_value == null) return false;
                    foreach (var value in list_value)
                    {
                        if (value != null && value is RefSymbol && ((RefSymbol)value).Def is Literal)
                        {
                            return true;
                        }
                    }
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) => // type = 6
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                    if (list_value == null) return false;
                    foreach (var value in list_value)
                    {
                        if (value != null && value is RefSymbol && ((RefSymbol)value).Def is ClassSymbol)
                        {
                            return true;
                        }
                    }
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) => // class
                {
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) => // field
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                    if (list_value == null) return false;
                    foreach (var value in list_value)
                    {
                        if (value != null && value is RefSymbol && ((RefSymbol)value).Def is FieldSymbol)
                        {
                            return true;
                        }
                    }
                    return false;
                },
        };

        public List<Func<IGrammarDescription, Dictionary<IParseTree, IList<CombinedScopeSymbol>>, IParseTree, bool>> IdentifyDefinition { get; } = new List<Func<IGrammarDescription, Dictionary<IParseTree, IList<CombinedScopeSymbol>>, IParseTree, bool>>()
        {
            (IGrammarDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) => // variable
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                    if (list_value == null) return false;
                    foreach (var value in list_value)
                    {
                        if (value != null)
                        {
                            switch (value)
                            {
                                case LocalSymbol ss:
                                    return true;
                                case ParameterSymbol ss:
                                    return true;
                                default:
                                    return false;
                            }
                        }
                    }
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) => // method
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                    if (list_value == null) return false;
                    foreach (var value in list_value)
                    {
                        if (value != null && value is MethodSymbol)
                        {
                            return true;
                        }
                    }
                    return false;
                },
            null, // comment
            null, // keyword
            null, // keyword-control
            null, // literal
            null, // type
            (IGrammarDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) => // class
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                    if (list_value == null) return false;
                    foreach (var value in list_value)
                    {
                        if (value != null && value is ClassSymbol)
                        {
                            return true;
                        }
                    }
                    return false;
                },
            (IGrammarDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) => // field
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    st.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                    if (list_value == null) return false;
                    foreach (var value in list_value)
                    {
                        if (value != null && value is FieldSymbol)
                        {
                            return true;
                        }
                    }
                    return false;
                },
        };

        public List<Func<ParserDetails, IParseTree, string>> PopUpDefinition { get; } = new List<Func<ParserDetails, IParseTree, string>>()
        {
            (ParserDetails pd, IParseTree t) => // variable
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return null;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    var dir = System.IO.Path.GetDirectoryName(pd.Item.FullPath);
                    pd.Attributes.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                    if (list_value == null) return null;
                    StringBuilder sb = new StringBuilder();
                    foreach (var value in list_value)
                    {
                        if (value == null) continue;
                        var sym = value as ISymbol;
                        if (sym == null) continue;
                        if (sym is RefSymbol)
                        {
                            sym = sym.resolve();
                        }
                        if (sym is VariableSymbol)
                            sb.Append("Variable ");
                        else continue;
                        var def_file = sym.file;
                        if (def_file == null) continue;
                        var def_document = Workspaces.Workspace.Instance.FindDocument(def_file);
                        if (def_document == null) continue;
                        var def_pd = ParserDetailsFactory.Create(def_document);
                        if (def_pd == null) continue;
                        // Note: Compiler fails to catch comparison of ISymbol to a IList<CombinedScopeSymbol>. How is this possible?
                        //var fod = def_pd.Attributes.Where(kvp => kvp.Value == sym).Select(kvp => kvp.Key).FirstOrDefault();
                        var fod = def_pd.Attributes.Where(
                            kvp => kvp.Value.Contains(value))
                            .Select(kvp => kvp.Key).FirstOrDefault();
                        if (fod == null) continue;
                        sb.Append("defined in ");
                        sb.Append(sym.file);
                        sb.AppendLine();
                        var node = fod;
                        for (; node != null; node = node.Parent)
                            if (node is Java9Parser.FieldDeclarationContext
                            || node is Java9Parser.LocalVariableDeclarationContext
                            || node is Java9Parser.FormalParameterContext) break;
                        if (node == null) continue;
                        Reconstruct.Doit(sb, node);
                    }
                    return sb.ToString();
                },
            (ParserDetails pd, IParseTree t) => // method
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return null;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    var dir = System.IO.Path.GetDirectoryName(pd.Item.FullPath);
                    pd.Attributes.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                    if (list_value == null) return null;
                    StringBuilder sb = new StringBuilder();
                    foreach (var value in list_value)
                    {
                        if (value == null) continue;
                        var sym = value as ISymbol;
                        if (sym == null) continue;
                        if (sym is RefSymbol)
                        {
                            sym = sym.resolve();
                        }
                        if (sym is MethodSymbol)
                            sb.Append("Method ");
                        else continue;
                        var def_file = sym.file;
                        if (def_file == null) continue;
                        var def_document = Workspaces.Workspace.Instance.FindDocument(def_file);
                        if (def_document == null) continue;
                        var def_pd = ParserDetailsFactory.Create(def_document);
                        if (def_pd == null) continue;
                        // Note: Compiler fails to catch comparison of ISymbol to a IList<CombinedScopeSymbol>. How is this possible?
                        //var fod = def_pd.Attributes.Where(kvp => kvp.Value == sym).Select(kvp => kvp.Key).FirstOrDefault();
                        var fod = def_pd.Attributes.Where(
                            kvp => kvp.Value.Contains(value))
                            .Select(kvp => kvp.Key).FirstOrDefault();
                        if (fod == null) continue;
                        sb.Append("defined in ");
                        sb.Append(sym.file);
                        sb.AppendLine();
                        var node = fod;
                        for (; node != null; node = node.Parent)
                            if (node is Java9Parser.MethodDeclarationContext) break;
                        if (node == null) continue;
                        // Take all children except class body.
                        int end = node.ChildCount - 1;
                        Reconstruct.Doit(sb, node, 0, end);
                    }
                    return sb.ToString();
                },
            null, // comment
            null, // keyword
            null, // keyword-control
            null, // literal
            (ParserDetails pd, IParseTree t) => // type
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return null;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    var dir = System.IO.Path.GetDirectoryName(pd.Item.FullPath);
                    pd.Attributes.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                    if (list_value == null) return null;
                    StringBuilder sb = new StringBuilder();
                    foreach (var value in list_value)
                    {
                        if (value == null) continue;
                        var sym = value as ISymbol;
                        if (sym == null) continue;
                        if (sym is RefSymbol)
                        {
                            sym = sym.resolve();
                        }
                        if (sym is ClassSymbol)
                            sb.Append("Class ");
                        else continue;
                        var def_file = sym.file;
                        if (def_file == null) continue;
                        var def_document = Workspaces.Workspace.Instance.FindDocument(def_file);
                        if (def_document == null) continue;
                        var def_pd = ParserDetailsFactory.Create(def_document);
                        if (def_pd == null) continue;
                        //var fod = def_pd.Attributes.Where(kvp => kvp.Value == sym).Select(kvp => kvp.Key).FirstOrDefault();
                        var fod = def_pd.Attributes.Where(
                            kvp => kvp.Value.Contains(value))
                            .Select(kvp => kvp.Key).FirstOrDefault();
                        if (fod == null) continue;
                        sb.Append("defined in ");
                        sb.Append(sym.file);
                        sb.AppendLine();
                        var node = fod;
                        for (; node != null; node = node.Parent)
                            if (node is Java9Parser.NormalClassDeclarationContext) break;
                        if (node == null) continue;
                        // Take all children except class body.
                        int end = node.ChildCount - 1;
                        Reconstruct.Doit(sb, node, 0, end);
                    }
                    return sb.ToString();
                },
            null,
            (ParserDetails pd, IParseTree t) => // field
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return null;
                    Antlr4.Runtime.Tree.IParseTree p = term;
                    var dir = System.IO.Path.GetDirectoryName(pd.Item.FullPath);
                    pd.Attributes.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                    if (list_value == null) return null;
                    StringBuilder sb = new StringBuilder();
                    foreach (var value in list_value)
                    {
                        if (value == null) continue;
                        var sym = value as ISymbol;
                        if (sym == null) continue;
                        if (sym is RefSymbol)
                        {
                            sym = sym.resolve();
                        }
                        if (sym is FieldSymbol)
                            sb.Append("Field ");
                        else continue;
                        var def_file = sym.file;
                        if (def_file == null) continue;
                        var def_document = Workspaces.Workspace.Instance.FindDocument(def_file);
                        if (def_document == null) continue;
                        var def_pd = ParserDetailsFactory.Create(def_document);
                        if (def_pd == null) continue;
                        //var fod = def_pd.Attributes.Where(kvp => kvp.Value == sym).Select(kvp => kvp.Key).FirstOrDefault();
                        var fod = def_pd.Attributes.Where(
                            kvp => kvp.Value.Contains(value))
                            .Select(kvp => kvp.Key).FirstOrDefault();
                        if (fod == null) continue;
                        sb.Append("defined in ");
                        sb.Append(sym.file);
                        sb.AppendLine();
                        var node = fod;
                        for (; node != null; node = node.Parent)
                            if (node is Java9Parser.FieldDeclarationContext) break;
                        if (node == null) continue;
                        Reconstruct.Doit(sb, node);
                    }
                    return sb.ToString();
                },
        };

        public bool CanNextRule { get { return false; } }

        public bool DoErrorSquiggles { get { return false; } }

        public bool CanReformat { get { return true; } }
    }
}
