namespace org.antlr.codebuff.validation
{

//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.languages;

	public class DropWSFeaturesFromAll : DropWSFeatures
	{
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws Exception
		public static void Main(string[] args)
		{
			testFeatures(languages, true);
		}
	}

}