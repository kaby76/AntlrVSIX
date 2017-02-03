using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using org.antlr.codebuff.misc;
using System.Linq;

namespace org.antlr.codebuff.walkers
{

    using BuffUtils = org.antlr.codebuff.misc.BuffUtils;
    using CodeBuffTokenStream = org.antlr.codebuff.misc.CodeBuffTokenStream;
    //using HashBag = org.antlr.codebuff.misc.HashBag;
    using ParentSiblingListKey = org.antlr.codebuff.misc.ParentSiblingListKey;
    using SiblingListStats = org.antlr.codebuff.misc.SiblingListStats;
    using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;
    using Token = Antlr4.Runtime.IToken;
    using ErrorNode = Antlr4.Runtime.Tree.IErrorNode;
    using TerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
    using Antlr4.Runtime;
    using System;


    /// <summary>
    /// [USED IN TRAINING ONLY]
    ///  Find subtree roots with repeated child rule subtrees and separators.
    ///  Track oversize and regular lists are sometimes treated differently, such as
    ///  formal arg lists in Java. Sometimes they are split across lines.
    /// 
    ///  A single instance is shared across all training docs to collect complete info.
    /// </summary>
    public class CollectSiblingLists : VisitSiblingLists
	{
		// listInfo and splitListInfo are used to collect statistics for use by the formatting engine when computing "is oversize list"

		/// <summary>
		/// Track set of (parent:alt,child:alt) list pairs and their min,median,variance,max
		///  but only if the list is all on one line and has a separator.
		/// </summary>
		public IDictionary<ParentSiblingListKey, IList<int>> listInfo = new Dictionary<ParentSiblingListKey, IList<int>>();

		/// <summary>
		/// Track set of (parent:alt,child:alt) list pairs and their min,median,variance,max
		///  but only if the list is split with at least one '\n' before/after
		///  a separator.
		/// </summary>
		public IDictionary<ParentSiblingListKey, IList<int>> splitListInfo = new Dictionary<ParentSiblingListKey, IList<int>>();

		/// <summary>
		/// Debugging </summary>
		public IDictionary<ParentSiblingListKey, IList<int>> splitListForm = new Dictionary<ParentSiblingListKey, IList<int>>();

		/// <summary>
		/// Map token to ("is oversize", element type). Used to compute feature vector. </summary>
		public IDictionary<Token, org.antlr.codebuff.misc.Pair<bool, int>> tokenToListInfo = new Dictionary<Token, org.antlr.codebuff.misc.Pair<bool, int>>();

		public IDictionary<Token, TerminalNode> tokenToNodeMap = null;

		public CodeBuffTokenStream tokens;

		// reuse object so the maps above fill from multiple files during training
		public virtual void setTokens(CodeBuffTokenStream tokens, ParserRuleContext root, IDictionary<Token, TerminalNode> tokenToNodeMap)
		{
			this.tokens = tokens;
			this.tokenToNodeMap = tokenToNodeMap;
		}

