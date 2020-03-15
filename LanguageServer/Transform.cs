namespace LanguageServer
{
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Graphs;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Document = Workspaces.Document;
    using ISymbol = Symtab.ISymbol;

    public class Transform
    {

        private class ExtractGrammarType : ANTLRv4ParserBaseListener
        {
            public enum GrammarType
            {
                Combined,
                Parser,
                Lexer
            }

            public GrammarType Type;

            public ExtractGrammarType()
            {
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

        private class FindFirstRule : ANTLRv4ParserBaseListener
        {
            public IParseTree First = null;
            public IParseTree Last = null;

            public FindFirstRule() { }

            public override void EnterRules([NotNull] ANTLRv4Parser.RulesContext context)
            {
                ANTLRv4Parser.RuleSpecContext[] rule_spec = context.ruleSpec();
                if (rule_spec == null) return;
                First = rule_spec[0].parserRuleSpec();
            }
        }

        private class ExtractRules : ANTLRv4ParserBaseListener
        {
            public List<IParseTree> Rules = new List<IParseTree>();
            public List<ITerminalNode> LHS = new List<ITerminalNode>();
            public Dictionary<ITerminalNode, List<ITerminalNode>> RHS = new Dictionary<ITerminalNode, List<ITerminalNode>>();
            private ITerminalNode current_nonterminal;

            public override void EnterParserRuleSpec([NotNull] ANTLRv4Parser.ParserRuleSpecContext context)
            {
                Rules.Add(context);
                ITerminalNode rule_ref = context.RULE_REF();
                LHS.Add(rule_ref);
                current_nonterminal = rule_ref;
                RHS[current_nonterminal] = new List<ITerminalNode>();
            }

            public override void EnterRuleref([NotNull] ANTLRv4Parser.RulerefContext context)
            {
                RHS[current_nonterminal].Add(context.GetChild(0) as ITerminalNode);
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

        private static Dictionary<string, SyntaxTree> ReadCsharpSource(Document document)
        {
            Dictionary<string, SyntaxTree> trees = new Dictionary<string, SyntaxTree>();
            string g4_file_path = document.FullPath;
            string current_dir = Path.GetDirectoryName(g4_file_path);
            if (current_dir == null)
            {
                return trees;
            }
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
            return trees;
        }
        
        private class Table
        {
            public class Row
            {
                public IParseTree rule;
                public string LHS;
                public List<string> RHS;
                public int start_index;
                public int end_index;
                public bool is_start;
                public bool is_used;
            }

            public List<Row> rules = new List<Row>();
            public Dictionary<string, int> nt_to_index = new Dictionary<string, int>();
            ExtractRules listener;
            AntlrParserDetails pd_parser;
            Document document;
            Dictionary<string, SyntaxTree> trees;

            public Table(AntlrParserDetails p, Document d)
            {
                pd_parser = p;
                document = d;
                trees = ReadCsharpSource(document);
            }

            public void ReadRules()
            {
                // Get rules, lhs, rhs.
                listener = new ExtractRules();
                ParseTreeWalker.Default.Walk(listener, pd_parser.ParseTree);
                List<ITerminalNode> nonterminals = listener.LHS;
                Dictionary<ITerminalNode, List<ITerminalNode>> rhs = listener.RHS;
                for (int i = 0; i < listener.Rules.Count; ++i)
                {
                    this.rules.Add(new Row()
                    {
                        rule = listener.Rules[i],
                        LHS = nonterminals[i].GetText(),
                        RHS = rhs[nonterminals[i]].Select(t => t.GetText()).ToList(),
                    });
                }
                for (int i = 0; i < rules.Count; ++i)
                {
                    string t = rules[i].LHS;
                    nt_to_index[t] = i;
                }
            }

            public void FindPartitions()
            {
                var find_first_rule = new FindFirstRule();
                ParseTreeWalker.Default.Walk(find_first_rule, pd_parser.ParseTree);
                var first_rule = find_first_rule.First;
                if (first_rule == null) return;

                var insertion = first_rule.SourceInterval.a;
                var insertion_tok = pd_parser.TokStream.Get(insertion);
                var insertion_ind = insertion_tok.StartIndex;
                string old_code = document.Code;
                for (int i = 0; i < rules.Count; ++i)
                {
                    IParseTree rule = rules[i].rule;
                    // Find range indices for rule including comments. Note, start index is inclusive; end
                    // index is exclusive. We make the assumption
                    // that the preceeding whitespace and comments are grouped with a rule all the way
                    // from the end a previous non-whitespace or comment, such as options, headers, or rule.
                    Interval token_interval = rule.SourceInterval;
                    var end = token_interval.b;
                    var end_tok = pd_parser.TokStream.Get(end);
                    Antlr4.Runtime.IToken last = end_tok;
                    var end_ind = old_code.Length <= last.StopIndex ? last.StopIndex : last.StopIndex + 1;
                    bool on_end = false;
                    for (int j = end_ind; j < old_code.Length; j++)
                    {
                        if (old_code[j] == '\r')
                        {
                            if (j + 1 < old_code.Length && old_code[j + 1] == '\n')
                                end_ind = j + 2;
                            else
                                end_ind = j + 1;
                            break;
                        }
                        end_ind = j;
                    }
                    var inter = pd_parser.TokStream.GetHiddenTokensToRight(end_tok.TokenIndex);
                    var start = token_interval.a;
                    var start_tok = pd_parser.TokStream.Get(start);
                    var start_ind = start_tok.StartIndex;
                    rules[i].start_index = start_ind;
                    rules[i].end_index = end_ind;
                }
                for (int i = 0; i < rules.Count; ++i)
                {
                    if (i > 0)
                    {
                        rules[i].start_index = rules[i - 1].end_index;
                    }
                }
                bool bad = false;
                for (int i = 0; i < rules.Count; ++i)
                {
                    for (int j = rules[i].start_index; j < rules[i].end_index; ++j)
                    {
                        if (old_code[j] == '\r')
                        {
                            if (j + 1 < rules[i].end_index && old_code[j + 1] == '\n')
                                ;
                            else
                                bad = true;
                        }
                    }
                }
            }

            public void FindStartRules()
            {
                List<ITerminalNode> lhs = listener.LHS;
                for (int i = 0; i < rules.Count; ++i)
                {
                    for (int j = 0; j < rules[i].RHS.Count; ++j)
                    {
                        rules[nt_to_index[rules[i].RHS[j]]].is_used = true;
                    }
                }
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
                        for (int i = 0; i < rules.Count; ++i)
                        {
                            var nt_name = rules[i].LHS;
                            var call = "." + nt_name + "()";
                            foreach (var j in syntax_walker.Invocations)
                            {
                                if (j.Contains(call))
                                {
                                    rules[i].is_used = true;
                                    rules[i].is_start = true;
                                }
                            }
                        }
                    }
                }
                catch
                {
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

        public static Dictionary<string, string> RemoveUselessParserProductions(int pos, Document document)
        {
            var result = new Dictionary<string, string>();

            // Check if lexer grammar.
            AntlrParserDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrParserDetails;
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            var is_lexer = lp.Type == ExtractGrammarType.GrammarType.Lexer;
            if (is_lexer)
            {
                return result;
            }

            Table table = new Table(pd_parser, document);
            table.ReadRules();
            table.FindPartitions();
            table.FindStartRules();

            List<Pair<int, int>> deletions = new List<Pair<int, int>>();
            foreach (var r in table.rules)
            {
                if (r.is_used == false)
                {
                    deletions.Add(new Pair<int, int>(r.start_index, r.end_index));
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

            // Check if lexer grammar.
            AntlrParserDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrParserDetails;
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            var is_lexer = lp.Type == ExtractGrammarType.GrammarType.Lexer;
            if (is_lexer)
            {
                return result;
            }

            Table table = new Table(pd_parser, document);
            table.ReadRules();
            table.FindPartitions();
            table.FindStartRules();

            string old_code = document.Code;
            List<Pair<int, int>> move = new List<Pair<int, int>>();
            foreach (var r in table.rules)
            {
                if (r.is_start == true)
                {
                    move.Add(new Pair<int, int>(r.start_index, r.end_index));
                }
            }
            move = move.OrderBy(p => p.a).ThenBy(p => p.b).ToList();

            var find_first_rule = new FindFirstRule();
            ParseTreeWalker.Default.Walk(find_first_rule, pd_parser.ParseTree);
            var first_rule = find_first_rule.First;
            if (first_rule == null) return result;
            var insertion = first_rule.SourceInterval.a;
            var insertion_tok = pd_parser.TokStream.Get(insertion);
            var insertion_ind = insertion_tok.StartIndex;
            if (move.Count == 1 && move[0].a == insertion_ind)
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
            foreach (var l in move)
            {
                int index_start = l.a;
                int len = l.b - l.a;
                string add = old_code.Substring(index_start, len);
                sb.Append(add);
            }
            foreach (var l in move)
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

            // Check if lexer grammar.
            AntlrParserDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrParserDetails;
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            var is_lexer = lp.Type == ExtractGrammarType.GrammarType.Lexer;
            if (is_lexer)
            {
                return result;
            }

            Table table = new Table(pd_parser, document);
            table.ReadRules();
            table.FindPartitions();
            table.FindStartRules();

            // Find new order or rules.
            string old_code = document.Code;
            List<Pair<int, int>> reorder = new List<Pair<int, int>>();
            if (type == LspAntlr.ReorderType.DFS)
            {
                Digraph<string> graph = new Digraph<string>();
                foreach (var r in table.rules)
                {
                    graph.AddVertex(r.LHS);
                }
                foreach (var r in table.rules)
                {
                    var j = r.RHS;
                    //j.Reverse();
                    foreach (var rhs in j)
                    {
                        var e = new DirectedEdge<string>(r.LHS, rhs);
                        graph.AddEdge(e);
                    }
                }
                List<string> starts = new List<string>();
                foreach (var r in table.rules)
                {
                    if (r.is_start) starts.Add(r.LHS);
                }
                Graphs.DepthFirstOrder<string, DirectedEdge<string>> sort = new DepthFirstOrder<string, DirectedEdge<string>>(graph, starts);
                var ordered = sort.ToList();
                foreach (var s in ordered)
                {
                    var row = table.rules[table.nt_to_index[s]];
                    reorder.Add(new Pair<int, int>(row.start_index, row.end_index));
                }
            }
            else if (type == LspAntlr.ReorderType.BFS)
            {
                Digraph<string> graph = new Digraph<string>();
                foreach (var r in table.rules)
                {
                    graph.AddVertex(r.LHS);
                }
                foreach (var r in table.rules)
                {
                    var j = r.RHS;
                    //j.Reverse();
                    foreach (var rhs in j)
                    {
                        var e = new DirectedEdge<string>(r.LHS, rhs);
                        graph.AddEdge(e);
                    }
                }
                List<string> starts = new List<string>();
                foreach (var r in table.rules)
                {
                    if (r.is_start) starts.Add(r.LHS);
                }
                Graphs.BreadthFirstOrder<string, DirectedEdge<string>> sort = new BreadthFirstOrder<string, DirectedEdge<string>>(graph, starts);
                var ordered = sort.ToList();
                foreach (var s in ordered)
                {
                    var row = table.rules[table.nt_to_index[s]];
                    reorder.Add(new Pair<int, int>(row.start_index, row.end_index));
                }
            }
            else if (type == LspAntlr.ReorderType.Alphabetically)
            {
                var ordered = table.rules
                    .Select(r => r.LHS)
                    .OrderBy(r => r).ToList();
                foreach (var s in ordered)
                {
                    var row = table.rules[table.nt_to_index[s]];
                    reorder.Add(new Pair<int, int>(row.start_index, row.end_index));
                }
            }
            else
            {
                return result;
            }

            StringBuilder sb = new StringBuilder();
            int previous = 0;
            {
                int index_start = table.rules[0].start_index;
                int len = 0;
                string pre = old_code.Substring(previous, index_start - previous);
                sb.Append(pre);
                previous = index_start + len;
            }
            foreach (var l in reorder)
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

        public static Dictionary<string, string> SplitCombineGrammars(int pos, Document document, bool split)
        {
            var result = new Dictionary<string, string>();

            // Check if lexer grammar.
            AntlrParserDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrParserDetails;
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            var is_lexer = lp.Type == ExtractGrammarType.GrammarType.Lexer;
            if (is_lexer)
            {
                return result;
            }

            Table table = new Table(pd_parser, document);
            table.ReadRules();
            table.FindPartitions();
            table.FindStartRules();

            // Find new order or rules.
            string old_code = document.Code;
            List<Pair<int, int>> reorder = new List<Pair<int, int>>();
            if (split)
            {
                Digraph<string> graph = new Digraph<string>();
                foreach (var r in table.rules)
                {
                    graph.AddVertex(r.LHS);
                }
                foreach (var r in table.rules)
                {
                    var j = r.RHS;
                    //j.Reverse();
                    foreach (var rhs in j)
                    {
                        var e = new DirectedEdge<string>(r.LHS, rhs);
                        graph.AddEdge(e);
                    }
                }
                List<string> starts = new List<string>();
                foreach (var r in table.rules)
                {
                    if (r.is_start) starts.Add(r.LHS);
                }
                Graphs.DepthFirstOrder<string, DirectedEdge<string>> sort = new DepthFirstOrder<string, DirectedEdge<string>>(graph, starts);
                var ordered = sort.ToList();
                foreach (var s in ordered)
                {
                    var row = table.rules[table.nt_to_index[s]];
                    reorder.Add(new Pair<int, int>(row.start_index, row.end_index));
                }
            }
            else
            {
                Digraph<string> graph = new Digraph<string>();
                foreach (var r in table.rules)
                {
                    graph.AddVertex(r.LHS);
                }
                foreach (var r in table.rules)
                {
                    var j = r.RHS;
                    //j.Reverse();
                    foreach (var rhs in j)
                    {
                        var e = new DirectedEdge<string>(r.LHS, rhs);
                        graph.AddEdge(e);
                    }
                }
                List<string> starts = new List<string>();
                foreach (var r in table.rules)
                {
                    if (r.is_start) starts.Add(r.LHS);
                }
                Graphs.BreadthFirstOrder<string, DirectedEdge<string>> sort = new BreadthFirstOrder<string, DirectedEdge<string>>(graph, starts);
                var ordered = sort.ToList();
                foreach (var s in ordered)
                {
                    var row = table.rules[table.nt_to_index[s]];
                    reorder.Add(new Pair<int, int>(row.start_index, row.end_index));
                }
            }

            StringBuilder sb = new StringBuilder();
            int previous = 0;
            {
                int index_start = table.rules[0].start_index;
                int len = 0;
                string pre = old_code.Substring(previous, index_start - previous);
                sb.Append(pre);
                previous = index_start + len;
            }
            foreach (var l in reorder)
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
