using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using org.antlr.codebuff.misc;

namespace org.antlr.codebuff
{

	using CodeBuffTokenStream = org.antlr.codebuff.misc.CodeBuffTokenStream;
	using RuleAltKey = org.antlr.codebuff.misc.RuleAltKey;
	using CollectTokenPairs = org.antlr.codebuff.walkers.CollectTokenPairs;
	using CommonTokenStream = Antlr4.Runtime.CommonTokenStream;
	using Lexer = Antlr4.Runtime.Lexer;
	using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;
	using Token = Antlr4.Runtime.IToken;
	using Vocabulary = Antlr4.Runtime.Vocabulary;
	using ATN = Antlr4.Runtime.Atn.ATN;
	//using Pair = Antlr4.Runtime.Misc.Pair<,>;
	using ErrorNode = Antlr4.Runtime.Tree.IErrorNode;
	using ParseTree = Antlr4.Runtime.Tree.IParseTree;
	using ParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
	using ParseTreeWalker = Antlr4.Runtime.Tree.ParseTreeWalker;
	using TerminalNode = Antlr4.Runtime.Tree.ITerminalNode;

	/// <summary>
	/// Collect feature vectors trained on a single file.
	/// 
	///  The primary results are: (X, Y1, Y2)
	///  For each feature vector, features[i], injectWhitespace[i] and align[i] tell us
	///  the decisions associated with that context in a corpus file.
	///  After calling <seealso cref="#computeFeatureVectors()"/>, those lists
	/// 
	///  There is no shared state computed by this object, only static defs
	///  of feature types and category constants.
	/// </summary>
	public class Trainer
	{
		public const double MAX_WS_CONTEXT_DIFF_THRESHOLD = 0.12; //1.0/7;
		public const double MAX_ALIGN_CONTEXT_DIFF_THRESHOLD = 0.15;
		public const double MAX_CONTEXT_DIFF_THRESHOLD2 = 0.50;

        /// <summary>
        /// When computing child indexes, we use this value for any child list
        ///  element other than the first one.  If a parent has just one X child,
        ///  we use the actual child index. If parent has two or more X children,
        ///  and we are not the first X, use CHILD_INDEX_REPEATED_ELEMENT. If first
        ///  of two or more X children, use actual child index.
        /// </summary>
        public const int CHILD_INDEX_REPEATED_ELEMENT = 1111111111; //1_111_111_111;

		public const int LIST_PREFIX = 0;
		public const int LIST_FIRST_ELEMENT = 1;
		public const int LIST_FIRST_SEPARATOR = 2;
		public const int LIST_SEPARATOR = 3;
		public const int LIST_SUFFIX = 4;
        public const int LIST_MEMBER = 1111111111; //1_111_111_111;

		// Feature values for pair starts lines feature either T/F or:
		public const int NOT_PAIR = -1;

		// Categories for newline, whitespace. CAT_INJECT_NL+n<<8 or CAT_INJECT_WS+n<<8
		public const int CAT_NO_WS = 0;
		public const int CAT_INJECT_NL = 100;
		public const int CAT_INJECT_WS = 200;

		// Categories for alignment/indentation
		public const int CAT_ALIGN = 0;

		/* We want to identify alignment with a child's start token of some parent
		   but that parent could be a number of levels up the tree. The next category
		   values indicate alignment from the current token's left ancestor's
		   parent then it's parent and so on. For category value:
	
		    CAT_ALIGN_WITH_ANCESTOR_CHILD | delta<<8 | childindex<<16
	
		   current token is aligned with start token of child childindex,
		   delta levels up from ancestor.
		 */
		public const int CAT_ALIGN_WITH_ANCESTOR_CHILD = 10;

		/* We want to identify indentation from a parent's start token but that
		   parent could be a number of levels up the tree. The next category
		   values indicate indentation from the current token's left ancestor's
		   parent then it's parent and so on. For category value:
	
		    CAT_INDENT_FROM_ANCESTOR_FIRST_TOKEN | delta<<8
	
		   current token is indented from start token of node i levels up
		   from ancestor.
		 */
		public const int CAT_INDENT_FROM_ANCESTOR_CHILD = 20; // left ancestor's first token is really current token

		public const int CAT_INDENT = 30;

		// indexes into feature vector

		public const int INDEX_PREV_TYPE = 0;
		public const int INDEX_PREV_FIRST_ON_LINE = 1; // a \n right before this token?
		public const int INDEX_PREV_EARLIEST_RIGHT_ANCESTOR = 2;
		public const int INDEX_CUR_TOKEN_TYPE = 3;
		public const int INDEX_MATCHING_TOKEN_STARTS_LINE = 4;
		public const int INDEX_MATCHING_TOKEN_ENDS_LINE = 5;
		public const int INDEX_FIRST_ON_LINE = 6; // a \n right before this token?
		public const int INDEX_MEMBER_OVERSIZE_LIST = 7; // -1 if we don't know; false means list but not big list
		public const int INDEX_LIST_ELEMENT_TYPE = 8; // see LIST_PREFIX, etc...
		public const int INDEX_CUR_TOKEN_CHILD_INDEX = 9; // left ancestor
		public const int INDEX_EARLIEST_LEFT_ANCESTOR = 10;
		public const int INDEX_ANCESTORS_CHILD_INDEX = 11; // left ancestor
		public const int INDEX_ANCESTORS_PARENT_RULE = 12;
		public const int INDEX_ANCESTORS_PARENT_CHILD_INDEX = 13;
		public const int INDEX_ANCESTORS_PARENT2_RULE = 14;
		public const int INDEX_ANCESTORS_PARENT2_CHILD_INDEX = 15;
		public const int INDEX_ANCESTORS_PARENT3_RULE = 16;
		public const int INDEX_ANCESTORS_PARENT3_CHILD_INDEX = 17;
		public const int INDEX_ANCESTORS_PARENT4_RULE = 18;
		public const int INDEX_ANCESTORS_PARENT4_CHILD_INDEX = 19;
		public const int INDEX_ANCESTORS_PARENT5_RULE = 20;
		public const int INDEX_ANCESTORS_PARENT5_CHILD_INDEX = 21;

		public const int INDEX_INFO_FILE = 22;
		public const int INDEX_INFO_LINE = 23;
		public const int INDEX_INFO_CHARPOS = 24;

		public const int NUM_FEATURES = 25;
		public const int ANALYSIS_START_TOKEN_INDEX = 1; // we use current and previous token in context so can't start at index 0

