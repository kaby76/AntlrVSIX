using System;
using System.Collections.Generic;

namespace org.antlr.codebuff.validation
{

	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	using Utils = Antlr4.Runtime.Misc.Utils;

	public class AllJavaLeaveOneOutValidation
	{
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws Exception
		public static void Main(string[] args)
		{
			LangDescriptor[] languages = new LangDescriptor[] {JAVA_DESCR, JAVA8_DESCR, JAVA_GUAVA_DESCR};
			IList<string> corpusDirs = map(languages, l => l.corpusDir);
			string[] dirs = corpusDirs.ToArray();
			string python = testAllLanguages(languages, dirs, "all_java_leave_one_out.pdf");
			string fileName = "python/src/all_java_leave_one_out.py";
			Utils.writeFile(fileName, python);
			Console.WriteLine("wrote python code to " + fileName);
		}
	}

}