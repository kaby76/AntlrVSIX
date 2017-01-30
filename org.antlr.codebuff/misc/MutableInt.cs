namespace org.antlr.codebuff.misc
{

	public class MutableInt
	{
		public int i;

		public MutableInt(int i)
		{
			this.i = i;
		}

		public virtual void inc()
		{
			i++;
		}

		public virtual int asInt()
		{
			return i;
		}

		public override string ToString()
		{
			return i.ToString();
		}
	}

}