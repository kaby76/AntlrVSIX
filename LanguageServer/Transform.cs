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
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using ISymbol = Symtab.ISymbol;
    using Document = Workspaces.Document;

    public class Transform
    {
        private class IsParser : ANTLRv4ParserBaseVisitor<bool>
        {
            bool found = false;

            public IsParser()
            {
            }

            protected override bool ShouldVisitNextChild(IRuleNode node, bool currentResult)
            {
                return ! found;
            }

            public override bool VisitGrammarType([NotNull] ANTLRv4Parser.GrammarTypeContext context)
            {
                bool IsLexer;
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
                found = true;
                return !IsLexer;
            }
        }

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

        private class FindFirstRule : ANTLRv4ParserBaseVisitor<IParseTree>
        {
            bool found = false;

            public FindFirstRule()
            {
            }

            protected override bool ShouldVisitNextChild(IRuleNode node, IParseTree currentResult)
            {
                return !found;
            }

            public override IParseTree VisitRules([NotNull] ANTLRv4Parser.RulesContext context)
            {
                ANTLRv4Parser.RuleSpecContext[] rule_spec = context.ruleSpec();
                if (rule_spec == null) return null;
                var parser_rule_spec = rule_spec[0].parserRuleSpec();
                found = true;
                return parser_rule_spec;
            }
        }

        private class ExtractProductions : ANTLRv4ParserBaseListener
        {
            public List<ITerminalNode> Nonterminals = new List<ITerminalNode>();
            public Dictionary<ITerminalNode, List<ITerminalNode>> RhsReferences = new Dictionary<ITerminalNode, List<ITerminalNode>>();
            private ITerminalNode current_nonterminal;

            public override void EnterParserRuleSpec([NotNull] ANTLRv4Parser.ParserRuleSpecContext context)
            {
                ITerminalNode rule_ref = context.RULE_REF();
                Nonterminals.Add(rule_ref);
                current_nonterminal = rule_ref;
                RhsReferences[current_nonterminal] = new List<ITerminalNode>();
            }

            public override void EnterRuleref([NotNull] ANTLRv4Parser.RulerefContext context)
            {
                RhsReferences[current_nonterminal].Add(context.GetChild(0) as ITerminalNode);
            }
        }

        private class FindCalls : CSharpSyntaxWalker
        {
            public List<string> Invocations = new List<string>();

            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                Invocations.Add(node.ToString());
                base.VisitInvocationExpression(node);
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

        public static Dictionary<string, string> RemoveUselessParserProductions(int pos, Document document)
        {
            var result = new Dictionary<string, string>();
            AntlrParserDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrParserDetails;
            IsParser lp = new IsParser();
            var is_parser = lp.Visit(pd_parser.ParseTree);
            if (! is_parser)
            {
                return result;
            }

            // 1) Get non-terminals.
            // 2) Perform topological sort of uses.
            // 3) Get all non-terminals that do not have uses.
            // 4) Parse all .cs files in directory.
            // 5) Find all method calls to parser.
            // 6) Eliminate all unused non-terminals, and transitive closure of additional unused rules.
            // 7) Return new code.

            // 1
            ExtractProductions listener = new ExtractProductions();
            ParseTreeWalker.Default.Walk(listener, pd_parser.ParseTree);
            List<ITerminalNode> nonterminals = listener.Nonterminals;

            // 2
            var nonterminal_symbols = new Dictionary<ITerminalNode, ISymbol>();
            var rules = new Dictionary<ISymbol, List<ISymbol>>();
            nonterminals.ForEach(t =>
            {
                var nt = pd_parser.RootScope.LookupType(t.GetText()).First() as ISymbol;
                nonterminal_symbols.Add(t, nt);
                rules[nt] = new List<ISymbol>();
                listener.RhsReferences[t].ForEach(q =>
                {
                    var rhs = pd_parser.RootScope.LookupType(q.GetText()).First();
                    rules[nt].Add(rhs);
                });
            });

            // 3
            var has_use = new Dictionary<ISymbol, bool>();
            foreach (var r in rules)
            {
                has_use[r.Key] = false;
            }
            foreach (var r in rules)
            {
                r.Value.ForEach(x => has_use[x] = true);
            }

            // 4
            // Parse all the C# files in the current directory.
            string g4_file_path = document.FullPath;
            string current_dir = Path.GetDirectoryName(g4_file_path);
            if (current_dir == null)
            {
                return null;
            }

            Dictionary<string, SyntaxTree> trees = new Dictionary<string, SyntaxTree>();
            foreach (string f in Directory.EnumerateFiles(current_dir))
            {
                if (Path.GetExtension(f).ToLower() != ".cs")
                {
                    continue;
                }

                string file_name = f;
                string suffix = Path.GetExtension(file_name);
                if (suffix != ".cs")
                {
                    continue;
                }

                try
                {
                    string ffn = file_name;
                    StreamReader sr = new StreamReader(ffn);
                    string code = sr.ReadToEnd();
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
                    trees[ffn] = tree;
                }
                catch (Exception)
                {
                }
            }

            // 5
            try
            {
                foreach (KeyValuePair<string, SyntaxTree> kvp in trees)
                {
                    string file_name = kvp.Key;
                    SyntaxTree tree = kvp.Value;
                    CompilationUnitSyntax root = (CompilationUnitSyntax)tree.GetRoot();
                    if (root == null)
                    {
                        continue;
                    }
                    var syntax_walker = new FindCalls();
                    syntax_walker.Visit(root);
                    foreach (var nt in nonterminals)
                    {
                        var nt_name = nt.GetText();
                        var call = "." + nt_name + "()";
                        foreach (var i in syntax_walker.Invocations)
                        {
                            if (i.Contains(call))
                            {
                                has_use[nonterminal_symbols[nt]] = true;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            // 6
            List<Pair<int, int>> deletions = new List<Pair<int, int>>();
            foreach (var s in has_use)
            {
                if (s.Value == false)
                {
                    // s.Key is not used.
                    // Find range of code to eliminate.
                    var token = nonterminal_symbols.Where(t => t.Value == s.Key).First();
                    IParseTree p = token.Key;
                    for (; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.RuleSpecContext)
                            break;
                    }
                    if (p == null) continue;
                    var token_interval = p.SourceInterval;
                    var start = p.SourceInterval.a;
                    var end = p.SourceInterval.b;
                    var start_tok = pd_parser.TokStream.Get(start);
                    var end_tok = pd_parser.TokStream.Get(end);
                    var start_ind = start_tok.StartIndex;
                    var end_ind = end_tok.StopIndex + 1;
                    deletions.Add(new Pair<int, int>(start_ind, end_ind));
                }
            }
            deletions = deletions.OrderBy(p => p.a).ThenBy(p => p.b).ToList();
            StringBuilder sb = new StringBuilder();
            int previous = 0;
            string old_code = document.Code;
            foreach (var l in deletions)
            {
                int index_start = l.a;
                int len = l.b - l.a;
                string pre = old_code.Substring(previous, index_start - previous);
                sb.Append(pre);
                previous = index_start + len;
            }
            string rest = old_code.Substring(previous);
            sb.Append(rest);
            string new_code = sb.ToString();
            result.Add(document.FullPath, new_code);

            return result;
        }

        public static Dictionary<string, string> MoveStartRuleToTop(int pos, Document document)
        {
            var result = new Dictionary<string, string>();
            AntlrParserDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrParserDetails;
            IsParser lp = new IsParser();
            var is_parser = lp.Visit(pd_parser.ParseTree);
            if (!is_parser)
            {
                return result;
            }

            // 1) Get non-terminals.
            // 2) Perform topological sort of uses.
            // 3) Get all non-terminals that do not have uses.
            // 4) Parse all .cs files in directory.
            // 5) Find all method calls to parser.
            // 6) Move start rule to top.
            // 7) Return new code.

            // 1
            ExtractProductions listener = new ExtractProductions();
            ParseTreeWalker.Default.Walk(listener, pd_parser.ParseTree);
            List<ITerminalNode> nonterminals = listener.Nonterminals;

            // 2
            var nonterminal_symbols = new Dictionary<ITerminalNode, ISymbol>();
            var rules = new Dictionary<ISymbol, List<ISymbol>>();
            nonterminals.ForEach(t =>
            {
                var nt = pd_parser.RootScope.LookupType(t.GetText()).First() as ISymbol;
                nonterminal_symbols.Add(t, nt);
                rules[nt] = new List<ISymbol>();
                listener.RhsReferences[t].ForEach(q =>
                {
                    var rhs = pd_parser.RootScope.LookupType(q.GetText()).First();
                    rules[nt].Add(rhs);
                });
            });

            // 3
            var is_start_rule = new Dictionary<ISymbol, bool>();
            foreach (var r in rules)
            {
                is_start_rule[r.Key] = false;
            }

            // 4
            // Parse all the C# files in the current directory.
            string g4_file_path = document.FullPath;
            string current_dir = Path.GetDirectoryName(g4_file_path);
            if (current_dir == null)
            {
                return null;
            }

            Dictionary<string, SyntaxTree> trees = new Dictionary<string, SyntaxTree>();
            foreach (string f in Directory.EnumerateFiles(current_dir))
            {
                if (Path.GetExtension(f).ToLower() != ".cs")
                {
                    continue;
                }

                string file_name = f;
                string suffix = Path.GetExtension(file_name);
                if (suffix != ".cs")
                {
                    continue;
                }

                try
                {
                    string ffn = file_name;
                    StreamReader sr = new StreamReader(ffn);
                    string code = sr.ReadToEnd();
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
                    trees[ffn] = tree;
                }
                catch (Exception)
                {
                }
            }

            // 5
            try
            {
                foreach (KeyValuePair<string, SyntaxTree> kvp in trees)
                {
                    string file_name = kvp.Key;
                    SyntaxTree tree = kvp.Value;
                    CompilationUnitSyntax root = (CompilationUnitSyntax)tree.GetRoot();
                    if (root == null)
                    {
                        continue;
                    }
                    var syntax_walker = new FindCalls();
                    syntax_walker.Visit(root);
                    foreach (var nt in nonterminals)
                    {
                        var nt_name = nt.GetText();
                        var call = "." + nt_name + "()";
                        foreach (var i in syntax_walker.Invocations)
                        {
                            if (i.Contains(call))
                            {
                                is_start_rule[nonterminal_symbols[nt]] = true;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            // 6
            string old_code = document.Code;
            List<Pair<int, int>> deletions = new List<Pair<int, int>>();
            foreach (var s in is_start_rule)
            {
                if (s.Value == true)
                {
                    // s.Key is start rule.
                    // Find range of code to delete, and where to insert.
                    var token = nonterminal_symbols.Where(t => t.Value == s.Key).First();
                    IParseTree p = token.Key;
                    for (; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.RuleSpecContext)
                            break;
                    }
                    if (p == null) continue;
                    var token_interval = p.SourceInterval;
                    var start = p.SourceInterval.a;
                    var end = p.SourceInterval.b;
                    var start_tok = pd_parser.TokStream.Get(start);
                    var end_tok = pd_parser.TokStream.Get(end);
                    // Move forward to include white space.
                    var inter = pd_parser.TokStream.GetHiddenTokensToRight(end_tok.TokenIndex);
                    Antlr4.Runtime.IToken last = end_tok;
                    if (inter != null)
                    {
                        foreach (Antlr4.Runtime.IToken i in inter)
                        {
                            if (i.Channel == ANTLRv4Lexer.OFF_CHANNEL)
                            {
                                last = i;
                                break;
                            }
                        }
                    }
                    var end_ind = last.StopIndex + 1;
                    // Back up to beginning of line. We don't want partial lines.
                    for (int j = end_ind; ; j--)
                    {
                        if (old_code[j] == '\n' || old_code[j] == '\r')
                        {
                            end_ind = j + 1;
                            break;
                        }
                    }
                    var start_ind = start_tok.StartIndex;
                    deletions.Add(new Pair<int, int>(start_ind, end_ind));
                }
            }
            deletions = deletions.OrderBy(p => p.a).ThenBy(p => p.b).ToList();

            var find_first_rule = new FindFirstRule();
            var first_rule = find_first_rule.Visit(pd_parser.ParseTree);
            if (first_rule == null) return result;

            var insertion = first_rule.SourceInterval.a;
            var insertion_tok = pd_parser.TokStream.Get(insertion);
            var insertion_ind = insertion_tok.StartIndex;

            if (deletions.Count == 1 && deletions[0].a == insertion_ind)
            {
                return result;
            }

            StringBuilder sb = new StringBuilder();
            int previous = 0;
            {
                int index_start = insertion_ind;
                int len = 0;
                string pre = old_code.Substring(previous, index_start - previous);
                sb.Append(pre);
                previous = index_start + len;
            }
            foreach (var l in deletions)
            {
                int index_start = l.a;
                int len = l.b - l.a;
                string add = old_code.Substring(index_start, len);
                sb.Append(add);
            }
            foreach (var l in deletions)
            {
                int index_start = l.a;
                int len = l.b - l.a;
                string pre = old_code.Substring(previous, index_start - previous);
                sb.Append(pre);
                previous = index_start + len;
            }
            string rest = old_code.Substring(previous);
            sb.Append(rest);
            string new_code = sb.ToString();
            result.Add(document.FullPath, new_code);

            return result;
        }

        public static Dictionary<string, string> ReorderParserRules(int pos, Document document, LspAntlr.ReorderType type)
        {
            var result = new Dictionary<string, string>();
            AntlrParserDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrParserDetails;
            IsParser lp = new IsParser();
            var is_parser = lp.Visit(pd_parser.ParseTree);
            if (!is_parser)
            {
                return result;
            }

            // 1) Get non-terminals.
            // 2) Get all rules.
            // 3) Parse all .cs files in directory.
            // 4) Find all method calls to parser.
            // 5) Get start symbols.
            // 6) Perform topological sort of uses.
            // 7) Move rules.
            // 8) Return new code.

            // 1
            ExtractProductions listener = new ExtractProductions();
            ParseTreeWalker.Default.Walk(listener, pd_parser.ParseTree);
            List<ITerminalNode> nonterminals = listener.Nonterminals;

            // 2
            var nonterminal_symbols = new Dictionary<ITerminalNode, ISymbol>();
            var rules = new Dictionary<ISymbol, List<ISymbol>>();
            nonterminals.ForEach(t =>
            {
                var nt = pd_parser.RootScope.LookupType(t.GetText()).First() as ISymbol;
                nonterminal_symbols.Add(t, nt);
                rules[nt] = new List<ISymbol>();
                listener.RhsReferences[t].ForEach(q =>
                {
                    var rhs = pd_parser.RootScope.LookupType(q.GetText()).First();
                    rules[nt].Add(rhs);
                });
            });
            var is_start_rule = new Dictionary<ISymbol, bool>();
            foreach (var r in rules)
            {
                is_start_rule[r.Key] = false;
            }

            // 3
            string g4_file_path = document.FullPath;
            string current_dir = Path.GetDirectoryName(g4_file_path);
            if (current_dir == null)
            {
                return null;
            }
            Dictionary<string, SyntaxTree> trees = new Dictionary<string, SyntaxTree>();
            foreach (string f in Directory.EnumerateFiles(current_dir))
            {
                if (Path.GetExtension(f).ToLower() != ".cs")
                {
                    continue;
                }

                string file_name = f;
                string suffix = Path.GetExtension(file_name);
                if (suffix != ".cs")
                {
                    continue;
                }

                try
                {
                    string ffn = file_name;
                    StreamReader sr = new StreamReader(ffn);
                    string code = sr.ReadToEnd();
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
                    trees[ffn] = tree;
                }
                catch (Exception)
                {
                }
            }

            // 4
            try
            {
                foreach (KeyValuePair<string, SyntaxTree> kvp in trees)
                {
                    string file_name = kvp.Key;
                    SyntaxTree tree = kvp.Value;
                    CompilationUnitSyntax root = (CompilationUnitSyntax)tree.GetRoot();
                    if (root == null)
                    {
                        continue;
                    }
                    var syntax_walker = new FindCalls();
                    syntax_walker.Visit(root);
                    foreach (var nt in nonterminals)
                    {
                        var nt_name = nt.GetText();
                        var call = "." + nt_name + "()";
                        foreach (var i in syntax_walker.Invocations)
                        {
                            if (i.Contains(call))
                            {
                                is_start_rule[nonterminal_symbols[nt]] = true;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            // 5
            string old_code = document.Code;
            List<Pair<int, int>> additions = new List<Pair<int, int>>();
            if (type == LspAntlr.ReorderType.DFS)
            {
                Digraph<string> graph = new Digraph<string>();
                foreach (var r in rules)
                {
                    graph.AddVertex(r.Key.Name);
                }
                foreach (var r in rules)
                {
                    var j = r.Value.ToList();
                    j.Reverse();
                    foreach (var rhs in j)
                    {
                        var e = new DirectedEdge<string>(r.Key.Name, rhs.Name);
                        graph.AddEdge(e);
                    }
                }
                List<string> starts = new List<string>();
                foreach (var p in is_start_rule)
                {
                    starts.Add(p.Key.Name);
                }
                TarjanNoBackEdges<string, DirectedEdge<string>> sort = new TarjanNoBackEdges<string, DirectedEdge<string>>(graph, starts);
                var ordered = sort.Reverse().ToList();

                // 6
                List<Pair<int, int>> new_order = new List<Pair<int, int>>();
                foreach (var s in ordered)
                {
                    // Find range of code to copy, and where to insert.
                    var token = nonterminal_symbols.Where(t => t.Value.Name == s).First();
                    IParseTree p = token.Key;
                    for (; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.RuleSpecContext)
                            break;
                    }
                    if (p == null) continue;
                    var token_interval = p.SourceInterval;
                    var start = p.SourceInterval.a;
                    var end = p.SourceInterval.b;
                    var start_tok = pd_parser.TokStream.Get(start);
                    var end_tok = pd_parser.TokStream.Get(end);
                    // Move forward to include white space.
                    var inter = pd_parser.TokStream.GetHiddenTokensToRight(end_tok.TokenIndex);
                    Antlr4.Runtime.IToken last = end_tok;
                    if (inter != null)
                    {
                        foreach (Antlr4.Runtime.IToken i in inter)
                        {
                            if (i.Channel == ANTLRv4Lexer.OFF_CHANNEL)
                            {
                                last = i;
                                break;
                            }
                        }
                    }
                    var end_ind = last.StopIndex + 1;
                    // Back up to beginning of line. We don't want partial lines.
                    for (int j = end_ind; ; j--)
                    {
                        if (old_code.Length <= j)
                        {
                            end_ind = j - 1;
                            break;
                        }
                        if (old_code[j] == '\n' || old_code[j] == '\r')
                        {
                            end_ind = j + 1;
                            break;
                        }
                    }
                    var start_ind = start_tok.StartIndex;
                    additions.Add(new Pair<int, int>(start_ind, end_ind));
                }

            }
            else if (type == LspAntlr.ReorderType.BFS)
            {
            }
            else if (type == LspAntlr.ReorderType.Alphabetically)
            {
            }
            else
            {
                return result;
            }

            var find_first_rule = new FindFirstRule();
            var first_rule = find_first_rule.Visit(pd_parser.ParseTree);
            if (first_rule == null) return result;

            var insertion = first_rule.SourceInterval.a;
            var insertion_tok = pd_parser.TokStream.Get(insertion);
            var insertion_ind = insertion_tok.StartIndex;

            StringBuilder sb = new StringBuilder();
            int previous = 0;
            {
                int index_start = insertion_ind;
                int len = 0;
                string pre = old_code.Substring(previous, index_start - previous);
                sb.Append(pre);
                previous = index_start + len;
            }
            foreach (var l in additions)
            {
                int index_start = l.a;
                int len = l.b - l.a;
                string add = old_code.Substring(index_start, len);
                sb.Append(add);
            }
            //string rest = old_code.Substring(previous);
            //sb.Append(rest);
            string new_code = sb.ToString();
            result.Add(document.FullPath, new_code);

            return result;
        }
    }
}
