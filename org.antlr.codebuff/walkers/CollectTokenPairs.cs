using System.Collections.Generic;
using Antlr4.Runtime.Misc;

namespace org.antlr.codebuff.walkers
{

	using BuffUtils = org.antlr.codebuff.misc.BuffUtils;
	using RuleAltKey = org.antlr.codebuff.misc.RuleAltKey;
	using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;
	using Token = Antlr4.Runtime.IToken;
	using Vocabulary = Antlr4.Runtime.Vocabulary;
	//using Pair = Antlr4.Runtime.Misc.Pair;
	using ErrorNode = Antlr4.Runtime.Tree.IErrorNode;
	using ParseTree = Antlr4.Runtime.Tree.IParseTree;
	using ParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
	using TerminalNode = Antlr4.Runtime.Tree.ITerminalNode;


	/// <summary>
	/// Identify token dependencies like { }, [], etc... for use when
	///  computing features.  For example, if there is no newline after '{'
	///  in a method, probably shouldn't have a newline before the '}'. We need
	///  to find matching symbols in order to synchronize the newline generation.
	/// 
	/// RAW NOTES:
	/// 
	/// Sample counts of rule nodes and unique token lists:
	/// 
	/// 80 block: ['{', '}']
	/// 197 blockStatement:
	/// 3 classBody: ['{', '}']
	/// 52 classBodyDeclaration:
	/// 5 typeArguments: ['<', ',', '>']
	/// 9 typeArguments: ['<', '>']
	/// 14 qualifiedName: [Identifier, '.', Identifier, '.', Identifier]
	/// 6 qualifiedName: [Identifier]
	/// 1 typeDeclaration:
	/// 8 typeSpec: ['[', ']']
	/// 173 typeSpec:
	/// 40 parExpression: ['(', ')']
	/// 1 classDeclaration: ['class', Identifier, 'extends']
	/// 2 classDeclaration: ['class', Identifier]
	/// 1 enumDeclaration: ['enum', Identifier, '{', '}']
	/// 1 expression: ['!']
	/// 9 expression: ['!=']
	/// 2 expression: ['&&']
	/// 109 expression: ['(', ')']
	/// 1 forControl: [';', ';']
	/// 3 forControl:
	/// ...
	/// 
	/// Repeated tokens should not be counted as most likely they are separators or terminators
	/// but not always. E.g., forControl: [';', ';']
	/// 
	/// Build r->(a,b) tuples for every a,b in tokens list for r and unique a,b.
	/// E.g., typeSpec: ['[', ']']
	/// 
	/// gives
	/// 
	/// typeSpec -> ('[',']')
	/// 
	/// For any a=b, assume it's not a dependency so build set r->{a} for each rule r.
	/// E.g., qualifiedName: [Identifier, '.', Identifier, '.', Identifier]
	/// 
	/// gives
	/// 
	/// qualifiedName -> (Id,.), (Id,Id), (.,Id)
	/// qualifiedName -> {Id,.}
	/// 
	/// meaning we disregard all tuples for qualifiedName.
	/// 
	/// With too few samples, we might get confused. E.g.,
	/// 
	/// 5 typeArguments: ['<', ',', '>']
	/// 9 typeArguments: ['<', '>']
	/// 
	/// gives
	/// 
	/// typeArguments -> (<,','), (<,>), (',',>)
	/// 
	/// Hmm..the count is 9+5 for (<,>), but just 5 for (<,',') and (',',>).
	/// If we pick just one tuple for each rule, we get (<,>) as winner. cool.
	/// By that argument, these would yield (class,Id) which is probably ok.
	/// 
	/// 1 classDeclaration: ['class', Identifier, 'extends']
	/// 2 classDeclaration: ['class', Identifier]
	/// 
	/// If there are multiple unique tokens every time like:
	/// 
	/// enumDeclaration: ['enum', Identifier, '{', '}']
	/// 
	/// we can't decide which token to pair with which. For now we can choose
	/// arbitrarily but later maybe choose first to last token.
	/// 
	/// Oh! Actually, we should only consider tokens that are literals. Tokens
	/// like Id won't be that useful. That would give
	/// 
	/// enumDeclaration: ['enum', '{', '}']
	/// 
	/// which is easier.
	/// 
	/// Can't really choose most frequent all the time. E.g., statement yields:
	/// 
	/// statement: 11:'if','else' 11:'throw',';' 4:'for','(' 4:'for',')' 35:'return',';' 4:'(',')'
	/// 
	/// and then would pick ('return',';') as the dependency. Ah. We need pairs
	/// not by rule but rule and which alternative. Otherwise rules with lots of
	/// alts will not be able to pick stuff out. Well, that info isn't available
	/// except for interpreted parsing. dang. I would have to subclass the
	/// ATN simulator to create different context objects. Doable but ignore for now.
	/// 
	/// For matching symbols like {}, [], let's use that info to get a unique match
	/// when this algorithm doesn't give one.
	/// </summary>
	public class CollectTokenPairs : ParseTreeListener
	{
		/// <summary>
		/// a bit of "overfitting" or tailoring to the most common pairs in all
		///  computer languages to improve accuracy when choosing pairs.
		///  I.e., it never makes sense to choose ('@',')') when ('(',')') is
		///  available.  Don't assume these pairs exist, just give them
		///  preference IF they exist in the dependency pairs.
		/// </summary>
		public static readonly char[] CommonPairs = new char[255];
		static CollectTokenPairs()
		{
			CommonPairs['}'] = '{';
			CommonPairs[')'] = '(';
			CommonPairs[']'] = '[';
			CommonPairs['>'] = '<';
		}

