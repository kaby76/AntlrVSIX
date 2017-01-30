using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Linq;

namespace org.antlr.codebuff.walkers
{

	using BuffUtils = org.antlr.codebuff.misc.BuffUtils;
	using CodeBuffTokenStream = org.antlr.codebuff.misc.CodeBuffTokenStream;
	using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;
    using Token = Antlr4.Runtime.IToken;
	//using Pair = org.antlr.v4.runtime.misc.Pair;
	using ErrorNode = Antlr4.Runtime.Tree.IErrorNode;
	using ParseTree = Antlr4.Runtime.Tree.IParseTree;
	using ParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
	using TerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
	using Tree = Antlr4.Runtime.Tree.ITree;
	using Trees = Antlr4.Runtime.Tree.Trees;


	public abstract class VisitSiblingLists : ParseTreeListener
	{
	    public abstract void visitNonSingletonWithSeparator<T1>(ParserRuleContext ctx, IList<T1> siblings, Token separator)
	        where T1 : Antlr4.Runtime.ParserRuleContext;

		public static IList<Tree> getSeparators<T1>(ParserRuleContext ctx, IList<T1> siblings)
            where T1 : Antlr4.Runtime.ParserRuleContext
		{
			ParserRuleContext first = siblings[0] as ParserRuleContext;
			ParserRuleContext last = siblings[siblings.Count - 1] as ParserRuleContext;
			int start = BuffUtils.indexOf(ctx, first);
			int end = BuffUtils.indexOf(ctx, last);
            IEnumerable<ITree> xxxx = Trees.GetChildren(ctx).Where((n, i) => i >= start && i < end + 1);
		    IList<Tree> elements = xxxx.ToList();
			return BuffUtils.filter(elements, c => c is TerminalNode);
		}

		/// <summary>
		/// Return map for the various tokens related to this list re list membership </summary>
		public static IDictionary<Token, org.antlr.codebuff.misc.Pair<bool, int>> getInfoAboutListTokens<T1>(ParserRuleContext ctx, CodeBuffTokenStream tokens, IDictionary<Token, TerminalNode> tokenToNodeMap, IList<T1> siblings, bool isOversizeList) where T1 : Antlr4.Runtime.ParserRuleContext
		{
			IDictionary<Token, org.antlr.codebuff.misc.Pair<bool, int>> tokenToListInfo = new Dictionary<Token, org.antlr.codebuff.misc.Pair<bool, int>>();

			ParserRuleContext first = siblings[0] as ParserRuleContext;
			ParserRuleContext last = siblings[siblings.Count - 1] as ParserRuleContext;

			Token prefixToken = tokens.getPreviousRealToken(first.Start.TokenIndex); // e.g., '(' in an arg list or ':' in grammar def
			Token suffixToken = tokens.getNextRealToken(last.Stop.TokenIndex); // e.g., LT(1) is last token of list; LT(2) is ')' in an arg list of ';' in grammar def

			TerminalNode prefixNode = tokenToNodeMap[prefixToken];
			TerminalNode suffixNode = tokenToNodeMap[suffixToken];
			bool hasSurroundingTokens = prefixNode != null && prefixNode.Parent == suffixNode.Parent;

			if (hasSurroundingTokens)
			{
				tokenToListInfo[prefixToken] = new org.antlr.codebuff.misc.Pair<bool, int>(isOversizeList, Trainer.LIST_PREFIX);
				tokenToListInfo[suffixToken] = new org.antlr.codebuff.misc.Pair<bool, int>(isOversizeList, Trainer.LIST_SUFFIX);
			}

			IList<Tree> separators = getSeparators(ctx, siblings);
			Tree firstSep = separators[0];
			tokenToListInfo[(Token)firstSep.Payload] = new org.antlr.codebuff.misc.Pair<bool, int>(isOversizeList, Trainer.LIST_FIRST_SEPARATOR);
			foreach (Tree s in separators.Where((e, i) => i > 0 && i < separators.Count))
			{
				tokenToListInfo[(Token)s.Payload] = new org.antlr.codebuff.misc.Pair<bool, int>(isOversizeList, Trainer.LIST_SEPARATOR);
			}

			// handle sibling members
			tokenToListInfo[first.Start] = new org.antlr.codebuff.misc.Pair<bool, int>(isOversizeList, Trainer.LIST_FIRST_ELEMENT);
			foreach (T1 ss in siblings.Where((e, i) => i > 0 && i < siblings.Count))
			{
			    var s = ss as ParserRuleContext;
				tokenToListInfo[s.Start] = new org.antlr.codebuff.misc.Pair<bool, int>(isOversizeList, Trainer.LIST_MEMBER);
			}

			return tokenToListInfo;
		}

	    public virtual void VisitTerminal(ITerminalNode node)
	    {
	    }

	    public virtual void VisitErrorNode(IErrorNode node)
	    {
	    }

        public virtual void EnterEveryRule(ParserRuleContext ctx)
        {
            // Find sibling lists that are children of this parent node
            ISet<Type> completed = new HashSet<Type>(); // only count sibling list for each subtree type once
            for (int i = 0; i < ctx.ChildCount; i++)
            {
                ParseTree child = ctx.GetChild(i);

                if (completed.Contains(child.GetType()))
                {
                    continue; // avoid counting repeatedly
                }
                completed.Add(child.GetType());
                if (child is TerminalNode)
                {
                    continue; // tokens are separators at most not siblings
                }

                // found subtree child
                //JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
                //ORIGINAL LINE: java.util.List<? extends org.antlr.v4.runtime.ParserRuleContext> siblings = ctx.getRuleContexts(((org.antlr.v4.runtime.ParserRuleContext) child).getClass());

                var s = ctx.GetRuleContexts<ParserRuleContext>();
                IList<ParserRuleContext> siblings = s.Where((n) => n.GetType() == child.GetType()).ToList();

                //IList<ParserRuleContext> siblings = ctx.GetRuleContexts(((ParserRuleContext) child).GetType());
                if (siblings.Count > 1)
                { // we found a list
                  // check for separator by looking between first two siblings (assume all are same)
                    ParserRuleContext first = siblings[0];
                    ParserRuleContext second = siblings[1];
                    IList<ITree> children = Trees.GetChildren(ctx);

                    int firstIndex = children.IndexOf(first);
                    int secondIndex = children.IndexOf(second);

                    if (firstIndex + 1 == secondIndex)
                    {
                        continue; // nothing between first and second so no separator
                    }

                    ParseTree between = ctx.GetChild(firstIndex + 1);
                    if (between is TerminalNode)
                    { // is it a token?
                        Token separator = ((TerminalNode)between).Symbol;
                        visitNonSingletonWithSeparator(ctx, siblings, separator);
                    }
                }
            }
        }

        public virtual void ExitEveryRule(ParserRuleContext ctx)
	    {
	    }
	}

}