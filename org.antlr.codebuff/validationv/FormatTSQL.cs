using System;
using System.Collections.Generic;

namespace org.antlr.codebuff.validation
{

    using Triple = Antlr4.Runtime.Misc.Triple;

    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.Tool.TSQL_CLEAN_DESCR;

    public class FormatTSQL
	{
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws Exception
		public static void Main(string[] args)
		{
			LeaveOneOutValidator validator = new LeaveOneOutValidator(TSQL_CLEAN_DESCR.corpusDir, TSQL_CLEAN_DESCR);
			Triple<IList<Formatter>, IList<float?>, IList<float?>> results = validator.validateDocuments(false, "output");
			Console.WriteLine(results.b);
			Console.WriteLine(results.c);
		}
	}

}