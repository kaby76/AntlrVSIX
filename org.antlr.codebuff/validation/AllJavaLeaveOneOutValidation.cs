using System;
using System.Collections.Generic;
using System.Linq;
using org.antlr.codebuff.misc;

namespace org.antlr.codebuff.validation
{

	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	using Utils = Antlr4.Runtime.Misc.Utils;

	public class AllJavaLeaveOneOutValidation
	{
		public static void Main(string[] args)
		{
			LangDescriptor[] languages = new LangDescriptor[] {JAVA_DESCR, JAVA8_DESCR, JAVA_GUAVA_DESCR};
			IList<string> corpusDirs = BuffUtils.map(languages, l => l.corpusDir);
			string[] dirs = corpusDirs.ToArray();
			string python = LeaveOneOutValidator.testAllLanguages(languages, dirs, "all_java_leave_one_out.pdf");
			string fileName = "python/src/all_java_leave_one_out.py";
            org.antlr.codebuff.misc.Utils.writeFile(fileName, python);
			Console.WriteLine("wrote python code to " + fileName);
		}
	}

}