using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;
using System.Linq;

namespace org.antlr.codebuff.misc
{

	using CommonToken = CommonToken;
	using CommonTokenStream = CommonTokenStream;
	using Lexer = Lexer;
	using Token = IToken;
	using TokenSource = ITokenSource;


	/// <summary>
	/// Override to fix bug in LB() </summary>
	public class CodeBuffTokenStream : CommonTokenStream
	{
		public CodeBuffTokenStream(CommonTokenStream stream) : base(stream.TokenSource)
		{
			this.fetchedEOF = false;
			foreach (Token t in stream.GetTokens())
			{
				tokens.Add(new CommonToken(t));
			}
			Reset(); // Virtual member called in constructor...
		}

		public CodeBuffTokenStream(TokenSource tokenSource) : base(tokenSource)
		{
		}

        protected override Token Lb(int k)
        {
			if (k == 0 || (p - k) < 0)
			{
				return null;
			}

			int i = p;
			int n = 1;
			// find k good tokens looking backwards
			while (i >= 1 && n <= k)
			{
				// skip off-channel tokens
				i = PreviousTokenOnChannel(i - 1, channel);
				n++;
			}
			if (i < 0)
			{
				return null;
			}
			return tokens[i];
		}

		public virtual Token getPreviousRealToken(int i)
		{
			i--; // previousTokenOnChannel is inclusive
			int pi = PreviousTokenOnChannel(i, TokenConstants.DefaultChannel);
			if (pi >= 0 && pi < this.Size)
			{
				return this.Get(pi);
			}
			return null;
		}

		public virtual Token getNextRealToken(int i)
		{
			i++; // nextTokenOnChannel is inclusive
			int ni = NextTokenOnChannel(i, TokenConstants.DefaultChannel);
			if (ni >= 0 && ni < this.Size)
			{
				return this.Get(ni);
			}
			return null;
		}

		public virtual IList<Token> RealTokens
		{
			get
			{
				return getRealTokens(0, this.Size - 1);
			}
		}

		public virtual IList<Token> getRealTokens(int from, int to)
		{
			IList<Token> real = new List<Token>();
			for (int i = from; i <= to; i++)
			{
				Token t = tokens[i];
				if (t.Channel == Lexer.DefaultTokenChannel)
				{
					real.Add(t);
				}
			}
			if (real.Count == 0)
			{
				return null;
			}
			return real;
		}
	}

}