using System;
using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;
using System.Linq;

namespace org.antlr.codebuff.misc
{
	using ParserRuleContext = ParserRuleContext;
	using ParseTree = Antlr4.Runtime.Tree.IParseTree;

	public class BuffUtils
	{

		public static int indexOf(ParserRuleContext parent, ParseTree child)
		{
			for (int i = 0; i < parent.ChildCount; i++)
			{
				if (parent.GetChild(i) == child)
				{
					return i;
				}
			}
			return -1;
		}

		//  Generic filtering, mapping, joining that should be in the standard library but aren't
		public static T findFirst<T>(IList<T> data, System.Func<T, bool> pred)
            where T: class
        {
			if (data != null)
			{
				foreach (T x in data)
				{
				    if (pred(x))
				    {
					    return x;
				    }
				}
			}
			return null;
		}

		public static IList<T> filter<T>(IList<T> data, System.Func<T, bool> pred)
		{
			IList<T> output = new List<T>();
			if (data != null)
			{
				foreach (T x in data)
				{
				    if (pred(x))
				    {
					    output.Add(x);
				    }
				}
			}
			return output;
		}

		public static IList<T> filter<T>(ICollection<T> data, System.Func<T, bool> pred)
		{
			IList<T> output = new List<T>();
			foreach (T x in data)
			{
				if (pred(x))
				{
					output.Add(x);
				}
			}
			return output;
		}

		public static IList<R> map<T, R>(ICollection<T> data, System.Func<T, R> getter)
		{
			IList<R> output = new List<R>();
			if (data != null)
			{
				foreach (T x in data)
				{
				    output.Add(getter(x));
				}
			}
			return output;
		}

		public static IList<R> map<T, R>(T[] data, System.Func<T, R> getter)
		{
			IList<R> output = new List<R>();
			if (data != null)
			{
				foreach (T x in data)
				{
				    output.Add(getter(x));
				}
			}
			return output;
		}

		public static double variance(IList<int> data)
		{
			int n = data.Count;
			double sum = 0;
			double avg = /* sumDoubles(data) */ data.Sum() / ((double) n);
			foreach (int d in data)
			{
				sum += (d - avg) * (d - avg);
			}
			return sum / n;
		}

		public static double varianceFloats(IList<float> data)
		{
			int n = data.Count;
			double sum = 0;
			double avg = data.Sum() / ((double) n);
			foreach (float d in data)
			{
				sum += (d - avg) * (d - avg);
			}
			return sum / n;
		}

		public static int sum(ICollection<int> data)
		{
			int sum = 0;
			foreach (int d in data)
			{
				sum += d;
			}
			return sum;
		}

		public static float sumFloats(ICollection<float> data)
		{
			float sum = 0;
			foreach (float d in data)
			{
				sum += d;
			}
			return sum;
		}

		public static float sumDoubles(ICollection<double> data)
		{
			float sum = 0;
			foreach (double d in data)
			{
				sum += (float)d;
			}
			return sum;
		}

		public static IList<double> diffFloats(IList<float> a, IList<float> b)
		{
			IList<double> diffs = new List<double>();
			for (int i = 0; i < a.Count; i++)
			{
				diffs.Add((double) a[i] - b[i]);
			}
			return diffs;
		}

		public static T median<T>(IList<T> data)
		{
            List<double> l = data as List<double>;
            l.Sort(); // IN-PLACE SORT!!!
			int n = data.Count;
			return data[n / 2];
		}

		public static double max(IList<double> data)
		{
		    List<double> l = data as List<double>;
            l.Sort(); // IN-PLACE SORT!!!
            int n = data.Count;
			return data[n - 1];
		}

		public static double min(IList<double> data)
		{
            List<double> l = data as List<double>;
            l.Sort(); // IN-PLACE SORT!!!
            int n = data.Count;
			return data[0];
		}

		public static double mean(ICollection<double> data)
		{
			double sum = 0.0;
			foreach (double d in data)
			{
				sum += d;
			}
			return sum / data.Count;
		}
	}

}