namespace org.antlr.codebuff.misc
{

	using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;
	using MurmurHash = Antlr4.Runtime.Misc.MurmurHash;

	/// <summary>
	/// A key that identifies a parent/child/separator relationship where the child
	///  is a sibling list. The separator must be part of key so that expressions
	///  can distinguish between different operators.
	/// </summary>
	public class ParentSiblingListKey
	{
		public int parentRuleIndex;
		public int parentRuleAlt;
		public int childRuleIndex;
		public int childRuleAlt;
		public int separatorTokenType;

		public ParentSiblingListKey(ParserRuleContext parent, ParserRuleContext child, int separatorTokenType)
		{
			parentRuleIndex = parent.RuleIndex;
			parentRuleAlt = parent.getAltNumber();;
			childRuleIndex = child.RuleIndex;
			childRuleAlt = child.getAltNumber();
			this.separatorTokenType = separatorTokenType;
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			else if (!(obj is ParentSiblingListKey))
			{
				return false;
			}

			ParentSiblingListKey other = (ParentSiblingListKey)obj;
			return parentRuleIndex == other.parentRuleIndex && parentRuleAlt == other.parentRuleAlt && childRuleIndex == other.childRuleIndex && childRuleAlt == other.childRuleAlt && separatorTokenType == other.separatorTokenType;
		}

		public override int GetHashCode()
		{
			int hash = org.antlr.codebuff.misc.MurmurHash.Initialize();
			hash = org.antlr.codebuff.misc.MurmurHash.Update(hash, parentRuleIndex);
			hash = org.antlr.codebuff.misc.MurmurHash.Update(hash, parentRuleAlt);
			hash = org.antlr.codebuff.misc.MurmurHash.Update(hash, childRuleIndex);
			hash = org.antlr.codebuff.misc.MurmurHash.Update(hash, childRuleAlt);
			hash = org.antlr.codebuff.misc.MurmurHash.Update(hash, separatorTokenType);
			return org.antlr.codebuff.misc.MurmurHash.Finish(hash, 5);
		}

		public override string ToString()
		{
			return string.Format("({0:D}, {1:D}, {2:D}, {3:D}, {4:D})", parentRuleIndex, parentRuleAlt, childRuleIndex, childRuleAlt, separatorTokenType);
		}
	}

}