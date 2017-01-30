namespace org.antlr.codebuff.misc
{

	using MurmurHash = Antlr4.Runtime.Misc.MurmurHash;
	using ObjectEqualityComparator = Antlr4.Runtime;

	public class Quad<A, B, C, D>
	{
		public readonly A a;
		public readonly B b;
		public readonly C c;
		public readonly D d;

		public Quad(A a, B b, C c, D d)
		{
			this.a = a;
			this.b = b;
			this.c = c;
			this.d = d;
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			else if (!(obj is Quad<A,B,C,D>))
			{
				return false;
			}
			Quad<A, B, C, D> other = (Quad<A, B, C, D>)obj;
			return Equals(a, other.a) && Equals(b, other.b) && Equals(c, other.c) && Equals(d, other.d);
		}

		public override int GetHashCode()
		{
			int hash = org.antlr.codebuff.misc.MurmurHash.Initialize();
			hash = org.antlr.codebuff.misc.MurmurHash.Update(hash, a);
			hash = org.antlr.codebuff.misc.MurmurHash.Update(hash, b);
			hash = org.antlr.codebuff.misc.MurmurHash.Update(hash, c);
			hash = org.antlr.codebuff.misc.MurmurHash.Update(hash, d);
			return org.antlr.codebuff.misc.MurmurHash.Finish(hash, 4);
		}

		public override string ToString()
		{
			return string.Format("({0}, {1}, {2}, {3})", a, b, c, d);
		}
	}

}