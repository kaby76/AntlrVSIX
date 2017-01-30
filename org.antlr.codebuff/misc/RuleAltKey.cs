namespace org.antlr.codebuff.misc
{

	using MurmurHash = Antlr4.Runtime.Misc.MurmurHash;

	public class RuleAltKey
	{
		public string ruleName;
		public int altNum;

		public RuleAltKey(string ruleName, int altNum)
		{
			this.altNum = altNum;
			this.ruleName = ruleName;
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			else if (!(obj is RuleAltKey))
			{
				return false;
			}

			RuleAltKey other = (RuleAltKey)obj;
			return ruleName.Equals(other.ruleName) && altNum == other.altNum;
		}

		public override int GetHashCode()
		{
			int hash = org.antlr.codebuff.misc.MurmurHash.Initialize();
			hash = org.antlr.codebuff.misc.MurmurHash.Update(hash, ruleName);
			hash = org.antlr.codebuff.misc.MurmurHash.Update(hash, altNum);
			return org.antlr.codebuff.misc.MurmurHash.Finish(hash, 2);
		}

		public override string ToString()
		{
			return string.Format("{0}:{1:D}", ruleName, altNum);
		}
	}

}