		public static readonly FeatureMetaData[] FEATURES_INJECT_WS = new FeatureMetaData[]
		{
			new FeatureMetaData(FeatureType.TOKEN, new string[] {"", "LT(-1)"}, 1),
			new FeatureMetaData(FeatureType.BOOL, new string[] {"Strt", "line"}, 1),
			new FeatureMetaData(FeatureType.RULE, new string[] {"LT(-1)", "right ancestor"}, 1),
			new FeatureMetaData(FeatureType.TOKEN, new string[] {"", "LT(1)"}, 1),
			new FeatureMetaData(FeatureType.BOOL, new string[] {"Pair", "strt\\n"}, 1),
			new FeatureMetaData(FeatureType.BOOL, new string[] {"Pair", "end\\n"}, 1),
			FeatureMetaData.UNUSED,
			new FeatureMetaData(FeatureType.BOOL, new string[] {"Big", "list"}, 2),
			new FeatureMetaData(FeatureType.INT, new string[] {"List", "elem."}, 1),
			new FeatureMetaData(FeatureType.INT, new string[] {"token", "child index"}, 1),
			new FeatureMetaData(FeatureType.RULE, new string[] {"LT(1)", "left ancestor"}, 1),
			FeatureMetaData.UNUSED,
			new FeatureMetaData(FeatureType.RULE, new string[] {"", "parent"}, 1),
			FeatureMetaData.UNUSED,
			FeatureMetaData.UNUSED,
			FeatureMetaData.UNUSED,
			FeatureMetaData.UNUSED,
			FeatureMetaData.UNUSED,
			FeatureMetaData.UNUSED,
			FeatureMetaData.UNUSED,
			FeatureMetaData.UNUSED,
			FeatureMetaData.UNUSED,
			new FeatureMetaData(FeatureType.INFO_FILE, new string[] {"", "file"}, 0),
			new FeatureMetaData(FeatureType.INFO_LINE, new string[] {"", "line"}, 0),
			new FeatureMetaData(FeatureType.INFO_CHARPOS, new string[] {"char", "pos"}, 0)
		};

		public static readonly FeatureMetaData[] FEATURES_HPOS = new FeatureMetaData[]
		{
		    FeatureMetaData.UNUSED,
            FeatureMetaData.UNUSED,
            new FeatureMetaData(FeatureType.RULE,
                new string[] {"LT(-1)", "right ancestor"}, 1),
            new FeatureMetaData(FeatureType.TOKEN,
                new string[] {"", "LT(1)"}, 1),
            FeatureMetaData.UNUSED,
            FeatureMetaData.UNUSED,
            new FeatureMetaData(FeatureType.BOOL,
                new string[] {"Strt", "line"}, 4),
            new FeatureMetaData(FeatureType.BOOL,
                new string[] {"Big", "list"}, 1),
            new FeatureMetaData(FeatureType.INT,
                new string[] {"List", "elem."}, 2),
            new FeatureMetaData(FeatureType.INT,
                new string[] {"token", "child index"}, 1),
            new FeatureMetaData(FeatureType.RULE,
                new string[] {"LT(1)", "left ancestor"}, 1),
            new FeatureMetaData(FeatureType.INT,
                new string[] {"ancestor", "child index"}, 1),
            new FeatureMetaData(FeatureType.RULE,
                new string[] {"", "parent"}, 1),
            new FeatureMetaData(FeatureType.INT,
                new string[] {"parent", "child index"}, 1),
            new FeatureMetaData(FeatureType.RULE,
                new string[] {"", "parent^2"}, 1),
            new FeatureMetaData(FeatureType.INT,
                new string[] {"parent^2", "child index"}, 1),
            new FeatureMetaData(FeatureType.RULE,
                new string[] {"", "parent^3"}, 1),
            new FeatureMetaData(FeatureType.INT,
                new string[] {"parent^3", "child index"}, 1),
            new FeatureMetaData(FeatureType.RULE,
                new string[] {"", "parent^4"}, 1),
            new FeatureMetaData(FeatureType.INT,
                new string[] {"parent^4", "child index"}, 1),
            new FeatureMetaData(FeatureType.RULE,
                new string[] {"", "parent^5"}, 1),
            new FeatureMetaData(FeatureType.INT,
                new string[] {"parent^5", "child index"}, 1),
            new FeatureMetaData(FeatureType.INFO_FILE,
                new string[] {"", "file"}, 0),
            new FeatureMetaData(FeatureType.INFO_LINE,
                new string[] {"", "line"}, 0),
            new FeatureMetaData(FeatureType.INFO_CHARPOS,
                new string[] {"char", "pos"}, 0)
		};

		public static readonly FeatureMetaData[] FEATURES_ALL = new FeatureMetaData[]
		{
			new FeatureMetaData(FeatureType.TOKEN, new string[] {"", "LT(-1)"}, 1),
			new FeatureMetaData(FeatureType.BOOL, new string[] {"Strt", "line"}, 1),
			new FeatureMetaData(FeatureType.RULE, new string[] {"LT(-1)", "right ancestor"}, 1),
			new FeatureMetaData(FeatureType.TOKEN, new string[] {"", "LT(1)"}, 1),
			new FeatureMetaData(FeatureType.BOOL, new string[] {"Pair", "strt\\n"}, 1),
			new FeatureMetaData(FeatureType.BOOL, new string[] {"Pair", "end\\n"}, 1),
			new FeatureMetaData(FeatureType.BOOL, new string[] {"Strt", "line"}, 1),
			new FeatureMetaData(FeatureType.BOOL, new string[] {"Big", "list"}, 1),
			new FeatureMetaData(FeatureType.INT, new string[] {"List", "elem."}, 1),
			new FeatureMetaData(FeatureType.INT, new string[] {"token", "child index"}, 1),
			new FeatureMetaData(FeatureType.RULE, new string[] {"LT(1)", "left ancestor"}, 1),
			new FeatureMetaData(FeatureType.INT, new string[] {"ancestor", "child index"}, 1),
			new FeatureMetaData(FeatureType.RULE, new string[] {"", "parent"}, 1),
			new FeatureMetaData(FeatureType.INT, new string[] {"parent", "child index"}, 1),
			new FeatureMetaData(FeatureType.RULE, new string[] {"", "parent^2"}, 1),
			new FeatureMetaData(FeatureType.INT, new string[] {"parent^2", "child index"}, 1),
			new FeatureMetaData(FeatureType.RULE, new string[] {"", "parent^3"}, 1),
			new FeatureMetaData(FeatureType.INT, new string[] {"parent^3", "child index"}, 1),
			new FeatureMetaData(FeatureType.RULE, new string[] {"", "parent^4"}, 1),
			new FeatureMetaData(FeatureType.INT, new string[] {"parent^4", "child index"}, 1),
			new FeatureMetaData(FeatureType.RULE, new string[] {"", "parent^5"}, 1),
			new FeatureMetaData(FeatureType.INT, new string[] {"parent^5", "child index"}, 1),
			new FeatureMetaData(FeatureType.INFO_FILE, new string[] {"", "file"}, 0),
			new FeatureMetaData(FeatureType.INFO_LINE, new string[] {"", "line"}, 0),
			new FeatureMetaData(FeatureType.INFO_CHARPOS, new string[] {"char", "pos"}, 0)
		};