        public override void visitNonSingletonWithSeparator<T1>(ParserRuleContext ctx, IList<T1> siblings, IToken separator)
        {
			ParserRuleContext first = siblings[0] as Antlr4.Runtime.ParserRuleContext;
			ParserRuleContext last = siblings[siblings.Count - 1] as Antlr4.Runtime.ParserRuleContext;
			IList<Token> hiddenToLeft = tokens.GetHiddenTokensToLeft(first.Start.TokenIndex);
			IList<Token> hiddenToLeftOfSep = tokens.GetHiddenTokensToLeft(separator.TokenIndex);
			IList<Token> hiddenToRightOfSep = tokens.GetHiddenTokensToRight(separator.TokenIndex);
			IList<Token> hiddenToRight = tokens.GetHiddenTokensToRight(last.Stop.TokenIndex);

			Token hiddenTokenToLeft = hiddenToLeft != null ? hiddenToLeft[0] : null;
			Token hiddenTokenToRight = hiddenToRight != null ? hiddenToRight[0] : null;

			int[] ws = new int[4]; // '\n' (before list, before sep, after sep, after last element)
            // KED. naked new lines is not platform independent!!!!!!!!!!!!!!!!!!!!
            // STOP using naked new lines!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (hiddenTokenToLeft != null && Tool.count(hiddenTokenToLeft.Text, '\n') > 0)
			{
				ws[0] = '\n';
			}
			if (hiddenToLeftOfSep != null && Tool.count(hiddenToLeftOfSep[0].Text, '\n') > 0)
			{
				ws[1] = '\n';
	//			System.out.println("BEFORE "+JavaParser.ruleNames[ctx.getRuleIndex()]+
	//				                   "->"+JavaParser.ruleNames[ctx.getRuleIndex()]+" sep "+
	//				                   JavaParser.tokenNames[separator.getType()]+
	//				                   " "+separator);
			}
			if (hiddenToRightOfSep != null && Tool.count(hiddenToRightOfSep[0].Text, '\n') > 0)
			{
				ws[2] = '\n';
	//			System.out.println("AFTER "+JavaParser.ruleNames[ctx.getRuleIndex()]+
	//				                   "->"+JavaParser.ruleNames[ctx.getRuleIndex()]+" sep "+
	//				                   JavaParser.tokenNames[separator.getType()]+
	//				                   " "+separator);
			}
			if (hiddenTokenToRight != null && Tool.count(hiddenTokenToRight.Text, '\n') > 0)
			{
				ws[3] = '\n';
			}
			bool isSplitList = ws[1] == '\n' || ws[2] == '\n';

			// now track length of parent:alt,child:alt list or split-list
			ParentSiblingListKey pair = new ParentSiblingListKey(ctx, first, separator.Type);
			IDictionary<ParentSiblingListKey, IList<int>> info = isSplitList ? splitListInfo : listInfo;
            IList<int> lens = null;
            info.TryGetValue(pair, out lens);
			if (lens == null)
			{
				lens = new List<int>();
				info[pair] = lens;
			}
			lens.Add(Trainer.getSiblingsLength(siblings));

			// track the form split lists take for debugging
			if (isSplitList)
			{
				int form = Trainer.listform(ws);
			    IList<int> forms = null;
                splitListForm.TryGetValue(pair, out forms);
				if (forms == null)
				{
					forms = new List<int>();
					splitListForm[pair] = forms;
				}
				forms.Add(form); // track where we put newlines for this list
			}

			IDictionary<Token, org.antlr.codebuff.misc.Pair<bool, int>> tokenInfo = getInfoAboutListTokens(ctx, tokens, tokenToNodeMap, siblings, isSplitList);

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

		// for debugging
		public virtual IDictionary<ParentSiblingListKey, int> SplitListForms
		{
			get
			{
				IDictionary<ParentSiblingListKey, int> results = new Dictionary<ParentSiblingListKey, int>();
				foreach (ParentSiblingListKey pair in splitListForm.Keys)
				{
					HashBag<int> votes = new HashBag<int>();
				    IList<int> forms = null;
                    splitListForm.TryGetValue(pair, out forms);
                    if (forms != null)
					    foreach (var f in forms)
                            votes.add(f);
					int mostCommonForm = kNNClassifier.getCategoryWithMostVotes(votes);
					results[pair] = mostCommonForm;
				}
				return results;
			}
		}

		public virtual IDictionary<ParentSiblingListKey, SiblingListStats> ListStats
		{
			get
			{
				return getListStats(listInfo);
			}
		}

		public virtual IDictionary<ParentSiblingListKey, SiblingListStats> SplitListStats
		{
			get
			{
				return getListStats(splitListInfo);
			}
		}

		public virtual IDictionary<ParentSiblingListKey, SiblingListStats> getListStats(IDictionary<ParentSiblingListKey, IList<int>> map)
		{
			IDictionary<ParentSiblingListKey, SiblingListStats> listSizes = new Dictionary<ParentSiblingListKey, SiblingListStats>();
			foreach (ParentSiblingListKey pair in map.Keys)
			{
				IList<int> lens = map[pair];
			    var new_lens = lens.OrderBy(i => i).ToList();
                lens = new_lens;
				int n = lens.Count;
				int? min = lens[0];
				int? median = lens[n / 2];
				int? max = lens[n - 1];
				double @var = BuffUtils.variance(lens);
				listSizes[pair] = new SiblingListStats(n, min.Value, median.Value, @var, max.Value);
			}
			return listSizes;
		}

		public virtual IDictionary<Token, org.antlr.codebuff.misc.Pair<bool, int>> TokenToListInfo
		{
			get
			{
				return tokenToListInfo;
			}
		}

		public override void VisitTerminal(TerminalNode node)
		{
		}

        public override void VisitErrorNode(ErrorNode node)
		{
		}

		public override void ExitEveryRule(ParserRuleContext ctx)
		{
		}
	}

}