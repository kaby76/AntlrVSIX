using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using org.antlr.codebuff.misc;
using org.antlr.codebuff.validation;

namespace org.antlr.codebuff
{

	//using HashBag = org.antlr.codebuff.misc.HashBag;
	using MutableDouble = org.antlr.codebuff.misc.MutableDouble;
	//using FeatureVectorAsObject = org.antlr.codebuff.validation.FeatureVectorAsObject;
	//using Pair = Antlr4.Runtime.Misc.Pair;

	/*
	 It would appear the context information provides the strongest evidence for
	 injecting a new line or not.  The column number and even the width of the next
	 statement or expression does not help and in fact probably confuses the
	 issue. Looking at the histograms of both column and earliest ancestor
	 with, I find that they overlap almost always. The sum of the two
	 elements also overlapped heavily. It's possible that using the two
	 values as coordinates would yield separability but it didn't look
	 like it from a scan of the data.
	
	 Using just context information provides amazingly polarized
	 decisions. All but a few were 100% for or against. The closest
	 decision was 11 against and 18 for injecting a newline at context
	 where 'return'his current token:
	
	     ')' '{' 23 'return', statement 49 Identifier
	
	 There were k (29) exact context matches, but 62% of the time a new
	 line was used. It was this case	that I looked at for column or with
	 information as the distinguishing characteristic, but it didn't seem
	 to help.  I bumped that to k=201 for all ST4 source (like 41000 records)
	 and I get 60 against, 141 for a newline (70%).
	
	 We can try more context but weight the significance of misses close to
	 current token more heavily than tokens farther away.
	 */

	/// <summary>
	/// A kNN (k-Nearest Neighbor) classifier </summary>
	public class kNNClassifier
	{
		protected internal readonly Corpus corpus;
		protected internal readonly FeatureMetaData[] FEATURES;
		protected internal IList<int> Y;
		protected internal readonly int maxDistanceCount;

		public bool dumpVotes = false;

		public IDictionary<FeatureVectorAsObject, int?> classifyCache = new Dictionary<FeatureVectorAsObject, int?>();
		public static int nClassifyCalls = 0;
		public static int nClassifyCacheHits = 0;

		public IDictionary<FeatureVectorAsObject, Neighbor[]> neighborCache = new Dictionary<FeatureVectorAsObject, Neighbor[]>();
		public static int nNNCalls = 0;
		public static int nNNCacheHits = 0;

		public kNNClassifier(Corpus corpus, FeatureMetaData[] FEATURES, IList<int> Y)
		{
			this.corpus = corpus;
			this.FEATURES = FEATURES;
			Debug.Assert(FEATURES.Length <= Trainer.NUM_FEATURES);
			int n = 0;
			foreach (FeatureMetaData FEATURE in FEATURES)
			{
				n += (int)FEATURE.mismatchCost;
			}
			maxDistanceCount = n;
			this.Y = Y;
		}

		public virtual void resetCache()
		{
			classifyCache.Clear();
			neighborCache.Clear();
			nClassifyCacheHits = 0;
			nClassifyCalls = 0;
			nClassifyCacheHits = 0;
			nNNCalls = 0;
			nNNCacheHits = 0;
		}

		public virtual int classify(int k, int[] unknown, double distanceThreshold)
		{
			FeatureVectorAsObject key = new FeatureVectorAsObject(unknown, FEATURES);

            int? catI = null;
            classifyCache.TryGetValue(key, out catI);
			nClassifyCalls++;
			if (catI != null)
			{
				nClassifyCacheHits++;
				return catI.Value;
			}
			Neighbor[] kNN = this.kNN(unknown, k, distanceThreshold);
			IDictionary<int, MutableDouble> similarities = getCategoryToSimilarityMap(kNN, k, Y);
			int cat = getCategoryWithMaxValue(similarities);

			if (cat == -1)
			{
				// try with less strict match threshold to get some indication of alignment
				kNN = this.kNN(unknown, k, org.antlr.codebuff.Trainer.MAX_CONTEXT_DIFF_THRESHOLD2);
				similarities = getCategoryToSimilarityMap(kNN, k, Y);
				cat = getCategoryWithMaxValue(similarities);
			}

			classifyCache[key] = cat;
			return cat;
		}

		public static int getCategoryWithMostVotes(HashBag<int> votes)
		{
			int max = int.MinValue;
			int catWithMostVotes = 0;
			foreach (int category in votes.Keys)
			{
                int? i = votes.get(category);
			    int ii = i ?? default(int);
                if (i > max)
				{
					max = ii;
					catWithMostVotes = category;
				}
			}

			return catWithMostVotes;
		}

		public virtual HashBag<int> votes(int k, int[] unknown, IList<int> Y, double distanceThreshold)
		{
			Neighbor[] kNN = this.kNN(unknown, k, distanceThreshold);
			return getVotesBag(kNN, k, unknown, Y);
		}

