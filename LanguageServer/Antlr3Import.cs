namespace LanguageServer
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using org.eclipse.wst.xml.xpath2.processor.util;

    public class Antlr3Import
    {
        public static void Try(string ffn, string input, ref Dictionary<string, string> results)
        {
            var convert_undefined_to_terminals = true;
            var now = DateTime.Now.ToString();
            var errors = new StringBuilder();
            var str = new AntlrInputStream(input);
            var lexer = new ANTLRv3Lexer(str);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ANTLRv3Parser(tokens);
            var elistener = new ErrorListener<IToken>(parser, lexer, tokens);
            parser.AddErrorListener(elistener);
            var tree = parser.grammarDef();
            var error_file_name = ffn;
            error_file_name = error_file_name.EndsWith(".g3")
                ? (error_file_name.Substring(0, error_file_name.Length - 3) + ".txt") : error_file_name;
            error_file_name = error_file_name.EndsWith(".g")
                ? (error_file_name.Substring(0, error_file_name.Length - 2) + ".txt") : error_file_name;

            var new_ffn = ffn;
            new_ffn = new_ffn.EndsWith(".g3")
                ? (new_ffn.Substring(0, new_ffn.Length - 3) + ".g4") : new_ffn;
            new_ffn = new_ffn.EndsWith(".g")
                ? (new_ffn.Substring(0, new_ffn.Length - 2) + ".g4") : new_ffn;

            if (elistener.had_error)
            {
                results.Add(error_file_name, errors.ToString());
                return;
            }
            else
            {
                errors.AppendLine("File " + ffn + " parsed successfully.");
                errors.AppendLine("Date: " + now);
            }

            // Transforms derived from two sources:
            // https://github.com/senseidb/sensei/pull/23
            var (text_before, other) = TreeEdits.TextToLeftOfLeaves(tokens, tree);


            {
                // Replace name.
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();
                AntlrDOM.AntlrDynamicContext dynamicContext =
                    AntlrDOM.ConvertToDOM.Try(tree, parser);
                var node = engine.parseExpression(
                        @"//grammarDef/id/(TOKEN_REF | RULE_REF)",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrDOM.AntlrElement).AntlrIParseTree).First();
                var term = node as TerminalNodeImpl;
                var old = term.Symbol;
                TreeEdits.Replace(term.Parent, (in IParseTree n, out bool c) =>
                {
                    c = true;
                    if (n == term)
                        return new TerminalNodeImpl(new CommonToken(old.Type)
                            { Line = -1, Column = -1, Text = old.Text + "v4" });
                    return null;
                });
            }

            {
                // Remove unused options.
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();
                AntlrDOM.AntlrDynamicContext dynamicContext =
                    AntlrDOM.ConvertToDOM.Try(tree, parser);
                // Allow language, tokenVocab, TokenLabelType, superClass
                var nodes = engine.parseExpression(
                        @"//optionsSpec
                            /option
                                [id
                                    /(TOKEN_REF | RULE_REF)
                                        [text() = 'output'
                                        or text() = 'backtrack'
                                        or text() = 'memoize'
                                        or text() = 'ASTLabelType'
                                        or text() = 'rewrite']]",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrDOM.AntlrElement).AntlrIParseTree);
                TreeEdits.Delete(tree, (in IParseTree n, out bool c) =>
                {
                    c = true;
                    return nodes.Contains(n) ? n : null;
                });
                var options = engine.parseExpression(
                        @"//optionsSpec",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrDOM.AntlrElement).AntlrIParseTree);
                if (options.Count() == 1 && options.First().ChildCount == 3)
                {
                    TreeEdits.Delete(tree, (in IParseTree n, out bool c) =>
                    {
                        c = true;
                        return options.Contains(n) ? n : null;
                    });
                }
            }


            {
                // Use new tokens{} syntax
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();
                AntlrDOM.AntlrDynamicContext dynamicContext =
                    AntlrDOM.ConvertToDOM.Try(tree, parser);
                var nodes = engine.parseExpression(
                        @"//tokensSpec
                            /tokenSpec
                                /SEMI",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrDOM.AntlrElement).AntlrIParseTree);
                if (nodes.Any())
                {
                    var last = nodes.Last();
                    TreeEdits.Delete(last, (in IParseTree n, out bool c) =>
                    {
                        c = false;
                        return n;
                    });
                    TreeEdits.Replace(tree, (in IParseTree n, out bool c) =>
                    {
                        c = true;
                        if (!nodes.Contains(n) && n != last) return null;
                        var t = n as TerminalNodeImpl;
                        var new_sym = new TerminalNodeImpl(new CommonToken(ANTLRv4Lexer.COMMA)
                            {Line = -1, Column = -1, Text = ","});
                        text_before.TryGetValue(t, out string v);
                        if (v != null)
                            text_before.Add(new_sym, v);
                        return new_sym;
                    });
                }
            }

            // Note-- @rulecatch does not exist in Antlr3!
            // Remove unnecessary rulecatch block (use BailErrorStrategy instead)

            {
                // Remove unsupported rewrite syntax and AST operators
                // Note, for this operation, we are just deleting everything,
                // no conversion to a visitor.
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();
                AntlrDOM.AntlrDynamicContext dynamicContext =
                    AntlrDOM.ConvertToDOM.Try(tree, parser);
                var n1 = engine.parseExpression(
                        @"//atom
                            /(ROOT | BANG)",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrDOM.AntlrElement).AntlrIParseTree);
                if (n1.Any())
                {
                    TreeEdits.Delete(tree, (in IParseTree n, out bool c) =>
                    {
                        c = true;
                        return n1.Contains(n) ? n : null;
                    });
                }
                var n2 = engine.parseExpression(
                        @"//terminal_
                            /(ROOT | BANG)",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrDOM.AntlrElement).AntlrIParseTree);
                if (n2.Any())
                {
                    TreeEdits.Delete(tree, (in IParseTree n, out bool c) =>
                    {
                        c = true;
                        return n2.Contains(n) ? n : null;
                    });
                }
                var n3 = engine.parseExpression(
                        @"//rewrite",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrDOM.AntlrElement).AntlrIParseTree);
                if (n3.Any())
                {
                    TreeEdits.Delete(tree, (in IParseTree n, out bool c) =>
                    {
                        c = true;
                        return n3.Contains(n) ? n : null;
                    });
                }
            }

            {
                // Remove syntactic predicates (unnecessary and unsupported in ANTLR 4)
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();
                AntlrDOM.AntlrDynamicContext dynamicContext =
                    AntlrDOM.ConvertToDOM.Try(tree, parser);
                var n1 = engine.parseExpression(
                        @"//elementNoOptionSpec
                            [(actionBlock and SEMPREDOP)]",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrDOM.AntlrElement).AntlrIParseTree);
                if (n1.Any())
                {
                    TreeEdits.Delete(tree, (in IParseTree n, out bool c) =>
                    {
                        c = true;
                        return n1.Contains(n) ? n : null;
                    });
                }
                var n2 = engine.parseExpression(
                        @"//ebnf
                            [SEMPREDOP]",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrDOM.AntlrElement).AntlrIParseTree);
                if (n2.Any())
                {
                    TreeEdits.Delete(tree, (in IParseTree n, out bool c) =>
                    {
                        c = true;
                        return n2.Contains(n) ? n : null;
                    });
                }
            }


            //// First, collect information about the grammar.
            //ANTLRv3BaseListener listener = new ANTLRv3BaseListener();
            //ParseTreeWalker.Default.Walk(listener, tree);

            //// Get list of tokens. Convert this to a list of capitalized names.
            //Dictionary<string, Tuple<string, string>> terminals = new Dictionary<string, Tuple<string, string>>();
            //Dictionary<string, string> nonterminals = new Dictionary<string, string>();

            //foreach (IParseTree token in listener.terminals)
            //{
            //    Antlr4.Runtime.Tree.IParseTree parent;
            //    for (parent = token; parent != null; parent = parent.Parent)
            //    {
            //        if (parent is BisonParser.Token_declsContext)
            //        {
            //            BisonParser.Token_declsContext token_decls = parent as BisonParser.Token_declsContext;
            //            int count = token_decls.ChildCount;
            //            if (count == 1)
            //            {
            //                string tag = "";
            //                string tok = token.GetText();
            //                string cap_tok = tok.Length == 1 ? char.ToUpper(tok[0]).ToString() :
            //                    (char.ToUpper(tok[0]) + tok.Substring(1));
            //                terminals.Add(tok, new Tuple<string, string>(tag, cap_tok));
            //            }
            //            else if (count == 2)
            //            {
            //                string tag = parent.GetChild(0).GetText();
            //                tag = tag.Replace("<", "").Replace(">", "");
            //                string tok = token.GetText();
            //                string cap_tok = tok.Length == 1 ? char.ToUpper(tok[0]).ToString() :
            //                    (char.ToUpper(tok[0]) + tok.Substring(1));
            //                terminals.Add(tok, new Tuple<string, string>(tag, cap_tok));
            //            }
            //            else if (count == 3)
            //            {
            //                string tag = parent.GetChild(1).GetText();
            //                tag = tag.Replace("<", "").Replace(">", "");
            //                string tok = token.GetText();
            //                string cap_tok = tok.Length == 1 ? char.ToUpper(tok[0]).ToString() :
            //                    (char.ToUpper(tok[0]) + tok.Substring(1));
            //                terminals.Add(tok, new Tuple<string, string>(tag, cap_tok));
            //            }
            //        }
            //    }
            //}

            //if (convert_undefined_to_terminals)
            //{
            //    foreach (Tuple<string, List<List<string>>> r in listener.rules)
            //    {
            //        List<List<string>> rhs = r.Item2;
            //        foreach (List<string> s in rhs)
            //        {
            //            foreach (string c in s)
            //            {
            //                if (listener.rules.Where(rr => rr.Item1 == c).Any())
            //                {
            //                    continue;
            //                }

            //                if (terminals.ContainsKey(c))
            //                {
            //                    continue;
            //                }

            //                if (c[0] == '\'')
            //                {
            //                    continue;
            //                }

            //                if (c == "error")
            //                {
            //                    continue;
            //                }
            //                // RHS symbol is not a non-terminal and not a %token terminal.
            //                // Enter it as a terminal.
            //                string tag = "";
            //                string tok = c;
            //                string cap_tok = tok.Length == 1 ? char.ToUpper(tok[0]).ToString() :
            //                    (char.ToUpper(tok[0]) + tok.Substring(1));
            //                terminals.Add(tok, new Tuple<string, string>(tag, cap_tok));
            //                // Note that in the error list.
            //                errors.AppendLine("Symbol " + c + " not declared, assuming it is a terminal.");
            //            }
            //        }
            //    }
            //}

            //// Convert any nonterminals which are keywords in Antlr...
            //foreach (Tuple<string, List<List<string>>> r in listener.rules)
            //{
            //    if (!terminals.ContainsKey(r.Item1)
            //        && (r.Item1 == "options"
            //            || r.Item1 == "grammar"
            //            || r.Item1 == "tokenVocab"
            //            || r.Item1 == "lexer"
            //            || r.Item1 == "parser"
            //            || r.Item1 == "rule"
            //        ))
            //    {
            //        if (!nonterminals.ContainsKey(r.Item1))
            //            nonterminals.Add(r.Item1, r.Item1 + "_nonterminal");
            //    }
            //    else
            //    {
            //        if (!nonterminals.ContainsKey(r.Item1))
            //            nonterminals.Add(r.Item1, r.Item1);
            //    }
            //}

            //// Get the name of the grammar.
            //string name = System.IO.Path.GetFileNameWithoutExtension(input);
            //sb.AppendLine("// Combined Antlr4 grammar generated by Antlrvsix.");
            //sb.AppendLine("// Input grammar: " + input);
            //sb.AppendLine("// Date: " + now);
            //sb.AppendLine();
            //sb.AppendLine("grammar " + name + ";");

            //// Output the rules.
            //foreach (Tuple<string, List<List<string>>> r in listener.rules)
            //{
            //    string lhs = r.Item1;
            //    lhs = nonterminals[lhs];
            //    sb.Append(lhs);
            //    List<List<string>> rhs = r.Item2;
            //    bool first = true;
            //    foreach (List<string> s in rhs)
            //    {
            //        sb.Append(first ? "  :" : "  |");
            //        first = false;
            //        foreach (string c in s)
            //        {
            //            var re = c;
            //            if (terminals.ContainsKey(re)) re = terminals[re].Item2;
            //            else if (nonterminals.ContainsKey(re)) re = nonterminals[re];
            //            sb.Append(" " + re);
            //        }

            //        sb.AppendLine();
            //    }
            //    sb.AppendLine("  ;");
            //}

            //// Check for use of error symbol. The semantics is for the parser to look for
            //// the ERROR token.
            //bool found = false;
            //foreach (Tuple<string, List<List<string>>> r in listener.rules)
            //{
            //    List<List<string>> rhs = r.Item2;
            //    foreach (List<string> s in rhs)
            //    {
            //        foreach (string c in s)
            //        {
            //            if (c == "error")
            //            {
            //                found = true;
            //                goto bigexit;
            //            }
            //        }
            //    }
            //}
            //bigexit:
            //    results.Add(input.Replace(".g3", ".txt"), errors.ToString());
            //    results.Add(input.Replace(".g3", ".g4"), sb.ToString());
            StringBuilder sb = new StringBuilder();
            TreeEdits.Reconstruct(sb, tree, text_before);
            var new_code = sb.ToString();
            results.Add(new_ffn, new_code);
        }
    }
}