		protected internal Corpus corpus;
		protected internal InputDocument doc;
		protected internal ParserRuleContext root;
		protected internal CodeBuffTokenStream tokens; // track stream so we can examine previous tokens
		protected internal int indentSize;

		/// <summary>
		/// Make it fast to get a node for a specific token </summary>
		protected internal IDictionary<Token, TerminalNode> tokenToNodeMap = null;

		public Trainer(Corpus corpus, InputDocument doc, int indentSize)
		{
			this.corpus = corpus;
			this.doc = doc;
			this.root = doc.tree;
			this.tokenToNodeMap = doc.tokenToNodeMap;
			this.tokens = doc.tokens;
			this.indentSize = indentSize;
		}

		public virtual void computeFeatureVectors()
		{
			IList<Token> realTokens = getRealTokens(tokens);

			for (int i = ANALYSIS_START_TOKEN_INDEX; i < realTokens.Count; i++)
			{ // can't process first token
				int tokenIndexInStream = realTokens[i].TokenIndex;
				computeFeatureVectorForToken(tokenIndexInStream);
			}
		}

		public virtual void computeFeatureVectorForToken(int i)
		{
			Token curToken = tokens.Get(i);
			if (curToken.Type == TokenConstants.EOF)
			{
				return;
			}

			int[] features = getFeatures(i);

			int injectNL_WS = getInjectWSCategory(tokens, i);

			int aligned = -1; // "don't care"
			if ((injectNL_WS & 0xFF) == CAT_INJECT_NL)
			{
				TerminalNode node = tokenToNodeMap[curToken];
				aligned = getAlignmentCategory(doc, node, indentSize);
			}

			// track feature -> injectws, align decisions for token i
			corpus.addExemplar(doc, features, injectNL_WS, aligned);
		}

		public static int getInjectWSCategory(CodeBuffTokenStream tokens, int i)
		{
			int precedingNL = getPrecedingNL(tokens, i); // how many lines to inject

			Token curToken = tokens.Get(i);
			Token prevToken = tokens.getPreviousRealToken(i);

			int ws = 0;
			if (precedingNL == 0)
			{
				ws = curToken.Column - (prevToken.Column + prevToken.Text.Length);
			}

			int injectNL_WS = CAT_NO_WS;
			if (precedingNL > 0)
			{
				injectNL_WS = nlcat(precedingNL);
			}
			else if (ws > 0)
			{
				injectNL_WS = wscat(ws);
			}

			return injectNL_WS;
		}

		// at a newline, are we aligned with a prior sibling (in a list) etc...
		public static int getAlignmentCategory(InputDocument doc, TerminalNode node, int indentSize)
		{
            org.antlr.codebuff.misc.Pair<int, int> alignInfo = null;
            org.antlr.codebuff.misc.Pair<int, int> indentInfo = null;

			Token curToken = node.Symbol;

			// at a newline, are we aligned with a prior sibling (in a list) etc...
			ParserRuleContext earliestLeftAncestor = earliestAncestorStartingWithToken(node);
            org.antlr.codebuff.misc.Pair<ParserRuleContext, int> alignPair = earliestAncestorWithChildStartingAtCharPos(earliestLeftAncestor, curToken, curToken.Column);
	//		String[] ruleNames = doc.parser.getRuleNames();
	//		Token prevToken = doc.tokens.getPreviousRealToken(curToken.getTokenIndex());
			if (alignPair != null)
			{
				int deltaFromLeftAncestor = getDeltaToAncestor(earliestLeftAncestor, alignPair.a);
				alignInfo = new org.antlr.codebuff.misc.Pair<int, int>(deltaFromLeftAncestor, alignPair.b);
	//			int ruleIndex = pair.a.getRuleIndex();
	//			System.out.printf("ALIGN %s %s i=%d %s %s\n",
	//			                  curToken,
	//			                  ruleNames[ruleIndex],
	//			                  pair.b, alignInfo, doc.fileName);
			}

			// perhaps we are indented as well?
			int tokenIndexInStream = node.Symbol.TokenIndex;
			IList<Token> tokensOnPreviousLine = getTokensOnPreviousLine(doc.tokens, tokenIndexInStream, curToken.Line);
			Token firstTokenOnPrevLine = null;
			int columnDelta = 0;
			if (tokensOnPreviousLine.Count > 0)
			{
				firstTokenOnPrevLine = tokensOnPreviousLine[0];
				columnDelta = curToken.Column - firstTokenOnPrevLine.Column;
			}

            org.antlr.codebuff.misc.Pair<ParserRuleContext, int> indentPair = null;
			if (columnDelta != 0)
			{
				int indentedFromPos = curToken.Column - indentSize;
				indentPair = earliestAncestorWithChildStartingAtCharPos(earliestLeftAncestor, curToken, indentedFromPos);
				if (indentPair == null)
				{
					// try with 2 indents (commented out for now; can't encode how many indents in directive)
	//				indentedFromPos = curToken.getCharPositionInLine()-2*indentSize;
	//				pair = earliestAncestorWithChildStartingAtCharPos(earliestLeftAncestor, curToken, indentedFromPos);
				}
				if (indentPair != null)
				{
					int deltaFromLeftAncestor = getDeltaToAncestor(earliestLeftAncestor, indentPair.a);
					indentInfo = new org.antlr.codebuff.misc.Pair<int, int>(deltaFromLeftAncestor, indentPair.b);
	//				int ruleIndex = pair.a.getRuleIndex();
	//				System.out.printf("INDENT %s %s i=%d %s %s\n",
	//				                  curToken,
	//				                  ruleNames[ruleIndex],
	//				                  pair.b, indentInfo, doc.fileName);
				}
			}

			/*
			I tried reducing all specific and alignment operations to the generic
			"indent/align from first token on previous line" directives but
			the contexts were not sufficiently precise. method bodies got doubly
			indented when there is a throws clause etc... Might be worth pursuing
			in the future if I can increase context information regarding
			exactly what kind of method declaration signature it is. See
			targetTokenIsFirstTokenOnPrevLine().
			 */
			// If both align and indent from ancestor child exist, choose closest (lowest delta up tree)
			if (alignInfo != null && indentInfo != null)
			{
				if (alignInfo.a < indentInfo.a)
				{
					return aligncat(alignInfo.a, alignInfo.b);
				}
				// Choose indentation over alignment if both at same ancestor level
				return indentcat(indentInfo.a, indentInfo.b);
	//			return aligncat(alignInfo.a, alignInfo.b); // Should not use alignment over indentation; manual review of output shows indentation kinda messed up
			}

			// otherwise just return the align or indent we computed
			if (alignInfo != null)
			{
				return aligncat(alignInfo.a, alignInfo.b);
			}
			else if (indentInfo != null)
			{
				return indentcat(indentInfo.a, indentInfo.b);
			}

			if (columnDelta != 0)
			{
				return CAT_INDENT; // indent standard amount
			}

			return CAT_ALIGN; // otherwise just line up with first token of previous line
		}

