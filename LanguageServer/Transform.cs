namespace LanguageServer
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Algorithms;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Document = Workspaces.Document;

    public class Transform
    {

        private class ExtractGrammarType : ANTLRv4ParserBaseListener
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

        private class LiteralsGrammar : ANTLRv4ParserBaseListener
        {
            public List<TerminalNodeImpl> Literals = new List<TerminalNodeImpl>();
            private readonly AntlrGrammarDetails _pd;

            public LiteralsGrammar(AntlrGrammarDetails pd)
            {
                _pd = pd;
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
                if (rule_spec == null)
                {
                    return;
                }

                First = rule_spec[0];
            }
        }

        private class FindFirstMode : ANTLRv4ParserBaseListener
        {
            public IParseTree First = null;
            public IParseTree Last = null;

            public FindFirstMode() { }


            public override void EnterModeSpec([NotNull] ANTLRv4Parser.ModeSpecContext context)
            {
                First = context;
            }
        }

        private class FindOptions : ANTLRv4ParserBaseListener
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

            public override void EnterLexerRuleSpec([NotNull] ANTLRv4Parser.LexerRuleSpecContext context)
            {
                Rules.Add(context);
                ITerminalNode token_ref = context.TOKEN_REF();
                LHS.Add(token_ref);
                current_nonterminal = token_ref;
                RHS[current_nonterminal] = new List<ITerminalNode>();
            }

            public override void EnterRuleref([NotNull] ANTLRv4Parser.RulerefContext context)
            {
                RHS[current_nonterminal].Add(context.GetChild(0) as ITerminalNode);
            }
        }

        private class ExtractModes : ANTLRv4ParserBaseListener
        {
            public List<ANTLRv4Parser.ModeSpecContext> Modes = new List<ANTLRv4Parser.ModeSpecContext>();

            public override void EnterModeSpec([NotNull] ANTLRv4Parser.ModeSpecContext context)
            {
                Modes.Add(context);
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

        private class TableOfRules
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
                List<ITerminalNode> nonterminals = listener.LHS;
                Dictionary<ITerminalNode, List<ITerminalNode>> rhs = listener.RHS;
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

                int insertion = first_rule.SourceInterval.a;
                Antlr4.Runtime.IToken insertion_tok = pd_parser.TokStream.Get(insertion);
                int insertion_ind = insertion_tok.StartIndex;
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
                    IList<Antlr4.Runtime.IToken> inter = pd_parser.TokStream.GetHiddenTokensToRight(end_tok.TokenIndex);
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

                int insertion = first_rule.SourceInterval.a;
                Antlr4.Runtime.IToken insertion_tok = pd_parser.TokStream.Get(insertion);
                int insertion_ind = insertion_tok.StartIndex;
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
                    IList<Antlr4.Runtime.IToken> inter = pd_parser.TokStream.GetHiddenTokensToRight(end_tok.TokenIndex);
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

        private class TableOfModes
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
            private readonly Dictionary<string, SyntaxTree> trees;

            public TableOfModes(AntlrGrammarDetails p, Document d)
            {
                pd_parser = p;
                document = d;
                trees = ReadCsharpSource(document);
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
                        name = listener.Modes[i].id().GetText(),
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

                int insertion = first_rule.SourceInterval.a;
                Antlr4.Runtime.IToken insertion_tok = pd_parser.TokStream.Get(insertion);
                int insertion_ind = insertion_tok.StartIndex;
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
                    IList<Antlr4.Runtime.IToken> inter = pd_parser.TokStream.GetHiddenTokensToRight(end_tok.TokenIndex);
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

        public static void Reconstruct(StringBuilder sb, IParseTree tree, ref int previous, Func<TerminalNodeImpl, string> replace)
        {
            if (tree as TerminalNodeImpl != null)
            {
                TerminalNodeImpl tok = tree as TerminalNodeImpl;
                ICharStream stream = tok.Payload.InputStream;
                var start = tok.Payload.StartIndex;
                var stop = tok.Payload.StopIndex + 1;
                if (previous < start)
                {
                    Interval previous_interval = new Interval(previous, start - 1);
                    string inter = stream.GetText(previous_interval);
                    sb.Append(inter);
                }
                if (tok.Symbol.Type == TokenConstants.EOF)
                    return;
                string new_s = replace(tok);
                sb.Append(new_s);
                previous = stop;
            }
            else
            {
                for (int i = 0; i < tree.ChildCount; ++i)
                {
                    var c = tree.GetChild(i);
                    Reconstruct(sb, c, ref previous, replace);
                }
            }
        }

        public static Dictionary<string, string> ReplaceLiterals(int index, Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if initial file is a grammar.
            AntlrGrammarDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrGrammarDetails;
            ExtractGrammarType egt = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(egt, pd_parser.ParseTree);
            bool is_grammar = egt.Type == ExtractGrammarType.GrammarType.Parser
                || egt.Type == ExtractGrammarType.GrammarType.Combined
                || egt.Type == ExtractGrammarType.GrammarType.Lexer;
            if (!is_grammar)
            {
                return result;
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

            // Find rewrite rules, i.e., string literal to symbol name.
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
                LiteralsGrammar lp_whatever = new LiteralsGrammar(pd_whatever);
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
                StringBuilder sb = new StringBuilder();
                int pre = 0;
                Reconstruct(sb, pd_parser.ParseTree, ref pre,
                    n =>
                    {
                        if (n.Payload.Type != ANTLRv4Lexer.STRING_LITERAL)
                        {
                            return n.GetText();
                        }
                        bool no = false;
                        // Make sure this literal does not appear in lexer rule.
                        for (IRuleNode p = n.Parent; p != null; p = p.Parent)
                        {
                            if (p is ANTLRv4Parser.LexerRuleSpecContext)
                            {
                                no = true;
                                break;
                            }
                        }
                        if (no)
                        {
                            return n.GetText();
                        }
                        var r = n.GetText();
                        subs.TryGetValue(r, out string value);
                        if (value != null)
                        {
                            r = value;
                        }
                        return r;
                    });
                var new_code = sb.ToString();
                if (new_code != pd_parser.Code)
                {
                    result.Add(f, new_code);
                }
            }
            return result;
        }

        public static Dictionary<string, string> RemoveUselessParserProductions(int pos, Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if lexer grammar.
            AntlrGrammarDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrGrammarDetails;
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            bool is_lexer = lp.Type == ExtractGrammarType.GrammarType.Lexer;
            if (is_lexer)
            {
                // We don't consider lexer grammars.
                return result;
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
            result.Add(document.FullPath, new_code);

            return result;
        }

        public static Dictionary<string, string> MoveStartRuleToTop(int pos, Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if lexer grammar.
            AntlrGrammarDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrGrammarDetails;
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            bool is_lexer = lp.Type == ExtractGrammarType.GrammarType.Lexer;
            if (is_lexer)
            {
                // We don't consider lexer grammars.
                return result;
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
            result.Add(document.FullPath, new_code);

            return result;
        }

        public static Dictionary<string, string> ReorderParserRules(int pos, Document document, LspAntlr.ReorderType type)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if lexer grammar.
            AntlrGrammarDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrGrammarDetails;
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            bool is_lexer = lp.Type == ExtractGrammarType.GrammarType.Lexer;
            if (is_lexer)
            {
                return result;
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
            result.Add(document.FullPath, new_code);

            return result;
        }

        public static Dictionary<string, string> SplitCombineGrammars(int pos, Document document, bool split)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if lexer grammar.
            AntlrGrammarDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrGrammarDetails;
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            if (split && lp.Type != ExtractGrammarType.GrammarType.Combined)
            {
                return null;
            }
            if ((!split) && lp.Type != ExtractGrammarType.GrammarType.Parser)
            {
                return null;
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
                ANTLRv4Parser.GrammarSpecContext root = pd_parser.ParseTree as ANTLRv4Parser.GrammarSpecContext;
                if (root == null)
                {
                    return null;
                }

                int grammar_type_index = 0;
                if (root.DOC_COMMENT() != null)
                {
                    grammar_type_index++;
                }

                ANTLRv4Parser.GrammarTypeContext grammar_type_tree = root.grammarType();
                ANTLRv4Parser.IdContext id = root.id();
                ITerminalNode semi_tree = root.SEMI();
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
                    if (oo.id() != null && oo.id().GetText() == "tokenVocab")
                    {
                        tokenVocab = oo;
                    }
                }
                bool remove_options_spec = tokenVocab != null && find_options.Options.Count == 1;
                bool rewrite_options_spec = tokenVocab != null;

                // Create a combined parser grammar.
                StringBuilder sb_parser = new StringBuilder();
                ANTLRv4Parser.GrammarSpecContext root = pd_parser.ParseTree as ANTLRv4Parser.GrammarSpecContext;
                if (root == null)
                {
                    return null;
                }

                int grammar_type_index = 0;
                if (root.DOC_COMMENT() != null)
                {
                    grammar_type_index++;
                }

                ANTLRv4Parser.GrammarTypeContext grammar_type_tree = root.grammarType();
                ANTLRv4Parser.IdContext id = root.id();
                ITerminalNode semi_tree = root.SEMI();
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
                        if (oo.id() != null && oo.id().GetText() == "tokenVocab")
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

        public static Dictionary<string, string> EliminateLeftRecursion(int index, Document document)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Check if initial file is a grammar.
            AntlrGrammarDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrGrammarDetails;
            ExtractGrammarType egt = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(egt, pd_parser.ParseTree);
            bool is_grammar = egt.Type == ExtractGrammarType.GrammarType.Parser
                || egt.Type == ExtractGrammarType.GrammarType.Combined
                || egt.Type == ExtractGrammarType.GrammarType.Lexer;
            if (!is_grammar)
            {
                return result;
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

            // Find rewrite rules, i.e., string literal to symbol name.
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
                LiteralsGrammar lp_whatever = new LiteralsGrammar(pd_whatever);
                ParseTreeWalker.Default.Walk(lp_whatever, pd_whatever.ParseTree);
                List<TerminalNodeImpl> list_literals = lp_whatever.Literals;
                every_damn_literal[whatever_document] = list_literals;

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
            Dictionary<TerminalNodeImpl, string> rewrites = new Dictionary<TerminalNodeImpl, string>();
            foreach (KeyValuePair<Document, List<TerminalNodeImpl>> pair in every_damn_literal)
            {
                Document doc = pair.Key;
                List<TerminalNodeImpl> list_literals = pair.Value;
                foreach (TerminalNodeImpl l in list_literals)
                {
                    bool no = false;
                    // Make sure this literal does not appear in lexer rule.
                    for (IRuleNode p = l.Parent; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.LexerRuleSpecContext)
                        {
                            no = true;
                            break;
                        }
                    }
                    if (no)
                    {
                        continue;
                    }

                    subs.TryGetValue(l.GetText(), out string re);
                    if (re != null)
                    {
                        rewrites.Add(l, re);
                    }
                }
            }

            IEnumerable<string> files = rewrites.Select(r => r.Key.Payload.TokenSource.SourceName).OrderBy(q => q).Distinct();
            List<Document> documents = files.Select(f => { return Workspaces.Workspace.Instance.FindDocument(f); }).ToList();
            foreach (Document f in documents)
            {
                string fn = f.FullPath;
                List<KeyValuePair<TerminalNodeImpl, string>> per_file_changes = rewrites.Where(z => z.Key.Payload.TokenSource.SourceName == f.FullPath)
                    .OrderBy(z => z.Key.Payload.TokenIndex).ToList();
                StringBuilder sb = new StringBuilder();
                int previous = 0;
                string code = f.Code;
                foreach (KeyValuePair<TerminalNodeImpl, string> l in per_file_changes)
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

        public static string EliminateAntlrKeywordsInRules(Document document)
        {
            string result = null;

            // Check if initial file is a parser or combined grammar.
            AntlrGrammarDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrGrammarDetails;
            ExtractGrammarType egt = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(egt, pd_parser.ParseTree);
            bool is_grammar = egt.Type == ExtractGrammarType.GrammarType.Parser
                || egt.Type == ExtractGrammarType.GrammarType.Combined
                ;
            if (!is_grammar)
            {
                return result;
            }

            TableOfRules table = new TableOfRules(pd_parser, document);
            table.ReadRules();
            table.FindPartitions();
            table.FindStartRules();

            StringBuilder sb = new StringBuilder();
            int pre = 0;
            Reconstruct(sb, pd_parser.ParseTree, ref pre,
                n =>
                {
                    var r = n.GetText();
                    if (n.Symbol.Type == ANTLRv4Lexer.RULE_REF)
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
            AntlrGrammarDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrGrammarDetails;
            ExtractGrammarType egt = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(egt, pd_parser.ParseTree);
            bool is_grammar = egt.Type == ExtractGrammarType.GrammarType.Parser
                || egt.Type == ExtractGrammarType.GrammarType.Combined
                ;
            if (!is_grammar)
            {
                return result;
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
                LiteralsGrammar lp_whatever = new LiteralsGrammar(pd_whatever);
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
                Reconstruct(sb, pd_parser.ParseTree, ref pre,
                    n =>
                {
                    if (n.Payload.Type != ANTLRv4Lexer.STRING_LITERAL)
                    {
                        return n.GetText();
                    }
                    bool no = false;
                        // Make sure this literal does not appear in lexer rule.
                        for (IRuleNode p = n.Parent; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.LexerRuleSpecContext)
                        {
                            no = true;
                            break;
                        }
                    }
                    if (no)
                    {
                        return n.GetText();
                    }
                    var r = n.GetText();
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
            AntlrGrammarDetails pd_parser = ParserDetailsFactory.Create(document) as AntlrGrammarDetails;
            ExtractGrammarType lp = new ExtractGrammarType();
            ParseTreeWalker.Default.Walk(lp, pd_parser.ParseTree);
            bool is_lexer = lp.Type == ExtractGrammarType.GrammarType.Lexer;
            if (! is_lexer)
            {
                return result;
            }

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
            result.Add(document.FullPath, new_code);

            return result;
        }
    }
}
