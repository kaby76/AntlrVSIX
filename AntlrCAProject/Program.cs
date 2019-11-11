using Antlr4.Runtime;

namespace $safeprojectname$
{
	public class Program
    {
        static void Main(string[] args)
        {
            var input = "1 + 2 * 3";
            var str = new AntlrInputStream(input);
            var lexer = new arithmeticLexer(str);
            var tokens = new CommonTokenStream(lexer);
            var parser = new arithmeticParser(tokens);
            var listener = new ErrorListener<IToken>();
            parser.AddErrorListener(listener);
            var tree = parser.file();
            if (listener.had_error)
            {
                System.Console.WriteLine("error in parse.");
            }
            else
            {
                System.Console.WriteLine("parse completed.");
                System.Console.WriteLine(tokens.OutputTokens());
                System.Console.WriteLine(tree.OutputTree(tokens));
            }
            var visitor = new CalculatorVisitor();
            visitor.Visit(tree);
        }
}
}
