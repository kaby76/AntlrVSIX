using System.Threading;

namespace org.antlr.codebuff
{

	using ANTLRFileStream = Antlr4.Runtime.AntlrFileStream;
	using CommonTokenStream = Antlr4.Runtime.CommonTokenStream;

	public class ProfileJava
	{
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws Exception
		public static void Main(string[] args)
		{
			Thread.Sleep(10000);
			ANTLRFileStream input = new ANTLRFileStream(args[0]);
			JavaLexer lexer = new JavaLexer(input);
			CommonTokenStream tokens = new CommonTokenStream(lexer);
			JavaParser parser = new JavaParser(tokens);
			JavaParser.CompilationUnitContext tree = parser.compilationUnit();
	//		System.out.println(tree.toStringTree(parser));
			Thread.Sleep(10000);
		}
	}

}