using System;
using System.Collections.Generic;

namespace org.antlr.codebuff.validation
{

	using BuffUtils = org.antlr.codebuff.misc.BuffUtils;
	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	using MultiMap = Antlr4.Runtime.Misc.MultiMap;


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.misc.BuffUtils.mean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.misc.BuffUtils.median;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.misc.BuffUtils.sumDoubles;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.validation.Entropy.getCategoryRatios;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.validation.Entropy.getNormalizedCategoryEntropy;

	public class CorpusConsistency
	{
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws Exception
		public static void Main(string[] args)
		{
			bool report = false;
			if (args.Length > 0 && args[0].Equals("-report"))
			{
				report = true;
			}
			foreach (LangDescriptor language in Tool.languages)
			{
				computeConsistency(language, report);
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void computeConsistency(org.antlr.codebuff.misc.LangDescriptor language, boolean report) throws Exception
		public static void computeConsistency(LangDescriptor language, bool report)
		{
			if (report)
			{
				Console.WriteLine("-----------------------------------");
				Console.WriteLine(language.name);
				Console.WriteLine("-----------------------------------");
			}
			Corpus corpus = new Corpus(language.corpusDir, language);
			corpus.train();
			// a map of feature vector to list of exemplar indexes of that feature
			MultiMap<FeatureVectorAsObject, int?> wsContextToIndex = new MultiMap<FeatureVectorAsObject, int?>();
			MultiMap<FeatureVectorAsObject, int?> hposContextToIndex = new MultiMap<FeatureVectorAsObject, int?>();

			int n = corpus.featureVectors.Count;
			for (int i = 0; i < n; i++)
			{
				int[] features = corpus.featureVectors[i];
				wsContextToIndex.map(new FeatureVectorAsObject(features, Trainer.FEATURES_INJECT_WS), i);
				hposContextToIndex.map(new FeatureVectorAsObject(features, Trainer.FEATURES_HPOS), i);
			}

			int num_ambiguous_ws_vectors = 0;
			int num_ambiguous_hpos_vectors = 0;

			// Dump output grouped by ws vs hpos then feature vector then category
			if (report)
			{
				Console.WriteLine(" --- INJECT WS ---");
			}
			IList<double?> ws_entropies = new List<double?>();
			foreach (FeatureVectorAsObject fo in wsContextToIndex.Keys)
			{
				IList<int?> exemplarIndexes = wsContextToIndex.get(fo);

				// we have group by feature vector, now group by cat with that set for ws
				MultiMap<int?, int?> wsCatToIndexes = new MultiMap<int?, int?>();
				foreach (int? i in exemplarIndexes)
				{
					wsCatToIndexes.map(corpus.injectWhitespace[i], i);
				}
				if (wsCatToIndexes.size() == 1)
				{
					continue;
				}
				if (report)
				{
					Console.WriteLine("Feature vector has " + exemplarIndexes.Count + " exemplars");
				}
				IList<int?> catCounts = BuffUtils.map(wsCatToIndexes.values(), IList.size);
				double wsEntropy = getNormalizedCategoryEntropy(getCategoryRatios(catCounts));
				if (report)
				{
					Console.Write("entropy={0,5:F4}\n", wsEntropy);
				}
				wsEntropy *= exemplarIndexes.Count;
				ws_entropies.Add(wsEntropy);
				num_ambiguous_ws_vectors += exemplarIndexes.Count;
				if (report)
				{
					Console.Write(Trainer.featureNameHeader(Trainer.FEATURES_INJECT_WS));
				}

				if (report)
				{
					foreach (int? cat in wsCatToIndexes.Keys)
					{
						IList<int?> indexes = wsCatToIndexes.get(cat);
						foreach (int? i in indexes)
						{
							string display = getExemplarDisplay(Trainer.FEATURES_INJECT_WS, corpus, corpus.injectWhitespace, i.Value);
							Console.WriteLine(display);
						}
						Console.WriteLine();
					}
				}
			}

			if (report)
			{
				Console.WriteLine(" --- HPOS ---");
			}
			IList<double?> hpos_entropies = new List<double?>();
			foreach (FeatureVectorAsObject fo in hposContextToIndex.Keys)
			{
				IList<int?> exemplarIndexes = hposContextToIndex.get(fo);

				// we have group by feature vector, now group by cat with that set for hpos
				MultiMap<int?, int?> hposCatToIndexes = new MultiMap<int?, int?>();
				foreach (int? i in exemplarIndexes)
				{
					hposCatToIndexes.map(corpus.hpos[i], i);
				}
				if (hposCatToIndexes.size() == 1)
				{
					continue;
				}
				if (report)
				{
					Console.WriteLine("Feature vector has " + exemplarIndexes.Count + " exemplars");
				}
				IList<int?> catCounts = BuffUtils.map(hposCatToIndexes.values(), IList.size);
				double hposEntropy = getNormalizedCategoryEntropy(getCategoryRatios(catCounts));
				if (report)
				{
					Console.Write("entropy={0,5:F4}\n", hposEntropy);
				}
				hposEntropy *= exemplarIndexes.Count;
				hpos_entropies.Add(hposEntropy);
				num_ambiguous_hpos_vectors += exemplarIndexes.Count;
				if (report)
				{
					Console.Write(Trainer.featureNameHeader(Trainer.FEATURES_HPOS));
				}

				if (report)
				{
					foreach (int? cat in hposCatToIndexes.Keys)
					{
						IList<int?> indexes = hposCatToIndexes.get(cat);
						foreach (int? i in indexes)
						{
							string display = getExemplarDisplay(Trainer.FEATURES_HPOS, corpus, corpus.hpos, i.Value);
							Console.WriteLine(display);
						}
						Console.WriteLine();
					}
				}
			}
			Console.WriteLine();
			Console.WriteLine(language.name);
			Console.WriteLine("There are " + wsContextToIndex.size() + " unique ws feature vectors out of " + n + " = " + string.Format("{0,3:F1}%",100.0 * wsContextToIndex.size() / n));
			Console.WriteLine("There are " + hposContextToIndex.size() + " unique hpos feature vectors out of " + n + " = " + string.Format("{0,3:F1}%",100.0 * hposContextToIndex.size() / n));
			float prob_ws_ambiguous = num_ambiguous_ws_vectors / (float) n;
			Console.Write("num_ambiguous_ws_vectors   = {0,5:D}/{1,5:D} = {2,5:F3}\n", num_ambiguous_ws_vectors, n, prob_ws_ambiguous);
			float prob_hpos_ambiguous = num_ambiguous_hpos_vectors / (float) n;
			Console.Write("num_ambiguous_hpos_vectors = {0,5:D}/{1,5:D} = {2,5:F3}\n", num_ambiguous_hpos_vectors, n, prob_hpos_ambiguous);
	//		Collections.sort(ws_entropies);
	//		System.out.println("ws_entropies="+ws_entropies);
			Console.WriteLine("ws median,mean = " + median(ws_entropies) + "," + mean(ws_entropies));
			double expected_ws_entropy = (sumDoubles(ws_entropies) / num_ambiguous_ws_vectors) * prob_ws_ambiguous;
			Console.WriteLine("expected_ws_entropy=" + expected_ws_entropy);

			Console.WriteLine("hpos median,mean = " + median(hpos_entropies) + "," + mean(hpos_entropies));
			double expected_hpos_entropy = (sumDoubles(hpos_entropies) / num_ambiguous_hpos_vectors) * prob_hpos_ambiguous;
			Console.WriteLine("expected_hpos_entropy=" + expected_hpos_entropy);
		}

		public static string getExemplarDisplay(FeatureMetaData[] FEATURES, Corpus corpus, IList<int?> Y, int corpusVectorIndex)
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
			int cat = Y[corpusVectorIndex].Value;
			string displayCat;
			if ((cat & 0xFF) == Trainer.CAT_INJECT_WS || (cat & 0xFF) == Trainer.CAT_INJECT_NL)
			{
				displayCat = Formatter.getWSCategoryStr(cat);
			}
			else
			{
				displayCat = Formatter.getHPosCategoryStr(cat);
			}

			return string.Format("{0} {1,9} {2}", features, displayCat, lineText);
		}
	}

}