		public static int getPrecedingNL(CommonTokenStream tokens, int i)
		{
			int precedingNL = 0;
			IList<Token> previousWS = getPreviousWS(tokens, i);
			if (previousWS != null)
			{
				foreach (Token ws in previousWS)
				{
					precedingNL += Tool.count(ws.Text, '\n');
				}
			}
			return precedingNL;
		}

		// if we have non-ws tokens like comments, we only count ws not in comments
		public static IList<Token> getPreviousWS(CommonTokenStream tokens, int i)
		{
			IList<Token> hiddenTokensToLeft = tokens.GetHiddenTokensToLeft(i);
			if (hiddenTokensToLeft == null)
			{
				return null;
			}
		    Regex rex = new Regex("^\\s+$");
			return BuffUtils.filter(hiddenTokensToLeft, t => rex.IsMatch(t.Text));
		}

		public static bool hasCommentToken(IList<Token> hiddenTokensToLeft)
		{
			bool hasComment = false;
			foreach (Token hidden in hiddenTokensToLeft)
			{
				string hiddenText = hidden.Text;
                Regex rex = new Regex("^\\s+$");
                if (!rex.IsMatch(hiddenText))
				{
					hasComment = true;
					break;
				}
			}
			return hasComment;
		}

		/// <summary>
		/// Return first ancestor of p that is not an only child including p.
		///  So if p.getParent().getChildCount()>1, this returns p.  If we
		///  have found a chain rule at p's parent (p is its only child), then
		///  move p to its parent and try again.
		/// </summary>
		public static ParserRuleContext getParentClosure(ParserRuleContext p)
		{
			if (p == null)
			{
				return null;
			}
			// if p not an only child, return p
			if (p.Parent == null || p.Parent.ChildCount > 1)
			{
				return p;
			}
			// we found a chain rule node
			return getParentClosure(p.Parent as ParserRuleContext);
		}

		/// <summary>
		/// Walk upwards from node while p.start == token; return null if there is
		///  no ancestor starting at token.
		/// </summary>
		/// <summary>
		/// Walk upwards from node while p.start == token; return immediate parent
		///  if there is no ancestor starting at token. This is the earliest
		///  left ancestor. E.g, for '{' of a block, return parent up the chain from
		///  block starting with '{'. For '}' of block, return just block as nothing
		///  starts with '}'. (block stops with it).
		/// </summary>
		public static ParserRuleContext earliestAncestorStartingWithToken(TerminalNode node)
		{
			Token token = node.Symbol;
			ParserRuleContext p = (ParserRuleContext)node.Parent;
			ParserRuleContext prev = null;
			while (p != null && p.Start == token)
			{
				prev = p;
				p = p.Parent as ParserRuleContext;
			}
			if (prev == null)
			{
				return (ParserRuleContext)node.Parent;
			}
			return prev;
		}

		/// <summary>
		/// Walk upwards from node while p.stop == token; return immediate parent
		///  if there is no ancestor stopping at token. This is the earliest
		///  right ancestor. E.g, for '}' of a block, return parent up the chain from
		///  block stopping with '}'. For '{' of block, return just block as nothing
		///  stops with '{'. (block starts with it).
		/// </summary>
		public static ParserRuleContext earliestAncestorEndingWithToken(TerminalNode node)
		{
			Token token = node.Symbol;
			ParserRuleContext p = (ParserRuleContext)node.Parent;
			ParserRuleContext prev = null;
			while (p != null && p.Stop == token)
			{
				prev = p;
				p = p.Parent as ParserRuleContext;
			}
			if (prev == null)
			{
				return (ParserRuleContext)node.Parent;
			}
			return prev;
		}

		/// <summary>
		/// Walk upwards from node until we find a child of p at t's char position.
		///  Don't see alignment with self, t, or element *after* us.
		///  return null if there is no such ancestor p.
		/// </summary>
		public static org.antlr.codebuff.misc.Pair<ParserRuleContext, int> earliestAncestorWithChildStartingAtCharPos(ParserRuleContext node, Token t, int charpos)
		{
			ParserRuleContext p = node;
			while (p != null)
			{
				// check all children of p to see if one of them starts at charpos
				for (int i = 0; i < p.ChildCount; i++)
				{
					ParseTree child = p.GetChild(i);
					Token start;
					if (child is ParserRuleContext)
					{
						start = ((ParserRuleContext) child).Start;
					}
					else
					{ // must be token
						start = ((TerminalNode)child).Symbol;
					}
					// check that we don't see alignment with self or element *after* us
					if (start.TokenIndex < t.TokenIndex && start.Column == charpos)
					{
						return new org.antlr.codebuff.misc.Pair<ParserRuleContext, int>(p,i);
					}
				}
				p = p.Parent as ParserRuleContext;
			}
			return null;
		}

		/// <summary>
		/// Return the number of hops to get to ancestor from node or -1 if we
		///  don't find ancestor on path to root.
		/// </summary>
		public static int getDeltaToAncestor(ParserRuleContext node, ParserRuleContext ancestor)
		{
			int n = 0;
			ParserRuleContext p = node;
			while (p != null && p != ancestor)
			{
				n++;
				p = p.Parent as ParserRuleContext;
			}
			if (p == null)
			{
				return -1;
			}
			return n;
		}

		public static ParserRuleContext getAncestor(ParserRuleContext node, int delta)
		{
			int n = 0;
			ParserRuleContext p = node;
			while (p != null && n != delta)
			{
				n++;
				p = p.Parent as ParserRuleContext;
			}
			return p;
		}

