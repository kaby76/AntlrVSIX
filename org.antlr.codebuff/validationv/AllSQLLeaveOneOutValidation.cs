using System;
using System.Collections.Generic;

namespace org.antlr.codebuff.validation
{

	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	using Utils = Antlr4.Runtime.Misc.Utils;

//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.SQLITE_CLEAN_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.SQLITE_NOISY_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.TSQL_CLEAN_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.TSQL_NOISY_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.misc.BuffUtils.map;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.validation.LeaveOneOutValidator.testAllLanguages;

	public class AllSQLLeaveOneOutValidation
	{
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws Exception
		public static void Main(string[] args)
		{
			LangDescriptor[] languages = new LangDescriptor[] {SQLITE_NOISY_DESCR, SQLITE_CLEAN_DESCR, TSQL_NOISY_DESCR, TSQL_CLEAN_DESCR};
			IList<string> corpusDirs = map(languages, l => l.corpusDir);
			string[] dirs = corpusDirs.ToArray();
			string python = testAllLanguages(languages, dirs, "all_sql_leave_one_out.pdf");
			string fileName = "python/src/all_sql_leave_one_out.py";
			Utils.writeFile(fileName, python);
			Console.WriteLine("wrote python code to " + fileName);
		}

	}

}