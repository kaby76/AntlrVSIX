namespace LanguageServer
{
    using Algorithms;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using GrammarGrammar;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Document = Workspaces.Document;

    public class Transform
    {
        public class ExtractGrammarType : ANTLRv4ParserBaseListener
        {
            public enum GrammarType
            {
                Combined,
                Parser,
                Lexer,
                NotAGrammar
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

        public class LiteralsGrammar : ANTLRv4ParserBaseListener
        {
            public List<TerminalNodeImpl> Literals = new List<TerminalNodeImpl>();

            public LiteralsGrammar()
            {
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

        public class FindFirstRule : ANTLRv4ParserBaseListener
        {
            public IParseTree First = null;
            public IParseTree Last = null;

            public FindFirstRule() { }

            public override void EnterRules([NotNull] ANTLRv4Parser.RulesContext context)
            {
                ANTLRv4Parser.RuleSpecContext[] rule_spec = context.ruleSpec();
                if (rule_spec == null)
                {
                    return;
                }

                First = rule_spec[0];
            }
        }

        public class FindFirstMode : ANTLRv4ParserBaseListener
        {
            public IParseTree First = null;
            public IParseTree Last = null;

            public FindFirstMode() { }


            public override void EnterModeSpec([NotNull] ANTLRv4Parser.ModeSpecContext context)
            {
                First = context;
            }
        }

        public class FindOptions : ANTLRv4ParserBaseListener
        {
            public IParseTree OptionsSpec = null;
            public List<IParseTree> Options = new List<IParseTree>();

            public override void EnterOption([NotNull] ANTLRv4Parser.OptionContext context)
            {
                Options.Add(context);
                base.EnterOption(context);
            }

            public override void EnterOptionsSpec([NotNull] ANTLRv4Parser.OptionsSpecContext context)
            {
                OptionsSpec = context;
                base.EnterOptionsSpec(context);
            }
        }

        public class ExtractRules : ANTLRv4ParserBaseListener
        {
            public List<ANTLRv4Parser.ParserRuleSpecContext> ParserRules = new List<ANTLRv4Parser.ParserRuleSpecContext>();
            public List<ANTLRv4Parser.LexerRuleSpecContext> LexerRules = new List<ANTLRv4Parser.LexerRuleSpecContext>();
            public List<IParseTree> Rules = new List<IParseTree>();
            public List<ITerminalNode> LhsSymbol = new List<ITerminalNode>();
            public Dictionary<ITerminalNode, List<ITerminalNode>> RhsSymbols = new Dictionary<ITerminalNode, List<ITerminalNode>>();
            private ITerminalNode current_nonterminal;

            public override void EnterParserRuleSpec([NotNull] ANTLRv4Parser.ParserRuleSpecContext context)
            {
                ParserRules.Add(context);
                Rules.Add(context);
                ITerminalNode rule_ref = context.RULE_REF();
                LhsSymbol.Add(rule_ref);
                current_nonterminal = rule_ref;
                RhsSymbols[current_nonterminal] = new List<ITerminalNode>();
            }

            public override void EnterLexerRuleSpec([NotNull] ANTLRv4Parser.LexerRuleSpecContext context)
            {
                LexerRules.Add(context);
                Rules.Add(context);
                ITerminalNode token_ref = context.TOKEN_REF();
                LhsSymbol.Add(token_ref);
                current_nonterminal = token_ref;
                RhsSymbols[current_nonterminal] = new List<ITerminalNode>();
            }

            public override void EnterRuleref([NotNull] ANTLRv4Parser.RulerefContext context)
            {
                RhsSymbols[current_nonterminal].Add(context.GetChild(0) as ITerminalNode);
            }
        }

        public class ExtractModes : ANTLRv4ParserBaseListener
        {
            public List<ANTLRv4Parser.ModeSpecContext> Modes = new List<ANTLRv4Parser.ModeSpecContext>();

            public override void EnterModeSpec([NotNull] ANTLRv4Parser.ModeSpecContext context)
            {
                Modes.Add(context);
            }
        }

        public class FindCalls : CSharpSyntaxWalker
        {
            public List<string> Invocations = new List<string>();

            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                var t = node.GetText().ToString();
                if (t.IndexOf("context.") == -1)
                {
                    Invocations.Add(t);
                }
                base.VisitInvocationExpression(node);
            }
        }

        public static Dictionary<string, SyntaxTree> ReadCsharpSource(Document document)
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

        public class TableOfRules
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
                public bool is_parser_rule;
            }

            public List<Row> rules = new List<Row>();
            public Dictionary<string, int> nt_to_index = new Dictionary<string, int>();
            public ExtractRules listener;
            private readonly AntlrGrammarDetails pd_parser;
            private readonly Document document;
            private readonly Dictionary<string, SyntaxTree> trees;

            public TableOfRules(AntlrGrammarDetails p, Document d)
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
                List<ITerminalNode> nonterminals = listener.LhsSymbol;
                Dictionary<ITerminalNode, List<ITerminalNode>> rhs = listener.RhsSymbols;
                for (int i = 0; i < listener.Rules.Count; ++i)
                {
                    rules.Add(new Row()
                    {
                        rule = listener.Rules[i],
                        LHS = nonterminals[i].GetText(),
                        is_parser_rule = char.IsLower(nonterminals[i].GetText()[0]),
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
                FindFirstRule find_first_rule = new FindFirstRule();
                ParseTreeWalker.Default.Walk(find_first_rule, pd_parser.ParseTree);
                IParseTree first_rule = find_first_rule.First;
                if (first_rule == null)
                {
                    return;
                }

                string old_code = document.Code;
                for (int i = 0; i < rules.Count; ++i)
                {
                    IParseTree rule = rules[i].rule;
                    // Find range indices for rule including comments. Note, start index is inclusive; end
                    // index is exclusive. We make the assumption
                    // that the preceeding whitespace and comments are grouped with a rule all the way
                    // from the end a previous non-whitespace or comment, such as options, headers, or rule.
                    Interval token_interval = rule.SourceInterval;
                    int end = token_interval.b;
                    Antlr4.Runtime.IToken end_tok = pd_parser.TokStream.Get(end);
                    Antlr4.Runtime.IToken last = end_tok;
                    int end_ind = old_code.Length <= last.StopIndex ? last.StopIndex : last.StopIndex + 1;
                    for (int j = end_ind; j < old_code.Length; j++)
                    {
                        if (old_code[j] == '\r')
                        {
                            if (j + 1 < old_code.Length && old_code[j + 1] == '\n')
                            {
                                end_ind = j + 2;
                            }
                            else
                            {
                                end_ind = j + 1;
                            }

                            break;
                        }
                        end_ind = j;
                    }
                    int start = token_interval.a;
                    Antlr4.Runtime.IToken start_tok = pd_parser.TokStream.Get(start);
                    int start_ind = start_tok.StartIndex;
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
                for (int i = 0; i < rules.Count; ++i)
                {
                    for (int j = rules[i].start_index; j < rules[i].end_index; ++j)
                    {
                        if (old_code[j] == '\r')
                        {
                            if (j + 1 < rules[i].end_index && old_code[j + 1] == '\n')
                            {
                                ;
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }

            public void FindModePartitions()
            {
                FindFirstMode find_first_mode = new FindFirstMode();
                ParseTreeWalker.Default.Walk(find_first_mode, pd_parser.ParseTree);
                IParseTree first_rule = find_first_mode.First;
                if (first_rule == null)
                {
                    return;
                }

                string old_code = document.Code;
                for (int i = 0; i < rules.Count; ++i)
                {
                    IParseTree rule = rules[i].rule;
                    // Find range indices for rule including comments. Note, start index is inclusive; end
                    // index is exclusive. We make the assumption
                    // that the preceeding whitespace and comments are grouped with a rule all the way
                    // from the end a previous non-whitespace or comment, such as options, headers, or rule.
                    Interval token_interval = rule.SourceInterval;
                    int end = token_interval.b;
                    Antlr4.Runtime.IToken end_tok = pd_parser.TokStream.Get(end);
                    Antlr4.Runtime.IToken last = end_tok;
                    int end_ind = old_code.Length <= last.StopIndex ? last.StopIndex : last.StopIndex + 1;
                    for (int j = end_ind; j < old_code.Length; j++)
                    {
                        if (old_code[j] == '\r')
                        {
                            if (j + 1 < old_code.Length && old_code[j + 1] == '\n')
                            {
                                end_ind = j + 2;
                            }
                            else
                            {
                                end_ind = j + 1;
                            }

                            break;
                        }
                        end_ind = j;
                    }
                    int start = token_interval.a;
                    Antlr4.Runtime.IToken start_tok = pd_parser.TokStream.Get(start);
                    int start_ind = start_tok.StartIndex;
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
                for (int i = 0; i < rules.Count; ++i)
                {
                    for (int j = rules[i].start_index; j < rules[i].end_index; ++j)
                    {
                        if (old_code[j] == '\r')
                        {
                            if (j + 1 < rules[i].end_index && old_code[j + 1] == '\n')
                            {
                                ;
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }

            public void FindStartRules()
            {
                for (int i = 0; i < rules.Count; ++i)
                {
                    for (int j = 0; j < rules[i].RHS.Count; ++j)
                    {
                        string rhs_symbol = rules[i].RHS[j];
                        if (nt_to_index.ContainsKey(rhs_symbol))
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
                        FindCalls syntax_walker = new FindCalls();
                        syntax_walker.Visit(root);
                        for (int i = 0; i < rules.Count; ++i)
                        {
                            string nt_name = rules[i].LHS;
                            string call = "." + nt_name + "()";
                            foreach (string j in syntax_walker.Invocations)
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

        public class TableOfModes
        {
            public class Row
            {
                public ANTLRv4Parser.ModeSpecContext mode;
                public string name;
                public int start_index;
                public int end_index;
            }

            public List<Row> modes = new List<Row>();
            public Dictionary<string, int> name_to_index = new Dictionary<string, int>();
            public ExtractModes listener;
            private readonly AntlrGrammarDetails pd_parser;
            private readonly Document document;

            public TableOfModes(AntlrGrammarDetails p, Document d)
            {
                pd_parser = p;
                document = d;
            }

            public void ReadModes()
            {
                // Get modes.
                listener = new ExtractModes();
                ParseTreeWalker.Default.Walk(listener, pd_parser.ParseTree);
                for (int i = 0; i < listener.Modes.Count; ++i)
                {
                    modes.Add(new Row()
                    {
                        mode = listener.Modes[i],
                        name = listener.Modes[i].identifier().GetText(),
                    });
                }
                for (int i = 0; i < modes.Count; ++i)
                {
                    string t = modes[i].name;
                    name_to_index[t] = i;
                }
            }

            public void FindPartitions()
            {
                FindFirstMode find_first_mode = new FindFirstMode();
                ParseTreeWalker.Default.Walk(find_first_mode, pd_parser.ParseTree);
                IParseTree first_rule = find_first_mode.First;
                if (first_rule == null)
                {
                    return;
                }

                string old_code = document.Code;
                for (int i = 0; i < modes.Count; ++i)
                {
                    var mode = modes[i].mode;
                    // Find range indices for modes including comments. Note, start index is inclusive; end
                    // index is exclusive. We make the assumption
                    // that the preceeding whitespace and comments are grouped with a rule all the way
                    // from the end a previous non-whitespace or comment, such as options, headers, or rule.
                    Interval token_interval = mode.SourceInterval;
                    int end = token_interval.b;
                    Antlr4.Runtime.IToken end_tok = pd_parser.TokStream.Get(end);
                    Antlr4.Runtime.IToken last = end_tok;
                    int end_ind = old_code.Length <= last.StopIndex ? last.StopIndex : last.StopIndex + 1;
                    for (int j = end_ind; j < old_code.Length; j++)
                    {
                        if (old_code[j] == '\r')
                        {
                            if (j + 1 < old_code.Length && old_code[j + 1] == '\n')
                            {
                                end_ind = j + 2;
                            }
                            else
                            {
                                end_ind = j + 1;
                            }

                            break;
                        }
                        end_ind = j;
                    }
                    int start = token_interval.a;
                    Antlr4.Runtime.IToken start_tok = pd_parser.TokStream.Get(start);
                    int start_ind = start_tok.StartIndex;
                    modes[i].start_index = start_ind;
                    modes[i].end_index = end_ind;
                }
                for (int i = 0; i < modes.Count; ++i)
                {
                    if (i > 0)
                    {
                        modes[i].start_index = modes[i - 1].end_index;
                    }
                }
                for (int i = 0; i < modes.Count; ++i)
                {
                    for (int j = modes[i].start_index; j < modes[i].end_index; ++j)
                    {
                        if (old_code[j] == '\r')
                        {
                            if (j + 1 < modes[i].end_index && old_code[j + 1] == '\n')
                            {
                                ;
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }
        }

        public static void Reconstruct(StringBuilder sb, CommonTokenStream stream, IParseTree tree, ref int previous, Func<IParseTree, string> replace)
        {
            if (tree as TerminalNodeImpl != null)
            {
                TerminalNodeImpl tok = tree as TerminalNodeImpl;
                var payload = tok.Payload;
                if (payload.Line < 0)
                {
                    sb.Append(" ");
                }
                else
                {
                    var start = payload.StartIndex;
                    var stop = payload.StopIndex + 1;
                    ICharStream charstream = payload.InputStream;
                    if (previous < start)
                    {
                        Interval previous_interval = new Interval(previous, start - 1);
                        string inter = charstream.GetText(previous_interval);
                        sb.Append(inter);
                    }
                    previous = stop;
                }
                if (tok.Symbol.Type == TokenConstants.EOF)
                    return;
                string new_s = replace(tok);
                if (new_s != null)
                    sb.Append(new_s);
                else
                    sb.Append(tok.GetText());
            }
            else
            {
                var new_s = replace(tree);
                if (new_s != null)
                {
                    Interval source_interval = tree.SourceInterval;
                    int a = source_interval.a;
                    int b = source_interval.b;
                    IToken ta = stream.Get(a);
                    IToken tb = stream.Get(b);
                    var start = ta.StartIndex;
                    var stop = tb.StopIndex + 1;
                    ICharStream charstream = ta.InputStream;
                    if (previous < start)
                    {
                        Interval previous_interval = new Interval(previous, start - 1);
                        string inter = charstream.GetText(previous_interval);
                        sb.Append(inter);
                    }
                    sb.Append(new_s);
                    previous = stop;
                }
                else
                {
                    for (int i = 0; i < tree.ChildCount; ++i)
                    {
                        var c = tree.GetChild(i);
                        Reconstruct(sb, stream, c, ref previous, replace);
                    }
                }
            }
        }

        public static void Output(StringBuilder sb, CommonTokenStream stream, IParseTree tree)
        {
            if (tree as TerminalNodeImpl != null)
            {
                TerminalNodeImpl tok = tree as TerminalNodeImpl;
                if (tok.Symbol.Type == TokenConstants.EOF)
                    return;
                sb.Append(" " + tok.GetText());
            }
            else
            {
                for (int i = 0; i < tree.ChildCount; ++i)
                {
                    var c = tree.GetChild(i);
                    Output(sb, stream, c);
                }
            }
        }

        public static Dictionary<string, string> ReplaceLiterals(int index, Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if initial file is a grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");

            ExtractGrammarType egt = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(egt, pd_parser.ParseTree);
            bool is_grammar = egt.Type == ExtractGrammarType.GrammarType.Parser
                || egt.Type == ExtractGrammarType.GrammarType.Combined
                || egt.Type == ExtractGrammarType.GrammarType.Lexer;
            if (!is_grammar)
            {
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            }

            // Find all other grammars by walking dependencies (import, vocab, file names).
            HashSet<string> read_files = new HashSet<string>
            {
                document.FullPath
            };
            Dictionary<Workspaces.Document, List<TerminalNodeImpl>> every_damn_literal =
                new Dictionary<Workspaces.Document, List<TerminalNodeImpl>>();
            for (; ; )
            {
                int before_count = read_files.Count;
                foreach (string f in read_files)
                {
                    List<string> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Value.Contains(f)).Select(
                        t => t.Key).ToList();
                    read_files = read_files.Union(additional).ToHashSet();
                }
                foreach (string f in read_files)
                {
                    IEnumerable<List<string>> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Key == f).Select(
                        t => t.Value);
                    foreach (List<string> t in additional)
                    {
                        read_files = read_files.Union(t).ToHashSet();
                    }
                }
                int after_count = read_files.Count;
                if (after_count == before_count)
                {
                    break;
                }
            }

            // Find rewrite rules, i.e., string literal to string symbol name.
            Dictionary<string, string> subs = new Dictionary<string, string>();
            foreach (string f in read_files)
            {
                Workspaces.Document whatever_document = Workspaces.Workspace.Instance.FindDocument(f);
                if (whatever_document == null)
                {
                    continue;
                }
                AntlrGrammarDetails pd_whatever = ParserDetailsFactory.Create(whatever_document) as AntlrGrammarDetails;

                // Find literals in grammars.
                LiteralsGrammar lp_whatever = new LiteralsGrammar();
                ParseTreeWalker.Default.Walk(lp_whatever, pd_whatever.ParseTree);
                List<TerminalNodeImpl> list_literals = lp_whatever.Literals;
                foreach (TerminalNodeImpl lexer_literal in list_literals)
                {
                    string old_name = lexer_literal.GetText();
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
                    IRuleNode p1 = lexer_literal.Parent;
                    if (p1.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p1 is ANTLRv4Parser.TerminalContext))
                    {
                        continue;
                    }

                    IRuleNode p2 = p1.Parent;
                    if (p2.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p2 is ANTLRv4Parser.LexerAtomContext))
                    {
                        continue;
                    }

                    IRuleNode p3 = p2.Parent;
                    if (p3.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p3 is ANTLRv4Parser.LexerElementContext))
                    {
                        continue;
                    }

                    IRuleNode p4 = p3.Parent;
                    if (p4.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p4 is ANTLRv4Parser.LexerElementsContext))
                    {
                        continue;
                    }

                    IRuleNode p5 = p4.Parent;
                    if (p5.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p5 is ANTLRv4Parser.LexerAltContext))
                    {
                        continue;
                    }

                    IRuleNode p6 = p5.Parent;
                    if (p6.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p6 is ANTLRv4Parser.LexerAltListContext))
                    {
                        continue;
                    }

                    IRuleNode p7 = p6.Parent;
                    if (p7.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p7 is ANTLRv4Parser.LexerRuleBlockContext))
                    {
                        continue;
                    }

                    IRuleNode p8 = p7.Parent;
                    if (p8.ChildCount != 4)
                    {
                        continue;
                    }

                    if (!(p8 is ANTLRv4Parser.LexerRuleSpecContext))
                    {
                        continue;
                    }

                    IParseTree alt = p8.GetChild(0);
                    string new_name = alt.GetText();
                    subs.Add(old_name, new_name);
                }
            }

            // Find string literals in parser and combined grammars and substitute.
            foreach (string f in read_files)
            {
                Workspaces.Document whatever_document = Workspaces.Workspace.Instance.FindDocument(f);
                if (whatever_document == null)
                {
                    continue;
                }
                AntlrGrammarDetails pd_whatever = ParserDetailsFactory.Create(whatever_document) as AntlrGrammarDetails;

                // Get all intertoken text immediately for source reconstruction.
                var (text_before, other) = TreeEdits.TextToLeftOfLeaves(pd_whatever.TokStream, pd_whatever.ParseTree);

                TreeEdits.Replace(pd_whatever.ParseTree,
                    n =>
                    {
                        if (!(n is TerminalNodeImpl))
                        {
                            return null;
                        }
                        var t = n as TerminalNodeImpl;
                        if (t.Payload.Type != ANTLRv4Lexer.STRING_LITERAL)
                        {
                            return null;
                        }
                        bool no = false;
                        // Make sure this literal does not appear in lexer rule.
                        for (IRuleNode p = t.Parent; p != null; p = p.Parent)
                        {
                            if (p is ANTLRv4Parser.LexerRuleSpecContext)
                            {
                                no = true;
                                break;
                            }
                        }
                        if (no)
                        {
                            return null;
                        }
                        subs.TryGetValue(t.GetText(), out string value);
                        if (value == null)
                        {
                            return null;
                        }
                        var token = new CommonToken(ANTLRv4Lexer.TOKEN_REF) { Line = -1, Column = -1, Text = value };
                        var new_sym = new TerminalNodeImpl(token);
                        text_before.TryGetValue(t, out string v);
                        if (v != null)
                            text_before.Add(new_sym, v);
                        return new_sym;
                    });
                StringBuilder sb = new StringBuilder();
                TreeEdits.Reconstruct(sb, pd_whatever.ParseTree, text_before);
                var new_code = sb.ToString();
                if (new_code != pd_parser.Code)
                {
                    result.Add(whatever_document.FullPath, new_code);
                }
            }
            return result;
        }

        public static Dictionary<string, string> RemoveUselessParserProductions(int pos, Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if lexer grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
            {
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            }
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            bool is_lexer = lp.Type == ExtractGrammarType.GrammarType.Lexer;
            if (is_lexer)
            {
                throw new LanguageServerException("A parser or combined grammar file is not selected. Please select one first.");
            }

            // Consider only the target grammar.
            TableOfRules table = new TableOfRules(pd_parser, document);
            table.ReadRules();
            table.FindPartitions();
            table.FindStartRules();

            List<Pair<int, int>> deletions = new List<Pair<int, int>>();
            foreach (TableOfRules.Row r in table.rules)
            {
                if (r.is_parser_rule && r.is_used == false)
                {
                    deletions.Add(new Pair<int, int>(r.start_index, r.end_index));
                }
            }
            deletions = deletions.OrderBy(p => p.a).ThenBy(p => p.b).ToList();
            StringBuilder sb = new StringBuilder();
            int previous = 0;
            string old_code = document.Code;
            foreach (Pair<int, int> l in deletions)
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
            if (new_code != pd_parser.Code)
            {
                result.Add(pd_parser.FullFileName, new_code);
            }
            return result;
        }

        public static Dictionary<string, string> MoveStartRuleToTop(int pos, Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if lexer grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            bool is_lexer = lp.Type == ExtractGrammarType.GrammarType.Lexer;
            if (is_lexer)
            {
                throw new LanguageServerException("A parser or combined grammar file is not selected. Please select one first.");
            }

            // Consider only the target grammar.
            TableOfRules table = new TableOfRules(pd_parser, document);
            table.ReadRules();
            table.FindPartitions();
            table.FindStartRules();

            string old_code = document.Code;
            List<Pair<int, int>> move = new List<Pair<int, int>>();
            foreach (TableOfRules.Row r in table.rules)
            {
                if (r.is_parser_rule && r.is_start == true)
                {
                    move.Add(new Pair<int, int>(r.start_index, r.end_index));
                }
            }
            move = move.OrderBy(p => p.a).ThenBy(p => p.b).ToList();

            FindFirstRule find_first_rule = new FindFirstRule();
            ParseTreeWalker.Default.Walk(find_first_rule, pd_parser.ParseTree);
            IParseTree first_rule = find_first_rule.First;
            if (first_rule == null)
            {
                return result;
            }

            int insertion = first_rule.SourceInterval.a;
            Antlr4.Runtime.IToken insertion_tok = pd_parser.TokStream.Get(insertion);
            int insertion_ind = insertion_tok.StartIndex;
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
            foreach (Pair<int, int> l in move)
            {
                int index_start = l.a;
                int len = l.b - l.a;
                string add = old_code.Substring(index_start, len);
                sb.Append(add);
            }
            foreach (Pair<int, int> l in move)
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
            if (new_code != pd_parser.Code)
            {
                result.Add(document.FullPath, new_code);
            }
            return result;
        }

        public static Dictionary<string, string> ReorderParserRules(Document document, LspAntlr.ReorderType type)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if lexer grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            bool is_lexer = lp.Type == ExtractGrammarType.GrammarType.Lexer;
            if (is_lexer)
            {
                throw new LanguageServerException("A parser or combined grammar file is not selected. Reordering only applies to grammars that contain parser rules; lexer rules cannot be reordered. Please select one first.");
            }

            TableOfRules table = new TableOfRules(pd_parser, document);
            table.ReadRules();
            table.FindPartitions();
            table.FindStartRules();

            // Find new order of rules.
            string old_code = document.Code;
            List<Pair<int, int>> reorder = new List<Pair<int, int>>();
            if (type == LspAntlr.ReorderType.DFS)
            {
                Digraph<string> graph = new Digraph<string>();
                foreach (TableOfRules.Row r in table.rules)
                {
                    if (!r.is_parser_rule)
                    {
                        continue;
                    }

                    graph.AddVertex(r.LHS);
                }
                foreach (TableOfRules.Row r in table.rules)
                {
                    if (!r.is_parser_rule)
                    {
                        continue;
                    }

                    List<string> j = r.RHS;
                    //j.Reverse();
                    foreach (string rhs in j)
                    {
                        TableOfRules.Row sym = table.rules.Where(t => t.LHS == rhs).FirstOrDefault();
                        if (!sym.is_parser_rule)
                        {
                            continue;
                        }

                        DirectedEdge<string> e = new DirectedEdge<string>(r.LHS, rhs);
                        graph.AddEdge(e);
                    }
                }
                List<string> starts = new List<string>();
                foreach (TableOfRules.Row r in table.rules)
                {
                    if (r.is_parser_rule && r.is_start)
                    {
                        starts.Add(r.LHS);
                    }
                }
                Algorithms.DepthFirstOrder<string, DirectedEdge<string>> sort = new DepthFirstOrder<string, DirectedEdge<string>>(graph, starts);
                List<string> ordered = sort.ToList();
                foreach (string s in ordered)
                {
                    TableOfRules.Row row = table.rules[table.nt_to_index[s]];
                    reorder.Add(new Pair<int, int>(row.start_index, row.end_index));
                }
            }
            else if (type == LspAntlr.ReorderType.BFS)
            {
                Digraph<string> graph = new Digraph<string>();
                foreach (TableOfRules.Row r in table.rules)
                {
                    if (!r.is_parser_rule)
                    {
                        continue;
                    }

                    graph.AddVertex(r.LHS);
                }
                foreach (TableOfRules.Row r in table.rules)
                {
                    if (!r.is_parser_rule)
                    {
                        continue;
                    }

                    List<string> j = r.RHS;
                    //j.Reverse();
                    foreach (string rhs in j)
                    {
                        TableOfRules.Row sym = table.rules.Where(t => t.LHS == rhs).FirstOrDefault();
                        if (!sym.is_parser_rule)
                        {
                            continue;
                        }

                        DirectedEdge<string> e = new DirectedEdge<string>(r.LHS, rhs);
                        graph.AddEdge(e);
                    }
                }
                List<string> starts = new List<string>();
                foreach (TableOfRules.Row r in table.rules)
                {
                    if (r.is_parser_rule && r.is_start)
                    {
                        starts.Add(r.LHS);
                    }
                }
                Algorithms.BreadthFirstOrder<string, DirectedEdge<string>> sort = new BreadthFirstOrder<string, DirectedEdge<string>>(graph, starts);
                List<string> ordered = sort.ToList();
                foreach (string s in ordered)
                {
                    TableOfRules.Row row = table.rules[table.nt_to_index[s]];
                    reorder.Add(new Pair<int, int>(row.start_index, row.end_index));
                }
            }
            else if (type == LspAntlr.ReorderType.Alphabetically)
            {
                List<string> ordered = table.rules
                    .Where(r => r.is_parser_rule)
                    .Select(r => r.LHS)
                    .OrderBy(r => r).ToList();
                foreach (string s in ordered)
                {
                    TableOfRules.Row row = table.rules[table.nt_to_index[s]];
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
            foreach (Pair<int, int> l in reorder)
            {
                int index_start = l.a;
                int len = l.b - l.a;
                string add = old_code.Substring(index_start, len);
                sb.Append(add);
            }
            // Now add all non-parser rules.
            foreach (TableOfRules.Row r in table.rules)
            {
                if (r.is_parser_rule)
                {
                    continue;
                }

                int index_start = r.start_index;
                int len = r.end_index - r.start_index;
                string add = old_code.Substring(index_start, len);
                sb.Append(add);
            }
            //string rest = old_code.Substring(previous);
            //sb.Append(rest);
            string new_code = sb.ToString();
            if (new_code != pd_parser.Code)
            {
                result.Add(document.FullPath, new_code);
            }
            return result;
        }

        public static Dictionary<string, string> SplitCombineGrammars(int pos, Document document, bool split)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if lexer grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            if (split && lp.Type != ExtractGrammarType.GrammarType.Combined)
            {
                throw new LanguageServerException("A combined grammar file is not selected. Please select one first.");
            }
            if ((!split) && lp.Type != ExtractGrammarType.GrammarType.Parser)
            {
                throw new LanguageServerException("A split grammar file is not selected. Please select one first.");
            }

            TableOfRules table = new TableOfRules(pd_parser, document);
            table.ReadRules();
            table.FindPartitions();
            table.FindStartRules();

            string old_code = document.Code;
            if (split)
            {
                // Create a parser and lexer grammar.
                StringBuilder sb_parser = new StringBuilder();
                StringBuilder sb_lexer = new StringBuilder();
                if (!(pd_parser.ParseTree is ANTLRv4Parser.GrammarSpecContext root))
                {
                    return null;
                }

                int grammar_type_index = 0;
                if (root.DOC_COMMENT() != null)
                {
                    grammar_type_index++;
                }

                ANTLRv4Parser.GrammarDeclContext grammar_type_tree = root.grammarDecl();
                ANTLRv4Parser.IdentifierContext id = grammar_type_tree.identifier();
                ITerminalNode semi_tree = grammar_type_tree.SEMI();
                ANTLRv4Parser.RulesContext rules_tree = root.rules();
                string pre = old_code.Substring(0, pd_parser.TokStream.Get(grammar_type_tree.SourceInterval.a).StartIndex - 0);
                sb_parser.Append(pre);
                sb_lexer.Append(pre);
                sb_parser.Append("parser grammar " + id.GetText() + "Parser;" + Environment.NewLine);
                sb_lexer.Append("lexer grammar " + id.GetText() + "Lexer;" + Environment.NewLine);
                int x1 = pd_parser.TokStream.Get(semi_tree.SourceInterval.b).StopIndex + 1;
                int x2 = pd_parser.TokStream.Get(rules_tree.SourceInterval.a).StartIndex;
                string n1 = old_code.Substring(x1, x2 - x1);
                sb_parser.Append(n1);
                sb_lexer.Append(n1);
                sb_parser.AppendLine("options { tokenVocab=" + id.GetText() + "Lexer; }");
                int end = 0;
                for (int i = 0; i < table.rules.Count; ++i)
                {
                    TableOfRules.Row r = table.rules[i];
                    // Partition rule symbols.
                    if (r.is_parser_rule)
                    {
                        string n2 = old_code.Substring(r.start_index, r.end_index - r.start_index);
                        sb_parser.Append(n2);
                    }
                    else
                    {
                        string n2 = old_code.Substring(r.start_index, r.end_index - r.start_index);
                        sb_lexer.Append(n2);
                    }
                    end = r.end_index + 1;
                }
                if (end < old_code.Length)
                {
                    string rest = old_code.Substring(end);
                    sb_parser.Append(rest);
                    sb_lexer.Append(rest);
                }
                string g4_file_path = document.FullPath;
                string current_dir = Path.GetDirectoryName(g4_file_path);
                if (current_dir == null)
                {
                    return null;
                }
                string orig_name = Path.GetFileNameWithoutExtension(g4_file_path);
                string new_code_parser = sb_parser.ToString();
                string new_parser_ffn = current_dir + Path.DirectorySeparatorChar
                    + orig_name + "Parser.g4";
                string new_lexer_ffn = current_dir + Path.DirectorySeparatorChar
                    + orig_name + "Lexer.g4";
                string new_code_lexer = sb_lexer.ToString();
                result.Add(new_parser_ffn, new_code_parser);
                result.Add(new_lexer_ffn, new_code_lexer);
                result.Add(g4_file_path, null);
            }
            else
            {
                // Parse grammar.
                HashSet<string> read_files = new HashSet<string>
                {
                    document.FullPath
                };
                for (; ; )
                {
                    int before_count = read_files.Count;
                    foreach (string f in read_files)
                    {
                        List<string> additional = AntlrGrammarDetails._dependent_grammars.Where(
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
                List<AntlrGrammarDetails> grammars = new List<AntlrGrammarDetails>();
                foreach (string f in read_files)
                {
                    Workspaces.Document d = Workspaces.Workspace.Instance.FindDocument(f);
                    if (d == null)
                    {
                        continue;
                    }
                    AntlrGrammarDetails x = ParserDetailsFactory.Create(d) as AntlrGrammarDetails;
                    grammars.Add(x);
                }

                // I'm going to have to assume two grammars, one lexer and one parser grammar each.
                if (grammars.Count != 2)
                {
                    return null;
                }

                // Read now lexer grammar. The parser grammar was already read.
                AntlrGrammarDetails pd_lexer = grammars[1];
                Workspaces.Document ldocument = Workspaces.Workspace.Instance.FindDocument(pd_lexer.FullFileName);
                TableOfRules lexer_table = new TableOfRules(pd_lexer, ldocument);
                lexer_table.ReadRules();
                lexer_table.FindPartitions();
                lexer_table.FindStartRules();

                // Look for tokenVocab.
                FindOptions find_options = new FindOptions();
                ParseTreeWalker.Default.Walk(find_options, pd_parser.ParseTree);
                ANTLRv4Parser.OptionContext tokenVocab = null;
                foreach (var o in find_options.Options)
                {
                    var oo = o as ANTLRv4Parser.OptionContext;
                    if (oo.identifier() != null && oo.identifier().GetText() == "tokenVocab")
                    {
                        tokenVocab = oo;
                    }
                }
                bool remove_options_spec = tokenVocab != null && find_options.Options.Count == 1;
                bool rewrite_options_spec = tokenVocab != null;

                // Create a combined parser grammar.
                StringBuilder sb_parser = new StringBuilder();
                if (!(pd_parser.ParseTree is ANTLRv4Parser.GrammarSpecContext root))
                {
                    return null;
                }

                int grammar_type_index = 0;
                if (root.DOC_COMMENT() != null)
                {
                    grammar_type_index++;
                }

                ANTLRv4Parser.GrammarDeclContext grammar_type_tree = root.grammarDecl();
                ANTLRv4Parser.IdentifierContext id = grammar_type_tree.identifier();
                ITerminalNode semi_tree = grammar_type_tree.SEMI();
                ANTLRv4Parser.RulesContext rules_tree = root.rules();
                string pre = old_code.Substring(0, pd_parser.TokStream.Get(grammar_type_tree.SourceInterval.a).StartIndex - 0);
                sb_parser.Append(pre);
                sb_parser.Append("grammar " + id.GetText().Replace("Parser", "") + ";" + Environment.NewLine);

                if (!(remove_options_spec || rewrite_options_spec))
                {
                    int x1 = pd_parser.TokStream.Get(semi_tree.SourceInterval.b).StopIndex + 1;
                    int x2 = pd_parser.TokStream.Get(rules_tree.SourceInterval.a).StartIndex;
                    string n1 = old_code.Substring(x1, x2 - x1);
                    sb_parser.Append(n1);
                }
                else if (remove_options_spec)
                {
                    int x1 = pd_parser.TokStream.Get(semi_tree.SourceInterval.b).StopIndex + 1;
                    int x2 = pd_parser.TokStream.Get(find_options.OptionsSpec.SourceInterval.a).StartIndex;
                    int x3 = pd_parser.TokStream.Get(find_options.OptionsSpec.SourceInterval.b).StopIndex + 1;
                    int x4 = pd_parser.TokStream.Get(rules_tree.SourceInterval.a).StartIndex;
                    string n1 = old_code.Substring(x1, x2 - x1);
                    sb_parser.Append(n1);
                    string n3 = old_code.Substring(x3, x4 - x3);
                    sb_parser.Append(n3);
                }
                else if (rewrite_options_spec)
                {
                    int x1 = pd_parser.TokStream.Get(semi_tree.SourceInterval.b).StopIndex + 1;
                    int x2 = 0;
                    int x3 = 0;
                    foreach (var o in find_options.Options)
                    {
                        var oo = o as ANTLRv4Parser.OptionContext;
                        if (oo.identifier() != null && oo.identifier().GetText() == "tokenVocab")
                        {
                            x2 = pd_parser.TokStream.Get(oo.SourceInterval.a).StartIndex;
                            int j;
                            for (j = oo.SourceInterval.b + 1; ; j++)
                            {
                                if (pd_parser.TokStream.Get(j).Text == ";")
                                {
                                    j++;
                                    break;
                                }
                            }
                            x3 = pd_parser.TokStream.Get(j).StopIndex + 1;
                            break;
                        }
                    }
                    int x4 = pd_parser.TokStream.Get(rules_tree.SourceInterval.a).StartIndex;
                    string n1 = old_code.Substring(x1, x2 - x1);
                    sb_parser.Append(n1);
                    string n2 = old_code.Substring(x2, x3 - x2);
                    sb_parser.Append(n2);
                    string n4 = old_code.Substring(x3, x4 - x3);
                    sb_parser.Append(n4);
                }
                int end = 0;
                for (int i = 0; i < table.rules.Count; ++i)
                {
                    TableOfRules.Row r = table.rules[i];
                    if (r.is_parser_rule)
                    {
                        string n2 = old_code.Substring(r.start_index, r.end_index - r.start_index);
                        sb_parser.Append(n2);
                    }
                    end = r.end_index + 1;
                }
                if (end < old_code.Length)
                {
                    string rest = old_code.Substring(end);
                    sb_parser.Append(rest);
                }
                end = 0;
                string lexer_old_code = ldocument.Code;
                for (int i = 0; i < lexer_table.rules.Count; ++i)
                {
                    TableOfRules.Row r = lexer_table.rules[i];
                    if (!r.is_parser_rule)
                    {
                        string n2 = lexer_old_code.Substring(r.start_index, r.end_index - r.start_index);
                        sb_parser.Append(n2);
                    }
                    end = r.end_index + 1;
                }
                if (end < lexer_old_code.Length)
                {
                    string rest = lexer_old_code.Substring(end);
                    sb_parser.Append(rest);
                }
                string g4_file_path = document.FullPath;
                string current_dir = Path.GetDirectoryName(g4_file_path);
                if (current_dir == null)
                {
                    return null;
                }

                string orig_name = Path.GetFileName(g4_file_path);
                string new_name = orig_name.Replace("Parser.g4", "");
                string new_code_parser = sb_parser.ToString();
                string new_parser_ffn = current_dir + Path.DirectorySeparatorChar
                    + new_name + ".g4";
                result.Add(new_parser_ffn, new_code_parser);
                result.Add(pd_parser.FullFileName, null);
                result.Add(pd_lexer.FullFileName, null);
            }

            return result;
        }

        private static bool HasDirectLeftRecursion(IParseTree rule)
        {
            if (!(rule is ANTLRv4Parser.ParserRuleSpecContext))
                return false;
            var r = rule as ANTLRv4Parser.ParserRuleSpecContext;
            var lhs = r.RULE_REF();
            var rb = r.ruleBlock();
            if (rb == null) return false;
            var ral = rb.ruleAltList();
            foreach (var la in ral.labeledAlt())
            {
                if (la
                    .alternative()?
                    .element()?
                    .FirstOrDefault()?
                    .atom()?
                    .ruleref()?
                    .GetChild(0) is TerminalNodeImpl t1 && t1.GetText() == lhs.GetText())
                {
                    return true;
                }

                if (la
                    .alternative()?
                    .element()?
                    .FirstOrDefault()?
                    .labeledElement()?
                    .atom()?
                    .ruleref()?
                    .GetChild(0) is TerminalNodeImpl t2 && t2.GetText() == lhs.GetText())
                {
                    return true;
                }
            }
            return false;
        }

        private static bool HasDirectRightRecursion(IParseTree rule)
        {
            if (!(rule is ANTLRv4Parser.ParserRuleSpecContext))
                return false;
            var r = rule as ANTLRv4Parser.ParserRuleSpecContext;
            var lhs = r.RULE_REF();
            var rb = r.ruleBlock();
            if (rb == null) return false;
            var ral = rb.ruleAltList();
            foreach (var la in ral.labeledAlt())
            {
                if (la
                    .alternative()?
                    .element()?
                    .LastOrDefault()?
                    .atom()?
                    .ruleref()?
                    .GetChild(0) is TerminalNodeImpl t1 && t1.GetText() == lhs.GetText())
                {
                    return true;
                }
                if (la
                    .alternative()?
                    .element()?
                    .LastOrDefault()?
                    .labeledElement()?
                    .atom()?
                    .ruleref()?
                    .GetChild(0) is TerminalNodeImpl t2 && t2.GetText() == lhs.GetText())
                {
                    return true;
                }
            }
            return false;
        }

        private static (IParseTree, IParseTree) GenerateReplacementRules(string new_symbol_name, IParseTree rule, Dictionary<TerminalNodeImpl, string> text_before)
        {
            ANTLRv4Parser.ParserRuleSpecContext new_a_rule = new ANTLRv4Parser.ParserRuleSpecContext(null, 0);
            {
                var r = rule as ANTLRv4Parser.ParserRuleSpecContext;
                var lhs = r.RULE_REF()?.GetText();
                {
                    TreeEdits.CopyTreeRecursive(r.RULE_REF(), new_a_rule, text_before);
                }
                // Now have "A"
                {
                    var token2 = new CommonToken(ANTLRv4Parser.COLON) { Line = -1, Column = -1, Text = ":" };
                    var new_colon = new TerminalNodeImpl(token2);
                    new_a_rule.AddChild(new_colon);
                    new_colon.Parent = new_a_rule;
                }
                // Now have "A :"
                ANTLRv4Parser.RuleAltListContext rule_alt_list = new ANTLRv4Parser.RuleAltListContext(null, 0);
                {
                    ANTLRv4Parser.RuleBlockContext new_rule_block_context = new ANTLRv4Parser.RuleBlockContext(new_a_rule, 0);
                    new_a_rule.AddChild(new_rule_block_context);
                    new_rule_block_context.Parent = new_a_rule;
                    new_rule_block_context.AddChild(rule_alt_list);
                    rule_alt_list.Parent = new_rule_block_context;
                }
                // Now have "A : <rb <ral> >"
                {
                    var token3 = new CommonToken(ANTLRv4Parser.SEMI) { Line = -1, Column = -1, Text = ";" };
                    var new_semi = new TerminalNodeImpl(token3);
                    new_a_rule.AddChild(new_semi);
                    new_semi.Parent = new_a_rule;
                }
                // Now have "A : <rb <ral> > ;"
                {
                    TreeEdits.CopyTreeRecursive(r.exceptionGroup(), new_a_rule, text_before);
                }
                // Now have "A : <rb <ral> > ; <eg>"
                bool first = true;
                foreach (ANTLRv4Parser.AlternativeContext alt in EnumeratorOfAlts(rule))
                {
                    ANTLRv4Parser.AtomContext atom = EnumeratorOfRHS(alt)?.FirstOrDefault();
                    if (lhs == atom?.GetText())
                    {
                        // skip alts that have direct left recursion.
                        continue;
                    }
                    {
                        if (!first)
                        {
                            var token4 = new CommonToken(ANTLRv4Lexer.OR) { Line = -1, Column = -1, Text = "|" };
                            var new_or = new TerminalNodeImpl(token4);
                            rule_alt_list.AddChild(new_or);
                            new_or.Parent = rule_alt_list;
                        }
                        first = false;
                    }
                    ANTLRv4Parser.LabeledAltContext l_alt = new ANTLRv4Parser.LabeledAltContext(rule_alt_list, 0);
                    rule_alt_list.AddChild(l_alt);
                    l_alt.Parent = rule_alt_list;
                    // Create new alt "beta A'".
                    ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                    l_alt.AddChild(new_alt);
                    new_alt.Parent = l_alt;
                    foreach (var element in alt.element())
                    {
                        TreeEdits.CopyTreeRecursive(element, new_alt, text_before);
                    }
                    var token = new CommonToken(ANTLRv4Lexer.RULE_REF) { Line = -1, Column = -1, Text = new_symbol_name };
                    var new_rule_ref = new TerminalNodeImpl(token);
                    var new_ruleref = new ANTLRv4Parser.RulerefContext(null, 0);
                    new_ruleref.AddChild(new_rule_ref);
                    new_rule_ref.Parent = new_ruleref;
                    var new_atom = new ANTLRv4Parser.AtomContext(null, 0);
                    new_atom.AddChild(new_ruleref);
                    new_ruleref.Parent = new_atom;
                    var new_element = new ANTLRv4Parser.ElementContext(null, 0);
                    new_element.AddChild(new_atom);
                    new_atom.Parent = new_element;
                    new_alt.AddChild(new_element);
                    new_element.Parent = new_alt;
                }
            }
            // Now have "A : beta1 A' | beta2 A' | ... ; <eg>"

            ANTLRv4Parser.ParserRuleSpecContext new_ap_rule = new ANTLRv4Parser.ParserRuleSpecContext(null, 0);
            {
                var r = rule as ANTLRv4Parser.ParserRuleSpecContext;
                var lhs = r.RULE_REF()?.GetText();
                {
                    var token = new CommonToken(ANTLRv4Parser.RULE_REF) { Line = -1, Column = -1, Text = new_symbol_name };
                    var new_rule_ref = new TerminalNodeImpl(token);
                    new_ap_rule.AddChild(new_rule_ref);
                    new_rule_ref.Parent = new_ap_rule;
                }
                // Now have "A'"
                {
                    var token2 = new CommonToken(ANTLRv4Parser.COLON) { Line = -1, Column = -1, Text = ":" };
                    var new_colon = new TerminalNodeImpl(token2);
                    new_ap_rule.AddChild(new_colon);
                    new_colon.Parent = new_ap_rule;
                }
                // Now have "A' :"
                ANTLRv4Parser.RuleAltListContext rule_alt_list = new ANTLRv4Parser.RuleAltListContext(null, 0);
                {
                    ANTLRv4Parser.RuleBlockContext new_rule_block_context = new ANTLRv4Parser.RuleBlockContext(new_a_rule, 0);
                    new_ap_rule.AddChild(new_rule_block_context);
                    new_rule_block_context.Parent = new_ap_rule;
                    new_rule_block_context.AddChild(rule_alt_list);
                    rule_alt_list.Parent = new_rule_block_context;
                }
                // Now have "A' : <rb <ral> >"
                {
                    var token3 = new CommonToken(ANTLRv4Parser.SEMI) { Line = -1, Column = -1, Text = ";" };
                    var new_semi = new TerminalNodeImpl(token3);
                    new_ap_rule.AddChild(new_semi);
                    new_semi.Parent = new_ap_rule;
                }
                // Now have "A : <rb <ral> > ;"
                {
                    TreeEdits.CopyTreeRecursive(r.exceptionGroup(), new_a_rule, text_before);
                }
                // Now have "A' : <rb <ral> > ; <eg>"
                bool first = true;
                foreach (ANTLRv4Parser.AlternativeContext alt in EnumeratorOfAlts(rule))
                {
                    ANTLRv4Parser.AtomContext atom = EnumeratorOfRHS(alt)?.FirstOrDefault();
                    if (lhs != atom?.GetText())
                    {
                        // skip alts that DO NOT have direct left recursion.
                        continue;
                    }
                    {
                        if (!first)
                        {
                            var token4 = new CommonToken(ANTLRv4Lexer.OR) { Line = -1, Column = -1, Text = "|" };
                            var new_or = new TerminalNodeImpl(token4);
                            rule_alt_list.AddChild(new_or);
                            new_or.Parent = rule_alt_list;
                        }
                        first = false;
                    }
                    ANTLRv4Parser.LabeledAltContext l_alt = new ANTLRv4Parser.LabeledAltContext(rule_alt_list, 0);
                    rule_alt_list.AddChild(l_alt);
                    l_alt.Parent = rule_alt_list;
                    // Create new alt "alpha A'".
                    ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                    l_alt.AddChild(new_alt);
                    new_alt.Parent = l_alt;
                    bool first2 = true;
                    foreach (var element in alt.element())
                    {
                        if (first2)
                        {
                            first2 = false;
                            continue;
                        }
                        TreeEdits.CopyTreeRecursive(element, new_alt, text_before);
                    }
                    var token = new CommonToken(ANTLRv4Parser.RULE_REF) { Line = -1, Column = -1, Text = new_symbol_name };
                    var new_rule_ref = new TerminalNodeImpl(token);
                    var new_ruleref = new ANTLRv4Parser.RulerefContext(null, 0);
                    new_ruleref.AddChild(new_rule_ref);
                    new_rule_ref.Parent = new_ruleref;
                    var new_atom = new ANTLRv4Parser.AtomContext(null, 0);
                    new_atom.AddChild(new_ruleref);
                    new_ruleref.Parent = new_atom;
                    var new_element = new ANTLRv4Parser.ElementContext(null, 0);
                    new_element.AddChild(new_atom);
                    new_atom.Parent = new_element;
                    new_alt.AddChild(new_element);
                    new_element.Parent = new_alt;
                }
                {
                    if (!first)
                    {
                        var token4 = new CommonToken(ANTLRv4Lexer.OR) { Line = -1, Column = -1, Text = "|" };
                        var new_or = new TerminalNodeImpl(token4);
                        rule_alt_list.AddChild(new_or);
                        new_or.Parent = rule_alt_list;
                    }
#pragma warning disable IDE0059
                    first = false;
#pragma warning restore IDE0059
                    ANTLRv4Parser.LabeledAltContext l_alt = new ANTLRv4Parser.LabeledAltContext(rule_alt_list, 0);
                    rule_alt_list.AddChild(l_alt);
                    l_alt.Parent = rule_alt_list;
                    // Create new empty alt.
                    ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                    l_alt.AddChild(new_alt);
                    new_alt.Parent = l_alt;
                }
            }
            // Now have "A' : alpha1 A' | alpha2 A' | ... ;"

            return ((IParseTree)new_a_rule, (IParseTree)new_ap_rule);
        }

        private static IParseTree ReplaceWithKleeneRules(bool has_direct_left_recursion, bool has_direct_right_recursion, IParseTree rule, Dictionary<TerminalNodeImpl, string> text_before)
        {
            // Left recursion:
            // Convert A -> A beta1 | A beta2 | ... | alpha1 | alpha2 | ... ;
            // into A ->  (alpha1 | alpha2 | ... ) (beta1 | beta2 | ...)*
            //
            // Right recursion:
            // Convert A -> beta1 A | beta2 A | ... | alpha1 | alpha2 | ... ;
            // into A ->   (beta1 | beta2 | ...)* (alpha1 | alpha2 | ... )

            ANTLRv4Parser.ParserRuleSpecContext new_a_rule = new ANTLRv4Parser.ParserRuleSpecContext(null, 0);
            {
                var r = rule as ANTLRv4Parser.ParserRuleSpecContext;
                var lhs = r.RULE_REF()?.GetText();
                {
                    TreeEdits.CopyTreeRecursive(r.RULE_REF(), new_a_rule, text_before);
                }
                // Now have "A"
                {
                    var token2 = new CommonToken(ANTLRv4Parser.COLON) { Line = -1, Column = -1, Text = ":" };
                    var new_colon = new TerminalNodeImpl(token2);
                    new_a_rule.AddChild(new_colon);
                    new_colon.Parent = new_a_rule;
                }
                // Now have "A :"
                ANTLRv4Parser.RuleAltListContext rule_alt_list = new ANTLRv4Parser.RuleAltListContext(null, 0);
                ANTLRv4Parser.AltListContext altlist1 = new ANTLRv4Parser.AltListContext(null, 0);
                ANTLRv4Parser.AltListContext altlist2 = new ANTLRv4Parser.AltListContext(null, 0);
                {
                    ANTLRv4Parser.RuleBlockContext new_rule_block_context = new ANTLRv4Parser.RuleBlockContext(new_a_rule, 0);
                    new_a_rule.AddChild(new_rule_block_context);
                    new_rule_block_context.Parent = new_a_rule;
                    new_rule_block_context.AddChild(rule_alt_list);
                    rule_alt_list.Parent = new_rule_block_context;
                    ANTLRv4Parser.LabeledAltContext l_alt = new ANTLRv4Parser.LabeledAltContext(null, 0);
                    rule_alt_list.AddChild(l_alt);
                    l_alt.Parent = rule_alt_list;
                    ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                    l_alt.AddChild(new_alt);
                    new_alt.Parent = l_alt;
                    var element1 = new ANTLRv4Parser.ElementContext(null, 0);
                    var element2 = new ANTLRv4Parser.ElementContext(null, 0);
                    if (has_direct_left_recursion && !has_direct_right_recursion)
                    {
                        new_alt.AddChild(element1);
                        element1.Parent = new_alt;
                        new_alt.AddChild(element2);
                        element2.Parent = new_alt;
                    }
                    else
                    {
                        new_alt.AddChild(element2);
                        element2.Parent = new_alt;
                        new_alt.AddChild(element1);
                        element1.Parent = new_alt;
                    }
                    {
                        var new_ebnf = new ANTLRv4Parser.EbnfContext(null, 0);
                        element1.AddChild(new_ebnf);
                        new_ebnf.Parent = element1;
                        var new_block = new ANTLRv4Parser.BlockContext(null, 0);
                        new_ebnf.AddChild(new_block);
                        new_block.Parent = new_ebnf;
                        var lparen_token = new CommonToken(ANTLRv4Lexer.LPAREN) { Line = -1, Column = -1, Text = "(" };
                        var new_lparen = new TerminalNodeImpl(lparen_token);
                        new_block.AddChild(new_lparen);
                        new_lparen.Parent = new_block;
                        new_block.AddChild(altlist1);
                        altlist1.Parent = new_block;
                        var rparen_token = new CommonToken(ANTLRv4Lexer.RPAREN) { Line = -1, Column = -1, Text = ")" };
                        var new_rparen = new TerminalNodeImpl(rparen_token);
                        new_block.AddChild(new_rparen);
                        new_rparen.Parent = new_block;
                    }
                    {
                        var new_ebnf = new ANTLRv4Parser.EbnfContext(null, 0);
                        element2.AddChild(new_ebnf);
                        new_ebnf.Parent = element2;
                        var new_block = new ANTLRv4Parser.BlockContext(null, 0);
                        new_ebnf.AddChild(new_block);
                        new_block.Parent = new_ebnf;
                        var new_blocksuffix = new ANTLRv4Parser.BlockSuffixContext(null, 0);
                        new_ebnf.AddChild(new_blocksuffix);
                        new_blocksuffix.Parent = new_ebnf;
                        var new_ebnfsuffix = new ANTLRv4Parser.EbnfSuffixContext(null, 0);
                        new_blocksuffix.AddChild(new_ebnfsuffix);
                        new_ebnfsuffix.Parent = new_blocksuffix;
                        var star_token = new CommonToken(ANTLRv4Lexer.STAR) { Line = -1, Column = -1, Text = "*" };
                        var new_star = new TerminalNodeImpl(star_token);
                        new_ebnfsuffix.AddChild(new_star);
                        new_star.Parent = new_ebnfsuffix;
                        var lparen_token = new CommonToken(ANTLRv4Lexer.LPAREN) { Line = -1, Column = -1, Text = "(" };
                        var new_lparen = new TerminalNodeImpl(lparen_token);
                        new_block.AddChild(new_lparen);
                        new_lparen.Parent = new_block;
                        new_block.AddChild(altlist2);
                        altlist2.Parent = new_block;
                        var rparen_token = new CommonToken(ANTLRv4Lexer.RPAREN) { Line = -1, Column = -1, Text = ")" };
                        var new_rparen = new TerminalNodeImpl(rparen_token);
                        new_block.AddChild(new_rparen);
                        new_rparen.Parent = new_block;
                    }
                }
                {
                    var token3 = new CommonToken(ANTLRv4Parser.SEMI) { Line = -1, Column = -1, Text = ";" };
                    var new_semi = new TerminalNodeImpl(token3);
                    new_a_rule.AddChild(new_semi);
                    new_semi.Parent = new_a_rule;
                }
                {
                    TreeEdits.CopyTreeRecursive(r.exceptionGroup(), new_a_rule, text_before);
                }
                // Now have "A : <ruleBlock
                //                  <ruleAltList
                //                     <labeledAlt
                //                        <alternative
                //                           <element
                //                              <ebnf
                //                                 <block
                //                                    '('
                //                                       <altList1>
                //                                    ')'
                //                           >  >  >
                //                           <element
                //                              <ebnf
                //                                 <block
                //                                    '('
                //                                       <altList2>
                //                                    ')'
                //                                 >
                //                                 <blockSuffix
                //                                    <ebnfSuffix
                //                                       STAR
                //               >  >  >  >  >  >  >  >  ;  <eg>"
                {
                    bool first1 = true;
                    bool first2 = true;
                    foreach (ANTLRv4Parser.AlternativeContext alt in EnumeratorOfAlts(rule))
                    {
                        ANTLRv4Parser.AtomContext atom = null;
                        if (has_direct_left_recursion && !has_direct_right_recursion)
                            atom = EnumeratorOfRHS(alt)?.FirstOrDefault();
                        else
                            atom = EnumeratorOfRHS(alt)?.LastOrDefault();
                        if (lhs == atom?.GetText())
                        {
                            if (!first1)
                            {
                                var token4 = new CommonToken(ANTLRv4Lexer.OR) { Line = -1, Column = -1, Text = "|" };
                                var new_or = new TerminalNodeImpl(token4);
                                altlist2.AddChild(new_or);
                                new_or.Parent = altlist2;
                            }
                            first1 = false;
                            ANTLRv4Parser.LabeledAltContext l_alt = new ANTLRv4Parser.LabeledAltContext(null, 0);
                            altlist2.AddChild(l_alt);
                            l_alt.Parent = altlist2;
                            ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                            l_alt.AddChild(new_alt);
                            new_alt.Parent = l_alt;
                            ANTLRv4Parser.ElementContext[] elements = alt.element();
                            var i = (has_direct_left_recursion && !has_direct_right_recursion) ? 1 : 0;
                            var j = (has_direct_left_recursion && !has_direct_right_recursion) ? elements.Length : elements.Length - 1;
                            for (; i < j; ++i)
                            {
                                var element = elements[i];
                                TreeEdits.CopyTreeRecursive(element, new_alt, text_before);
                            }
                        }
                        else
                        {
                            if (!first2)
                            {
                                var token4 = new CommonToken(ANTLRv4Lexer.OR) { Line = -1, Column = -1, Text = "|" };
                                var new_or = new TerminalNodeImpl(token4);
                                altlist1.AddChild(new_or);
                                new_or.Parent = altlist1;
                            }
                            first2 = false;
                            ANTLRv4Parser.LabeledAltContext l_alt = new ANTLRv4Parser.LabeledAltContext(null, 0);
                            altlist1.AddChild(l_alt);
                            l_alt.Parent = altlist1;
                            ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                            l_alt.AddChild(new_alt);
                            new_alt.Parent = l_alt;
                            foreach (var element in alt.element())
                            {
                                TreeEdits.CopyTreeRecursive(element, new_alt, text_before);
                            }
                        }
                    }
                }
            }
            return (IParseTree)new_a_rule;
        }

        public static Dictionary<string, string> ConvertRecursionToKleeneOperator(int index, Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if initial file is a grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            ExtractGrammarType egt = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(egt, pd_parser.ParseTree);
            bool is_grammar = egt.Type == ExtractGrammarType.GrammarType.Parser
                || egt.Type == ExtractGrammarType.GrammarType.Combined
                || egt.Type == ExtractGrammarType.GrammarType.Lexer;
            if (!is_grammar)
            {
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            }

            // Find all other grammars by walking dependencies (import, vocab, file names).
            HashSet<string> read_files = new HashSet<string>
            {
                document.FullPath
            };
            Dictionary<Workspaces.Document, List<TerminalNodeImpl>> every_damn_literal =
                new Dictionary<Workspaces.Document, List<TerminalNodeImpl>>();
            for (; ; )
            {
                int before_count = read_files.Count;
                foreach (string f in read_files)
                {
                    List<string> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Value.Contains(f)).Select(
                        t => t.Key).ToList();
                    read_files = read_files.Union(additional).ToHashSet();
                }
                foreach (string f in read_files)
                {
                    IEnumerable<List<string>> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Key == f).Select(
                        t => t.Value);
                    foreach (List<string> t in additional)
                    {
                        read_files = read_files.Union(t).ToHashSet();
                    }
                }
                int after_count = read_files.Count;
                if (after_count == before_count)
                {
                    break;
                }
            }

            // Assume cursor positioned at the rule that contains left recursion.
            // Find rule.
            IParseTree rule = null;
            IParseTree it = pd_parser.AllNodes.Where(n =>
            {
                if (!(n is ANTLRv4Parser.ParserRuleSpecContext || n is ANTLRv4Parser.LexerRuleSpecContext))
                    return false;
                Interval source_interval = n.SourceInterval;
                int a = source_interval.a;
                int b = source_interval.b;
                IToken ta = pd_parser.TokStream.Get(a);
                IToken tb = pd_parser.TokStream.Get(b);
                var start = ta.StartIndex;
                var stop = tb.StopIndex + 1;
                return start <= index && index < stop;
            }).FirstOrDefault();
            rule = it ?? throw new LanguageServerException("A parser rule is not selected. Please select one first.");

            // We are now at the rule that the user identified to eliminate direct
            // left recursion.
            // Check if the rule has direct left recursion.

            bool has_direct_left_recursion = HasDirectLeftRecursion(rule);
            bool has_direct_right_recursion = HasDirectRightRecursion(rule);
            if (!(has_direct_left_recursion || has_direct_right_recursion))
            {
                throw new LanguageServerException("The rule selected does not have direct left or right recursion. Please select one first.");
            }
            else if (has_direct_left_recursion && has_direct_right_recursion)
            {
                throw new LanguageServerException("The rule selected has both direct left or right recursion. Please select one first.");
            }

            // Get all intertoken text immediately for source reconstruction.
            var (text_before, other) = TreeEdits.TextToLeftOfLeaves(pd_parser.TokStream, pd_parser.ParseTree);

            // Has direct recursion.
            rule = ReplaceWithKleeneRules(has_direct_left_recursion, has_direct_right_recursion, rule, text_before);
            {
                // Now edit the file and return.
                StringBuilder sb = new StringBuilder();
                int pre = 0;
                Reconstruct(sb, pd_parser.TokStream, pd_parser.ParseTree, ref pre,
                    x =>
                    {
                        if (x is ANTLRv4Parser.ParserRuleSpecContext)
                        {
                            var y = x as ANTLRv4Parser.ParserRuleSpecContext;
                            var name = y.RULE_REF()?.GetText();
                            if (name == Lhs((ANTLRv4Parser.ParserRuleSpecContext)rule).GetText())
                            {
                                StringBuilder sb2 = new StringBuilder();

                                TreeOutput.OutputTree(rule, pd_parser.TokStream);

                                Output(sb2, pd_parser.TokStream, rule);
                                return sb2.ToString();
                            }
                        }
                        return null;
                    });
                var new_code = sb.ToString();
                if (new_code != pd_parser.Code)
                {
                    result.Add(document.FullPath, new_code);
                }
            }

            return result;
        }

        private static string GenerateNewName(IParseTree rule, AntlrGrammarDetails pd_parser)
        {
            if (!(rule is ANTLRv4Parser.ParserRuleSpecContext r))
                return null;

            var b = r.RULE_REF().GetText();
            var list = pd_parser.AllNodes.Where(n =>
            {
                return (n is ANTLRv4Parser.ParserRuleSpecContext || n is ANTLRv4Parser.LexerRuleSpecContext);
            }).Select(n =>
            {
                if (n is ANTLRv4Parser.ParserRuleSpecContext)
                {
                    var z = n as ANTLRv4Parser.ParserRuleSpecContext;
                    var lhs = z.RULE_REF();
                    return lhs.GetText();
                }
                return "";
            }).ToList();
            int gnum = 1;
            for (; ; )
            {
                if (!list.Contains(b + gnum.ToString()))
                    break;
                gnum++;
            }
            return b + gnum.ToString();
        }

        public static Dictionary<string, string> EliminateDirectLeftRecursion(int index, Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if initial file is a grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            ExtractGrammarType egt = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(egt, pd_parser.ParseTree);
            bool is_grammar = egt.Type == ExtractGrammarType.GrammarType.Parser
                || egt.Type == ExtractGrammarType.GrammarType.Combined
                || egt.Type == ExtractGrammarType.GrammarType.Lexer;
            if (!is_grammar)
            {
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            }

            // Find all other grammars by walking dependencies (import, vocab, file names).
            HashSet<string> read_files = new HashSet<string>
            {
                document.FullPath
            };
            Dictionary<Workspaces.Document, List<TerminalNodeImpl>> every_damn_literal =
                new Dictionary<Workspaces.Document, List<TerminalNodeImpl>>();
            for (; ; )
            {
                int before_count = read_files.Count;
                foreach (string f in read_files)
                {
                    List<string> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Value.Contains(f)).Select(
                        t => t.Key).ToList();
                    read_files = read_files.Union(additional).ToHashSet();
                }
                foreach (string f in read_files)
                {
                    IEnumerable<List<string>> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Key == f).Select(
                        t => t.Value);
                    foreach (List<string> t in additional)
                    {
                        read_files = read_files.Union(t).ToHashSet();
                    }
                }
                int after_count = read_files.Count;
                if (after_count == before_count)
                {
                    break;
                }
            }

            // Assume cursor positioned at the rule that contains left recursion.
            // Find rule.
            IParseTree rule = null;
            IParseTree it = pd_parser.AllNodes.Where(n =>
            {
                if (!(n is ANTLRv4Parser.ParserRuleSpecContext || n is ANTLRv4Parser.LexerRuleSpecContext))
                    return false;
                Interval source_interval = n.SourceInterval;
                int a = source_interval.a;
                int b = source_interval.b;
                IToken ta = pd_parser.TokStream.Get(a);
                IToken tb = pd_parser.TokStream.Get(b);
                var start = ta.StartIndex;
                var stop = tb.StopIndex + 1;
                return start <= index && index < stop;
            }).FirstOrDefault();
            rule = it ?? throw new LanguageServerException("A parser rule is not selected. Please select one first.");

            // We are now at the rule that the user identified to eliminate direct
            // left recursion.
            // Check if the rule has direct left recursion.

            bool has_direct_left_recursion = HasDirectLeftRecursion(rule);
            if (!has_direct_left_recursion)
            {
                throw new LanguageServerException("Parser rule selected does not have direct left recursion. Please select another rule.");
            }

            // Has direct left recursion.

            // Replace rule with two new rules.
            //
            // Original rule:
            // A
            //   : A a1
            //   | A a2
            //   | A a3
            //   | B1
            //   | B2
            //   ...
            //   ;
            // Note a1, a2, a3 ... cannot be empty sequences.
            // B1, B2, ... cannot start with A.
            //
            // New rules.
            //
            // A
            //   : B1 A'
            //   | B2 A'
            //   | ...
            //   ;
            // A'
            //   : a1 A'
            //   | a2 A'
            //   | ...
            //   | (empty)
            //   ;
            //

            // Get all intertoken text immediately for source reconstruction.
            var (text_before, other) = TreeEdits.TextToLeftOfLeaves(pd_parser.TokStream, pd_parser.ParseTree);
            
            string generated_name = GenerateNewName(rule, pd_parser);

            {
                ANTLRv4Parser.ParserRuleSpecContext new_a_rule = new ANTLRv4Parser.ParserRuleSpecContext(null, 0);
                {
                    var r = rule as ANTLRv4Parser.ParserRuleSpecContext;
                    var old_lhs = r.RULE_REF();
                    var lhs = old_lhs.GetText();
                    {
                        var new_lhs = TreeEdits.CopyTreeRecursive(old_lhs, new_a_rule, text_before);
                    }
                    // Now have "A"
                    {
                        var token2 = new CommonToken(ANTLRv4Parser.COLON) { Line = -1, Column = -1, Text = ":" };
                        var new_colon = new TerminalNodeImpl(token2);
                        new_a_rule.AddChild(new_colon);
                        new_colon.Parent = new_a_rule;
                    }
                    // Now have "A :"
                    ANTLRv4Parser.RuleAltListContext rule_alt_list = new ANTLRv4Parser.RuleAltListContext(null, 0);
                    {
                        ANTLRv4Parser.RuleBlockContext new_rule_block_context = new ANTLRv4Parser.RuleBlockContext(new_a_rule, 0);
                        new_a_rule.AddChild(new_rule_block_context);
                        new_rule_block_context.Parent = new_a_rule;
                        new_rule_block_context.AddChild(rule_alt_list);
                        rule_alt_list.Parent = new_rule_block_context;
                    }
                    // Now have "A : <rb <ral> >"
                    {
                        var token3 = new CommonToken(ANTLRv4Parser.SEMI) { Line = -1, Column = -1, Text = ";" };
                        var new_semi = new TerminalNodeImpl(token3);
                        new_a_rule.AddChild(new_semi);
                        new_semi.Parent = new_a_rule;
                    }
                    // Now have "A : <rb <ral> > ;"
                    {
                        TreeEdits.CopyTreeRecursive(r.exceptionGroup(), new_a_rule, text_before);
                    }
                    // Now have "A : <rb <ral> > ; <eg>"
                    bool first = true;
                    foreach (ANTLRv4Parser.AlternativeContext alt in EnumeratorOfAlts(rule))
                    {
                        ANTLRv4Parser.AtomContext atom = EnumeratorOfRHS(alt)?.FirstOrDefault();
                        if (lhs == atom?.GetText())
                        {
                            // skip alts that have direct left recursion.
                            continue;
                        }
                        {
                            if (!first)
                            {
                                var token4 = new CommonToken(ANTLRv4Lexer.OR) { Line = -1, Column = -1, Text = "|" };
                                var new_or = new TerminalNodeImpl(token4);
                                rule_alt_list.AddChild(new_or);
                                new_or.Parent = rule_alt_list;
                            }
                            first = false;
                        }
                        ANTLRv4Parser.LabeledAltContext l_alt = new ANTLRv4Parser.LabeledAltContext(rule_alt_list, 0);
                        rule_alt_list.AddChild(l_alt);
                        l_alt.Parent = rule_alt_list;
                        // Create new alt "beta A'".
                        ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                        l_alt.AddChild(new_alt);
                        new_alt.Parent = l_alt;
                        foreach (var element in alt.element())
                        {
                            TreeEdits.CopyTreeRecursive(element, new_alt, text_before);
                        }
                        var token = new CommonToken(ANTLRv4Lexer.RULE_REF) { Line = -1, Column = -1, Text = generated_name };
                        var new_rule_ref = new TerminalNodeImpl(token);
                        var new_ruleref = new ANTLRv4Parser.RulerefContext(null, 0);
                        new_ruleref.AddChild(new_rule_ref);
                        new_rule_ref.Parent = new_ruleref;
                        var new_atom = new ANTLRv4Parser.AtomContext(null, 0);
                        new_atom.AddChild(new_ruleref);
                        new_ruleref.Parent = new_atom;
                        var new_element = new ANTLRv4Parser.ElementContext(null, 0);
                        new_element.AddChild(new_atom);
                        new_atom.Parent = new_element;
                        new_alt.AddChild(new_element);
                        new_element.Parent = new_alt;
                    }
                }
                // Now have "A : beta1 A' | beta2 A' | ... ; <eg>"

                ANTLRv4Parser.ParserRuleSpecContext new_ap_rule = new ANTLRv4Parser.ParserRuleSpecContext(null, 0);
                {
                    var r = rule as ANTLRv4Parser.ParserRuleSpecContext;
                    var lhs = r.RULE_REF()?.GetText();
                    {
                        var token = new CommonToken(ANTLRv4Parser.RULE_REF) { Line = -1, Column = -1, Text = generated_name };
                        var new_rule_ref = new TerminalNodeImpl(token);
                        text_before.Add(new_rule_ref,
                            System.Environment.NewLine + System.Environment.NewLine);
                        new_ap_rule.AddChild(new_rule_ref);
                        new_rule_ref.Parent = new_ap_rule;
                    }
                    // Now have "A'"
                    {
                        var token2 = new CommonToken(ANTLRv4Parser.COLON) { Line = -1, Column = -1, Text = ":" };
                        var new_colon = new TerminalNodeImpl(token2);
                        new_ap_rule.AddChild(new_colon);
                        new_colon.Parent = new_ap_rule;
                    }
                    // Now have "A' :"
                    ANTLRv4Parser.RuleAltListContext rule_alt_list = new ANTLRv4Parser.RuleAltListContext(null, 0);
                    {
                        ANTLRv4Parser.RuleBlockContext new_rule_block_context = new ANTLRv4Parser.RuleBlockContext(new_a_rule, 0);
                        new_ap_rule.AddChild(new_rule_block_context);
                        new_rule_block_context.Parent = new_ap_rule;
                        new_rule_block_context.AddChild(rule_alt_list);
                        rule_alt_list.Parent = new_rule_block_context;
                    }
                    // Now have "A' : <rb <ral> >"
                    {
                        var token3 = new CommonToken(ANTLRv4Parser.SEMI) { Line = -1, Column = -1, Text = ";" };
                        var new_semi = new TerminalNodeImpl(token3);
                        new_ap_rule.AddChild(new_semi);
                        new_semi.Parent = new_ap_rule;
                    }
                    // Now have "A : <rb <ral> > ;"
                    {
                        TreeEdits.CopyTreeRecursive(r.exceptionGroup(), new_a_rule, text_before);
                    }
                    // Now have "A' : <rb <ral> > ; <eg>"
                    bool first = true;
                    foreach (ANTLRv4Parser.AlternativeContext alt in EnumeratorOfAlts(rule))
                    {
                        ANTLRv4Parser.AtomContext atom = EnumeratorOfRHS(alt)?.FirstOrDefault();
                        if (lhs != atom?.GetText())
                        {
                            // skip alts that DO NOT have direct left recursion.
                            continue;
                        }
                        {
                            if (!first)
                            {
                                var token4 = new CommonToken(ANTLRv4Lexer.OR) { Line = -1, Column = -1, Text = "|" };
                                var new_or = new TerminalNodeImpl(token4);
                                rule_alt_list.AddChild(new_or);
                                new_or.Parent = rule_alt_list;
                            }
                            first = false;
                        }
                        ANTLRv4Parser.LabeledAltContext l_alt = new ANTLRv4Parser.LabeledAltContext(rule_alt_list, 0);
                        rule_alt_list.AddChild(l_alt);
                        l_alt.Parent = rule_alt_list;
                        // Create new alt "alpha A'".
                        ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                        l_alt.AddChild(new_alt);
                        new_alt.Parent = l_alt;
                        bool first2 = true;
                        foreach (var element in alt.element())
                        {
                            if (first2)
                            {
                                first2 = false;
                                continue;
                            }
                            TreeEdits.CopyTreeRecursive(element, new_alt, text_before);
                        }
                        var token = new CommonToken(ANTLRv4Parser.RULE_REF) { Line = -1, Column = -1, Text = generated_name };
                        var new_rule_ref = new TerminalNodeImpl(token);
                        var new_ruleref = new ANTLRv4Parser.RulerefContext(null, 0);
                        new_ruleref.AddChild(new_rule_ref);
                        new_rule_ref.Parent = new_ruleref;
                        var new_atom = new ANTLRv4Parser.AtomContext(null, 0);
                        new_atom.AddChild(new_ruleref);
                        new_ruleref.Parent = new_atom;
                        var new_element = new ANTLRv4Parser.ElementContext(null, 0);
                        new_element.AddChild(new_atom);
                        new_atom.Parent = new_element;
                        new_alt.AddChild(new_element);
                        new_element.Parent = new_alt;
                    }
                    {
                        if (!first)
                        {
                            var token4 = new CommonToken(ANTLRv4Lexer.OR) { Line = -1, Column = -1, Text = "|" };
                            var new_or = new TerminalNodeImpl(token4);
                            rule_alt_list.AddChild(new_or);
                            new_or.Parent = rule_alt_list;
                        }
                        first = false;
                        ANTLRv4Parser.LabeledAltContext l_alt = new ANTLRv4Parser.LabeledAltContext(rule_alt_list, 0);
                        rule_alt_list.AddChild(l_alt);
                        l_alt.Parent = rule_alt_list;
                        // Create new empty alt.
                        ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                        l_alt.AddChild(new_alt);
                        new_alt.Parent = l_alt;
                    }
                }
                // Now have "A' : alpha1 A' | alpha2 A' | ... ;"

                // Replace rule and add new rule afterwards.
                {
                    var rs = rule.Parent;
                    var rule_spec = rs as ANTLRv4Parser.RuleSpecContext;
                    var r = rule_spec.Parent;
                    var rules = r as ANTLRv4Parser.RulesContext;
                    TreeEdits.Replace(rule_spec, n =>
                    {
                        if (n == rule)
                            return new_a_rule;
                        return null;
                    });
                    var new_rs = new ANTLRv4Parser.RuleSpecContext(null, 0);
                    new_rs.AddChild(new_ap_rule);
                    new_ap_rule.Parent = new_rs;
                    int i = 0;
                    for (; i < rules.ChildCount; ++i)
                    {
                        if (rules.GetChild(i) == rule_spec)
                            break;
                    }
                    rules.children.Insert(i + 1, new_rs);
                    new_rs.Parent = rules;
                }
            }

            StringBuilder sb = new StringBuilder();
            TreeEdits.Reconstruct(sb, pd_parser.ParseTree, text_before);
            var new_code = sb.ToString();
            if (new_code != pd_parser.Code)
            {
                result.Add(document.FullPath, new_code);
            }

            return result;
        }

        public static Dictionary<string, string> EliminateIndirectLeftRecursion(int index, Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if initial file is a grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            ExtractGrammarType egt = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(egt, pd_parser.ParseTree);
            bool is_grammar = egt.Type == ExtractGrammarType.GrammarType.Parser
                || egt.Type == ExtractGrammarType.GrammarType.Combined
                || egt.Type == ExtractGrammarType.GrammarType.Lexer;
            if (!is_grammar)
            {
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            }

            // Find all other grammars by walking dependencies (import, vocab, file names).
            HashSet<string> read_files = new HashSet<string>
            {
                document.FullPath
            };
            Dictionary<Workspaces.Document, List<TerminalNodeImpl>> every_damn_literal =
                new Dictionary<Workspaces.Document, List<TerminalNodeImpl>>();
            for (; ; )
            {
                int before_count = read_files.Count;
                foreach (string f in read_files)
                {
                    List<string> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Value.Contains(f)).Select(
                        t => t.Key).ToList();
                    read_files = read_files.Union(additional).ToHashSet();
                }
                foreach (string f in read_files)
                {
                    IEnumerable<List<string>> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Key == f).Select(
                        t => t.Value);
                    foreach (List<string> t in additional)
                    {
                        read_files = read_files.Union(t).ToHashSet();
                    }
                }
                int after_count = read_files.Count;
                if (after_count == before_count)
                {
                    break;
                }
            }

            // Construct graph of symbol usage.
            TableOfRules table = new TableOfRules(pd_parser, document);
            table.ReadRules();
            table.FindPartitions();
            table.FindStartRules();
            Digraph<string> graph = new Digraph<string>();
            foreach (TableOfRules.Row r in table.rules)
            {
                if (!r.is_parser_rule)
                {
                    continue;
                }
                graph.AddVertex(r.LHS);
            }
            foreach (TableOfRules.Row r in table.rules)
            {
                if (!r.is_parser_rule)
                {
                    continue;
                }
                List<string> j = r.RHS;
                //j.Reverse();
                foreach (string rhs in j)
                {
                    TableOfRules.Row sym = table.rules.Where(t => t.LHS == rhs).FirstOrDefault();
                    if (!sym.is_parser_rule)
                    {
                        continue;
                    }
                    DirectedEdge<string> e = new DirectedEdge<string>(r.LHS, rhs);
                    graph.AddEdge(e);
                }
            }
            List<string> starts = new List<string>();
            List<string> parser_lhs_rules = new List<string>();
            foreach (TableOfRules.Row r in table.rules)
            {
                if (r.is_parser_rule)
                {
                    parser_lhs_rules.Add(r.LHS);
                    if (r.is_start)
                    {
                        starts.Add(r.LHS);
                    }
                }
            }

            // We are only going to eliminate the indirect recursion of what is being pointed to.
            // Check rule and graph.
            // Assume cursor positioned at the rule that participates in indirect left recursion.
            // Find rule.
            IParseTree rule = null;
            IParseTree it = pd_parser.AllNodes.Where(n =>
            {
                if (!(n is ANTLRv4Parser.ParserRuleSpecContext || n is ANTLRv4Parser.LexerRuleSpecContext))
                    return false;
                Interval source_interval = n.SourceInterval;
                int a = source_interval.a;
                int b = source_interval.b;
                IToken ta = pd_parser.TokStream.Get(a);
                IToken tb = pd_parser.TokStream.Get(b);
                var start = ta.StartIndex;
                var stop = tb.StopIndex + 1;
                return start <= index && index < stop;
            }).FirstOrDefault();
            if (it == null)
            {
                return result;
            }
            rule = it;
            var k = (ANTLRv4Parser.ParserRuleSpecContext)rule;
            var tarjan = new TarjanSCC<string, DirectedEdge<string>>(graph);
            List<string> ordered = new List<string>();
            var sccs = tarjan.Compute();
            var scc = sccs[k.RULE_REF().ToString()];
            foreach (var v in scc)
            {
                ordered.Add(v);
            }

            // We are now at the rule that the user identified to eliminate indirect
            // left recursion.
            // Check if the rule participates in indirect left recursion.

            // Get all intertoken text immediately for source reconstruction.
            var (text_before, other) = TreeEdits.TextToLeftOfLeaves(pd_parser.TokStream, pd_parser.ParseTree);

            Dictionary<string, IParseTree> rules = new Dictionary<string, IParseTree>();
            foreach (string s in ordered)
            {
                var ai = table.rules.Where(r => r.LHS == s).First();
                var air = (ANTLRv4Parser.ParserRuleSpecContext)ai.rule;
                rules[s] = TreeEdits.CopyTreeRecursive(air, null, text_before);
            }
            for (int i = 0; i < ordered.Count; ++i)
            {
                var ai = ordered[i];
                var ai_tree = rules[ai];
                ANTLRv4Parser.ParserRuleSpecContext new_a_rule = new ANTLRv4Parser.ParserRuleSpecContext(null, 0);
                var r = ai_tree as ANTLRv4Parser.ParserRuleSpecContext;
                var lhs = r.RULE_REF()?.GetText();
                TreeEdits.CopyTreeRecursive(r.RULE_REF(), new_a_rule, text_before);
                // Now have "A"
                {
                    var token2 = new CommonToken(ANTLRv4Parser.COLON) { Line = -1, Column = -1, Text = ":" };
                    var new_colon = new TerminalNodeImpl(token2);
                    new_a_rule.AddChild(new_colon);
                    new_colon.Parent = new_a_rule;
                }
                // Now have "A :"
                ANTLRv4Parser.RuleAltListContext rule_alt_list = new ANTLRv4Parser.RuleAltListContext(null, 0);
                {
                    ANTLRv4Parser.RuleBlockContext new_rule_block_context = new ANTLRv4Parser.RuleBlockContext(new_a_rule, 0);
                    new_a_rule.AddChild(new_rule_block_context);
                    new_rule_block_context.Parent = new_a_rule;
                    new_rule_block_context.AddChild(rule_alt_list);
                    rule_alt_list.Parent = new_rule_block_context;
                }
                // Now have "A : <rb <ral> >"
                {
                    var token3 = new CommonToken(ANTLRv4Parser.SEMI) { Line = -1, Column = -1, Text = ";" };
                    var new_semi = new TerminalNodeImpl(token3);
                    new_a_rule.AddChild(new_semi);
                    new_semi.Parent = new_a_rule;
                }
                // Now have "A : <rb <ral> > ;"
                {
                    TreeEdits.CopyTreeRecursive(r.exceptionGroup(), new_a_rule, text_before);
                }
                // Now have "A : <rb <ral> > ; <eg>"
                for (int j = 0; j < i; ++j)
                {
                    var aj = ordered[j];
                    var aj_tree = rules[aj];
                    var new_alts = new List<ANTLRv4Parser.AlternativeContext>();
                    foreach (ANTLRv4Parser.AlternativeContext alt in EnumeratorOfAlts(ai_tree))
                    {
                        ANTLRv4Parser.AtomContext atom = EnumeratorOfRHS(alt)?.FirstOrDefault();
                        if (aj != atom?.GetText())
                        {
                            // Leave alt unchanged.
                            new_alts.Add(alt);
                            continue;
                        }
                        
                        // Substitute Aj into Ai.
                        // Example:
                        // s : a A | B;
                        // a : a C | s D | ;
                        // ts order of symbols = [s, a].
                        // i = 1, j = 0.
                        // => a : a C | a A D | B D | ;

                        foreach (ANTLRv4Parser.AlternativeContext alt2 in EnumeratorOfAlts(aj_tree))
                        {
                            ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                            foreach (var element in alt2.element())
                            {
                                TreeEdits.CopyTreeRecursive(element, new_alt, text_before);
                            }
                            bool first = true;
                            foreach (var element in alt.element())
                            {
                                if (first)
                                {
                                    first = false;
                                    continue;
                                }
                                TreeEdits.CopyTreeRecursive(element, new_alt, text_before);
                            }
                            new_alts.Add(new_alt);
                        }
                    }
                    {
                        bool first = true;
                        foreach (var new_alt in new_alts)
                        {
                            if (!first)
                            {
                                var token4 = new CommonToken(ANTLRv4Lexer.OR) { Line = -1, Column = -1, Text = "|" };
                                var new_or = new TerminalNodeImpl(token4);
                                rule_alt_list.AddChild(new_or);
                                new_or.Parent = rule_alt_list;
                            }
                            first = false;
                            ANTLRv4Parser.LabeledAltContext l_alt = new ANTLRv4Parser.LabeledAltContext(rule_alt_list, 0);
                            rule_alt_list.AddChild(l_alt);
                            l_alt.Parent = rule_alt_list;
                            l_alt.AddChild(new_alt);
                            new_alt.Parent = l_alt;
                        }
                    }
                    rules[ai] = new_a_rule;
                }

                // Check if the rule ai has direct left recursion.
                bool has_direct_left_recursion = HasDirectLeftRecursion(rules[ai]);
                if (!has_direct_left_recursion)
                {
                    continue;
                }

                // Has direct left recursion.

                // Replace rule with two new rules.
                //
                // Original rule:
                // A
                //   : A a1
                //   | A a2
                //   | A a3
                //   | B1
                //   | B2
                //   ...
                //   ;
                // Note a1, a2, a3 ... cannot be empty sequences.
                // B1, B2, ... cannot start with A.
                //
                // New rules.
                //
                // A
                //   : B1 A'
                //   | B2 A'
                //   | ...
                //   ;
                // A'
                //   : a1 A'
                //   | a2 A'
                //   | ...
                //   | (empty)
                //   ;
                //

                string generated_name = GenerateNewName(rules[ai], pd_parser);
                var (fixed_rule, new_rule) = GenerateReplacementRules(generated_name, rules[ai], text_before);
                rules[ai] = fixed_rule;
                rules[generated_name] = new_rule;
            }

            {
                // Now edit the file and return.
                StringBuilder sb = new StringBuilder();
                int pre = 0;
                Reconstruct(sb, pd_parser.TokStream, pd_parser.ParseTree, ref pre,
                    x =>
                    {
                        if (x is ANTLRv4Parser.ParserRuleSpecContext)
                        {
                            var y = x as ANTLRv4Parser.ParserRuleSpecContext;
                            var name = y.RULE_REF()?.GetText();
                            rules.TryGetValue(name, out IParseTree replacement);
                            if (replacement != null)
                            {
                                StringBuilder sb2 = new StringBuilder();
                                Output(sb2, pd_parser.TokStream, replacement);
                                foreach (var r in rules)
                                {
                                    var z = r.Key != name && r.Key.Contains(name);
                                    if (z)
                                    {
                                        sb2.AppendLine();
                                        Output(sb2, pd_parser.TokStream, r.Value);
                                    }
                                }
                                return sb2.ToString();
                            }
                        }
                        return null;
                    });
                var new_code = sb.ToString();
                if (new_code != pd_parser.Code)
                {
                    result.Add(document.FullPath, new_code);
                }
            }
            return result;
        }

        public static ITerminalNode Lhs(ANTLRv4Parser.ParserRuleSpecContext ai)
        {
            ITerminalNode lhs = ai.RULE_REF();
            return lhs;
        }

        public static IEnumerable<ANTLRv4Parser.AlternativeContext> EnumeratorOfAlts(IParseTree ai)
        {
            var r = ai as ANTLRv4Parser.ParserRuleSpecContext;
            ANTLRv4Parser.RuleBlockContext rhs = r.ruleBlock();
            ANTLRv4Parser.RuleAltListContext rule_alt_list = rhs.ruleAltList();
            foreach (ANTLRv4Parser.LabeledAltContext l_alt in rule_alt_list.labeledAlt())
            {
                ANTLRv4Parser.AlternativeContext alt = l_alt.alternative();
                yield return alt;
            }
        }

        public static IEnumerable<ANTLRv4Parser.AtomContext> EnumeratorOfRHS(ANTLRv4Parser.AlternativeContext alt)
        {
            foreach (ANTLRv4Parser.ElementContext element in alt.element())
            {
                ANTLRv4Parser.LabeledElementContext le = element.labeledElement();
                ANTLRv4Parser.AtomContext atom = element.atom();
                if (le != null)
                {
                    yield return le.atom();
                }
                else if (atom != null)
                {
                    yield return atom;
                }
            }
        }

        public static string EliminateAntlrKeywordsInRules(Document document)
        {
            // Check if initial file is a parser or combined grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            ExtractGrammarType egt = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(egt, pd_parser.ParseTree);
            bool is_grammar = egt.Type == ExtractGrammarType.GrammarType.Parser
                || egt.Type == ExtractGrammarType.GrammarType.Combined
                ;
            if (!is_grammar)
            {
                throw new LanguageServerException("A parser or combined grammar file is not selected. Please select one first.");
            }

            TableOfRules table = new TableOfRules(pd_parser, document);
            table.ReadRules();
            table.FindPartitions();
            table.FindStartRules();

            StringBuilder sb = new StringBuilder();
            int pre = 0;
            Reconstruct(sb, pd_parser.TokStream, pd_parser.ParseTree, ref pre,
                n =>
                {
                    if (!(n is TerminalNodeImpl))
                    {
                        return null;
                    }
                    var t = n as TerminalNodeImpl;
                    var r = t.GetText();
                    if (t.Symbol.Type == ANTLRv4Lexer.RULE_REF)
                    {
                        if (r == "options"
                            || r == "grammar"
                            || r == "tokenVocab"
                            || r == "lexer"
                            || r == "parser"
                            || r == "rule")
                            return r + "_nonterminal";
                    }
                    return r;
                });
            return sb.ToString();
        }

        public static Dictionary<string, string> AddLexerRulesForStringLiterals(Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if initial file is a grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            ExtractGrammarType egt = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(egt, pd_parser.ParseTree);
            bool is_grammar = egt.Type == ExtractGrammarType.GrammarType.Parser
                || egt.Type == ExtractGrammarType.GrammarType.Combined
                ;
            if (!is_grammar)
            {
                throw new LanguageServerException("A parser or combined grammar file is not selected. Please select one first.");
            }

            // Find all other grammars by walking dependencies (import, vocab, file names).
            HashSet<string> read_files = new HashSet<string>
            {
                document.FullPath
            };
            Dictionary<Workspaces.Document, List<TerminalNodeImpl>> every_damn_literal =
                new Dictionary<Workspaces.Document, List<TerminalNodeImpl>>();
            for (; ; )
            {
                int before_count = read_files.Count;
                foreach (string f in read_files)
                {
                    List<string> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Value.Contains(f)).Select(
                        t => t.Key).ToList();
                    read_files = read_files.Union(additional).ToHashSet();
                }
                foreach (string f in read_files)
                {
                    IEnumerable<List<string>> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Key == f).Select(
                        t => t.Value);
                    foreach (List<string> t in additional)
                    {
                        read_files = read_files.Union(t).ToHashSet();
                    }
                }
                int after_count = read_files.Count;
                if (after_count == before_count)
                {
                    break;
                }
            }

            // Find rewrite rules, i.e., lexer rule "<TOKEN_REF> : <string literal>"
            Dictionary<string, string> subs = new Dictionary<string, string>();
            foreach (string f in read_files)
            {
                Workspaces.Document whatever_document = Workspaces.Workspace.Instance.FindDocument(f);
                if (whatever_document == null)
                {
                    continue;
                }
                AntlrGrammarDetails pd_whatever = ParserDetailsFactory.Create(whatever_document) as AntlrGrammarDetails;

                // Find literals in grammars.
                LiteralsGrammar lp_whatever = new LiteralsGrammar();
                ParseTreeWalker.Default.Walk(lp_whatever, pd_whatever.ParseTree);
                List<TerminalNodeImpl> list_literals = lp_whatever.Literals;
                foreach (TerminalNodeImpl lexer_literal in list_literals)
                {
                    string old_name = lexer_literal.GetText();
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
                    IRuleNode p1 = lexer_literal.Parent;
                    if (p1.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p1 is ANTLRv4Parser.TerminalContext))
                    {
                        continue;
                    }

                    IRuleNode p2 = p1.Parent;
                    if (p2.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p2 is ANTLRv4Parser.LexerAtomContext))
                    {
                        continue;
                    }

                    IRuleNode p3 = p2.Parent;
                    if (p3.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p3 is ANTLRv4Parser.LexerElementContext))
                    {
                        continue;
                    }

                    IRuleNode p4 = p3.Parent;
                    if (p4.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p4 is ANTLRv4Parser.LexerElementsContext))
                    {
                        continue;
                    }

                    IRuleNode p5 = p4.Parent;
                    if (p5.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p5 is ANTLRv4Parser.LexerAltContext))
                    {
                        continue;
                    }

                    IRuleNode p6 = p5.Parent;
                    if (p6.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p6 is ANTLRv4Parser.LexerAltListContext))
                    {
                        continue;
                    }

                    IRuleNode p7 = p6.Parent;
                    if (p7.ChildCount != 1)
                    {
                        continue;
                    }

                    if (!(p7 is ANTLRv4Parser.LexerRuleBlockContext))
                    {
                        continue;
                    }

                    IRuleNode p8 = p7.Parent;
                    if (p8.ChildCount != 4)
                    {
                        continue;
                    }

                    if (!(p8 is ANTLRv4Parser.LexerRuleSpecContext))
                    {
                        continue;
                    }

                    IParseTree alt = p8.GetChild(0);
                    string new_name = alt.GetText();
                    subs.Add(old_name, new_name);
                }
            }

            // Determine where to put any new rules.
            string where_to_stuff = null;
            foreach (string f in read_files)
            {
                Workspaces.Document whatever_document = Workspaces.Workspace.Instance.FindDocument(f);
                if (whatever_document == null)
                {
                    continue;
                }
                AntlrGrammarDetails pd_whatever = ParserDetailsFactory.Create(whatever_document) as AntlrGrammarDetails;
                ExtractGrammarType x1 = new ExtractGrammarType();
                ParseTreeWalker.Default.Walk(x1, pd_whatever.ParseTree);
                bool is_right_grammar =
                           x1.Type == ExtractGrammarType.GrammarType.Combined
                           || x1.Type == ExtractGrammarType.GrammarType.Lexer;
                if (!is_right_grammar)
                    continue;
                if (where_to_stuff != null)
                    return null;
                where_to_stuff = f;
            }

            // Find string literals in parser and combined grammars and substitute.
            Dictionary<string, string> new_subs = new Dictionary<string, string>();
            foreach (string f in read_files)
            {
                Workspaces.Document whatever_document = Workspaces.Workspace.Instance.FindDocument(f);
                if (whatever_document == null)
                {
                    continue;
                }
                AntlrGrammarDetails pd_whatever = ParserDetailsFactory.Create(whatever_document) as AntlrGrammarDetails;
                StringBuilder sb = new StringBuilder();
                int pre = 0;
                Reconstruct(sb, pd_parser.TokStream, pd_parser.ParseTree, ref pre,
                n =>
                {
                    if (!(n is TerminalNodeImpl))
                    {
                        return null;
                    }
                    var t = n as TerminalNodeImpl;
                    if (t.Payload.Type != ANTLRv4Lexer.STRING_LITERAL)
                    {
                        return t.GetText();
                    }
                    bool no = false;
                        // Make sure this literal does not appear in lexer rule.
                        for (IRuleNode p = t.Parent; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.LexerRuleSpecContext)
                        {
                            no = true;
                            break;
                        }
                    }
                    if (no)
                    {
                        return t.GetText();
                    }
                    var r = t.GetText();
                    subs.TryGetValue(r, out string value);
                    if (value == null)
                    {
                        string now = DateTime.Now.ToString()
                            .Replace("/", "_")
                            .Replace(":", "_")
                            .Replace(" ", "_");
                        var new_r = "GENERATED_" + now;
                        subs[r] = new_r;
                        new_subs[r] = new_r;
                    }
                    return r;
                });
                var new_code = sb.ToString();
                if (new_code != pd_parser.Code)
                {
                    result.Add(f, new_code);
                }
            }
            if (new_subs.Count > 0)
            {
                result.TryGetValue(where_to_stuff, out string old_code);
                if (old_code == null)
                {
                    Workspaces.Document whatever_document = Workspaces.Workspace.Instance.FindDocument(where_to_stuff);
                    AntlrGrammarDetails pd_whatever = ParserDetailsFactory.Create(whatever_document) as AntlrGrammarDetails;
                    old_code = pd_whatever.Code;
                }
                StringBuilder sb = new StringBuilder();
                sb.Append(old_code);
                sb.AppendLine();
                foreach (var sub in new_subs)
                {
                    sb.AppendLine(sub.Value + " : " + sub.Key + " ;");
                }
                var new_code = sb.ToString();
                result[where_to_stuff] = new_code;
            }

            return result;
        }

        public static Dictionary<string, string> SortModes(Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if lexer grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            if (lp.Type != ExtractGrammarType.GrammarType.Lexer)
                throw new LanguageServerException("A lexer grammar file is not selected. Please select one first.");
            TableOfModes table = new TableOfModes(pd_parser, document);
            table.ReadModes();
            table.FindPartitions();

            // Find new order of modes.
            string old_code = document.Code;
            List<Pair<int, int>> reorder = new List<Pair<int, int>>();
            {
                List<string> ordered = table.modes
                    .Select(r => r.name)
                    .OrderBy(r => r).ToList();
                foreach (string s in ordered)
                {
                    TableOfModes.Row row = table.modes[table.name_to_index[s]];
                    reorder.Add(new Pair<int, int>(row.start_index, row.end_index));
                }
            }

            StringBuilder sb = new StringBuilder();
            int previous = 0;
            {
                int index_start = table.modes[0].start_index;
                int len = 0;
                string pre = old_code.Substring(previous, index_start - previous);
                sb.Append(pre);
                previous = index_start + len;
            }
            foreach (Pair<int, int> l in reorder)
            {
                int index_start = l.a;
                int len = l.b - l.a;
                string add = old_code.Substring(index_start, len);
                sb.Append(add);
            }
            //string rest = old_code.Substring(previous);
            //sb.Append(rest);
            string new_code = sb.ToString();
            if (new_code != pd_parser.Code)
            {
                result.Add(document.FullPath, new_code);
            }
            return result;
        }

        public static Dictionary<string, string> Unfold(int index, Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if initial file is a grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            ExtractGrammarType egt = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(egt, pd_parser.ParseTree);
            bool is_grammar = egt.Type == ExtractGrammarType.GrammarType.Parser
                || egt.Type == ExtractGrammarType.GrammarType.Combined
                || egt.Type == ExtractGrammarType.GrammarType.Lexer;
            if (!is_grammar)
            {
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            }

            // Find all other grammars by walking dependencies (import, vocab, file names).
            HashSet<string> read_files = new HashSet<string>
            {
                document.FullPath
            };
            Dictionary<Workspaces.Document, List<TerminalNodeImpl>> every_damn_literal =
                new Dictionary<Workspaces.Document, List<TerminalNodeImpl>>();
            for (; ; )
            {
                int before_count = read_files.Count;
                foreach (string f in read_files)
                {
                    List<string> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Value.Contains(f)).Select(
                        t => t.Key).ToList();
                    read_files = read_files.Union(additional).ToHashSet();
                }
                foreach (string f in read_files)
                {
                    IEnumerable<List<string>> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Key == f).Select(
                        t => t.Value);
                    foreach (List<string> t in additional)
                    {
                        read_files = read_files.Union(t).ToHashSet();
                    }
                }
                int after_count = read_files.Count;
                if (after_count == before_count)
                {
                    break;
                }
            }

            // Check cursor position. It is either the LHS symbol of a rule,
            // which means the user wants to unroll all applied occurrences of the rule
            // or it is on a symbol in the RHS of the rule, which means the
            // user wants to unroll this specific applied occurrence of the rule.
            var refs_and_defs = Module.FindRefsAndDefs(index, document);
            var defs = Module.FindDefs(index, document);
            var sym = Module.GetDocumentSymbol(index, document);
            bool is_cursor_on_def = false;
            bool is_cursor_on_ref = false;
            IEnumerable<Location> locations = null;
            foreach (var d in defs)
            {
                if (sym.range.Start.Value == d.Range.Start.Value
                    && sym.range.End.Value == d.Range.End.Value)
                {
                    is_cursor_on_def = true;
                    // This means that user wants to unfold all occurrences on RHS,
                    // not a specific instance.
                    // This means that user wants to unfold a specific
                    // instance of a RHS symbol.
                    locations = refs_and_defs.Where(t =>
                    {
                        foreach (var x in defs)
                        {
                            if (x.Range.Start.Value == t.Range.Start.Value
                                && x.Range.End.Value == t.Range.End.Value)
                                return false;
                        }
                        return true;
                    });
                    break;
                }
            }
            foreach (var d in refs_and_defs)
            {
                if (sym.range.Start.Value == d.Range.Start.Value
                    && sym.range.End.Value == d.Range.End.Value
                    && !is_cursor_on_def)
                {
                    is_cursor_on_ref = true;
                    // This means that user wants to unfold a specific occurrence on RHS.
                    locations = new List<Location>() { d };
                    break;
                }
            }

            IParseTree rule = null;
            IParseTree it = pd_parser.AllNodes.Where(n =>
            {
                if (!(n is ANTLRv4Parser.ParserRuleSpecContext))
                    return false;
                var r = n as ANTLRv4Parser.ParserRuleSpecContext;
                var lhs = r.RULE_REF();
                return lhs.GetText() == sym.name;
            }).FirstOrDefault();
            rule = it ?? throw new Exception("Cannot find a rule corresponding to symbol "
                    + sym.name);

            if (!(is_cursor_on_def || is_cursor_on_ref))
            {
                throw new LanguageServerException("Please position the cursor on either a LHS symbol (which means "
                    + " to replace all RHS occurrences of the symbol), or on a RHS symbol (which means"
                    + " to replace the specific RHS occurrence of the symbol, then try again.");
            }

            // Make sure it's a parser rule.
            if (!(rule is ANTLRv4Parser.ParserRuleSpecContext))
            {
                throw new LanguageServerException("Please position the cursor on either a LHS symbol (which means "
                    + " to replace all RHS occurrences of the symbol), or on a RHS symbol (which means"
                    + " to replace the specific RHS occurrence of the symbol, then try again.");
            }

            if (locations.Count() == 0)
            {
                // You can't replace a symbol if there's no use.
                // Note that there's always one use as long as there's a
                // definition.
                throw new LanguageServerException("There is no use of the symbol "
                    + sym.name + ". Position the cursor to another symbol and try again.");
            }

            // Get all intertoken text immediately for source reconstruction.
            var (text_before, other) = TreeEdits.TextToLeftOfLeaves(pd_parser.TokStream, pd_parser.ParseTree);

            // Substitute RHS into all applied occurences.
            foreach (var location in locations)
            {
                // For symbol rule.RULE_REF(), replace occurrence in parse tree
                // with modified RHS list.
                var parser_rule = rule as ANTLRv4Parser.ParserRuleSpecContext;
                var td = location.Uri;
                AntlrGrammarDetails pd = ParserDetailsFactory.Create(td) as AntlrGrammarDetails;
                var range = location.Range;
                var rhs = parser_rule.ruleBlock();
                var pt = pd.ParseTree;
                var sym_pt = LanguageServer.Util.Find(index, td);
                if (sym_pt == null) throw new Exception("Inexplicably can't find document symbol in DoFold.");

                var s = Module.GetDocumentSymbol(range.Start.Value, td);
                if (s == null) throw new Exception("Inexplicably can't find document symbol in DoFold.");

                TreeEdits.Replace(pt, (t) =>
                {
                    if (!(t is ANTLRv4Parser.ElementContext))
                        return null;
                    var u = t as ANTLRv4Parser.ElementContext;
                    var id = u.atom()?.ruleref()?.RULE_REF();
                    if (id == null) return null;
                    if (id.GetText() != s.name) return null;
                    if (!(id is TerminalNodeImpl)) return null;
                    var tni = id as TerminalNodeImpl;
                    if (tni.Payload.StartIndex == range.Start.Value
                        && tni.Payload.StopIndex == range.End.Value)
                    {
                        var element_p = t;
                        var alternative_p = t.Parent;
                        var element = element_p as ANTLRv4Parser.ElementContext;
                        var alternative = alternative_p as ANTLRv4Parser.AlternativeContext;
                        var ebnf_suffix = element.ebnfSuffix();
                        var element1 = new ANTLRv4Parser.ElementContext(null, 0);
                        bool modified = false;
                        int i = 0;
                        for (; i < alternative.ChildCount; ++i)
                        {
                            if (alternative.children[i] == element)
                            {
                                modified = true;
                                break;
                            }
                        }
                        if (!modified) return null;
                        var new_ebnf = new ANTLRv4Parser.EbnfContext(null, 0);
                        element1.AddChild(new_ebnf);
                        new_ebnf.Parent = element1;
                        var new_block = new ANTLRv4Parser.BlockContext(null, 0);
                        new_ebnf.AddChild(new_block);
                        new_block.Parent = new_ebnf;
                        if (ebnf_suffix != null)
                        {
                            var blocksuffix = new ANTLRv4Parser.BlockSuffixContext(null, 0);
                            TreeEdits.CopyTreeRecursive(ebnf_suffix, new_ebnf, text_before);
                        }
                        var lparen_token = new CommonToken(ANTLRv4Lexer.LPAREN) { Line = -1, Column = -1, Text = "(" };
                        var new_lparen = new TerminalNodeImpl(lparen_token);
                        new_block.AddChild(new_lparen);
                        new_lparen.Parent = new_block;
                        text_before.Add(new_lparen, " ");
                        ANTLRv4Parser.AltListContext altlist1 = new ANTLRv4Parser.AltListContext(null, 0);
                        new_block.AddChild(altlist1);
                        altlist1.Parent = new_block;
                        var rparen_token = new CommonToken(ANTLRv4Lexer.RPAREN) { Line = -1, Column = -1, Text = ")" };
                        var new_rparen = new TerminalNodeImpl(rparen_token);
                        new_block.AddChild(new_rparen);
                        new_rparen.Parent = new_block;

                        // RHS of rule is wrapped in <ruleBlock <ruleAltList ...>>
                        // Peel off each alternative in ruleAltList/labeledAlt, and
                        // insert in newly constructed altlist.

                        var ruleAltList = rhs.ruleAltList();
                        bool first = true;
                        foreach (var labeledAlt in ruleAltList.labeledAlt())
                        {
                            if (!first)
                            {
                                var token4 = new CommonToken(ANTLRv4Lexer.OR) { Line = -1, Column = -1, Text = "|" };
                                var new_or = new TerminalNodeImpl(token4);
                                altlist1.AddChild(new_or);
                                new_or.Parent = altlist1;
                            }
                            first = false;
                            var a = labeledAlt.alternative();
                            var copy = TreeEdits.CopyTreeRecursive(a, altlist1, text_before);
                        }
                        return element1;
                    }
                    return null;
                });
            }

            StringBuilder sb = new StringBuilder();
            TreeEdits.Reconstruct(sb, pd_parser.ParseTree, text_before);
            var new_code = sb.ToString();
            if (new_code != pd_parser.Code)
            {
                result.Add(document.FullPath, new_code);
            }

            return result;
        }

        static int fold_number = 0;

        public static Dictionary<string, string> Fold(int start, int end, Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if initial file is a grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            ExtractGrammarType egt = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(egt, pd_parser.ParseTree);
            bool is_grammar = egt.Type == ExtractGrammarType.GrammarType.Parser
                || egt.Type == ExtractGrammarType.GrammarType.Combined
                || egt.Type == ExtractGrammarType.GrammarType.Lexer;
            if (!is_grammar)
            {
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            }

            // Find all other grammars by walking dependencies (import, vocab, file names).
            HashSet<string> read_files = new HashSet<string>
            {
                document.FullPath
            };
            Dictionary<Workspaces.Document, List<TerminalNodeImpl>> every_damn_literal =
                new Dictionary<Workspaces.Document, List<TerminalNodeImpl>>();
            for (; ; )
            {
                int before_count = read_files.Count;
                foreach (string f in read_files)
                {
                    List<string> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Value.Contains(f)).Select(
                        t => t.Key).ToList();
                    read_files = read_files.Union(additional).ToHashSet();
                }
                foreach (string f in read_files)
                {
                    IEnumerable<List<string>> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Key == f).Select(
                        t => t.Value);
                    foreach (List<string> t in additional)
                    {
                        read_files = read_files.Union(t).ToHashSet();
                    }
                }
                int after_count = read_files.Count;
                if (after_count == before_count)
                {
                    break;
                }
            }

            // Check cursor position. Many things can happen here, but we have to try
            // and make some sense of what the user is pointing out.
            // It is either the LHS symbol of a rule,
            // which means the user wants to fold all occurrences of the rule
            // RHS or it is a selection of symbols in the RHS of the rule, which means the
            // user wants to fold this specific sequence and then create a new rule.
            TerminalNodeImpl sym_start = null;
            TerminalNodeImpl sym_end = null;
            if (start == end)
            {
                // Selection is a single point.
                sym_end = sym_start = LanguageServer.Util.Find(start, document);
            }
            else
            {
                // Selection is of a list of characters. Go up the tree to find an
                // exact match.
                if (start >= end)
                {
                    var temp = end;
                    end = start;
                    start = temp;
                }
                sym_start = LanguageServer.Util.Find(start, document);
                sym_end = LanguageServer.Util.Find(end-1, document);
            }
            if (sym_end == null || sym_start == null)
            {
                throw new LanguageServerException("Please define a span within the RHS of a rule, or just the LHS symbol, then try again.");
            }
            // Go up tree to find a common parent.
            List<IParseTree> lhs_path = new List<IParseTree>();
            List<IParseTree> rhs_path = new List<IParseTree>();
            for (var p = (IParseTree)sym_start; p != null; p = p.Parent) lhs_path.Insert(0, p);
            for (var p = (IParseTree)sym_end; p != null; p = p.Parent) rhs_path.Insert(0, p);
            //List<Type> lhs_types = new List<Type>();
            //List<Type> rhs_types = new List<Type>();
            //for (var p = (IParseTree)sym_start; p != null; p = p.Parent) lhs_types.Insert(0, p.GetType());
            //for (var p = (IParseTree)sym_end; p != null; p = p.Parent) rhs_types.Insert(0, p.GetType());
            //List<string> lhs_string = new List<string>();
            //List<string> rhs_string = new List<string>();
            //for (var p = (IParseTree)sym_start; p != null; p = p.Parent) lhs_string.Insert(0, p.GetText());
            //for (var p = (IParseTree)sym_end; p != null; p = p.Parent) rhs_string.Insert(0, p.GetText());

            int i = 0;
            for (; ; )
            {
                if (lhs_path[i] != rhs_path[i])
                {
                    --i;
                    break;
                }
                ++i;
                if (i >= lhs_path.Count || i > rhs_path.Count)
                {
                    --i;
                    break;
                }
            }
            if (i < 0)
            {
                throw new LanguageServerException("Please define a span within the RHS of a rule, or just the LHS symbol, then try again.");
            }
            if (lhs_path[i] is ANTLRv4Parser.ParserRuleSpecContext)
            {
                throw new LanguageServerException("Please define a span within the RHS of a rule, or just the LHS symbol, then try again.");
            }
            if (lhs_path[i] is ANTLRv4Parser.RuleAltListContext)
            {
                throw new LanguageServerException("Please define a span within the RHS of a rule, or just the LHS symbol, then try again.");
            }
            int j = i;
            for (; j >= 0; --j)
            {
                if (lhs_path[j] is ANTLRv4Parser.ParserRuleSpecContext)
                    break;
            }
            if (j < 0)
            {
                throw new LanguageServerException("Please define a span within the RHS of a rule, or just the LHS symbol, then try again.");
            }
            if (lhs_path[j] is ANTLRv4Parser.ParserRuleSpecContext
                && j + 2 == lhs_path.Count
                && sym_start != sym_end)
            {
                throw new LanguageServerException("Please define a span within the RHS of a rule, or just the LHS symbol, then try again.");
            }

            bool replace_all = sym_start == sym_end && sym_start.Parent is ANTLRv4Parser.ParserRuleSpecContext;

            // Get all intertoken text immediately for source reconstruction.
            var (text_before, other) = TreeEdits.TextToLeftOfLeaves(pd_parser.TokStream, pd_parser.ParseTree);

            if (replace_all)
            {
                // Find complete RHS elsewhere and use LHS symbol if there's a match.
                var rule = lhs_path[j] as ANTLRv4Parser.ParserRuleSpecContext;
                AntlrGrammarDetails pd = pd_parser;
                var pt = pd.ParseTree;
                var replace_name = rule.RULE_REF().GetText();
                var rule_block = rule.ruleBlock();
                var rule_alt_list = rule_block.ruleAltList();
                var str = rule_alt_list.GetText();
                foreach (var replace_this in TreeEdits.FindTopDown(pt,
                    (in IParseTree t, out bool c) =>
                    {
                        if (str == t.GetText())
                        {
                            for (var p = t; p != null; p = p.Parent)
                            {
                                if (p == rule)
                                {
                                    c = false;
                                    return null;
                                }
                            }
                            c = false;
                            return t;
                        }
                        c = true;
                        return null;
                    }))
                {
                    // Replace entire block.
                    // Create a new block with one symbol.
                    var new_block = new ANTLRv4Parser.BlockContext(null, 0);
                    {
                        var lparen_token = new CommonToken(ANTLRv4Lexer.LPAREN) { Line = -1, Column = -1, Text = "(" };
                        var new_lparen = new TerminalNodeImpl(lparen_token);
                        new_block.AddChild(new_lparen);
                        new_lparen.Parent = new_block;
                        text_before.Add(new_lparen, " ");
                        ANTLRv4Parser.AltListContext l_alt = new ANTLRv4Parser.AltListContext(null, 0);
                        new_block.AddChild(l_alt);
                        l_alt.Parent = new_block;
                        var rparen_token = new CommonToken(ANTLRv4Lexer.RPAREN) { Line = -1, Column = -1, Text = ")" };
                        var new_rparen = new TerminalNodeImpl(rparen_token);
                        new_block.AddChild(new_rparen);
                        new_rparen.Parent = new_block;
                        ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                        l_alt.AddChild(new_alt);
                        new_alt.Parent = l_alt;
                        var new_element = new ANTLRv4Parser.ElementContext(null, 0);
                        new_alt.AddChild(new_element);
                        new_element.Parent = new_alt;
                        var new_atom = new ANTLRv4Parser.AtomContext(null, 0);
                        new_element.AddChild(new_atom);
                        new_atom.Parent = new_element;
                        var new_ruleref = new ANTLRv4Parser.RulerefContext(null, 0);
                        new_atom.AddChild(new_ruleref);
                        new_ruleref.Parent = new_atom;
                        var token = new CommonToken(ANTLRv4Lexer.RULE_REF) { Line = -1, Column = -1, Text = replace_name };
                        var new_rule_ref = new TerminalNodeImpl(token);
                        new_ruleref.AddChild(new_rule_ref);
                        new_rule_ref.Parent = new_ruleref;

                        TreeEdits.Replace(pt, (t) =>
                        {
                            if (t != replace_this)
                                return null;
                            return new_block;
                        });
                    }
                }
            }
            else
            {
                var rule = lhs_path[j] as ANTLRv4Parser.ParserRuleSpecContext;
                AntlrGrammarDetails pd = pd_parser;
                var pt = pd.ParseTree;
                var replace_this = lhs_path[i];
                string generated_name = "fold" + fold_number++;
                if (replace_this is ANTLRv4Parser.BlockContext
                    && replace_this.Parent is ANTLRv4Parser.EbnfContext)
                {
                    // Replace entire block.
                    // Create a new block with one symbol.
                    var new_block = new ANTLRv4Parser.BlockContext(null, 0);
                    {
                        var lparen_token = new CommonToken(ANTLRv4Lexer.LPAREN) { Line = -1, Column = -1, Text = "(" };
                        var new_lparen = new TerminalNodeImpl(lparen_token);
                        new_block.AddChild(new_lparen);
                        new_lparen.Parent = new_block;
                        text_before.Add(new_lparen, " ");
                        ANTLRv4Parser.AltListContext l_alt = new ANTLRv4Parser.AltListContext(null, 0);
                        new_block.AddChild(l_alt);
                        l_alt.Parent = new_block;
                        var rparen_token = new CommonToken(ANTLRv4Lexer.RPAREN) { Line = -1, Column = -1, Text = ")" };
                        var new_rparen = new TerminalNodeImpl(rparen_token);
                        new_block.AddChild(new_rparen);
                        new_rparen.Parent = new_block;
                        ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                        l_alt.AddChild(new_alt);
                        new_alt.Parent = l_alt;
                        var new_element = new ANTLRv4Parser.ElementContext(null, 0);
                        new_alt.AddChild(new_element);
                        new_element.Parent = new_alt;
                        var new_atom = new ANTLRv4Parser.AtomContext(null, 0);
                        new_element.AddChild(new_atom);
                        new_atom.Parent = new_element;
                        var new_ruleref = new ANTLRv4Parser.RulerefContext(null, 0);
                        new_atom.AddChild(new_ruleref);
                        new_ruleref.Parent = new_atom;
                        var token = new CommonToken(ANTLRv4Lexer.RULE_REF) { Line = -1, Column = -1, Text = generated_name };
                        var new_rule_ref = new TerminalNodeImpl(token);
                        new_ruleref.AddChild(new_rule_ref);
                        new_rule_ref.Parent = new_ruleref;

                        TreeEdits.Replace(pt, (t) =>
                        {
                            if (t != replace_this)
                                return null;
                            return new_block;
                        });
                    }

                    // Now create a new rule.
                    var rule_spec = new ANTLRv4Parser.RuleSpecContext(null, 0);
                    {
                        ANTLRv4Parser.ParserRuleSpecContext new_ap_rule = new ANTLRv4Parser.ParserRuleSpecContext(null, 0);
                        rule_spec.AddChild(new_ap_rule);
                        new_ap_rule.Parent = rule_spec;
                        ANTLRv4Parser.ParserRuleSpecContext r = rule;
                        var lhs = generated_name;
                        {
                            var token = new CommonToken(ANTLRv4Parser.RULE_REF) { Line = -1, Column = -1, Text = generated_name };
                            var new_rule_ref = new TerminalNodeImpl(token);
                            text_before.Add(new_rule_ref,
                                System.Environment.NewLine + System.Environment.NewLine);
                            new_ap_rule.AddChild(new_rule_ref);
                            new_rule_ref.Parent = new_ap_rule;
                        }
                        // Now have "A'"
                        {
                            var token2 = new CommonToken(ANTLRv4Parser.COLON) { Line = -1, Column = -1, Text = ":" };
                            var new_colon = new TerminalNodeImpl(token2);
                            new_ap_rule.AddChild(new_colon);
                            new_colon.Parent = new_ap_rule;
                        }
                        // Now have "A' :"
                        ANTLRv4Parser.RuleAltListContext rule_alt_list = new ANTLRv4Parser.RuleAltListContext(null, 0);
                        {
                            ANTLRv4Parser.RuleBlockContext new_rule_block_context = new ANTLRv4Parser.RuleBlockContext(null, 0);
                            new_ap_rule.AddChild(new_rule_block_context);
                            new_rule_block_context.Parent = new_ap_rule;
                            new_rule_block_context.AddChild(rule_alt_list);
                            rule_alt_list.Parent = new_rule_block_context;
                        }
                        // Now have "A' : <rb <ral> >"
                        {
                            var token3 = new CommonToken(ANTLRv4Parser.SEMI) { Line = -1, Column = -1, Text = ";" };
                            var new_semi = new TerminalNodeImpl(token3);
                            new_ap_rule.AddChild(new_semi);
                            new_semi.Parent = new_ap_rule;
                        }
                        // Now have "A : <rb <ral> > ;"
                        {
                            TreeEdits.CopyTreeRecursive(r.exceptionGroup(), new_ap_rule, text_before);
                        }
                        // Now have "A' : <rb <ral> > ; <eg>"
                        {
                            ANTLRv4Parser.LabeledAltContext l_alt = new ANTLRv4Parser.LabeledAltContext(rule_alt_list, 0);
                            rule_alt_list.AddChild(l_alt);
                            l_alt.Parent = rule_alt_list;
                            // Create new alt "alpha A'".
                            ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                            l_alt.AddChild(new_alt);
                            new_alt.Parent = l_alt;
                            var new_element = new ANTLRv4Parser.ElementContext(null, 0);
                            new_alt.AddChild(new_element);
                            new_element.Parent = new_alt;
                            var new_ebnf = new ANTLRv4Parser.EbnfContext(null, 0);
                            new_element.AddChild(new_ebnf);
                            new_ebnf.Parent = new_element;
                            TreeEdits.CopyTreeRecursive(replace_this, new_ebnf, text_before);
                        }

                        // Now have "A' : ... ;"
                        TreeEdits.InsertAfter(pt, (t) =>
                        {
                            if (t != rule.Parent)
                                return null;
                            return rule_spec;
                        });
                    }
                }
                else if (replace_this is ANTLRv4Parser.BlockContext
                    && replace_this is ANTLRv4Parser.AlternativeContext)
                {

                }
                else if (replace_this is ANTLRv4Parser.BlockContext
                    && replace_this is ANTLRv4Parser.EbnfContext)
                {

                }
                else if (replace_this is TerminalNodeImpl)
                {
                    // Replace the symbol on the RHS with another symbol.
                    var old_sym = replace_this as TerminalNodeImpl;
                    {
                        var token = new CommonToken(ANTLRv4Parser.RULE_REF) { Line = -1, Column = -1, Text = generated_name };
                        var new_rule_ref = new TerminalNodeImpl(token);
                        text_before.Add(new_rule_ref, text_before[old_sym]);
                        TreeEdits.Replace(pt, (t) =>
                        {
                            if (t != replace_this)
                                return null;
                            return new_rule_ref;
                        });
                    }

                    // Now create a new rule.
                    var rule_spec = new ANTLRv4Parser.RuleSpecContext(null, 0);
                    {
                        ANTLRv4Parser.ParserRuleSpecContext new_ap_rule = new ANTLRv4Parser.ParserRuleSpecContext(null, 0);
                        rule_spec.AddChild(new_ap_rule);
                        new_ap_rule.Parent = rule_spec;
                        ANTLRv4Parser.ParserRuleSpecContext r = rule;
                        var lhs = generated_name;
                        {
                            var token = new CommonToken(ANTLRv4Parser.RULE_REF) { Line = -1, Column = -1, Text = generated_name };
                            var new_rule_ref = new TerminalNodeImpl(token);
                            text_before.Add(new_rule_ref,
                                System.Environment.NewLine + System.Environment.NewLine);
                            new_ap_rule.AddChild(new_rule_ref);
                            new_rule_ref.Parent = new_ap_rule;
                        }
                        // Now have "A'"
                        {
                            var token2 = new CommonToken(ANTLRv4Parser.COLON) { Line = -1, Column = -1, Text = ":" };
                            var new_colon = new TerminalNodeImpl(token2);
                            new_ap_rule.AddChild(new_colon);
                            new_colon.Parent = new_ap_rule;
                        }
                        // Now have "A' :"
                        ANTLRv4Parser.RuleAltListContext rule_alt_list = new ANTLRv4Parser.RuleAltListContext(null, 0);
                        {
                            ANTLRv4Parser.RuleBlockContext new_rule_block_context = new ANTLRv4Parser.RuleBlockContext(null, 0);
                            new_ap_rule.AddChild(new_rule_block_context);
                            new_rule_block_context.Parent = new_ap_rule;
                            new_rule_block_context.AddChild(rule_alt_list);
                            rule_alt_list.Parent = new_rule_block_context;
                        }
                        // Now have "A' : <rb <ral> >"
                        {
                            var token3 = new CommonToken(ANTLRv4Parser.SEMI) { Line = -1, Column = -1, Text = ";" };
                            var new_semi = new TerminalNodeImpl(token3);
                            new_ap_rule.AddChild(new_semi);
                            new_semi.Parent = new_ap_rule;
                        }
                        // Now have "A : <rb <ral> > ;"
                        {
                            TreeEdits.CopyTreeRecursive(r.exceptionGroup(), new_ap_rule, text_before);
                        }
                        // Now have "A' : <rb <ral> > ; <eg>"
                        {
                            ANTLRv4Parser.LabeledAltContext l_alt = new ANTLRv4Parser.LabeledAltContext(rule_alt_list, 0);
                            rule_alt_list.AddChild(l_alt);
                            l_alt.Parent = rule_alt_list;
                            // Create new alt "alpha A'".
                            ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                            l_alt.AddChild(new_alt);
                            new_alt.Parent = l_alt;
                            var new_element = new ANTLRv4Parser.ElementContext(null, 0);
                            new_alt.AddChild(new_element);
                            new_element.Parent = new_alt;
                            var new_ebnf = new ANTLRv4Parser.EbnfContext(null, 0);
                            new_element.AddChild(new_ebnf);
                            new_ebnf.Parent = new_element;
                            TreeEdits.CopyTreeRecursive(replace_this, new_ebnf, text_before);
                        }

                        // Now have "A' : ... ;"
                        TreeEdits.InsertAfter(pt, (t) =>
                        {
                            if (t != rule.Parent)
                                return null;
                            return rule_spec;
                        });
                    }
                }
                else if (replace_this is ANTLRv4Parser.AltListContext)
                {
                    // The only possible parent is a block, so just replace the
                    // altlist with new altlist with just the symbol.
                    {
                        ANTLRv4Parser.AltListContext l_alt = new ANTLRv4Parser.AltListContext(null, 0);
                        ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                        l_alt.AddChild(new_alt);
                        new_alt.Parent = l_alt;
                        var new_element = new ANTLRv4Parser.ElementContext(null, 0);
                        new_alt.AddChild(new_element);
                        new_element.Parent = new_alt;
                        var new_atom = new ANTLRv4Parser.AtomContext(null, 0);
                        new_element.AddChild(new_atom);
                        new_atom.Parent = new_element;
                        var new_ruleref = new ANTLRv4Parser.RulerefContext(null, 0);
                        new_atom.AddChild(new_ruleref);
                        new_ruleref.Parent = new_atom;
                        var token = new CommonToken(ANTLRv4Lexer.RULE_REF) { Line = -1, Column = -1, Text = generated_name };
                        var new_rule_ref = new TerminalNodeImpl(token);
                        new_ruleref.AddChild(new_rule_ref);
                        new_rule_ref.Parent = new_ruleref;
                        TreeEdits.Replace(pt, (t) =>
                        {
                            if (t != replace_this)
                                return null;
                            return l_alt;
                        });
                    }

                    // Now create a new rule.
                    var rule_spec = new ANTLRv4Parser.RuleSpecContext(null, 0);
                    {
                        ANTLRv4Parser.ParserRuleSpecContext new_ap_rule = new ANTLRv4Parser.ParserRuleSpecContext(null, 0);
                        rule_spec.AddChild(new_ap_rule);
                        new_ap_rule.Parent = rule_spec;
                        ANTLRv4Parser.ParserRuleSpecContext r = rule;
                        var lhs = generated_name;
                        {
                            var token = new CommonToken(ANTLRv4Parser.RULE_REF) { Line = -1, Column = -1, Text = generated_name };
                            var new_rule_ref = new TerminalNodeImpl(token);
                            text_before.Add(new_rule_ref,
                                System.Environment.NewLine + System.Environment.NewLine);
                            new_ap_rule.AddChild(new_rule_ref);
                            new_rule_ref.Parent = new_ap_rule;
                        }
                        // Now have "A'"
                        {
                            var token2 = new CommonToken(ANTLRv4Parser.COLON) { Line = -1, Column = -1, Text = ":" };
                            var new_colon = new TerminalNodeImpl(token2);
                            new_ap_rule.AddChild(new_colon);
                            new_colon.Parent = new_ap_rule;
                        }
                        // Now have "A' :"
                        ANTLRv4Parser.RuleAltListContext rule_alt_list = new ANTLRv4Parser.RuleAltListContext(null, 0);
                        {
                            ANTLRv4Parser.RuleBlockContext new_rule_block_context = new ANTLRv4Parser.RuleBlockContext(null, 0);
                            new_ap_rule.AddChild(new_rule_block_context);
                            new_rule_block_context.Parent = new_ap_rule;
                            new_rule_block_context.AddChild(rule_alt_list);
                            rule_alt_list.Parent = new_rule_block_context;
                        }
                        // Now have "A' : <rb <ral> >"
                        {
                            var token3 = new CommonToken(ANTLRv4Parser.SEMI) { Line = -1, Column = -1, Text = ";" };
                            var new_semi = new TerminalNodeImpl(token3);
                            new_ap_rule.AddChild(new_semi);
                            new_semi.Parent = new_ap_rule;
                        }
                        // Now have "A : <rb <ral> > ;"
                        {
                            TreeEdits.CopyTreeRecursive(r.exceptionGroup(), new_ap_rule, text_before);
                        }
                        // Now have "A' : <rb <ral> > ; <eg>"
                        {
                            ANTLRv4Parser.LabeledAltContext l_alt = new ANTLRv4Parser.LabeledAltContext(rule_alt_list, 0);
                            rule_alt_list.AddChild(l_alt);
                            l_alt.Parent = rule_alt_list;
                            // Create new alt "alpha A'".
                            ANTLRv4Parser.AlternativeContext new_alt = new ANTLRv4Parser.AlternativeContext(null, 0);
                            l_alt.AddChild(new_alt);
                            new_alt.Parent = l_alt;
                            var new_element = new ANTLRv4Parser.ElementContext(null, 0);
                            new_alt.AddChild(new_element);
                            new_element.Parent = new_alt;
                            var new_ebnf = new ANTLRv4Parser.EbnfContext(null, 0);
                            new_element.AddChild(new_ebnf);
                            new_ebnf.Parent = new_element;
                            TreeEdits.CopyTreeRecursive(replace_this, new_ebnf, text_before);
                        }

                        // Now have "A' : ... ;"
                        TreeEdits.InsertAfter(pt, (t) =>
                        {
                            if (t != rule.Parent)
                                return null;
                            return rule_spec;
                        });
                    }
                }
                else
#pragma warning disable CS0642 // Possible mistaken empty statement
                    ;
#pragma warning restore CS0642 // Possible mistaken empty statement
            }

            StringBuilder sb = new StringBuilder();
            TreeEdits.Reconstruct(sb, pd_parser.ParseTree, text_before);
            var new_code = sb.ToString();
            if (new_code != pd_parser.Code)
            {
                result.Add(document.FullPath, new_code);
            }

            return result;
        }

        static bool Match(IParseTree a, IParseTree b)
        {
            if ((a is ANTLRv4Parser.RuleAltListContext && b is ANTLRv4Parser.AltListContext)
                || (a is ANTLRv4Parser.ElementContext && b is ANTLRv4Parser.ElementContext)
                || (a is ANTLRv4Parser.AlternativeContext && b is ANTLRv4Parser.AlternativeContext)
                || (a is ANTLRv4Parser.AtomContext && b is ANTLRv4Parser.AtomContext)
                || (a is ANTLRv4Parser.LabeledElementContext && b is ANTLRv4Parser.LabeledElementContext)
                || (a is ANTLRv4Parser.EbnfContext && b is ANTLRv4Parser.EbnfContext)
                || (a is ANTLRv4Parser.EbnfSuffixContext && b is ANTLRv4Parser.EbnfSuffixContext)
                || (a is ANTLRv4Parser.ElementOptionsContext && b is ANTLRv4Parser.ElementOptionsContext)
                )
            {
                if (a.ChildCount != b.ChildCount)
                    return false;
                for (int i = 0; i < a.ChildCount; ++i)
                {
                    if (!Match(a.GetChild(i), b.GetChild(i)))
                        return false;
                }
                return true;
            }
            else if (a is ANTLRv4Parser.LabeledAltContext)
                return Match(a.GetChild(0), b);
            else if (a is TerminalNodeImpl && b is TerminalNodeImpl)
            {
                return a.GetText() == b.GetText();
            }
            return false;
        }

