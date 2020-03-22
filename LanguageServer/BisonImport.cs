// Template generated code from Antlr4BuildTasks.Template v 2.1

using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageServer
{
    using Antlr4.Runtime;

    public class BisonImport
    {
        private static string Try(string input)
        {
            StringBuilder sb = new StringBuilder();
            AntlrFileStream str = new AntlrFileStream(input);
            BisonLexer lexer = new BisonLexer(str);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            BisonParser parser = new BisonParser(tokens);
            BisonErrorListener<IToken> listener = new BisonErrorListener<IToken>(parser, lexer, tokens);
            parser.AddErrorListener(listener);
            BisonParser.InputContext tree = parser.input();
            if (listener.had_error)
            {
                return null;
            }
            else
            {
                BisonGrammarListener visitor = new BisonGrammarListener();
                ParseTreeWalker.Default.Walk(visitor, tree);
                var name = System.IO.Path.GetFileNameWithoutExtension(input);
                sb.AppendLine("grammar " + name + ";");
                foreach (Tuple<string, List<List<string>>> r in visitor.rules)
                {
                    string lhs = r.Item1;
                    sb.Append(lhs);
                    List<List<string>> rhs = r.Item2;
                    bool first = true;
                    foreach (List<string> s in rhs)
                    {
                        sb.Append(first ? "  :" : "  |");
                        first = false;
                        foreach (string c in s)
                        {
                            sb.Append(" " + c);
                        }

                        sb.AppendLine();
                    }
                    sb.AppendLine("  ;");
                }
            }
            return sb.ToString();
        }

        public static Dictionary<string, string> ImportGrammars(List<string> args)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var f in args)
            {
                var new_code = Try(f);
                var antlr = f.Replace(".y", ".g4");
                if (new_code != null)
                    result.Add(antlr, new_code);
            }
            return result;
        }
    }
}
