using System;
using System.Collections.Generic;
using org.antlr.codebuff.misc;

namespace org.antlr.codebuff.validation
{

	//using Triple = Antlr4.Runtime.Misc.Triple;

	public class FormatANTLR4
	{
		public static void Main(string[] args)
		{
			LeaveOneOutValidator validator = new LeaveOneOutValidator(Tool.ANTLR4_DESCR.corpusDir, Tool.ANTLR4_DESCR);
			Triple<IList<Formatter>, IList<float>, IList<float>> results = validator.validateDocuments(false, "output");
			Console.WriteLine(results.b);
			Console.WriteLine(results.c);
		}
	}

}