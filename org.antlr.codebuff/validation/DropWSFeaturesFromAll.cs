namespace org.antlr.codebuff.validation
{

	public class DropWSFeaturesFromAll : DropWSFeatures
	{
		public static void Main(string[] args)
		{
			testFeatures(Tool.languages, true);
		}
	}

}