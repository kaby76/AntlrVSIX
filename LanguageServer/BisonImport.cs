// Template generated code from Antlr4BuildTasks.Template v 2.1

using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;

namespace Bison
{
    using Antlr4.Runtime;

    public class BisonImport
    {
        private static void Try(string input)
        {
            AntlrFileStream str = new AntlrFileStream(input);
            BisonLexer lexer = new BisonLexer(str);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            BisonParser parser = new BisonParser(tokens);
            BisonErrorListener<IToken> listener = new BisonErrorListener<IToken>(parser, lexer, tokens);
            parser.AddErrorListener(listener);
            BisonParser.InputContext tree = parser.input();
            if (listener.had_error)
            {
                System.Console.WriteLine("error in parse.");
            }
            else
            {
                System.Console.WriteLine("parse completed.");
                BisonGrammarListener visitor = new BisonGrammarListener();
                ParseTreeWalker.Default.Walk(visitor, tree);
                foreach (Tuple<string, List<List<string>>> r in visitor.rules)
                {
                    string lhs = r.Item1;
                    System.Console.WriteLine(lhs);
                    List<List<string>> rhs = r.Item2;
                    bool first = true;
                    foreach (List<string> s in rhs)
                    {
                        System.Console.Write(first ? "  :" : "  |");
                        first = false;
                        foreach (string c in s)
                        {
                            System.Console.Write(" " + c);
                        }

                        System.Console.WriteLine();
                    }
                    System.Console.WriteLine("  ;");
                }
            }
        }

        private static void Main(string[] args)
        {
            Try(args[0]);
        }
    }
}
