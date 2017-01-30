using System;
using System.Collections.Generic;
using System.Linq;
using com.google.common.collect;
using org.antlr.codebuff.misc;

namespace org.antlr.codebuff.validation
{

	//using ArrayListMultimap = com.google.common.collect.ArrayListMultimap;
	//using ListMultimap = com.google.common.collect.ListMultimap;
	using BuffUtils = org.antlr.codebuff.misc.BuffUtils;
	//using HashBag = org.antlr.codebuff.misc.HashBag;
	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;


	/// 
	/// <summary>
	/// Walk corpus of single file, grouping by feature vectors (formatting context).
	/// Then consider the diversity index / entropy of the predicted categories
	/// and diversity of contexts themselves.
	/// 
	/// First goal: Measure how well the features capture the structure of the input.
	/// 
	/// Assumption: ignore generality.
	/// 
	/// If each specific context predicts many different categories and mostly at
	/// same likelihood, diversity/entropy is high and the predictor/formatter will likely
	/// perform poorly. In contrast, if each context predicts a single category,
	/// the model potentially captures the input structure well.  Call that
	/// context-category diversity.
	/// 
	/// Low context-category diversity could be a result of model "getting lucky".
	/// Imagine an input file like Java input enum type with almost all one context
	/// and one category. Define context diversity as ratio of number of contexts
	/// over number of tokens.  If context-cat diversity is low AND context diversity
	/// is high, we could have more confidence the model was capturing the input.
	/// 
	/// That only gives a do not exceed speed. Imagine a model whose context is
	/// just the token index. Context-category diversity would be 0 (or close to it)
	/// and diversity would be 1 as there'd be 1 context per token.  The model could
	/// reproduce the input perfectly but would not generalize in any way.
	/// 
	/// Assuming we don't use a stupid model like "token index -> formatting directive",
	/// 
	/// model capture index = (1.0 - context-category diversity) * context diversity
	/// 
	/// gives useful index values for the files in a corpus.
	/// 
	/// If context-category diversity is high, context diversity doesn't matter.
	/// A predictor cannot choose better than the entropy in the choices. This
	/// could be either or both of:
	/// 
	/// a) model does not distinguish between contexts well enough
	/// b) corpus is highly inconsistent in formatting
	/// 
	/// If we assume a single file is internally consistent (not always true;
	/// in principle, we could do divide up files into regions such as methods.) then
	/// high context-category diversity implies a bad model.
	/// 
	/// Now consider context-category diversity across corpus files. High
	/// context-category diversity in one file but low overall context-category
	/// diversity in other files combined, implies that file is inconsistent
	/// with corpus.
	/// 
	/// Even with low context-category diversity in one file and in corpus, they
	/// may differ in predicted category so must check.  If context-category diversity
	/// is low in file and in corpus and they are same, good consistency. If
	/// context-category diversity is low in both but not same, bad consistency.
	/// If context-category diversity is high in file but not corpus, file
	/// is internally inconsistent or with corpus or both. If context-category diversity
	/// is low in file, high in corpus, inconsistent.
	/// 
	/// It just occurred to me that the number of categories predicted for a specific
	/// feature vector is like the diversity index in that we could sort by it.
	/// In this case, it favors diversity with lots of choices rather than relative
	/// likelihood.
	/// 
	/// Important to point out: a perfect diversity of 0 for a file doesn't mean
	/// we can perfectly reproduce. The formatting directives may not be correct
	/// or rich enough. Low diversity just says we'll be able to pick what the
	/// formatting model thinks is correct.
	/// </summary>
	public class Entropy
	{

		public static void Main(string[] args)
		{
			runCaptureForOneLanguage(Tool.ANTLR4_DESCR);
		}

		public static void runCaptureForOneLanguage(LangDescriptor language)
		{
			IList<string> filenames = Tool.getFilenames(language.corpusDir, language.fileRegex);
			IList<InputDocument> documents = Tool.load(filenames, language);
			foreach (string fileName in filenames)
			{
				// Examine info for this file in isolation
				Corpus fileCorpus = new Corpus(fileName, language);
				fileCorpus.train();
				Console.WriteLine(fileName);
	//			examineCorpus(corpus);
				ArrayListMultiMap<FeatureVectorAsObject, int> ws = getWSContextCategoryMap(fileCorpus);
                ArrayListMultiMap<FeatureVectorAsObject, int> hpos = getHPosContextCategoryMap(fileCorpus);

				// Compare with corpus minus this file
				string path = fileName;
				IList<InputDocument> others = BuffUtils.filter(documents, d => !d.fileName.Equals(path));
				Corpus corpus = new Corpus(others, language);
				corpus.train();
                //			examineCorpus(corpus);
                ArrayListMultiMap<FeatureVectorAsObject, int> corpus_ws = getWSContextCategoryMap(corpus);
                ArrayListMultiMap<FeatureVectorAsObject, int> corpus_hpos = getHPosContextCategoryMap(corpus);

				foreach (FeatureVectorAsObject x in ws.Keys)
				{
					HashBag<int> fwsCats = getCategoriesBag(ws[x]);
					IList<float> fwsRatios = getCategoryRatios(fwsCats.Values);
					HashBag<int> wsCats = getCategoriesBag(corpus_ws[x]);
					IList<float> wsRatios = getCategoryRatios(wsCats.Values);
					// compare file predictions with corpus predictions
					if (!fwsRatios.SequenceEqual(wsRatios))
					{
						Console.WriteLine(fwsRatios + " vs " + wsRatios);
					}

					HashBag<int> fhposCats = getCategoriesBag(hpos[x]);
					HashBag<int> hposCats = getCategoriesBag(corpus_hpos[x]);
				}

				break;
			}
		}