		public virtual int[] getFeatures(int i)
		{
			CodeBuffTokenStream tokens = doc.tokens;
			TerminalNode node = tokenToNodeMap[tokens.Get(i)];
			if (node == null)
			{
				Console.Error.WriteLine("### No node associated with token " + tokens.Get(i));
				return null;
			}

			Token curToken = node.Symbol;
			Token prevToken = tokens.getPreviousRealToken(i);
			Token prevPrevToken = prevToken != null ? doc.tokens.getPreviousRealToken(prevToken.TokenIndex) : null;

			bool prevTokenStartsLine = false;
			if (prevToken != null && prevPrevToken != null)
			{
				prevTokenStartsLine = prevToken.Line > prevPrevToken.Line;
			}

			bool curTokenStartsNewLine = false;
			if (prevToken == null)
			{
				curTokenStartsNewLine = true; // we must be at start of file
			}
			else if (curToken.Line > prevToken.Line)
			{
				curTokenStartsNewLine = true;
			}

			int[] features = getContextFeatures(corpus, tokenToNodeMap, doc, i);

			setListInfoFeatures(corpus.tokenToListInfo, features, curToken);

			features[INDEX_PREV_FIRST_ON_LINE] = prevTokenStartsLine ? 1 : 0;
			features[INDEX_FIRST_ON_LINE] = curTokenStartsNewLine ? 1 : 0;

			return features;
		}

		/// <summary>
		/// Get the token type and tree ancestor features. These are computed
		///  the same for both training and formatting.
		/// </summary>
		public static int[] getContextFeatures(Corpus corpus, IDictionary<Token, TerminalNode> tokenToNodeMap, InputDocument doc, int i)
		{
			int[] features = new int[NUM_FEATURES];
			CodeBuffTokenStream tokens = doc.tokens;
			TerminalNode node = tokenToNodeMap[tokens.Get(i)];
			if (node == null)
			{
				Console.Error.WriteLine("### No node associated with token " + tokens.Get(i));
				return features;
			}
			Token curToken = node.Symbol;

			// Get context information for previous token
			Token prevToken = tokens.getPreviousRealToken(i);
			TerminalNode prevNode = tokenToNodeMap[prevToken];

			ParserRuleContext prevEarliestRightAncestor = earliestAncestorEndingWithToken(prevNode);
			int prevEarliestAncestorRuleIndex = prevEarliestRightAncestor.RuleIndex;
			int prevEarliestAncestorRuleAltNum = prevEarliestRightAncestor.getAltNumber();

			// Get context information for current token
			ParserRuleContext earliestLeftAncestor = earliestAncestorStartingWithToken(node);

			ParserRuleContext earliestLeftAncestorParent = (earliestLeftAncestor != null ? earliestLeftAncestor.Parent : null) as ParserRuleContext;

			ParserRuleContext earliestLeftAncestorParent2 = (earliestLeftAncestorParent != null ? earliestLeftAncestorParent.Parent : null) as ParserRuleContext;

			ParserRuleContext earliestLeftAncestorParent3 = (earliestLeftAncestorParent2 != null ? earliestLeftAncestorParent2.Parent : null) as ParserRuleContext;

			ParserRuleContext earliestLeftAncestorParent4 = (earliestLeftAncestorParent3 != null ? earliestLeftAncestorParent3.Parent : null) as ParserRuleContext;

			ParserRuleContext earliestLeftAncestorParent5 = (earliestLeftAncestorParent4 != null ? earliestLeftAncestorParent4.Parent : null) as ParserRuleContext;

			features[INDEX_PREV_TYPE] = prevToken.Type;
			features[INDEX_PREV_EARLIEST_RIGHT_ANCESTOR] = rulealt(prevEarliestAncestorRuleIndex,prevEarliestAncestorRuleAltNum);
			features[INDEX_CUR_TOKEN_TYPE] = curToken.Type;
			features[INDEX_CUR_TOKEN_CHILD_INDEX] = getChildIndexOrListMembership(node);
			features[INDEX_EARLIEST_LEFT_ANCESTOR] = rulealt(earliestLeftAncestor);
			features[INDEX_ANCESTORS_CHILD_INDEX] = getChildIndexOrListMembership(earliestLeftAncestor);
			features[INDEX_ANCESTORS_PARENT_RULE] = earliestLeftAncestorParent != null ? rulealt(earliestLeftAncestorParent) : -1;
			features[INDEX_ANCESTORS_PARENT_CHILD_INDEX] = getChildIndexOrListMembership(earliestLeftAncestorParent);
			features[INDEX_ANCESTORS_PARENT2_RULE] = earliestLeftAncestorParent2 != null ? rulealt(earliestLeftAncestorParent2) : -1;
			features[INDEX_ANCESTORS_PARENT2_CHILD_INDEX] = getChildIndexOrListMembership(earliestLeftAncestorParent2);
			features[INDEX_ANCESTORS_PARENT3_RULE] = earliestLeftAncestorParent3 != null ? rulealt(earliestLeftAncestorParent3) : -1;
			features[INDEX_ANCESTORS_PARENT3_CHILD_INDEX] = getChildIndexOrListMembership(earliestLeftAncestorParent3);
			features[INDEX_ANCESTORS_PARENT4_RULE] = earliestLeftAncestorParent4 != null ? rulealt(earliestLeftAncestorParent4) : -1;
			features[INDEX_ANCESTORS_PARENT4_CHILD_INDEX] = getChildIndexOrListMembership(earliestLeftAncestorParent4);
			features[INDEX_ANCESTORS_PARENT5_RULE] = earliestLeftAncestorParent5 != null ? rulealt(earliestLeftAncestorParent5) : -1;
			features[INDEX_ANCESTORS_PARENT5_CHILD_INDEX] = getChildIndexOrListMembership(earliestLeftAncestorParent5);

			features[INDEX_MATCHING_TOKEN_STARTS_LINE] = getMatchingSymbolStartsLine(corpus, doc, node);
			features[INDEX_MATCHING_TOKEN_ENDS_LINE] = getMatchingSymbolEndsLine(corpus, doc, node);

			features[INDEX_INFO_FILE] = 0; // dummy; _toString() dumps filename w/o this value; placeholder for col in printout
			features[INDEX_INFO_LINE] = curToken.Line;
			features[INDEX_INFO_CHARPOS] = curToken.Column;

			return features;
		}

		public static void setListInfoFeatures(IDictionary<Token, org.antlr.codebuff.misc.Pair<bool, int>> tokenToListInfo, int[] features, Token curToken)
		{
			int isOversizeList = -1;
			int listElementType = -1;
            org.antlr.codebuff.misc.Pair<bool, int> listInfo = null;
            tokenToListInfo.TryGetValue(curToken, out listInfo);
			if (listInfo != null)
			{
				isOversizeList = listInfo.a ? 1 : 0;
				listElementType = listInfo.b;
			}
			features[INDEX_MEMBER_OVERSIZE_LIST] = isOversizeList; // -1 if we don't know; false means list but not big list
			features[INDEX_LIST_ELEMENT_TYPE] = listElementType;
		}