#pragma warning disable IDE0060
        public static Dictionary<string, string> RemoveUselessParentheses(int start, int end, Document document)
#pragma warning restore IDE0060
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if initial file is a grammar.
            if (!(ParserDetailsFactory.Create(document) is AntlrGrammarDetails pd_parser))
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            ExtractGrammarType egt = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(egt, pd_parser.ParseTree);
            bool is_grammar = egt.Type == ExtractGrammarType.GrammarType.Parser
                || egt.Type == ExtractGrammarType.GrammarType.Combined
                || egt.Type == ExtractGrammarType.GrammarType.Lexer;
            if (!is_grammar)
            {
                throw new LanguageServerException("A grammar file is not selected. Please select one first.");
            }

            // Find all other grammars by walking dependencies (import, vocab, file names).
            HashSet<string> read_files = new HashSet<string>
            {
                document.FullPath
            };
            Dictionary<Workspaces.Document, List<TerminalNodeImpl>> every_damn_literal =
                new Dictionary<Workspaces.Document, List<TerminalNodeImpl>>();
            for (; ; )
            {
                int before_count = read_files.Count;
                foreach (string f in read_files)
                {
                    List<string> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Value.Contains(f)).Select(
                        t => t.Key).ToList();
                    read_files = read_files.Union(additional).ToHashSet();
                }
                foreach (string f in read_files)
                {
                    IEnumerable<List<string>> additional = AntlrGrammarDetails._dependent_grammars.Where(
                        t => t.Key == f).Select(
                        t => t.Value);
                    foreach (List<string> t in additional)
                    {
                        read_files = read_files.Union(t).ToHashSet();
                    }
                }
                int after_count = read_files.Count;
                if (after_count == before_count)
                {
                    break;
                }
            }

            //// Check cursor position. Many things can happen here, but we have to try
            //// and make some sense of what the user is pointing out.
            //// It is either the LHS symbol of a rule,
            //// which means the user wants to fold all occurrences of the rule
            //// RHS or it is a selection of symbols in the RHS of the rule, which means the
            //// user wants to fold this specific sequence and then create a new rule.
            //IEnumerable<Location> refs_and_defs = null;
            //IList<Location> defs = null;
            //TerminalNodeImpl sym_start = null;
            //TerminalNodeImpl sym_end = null;
            //if (start == end)
            //{
            //    // Selection is a single point.
            //    sym_end = sym_start = LanguageServer.Util.Find(start, document);
            //}
            //else
            //{
            //    // Selection is of a list of characters. Go up the tree to find an
            //    // exact match.
            //    if (start >= end)
            //    {
            //        var temp = end;
            //        end = start;
            //        start = temp;
            //    }
            //    sym_start = LanguageServer.Util.Find(start, document);
            //    sym_end = LanguageServer.Util.Find(end - 1, document);
            //}
            //if (sym_end == null || sym_start == null)
            //{
            //    throw new LanguageServerException("Please define a span within the RHS of a rule, or just the LHS symbol, then try again.");
            //}
            //// Go up tree to find a common parent.
            //List<IParseTree> lhs_path = new List<IParseTree>();
            //List<IParseTree> rhs_path = new List<IParseTree>();
            //for (var p = (IParseTree)sym_start; p != null; p = p.Parent) lhs_path.Insert(0, p);
            //for (var p = (IParseTree)sym_end; p != null; p = p.Parent) rhs_path.Insert(0, p);
            ////List<Type> lhs_types = new List<Type>();
            ////List<Type> rhs_types = new List<Type>();
            ////for (var p = (IParseTree)sym_start; p != null; p = p.Parent) lhs_types.Insert(0, p.GetType());
            ////for (var p = (IParseTree)sym_end; p != null; p = p.Parent) rhs_types.Insert(0, p.GetType());
            ////List<string> lhs_string = new List<string>();
            ////List<string> rhs_string = new List<string>();
            ////for (var p = (IParseTree)sym_start; p != null; p = p.Parent) lhs_string.Insert(0, p.GetText());
            ////for (var p = (IParseTree)sym_end; p != null; p = p.Parent) rhs_string.Insert(0, p.GetText());

            //int i = 0;
            //for (; ; )
            //{
            //    if (lhs_path[i] != rhs_path[i])
            //    {
            //        --i;
            //        break;
            //    }
            //    ++i;
            //    if (i >= lhs_path.Count || i > rhs_path.Count)
            //    {
            //        --i;
            //        break;
            //    }
            //}
            //if (i < 0)
            //{
            //    throw new LanguageServerException("Please define a span within the RHS of a rule, or just the LHS symbol, then try again.");
            //}
            //if (lhs_path[i] is ANTLRv4Parser.ParserRuleSpecContext)
            //{
            //    throw new LanguageServerException("Please define a span within the RHS of a rule, or just the LHS symbol, then try again.");
            //}
            //if (lhs_path[i] is ANTLRv4Parser.RuleAltListContext)
            //{
            //    throw new LanguageServerException("Please define a span within the RHS of a rule, or just the LHS symbol, then try again.");
            //}
            //int j = i;
            //for (; j >= 0; --j)
            //{
            //    if (lhs_path[j] is ANTLRv4Parser.ParserRuleSpecContext)
            //        break;
            //}
            //if (j < 0)
            //{
            //    throw new LanguageServerException("Please define a span within the RHS of a rule, or just the LHS symbol, then try again.");
            //}
            //if (lhs_path[j] is ANTLRv4Parser.ParserRuleSpecContext
            //    && j + 2 == lhs_path.Count
            //    && sym_start != sym_end)
            //{
            //    throw new LanguageServerException("Please define a span within the RHS of a rule, or just the LHS symbol, then try again.");
            //}

            //bool replace_all = sym_start == sym_end && sym_start.Parent is ANTLRv4Parser.ParserRuleSpecContext;

            bool replace_all = true;

            // Get all intertoken text immediately for source reconstruction.
            var (text_before, other) = TreeEdits.TextToLeftOfLeaves(pd_parser.TokStream, pd_parser.ParseTree);

            if (replace_all)
            {
                AntlrGrammarDetails pd = pd_parser;
                var pt = pd.ParseTree;
                foreach (var replace_this in TreeEdits.FindTopDown(pt,
                    (in IParseTree t, out bool c) =>
                    {
                        c = true;
                        if (t is ANTLRv4Parser.BlockContext)
                        {
                            var block = t as ANTLRv4Parser.BlockContext;
                            var p = block.Parent;
                            if (p is ANTLRv4Parser.EbnfContext)
                            {
                                var ebnf = p as ANTLRv4Parser.EbnfContext;
                                if (ebnf.blockSuffix() != null)
                                {
                                    return null;
                                }
                            }
                            var pp = p.Parent; // element
                            var ppp = pp.Parent; // alternative
                            var pppp = ppp.Parent; // altList or labeledAlt
                            if (pppp is ANTLRv4Parser.AltListContext)
                            {
                                if (pppp.ChildCount != 1)
                                    return null;
                            }
                            if (pppp is ANTLRv4Parser.LabeledAltContext)
                            {
                                if (pppp.ChildCount != 1)
                                    return null;
                            }
                            if (block.COLON() != null)
                            {
                                return null;
                            }
                            var alt_list = block.altList();
                            if (alt_list.ChildCount > 1 && ppp.ChildCount > 1)
                            {
                                return null;
                            }
                            return t;
                        }
                        if (t is ANTLRv4Parser.RuleBlockContext)
                        {

                        }
                        return null;
                    }))
                {
                    // Remove block by hoisting all parts of altList of the block into an element.
                    var block = replace_this as ANTLRv4Parser.BlockContext;
                    var ebnf = block?.Parent as ANTLRv4Parser.EbnfContext;
                    var element = ebnf?.Parent as ANTLRv4Parser.ElementContext;
                    var parent_alternative = element?.Parent as ANTLRv4Parser.AlternativeContext;
                    int i = 0;
                    for (; i < parent_alternative.ChildCount; )
                    {
                        if (parent_alternative.children[i] == element)
                            break;
                        ++i;
                    }
                    parent_alternative.children.RemoveAt(i);
                    var alt_list = block?.altList();
                    var alternatives = alt_list?.alternative();
                    if (alternatives.Length > 1)
                    {
                        IParseTree rule_alt_list_p = block;
                        for (; rule_alt_list_p != null; rule_alt_list_p = rule_alt_list_p.Parent)
                        {
                            if (rule_alt_list_p is ANTLRv4Parser.RuleAltListContext)
                                break;
                        }
                        var rule_alt_list = rule_alt_list_p as ANTLRv4Parser.RuleAltListContext;
                        bool first = true;
                        foreach (var alternative in alternatives)
                        {
                            if (! first)
                            {
                                var token4 = new CommonToken(ANTLRv4Lexer.OR) { Line = -1, Column = -1, Text = "|" };
                                var new_or = new TerminalNodeImpl(token4);
                                rule_alt_list.AddChild(new_or);
                                new_or.Parent = rule_alt_list;
                            }
                            first = false;
                            var labeled_alt = new ANTLRv4Parser.LabeledAltContext(null, 0);
                            TreeEdits.CopyTreeRecursive(alternative, labeled_alt, text_before);
                            rule_alt_list.AddChild(labeled_alt);
                            labeled_alt.Parent = rule_alt_list;
                        }
                    }
                    else
                    {
                        var alternative = alt_list?.GetChild(0) as ANTLRv4Parser.AlternativeContext;
                        foreach (var e in alternative.element())
                        {
                            var copy = TreeEdits.CopyTreeRecursive(e, null, text_before) as ANTLRv4Parser.ElementContext;
                            parent_alternative.children.Insert(i, copy);
                            copy.Parent = parent_alternative;
                            i++;
                        }
                    }
                }
            }
            else
            {
            }

            StringBuilder sb = new StringBuilder();
            TreeEdits.Reconstruct(sb, pd_parser.ParseTree, text_before);
            var new_code = sb.ToString();
            if (new_code != pd_parser.Code)
            {
                result.Add(document.FullPath, new_code);
            }

            return result;
        }


    }
}
