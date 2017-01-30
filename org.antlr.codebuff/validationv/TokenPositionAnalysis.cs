namespace org.antlr.codebuff.validation
{

	using Token = Antlr4.Runtime.IToken;

	public class TokenPositionAnalysis
	{
		public Token t; // token from the input stream; it's position will usually differ from charIndexStart etc...
		public int charIndexStart; // where in *output* buffer the associated token starts; used to respond to clicks in formatted text
		public int charIndexStop; // stop index (inclusive)
		public int wsPrediction; // predicted category, '\n' or ' '
		public int alignPrediction; // predicted category, align/indent if ws indicates newline
		public int actualWS; // actual category, '\n' or ' '
		public int actualAlign; // actual category
		public string wsAnalysis = "n/a";
		public string alignAnalysis = "n/a";

		public TokenPositionAnalysis(Token t, int wsPrediction, string wsAnalysis, int alignPrediction, string alignAnalysis)
		{
			this.t = t;
			this.wsPrediction = wsPrediction;
			this.wsAnalysis = wsAnalysis;
			this.alignPrediction = alignPrediction;
			this.alignAnalysis = alignAnalysis;
		}
	}

}