		/// <summary>
		/// Map a rule name to a bag of (a,b) tuples that counts occurrences </summary>
		protected internal IDictionary<RuleAltKey, ISet<org.antlr.codebuff.misc.Pair<int, int>>> ruleToPairsBag = new Dictionary<RuleAltKey, ISet<org.antlr.codebuff.misc.Pair<int, int>>>();

		/// <summary>
		/// Track repeated token refs per rule </summary>
		protected internal IDictionary<RuleAltKey, ISet<int>> ruleToRepeatedTokensSet = new Dictionary<RuleAltKey, ISet<int>>();

		/// <summary>
		/// We need parser vocabulary so we can filter for literals like '{' vs ID </summary>
		protected internal Vocabulary vocab;

		protected internal string[] ruleNames;

		public CollectTokenPairs(Vocabulary vocab, string[] ruleNames)
		{
			this.vocab = vocab;
			this.ruleNames = ruleNames;
		}

		public void EnterEveryRule(ParserRuleContext ctx)
		{
			string ruleName = ruleNames[ctx.RuleIndex];
			IList<TerminalNode> tnodes = getDirectTerminalChildren(ctx);
			// Find all ordered unique pairs of literals;
			// no (a,a) pairs and only literals like '{', 'begin', '}', ...
			// Add a for (a,a) into ruleToRepeatedTokensSet for later filtering
			RuleAltKey ruleAltKey = new RuleAltKey(ruleName, ctx.getAltNumber());
			for (int i = 0; i < tnodes.Count; i++)
			{
				for (int j = i + 1; j < tnodes.Count; j++)
				{
					TerminalNode a = tnodes[i];
					TerminalNode b = tnodes[j];
					int atype = a.Symbol.Type;
					int btype = b.Symbol.Type;

                    // KED: THIS CODE DOES NOT WORK WITH GRAMMARS CONTAINING FRAGMENTS.
                    // IN ANTLRV4LEXER.G4 THAT IS IN THIS DIRECTORY, THE GRAMMAR DOES NOT USE
                    // FRAGMENT FOR COLON. BUT THE G4 GRAMMAR IN THE ANTLR GRAMMARS-G4 EXAMPLES,
                    // IT DOES. CONSEQUENTLY, GETLITERALNAME RETURNS NULL!
                    // FRAGMENTS AREN'T PART OF THE VOCABULARY, SO THIS CODE DOES NOT WORK!!
					
                    // only include literals like '{' and ':' not IDENTIFIER etc...
					if (vocab.GetLiteralName(atype) == null || vocab.GetLiteralName(btype) == null)
					{
						continue;
					}

					if (atype == btype)
					{
					    ISet<int> repeatedTokensSet = null;
                        if (! ruleToRepeatedTokensSet.TryGetValue(ruleAltKey, out repeatedTokensSet))
						{
							repeatedTokensSet = new HashSet<int>();
							ruleToRepeatedTokensSet[ruleAltKey] = repeatedTokensSet;
						}
						repeatedTokensSet.Add(atype);
					}
					else
					{
                        org.antlr.codebuff.misc.Pair<int, int> pair = new org.antlr.codebuff.misc.Pair<int, int>(atype, btype);
					    ISet<org.antlr.codebuff.misc.Pair<int, int>> pairsBag = null;
                        if (! ruleToPairsBag.TryGetValue(ruleAltKey, out pairsBag))
						{
							pairsBag = new HashSet<org.antlr.codebuff.misc.Pair<int, int>>();
							ruleToPairsBag[ruleAltKey] = pairsBag;
						}
						pairsBag.Add(pair);
					}
				}
			}
		}

