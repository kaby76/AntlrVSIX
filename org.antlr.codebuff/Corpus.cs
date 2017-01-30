using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using org.antlr.codebuff.misc;

namespace org.antlr.codebuff
{

	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	using ParentSiblingListKey = org.antlr.codebuff.misc.ParentSiblingListKey;
	using RuleAltKey = org.antlr.codebuff.misc.RuleAltKey;
	using SiblingListStats = org.antlr.codebuff.misc.SiblingListStats;
	using FeatureVectorAsObject = org.antlr.codebuff.validation.FeatureVectorAsObject;
	using CollectSiblingLists = org.antlr.codebuff.walkers.CollectSiblingLists;
	using CollectTokenPairs = org.antlr.codebuff.walkers.CollectTokenPairs;
    using Token = Antlr4.Runtime.IToken;
	using Vocabulary = Antlr4.Runtime.Vocabulary;
	using ParseTreeWalker = Antlr4.Runtime.Tree.ParseTreeWalker;

	public class Corpus
	{
		public const int FEATURE_VECTOR_RANDOM_SEED = 314159; // need randomness but use same seed to get reproducibility

		public const int NUM_DEPENDENT_VARS = 2;
		public const int INDEX_FEATURE_NEWLINES = 0;
		public const int INDEX_FEATURE_ALIGN_WITH_PREVIOUS = 1;

		internal IList<InputDocument> documents; // A list of all input docs to train on

		public IList<InputDocument> documentsPerExemplar; // an entry for each featureVector
		public IList<int[]> featureVectors;
		public IList<int> injectWhitespace;
		public IList<int> hpos;

		public virtual void addExemplar(InputDocument doc, int[] features, int ws, int hpos)
		{
			documentsPerExemplar.Add(doc);
			featureVectors.Add(features);
			injectWhitespace.Add(ws);
			this.hpos.Add(hpos);
		}

		public string rootDir;
		public LangDescriptor language;

		/// <summary>
		/// an index to narrow down the number of vectors we compute distance() on each classification.
		///  The key is (previous token's rule index, current token's rule index). It yields
		///  a list of vectors with same key. Created by <seealso cref="#buildTokenContextIndex"/>.
		/// </summary>
		public MyMultiMap<org.antlr.codebuff.misc.Pair<int, int>, int> curAndPrevTokenRuleIndexToExemplarIndexes;
		public MyMultiMap<FeatureVectorAsObject, int> wsFeaturesToExemplarIndexes;
		public MyMultiMap<FeatureVectorAsObject, int> hposFeaturesToExemplarIndexes;

		public IDictionary<RuleAltKey, IList<org.antlr.codebuff.misc.Pair<int, int>>> ruleToPairsBag = null;
		public IDictionary<ParentSiblingListKey, SiblingListStats> rootAndChildListStats;
		public IDictionary<ParentSiblingListKey, SiblingListStats> rootAndSplitChildListStats;
		public IDictionary<Token, org.antlr.codebuff.misc.Pair<bool, int>> tokenToListInfo;

		public Corpus(string rootDir, LangDescriptor language)
		{
			this.rootDir = rootDir;
			this.language = language;
			if (documents == null)
			{
				IList<string> allFiles = Tool.getFilenames(rootDir, language.fileRegex);
				documents = Tool.load(allFiles, language);
			}
		}

		public Corpus(IList<InputDocument> documents, LangDescriptor language)
		{
			this.documents = documents;
			this.language = language;
		}

		public virtual void train()
		{
			train(true);
		}

		public virtual void train(bool shuffleFeatureVectors)
		{
			collectTokenPairsAndSplitListInfo();

			trainOnSampleDocs();

			if (shuffleFeatureVectors)
			{
				randomShuffleInPlace();
			}

			buildTokenContextIndex();
		}

