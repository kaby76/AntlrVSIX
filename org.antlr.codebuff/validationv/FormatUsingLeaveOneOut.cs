namespace org.antlr.codebuff.validation
{

	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;

//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.ANTLR4_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.JAVA8_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.JAVA_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.JAVA_GUAVA_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.QUORUM_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.SQLITE_CLEAN_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.TSQL_CLEAN_DESCR;

	public class FormatUsingLeaveOneOut : LeaveOneOutValidator
	{
		public FormatUsingLeaveOneOut(string rootDir, LangDescriptor language) : base(rootDir, language)
		{
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws Exception
		public static void Main(string[] args)
		{
			LangDescriptor[] languages = new LangDescriptor[] {QUORUM_DESCR, JAVA_DESCR, JAVA8_DESCR, JAVA_GUAVA_DESCR, ANTLR4_DESCR, SQLITE_CLEAN_DESCR, TSQL_CLEAN_DESCR};

			// walk and generator output but no edit distance
			for (int i = 0; i < languages.Length; i++)
			{
				LangDescriptor language = languages[i];
				LeaveOneOutValidator validator = new LeaveOneOutValidator(language.corpusDir, language);
				validator.validateDocuments(false, "output");
			}
		}
	}

}