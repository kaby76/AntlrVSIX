namespace LanguageServer
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using GrammarGrammar;
    using Symtab;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class AntlrGrammarDescription : IGrammarDescription
    {
        public string Name { get; } = "Antlr";
        public System.Type Parser { get; } = typeof(ANTLRv4Parser);
        public System.Type Lexer { get; } = typeof(ANTLRv4Lexer);

        public ParserDetails CreateParserDetails(Workspaces.Document item)
        {
            return new AntlrGrammarDetails(item);
        }

        public void Parse(ParserDetails pd)
        {
            string ffn = pd.FullFileName;
            string code = pd.Code;

            IParseTree pt = null;

            // Set up Antlr to parse input grammar.
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            AntlrInputStream ais = new AntlrInputStream(
            new StreamReader(
                new MemoryStream(byteArray)).ReadToEnd())
            {
                name = ffn
            };
            ANTLRv4Lexer lexer = new ANTLRv4Lexer(ais);
            CommonTokenStream cts = new CommonTokenStream(lexer);
            ANTLRv4Parser parser = new ANTLRv4Parser(cts);
            var listener = new ErrorListener<IToken>(parser, lexer, cts);
            parser.AddErrorListener(listener);
            try
            {
                pt = parser.grammarSpec();
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
            if (listener.had_error)
            {
                System.Console.Error.WriteLine("Error in parse of " + ffn);
            }
            else
            {
                System.Console.Error.WriteLine("Parse completed of " + ffn);
            }

            pd.TokStream = cts;
            pd.Parser = parser;
            pd.Lexer = lexer;
            pd.ParseTree = pt;
        }

        public void Parse(string code,
            out CommonTokenStream TokStream,
            out Parser Parser,
            out Lexer Lexer,
            out IParseTree ParseTree)
        {
            IParseTree pt = null;

            // Set up Antlr to parse input grammar.
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            AntlrInputStream ais = new AntlrInputStream(
            new StreamReader(
                new MemoryStream(byteArray)).ReadToEnd());
            ANTLRv4Lexer lexer = new ANTLRv4Lexer(ais);
            CommonTokenStream cts = new CommonTokenStream(lexer);
            ANTLRv4Parser parser = new ANTLRv4Parser(cts);
            try
            {
                pt = parser.grammarSpec();
            }
            catch (Exception)
            {
                // Parsing error.
            }

            TokStream = cts;
            Parser = parser;
            Lexer = lexer;
            ParseTree = pt;
        }

        public Dictionary<IToken, int> ExtractComments(string code)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            CommonTokenStream cts_off_channel = new CommonTokenStream(
                new ANTLRv4Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())),
                ANTLRv4Lexer.COMMENT);
            Dictionary<IToken, int> new_list = new Dictionary<IToken, int>();
            int type = (int)AntlrClassifications.ClassificationComment;
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
            if (ffn == null)
            {
                return false;
            }

            List<string> allowable_suffices = FileExtension.Split(';').ToList<string>();
            string suffix = Path.GetExtension(ffn).ToLower();
            foreach (string s in allowable_suffices)
            {
                if (suffix == s)
                {
                    return true;
                }
            }

            return false;
        }

        public Dictionary<IParseTree, ISymbol> GetSymbolTable()
        {
            return new Dictionary<IParseTree, ISymbol>();
        }


        /* Tagging and classification types. */
        public enum AntlrClassifications : int
        {
            ClassificationNonterminalDef = 0,
            ClassificationNonterminalRef,
            ClassificationTerminalDef,
            ClassificationTerminalRef,
            ClassificationComment,
            ClassificationKeyword,
            ClassificationLiteral,
            ClassificationModeDef,
            ClassificationModeRef,
            ClassificationChannelDef,
            ClassificationChannelRef,
            ClassificationPunctuation,
            ClassificationOperator,
        }



        public string[] Map { get; } = new string[]
        {
         "Antlr - nonterminal def",
         "Antlr - nonterminal ref",
         "Antlr - terminal def",
         "Antlr - terminal ref",
         "Antlr - comment",
         "Antlr - keyword",
         "Antlr - literal",
         "Antlr - mode def",
         "Antlr - mode ref",
         "Antlr - channel def",
         "Antlr - channel ref",
         "Antlr - punctuation",
         "Antlr - operator",
        };


        public List<bool> CanFindAllRefs { get; } = new List<bool>()
        {
            true, // nonterminal
            true, // nonterminal
            true, // Terminal
            true, // Terminal
            false, // comment
            false, // keyword
            true, // literal
            true, // mode
            true, // mode
            true, // channel
            true, // channel
            false, // punctuation
            false, // operator
        };

        public List<bool> CanRename { get; } = new List<bool>()
        {
            true, // nonterminal
            true, // nonterminal
            true, // Terminal
            true, // Terminal
            false, // comment
            false, // keyword
            false, // literal
            true, // mode
            true, // mode
            true, // channel
            true, // channel
            false, // punctuation
            false, // operator
        };

        public List<bool> CanGotodef { get; } = new List<bool>()
        {
            true, // nonterminal
            true, // nonterminal
            true, // Terminal
            true, // Terminal
            false, // comment
            false, // keyword
            false, // literal
            true, // mode
            true, // mode
            true, // channel
            true, // channel
            false, // punctuation
            false, // operator
        };

        public List<bool> CanGotovisitor { get; } = new List<bool>()
        {
            true, // nonterminal
            true, // nonterminal
            false, // Terminal
            false, // Terminal
            false, // comment
            false, // keyword
            false, // literal
            false, // mode
            false, // mode
            false, // channel
            false, // channel
            false, // punctuation
            false, // operator
        };

        private static readonly List<string> _antlr_keywords = new List<string>()
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


        public Func<IGrammarDescription, Dictionary<IParseTree, IList<CombinedScopeSymbol>>, IParseTree, int>
            Classify { get; } = 
            (IGrammarDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) =>
            {
                TerminalNodeImpl term = t as TerminalNodeImpl;
                Antlr4.Runtime.Tree.IParseTree p = term;
                st.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                if (list_value != null)
                {
                    // There's a symbol table entry for the leaf node.
                    // So, it is either a terminal, nonterminal,
                    // channel, mode.
                    // We don't care if it's a defining occurrence or
                    // applied occurrence, just what type of symbol it
                    // is.
                    foreach (CombinedScopeSymbol value in list_value)
                    {
                        if (value is RefSymbol)
                        {
                            List<ISymbol> defs = ((RefSymbol) value).Def;
                            foreach (var d in defs)
                            {
                                if (d is NonterminalSymbol)
                                {
                                    return (int)AntlrClassifications.ClassificationNonterminalRef;
                                }
                                else if (d is TerminalSymbol)
                                {
                                    return (int)AntlrClassifications.ClassificationNonterminalRef;
                                }
                                else if (d is ModeSymbol)
                                {
                                    return (int)AntlrClassifications.ClassificationModeRef; ;
                                }
                                else if (d is ChannelSymbol)
                                {
                                    return (int)AntlrClassifications.ClassificationChannelRef; ;
                                }
                            }
                        }
                        else if (value is NonterminalSymbol)
                        {
                            return (int)AntlrClassifications.ClassificationNonterminalDef;
                        }
                        else if (value is TerminalSymbol)
                        {
                            return (int)AntlrClassifications.ClassificationTerminalDef;
                        }
                        else if (value is ModeSymbol)
                        {
                            return (int)AntlrClassifications.ClassificationModeDef;
                        }
                        else if (value is ChannelSymbol)
                        {
                            return (int)AntlrClassifications.ClassificationChannelDef;
                        }
                    }
                }
                else
                {
                    // It is either a keyword, literal, comment.
                    string text = term.GetText();
                    if (_antlr_keywords.Contains(text))
                    {
                        return (int)AntlrClassifications.ClassificationKeyword;
                    }
                    if ((term.Symbol.Type == ANTLRv4Parser.STRING_LITERAL
                          || term.Symbol.Type == ANTLRv4Parser.INT
                          || term.Symbol.Type == ANTLRv4Parser.LEXER_CHAR_SET))
                    {
                        return (int)AntlrClassifications.ClassificationLiteral;
                    }
                    // The token could be part of parserRuleSpec context.
                    //for (IRuleNode r = term.Parent; r != null; r = r.Parent)
                    //{
                    //    if (r is ANTLRv4Parser.ParserRuleSpecContext ||
                    //          r is ANTLRv4Parser.LexerRuleSpecContext)
                    //    {
                    //        return 4;
                    //    }
                    //}
                    if (term.Payload.Channel == ANTLRv4Lexer.OFF_CHANNEL
                        || term.Symbol.Type == ANTLRv4Lexer.DOC_COMMENT
                        || term.Symbol.Type == ANTLRv4Lexer.BLOCK_COMMENT
                        || term.Symbol.Type == ANTLRv4Lexer.LINE_COMMENT)
                    {
                        return (int)AntlrClassifications.ClassificationComment;
                    }
                }
                return -1;
            };

        public bool CanNextRule => true;

        public bool DoErrorSquiggles => true;

        public bool CanReformat => true;

        public List<Func<ParserDetails, IParseTree, string>> PopUpDefinition { get; } =
        new List<Func<ParserDetails, IParseTree, string>>()
        {
            (ParserDetails pd, IParseTree t) => // nonterminal
            {
                TerminalNodeImpl term = t as TerminalNodeImpl;
                if (term == null)
                {
                    return null;
                }
                Antlr4.Runtime.Tree.IParseTree p = term;
                string dir = System.IO.Path.GetDirectoryName(pd.Item.FullPath);
                pd.Attributes.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                if (list_value == null)
                {
                    return null;
                }
                bool first = true;
                StringBuilder sb = new StringBuilder();
                foreach (CombinedScopeSymbol value in list_value)
                {
                    if (value == null)
                    {
                        continue;
                    }
                    ISymbol sym = value as ISymbol;
                    if (sym == null)
                    {
                        continue;
                    }
                    List<ISymbol> list_of_syms = new List<ISymbol>() { sym };
                    if (sym is RefSymbol)
                    {
                        list_of_syms = sym.resolve();
                    }
                    foreach (ISymbol s in list_of_syms)
                    {
                        if (! first)
                        {
                            sb.AppendLine();
                        }
                        first = false;
                        if (s is TerminalSymbol)
                        {
                            sb.Append("Terminal ");
                        }
                        else if (s is NonterminalSymbol)
                        {
                            sb.Append("Nonterminal ");
                        }
                        else
                        {
                            continue;
                        }
                        string def_file = s.file;
                        if (def_file == null)
                        {
                            continue;
                        }
                        Workspaces.Document def_document = Workspaces.Workspace.Instance.FindDocument(def_file);
                        if (def_document == null)
                        {
                            continue;
                        }
                        ParserDetails def_pd = ParserDetailsFactory.Create(def_document);
                        if (def_pd == null)
                        {
                            continue;
                        }
                        IParseTree fod = def_pd.Attributes.Where(
                                kvp =>
                                {
                                    IParseTree key = kvp.Key;
                                    if (!(key is TerminalNodeImpl))
                                        return false;
                                    TerminalNodeImpl t1 = key as TerminalNodeImpl;
                                    IToken s1 = t1.Symbol;
                                    if (s1 == s.Token)
                                        return true;
                                    return false;
                                })
                            .Select(kvp => kvp.Key).FirstOrDefault();
                        if (fod == null)
                        {
                            continue;
                        }
                        sb.Append("defined in ");
                        sb.Append(s.file);
                        sb.AppendLine();
                        IParseTree node = fod;
                        for (; node != null; node = node.Parent)
                        {
                            if (node is ANTLRv4Parser.LexerRuleSpecContext ||
                                node is ANTLRv4Parser.ParserRuleSpecContext ||
                                node is ANTLRv4Parser.TokensSpecContext)
                            {
                                break;
                            }
                        }
                        if (node == null)
                        {
                            continue;
                        }
                        Reconstruct.Doit(sb, node);
                    }
                }
                return sb.ToString();
            },
            (ParserDetails pd, IParseTree t) => // terminal
            {
                TerminalNodeImpl term = t as TerminalNodeImpl;
                if (term == null)
                {
                    return null;
                }
                Antlr4.Runtime.Tree.IParseTree p = term;
                string dir = System.IO.Path.GetDirectoryName(pd.Item.FullPath);
                pd.Attributes.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
                if (list_value == null)
                {
                    return null;
                }
                bool first = true;
                StringBuilder sb = new StringBuilder();
                foreach (CombinedScopeSymbol value in list_value)
                {
                    if (value == null)
                    {
                        continue;
                    }
                    ISymbol sym = value as ISymbol;
                    if (sym == null)
                    {
                        continue;
                    }
                    List<ISymbol> list_of_syms = new List<ISymbol>() { sym };
                    if (sym is RefSymbol)
                    {
                        list_of_syms = sym.resolve();
                    }
                    foreach (ISymbol s in list_of_syms)
                    {
                        if (! first)
                        {
                            sb.AppendLine();
                        }
                        first = false;
                        if (s is TerminalSymbol)
                        {
                            sb.Append("Terminal ");
                        }
                        else if (s is NonterminalSymbol)
                        {
                            sb.Append("Nonterminal ");
                        }
                        else
                        {
                            continue;
                        }
                        string def_file = s.file;
                        if (def_file == null)
                        {
                            continue;
                        }
                        Workspaces.Document def_document = Workspaces.Workspace.Instance.FindDocument(def_file);
                        if (def_document == null)
                        {
                            continue;
                        }
                        ParserDetails def_pd = ParserDetailsFactory.Create(def_document);
                        if (def_pd == null)
                        {
                            continue;
                        }
                        IParseTree fod = def_pd.Attributes.Where(
                                kvp =>
                                {
                                    IParseTree key = kvp.Key;
                                    if (!(key is TerminalNodeImpl))
                                        return false;
                                    TerminalNodeImpl t1 = key as TerminalNodeImpl;
                                    IToken s1 = t1.Symbol;
                                    if (s1 == s.Token)
                                        return true;
                                    return false;
                                })
                            .Select(kvp => kvp.Key).FirstOrDefault();
                        if (fod == null)
                        {
                            continue;
                        }
                        sb.Append("defined in ");
                        sb.Append(s.file);
                        sb.AppendLine();
                        IParseTree node = fod;
                        for (; node != null; node = node.Parent)
                        {
                            if (node is ANTLRv4Parser.LexerRuleSpecContext ||
                                node is ANTLRv4Parser.ParserRuleSpecContext ||
                                node is ANTLRv4Parser.TokensSpecContext)
                            {
                                break;
                            }
                        }
                        if (node == null)
                        {
                            continue;
                        }
                        Reconstruct.Doit(sb, node);
                    }
                }

                return sb.ToString();
            },
            null, // comment
            null, // keyword
            null, // literal
            null,
            null,
        };


    }
}
