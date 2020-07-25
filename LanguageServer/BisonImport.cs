// Template generated code from Antlr4BuildTasks.Template v 3.0

using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LanguageServer
{
    using Antlr4.Runtime;

    public class BisonImport
    {
        internal static void Try(string ffn, string input, ref Dictionary<string, string> results)
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
            string error_file_name = input.Replace(".g4", ".txt");
            if (elistener.had_error)
            {
                results.Add(input.Replace(".y", ".txt"), errors.ToString());
                return;
            }
            else
            {
                errors.AppendLine("File " + input + " parsed successfully.");
                errors.AppendLine("Date: " + now);
            }

            // First, collect information about the grammar.
            BisonGrammarListener listener = new BisonGrammarListener();
            ParseTreeWalker.Default.Walk(listener, tree);

            // Get list of tokens. Convert this to a list of capitalized names.
            Dictionary<string, Tuple<string, string>> terminals = new Dictionary<string, Tuple<string, string>>();
            Dictionary<string, string> nonterminals = new Dictionary<string, string>();
            
            foreach (IParseTree token in listener.terminals)
            {
                Antlr4.Runtime.Tree.IParseTree parent;
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

            if (convert_undefined_to_terminals)
            {
                foreach (Tuple<string, List<List<string>>> r in listener.rules)
                {
                    List<List<string>> rhs = r.Item2;
                    foreach (List<string> s in rhs)
                    {
                        foreach (string c in s)
                        {
                            if (listener.rules.Where(rr => rr.Item1 == c).Any())
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
            foreach (Tuple<string, List<List<string>>> r in listener.rules)
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
            string name = System.IO.Path.GetFileNameWithoutExtension(input);
            sb.AppendLine("// Combined Antlr4 grammar generated by Antlrvsix.");
            sb.AppendLine("// Input grammar: " + input);
            sb.AppendLine("// Date: " + now);
            sb.AppendLine();
            sb.AppendLine("grammar " + name + ";");

            // Output the rules.
            foreach (Tuple<string, List<List<string>>> r in listener.rules)
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
            foreach (Tuple<string, List<List<string>>> r in listener.rules)
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
            results.Add(input.Replace(".y", ".txt"), errors.ToString());
            results.Add(input.Replace(".y", ".g4"), sb.ToString());
        }
    }
}
