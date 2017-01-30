namespace org.antlr.codebuff.misc
{

	/// <summary>
	/// Track stats about a single parent:alt,child:alt list or split-list </summary>
	public class SiblingListStats
	{
		public readonly int numSamples, min, median, max;
		public readonly double variance;

		public SiblingListStats(int numSamples, int min, int median, double variance, int max)
		{
			this.numSamples = numSamples;
			this.max = max;
			this.median = median;
			this.min = min;
			this.variance = variance;
		}

		public override string ToString()
		{
			return "(" + numSamples + "," + min + "," + median + "," + variance + "," + max + ")";
		}
	}

}