		public virtual HashBag<int> getVotesBag(Neighbor[] kNN, int k, int[] unknown, IList<int> Y)
		{
			HashBag<int> votes = new HashBag<int>();
			for (int i = 0; i < k && i < kNN.Length; i++)
			{
				votes.add(Y[kNN[i].corpusVectorIndex]);
			}
			if (dumpVotes && kNN.Length > 0)
			{
				Console.Write(Trainer.featureNameHeader(FEATURES));
				InputDocument firstDoc = corpus.documentsPerExemplar[kNN[0].corpusVectorIndex]; // pick any neighbor to get parser
				Console.WriteLine(Trainer._toString(FEATURES, firstDoc, unknown) + "->" + votes);
			    int size = Math.Min(k, kNN.Length);
                kNN = kNN.Take(Math.Min(k, kNN.Length)).ToArray();
				StringBuilder buf = new StringBuilder();
				foreach (Neighbor n in kNN)
				{
					buf.Append(n.ToString(FEATURES, Y));
					buf.Append("\n");
				}
				Console.WriteLine(buf);
			}
			return votes;
		}

		// get category similarity (1.0-distance) so we can weight votes. Just add up similarity.
		// I.e., weight votes with similarity 1 (distances of 0) more than votes with lower similarity
		// If we have 2 votes of distance 0.2 and 1 vote of distance 0, it means
		// we have 2 votes of similarity .8 and 1 of similarity of 1, 1.6 vs 6.
		// The votes still outweigh the similarity in this case. For a tie, however,
		// the weights will matter.
		public virtual IDictionary<int, MutableDouble> getCategoryToSimilarityMap(Neighbor[] kNN, int k, IList<int> Y)
		{
			IDictionary<int, MutableDouble> catSimilarities = new Dictionary<int, MutableDouble>();
			for (int i = 0; i < k && i < kNN.Length; i++)
			{
				int y = Y[kNN[i].corpusVectorIndex];
			    MutableDouble d = null;
                catSimilarities.TryGetValue(y, out d);
				if (d == null)
				{
					d = new MutableDouble(0.0);
					catSimilarities[y] = d;
				}
				double emphasizedDistance = Math.Pow(kNN[i].distance, 1.0 / 3); // cube root
				d.add(1.0 - emphasizedDistance);
			}
			return catSimilarities;
		}

		public virtual int getCategoryWithMaxValue(IDictionary<int, MutableDouble> catSimilarities)
		{
			double max = int.MinValue;
			int catWithMaxSimilarity = -1;
			foreach (int category in catSimilarities.Keys)
			{
			    MutableDouble mutableDouble = null;
                catSimilarities.TryGetValue(category, out mutableDouble);
				if (mutableDouble.d > max)
				{
					max = mutableDouble.d;
					catWithMaxSimilarity = category;
				}
			}

			return catWithMaxSimilarity;
		}

		public virtual string getPredictionAnalysis(InputDocument doc, int k, int[] unknown, IList<int> Y, double distanceThreshold)
		{
			FeatureVectorAsObject key = new FeatureVectorAsObject(unknown, FEATURES);
		    Neighbor[] kNN = null;
            neighborCache.TryGetValue(key, out kNN);
			nNNCalls++;
			if (kNN == null)
			{
				kNN = this.kNN(unknown, k, distanceThreshold);
				neighborCache[key] = kNN;
			}
			else
			{
				nNNCacheHits++;
			}
			IDictionary<int, MutableDouble> similarities = getCategoryToSimilarityMap(kNN, k, Y);
			int cat = getCategoryWithMaxValue(similarities);
			if (cat == -1)
			{
				// try with less strict match threshold to get some indication of alignment
				kNN = this.kNN(unknown, k, org.antlr.codebuff.Trainer.MAX_CONTEXT_DIFF_THRESHOLD2);
				similarities = getCategoryToSimilarityMap(kNN, k, Y);
				cat = getCategoryWithMaxValue(similarities);
			}

			string displayCat;
			int c = cat & 0xFF;
			if (c == org.antlr.codebuff.Trainer.CAT_INJECT_NL || c == org.antlr.codebuff.Trainer.CAT_INJECT_WS)
			{
				displayCat = Formatter.getWSCategoryStr(cat);
			}
			else
			{
				displayCat = Formatter.getHPosCategoryStr(cat);
			}
			displayCat = !string.ReferenceEquals(displayCat, null) ? displayCat : "none";

			StringBuilder buf = new StringBuilder();
			buf.Append(Trainer.featureNameHeader(FEATURES));
			buf.Append(Trainer._toString(FEATURES, doc, unknown) + "->" + similarities + " predicts " + displayCat);
			buf.Append("\n");
			if (kNN.Length > 0)
			{
				kNN = kNN.Take(Math.Min(k, kNN.Length)).ToArray();
				foreach (Neighbor n in kNN)
				{
					buf.Append(n.ToString(FEATURES, Y));
					buf.Append("\n");
				}
			}
			return buf.ToString();
		}