		public static int getSiblingsLength<T1>(IList<T1> siblings) where T1 : Antlr4.Runtime.ParserRuleContext
		{
			int len = 0;
			foreach (ParserRuleContext sib in siblings)
			{
				len += sib.GetText().Length;
			}
			return len;
		}

		public static string getText<T1>(IList<T1> tokens) where T1 : Antlr4.Runtime.IToken
		{
			if (tokens == null)
			{
				return "";
			}
			StringBuilder buf = new StringBuilder();
			foreach (Token sib in tokens)
			{
				buf.Append(sib.Text);
			}
			return buf.ToString();
		}


		public static int getMatchingSymbolStartsLine(Corpus corpus, InputDocument doc, TerminalNode node)
		{
			TerminalNode matchingLeftNode = getMatchingLeftSymbol(corpus, doc, node);
			if (matchingLeftNode != null)
			{
				Token matchingLeftToken = matchingLeftNode.Symbol;
				int i = matchingLeftToken.TokenIndex;
				if (i == 0)
				{
					return 1; // first token is considered first on line
				}
				Token tokenBeforeMatchingToken = doc.tokens.getPreviousRealToken(i);
	//			System.out.printf("doc=%s node=%s, pair=%s, before=%s\n",
	//			                  new File(doc.fileName).getName(), node.getSymbol(), matchingLeftToken, tokenBeforeMatchingToken);
				if (tokenBeforeMatchingToken != null)
				{
					return matchingLeftToken.Line > tokenBeforeMatchingToken.Line ? 1 : 0;
				}
				else
				{ // matchingLeftToken must be first in file
					return 1;
				}
			}
			return NOT_PAIR;
		}

		public static int getMatchingSymbolEndsLine(Corpus corpus, InputDocument doc, TerminalNode node)
		{
			TerminalNode matchingLeftNode = getMatchingLeftSymbol(corpus, doc, node);
			if (matchingLeftNode != null)
			{
				Token matchingLeftToken = matchingLeftNode.Symbol;
				int i = matchingLeftToken.TokenIndex;
				Token tokenAfterMatchingToken = doc.tokens.getNextRealToken(i);
	//			System.out.printf("doc=%s node=%s, pair=%s, after=%s\n",
	//			                  new File(doc.fileName).getName(), node.getSymbol(), matchingLeftToken, tokenAfterMatchingToken);
				if (tokenAfterMatchingToken != null)
				{
					if (tokenAfterMatchingToken.Type == TokenConstants.EOF)
					{
						return 1;
					}
					return tokenAfterMatchingToken.Line > matchingLeftToken.Line ? 1 : 0;
				}
			}
			return NOT_PAIR;
		}

		public static TerminalNode getMatchingLeftSymbol(Corpus corpus, InputDocument doc, TerminalNode node)
		{
			ParserRuleContext parent = (ParserRuleContext)node.Parent;
			int curTokensParentRuleIndex = parent.RuleIndex;
			Token curToken = node.Symbol;
			if (corpus.ruleToPairsBag != null)
			{
				string ruleName = doc.parser.RuleNames[curTokensParentRuleIndex];
				RuleAltKey ruleAltKey = new RuleAltKey(ruleName, parent.getAltNumber());
			    IList<org.antlr.codebuff.misc.Pair<int, int>> pairs = null;
                corpus.ruleToPairsBag.TryGetValue(ruleAltKey, out pairs);
				if (pairs != null)
				{
					// Find appropriate pair given current token
					// If more than one pair (a,b) with b=current token pick first one
					// or if a common pair like ({,}), then give that one preference.
					// or if b is punctuation, prefer a that is punct
					IList<int> viableMatchingLeftTokenTypes = viableLeftTokenTypes(parent, curToken, pairs);
					Vocabulary vocab = doc.parser.Vocabulary as Vocabulary;
					if (viableMatchingLeftTokenTypes.Count > 0)
					{
						int matchingLeftTokenType = CollectTokenPairs.getMatchingLeftTokenType(curToken, viableMatchingLeftTokenTypes, vocab);
						IList<TerminalNode> matchingLeftNodes = parent.GetTokens(matchingLeftTokenType);
						// get matching left node by getting last node to left of current token
						IList<TerminalNode> nodesToLeftOfCurrentToken = BuffUtils.filter(matchingLeftNodes, n => n.Symbol.TokenIndex < curToken.TokenIndex);
						TerminalNode matchingLeftNode = nodesToLeftOfCurrentToken[nodesToLeftOfCurrentToken.Count - 1];
						if (matchingLeftNode == null)
						{
							Console.Error.WriteLine("can't find matching node for " + node.Symbol);
						}
						return matchingLeftNode;
					}
				}
			}
			return null;
		}

		public static IList<int> viableLeftTokenTypes(ParserRuleContext node, Token curToken, IList<org.antlr.codebuff.misc.Pair<int, int>> pairs)
		{
			IList<int> newPairs = new List<int>();
			foreach (org.antlr.codebuff.misc.Pair<int, int> p in pairs)
			{
				if (p.b == curToken.Type && node.GetTokens(p.a).Any())
				{
					newPairs.Add(p.a);
				}
			}
			return newPairs;
		}

		/// <summary>
		/// Search backwards from tokIndex into 'tokens' stream and get all on-channel
		///  tokens on previous line with respect to token at tokIndex.
		///  return empty list if none found. First token in returned list is
		///  the first token on the line.
		/// </summary>
		public static IList<Token> getTokensOnPreviousLine(CommonTokenStream tokens, int tokIndex, int curLine)
		{
			// first find previous line by looking for real token on line < tokens.get(i)
			int prevLine = 0;
			for (int i = tokIndex - 1; i >= 0; i--)
			{
				Token t = tokens.Get(i);
				if (t.Channel == TokenConstants.DefaultChannel && t.Line < curLine)
				{
					prevLine = t.Line;
					tokIndex = i; // start collecting at this index
					break;
				}
			}

			// Now collect the on-channel real tokens for this line
			IList<Token> online = new List<Token>();
			for (int i = tokIndex; i >= 0; i--)
			{
				Token t = tokens.Get(i);
				if (t.Channel == TokenConstants.DefaultChannel)
				{
					if (t.Line < prevLine)
					{
						break; // found last token on that previous line
					}
					online.Add(t);
				}
			}
			online.Reverse();
			return online;
		}

		public static string _toString(FeatureMetaData[] FEATURES, InputDocument doc, int[] features)
		{
			return _toString(FEATURES, doc, features, true);
		}