		/// <summary>
		/// Walk all documents to compute matching token dependencies (we need this for feature computation)
		///  While we're at it, find sibling lists.
		/// </summary>
		public virtual void collectTokenPairsAndSplitListInfo()
		{
			Vocabulary vocab = Tool.getLexer(language.lexerClass, null).Vocabulary as Vocabulary;
			string[] ruleNames = Tool.getParser(language.parserClass, null).RuleNames;
			CollectTokenPairs collectTokenPairs = new CollectTokenPairs(vocab, ruleNames);
			CollectSiblingLists collectSiblingLists = new CollectSiblingLists();
			foreach (InputDocument doc in documents)
			{
				collectSiblingLists.setTokens(doc.tokens, doc.tree, doc.tokenToNodeMap);
				ParseTreeWalker.Default.Walk(collectTokenPairs, doc.tree);
				ParseTreeWalker.Default.Walk(collectSiblingLists, doc.tree);
			    var cc = collectSiblingLists.SplitListForms;
			}
			ruleToPairsBag = collectTokenPairs.Dependencies;
			rootAndChildListStats = collectSiblingLists.ListStats;
			rootAndSplitChildListStats = collectSiblingLists.SplitListStats;
			tokenToListInfo = collectSiblingLists.TokenToListInfo;

			if (false)
			{
				foreach (RuleAltKey ruleAltKey in ruleToPairsBag.Keys)
				{
					IList<org.antlr.codebuff.misc.Pair<int, int>> pairs = ruleToPairsBag[ruleAltKey];
					Console.Write(ruleAltKey + " -> ");
					foreach (org.antlr.codebuff.misc.Pair<int, int> p in pairs)
					{
						Console.Write(vocab.GetDisplayName(p.a) + "," + vocab.GetDisplayName(p.b) + " ");
					}
					Console.WriteLine();
				}
			}

			if (false)
			{
				foreach (ParentSiblingListKey siblingPairs in rootAndChildListStats.Keys)
				{
					string parent = ruleNames[siblingPairs.parentRuleIndex];
					parent = parent.Replace("Context","");
					string siblingListName = ruleNames[siblingPairs.childRuleIndex];
					siblingListName = siblingListName.Replace("Context","");
					Console.WriteLine(parent + ":" + siblingPairs.parentRuleAlt + "->" + siblingListName + ":" + siblingPairs.childRuleAlt + " (n,min,median,var,max)=" + rootAndChildListStats[siblingPairs]);
				}
				IDictionary<ParentSiblingListKey, int> splitListForms = collectSiblingLists.SplitListForms;
				foreach (ParentSiblingListKey siblingPairs in rootAndSplitChildListStats.Keys)
				{
					string parent = ruleNames[siblingPairs.parentRuleIndex];
					parent = parent.Replace("Context","");
					string siblingListName = ruleNames[siblingPairs.childRuleIndex];
					siblingListName = siblingListName.Replace("Context","");
					Console.WriteLine("SPLIT " + parent + ":" + siblingPairs.parentRuleAlt + "->" + siblingListName + ":" + siblingPairs.childRuleAlt + " (n,min,median,var,max)=" + rootAndSplitChildListStats[siblingPairs] + " form " + splitListForms[siblingPairs]);
				}
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void trainOnSampleDocs() throws Exception
		public virtual void trainOnSampleDocs()
		{
			documentsPerExemplar = new List<InputDocument>();
			featureVectors = new List<int[]>();
			injectWhitespace = new List<int>();
			hpos = new List<int>();

			foreach (InputDocument doc in documents)
			{
				if (Tool.showFileNames)
				{
					Console.WriteLine(doc);
				}
				// Parse document, add feature vectors to this corpus
				Trainer trainer = new Trainer(this, doc, language.indentSize);
				trainer.computeFeatureVectors();
			}
		}

		/// <summary>
		/// Feature vectors in X are lumped together as they are read in each
		///  document. In kNN, this tends to find features from the same document
		///  rather than from across the corpus since we grab k neighbors.
		///  For k=11, we might only see exemplars from a single corpus document.
		///  If all exemplars fit in k, this wouldn't be an issue.
		/// 
		///  Fisher-Yates / Knuth shuffling
		///  "To shuffle an array a of n elements (indices 0..n-1)":
		///  https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
		/// </summary>
		public virtual void randomShuffleInPlace()
		{
			Random r = new Random(FEATURE_VECTOR_RANDOM_SEED);
			// for i from n−1 downto 1 do
			int n = featureVectors.Count;
			for (int i = n - 1; i >= 1; i--)
			{
				// j ← random integer such that 0 ≤ j ≤ i
				int j = r.Next(i + 1);
				// exchange a[j] and a[i]
				// Swap X
				int[] tmp = featureVectors[i];
				featureVectors[i] = featureVectors[j];
				featureVectors[j] = tmp;
				// And now swap all prediction lists
				int tmpI = injectWhitespace[i];
				injectWhitespace[i] = injectWhitespace[j];
				injectWhitespace[j] = tmpI;
				tmpI = hpos[i];
				hpos[i] = hpos[j];
				hpos[j] = tmpI;
				// Finally, swap documents
				InputDocument tmpD = documentsPerExemplar[i];
				documentsPerExemplar[i] = documentsPerExemplar[j];
				documentsPerExemplar[j] = tmpD;
			}
		}

		public virtual void buildTokenContextIndex()
		{
			curAndPrevTokenRuleIndexToExemplarIndexes = new MyMultiMap<org.antlr.codebuff.misc.Pair<int, int>, int>();
			wsFeaturesToExemplarIndexes = new MyMultiMap<FeatureVectorAsObject, int>();
			hposFeaturesToExemplarIndexes = new MyMultiMap<FeatureVectorAsObject, int>();
			for (int i = 0; i < featureVectors.Count; i++)
			{
				int[] features = featureVectors[i];
				int curTokenRuleIndex = features[Trainer.INDEX_PREV_EARLIEST_RIGHT_ANCESTOR];
				int prevTokenRuleIndex = features[Trainer.INDEX_EARLIEST_LEFT_ANCESTOR];
				int pr = Trainer.unrulealt(prevTokenRuleIndex)[0];
				int cr = Trainer.unrulealt(curTokenRuleIndex)[0];
				curAndPrevTokenRuleIndexToExemplarIndexes.Map(new org.antlr.codebuff.misc.Pair<int,int>(pr, cr), i);
				wsFeaturesToExemplarIndexes.Map(new FeatureVectorAsObject(features, Trainer.FEATURES_INJECT_WS), i);
				hposFeaturesToExemplarIndexes.Map(new FeatureVectorAsObject(features, Trainer.FEATURES_HPOS), i);
			}
		}
	}

}