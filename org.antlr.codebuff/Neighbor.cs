using System.Collections.Generic;

namespace org.antlr.codebuff
{

//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Formatter.getHPosCategoryStr;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Formatter.getWSCategoryStr;

	public class Neighbor
	{
		public Corpus corpus;
		public readonly double distance;
		public readonly int corpusVectorIndex; // refers to both X (independent) and Y (dependent/predictor) variables

		public Neighbor(Corpus corpus, double distance, int corpusVectorIndex)
		{
			this.corpus = corpus;
			this.distance = distance;
			this.corpusVectorIndex = corpusVectorIndex;
		}

		public virtual string ToString(FeatureMetaData[] FEATURES, IList<int> Y)
		{
			int[] X = corpus.featureVectors[corpusVectorIndex];
			InputDocument doc = corpus.documentsPerExemplar[corpusVectorIndex];
			string features = Trainer._toString(FEATURES, doc, X);
			int line = X[Trainer.INDEX_INFO_LINE];
			string lineText = doc.getLine(line);
			int col = X[Trainer.INDEX_INFO_CHARPOS];
			// insert a dot right before char position
			if (!string.ReferenceEquals(lineText, null))
			{
				lineText = lineText.Substring(0, col) + '\u00B7' + lineText.Substring(col, lineText.Length - col);
			}
			int cat = Y[corpusVectorIndex];
			int[] elements = Trainer.triple(cat);
	//		String display = String.format("%d|%d|%d", cat&0xFF, elements[0], elements[1]);
			string wsDisplay = Formatter.getWSCategoryStr(cat);
			string alignDisplay = Formatter.getHPosCategoryStr(cat);
			string display = !string.ReferenceEquals(wsDisplay, null) ? wsDisplay : alignDisplay;
			if (string.ReferenceEquals(display, null))
			{
				display = string.Format("{0,8}","none");
			}
			return string.Format("{0} ({1},d={2,1:F3}): {3}", features, display, distance, lineText);
		}
	}

}