		public virtual Neighbor[] kNN(int[] unknown, int k, double distanceThreshold)
		{
			Neighbor[] distances = this.distances(unknown, k, distanceThreshold);
			Array.Sort(distances, (Neighbor o1, Neighbor o2) => o1.distance.CompareTo(o2.distance));
			return distances.Take(Math.Min(k, distances.Length)).ToArray();
		}

		public virtual Neighbor[] distances(int[] unknown, int k, double distanceThreshold)
		{
			int curTokenRuleIndex = unknown[Trainer.INDEX_PREV_EARLIEST_RIGHT_ANCESTOR];
			int prevTokenRuleIndex = unknown[Trainer.INDEX_EARLIEST_LEFT_ANCESTOR];
			int pr = Trainer.unrulealt(prevTokenRuleIndex)[0];
			int cr = Trainer.unrulealt(curTokenRuleIndex)[0];

			MyHashSet<int> vectorIndexesMatchingContext = null;

			// look for exact match and take result even if < k results.  If we have exact matches they always win let's say
			if (FEATURES == org.antlr.codebuff.Trainer.FEATURES_INJECT_WS)
			{
                vectorIndexesMatchingContext = null;
			    corpus.wsFeaturesToExemplarIndexes.TryGetValue(
			        new FeatureVectorAsObject(unknown, FEATURES),
			        out vectorIndexesMatchingContext);
			}
			else if (FEATURES == org.antlr.codebuff.Trainer.FEATURES_HPOS)
			{
                vectorIndexesMatchingContext = null;
                corpus.hposFeaturesToExemplarIndexes.TryGetValue(
                    new FeatureVectorAsObject(unknown, FEATURES),
                    out vectorIndexesMatchingContext);
			}
			// else might be specialized feature set for testing so ignore these caches in that case

			if (FEATURES == org.antlr.codebuff.Trainer.FEATURES_INJECT_WS && (vectorIndexesMatchingContext == null || vectorIndexesMatchingContext.Count <= 3)) // must have at 4 or more dist=0.0 for WS else we search wider -  can't use this cache if we are testing out different feature sets
			{
                // ok, not exact. look for match with prev and current rule index
                org.antlr.codebuff.misc.Pair<int, int> key = new org.antlr.codebuff.misc.Pair<int, int>(pr, cr);
			    vectorIndexesMatchingContext = null;
                corpus.curAndPrevTokenRuleIndexToExemplarIndexes.TryGetValue(key,
                    out vectorIndexesMatchingContext);
			}
			if (FEATURES == org.antlr.codebuff.Trainer.FEATURES_HPOS && (vectorIndexesMatchingContext == null || vectorIndexesMatchingContext.Count < k))
			{
                // ok, not exact. look for match with prev and current rule index
                org.antlr.codebuff.misc.Pair<int, int> key = new org.antlr.codebuff.misc.Pair<int, int>(pr, cr);
			    vectorIndexesMatchingContext = null;
                corpus.curAndPrevTokenRuleIndexToExemplarIndexes.TryGetValue(key,
                    out vectorIndexesMatchingContext);
			}

			if (distanceThreshold == org.antlr.codebuff.Trainer.MAX_CONTEXT_DIFF_THRESHOLD2)
			{ // couldn't find anything, open it all up.
				vectorIndexesMatchingContext = null;
			}
			IList<Neighbor> distances = new List<Neighbor>();
			if (vectorIndexesMatchingContext == null)
			{
				// no matching contexts for this feature, must rely on full training set
				int n = corpus.featureVectors.Count; // num training samples
				int num0 = 0; // how many 0-distance elements have we seen? If k we can stop!
				for (int i = 0; i < n; i++)
				{
					int[] x = corpus.featureVectors[i];
					double d = distance(x, unknown);
					if (d <= distanceThreshold)
					{
						Neighbor neighbor = new Neighbor(corpus, d, i);
						distances.Add(neighbor);
						if (d == 0.0)
						{
							num0++;
							if (num0 == k)
							{
								break;
							}
						}
					}
				}
			}
			else
			{
				int num0 = 0; // how many 0-distance elements have we seen? If k we can stop!
				foreach (int vectorIndex in vectorIndexesMatchingContext)
				{
					int[] x = corpus.featureVectors[vectorIndex];
					double d = distance(x, unknown);
					if (d <= distanceThreshold)
					{
						Neighbor neighbor = new Neighbor(corpus, d, vectorIndex);
						distances.Add(neighbor);
						if (d == 0.0)
						{
							num0++;
							if (num0 == k)
							{
								break;
							}
						}
					}
				}
			}
			return distances.ToArray();
		}

		/// <summary>
		/// Compute distance as a probability of match, based
		/// solely on context information.
		/// <para>
		/// Ratio of num differences / num total context positions.
		/// </para>
		/// </summary>
		public virtual double distance(int[] A, int[] B)
		{
	//		return ((float)Tool.L0_Distance(categorical, A, B))/num_categorical;
			double d = Tool.weightedL0_Distance(FEATURES, A, B);
			return d / maxDistanceCount;
		}
	}

}