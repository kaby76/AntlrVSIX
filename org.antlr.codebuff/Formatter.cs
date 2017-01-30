using System;
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
	using TokenPositionAnalysis = org.antlr.codebuff.validation.TokenPositionAnalysis;
	using IdentifyOversizeLists = org.antlr.codebuff.walkers.IdentifyOversizeLists;
	using CommonToken = Antlr4.Runtime.CommonToken;
	using CommonTokenStream = Antlr4.Runtime.CommonTokenStream;
	using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;
	using Token = Antlr4.Runtime.IToken;
	using WritableToken = Antlr4.Runtime.IWritableToken;
	using Interval = Antlr4.Runtime.Misc.Interval;
	//using Pair = org.antlr.v4.runtime.misc.Pair;
	using ParseTree = Antlr4.Runtime.Tree.IParseTree;
	using ParseTreeWalker = Antlr4.Runtime.Tree.ParseTreeWalker;
	using TerminalNode = Antlr4.Runtime.Tree.ITerminalNode;

	public class Formatter
	{
		public const int DEFAULT_K = 11;

		public Corpus corpus;

		public StringBuilder output;
		public CodeBuffTokenStream originalTokens; // copy of tokens with line/col info
		public IList<Token> realTokens; // just the real tokens from tokens

		/// <summary>
		/// A map from real token to node in the parse tree </summary>
		public IDictionary<Token, TerminalNode> tokenToNodeMap = null;
		public IDictionary<Token, TerminalNode> originalTokenToNodeMap = null; // for debugging purposes only

		/// <summary>
		/// analysis[i] is info about what we decided for token index i from
		///  original stream (not index into real token list)
		/// </summary>
		public List<TokenPositionAnalysis> analysis;

		/// <summary>
		/// Collected for formatting (not training) by SplitOversizeLists.
		///  Training finds split lists and normal lists. This uses list len
		///  (in char units) to decide split or not.  If size is closest to split median,
		///  we claim oversize list.
		/// </summary>
		public IDictionary<Token, org.antlr.codebuff.misc.Pair<bool, int>> tokenToListInfo;

		public kNNClassifier wsClassifier;
		public kNNClassifier hposClassifier;
		public int k;
		public FeatureMetaData[] wsFeatures = Trainer.FEATURES_INJECT_WS;
		public FeatureMetaData[] hposFeatures = Trainer.FEATURES_HPOS;

		public InputDocument originalDoc; // used only for debugging
		public InputDocument testDoc;

		public int indentSize; // size in spaces of an indentation

		public int line = 1;
		public int charPosInLine = 0;

		public Formatter(Corpus corpus, int indentSize, int k, FeatureMetaData[] wsFeatures, FeatureMetaData[] hposFeatures) : this(corpus, indentSize)
		{
			this.k = k;
			this.wsFeatures = wsFeatures;
			this.hposFeatures = hposFeatures;
		}

		public Formatter(Corpus corpus, int indentSize)
		{
			this.corpus = corpus;
	//		k = (int)Math.sqrt(corpus.X.size());
			k = DEFAULT_K;
			this.indentSize = indentSize;
		}

		public virtual string Output
		{
			get
			{
				return output.ToString();
			}
		}

		public virtual IList<TokenPositionAnalysis> AnalysisPerToken
		{
			get
			{
				return analysis;
			}
		}

		/// <summary>
		/// Free anything we can to reduce memory footprint after a format().
		///  keep analysis, testDoc as they are used for results.
		/// </summary>
		public virtual void releaseMemory()
		{
			corpus = null;
			realTokens = null;
			originalTokens = null;
			tokenToNodeMap = null;
			originalTokenToNodeMap = null;
			tokenToListInfo = null;
			wsClassifier = null;
			hposClassifier = null;
		}

		/// <summary>
		/// Format the document. Does not affect/alter doc. </summary>
		public virtual string format(InputDocument doc, bool collectAnalysis)
		{
			if (testDoc != null)
			{
				throw new System.ArgumentException("can't call format > once");
			}
			// for debugging we need a map from original token with actual line:col to tree node. used by token analysis
			originalDoc = doc;
			originalTokenToNodeMap = Trainer.indexTree(doc.tree);
			originalTokens = doc.tokens;

			this.testDoc = InputDocument.dup(doc); // make copy of doc, getting new tokens, tree
			output = new StringBuilder();
			this.realTokens = Trainer.getRealTokens(testDoc.tokens);
			// squeeze out ws and kill any line/col info so we can't use ground truth by mistake
			wipeCharPositionInfoAndWhitespaceTokens(testDoc.tokens); // all except for first token
			wsClassifier = new kNNClassifier(corpus, wsFeatures, corpus.injectWhitespace);
			hposClassifier = new kNNClassifier(corpus, hposFeatures, corpus.hpos);
		    
			analysis = new ArrayList<TokenPositionAnalysis>(testDoc.tokens.Size);
		    for (int i = 0; i < testDoc.tokens.Size; ++i) analysis.Add(null);

			// make an index on the duplicated doc tree with tokens missing line:col info
			if (tokenToNodeMap == null)
			{
				tokenToNodeMap = Trainer.indexTree(testDoc.tree);
			}

			IToken firstToken = testDoc.tokens.getNextRealToken(-1);
            
			string prefix = originalTokens.GetText(Interval.Of(0, firstToken.TokenIndex)); // gets any comments in front + first real token
		    charPosInLine = firstToken.Column + firstToken.Text.Length + 1; // start where first token left off
			line = Tool.count(prefix, '\n') + 1;
			output.Append(prefix);

			// first identify oversize lists with separators
			IdentifyOversizeLists splitter = new IdentifyOversizeLists(corpus, testDoc.tokens, tokenToNodeMap);
			ParseTreeWalker.Default.Walk(splitter, testDoc.tree);
			tokenToListInfo = splitter.tokenToListInfo;

			realTokens = Trainer.getRealTokens(testDoc.tokens);
			for (int i = Trainer.ANALYSIS_START_TOKEN_INDEX; i < realTokens.Count; i++)
			{ // can't process first token
				int tokenIndexInStream = realTokens[i].TokenIndex;
				processToken(i, tokenIndexInStream, collectAnalysis);
			}

			releaseMemory();

			return output.ToString();
		}

		public virtual float WSEditDistance
		{
			get
			{
			    Regex rex = new Regex("^\\s+$");
				IList<Token> wsTokens = BuffUtils.filter(originalTokens.GetTokens(), t => rex.IsMatch(t.Text)); // only count whitespace
				string originalWS = Dbg.tokenText(wsTokens);
    
				string formattedOutput = Output;
				CommonTokenStream formatted_tokens = Tool.tokenize(formattedOutput, corpus.language.lexerClass);
				wsTokens = BuffUtils.filter(formatted_tokens.GetTokens(), t => rex.IsMatch(t.Text));
				string formattedWS = Dbg.tokenText(wsTokens);
    
				float editDistance = Dbg.normalizedLevenshteinDistance(originalWS, formattedWS);
				return editDistance;
			}
		}

		public virtual void processToken(int indexIntoRealTokens, int tokenIndexInStream, bool collectAnalysis)
		{
			CommonToken curToken = (CommonToken)testDoc.tokens.Get(tokenIndexInStream);
			string tokText = curToken.Text;
			TerminalNode node = tokenToNodeMap[curToken];

			int[] features = getFeatures(testDoc, tokenIndexInStream);
			int[] featuresForAlign = new int[features.Length];
			Array.Copy(features, 0, featuresForAlign, 0, features.Length);

			int injectNL_WS = wsClassifier.classify(k, features, Trainer.MAX_WS_CONTEXT_DIFF_THRESHOLD);

			injectNL_WS = emitCommentsToTheLeft(tokenIndexInStream, injectNL_WS);

			int newlines = 0;
			int ws = 0;
			if ((injectNL_WS & 0xFF) == Trainer.CAT_INJECT_NL)
			{
				newlines = Trainer.unnlcat(injectNL_WS);
			}
			else if ((injectNL_WS & 0xFF) == Trainer.CAT_INJECT_WS)
			{
				ws = Trainer.unwscat(injectNL_WS);
			}

			if (newlines == 0 && ws == 0 && cannotJoin(realTokens[indexIntoRealTokens - 1], curToken))
			{ // failsafe!
				ws = 1;
			}

			int alignOrIndent = Trainer.CAT_ALIGN;

			if (newlines > 0)
			{
				output.Append(Tool.newlines(newlines));
				line += newlines;
				charPosInLine = 0;

				// getFeatures() doesn't know what line curToken is on. If \n, we need to find exemplars that start a line
				featuresForAlign[Trainer.INDEX_FIRST_ON_LINE] = 1; // use \n prediction to match exemplars for alignment

				alignOrIndent = hposClassifier.classify(k, featuresForAlign, Trainer.MAX_ALIGN_CONTEXT_DIFF_THRESHOLD);

				if ((alignOrIndent & 0xFF) == Trainer.CAT_ALIGN_WITH_ANCESTOR_CHILD)
				{
					align(alignOrIndent, node);
				}
				else if ((alignOrIndent & 0xFF) == Trainer.CAT_INDENT_FROM_ANCESTOR_CHILD)
				{
					indent(alignOrIndent, node);
				}
				else if ((alignOrIndent & 0xFF) == Trainer.CAT_ALIGN)
				{
					IList<Token> tokensOnPreviousLine = Trainer.getTokensOnPreviousLine(testDoc.tokens, tokenIndexInStream, line);
					if (tokensOnPreviousLine.Count > 0)
					{
						Token firstTokenOnPrevLine = tokensOnPreviousLine[0];
						int indentCol = firstTokenOnPrevLine.Column;
						charPosInLine = indentCol;
						output.Append(Tool.spaces(indentCol));
					}
				}
				else if ((alignOrIndent & 0xFF) == Trainer.CAT_INDENT)
				{
					indent(alignOrIndent, node);
				}
			}
			else
			{
				// inject whitespace instead of \n?
				output.Append(Tool.spaces(ws));
				charPosInLine += ws;
			}

			// update Token object with position information now that we are about
			// to emit it.
			curToken.Line = line;
			curToken.Column = charPosInLine;

			TokenPositionAnalysis tokenPositionAnalysis = getTokenAnalysis(features, featuresForAlign, tokenIndexInStream, injectNL_WS, alignOrIndent, collectAnalysis);

			analysis[tokenIndexInStream] = tokenPositionAnalysis;

			int n = tokText.Length;
			tokenPositionAnalysis.charIndexStart = output.Length;
			tokenPositionAnalysis.charIndexStop = tokenPositionAnalysis.charIndexStart + n - 1;

			// emit
			output.Append(tokText);
			charPosInLine += n;
		}

		public virtual void indent(int indentCat, TerminalNode node)
		{
			int tokenIndexInStream = node.Symbol.TokenIndex;
			IList<Token> tokensOnPreviousLine = Trainer.getTokensOnPreviousLine(testDoc.tokens, tokenIndexInStream, line);
			Token firstTokenOnPrevLine = null;
			if (tokensOnPreviousLine.Count > 0)
			{
				firstTokenOnPrevLine = tokensOnPreviousLine[0];
			}

			if (indentCat == Trainer.CAT_INDENT)
			{
				if (firstTokenOnPrevLine != null)
				{ // if not on first line, we cannot indent
					int indentedCol = firstTokenOnPrevLine.Column + indentSize;
					charPosInLine = indentedCol;
					output.Append(Tool.spaces(indentedCol));
				}
				else
				{
					// no prev token? ok, just indent from left edge
					charPosInLine = indentSize;
					output.Append(Tool.spaces(indentSize));
				}
				return;
			}
			int[] deltaChild = Trainer.unindentcat(indentCat);
			int deltaFromAncestor = deltaChild[0];
			int childIndex = deltaChild[1];
			ParserRuleContext earliestLeftAncestor = Trainer.earliestAncestorStartingWithToken(node);
			ParserRuleContext ancestor = Trainer.getAncestor(earliestLeftAncestor, deltaFromAncestor);
			Token start = null;
			if (ancestor == null)
			{
	//			System.err.println("Whoops. No ancestor at that delta");
			}
			else
			{
				ParseTree child = ancestor.GetChild(childIndex);
				if (child is ParserRuleContext)
				{
					start = ((ParserRuleContext) child).Start;
				}
				else if (child is TerminalNode)
				{
					start = ((TerminalNode) child).Symbol;
				}
				else
				{
					// uh oh.
	//				System.err.println("Whoops. Tried to access invalid child");
				}
			}
			if (start != null)
			{
				int indentCol = start.Column + indentSize;
				charPosInLine = indentCol;
				output.Append(Tool.spaces(indentCol));
			}
		}

		public virtual void align(int alignOrIndent, TerminalNode node)
		{
			int[] deltaChild = Trainer.triple(alignOrIndent);
			int deltaFromAncestor = deltaChild[0];
			int childIndex = deltaChild[1];
			ParserRuleContext earliestLeftAncestor = Trainer.earliestAncestorStartingWithToken(node);
			ParserRuleContext ancestor = Trainer.getAncestor(earliestLeftAncestor, deltaFromAncestor);
			Token start = null;
			if (ancestor == null)
			{
	//			System.err.println("Whoops. No ancestor at that delta");
			}
			else
			{
				ParseTree child = ancestor.GetChild(childIndex);
				if (child is ParserRuleContext)
				{
					start = ((ParserRuleContext) child).Start;
				}
				else if (child is TerminalNode)
				{
					start = ((TerminalNode) child).Symbol;
				}
				else
				{
					// uh oh.
	//				System.err.println("Whoops. Tried to access invalid child");
				}
			}
			if (start != null)
			{
				int indentCol = start.Column;
				charPosInLine = indentCol;
				output.Append(Tool.spaces(indentCol));
			}
		}

		public virtual int[] getFeatures(InputDocument doc, int tokenIndexInStream)
		{
			Token prevToken = doc.tokens.getPreviousRealToken(tokenIndexInStream);
			Token prevPrevToken = prevToken != null ? doc.tokens.getPreviousRealToken(prevToken.TokenIndex) : null;
			bool prevTokenStartsLine = false;
			if (prevToken != null && prevPrevToken != null)
			{
				prevTokenStartsLine = prevToken.Line > prevPrevToken.Line;
			}
			TerminalNode node = tokenToNodeMap[doc.tokens.Get(tokenIndexInStream)];
			if (node == null)
			{
				Console.Error.WriteLine("### No node associated with token " + doc.tokens.Get(tokenIndexInStream));
				return null;
			}

			Token curToken = node.Symbol;

			bool curTokenStartsNewLine = false;
			if (prevToken == null)
			{
				curTokenStartsNewLine = true; // we must be at start of file
			}
			else if (line > prevToken.Line)
			{
				curTokenStartsNewLine = true;
			}

			int[] features = Trainer.getContextFeatures(corpus, tokenToNodeMap, doc, tokenIndexInStream);

            Trainer.setListInfoFeatures(tokenToListInfo, features, curToken);

			features[Trainer.INDEX_PREV_FIRST_ON_LINE] = prevTokenStartsLine ? 1 : 0;
			features[Trainer.INDEX_FIRST_ON_LINE] = curTokenStartsNewLine ? 1 : 0;

			return features;
		}

		/// <summary>
		/// Look into the originalTokens stream to get the comments to the left of current
		///  token. Emit all whitespace and comments except for whitespace at the
		///  end as we'll inject that per newline prediction.
		/// 
		///  We able to see original input stream for comment purposes only. With all
		///  whitespace removed, we can't emit this stuff properly. This
		///  is the only place that examines the original token stream during formatting.
		/// </summary>
		public virtual int emitCommentsToTheLeft(int tokenIndexInStream, int injectNL_WS)
		{
			IList<Token> hiddenTokensToLeft = originalTokens.GetHiddenTokensToLeft(tokenIndexInStream);
			if (hiddenTokensToLeft != null)
			{
				// if at least one is not whitespace, assume it's a comment and print all hidden stuff including whitespace
				bool hasComment = Trainer.hasCommentToken(hiddenTokensToLeft);
				if (hasComment)
				{
					// avoid whitespace at end of sequence as we'll inject that
					int last = -1;
					for (int i = hiddenTokensToLeft.Count - 1; i >= 0; i--)
					{
						Token hidden = hiddenTokensToLeft[i];
						string hiddenText = hidden.Text;
					    Regex rex = new Regex("^\\s+$");
						if (!rex.IsMatch(hiddenText))
						{
							last = i;
							break;
						}
					}
					Token commentToken = hiddenTokensToLeft[last];
					IList<Token> truncated = hiddenTokensToLeft.Take(last + 1).ToList();
					foreach (Token hidden in truncated)
					{
						string hiddenText = hidden.Text;
						output.Append(hiddenText);
					    Regex rex = new Regex("^\\n+$"); // KED SUSPECT THIS MAY BE WRONG FOR WINDOWS (\n\r).
						if (rex.IsMatch(hiddenText))
						{
							line += Tool.count(hiddenText, '\n');
							charPosInLine = 0;
						}
						else
						{
							// if a comment or plain ' ', must count char position
							charPosInLine += hiddenText.Length;
						}
					}
					// failsafe. make sure single-line comments have \n on the end.
					// If not predicted, must override and inject one
					if (commentToken.Type == corpus.language.singleLineCommentType && (injectNL_WS & 0xFF) != Trainer.CAT_INJECT_NL)
					{
						return Trainer.nlcat(1); // force formatter to predict newline then trigger alignment
					}
				}
			}

			return injectNL_WS; // send same thing back out unless we trigger failsafe
		}

		public virtual TokenPositionAnalysis getTokenAnalysis(int[] features, int[] featuresForAlign, int tokenIndexInStream, int injectNL_WS, int alignOrIndent, bool collectAnalysis)
		{
			CommonToken curToken = (CommonToken)originalDoc.tokens.Get(tokenIndexInStream);
			TerminalNode nodeWithOriginalToken = originalTokenToNodeMap[curToken];

			int actualWS = Trainer.getInjectWSCategory(originalTokens, tokenIndexInStream);
			string actualWSNL = getWSCategoryStr(actualWS);
			actualWSNL = !string.ReferenceEquals(actualWSNL, null) ? actualWSNL : string.Format("{0,8}","none");

			string wsDisplay = getWSCategoryStr(injectNL_WS);
			if (string.ReferenceEquals(wsDisplay, null))
			{
				wsDisplay = string.Format("{0,8}","none");
			}
			string alignDisplay = getHPosCategoryStr(alignOrIndent);
			if (string.ReferenceEquals(alignDisplay, null))
			{
				alignDisplay = string.Format("{0,8}","none");
			}
			string newlinePredictionString = string.Format("### line {0:D}: predicted {1} actual {2}", curToken.Line, wsDisplay, actualWSNL);

			int actualAlignCategory = Trainer.getAlignmentCategory(originalDoc, nodeWithOriginalToken, indentSize);
			string actualAlignDisplay = getHPosCategoryStr(actualAlignCategory);
			actualAlignDisplay = !string.ReferenceEquals(actualAlignDisplay, null) ? actualAlignDisplay : string.Format("{0,8}","none");

			string alignPredictionString = string.Format("### line {0:D}: predicted {1} actual {2}", curToken.Line, alignDisplay, actualAlignDisplay);

			string newlineAnalysis = "";
			string alignAnalysis = "";
			if (collectAnalysis)
			{ // this can be slow
				newlineAnalysis = newlinePredictionString + "\n" + wsClassifier.getPredictionAnalysis(testDoc, k, features, corpus.injectWhitespace, Trainer.MAX_WS_CONTEXT_DIFF_THRESHOLD);
				if ((injectNL_WS & 0xFF) == Trainer.CAT_INJECT_NL)
				{
					alignAnalysis = alignPredictionString + "\n" + hposClassifier.getPredictionAnalysis(testDoc, k, featuresForAlign, corpus.hpos, Trainer.MAX_ALIGN_CONTEXT_DIFF_THRESHOLD);
				}
			}
			TokenPositionAnalysis a = new TokenPositionAnalysis(curToken, injectNL_WS, newlineAnalysis, alignOrIndent, alignAnalysis);
			a.actualWS = Trainer.getInjectWSCategory(originalTokens, tokenIndexInStream);
			a.actualAlign = actualAlignCategory;
			return a;
		}

		public static string getWSCategoryStr(int injectNL_WS)
		{
			int[] elements = Trainer.triple(injectNL_WS);
			int cat = injectNL_WS & 0xFF;
			string catS = "none";
			if (cat == Trainer.CAT_INJECT_NL)
			{
				catS = "'\\n'";
			}
			else if (cat == Trainer.CAT_INJECT_WS)
			{
				catS = "' '";
			}
			else
			{
				return null;
			}
			return string.Format("{0,4}|{1:D}|{2:D}", catS, elements[0], elements[1]);
		}

		public static string getHPosCategoryStr(int alignOrIndent)
		{
			int[] elements = Trainer.triple(alignOrIndent);
			int cat = alignOrIndent & 0xFF;
			string catS = "align";
			if (cat == Trainer.CAT_ALIGN_WITH_ANCESTOR_CHILD)
			{
				catS = "align^";
			}
			else if (cat == Trainer.CAT_INDENT_FROM_ANCESTOR_CHILD)
			{
				catS = "indent^";
			}
			else if (cat == Trainer.CAT_ALIGN)
			{
				catS = "align";
			}
			else if (cat == Trainer.CAT_INDENT)
			{
				catS = "indent";
			}
			else
			{
				return null;
			}
			return string.Format("{0,7}|{1:D}|{2:D}", catS, elements[0], elements[1]);
		}

		/// <summary>
		/// Do not join two words like "finaldouble" or numbers like "3double",
		///  "double3", "34", (3 and 4 are different tokens) etc...
		/// </summary>
		public static bool cannotJoin(Token prevToken, Token curToken)
		{
			string prevTokenText = prevToken.Text;
			char prevLastChar = prevTokenText[prevTokenText.Length - 1];
			string curTokenText = curToken.Text;
			char curFirstChar = curTokenText[0];
			return char.IsLetterOrDigit(prevLastChar) && char.IsLetterOrDigit(curFirstChar);
		}

		public static void wipeCharPositionInfoAndWhitespaceTokens(CodeBuffTokenStream tokens)
		{
			tokens.Fill();
			CommonToken dummy = new CommonToken(TokenConstants.InvalidType, "");
			dummy.Channel = TokenConstants.HiddenChannel;
			Token firstRealToken = tokens.getNextRealToken(-1);
			for (int i = 0; i < tokens.Size; i++)
			{
				if (i == firstRealToken.TokenIndex)
				{
					continue; // don't wack first token
				}
				CommonToken t = (CommonToken)tokens.Get(i);
			    Regex rex = new Regex("^\\s+$");
				if (rex.IsMatch(t.Text))
				{
                    tokens.GetTokens()[i] = dummy;
                    // wack whitespace token so we can't use it during prediction
				}
				else
				{
					t.Line = 0;
					t.Column = -1;
				}
			}
		}

	}

}