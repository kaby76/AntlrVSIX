using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;

namespace org.antlr.codebuff.walkers
{

	using CodeBuffTokenStream = org.antlr.codebuff.misc.CodeBuffTokenStream;
	using ParentSiblingListKey = org.antlr.codebuff.misc.ParentSiblingListKey;
	using SiblingListStats = org.antlr.codebuff.misc.SiblingListStats;
	using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;
	using Token = Antlr4.Runtime.IToken;
	//using Pair = org.antlr.v4.runtime.misc.Pair;
	using TerminalNode = Antlr4.Runtime.Tree.ITerminalNode;


	/// <summary>
	/// [USED IN FORMATTING ONLY]
	///  Walk tree and find and fill tokenToListInfo with all oversize lists with separators.
	/// </summary>
	public class IdentifyOversizeLists : VisitSiblingLists
	{
		internal Corpus corpus;
		internal CodeBuffTokenStream tokens;

		/// <summary>
		/// Map token to ("is oversize", element type). Used to compute feature vector. </summary>
		public IDictionary<Token, org.antlr.codebuff.misc.Pair<bool, int>> tokenToListInfo = new Dictionary<Token, org.antlr.codebuff.misc.Pair<bool, int>>();

		public IDictionary<Token, TerminalNode> tokenToNodeMap;

		public IdentifyOversizeLists(Corpus corpus, CodeBuffTokenStream tokens, IDictionary<Token, TerminalNode> tokenToNodeMap)
		{
			this.corpus = corpus;
			this.tokens = tokens;
			this.tokenToNodeMap = tokenToNodeMap;
		}

		public override void visitNonSingletonWithSeparator<T1>(ParserRuleContext ctx, IList<T1> siblings, Token separator)
            //where T1 : Antlr4.Runtime.ParserRuleContext
		{
			bool oversize = isOversizeList(ctx, siblings, separator);
			IDictionary<Token, org.antlr.codebuff.misc.Pair<bool, int>> tokenInfo = getInfoAboutListTokens(ctx, tokens, tokenToNodeMap, siblings, oversize);

			// copy sibling list info for associated tokens into overall list
			// but don't overwrite existing so that most general (largest construct)
			// list information is use/retained (i.e., not overwritten).
			foreach (Token t in tokenInfo.Keys)
			{
				if (!tokenToListInfo.ContainsKey(t))
				{
					tokenToListInfo[t] = tokenInfo[t];
				}
			}
		}

		/// <summary>
		/// Return true if we've only seen parent-sibling-separator combo as a split list.
		///  Return true if we've seen that combo as both list and split list AND
		///  len of all siblings is closer to split median than to regular nonsplit median.
		/// </summary>
		public virtual bool isOversizeList<T1>(ParserRuleContext ctx, IList<T1> siblings, Token separator) where T1 : Antlr4.Runtime.ParserRuleContext
		{
			ParserRuleContext first = siblings[0] as ParserRuleContext;
			ParentSiblingListKey pair = new ParentSiblingListKey(ctx, first, separator.Type);
		    SiblingListStats stats = null;
            corpus.rootAndChildListStats.TryGetValue(pair, out stats);
		    SiblingListStats splitStats = null;
            corpus.rootAndSplitChildListStats.TryGetValue(pair, out splitStats);
			bool oversize = stats == null && splitStats != null;

			if (stats != null && splitStats == null)
			{
				// note: if we've never seen a split version of this ctx, do nothing;
				// I used to have oversize failsafe
			}

			int len = Trainer.getSiblingsLength(siblings);
			if (stats != null && splitStats != null)
			{
				// compare distance in units of standard deviations to regular or split means
				// like a one-dimensional Mahalanobis distance.
				// actually i took out the stddev divisor. they are usually very spread out and overlapping.
				double distToSplit = Math.Abs(splitStats.median - len);
				double distToSplitSquared = Math.Pow(distToSplit,2);
				double distToSplitStddevUnits = distToSplitSquared / Math.Sqrt(splitStats.variance);

				double distToRegular = Math.Abs(stats.median - len);
				double distToRegularSquared = Math.Pow(distToRegular,2);
				double distToRegularStddevUnits = distToRegularSquared / Math.Sqrt(stats.variance);

				// consider a priori probabilities as well.
				float n = splitStats.numSamples + stats.numSamples;
				float probSplit = splitStats.numSamples / n;
				float probRegular = stats.numSamples / n;
				double adjDistToSplit = distToSplitSquared * (1 - probSplit); // make distance smaller if probSplit is high
				double adjDistToRegular = distToRegularSquared * (1 - probRegular);
				if (adjDistToSplit < adjDistToRegular)
				{
					oversize = true;
				}
			}
			return oversize;
		}
	}

}