using System;

namespace org.antlr.codebuff.misc
{

	public class MutableDouble
	{
		public double d;

		public MutableDouble(double d)
		{
			this.d = d;
		}

		public virtual double add(double value)
		{
			d += value;
			return d;
		}

		public virtual double div(double value)
		{
			d /= value;
			return d;
		}

		public override string ToString()
		{
			return Convert.ToString(d);
		}
	}

}