		public static ArrayListMultiMap<FeatureVectorAsObject, int> getWSContextCategoryMap(Corpus corpus)
		{
            ArrayListMultiMap<FeatureVectorAsObject, int> wsByFeatureVectorGroup = ArrayListMultiMap<FeatureVectorAsObject, int>.create();
			int numContexts = corpus.featureVectors.Count;
			for (int i = 0; i < numContexts; i++)
			{
				int[] X = corpus.featureVectors[i];
				int y = corpus.injectWhitespace[i];
				wsByFeatureVectorGroup.Add(new FeatureVectorAsObject(X, Trainer.FEATURES_INJECT_WS), y);
			}

			return wsByFeatureVectorGroup;
		}

		public static ArrayListMultiMap<FeatureVectorAsObject, int> getHPosContextCategoryMap(Corpus corpus)
		{
            ArrayListMultiMap<FeatureVectorAsObject, int> hposByFeatureVectorGroup = ArrayListMultiMap<FeatureVectorAsObject, int>.create();
			int numContexts = corpus.featureVectors.Count;
			for (int i = 0; i < numContexts; i++)
			{
				int[] X = corpus.featureVectors[i];
				int y = corpus.hpos[i];
				hposByFeatureVectorGroup.Add(new FeatureVectorAsObject(X, Trainer.FEATURES_HPOS), y);
			}

			return hposByFeatureVectorGroup;
		}

		public static void examineCorpus(Corpus corpus)
		{
            ArrayListMultiMap<FeatureVectorAsObject, int> wsByFeatureVectorGroup = ArrayListMultiMap<FeatureVectorAsObject, int>.create();
            ArrayListMultiMap<FeatureVectorAsObject, int> hposByFeatureVectorGroup = ArrayListMultiMap<FeatureVectorAsObject, int>.create();
			int numContexts = corpus.featureVectors.Count;
			for (int i = 0; i < numContexts; i++)
			{
				int[] X = corpus.featureVectors[i];
				int y1 = corpus.injectWhitespace[i];
				int y2 = corpus.hpos[i];
				wsByFeatureVectorGroup.Add(new FeatureVectorAsObject(X, Trainer.FEATURES_INJECT_WS), y1);
				hposByFeatureVectorGroup.Add(new FeatureVectorAsObject(X, Trainer.FEATURES_HPOS), y2);
			}
			IList<double> wsEntropies = new List<double>();
			IList<double> hposEntropies = new List<double>();
			foreach (FeatureVectorAsObject x in wsByFeatureVectorGroup.Keys)
			{
				var cats = wsByFeatureVectorGroup[x];
				var cats2 = hposByFeatureVectorGroup[x];
				HashBag<int> wsCats = getCategoriesBag(cats);
				HashBag<int> hposCats = getCategoriesBag(cats2);
				double wsEntropy = getNormalizedCategoryEntropy(getCategoryRatios(wsCats.Values));
				double hposEntropy = getNormalizedCategoryEntropy(getCategoryRatios(hposCats.Values));
				wsEntropies.Add(wsEntropy);
				hposEntropies.Add(hposEntropy);
				Console.Write("{0,130} : {1},{2} {3},{4}\n", x, wsCats, wsEntropy, hposCats, hposEntropy);
			}
			Console.WriteLine("MEAN " + BuffUtils.mean(wsEntropies));
			Console.WriteLine("MEAN " + BuffUtils.mean(hposEntropies));
			float contextRichness = wsEntropies.Count / (float) numContexts; // 0..1 where 1 means every token had different context
			Console.WriteLine("Context richness = " + contextRichness + " uniq ctxs=" + wsEntropies.Count + ", nctxs=" + numContexts);
		}

		/// <summary>
		/// Return diversity index, e^entropy.
		///  https://en.wikipedia.org/wiki/Diversity_index
		///  "Shannon entropy is the logarithm of 1D, the true diversity
		///   index with parameter equal to 1."
		/// </summary>
		public static double getCategoryDiversityIndex(ICollection<float> ratios)
		{
			return Math.Exp(getNormalizedCategoryEntropy(ratios));
		}

	//	public static double getCategoryDiversityIndex(double entropy) {
	//		return Math.exp(entropy);
	//	}

		/// <summary>
		/// Return Shannon's diversity index (entropy) normalized to 0..1
		///  https://en.wikipedia.org/wiki/Diversity_index
		///  "Shannon entropy is the logarithm of 1D, the true diversity
		///   index with parameter equal to 1."
		/// 
		///  "When all types in the dataset of interest are equally common,
		///   all pi values equal 1 / R, and the Shannon index hence takes
		///   the value ln(R)."
		/// 
		///   So, normalize to 0..1 by dividing by log(R). R here is the number
		///   of different categories.
		/// </summary>
		public static double getNormalizedCategoryEntropy(ICollection<float> ratios)
		{
			double entropy = 0.0;
			int R = ratios.Count;
			foreach (float r in ratios)
			{
				entropy += r * Math.Log(r);
			}
			entropy = -entropy;
			return R == 1 ? 0.0 : entropy / Math.Log(R);
	//		return R==1 ? 0.0 : entropy;
		}

		public static IList<float> getCategoryRatios(ICollection<int> catCounts)
		{
			IList<float> ratios = new List<float>();
			int n = BuffUtils.sum(catCounts);
			foreach (int count in catCounts)
			{
				float probCat = count / (float) n;
				ratios.Add(probCat);
			}
			return ratios;
		}

		public static HashBag<int> getCategoriesBag(ICollection<int> categories)
		{
			HashBag<int> votes = new HashBag<int>();
			foreach (int category in categories)
			{
				votes.add(category);
			}
			return votes;
		}
	}

}