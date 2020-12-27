namespace LanguageServer
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Antlr4ParsingResults : ParsingResults
    {
        public Antlr4ParsingResults(Workspaces.Document doc) : base(doc)
        {
            // Passes executed in order for all files.
            Passes.Add(() =>
            {
                // Create scopes for all files in workspace
                // if they don't already exist.
                foreach (KeyValuePair<string, HashSet<string>> dep in InverseImports)
                {
                    string name = dep.Key;
                    _scopes.TryGetValue(name, out IScope file_scope);
                    if (file_scope != null)
                    {
                        continue;
                    }

                    _scopes[name] = new FileScope(name, null);
                }

                // Set up search path scopes for Imports relationship.
                IScope root = _scopes[FullFileName];
                foreach (string dep in Imports)
                {
                    // Don't add if already have this search path.
                    IScope dep_scope = _scopes[dep];
                    bool found = false;
                    foreach (IScope scope in root.NestedScopes)
                    {
                        if (scope is SearchPathScope)
                        {
                            SearchPathScope spc = scope as SearchPathScope;
                            if (spc.NestedScopes.First() == dep_scope)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        SearchPathScope import = new SearchPathScope(root);
                        import.nest(dep_scope);
                        root.nest(import);
                    }
                }
                root.empty();
                RootScope = root;
                return false;
            });
            Passes.Add(() =>
            {
                if (ParseTree == null) return false;
                ParseTreeWalker.Default.Walk(new Pass2Listener(this), ParseTree);
                return false;
            });
            Passes.Add(() =>
            {
                if (ParseTree == null) return false;
                ParseTreeWalker.Default.Walk(new Pass3Listener(this), ParseTree);
                return false;
            });
        }


        public override List<bool> CanFindAllRefs { get; } = new List<bool>()
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
        public override List<bool> CanGotodef { get; } = new List<bool>()
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
        public override List<bool> CanGotovisitor { get; } = new List<bool>()
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
        public override bool CanNextRule
        {
            get
            {
                return true;
            }
        }
        public override List<bool> CanRename { get; } = new List<bool>()
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
        public override bool CanReformat
        {
            get
            {
                return true;
            }
        }
        public override Func<IParserDescription, Dictionary<IParseTree, IList<CombinedScopeSymbol>>, IParseTree, int> Classify { get; } =
         (IParserDescription gd, Dictionary<IParseTree, IList<CombinedScopeSymbol>> st, IParseTree t) =>
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
                         List<ISymbol> defs = ((RefSymbol)value).Def;
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
        public override bool DoErrorSquiggles => true;
        public override string FileExtension { get; } = ".g4;.g";
        public override string[] Map { get; } = new string[]
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
        public override string Name { get; } = "Antlr4";
        public override List<Func<ParsingResults, IParseTree, string>> PopUpDefinition { get; } =
            new List<Func<ParsingResults, IParseTree, string>>()
  {
            (ParsingResults pd, IParseTree t) => // nonterminal
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

                        var workspace = pd.Item.Workspace;
                        Workspaces.Document def_document = workspace.FindDocument(def_file);
                        if (def_document == null)
                        {
                            continue;
                        }
                        ParsingResults def_pd = ParsingResultsFactory.Create(def_document);
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
                                    if (s.Token.Contains(s1))
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
            (ParsingResults pd, IParseTree t) => // nonterminal
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

                        var workspace = pd.Item.Workspace;
                        Workspaces.Document def_document = workspace.FindDocument(def_file);
                        if (def_document == null)
                        {
                            continue;
                        }
                        ParsingResults def_pd = ParsingResultsFactory.Create(def_document);
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
                                    if (s.Token.Contains(s1))
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
            (ParsingResults pd, IParseTree t) => // terminal
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

                        var workspace = pd.Item.Workspace;
                        Workspaces.Document def_document = workspace.FindDocument(def_file);
                        if (def_document == null)
                        {
                            continue;
                        }
                        ParsingResults def_pd = ParsingResultsFactory.Create(def_document);
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
                                    if (s.Token.Contains(s1))
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
            (ParsingResults pd, IParseTree t) => // terminal
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

                        var workspace = pd.Item.Workspace;
                        Workspaces.Document def_document = workspace.FindDocument(def_file);
                        if (def_document == null)
                        {
                            continue;
                        }
                        ParsingResults def_pd = ParsingResultsFactory.Create(def_document);
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
                                    if (s.Token.Contains(s1))
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
            null, // Mode
            null, // Mode
            null, // Channel
            null, // Channel
            null, // Punctuation
            null, // Operator
  };
        public override int QuietAfter { get; set; } = 0;
        public override string StartRule { get; } = "grammarSpec";



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


        public override Dictionary<IToken, int> ExtractComments(string code)
        {
            if (code == null) return null;
            var cts = this.TokStream;
            int type = (int)AntlrClassifications.ClassificationComment;
            Dictionary<IToken, int> new_list = new Dictionary<IToken, int>();
            for (int i = 0; i < cts.Index; ++i)
            {
                IList<IToken> inter = cts.GetHiddenTokensToLeft(i);
                if (inter != null)
                    foreach (IToken token in inter)
                    {
                        if (token.Type == ANTLRv4Lexer.BLOCK_COMMENT
                            || token.Type == ANTLRv4Lexer.LINE_COMMENT
                            || token.Type == ANTLRv4Lexer.DOC_COMMENT)
                        {
                            new_list[token] = type;
                        }
                    }
            }



            //byte[] byteArray = Encoding.UTF8.GetBytes(code);
            //var ais = new AntlrInputStream(
            //            new StreamReader(
            //                new MemoryStream(byteArray)).ReadToEnd());
            //ANTLRv4Lexer lexer = new ANTLRv4Lexer(ais);
            //CommonTokenStream cts_off_channel = new CommonTokenStream(lexer, ANTLRv4Lexer.OFF_CHANNEL);
            //lexer.RemoveErrorListeners();
            //var lexer_error_listener = new ErrorListener<int>(null, lexer, cts_off_channel, this.QuietAfter);
            //lexer.AddErrorListener(lexer_error_listener);
            //while (cts_off_channel.LA(1) != ANTLRv4Parser.Eof)
            //{
            //    IToken token = cts_off_channel.LT(1);
            //    if (token.Type == ANTLRv4Lexer.BLOCK_COMMENT
            //        || token.Type == ANTLRv4Lexer.LINE_COMMENT
            //        || token.Type == ANTLRv4Lexer.DOC_COMMENT)
            //    {
            //        new_list[token] = type;
            //    }
            //    cts_off_channel.Consume();
            //}
            return new_list;
        }

        public Dictionary<IParseTree, ISymbol> GetSymbolTable()
        {
            return new Dictionary<IParseTree, ISymbol>();
        }

        public override bool IsFileType(string ffn)
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

        public override void Parse(ParsingResults pd, bool bail)
        {
            string ffn = pd.FullFileName;
            string code = pd.Code;
            if (ffn == null) return;
            if (code == null) return;
            this.QuietAfter = pd.QuietAfter;

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
            lexer.RemoveErrorListeners();
            var lexer_error_listener = new ErrorListener<int>(parser, lexer, pd.QuietAfter);
            lexer.AddErrorListener(lexer_error_listener);
            parser.RemoveErrorListeners();
            var parser_error_listener = new ErrorListener<IToken>(parser, lexer, pd.QuietAfter);
            parser.AddErrorListener(parser_error_listener);
            BailErrorHandler bail_error_handler = null;
            if (bail)
            {
                bail_error_handler = new BailErrorHandler();
                parser.ErrorHandler = bail_error_handler;
            }
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
            if (parser_error_listener.had_error || lexer_error_listener.had_error || (bail_error_handler != null && bail_error_handler.had_error))
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
            Stack<IParseTree> stack = new Stack<IParseTree>();
            stack.Push(pt);
            while (stack.Any())
            {
                var x = stack.Pop();
                if (x is TerminalNodeImpl leaf)
                {
                }
                else
                {
                    var y = x as AttributedParseTreeNode;
                    if (Object.ReferenceEquals(y, null)) y.ParserDetails = pd;
                    for (int i = 0; i < x.ChildCount; ++i)
                    {
                        var c = x.GetChild(i);
                        if (c != null) stack.Push(c);
                    }
                }
            }
        }

        public override void Parse(string code,
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
            lexer.RemoveErrorListeners();
            var lexer_error_listener = new ErrorListener<int>(parser, lexer, this.QuietAfter);
            lexer.AddErrorListener(lexer_error_listener);
            parser.RemoveErrorListeners();
            var parser_error_listener = new ErrorListener<IToken>(parser, lexer, this.QuietAfter);
            parser.AddErrorListener(parser_error_listener);
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

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void GetGrammarBasics()
        {
            // Gather Imports from grammars.
            // Gather InverseImports map.
            if (!ParsingResults.InverseImports.ContainsKey(this.FullFileName))
            {
                ParsingResults.InverseImports.Add(this.FullFileName, new HashSet<string>());
            }
            if (ParseTree == null) return;
            ParseTreeWalker.Default.Walk(new Pass0Listener(this), ParseTree);
            var workspace = this.Item.Workspace;
            foreach (KeyValuePair<string, HashSet<string>> dep in ParsingResults.InverseImports)
            {
                string name = dep.Key;
                Workspaces.Document x = workspace.FindDocument(name);
                if (x == null)
                {
                    // Add document.
                    Workspaces.Container proj = Item.Parent;
                    Workspaces.Document new_doc = new Workspaces.Document(name);
                    proj.AddChild(new_doc);
                }
            }
        }

        public class Pass0Listener : ANTLRv4ParserBaseListener
        {
            private readonly ParsingResults _pd;
            private bool saw_tokenVocab_option = false;
            private enum GrammarType
            {
                Combined,
                Parser,
                Lexer
            }

            private GrammarType Type;

            public Pass0Listener(ParsingResults pd)
            {
                _pd = pd;
                if (!ParsingResults.InverseImports.ContainsKey(_pd.FullFileName))
                {
                    ParsingResults.InverseImports.Add(_pd.FullFileName, new HashSet<string>());
                }
            }

            public override void EnterGrammarType([NotNull] ANTLRv4Parser.GrammarTypeContext context)
            {
                if (context.GetChild(0).GetText() == "parser")
                {
                    Type = GrammarType.Parser;
                }
                else if (context.GetChild(0).GetText() == "lexer")
                {
                    Type = GrammarType.Lexer;
                }
                else
                {
                    Type = GrammarType.Combined;
                }
            }

            public override void EnterOption([NotNull] ANTLRv4Parser.OptionContext context)
            {
                if (context.ChildCount < 3)
                {
                    return;
                }

                if (context.GetChild(0) == null)
                {
                    return;
                }

                if (context.GetChild(0).GetText() != "tokenVocab")
                {
                    return;
                }

                string dep_grammar = context.GetChild(2).GetText();
                string file = _pd.Item.FullPath;
                string dir = System.IO.Path.GetDirectoryName(file);
                string dep = dir + System.IO.Path.DirectorySeparatorChar + dep_grammar + ".g4";
                dep = Workspaces.Util.GetProperFilePathCapitalization(dep);
                if (dep == null)
                {
                    return;
                }

                _pd.Imports.Add(dep);
                if (!ParsingResults.InverseImports.ContainsKey(dep))
                {
                    ParsingResults.InverseImports.Add(dep, new HashSet<string>());
                }

                bool found = false;
                foreach (string f in ParsingResults.InverseImports[dep])
                {
                    if (f == file)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ParsingResults.InverseImports[dep].Add(file);
                }
                saw_tokenVocab_option = true;
            }

            public override void EnterDelegateGrammar([NotNull] ANTLRv4Parser.DelegateGrammarContext context)
            {
                if (context.ChildCount < 1)
                {
                    return;
                }

                if (context.GetChild(0) == null)
                {
                    return;
                }

                string dep_grammar = context.GetChild(0).GetText();
                string file = _pd.Item.FullPath;
                string dir = System.IO.Path.GetDirectoryName(file);
                string dep = dir + System.IO.Path.DirectorySeparatorChar + dep_grammar + ".g4";
                dep = Workspaces.Util.GetProperFilePathCapitalization(dep);
                if (dep == null)
                {
                    return;
                }

                _pd.Imports.Add(dep);
                if (!ParsingResults.InverseImports.ContainsKey(dep))
                {
                    ParsingResults.InverseImports.Add(dep, new HashSet<string>());
                }

                bool found = false;
                foreach (string f in ParsingResults.InverseImports[dep])
                {
                    if (f == file)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ParsingResults.InverseImports[dep].Add(file);
                }
            }

            public override void EnterRules([NotNull] ANTLRv4Parser.RulesContext context)
            {
                if (saw_tokenVocab_option)
                {
                    return;
                }

                if (Type == GrammarType.Lexer)
                {
                    string file = _pd.Item.FullPath;
                    string dep = file.Replace("Lexer.g4", "Parser.g4");
                    if (dep == file)
                    {
                        // If the file is not named correctly so that it ends in Parser.g4,
                        // then it's probably a mistake. I don't know where to get the lexer
                        // grammar.
                        return;
                    }
                    if (!ParsingResults.InverseImports.ContainsKey(dep))
                    {
                        ParsingResults.InverseImports.Add(dep, new HashSet<string>());
                    }

                    bool found = false;
                    foreach (string f in ParsingResults.InverseImports[dep])
                    {
                        if (f == file)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        ParsingResults.InverseImports[file].Add(dep);
                    }
                }
                if (Type == GrammarType.Parser)
                {
                    // It's a parser grammar, but we didn't see the tokenVocab option for the lexer.
                    // We must assume a lexer grammar in this directory.
                    // BUT!!!! There could be many things wrong here, so just don't do this willy nilly.

                    string file = _pd.Item.FullPath;
                    string dep = file.Replace("Parser.g4", "Lexer.g4");
                    if (dep == file)
                    {
                        // If the file is not named correctly so that it ends in Parser.g4,
                        // then it's probably a mistake. I don't know where to get the lexer
                        // grammar.
                        return;
                    }

                    string dir = System.IO.Path.GetDirectoryName(file);
                    _pd.Imports.Add(dep);
                    if (!ParsingResults.InverseImports.ContainsKey(dep))
                    {
                        ParsingResults.InverseImports.Add(dep, new HashSet<string>());
                    }

                    bool found = false;
                    foreach (string f in ParsingResults.InverseImports[dep])
                    {
                        if (f == file)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        ParsingResults.InverseImports[dep].Add(file);
                    }
                }
            }
        }

        public class Pass2Listener : ANTLRv4ParserBaseListener
        {
            private readonly ParsingResults _pd;

            public Pass2Listener(ParsingResults pd)
            {
                _pd = pd;
            }

            public IParseTree NearestScope(IParseTree node)
            {
                for (; node != null; node = node.Parent)
                {
                    _pd.Attributes.TryGetValue(node, out IList<CombinedScopeSymbol> list);
                    if (list != null)
                    {
                        if (list.Count == 1 && list[0] is IScope)
                        {
                            return node;
                        }
                    }
                }
                return null;
            }

            public IScope GetScope(IParseTree node)
            {
                if (node == null)
                {
                    return null;
                }

                _pd.Attributes.TryGetValue(node, out IList<CombinedScopeSymbol> list);
                if (list != null)
                {
                    if (list.Count == 1 && list[0] is IScope)
                    {
                        return list[0] as IScope;
                    }
                }
                return null;
            }

            public override void EnterGrammarSpec([NotNull] ANTLRv4Parser.GrammarSpecContext context)
            {
                _pd.Attributes[context] = new List<CombinedScopeSymbol>() { (CombinedScopeSymbol)_pd.RootScope };
            }

            public override void EnterParserRuleSpec([NotNull] ANTLRv4Parser.ParserRuleSpecContext context)
            {
                int i;
                for (i = 0; i < context.ChildCount; ++i)
                {
                    if (!(context.GetChild(i) is TerminalNodeImpl))
                    {
                        continue;
                    }

                    TerminalNodeImpl c = context.GetChild(i) as TerminalNodeImpl;
                    if (c.Symbol.Type == ANTLRv4Lexer.RULE_REF)
                    {
                        break;
                    }
                }
                if (i == context.ChildCount)
                {
                    return;
                }

                TerminalNodeImpl rule_ref = context.GetChild(i) as TerminalNodeImpl;
                string id = rule_ref.GetText();
                ISymbol sym = new NonterminalSymbol(id, new List<IToken>() { rule_ref.Symbol });
                _pd.RootScope.define(ref sym);
                CombinedScopeSymbol s = (CombinedScopeSymbol)sym;
                _pd.Attributes[context] = new List<CombinedScopeSymbol>() { s };
                _pd.Attributes[context.GetChild(i)] = new List<CombinedScopeSymbol>() { s };
            }

            public override void EnterLexerRuleSpec([NotNull] ANTLRv4Parser.LexerRuleSpecContext context)
            {
                int i;
                for (i = 0; i < context.ChildCount; ++i)
                {
                    if (!(context.GetChild(i) is TerminalNodeImpl))
                    {
                        continue;
                    }

                    TerminalNodeImpl c = context.GetChild(i) as TerminalNodeImpl;
                    if (c.Symbol.Type == ANTLRv4Lexer.TOKEN_REF)
                    {
                        break;
                    }
                }
                if (i == context.ChildCount)
                {
                    return;
                }

                TerminalNodeImpl token_ref = context.GetChild(i) as TerminalNodeImpl;
                string id = token_ref.GetText();
                ISymbol sym = new TerminalSymbol(id, new List<IToken>() { token_ref.Symbol });
                _pd.RootScope.define(ref sym);
                CombinedScopeSymbol s = (CombinedScopeSymbol)sym;
                _pd.Attributes[context] = new List<CombinedScopeSymbol>() { s };
                _pd.Attributes[context.GetChild(i)] = new List<CombinedScopeSymbol>() { s };
            }

            public override void EnterIdentifier([NotNull] ANTLRv4Parser.IdentifierContext context)
            {
                if (context.Parent is ANTLRv4Parser.ModeSpecContext)
                {
                    TerminalNodeImpl term = context.GetChild(0) as TerminalNodeImpl;
                    string id = term.GetText();
                    ISymbol sym = new ModeSymbol(id, new List<IToken>() { term.Symbol });
                    _pd.RootScope.define(ref sym);
                    CombinedScopeSymbol s = (CombinedScopeSymbol)sym;
                    _pd.Attributes[context] = new List<CombinedScopeSymbol>() { s };
                    _pd.Attributes[context.GetChild(0)] = new List<CombinedScopeSymbol>() { s };
                }
                else if (context.Parent is ANTLRv4Parser.IdListContext && context.Parent?.Parent is ANTLRv4Parser.ChannelsSpecContext)
                {
                    TerminalNodeImpl term = context.GetChild(0) as TerminalNodeImpl;
                    string id = term.GetText();
                    ISymbol sym = new ChannelSymbol(id, new List<IToken>() { term.Symbol });
                    _pd.RootScope.define(ref sym);
                    CombinedScopeSymbol s = (CombinedScopeSymbol)sym;
                    _pd.Attributes[context] = new List<CombinedScopeSymbol>() { s };
                    _pd.Attributes[term] = new List<CombinedScopeSymbol>() { s };
                }
                else
                {
                    var p = context.Parent;
                    var add_def = false;
                    for (; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.TokensSpecContext)
                        {
                            add_def = true;
                            break;
                        }
                    }
                    if (add_def)
                    {
                        TerminalNodeImpl term = context.GetChild(0) as TerminalNodeImpl;
                        string id = term.GetText();
                        ISymbol sym = new TerminalSymbol(id, new List<IToken>() { term.Symbol });
                        _pd.RootScope.define(ref sym);
                        CombinedScopeSymbol s = (CombinedScopeSymbol)sym;
                        _pd.Attributes[context] = new List<CombinedScopeSymbol>() { s };
                        _pd.Attributes[term] = new List<CombinedScopeSymbol>() { s };
                    }
                }
            }
        }

        public class Pass3Listener : ANTLRv4ParserBaseListener
        {
            private readonly ParsingResults _pd;

            public Pass3Listener(ParsingResults pd)
            {
                _pd = pd;
            }

            public override void EnterTerminal([NotNull] ANTLRv4Parser.TerminalContext context)
            {
                if (context.TOKEN_REF() != null)
                {
                    string id = context.TOKEN_REF().GetText();
                    List<ISymbol> list = _pd.RootScope.LookupType(id).ToList();
                    if (!list.Any())
                    {
                        ISymbol sym = new TerminalSymbol(id, new List<IToken>() { context.TOKEN_REF().Symbol });
                        _pd.RootScope.define(ref sym);
                        list = _pd.RootScope.LookupType(id).ToList();
                    }
                    List<CombinedScopeSymbol> new_attrs = new List<CombinedScopeSymbol>();
                    CombinedScopeSymbol s = new RefSymbol(new List<IToken>() { context.TOKEN_REF().Symbol }, list);
                    new_attrs.Add(s);
                    _pd.Attributes[context.TOKEN_REF()] = new_attrs;
                }
            }

            public override void EnterRuleref([NotNull] ANTLRv4Parser.RulerefContext context)
            {
                TerminalNodeImpl first = context.RULE_REF() as TerminalNodeImpl;
                string id = first.GetText();
                List<ISymbol> list = _pd.RootScope.LookupType(id).ToList();
                if (!list.Any())
                {
                    ISymbol sym = new NonterminalSymbol(id, new List<IToken>() { first.Symbol });
                    _pd.RootScope.define(ref sym);
                    list = _pd.RootScope.LookupType(id).ToList();
                }
                List<CombinedScopeSymbol> new_attrs = new List<CombinedScopeSymbol>();
                CombinedScopeSymbol s = new RefSymbol(new List<IToken>() { first.Symbol }, list);
                new_attrs.Add(s);
                _pd.Attributes[first] = new_attrs;
            }

            public override void EnterIdentifier([NotNull] ANTLRv4Parser.IdentifierContext context)
            {
                if (context.Parent is ANTLRv4Parser.LexerCommandExprContext && context.Parent.Parent is ANTLRv4Parser.LexerCommandContext)
                {
                    ANTLRv4Parser.LexerCommandContext lc = context.Parent.Parent as ANTLRv4Parser.LexerCommandContext;
                    if (lc.GetChild(0)?.GetChild(0)?.GetText() == "pushMode")
                    {
                        TerminalNodeImpl term = context.GetChild(0) as TerminalNodeImpl;
                        string id = term.GetText();
                        List<ISymbol> sym_list = _pd.RootScope.LookupType(id).ToList();
                        if (!sym_list.Any())
                        {
                            ISymbol sym = new ModeSymbol(id, null);
                            _pd.RootScope.define(ref sym);
                            sym_list = _pd.RootScope.LookupType(id).ToList();
                        }
                        List<CombinedScopeSymbol> ref_list = new List<CombinedScopeSymbol>();
                        CombinedScopeSymbol s = new RefSymbol(new List<IToken>() { term.Symbol }, sym_list);
                        ref_list.Add(s);
                        _pd.Attributes[context] = ref_list;
                        _pd.Attributes[context.GetChild(0)] = ref_list;
                    }
                    else if (lc.GetChild(0)?.GetChild(0)?.GetText() == "channel")
                    {
                        TerminalNodeImpl term = context.GetChild(0) as TerminalNodeImpl;
                        string id = term.GetText();
                        List<ISymbol> sym_list = _pd.RootScope.LookupType(id).ToList();
                        if (!sym_list.Any())
                        {
                            ISymbol sym = new ChannelSymbol(id, null);
                            _pd.RootScope.define(ref sym);
                            sym_list = _pd.RootScope.LookupType(id).ToList();
                        }
                        List<CombinedScopeSymbol> ref_list = new List<CombinedScopeSymbol>();
                        CombinedScopeSymbol s = new RefSymbol(new List<IToken>() { term.Symbol }, sym_list);
                        ref_list.Add(s);
                        _pd.Attributes[context] = ref_list;
                        _pd.Attributes[context.GetChild(0)] = ref_list;
                    }
                    else if (lc.GetChild(0)?.GetChild(0)?.GetText() == "type")
                    {
                        TerminalNodeImpl term = context.GetChild(0) as TerminalNodeImpl;
                        string id = term.GetText();
                        List<ISymbol> sym_list = _pd.RootScope.LookupType(id).ToList();
                        if (!sym_list.Any())
                        {
                            ISymbol sym = new TerminalSymbol(id, null);
                            _pd.RootScope.define(ref sym);
                            sym_list = _pd.RootScope.LookupType(id).ToList();
                        }
                        List<CombinedScopeSymbol> ref_list = new List<CombinedScopeSymbol>();
                        CombinedScopeSymbol s = new RefSymbol(new List<IToken>() { term.Symbol }, sym_list);
                        ref_list.Add(s);
                        _pd.Attributes[context] = ref_list;
                        _pd.Attributes[context.GetChild(0)] = ref_list;
                    }
                }
            }
        }
    }
}

