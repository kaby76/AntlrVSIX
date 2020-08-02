namespace LanguageServer
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using org.eclipse.wst.xml.xpath2.processor.util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BisonImport
    {
        public static void Try(string ffn, string input, ref Dictionary<string, string> results)
        {
            bool convert_undefined_to_terminals = true;
            string now = DateTime.Now.ToString();
            StringBuilder errors = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            AntlrInputStream str = new AntlrInputStream(input);
            BisonLexer lexer = new BisonLexer(str);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            BisonParser parser = new BisonParser(tokens);
            BisonErrorListener<IToken> elistener = new BisonErrorListener<IToken>(parser, lexer, tokens, errors);
            parser.AddErrorListener(elistener);
            BisonParser.InputContext tree = parser.input();
            string error_file_name = ffn.Replace(".g4", ".txt");
            if (elistener.had_error)
            {
                results.Add(ffn.Replace(".y", ".txt"), errors.ToString());
                return;
            }
            else
            {
                errors.AppendLine("File " + ffn + " parsed successfully.");
                errors.AppendLine("Date: " + now);
            }

            // Get list of tokens. Convert this to a list of capitalized names.
            Dictionary<string, Tuple<string, string>> terminals = new Dictionary<string, Tuple<string, string>>();
            Dictionary<string, string> nonterminals = new Dictionary<string, string>();
            List<Tuple<string, List<List<string>>>> rules = new List<Tuple<string, List<List<string>>>>();

            // Collect terminals.
            {
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();
                AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    AntlrTreeEditing.AntlrDOM.ConvertToDOM.Try(tree, parser);
                var nodes = engine.parseExpression(
                        @"//token_decls//token_decl/id[position() = 1]",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                Antlr4.Runtime.Tree.IParseTree parent;
                foreach (var token in nodes)
                {
                    for (parent = token; parent != null; parent = parent.Parent)
                    {
                        if (parent is BisonParser.Token_declsContext)
                        {
                            BisonParser.Token_declsContext token_decls = parent as BisonParser.Token_declsContext;
                            int count = token_decls.ChildCount;
                            if (count == 1)
                            {
                                string tag = "";
                                string tok = token.GetText();
                                string cap_tok = tok.Length == 1 ? char.ToUpper(tok[0]).ToString() :
                                    (char.ToUpper(tok[0]) + tok.Substring(1));
                                terminals.Add(tok, new Tuple<string, string>(tag, cap_tok));
                            }
                            else if (count == 2)
                            {
                                string tag = parent.GetChild(0).GetText();
                                tag = tag.Replace("<", "").Replace(">", "");
                                string tok = token.GetText();
                                string cap_tok = tok.Length == 1 ? char.ToUpper(tok[0]).ToString() :
                                    (char.ToUpper(tok[0]) + tok.Substring(1));
                                terminals.Add(tok, new Tuple<string, string>(tag, cap_tok));
                            }
                            else if (count == 3)
                            {
                                string tag = parent.GetChild(1).GetText();
                                tag = tag.Replace("<", "").Replace(">", "");
                                string tok = token.GetText();
                                string cap_tok = tok.Length == 1 ? char.ToUpper(tok[0]).ToString() :
                                    (char.ToUpper(tok[0]) + tok.Substring(1));
                                terminals.Add(tok, new Tuple<string, string>(tag, cap_tok));
                            }
                        }
                    }
                }
            }

            // Collect rules.
            {
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();
                AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    AntlrTreeEditing.AntlrDOM.ConvertToDOM.Try(tree, parser);
                var nodes = engine.parseExpression(
                        @"//rules",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement));
                foreach (var rule in nodes)
                {
                    var r = rule.AntlrIParseTree;
                    var lhs = r.GetChild(0).GetText();
                    var rhs = new List<List<string>>();
                    var rhses = engine.parseExpression(
                            @".//rhses_1/rhs",
                            new StaticContextBuilder()).evaluate(
                            dynamicContext, new object[] { rule })
                        .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement));
                    foreach (var r1 in rhses)
                    {
                        rhs.Add(new List<string>());
                        var sym = engine.parseExpression(
                                @".//symbol",
                                new StaticContextBuilder()).evaluate(
                                dynamicContext, new object[] { r1 })
                            .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                        foreach (var s in sym)
                        {
                            List<string> l = rhs.Last();
                            l.Insert(0, s.GetText());
                        }
                    }
                    rules.Add(new Tuple<string, List<List<string>>>(lhs, rhs));
                }
            }


            // First, collect information about the grammar.
  //          BisonGrammarListener listener = new BisonGrammarListener();
  //          ParseTreeWalker.Default.Walk(listener, tree);


            if (convert_undefined_to_terminals)
            {
                foreach (Tuple<string, List<List<string>>> r in rules)
                {
                    List<List<string>> rhs = r.Item2;
                    foreach (List<string> s in rhs)
                    {
                        foreach (string c in s)
                        {
                            if (rules.Where(rr => rr.Item1 == c).Any())
                            {
                                continue;
                            }

                            if (terminals.ContainsKey(c))
                            {
                                continue;
                            }

                            if (c[0] == '\'')
                            {
                                continue;
                            }

                            if (c == "error")
                            {
                                continue;
                            }
                            // RHS symbol is not a non-terminal and not a %token terminal.
                            // Enter it as a terminal.
                            string tag = "";
                            string tok = c;
                            string cap_tok = tok.Length == 1 ? char.ToUpper(tok[0]).ToString() :
                                (char.ToUpper(tok[0]) + tok.Substring(1));
                            terminals.Add(tok, new Tuple<string, string>(tag, cap_tok));
                            // Note that in the error list.
                            errors.AppendLine("Symbol " + c + " not declared, assuming it is a terminal.");
                        }
                    }
                }
            }

            // Convert any nonterminals which are keywords in Antlr...
            foreach (Tuple<string, List<List<string>>> r in rules)
            {
                if (!terminals.ContainsKey(r.Item1)
                    && (r.Item1 == "options"
                        || r.Item1 == "grammar"
                        || r.Item1 == "tokenVocab"
                        || r.Item1 == "lexer"
                        || r.Item1 == "parser"
                        || r.Item1 == "rule"
                    ))
                {
                    if (!nonterminals.ContainsKey(r.Item1))
                        nonterminals.Add(r.Item1, r.Item1 + "_nonterminal");
                }
                else
                {
                    if (! nonterminals.ContainsKey(r.Item1))
                        nonterminals.Add(r.Item1, r.Item1);
                }
            }

            // Get the name of the grammar.
            string name = System.IO.Path.GetFileNameWithoutExtension(ffn);
            sb.AppendLine("// Combined Antlr4 grammar generated by Antlrvsix.");
            sb.AppendLine("// Input grammar: " + ffn);
            sb.AppendLine("// Date: " + now);
            sb.AppendLine();
            sb.AppendLine("grammar " + name + ";");

            // Output the rules.
            foreach (Tuple<string, List<List<string>>> r in rules)
            {
                string lhs = r.Item1;
                lhs = nonterminals[lhs];
                sb.Append(lhs);
                List<List<string>> rhs = r.Item2;
                bool first = true;
                foreach (List<string> s in rhs)
                {
                    sb.Append(first ? "  :" : "  |");
                    first = false;
                    foreach (string c in s)
                    {
                        var re = c;
                        if (terminals.ContainsKey(re)) re = terminals[re].Item2;
                        else if (nonterminals.ContainsKey(re)) re = nonterminals[re];
                        sb.Append(" " + re);
                    }

                    sb.AppendLine();
                }
                sb.AppendLine("  ;");
            }

            // Check for use of error symbol. The semantics is for the parser to look for
            // the ERROR token.
            bool found = false;
            foreach (Tuple<string, List<List<string>>> r in rules)
            {
                List<List<string>> rhs = r.Item2;
                foreach (List<string> s in rhs)
                {
                    foreach (string c in s)
                    {
                        if (c == "error")
                        {
                            found = true;
                            goto bigexit;
                        }
                    }
                }
            }
        bigexit:
            if (found)
            {
                sb.AppendLine("error : ERROR ;");
            }
            results.Add(ffn.Replace(".y", ".g4"), sb.ToString());
            results.Add(ffn.Replace(".y", ".txt"), errors.ToString());
        }
    }
}
