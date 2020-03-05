// Template generated code from Antlr4BuildTasks.Template v 2.2
namespace $safeprojectname$
{
    using Antlr4.Runtime;

    public class Program
    {
        static void Try(string input)
        {
            var str = new AntlrInputStream(input);
            var lexer = new arithmeticLexer(str);
            var tokens = new CommonTokenStream(lexer);
            var parser = new arithmeticParser(tokens);
            var listener = new ErrorListener<IToken>(parser, lexer, tokens);
            parser.AddErrorListener(listener);
            var tree = parser.file();
            if (listener.had_error)
            {
                System.Console.WriteLine("error in parse.");
                System.Console.WriteLine(tokens.OutputTokens());
                System.Console.WriteLine(tree.OutputTree(tokens));
            }
            else
            {
                System.Console.WriteLine("parse completed.");
                System.Console.WriteLine(tokens.OutputTokens());
                System.Console.WriteLine(tree.OutputTree(tokens));
                var visitor = new CalculatorVisitor();
                visitor.Visit(tree);
            }
        }

        static void Main(string[] args)
        {
            Try("1 + 2 * 3");
            Try("1 + 2 3 * ");
        }
    }
}
