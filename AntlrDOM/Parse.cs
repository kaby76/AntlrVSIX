namespace AntlrDOM
{
    using Antlr4.Runtime.Tree;
    using Antlr4.Runtime;

    public class Parse
    {
        public static (IParseTree, Parser, Lexer) Try(string input)
        {
            var str = new AntlrInputStream(input);
            var lexer = new ANTLRv4Lexer(str);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ANTLRv4Parser(tokens);
            var listener = new ErrorListener<IToken>(parser, lexer, tokens);
            parser.AddErrorListener(listener);
            var tree = parser.grammarSpec();
            if (listener.had_error)
            {
                System.Console.WriteLine("error in parse.");
            }
            else
            {
                System.Console.WriteLine("parse completed.");
            }
            System.Console.WriteLine(tokens.OutputTokens());
            System.Console.WriteLine(tree.OutputTree(tokens));
            return (tree, parser, lexer);
        }
    }
}
