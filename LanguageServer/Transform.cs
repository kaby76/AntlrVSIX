namespace LanguageServer
{
    using Antlr4.Runtime.Tree;
    using Graphs;
    using Symtab;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Workspaces;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Collections.Generic;


    public class Transform
    {
        private class LiteralsParser : ANTLRv4ParserBaseListener
        {
            public List<TerminalNodeImpl> Literals = new List<TerminalNodeImpl>();
            private readonly AntlrParserDetails _pd;
            public bool IsLexer = false;

            public LiteralsParser(AntlrParserDetails pd)
            {
                _pd = pd;
            }

            public override void EnterGrammarType([NotNull] ANTLRv4Parser.GrammarTypeContext context)
            {
                if (context.GetChild(0).GetText() == "parser")
                {
                    IsLexer = false;
                }
                else if (context.GetChild(0).GetText() == "lexer")
                {
                    IsLexer = true;
                }
                else
                {
                    IsLexer = false;
                }
            }

            public override void EnterTerminal([NotNull] ANTLRv4Parser.TerminalContext context)
            {
                TerminalNodeImpl first = context.GetChild(0) as TerminalNodeImpl;
                if (first.Symbol.Type == ANTLRv4Parser.STRING_LITERAL)
                {
                    Literals.Add(first);
                }
            }
        }

        public static Dictionary<string, string> ReplaceLiterals(int index, Document document)
        {
            AntlrParserDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrParserDetails;
            // Find all literals in parser grammar.
            LiteralsParser lp = new LiteralsParser(pd_parser);
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            List<TerminalNodeImpl> list = lp.Literals;
            HashSet<string> read_files = new HashSet<string>();
            read_files.Add(document.FullPath);
            for (; ;)
            {
                int before_count = read_files.Count;
                foreach (var f in read_files)
                {
                    var additional = AntlrParserDetails._dependent_grammars.Where(
                        t => t.Value.Contains(f)).Select(
                        t => t.Key).ToList();
                    read_files = read_files.Union(additional).ToHashSet();
                }
                int after_count = read_files.Count;
                if (after_count == before_count)
                {
                    break;
                }
            }
            Dictionary<TerminalNodeImpl, string> rewrites = new Dictionary<TerminalNodeImpl, string>();
            foreach (string f in read_files)
            {
                Workspaces.Document lexer_document = Workspaces.Workspace.Instance.FindDocument(f);
                if (document == null)
                {
                    continue;
                }
                AntlrParserDetails pd_lexer = ParserDetailsFactory.Create(lexer_document) as AntlrParserDetails;
                // Find all literals in lexer grammar.
                LiteralsParser lp_lexer = new LiteralsParser(pd_lexer);
                ParseTreeWalker.Default.Walk(lp_lexer, pd_lexer.ParseTree);
                if (! lp_lexer.IsLexer)
                {
                    continue;
                }
                List<TerminalNodeImpl> list_lexer = lp_lexer.Literals;
                // Find equivalent lexer rules.
                foreach (var literal in list)
                {
                    foreach (var lexer_literal in list_lexer)
                    {
                        if (literal.GetText() == lexer_literal.GetText())
                        {
                            var rewritten_literal = lexer_literal;
                            // Given candidate, walk up tree to find lexer_rule.
                            /*
                                ( ruleSpec
                                  ( lexerRuleSpec
                                    ( OFF_CHANNEL text=\r\n\r\n
                                    )
                                    ( OFF_CHANNEL text=...
                                    )
                                    (OFF_CHANNEL text =\r\n\r\n
                                    )
                                    (OFF_CHANNEL text =...
                                    )
                                    (OFF_CHANNEL text =\r\n\r\n
                                    )
                                    (DEFAULT_TOKEN_CHANNEL i = 995 txt = NONASSOC tt = 1
                                    )
                                    (OFF_CHANNEL text =\r\n\t
                                    )
                                    (DEFAULT_TOKEN_CHANNEL i = 997 txt =: tt = 29
                                    )
                                    (lexerRuleBlock
                                      (lexerAltList
                                        (lexerAlt
                                          (lexerElements
                                            (lexerElement
                                              (lexerAtom
                                                (terminal
                                                  (OFF_CHANNEL text =
                                                  )
                                                  (DEFAULT_TOKEN_CHANNEL i = 999 txt = '%binary' tt = 8
                                    ))))))))
                                    (OFF_CHANNEL text =\r\n\t
                                    )
                                    (DEFAULT_TOKEN_CHANNEL i = 1001 txt =; tt = 32
                                ) ) )

                             * Make sure it fits the structure of the tree shown above.
                             * 
                             */
                            var p1 = rewritten_literal.Parent;
                            if (p1.ChildCount != 1) continue;
                            if (!(p1 is ANTLRv4Parser.TerminalContext)) continue;
                            var p2 = p1.Parent;
                            if (p2.ChildCount != 1) continue;
                            if (!(p2 is ANTLRv4Parser.LexerAtomContext)) continue;
                            var p3 = p2.Parent;
                            if (p3.ChildCount != 1) continue;
                            if (!(p3 is ANTLRv4Parser.LexerElementContext)) continue;
                            var p4 = p3.Parent;
                            if (p4.ChildCount != 1) continue;
                            if (!(p4 is ANTLRv4Parser.LexerElementsContext)) continue;
                            var p5 = p4.Parent;
                            if (p5.ChildCount != 1) continue;
                            if (!(p5 is ANTLRv4Parser.LexerAltContext)) continue;
                            var p6 = p5.Parent;
                            if (p6.ChildCount != 1) continue;
                            if (!(p6 is ANTLRv4Parser.LexerAltListContext)) continue;
                            var p7 = p6.Parent;
                            if (p7.ChildCount != 1) continue;
                            if (!(p7 is ANTLRv4Parser.LexerRuleBlockContext)) continue;
                            var p8 = p7.Parent;
                            if (p8.ChildCount != 4) continue;
                            if (!(p8 is ANTLRv4Parser.LexerRuleSpecContext)) continue;

                            var alt = p8.GetChild(0);
                            var new_name = alt.GetText();
                            rewrites.Add(literal, new_name);
                        }
                    }
                }
            }

            Dictionary<string, string> result = new Dictionary<string, string>();
            var files = rewrites.Select(r => r.Key.Payload.TokenSource.SourceName).OrderBy(q => q).Distinct();
            var documents = files.Select(f => { return Workspaces.Workspace.Instance.FindDocument(f); }).ToList();
            foreach (Document f in documents)
            {
                string fn = f.FullPath;
                var per_file_changes = rewrites.Where(z => z.Key.Payload.TokenSource.SourceName == f.FullPath)
                    .OrderBy(z => z.Key.Payload.TokenIndex).ToList();
                StringBuilder sb = new StringBuilder();
                int previous = 0;
                string code = f.Code;
                foreach (var l in per_file_changes)
                {
                    string original_text = l.Key.Payload.Text;
                    int index_start = l.Key.Payload.StartIndex;
                    int len = l.Key.Payload.Text.Length;
                    string new_text = l.Value;
                    string pre = code.Substring(previous, index_start - previous);
                    sb.Append(pre);
                    sb.Append(new_text);
                    previous = index_start + len;
                }
                string rest = code.Substring(previous);
                sb.Append(rest);
                string new_code = sb.ToString();
                result.Add(fn, new_code);
            }
            return result;
        }
    }
}
