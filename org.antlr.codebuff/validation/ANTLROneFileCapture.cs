namespace org.antlr.codebuff.validation
{

	public class ANTLROneFileCapture : OneFileCapture
	{
		public static void Main(string[] args)
		{
			runCaptureForOneLanguage(Tool.ANTLR4_DESCR);
		}
	}

}