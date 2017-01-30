using System;
using System.Collections.Generic;

namespace org.antlr.codebuff.validation
{
    using misc;
    using Triple = Antlr4.Runtime.Misc.Triple;


    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.Dbg.normalizedLevenshteinDistance;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.Tool.JAVA8_DESCR;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.Tool.JAVA8_GUAVA_DESCR;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.Tool.JAVA_DESCR;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.Tool.JAVA_GUAVA_DESCR;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.Tool.SQLITE_CLEAN_DESCR;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.Tool.TSQL_CLEAN_DESCR;

    public class GrammarInvariance
	{
		public static void Main(string[] args)
		{
			// we need to get all of the results in order so that we can compare
			LeaveOneOutValidator.FORCE_SINGLE_THREADED = true;
			float sql_median;
			float java_st_median;
			float java_guava_median;

			{
				// JAVA
				IList<float?> distances = new List<float?>();
				LeaveOneOutValidator javaValidator = new LeaveOneOutValidator(Tool.JAVA_DESCR.corpusDir, JAVA_DESCR);
				LeaveOneOutValidator java8Validator = new LeaveOneOutValidator(JAVA8_DESCR.corpusDir, JAVA8_DESCR);
				Triple<IList<Formatter>, IList<float?>, IList<float?>> javaResults = javaValidator.validateDocuments(false, null);
				Triple<IList<Formatter>, IList<float?>, IList<float?>> java8Results = java8Validator.validateDocuments(false, null);
				IList<Formatter> javaFormatters = javaResults.a;
				IList<Formatter> java8Formatters = java8Results.a;

				for (int i = 0; i < javaFormatters.Count; i++)
				{
					Formatter java = javaFormatters[i];
					Formatter java8 = java8Formatters[i];
					float editDistance = Dbg.normalizedLevenshteinDistance(java.Output, java8.Output);
					distances.Add(editDistance);
	//				System.out.println(java.testDoc.fileName+" edit distance "+editDistance);
				}

				{
					distances.Sort();
					int n = distances.Count;
					float min = distances[0].Value;
					float quart = distances[(int)(0.27 * n)].Value;
					float median = distances[n / 2].Value;
					float quart3 = distances[(int)(0.75 * n)].Value;
					float max = distances[distances.Count - 1].Value;
					string display = "(" + min + "," + median + "," + max + ")";
					java_st_median = median;
				}
			}

			{
				// JAVA GUAVA
				IList<float?> distances = new List<float?>();
				LeaveOneOutValidator java_guavaValidator = new LeaveOneOutValidator(JAVA_GUAVA_DESCR.corpusDir, JAVA_GUAVA_DESCR);
				LeaveOneOutValidator java8_guavaValidator = new LeaveOneOutValidator(JAVA8_GUAVA_DESCR.corpusDir, JAVA8_GUAVA_DESCR);
				Triple<IList<Formatter>, IList<float?>, IList<float?>> java_guavaResults = java_guavaValidator.validateDocuments(false, null);
				Triple<IList<Formatter>, IList<float?>, IList<float?>> java8_guavaResults = java8_guavaValidator.validateDocuments(false, null);
				IList<Formatter> java_guavaFormatters = java_guavaResults.a;
				IList<Formatter> java8_guavaFormatters = java8_guavaResults.a;

				for (int i = 0; i < java_guavaFormatters.Count; i++)
				{
					Formatter java_guava = java_guavaFormatters[i];
					Formatter java8_guava = java8_guavaFormatters[i];
					float editDistance = Dbg.normalizedLevenshteinDistance(java_guava.Output, java8_guava.Output);
					distances.Add(editDistance);
	//				System.out.println(java_guava.testDoc.fileName+" edit distance "+editDistance);
				}

				{
					distances.Sort();
					int n = distances.Count;
					float min = distances[0].Value;
					float quart = distances[(int)(0.27 * n)].Value;
					float median = distances[n / 2].Value;
					float quart3 = distances[(int)(0.75 * n)].Value;
					float max = distances[distances.Count - 1].Value;
					string display = "(" + min + "," + median + "," + max + ")";
					java_guava_median = median;
				}
			}
			Console.WriteLine("clean SQLite vs TSQL edit distance info median=" + sql_median);
			Console.WriteLine("Java vs Java8 edit distance info median=" + java_st_median);
			Console.WriteLine("Java vs Java8 guava edit distance info median=" + java_guava_median);
		}
	}

}