	    public static string _toString(FeatureMetaData[] FEATURES, InputDocument doc, int[] features, bool showInfo)
	    {
	        Vocabulary v = doc.parser.Vocabulary as Vocabulary;
	        string[] ruleNames = doc.parser.RuleNames;
	        StringBuilder buf = new StringBuilder();
	        for (int i = 0; i < FEATURES.Length; i++)
	        {
	            if (FEATURES[i].type.Equals(FeatureType.UNUSED))
	            {
	                continue;
	            }
	            if (i > 0)
	            {
	                buf.Append(" ");
	            }
	            if (i == INDEX_CUR_TOKEN_TYPE)
	            {
	                buf.Append("| "); // separate prev from current tokens
	            }
	            int displayWidth = FEATURES[i].type.displayWidth;
	            if (FEATURES[i].type == FeatureType.TOKEN)
	            {
	                string tokenName = v.GetDisplayName(features[i]);
	                string abbrev = StringUtils.abbreviateMiddle(tokenName, "*", displayWidth);
	                string centered = StringUtils.center(abbrev, displayWidth);
//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
//ORIGINAL LINE: buf.append(String.format("%"+displayWidth+"s", centered));
	                buf.Append(string.Format("%" + displayWidth + "s", centered));
	            }
	            else if (FEATURES[i].type == FeatureType.RULE)
	            {
	                if (features[i] >= 0)
	                {
	                    string ruleName = ruleNames[unrulealt(features[i])[0]];
	                    int ruleAltNum = unrulealt(features[i])[1];
	                    ruleName += ":" + ruleAltNum;
	                    var abbrev = StringUtils.abbreviateMiddle(ruleName, "*", displayWidth);
	                    buf.Append(string.Format("%" + displayWidth + "s", abbrev));
	                }
	                else
	                {
	                    buf.Append(Tool.sequence(displayWidth, " "));
	                }
	            }
	            else if (FEATURES[i].type == FeatureType.INT ||
	                     FEATURES[i].type == FeatureType.INFO_LINE ||
	                     FEATURES[i].type == FeatureType.INFO_CHARPOS)
	            {
	                if (showInfo)
	                {
	                    if (features[i] >= 0)
	                    {
	                        buf.Append(string.Format("%" + displayWidth + "s",
	                            StringUtils.center(features[i].ToString(), displayWidth)));
	                    }
	                    else
	                    {
	                        buf.Append(Tool.sequence(displayWidth, " "));
	                    }
	                }
	            }
	            else if (FEATURES[i].type == FeatureType.INFO_FILE)
	            {
	                if (showInfo)
	                {
	                    string fname = System.IO.Path.GetFileName(doc.fileName);
	                    fname = StringUtils.abbreviate(fname, displayWidth);
	                    buf.Append(string.Format("%" + displayWidth + "s", fname));
	                }
	            }
	            else if (FEATURES[i].type == FeatureType.BOOL)
	            {
	                if (features[i] != -1)
	                {
	                    buf.Append(features[i] == 1 ? "true " : "false");
	                }
	                else
	                {
	                    buf.Append(Tool.sequence(displayWidth, " "));
	                }
	            }
	            else
	            {
	                Console.Error.WriteLine("NO STRING FOR FEATURE TYPE: " + FEATURES[i].type);
	                break;
	            }
	        }
	        return buf.ToString();
	    }

	    public static string _toFileInfoString(FeatureMetaData[] FEATURES, InputDocument doc, int[] features)
		{
			StringBuilder buf = new StringBuilder();
			for (int i = 0; i < FEATURES.Length; i++)
			{
				if (FEATURES[i].type != FeatureType.INFO_FILE && FEATURES[i].type != FeatureType.INFO_LINE && FEATURES[i].type != FeatureType.INFO_CHARPOS)
				{
					continue;
				}
				if (i > 0)
				{
					buf.Append(" ");
				}
				int displayWidth = FEATURES[i].type.displayWidth;
			    if (FEATURES[i].type == FeatureType.INFO_LINE ||
			        FEATURES[i].type == FeatureType.INFO_CHARPOS)
			    {
			        if (features[i] >= 0)
			        {
			            buf.Append(string.Format("%" + displayWidth + "s",
			                StringUtils.center(features[i].ToString(), displayWidth)));
			        }
			        else
			        {
			            buf.Append(Tool.sequence(displayWidth, " "));
			        }
			    } else if (FEATURES[i].type == FeatureType.INFO_FILE)
			    {
			        string fname = System.IO.Path.GetFileName(doc.fileName);
			        fname = StringUtils.abbreviate(fname, displayWidth);
			        buf.Append(string.Format("%" + displayWidth + "s", fname));
			    }
			    else
			    {
			        Console.Error.WriteLine("NO STRING FOR FEATURE TYPE: " + FEATURES[i].type);
			    }
			}
			return buf.ToString();
		}

		public static string featureNameHeader(FeatureMetaData[] FEATURES)
		{
			StringBuilder buf = new StringBuilder();
			for (int i = 0; i < FEATURES.Length; i++)
			{
				if (FEATURES[i].type.Equals(FeatureType.UNUSED))
				{
					continue;
				}
				if (i > 0)
				{
					buf.Append(" ");
				}
				if (i == INDEX_CUR_TOKEN_TYPE)
				{
					buf.Append("| "); // separate prev from current tokens
				}
				int displayWidth = FEATURES[i].type.displayWidth;
				buf.Append(StringUtils.center(FEATURES[i].abbrevHeaderRows[0], displayWidth));
			}
			buf.Append("\n");
			for (int i = 0; i < FEATURES.Length; i++)
			{
				if (FEATURES[i].type.Equals(FeatureType.UNUSED))
				{
					continue;
				}
				if (i > 0)
				{
					buf.Append(" ");
				}
				if (i == INDEX_CUR_TOKEN_TYPE)
				{
					buf.Append("| "); // separate prev from current tokens
				}
				int displayWidth = FEATURES[i].type.displayWidth;
				buf.Append(StringUtils.center(FEATURES[i].abbrevHeaderRows[1], displayWidth));
			}
			buf.Append("\n");
			for (int i = 0; i < FEATURES.Length; i++)
			{
				if (FEATURES[i].type.Equals(FeatureType.UNUSED))
				{
					continue;
				}
				if (i > 0)
				{
					buf.Append(" ");
				}
				if (i == INDEX_CUR_TOKEN_TYPE)
				{
					buf.Append("| "); // separate prev from current tokens
				}
				int displayWidth = FEATURES[i].type.displayWidth;
				buf.Append(StringUtils.center("(" + ((int)FEATURES[i].mismatchCost) + ")", displayWidth));
			}
			buf.Append("\n");
			for (int i = 0; i < FEATURES.Length; i++)
			{
				if (FEATURES[i].type.Equals(FeatureType.UNUSED))
				{
					continue;
				}
				if (i > 0)
				{
					buf.Append(" ");
				}
				if (i == INDEX_CUR_TOKEN_TYPE)
				{
					buf.Append("| "); // separate prev from current tokens
				}
				int displayWidth = FEATURES[i].type.displayWidth;
				buf.Append(Tool.sequence(displayWidth, "="));
			}
			buf.Append("\n");
			return buf.ToString();
		}

