using System;
using System.Collections.Generic;
using System.Text;

namespace org.antlr.codebuff.validation
{
    using misc;
    using BuffUtils = org.antlr.codebuff.misc.BuffUtils;
    using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
    //using Triple = Antlr4.Runtime.Misc.Triple;
    using Utils = Antlr4.Runtime.Misc.Utils;

    public class TestK : LeaveOneOutValidator
	{
		public new const bool FORCE_SINGLE_THREADED = false;
		public readonly int k;

		public TestK(string rootDir, LangDescriptor language, int k) : base(rootDir, language)
		{
			this.k = k;
		}

		public static void Main(string[] args)
		{
			LangDescriptor[] languages = new LangDescriptor[] {Tool.ANTLR4_DESCR};

			int MAX_K = 98; // should be odd
			int OUTLIER_K = 99;
			IList<int?> ks = new List<int?>();
			for (int i = 1; i <= MAX_K; i += 2)
			{
				ks.Add(i);
			}
			ks.Add(OUTLIER_K);
			// track medians[language][k]
			float[][] medians = new float[languages.Length + 1][];

		    int ncpu = 1;
			if (FORCE_SINGLE_THREADED)
			{
				ncpu = 2;
			}
			ExecutorService pool = Executors.newFixedThreadPool(ncpu - 1);
			IList<Callable<Void>> jobs = new List<Callable<Void>>();

			for (int i = 0; i < languages.Length; i++)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.antlr.codebuff.misc.LangDescriptor language = languages[i];
				LangDescriptor language = languages[i];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int langIndex = i;
				int langIndex = i;
				Console.WriteLine(language.name);
				foreach (int k in ks)
				{
					medians[langIndex] = new float?[OUTLIER_K + 1];
					Callable<Void> job = () =>
				{
					try
					{
						TestK tester = new TestK(language.corpusDir, language, k);
						IList<float?> errorRates = tester.scoreDocuments();
						errorRates.Sort();
						int n = errorRates.Count;
						float median = errorRates[n / 2].Value;
//						double var = BuffUtils.varianceFloats(errorRates);
//						String display = String.format("%5.4f, %5.4f, %5.4f, %5.4f, %5.4f", min, quart, median, quart3, max);
						medians[langIndex][k] = median;
					}
					catch (Exception t)
					{
						t.printStackTrace(System.err);
					}
					return null;
				};
					jobs.Add(job);
				}
			}

			pool.invokeAll(jobs);
			pool.shutdown();
			bool terminated = pool.awaitTermination(60, TimeUnit.MINUTES);

			writePython(languages, ks, medians);
		}

		public static void writePython(LangDescriptor[] languages, IList<int?> ks, float[][] medians)
		{
			StringBuilder data = new StringBuilder();
			StringBuilder plot = new StringBuilder();
			for (int i = 0; i < languages.Length; i++)
			{
				LangDescriptor language = languages[i];
				IList<float?> filteredMedians = BuffUtils.filter(Arrays.asList(medians[i]), m => m != null);
				data.Append(language.name + '=' + filteredMedians + '\n');
				plot.Append(string.Format("ax.plot(ks, {0}, label=\"{1}\", marker='{2}', color='{3}')\n", language.name, language.name, nameToGraphMarker.get(language.name), nameToGraphColor.get(language.name)));
			}

			string python = "#\n" + "# AUTO-GENERATED FILE. DO NOT EDIT\n" + "# CodeBuff %s '%s'\n" + "#\n" + "import numpy as np\n" + "import matplotlib.pyplot as plt\n\n" + "%s\n" + "ks = %s\n" + "fig = plt.figure()\n" + "ax = plt.subplot(111)\n" + "%s" + "ax.tick_params(axis='both', which='major', labelsize=18)\n" + "ax.set_xlabel(\"$k$ nearest neighbors\", fontsize=20)\n" + "ax.set_ylabel(\"Median error rate\", fontsize=20)\n" + "#ax.set_title(\"k Nearest Neighbors vs\\nLeave-one-out Validation Error Rate\")\n" + "plt.legend(fontsize=18)\n\n" + "fig.savefig('images/vary_k.pdf', format='pdf')\n" + "plt.show()\n";
			string code = string.format(python, Tool.version, DateTime.Now, data, ks, plot);

			string fileName = "python/src/vary_k.py";
            org.antlr.codebuff.misc.Utils.writeFile(fileName, code);
			Console.WriteLine("wrote python code to " + fileName);
		}

		/// <summary>
		/// Return error rate for each document using leave-one-out validation </summary>
		public virtual IList<float?> scoreDocuments()
		{
			IList<string> allFiles = Tool.getFilenames(rootDir, language.fileRegex);
			IList<InputDocument> documents = Tool.load(allFiles, language);
			IList<float?> errors = new List<float?>();
			for (int i = 0; i < documents.Count; i++)
			{
				Triple<Formatter, float, float> results = validate(language, documents, documents[i].fileName, k, null, false, false);
				float? errorRate = results.c;
				errors.Add(errorRate);
			}
			return errors;
		}
	}

}