		/// <summary>
		/// Return the list of token dependences for each rule in a Map.
		/// </summary>
		public virtual IDictionary<RuleAltKey, IList<org.antlr.codebuff.misc.Pair<int, int>>> Dependencies
		{
			get
			{
				return stripPairsWithRepeatedTokens();
			}
		}

		/// <summary>
		/// Look for matching common single character literals.
		///  If bliteral is single char, prefer aliteral that is
		///  also a single char.
		///  Otherwise, just pick first aliteral
		/// </summary>
		public static int getMatchingLeftTokenType(Token curToken, IList<int> viableMatchingLeftTokenTypes, Vocabulary vocab)
		{
			int matchingLeftTokenType = viableMatchingLeftTokenTypes[0]; // by default just pick first
			// see if we have a matching common pair of punct
			string bliteral = vocab.GetLiteralName(curToken.Type);
			foreach (int ttype in viableMatchingLeftTokenTypes)
			{
				string aliteral = vocab.GetLiteralName(ttype);
				if (!string.ReferenceEquals(aliteral, null) && aliteral.Length == 3 && !string.ReferenceEquals(bliteral, null) && bliteral.Length == 3)
				{
					char leftChar = aliteral[1];
					char rightChar = bliteral[1];
					if (rightChar < 255 && CommonPairs[rightChar] == leftChar)
					{
						return ttype;
					}
				}
			}
			// not common pair, but give preference if we find two single-char vs ';' and 'fragment' for example
			foreach (int ttype in viableMatchingLeftTokenTypes)
			{
				string aliteral = vocab.GetLiteralName(ttype);
				if (!string.ReferenceEquals(aliteral, null) && aliteral.Length == 3 && !string.ReferenceEquals(bliteral, null) && bliteral.Length == 3)
				{
					return ttype;
				}
			}

			// oh well, just return first one
			return matchingLeftTokenType;
		}

		/// <summary>
		/// Return a new map from rulename to List of (a,b) pairs stripped of
		///  tuples (a,b) where a or b is in rule repeated token set.
		///  E.g., before removing repeated token ',', we see:
		/// 
		///  elementValueArrayInitializer: 4:'{',',' 1:'{','}' 4:',','}'
		/// 
		///  After removing tuples containing repeated tokens, we get:
		/// 
		///  elementValueArrayInitializer: 1:'{','}'
		/// </summary>
		protected internal virtual IDictionary<RuleAltKey, IList<org.antlr.codebuff.misc.Pair<int, int>>> stripPairsWithRepeatedTokens()
		{
			IDictionary<RuleAltKey, IList<org.antlr.codebuff.misc.Pair<int, int>>> ruleToPairsWoRepeats = new Dictionary<RuleAltKey, IList<org.antlr.codebuff.misc.Pair<int, int>>>();
			// For each rule
			foreach (RuleAltKey ruleAltKey in ruleToPairsBag.Keys)
			{
			    ISet<int> ruleRepeatedTokens = null;
                ruleToRepeatedTokensSet.TryGetValue(ruleAltKey, out ruleRepeatedTokens);
			    ISet<org.antlr.codebuff.misc.Pair<int, int>> pairsBag = null;
                ruleToPairsBag.TryGetValue(ruleAltKey, out pairsBag);
				// If there are repeated tokens for this rule
				if (ruleRepeatedTokens != null)
				{
					// Remove all (a,b) for b in repeated token set
					IList<org.antlr.codebuff.misc.Pair<int, int>> pairsWoRepeats = BuffUtils.filter(pairsBag, p => !ruleRepeatedTokens.Contains(p.a) && !ruleRepeatedTokens.Contains(p.b));
					ruleToPairsWoRepeats[ruleAltKey] = pairsWoRepeats;
				}
				else
				{
					ruleToPairsWoRepeats[ruleAltKey] = new List<org.antlr.codebuff.misc.Pair<int,int>>(pairsBag);
				}
			}
			return ruleToPairsWoRepeats;
		}

		/// <summary>
		/// Return a list of the set of terminal nodes that are direct children of ctx. </summary>
		public static IList<TerminalNode> getDirectTerminalChildren(ParserRuleContext ctx)
		{
			if (ctx.children == null)
			{
			    return new List<TerminalNode>();
			}

			IList<TerminalNode> tokenNodes = new List<TerminalNode>();
			foreach (ParseTree o in ctx.children)
			{
				if (o is TerminalNode)
				{
					TerminalNode tnode = (TerminalNode)o;
					tokenNodes.Add(tnode);
				}
			}

			return tokenNodes;
		}

		// satisfy interface only

	    public void VisitTerminal(TerminalNode node)
	    {
	    }

	    public void VisitErrorNode(ErrorNode node)
	    {
	    }

	    public void ExitEveryRule(ParserRuleContext ctx)
	    {
	    }
	}

}