		/// <summary>
		/// Make an index for fast lookup from Token to tree leaf </summary>
		public static IDictionary<Token, TerminalNode> indexTree(ParserRuleContext root)
		{
			IDictionary<Token, TerminalNode> tokenToNodeMap = new Dictionary<Token, TerminalNode>();
			ParseTreeWalker.Default.Walk(new ParseTreeListenerAnonymousInnerClass(tokenToNodeMap), root);
			return tokenToNodeMap;
		}

		private class ParseTreeListenerAnonymousInnerClass : ParseTreeListener
		{
			private IDictionary<Token, TerminalNode> tokenToNodeMap;

			public ParseTreeListenerAnonymousInnerClass(IDictionary<Token, TerminalNode> tokenToNodeMap)
			{
				this.tokenToNodeMap = tokenToNodeMap;
			}

			public void VisitTerminal(TerminalNode node)
			{
				Token curToken = node.Symbol;
				tokenToNodeMap[curToken] = node;
			}

			public void VisitErrorNode(ErrorNode node)
			{
			}

			public void EnterEveryRule(ParserRuleContext ctx)
			{
			}

			public void ExitEveryRule(ParserRuleContext ctx)
			{
			}
		}

		public static IList<Token> getRealTokens(CommonTokenStream tokens)
		{
			IList<Token> real = new List<Token>();
			for (int i = 0; i < tokens.Size; i++)
			{
				Token t = tokens.Get(i);
				if (t.Type != TokenConstants.EOF && t.Channel == Lexer.DefaultTokenChannel)
				{
					real.Add(t);
				}
			}
			return real;
		}

		/// <summary>
		/// Return the index 0..n-1 of t as child of t.parent.
		///  If t is index 0, always return 0.
		///  If t is a repeated subtree root and index within
		///  sibling list > 0, return CHILD_INDEX_LIST_ELEMENT.
		///  In all other cases, return the actual index of t. That means for a
		///  sibling list starting at child index 5, the first sibling will return
		///  5 but 2nd and beyond in list will return CHILD_INDEX_LIST_ELEMENT.
		/// </summary>
		public static int getChildIndexOrListMembership(ParseTree t)
		{
			if (t == null)
			{
				return -1;
			}
			ParseTree parent = t.Parent;
			if (parent == null)
			{
				return -1;
			}
			// we know we have a parent now
			// check to see if we are 2nd or beyond element in a sibling list
			if (t is ParserRuleContext)
			{
			    var s = ((ParserRuleContext) parent).GetRuleContexts<ParserRuleContext>();
			    IList<ParserRuleContext> siblings = s.Where((n) => n.GetType() == t.GetType()).ToList();

				//IList<ParserRuleContext> siblings = ((ParserRuleContext)parent).GetRuleContexts<ParserRuleContext>(((ParserRuleContext)t).GetType());
				if (siblings.Count > 1 && siblings.IndexOf(t as ParserRuleContext) > 0)
				{
					return CHILD_INDEX_REPEATED_ELEMENT;
				}
			}
			// check to see if we are 2nd or beyond repeated token
			if (t is TerminalNode)
			{
				IList<TerminalNode> repeatedTokens = ((ParserRuleContext) parent).GetTokens(((TerminalNode) t).Symbol.Type);
				if (repeatedTokens.Count > 1 && repeatedTokens.IndexOf(t as TerminalNode) > 0)
				{
					return CHILD_INDEX_REPEATED_ELEMENT;
				}
			}

			return getChildIndex(t);
		}

		public static int getChildIndex(ParseTree t)
		{
			if (t == null)
			{
				return -1;
			}
			ParseTree parent = t.Parent;
			if (parent == null)
			{
				return -1;
			}
			// Figure out which child index t is of parent
			for (int i = 0; i < parent.ChildCount; i++)
			{
				if (parent.GetChild(i) == t)
				{
					return i;
				}
			}
			return -1;
		}

		public static int rulealt(ParserRuleContext r)
		{
			return rulealt(r.RuleIndex, r.getAltNumber());
		}

		/// <summary>
		/// Pack a rule index and an alternative number into the same 32-bit integer. </summary>
		public static int rulealt(int rule, int alt)
		{
			if (rule == -1)
			{
				return -1;
			}
			return rule << 16 | alt;
		}

		/// <summary>
		/// Return {rule index, rule alt number} </summary>
		public static int[] unrulealt(int ra)
		{
			if (ra == -1)
			{
				return new int[] {-1, ATN.INVALID_ALT_NUMBER};
			}
			return new int[] {(ra >> 16) & 0xFFFF,ra & 0xFFFF};
		}

		public static int indentcat(int deltaFromLeftAncestor, int child)
		{
			return CAT_INDENT_FROM_ANCESTOR_CHILD | (deltaFromLeftAncestor << 8) | (child << 16);
		}

		public static int[] unindentcat(int v)
		{
			int deltaFromLeftAncestor = (v >> 8) & 0xFF;
			int child = (v >> 16) & 0xFFFF;
			return new int[] {deltaFromLeftAncestor, child};
		}

		public static int aligncat(int deltaFromLeftAncestor, int child)
		{
			return CAT_ALIGN_WITH_ANCESTOR_CHILD | (deltaFromLeftAncestor << 8) | (child << 16);
		}

		public static int[] triple(int v)
		{
			int deltaFromLeftAncestor = (v >> 8) & 0xFF;
			int child = (v >> 16) & 0xFFFF;
			return new int[] {deltaFromLeftAncestor, child};
		}

		public static int wscat(int n)
		{
			return CAT_INJECT_WS | (n << 8);
		}

		public static int nlcat(int n)
		{
			return CAT_INJECT_NL | (n << 8);
		}

		public static int unwscat(int v)
		{
			return v >> 8 & 0xFFFF;
		}

		public static int unnlcat(int v)
		{
			return v >> 8 & 0xFFFF;
		}

		// '\n' (before list, before sep, after sep, after last element)
		public static int listform(int[] ws)
		{
			bool[] nl = new bool[] {ws[0] == '\n', ws[1] == '\n', ws[2] == '\n', ws[3] == '\n'};
			return (nl[0]?0x01000000:0) | (nl[1]?0x00010000:0) | (nl[2]?0x00000100:0) | (nl[3]?0x00000001:0);
		}

		public static int[] unlistform(int v)
		{
			return new int[] {v >> 24 & 0xFF, v >> 16 & 0xFF, v >> 8 & 0xFF, v & 0xFF};
		}
	}

}