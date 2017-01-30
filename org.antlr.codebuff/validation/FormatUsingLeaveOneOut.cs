namespace org.antlr.codebuff.validation
{

	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;

	public class FormatUsingLeaveOneOut : LeaveOneOutValidator
	{
		public FormatUsingLeaveOneOut(string rootDir, LangDescriptor language) : base(rootDir, language)
		{
		}

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