using System;
using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using org.antlr.codebuff.misc;

namespace org.antlr.codebuff.validation
{

	using BuffUtils = org.antlr.codebuff.misc.BuffUtils;
	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;

	public class CorpusConsistency
	{
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
            MyMultiMap<FeatureVectorAsObject, int> wsContextToIndex = new MyMultiMap<FeatureVectorAsObject, int>();
            MyMultiMap<FeatureVectorAsObject, int> hposContextToIndex = new MyMultiMap<FeatureVectorAsObject, int>();

			int n = corpus.featureVectors.Count;
			for (int i = 0; i < n; i++)
			{
				int[] features = corpus.featureVectors[i];
				wsContextToIndex.Map(new FeatureVectorAsObject(features, Trainer.FEATURES_INJECT_WS), i);
				hposContextToIndex.Map(new FeatureVectorAsObject(features, Trainer.FEATURES_HPOS), i);
			}

			int num_ambiguous_ws_vectors = 0;
			int num_ambiguous_hpos_vectors = 0;

			// Dump output grouped by ws vs hpos then feature vector then category
			if (report)
			{
				Console.WriteLine(" --- INJECT WS ---");
			}
			IList<double> ws_entropies = new List<double>();
			foreach (FeatureVectorAsObject fo in wsContextToIndex.Keys)
			{
				var exemplarIndexes = wsContextToIndex[fo];

                // we have group by feature vector, now group by cat with that set for ws
                MyMultiMap<int, int> wsCatToIndexes = new MyMultiMap<int, int>();
				foreach (int i in exemplarIndexes)
				{
					wsCatToIndexes.Map(corpus.injectWhitespace[i], i);
				}
				if (wsCatToIndexes.Count == 1)
				{
					continue;
				}
				if (report)
				{
					Console.WriteLine("Feature vector has " + exemplarIndexes.size() + " exemplars");
				}
				IList<int> catCounts = BuffUtils.map(wsCatToIndexes.Values, (x)=> x.size());
				double wsEntropy = Entropy.getNormalizedCategoryEntropy(Entropy.getCategoryRatios(catCounts));
				if (report)
				{
					Console.Write("entropy={0,5:F4}\n", wsEntropy);
				}
				wsEntropy *= exemplarIndexes.size();
				ws_entropies.Add(wsEntropy);
				num_ambiguous_ws_vectors += exemplarIndexes.size();
				if (report)
				{
					Console.Write(Trainer.featureNameHeader(Trainer.FEATURES_INJECT_WS));
				}

				if (report)
				{
					foreach (int cat in wsCatToIndexes.Keys)
					{
						var indexes = wsCatToIndexes[cat];
						foreach (int i in indexes)
						{
							string display = getExemplarDisplay(Trainer.FEATURES_INJECT_WS, corpus, corpus.injectWhitespace, i);
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
			IList<double> hpos_entropies = new List<double>();
			foreach (FeatureVectorAsObject fo in hposContextToIndex.Keys)
			{
				MyHashSet<int> exemplarIndexes = hposContextToIndex[fo];

                // we have group by feature vector, now group by cat with that set for hpos
                MyMultiMap<int, int> hposCatToIndexes = new MyMultiMap<int, int>();
				foreach (int i in exemplarIndexes)
				{
					hposCatToIndexes.Map(corpus.hpos[i], i);
				}
				if (hposCatToIndexes.Count == 1)
				{
					continue;
				}
				if (report)
				{
					Console.WriteLine("Feature vector has " + exemplarIndexes.size() + " exemplars");
				}
				IList<int> catCounts = BuffUtils.map(hposCatToIndexes.Values, (x) => x.size());
				double hposEntropy = Entropy.getNormalizedCategoryEntropy(Entropy.getCategoryRatios(catCounts));
				if (report)
				{
					Console.Write("entropy={0,5:F4}\n", hposEntropy);
				}
				hposEntropy *= exemplarIndexes.size();
				hpos_entropies.Add(hposEntropy);
				num_ambiguous_hpos_vectors += exemplarIndexes.size();
				if (report)
				{
					Console.Write(Trainer.featureNameHeader(Trainer.FEATURES_HPOS));
				}

				if (report)
				{
					foreach (int cat in hposCatToIndexes.Keys)
					{
						var indexes = hposCatToIndexes[cat];
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
			Console.WriteLine("There are " + wsContextToIndex.Count + " unique ws feature vectors out of " + n + " = " + string.Format("{0,3:F1}%",100.0 * wsContextToIndex.Count / n));
			Console.WriteLine("There are " + hposContextToIndex.Count + " unique hpos feature vectors out of " + n + " = " + string.Format("{0,3:F1}%",100.0 * hposContextToIndex.Count / n));
			float prob_ws_ambiguous = num_ambiguous_ws_vectors / (float) n;
			Console.Write("num_ambiguous_ws_vectors   = {0,5:D}/{1,5:D} = {2,5:F3}\n", num_ambiguous_ws_vectors, n, prob_ws_ambiguous);
			float prob_hpos_ambiguous = num_ambiguous_hpos_vectors / (float) n;
			Console.Write("num_ambiguous_hpos_vectors = {0,5:D}/{1,5:D} = {2,5:F3}\n", num_ambiguous_hpos_vectors, n, prob_hpos_ambiguous);
	//		Collections.sort(ws_entropies);
	//		System.out.println("ws_entropies="+ws_entropies);
			Console.WriteLine("ws median,mean = " + BuffUtils.median(ws_entropies) + "," + BuffUtils.mean(ws_entropies));
			double expected_ws_entropy = (BuffUtils.sumDoubles(ws_entropies) / num_ambiguous_ws_vectors) * prob_ws_ambiguous;
			Console.WriteLine("expected_ws_entropy=" + expected_ws_entropy);

			Console.WriteLine("hpos median,mean = " + BuffUtils.median(hpos_entropies) + "," + BuffUtils.mean(hpos_entropies));
			double expected_hpos_entropy = (BuffUtils.sumDoubles(hpos_entropies) / num_ambiguous_hpos_vectors) * prob_hpos_ambiguous;
			Console.WriteLine("expected_hpos_entropy=" + expected_hpos_entropy);
		}

		public static string getExemplarDisplay(FeatureMetaData[] FEATURES, Corpus corpus, IList<int> Y, int corpusVectorIndex)
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