using System;
using System.Collections.Generic;
using org.antlr.codebuff.misc;

namespace org.antlr.codebuff.validation
{

	using BuffUtils = org.antlr.codebuff.misc.BuffUtils;
	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	using Triple = Antlr4.Runtime.Misc.Triple;
	using Utils = Antlr4.Runtime.Misc.Utils;
	using ST = org.stringtemplate.v4.ST;


	public class Stability
	{
		public const int STAGES = 5;

		public static void Main(string[] args)
		{
			LeaveOneOutValidator.FORCE_SINGLE_THREADED = true; // need this when we compare results file by file
			LangDescriptor[] languages = new LangDescriptor[]{QUORUM_DESCR};

			IDictionary<string, IList<float?>> results = new Dictionary<string, IList<float?>>();
			foreach (LangDescriptor language in languages)
			{
				IList<float?> errorRates = checkStability(language);
				Console.WriteLine(language.name + " " + errorRates);
				results[language.name] = errorRates;
			}
			foreach (string name in results.Keys)
			{
				Console.WriteLine(name + " = " + results[name]);
			}

			string python = "#\n" + "# AUTO-GENERATED FILE. DO NOT EDIT\n" + "# CodeBuff <version> '<date>'\n" + "#\n" + "import numpy as np\n" + "import matplotlib.pyplot as plt\n\n" + "import matplotlib\n" + "fig = plt.figure()\n" + "ax = plt.subplot(111)\n" + "N = <N>\n" + "sizes = range(0,N)\n" + "<results:{r |\n" + "<r> = [<results.(r); separator={,}>]\n" + "ax.plot(sizes, <r>, label=\"<r>\", marker='o')\n" + "}>\n" + "ax.yaxis.grid(True, linestyle='-', which='major', color='lightgrey', alpha=0.5)\n" + "xa = ax.get_xaxis()\n" + "xa.set_major_locator(matplotlib.ticker.MaxNLocator(integer=True))\n" + "ax.set_xlabel(\"Formatting Stage; stage 0 is first formatting pass\")\n" + "ax.set_ylabel(\"Median Leave-one-out Validation Error Rate\")\n" + "ax.set_title(\"<N>-Stage Formatting Stability\\nStage $n$ is formatted output of stage $n-1$\")\n" + "plt.legend()\n" + "plt.tight_layout()\n" + "fig.savefig('images/stability.pdf', format='pdf')\n" + "plt.show()\n";
			ST pythonST = new ST(python);
			pythonST.add("results", results);
			pythonST.add("version", version);
			pythonST.add("date", DateTime.Now);
			pythonST.add("N", STAGES + 1);
			string fileName = "python/src/stability.py";
            org.antlr.codebuff.misc.Utils.writeFile(fileName, pythonST.render());
			Console.WriteLine("wrote python code to " + fileName);
		}

		public static IList<float?> checkStability(LangDescriptor language)
		{
			IList<float?> errorRates = new List<float?>();

			// format the corpus into tmp dir
			LeaveOneOutValidator validator0 = new LeaveOneOutValidator(language.corpusDir, language);
			Triple<IList<Formatter>, IList<float>, IList<float>> results0 = validator0.validateDocuments(false, "/tmp/stability/1");
			errorRates.Add(BuffUtils.median(results0.c));

			IList<Formatter> formatters0 = results0.a;
			// now try formatting it over and over
			for (int i = 1; i <= STAGES; i++)
			{
				string inputDir = "/tmp/stability/" + i;
				string outputDir = "/tmp/stability/" + (i + 1);
				LeaveOneOutValidator validator = new LeaveOneOutValidator(inputDir, language);
				Triple<IList<Formatter>, IList<float>, IList<float>> results = validator.validateDocuments(false, outputDir);
				IList<Formatter> formatters = results.a;
				IList<float?> distances = new List<float?>();
				for (int j = 0; j < formatters.Count; j++)
				{
					Formatter f0 = formatters0[j];
					Formatter f = formatters[j];
					float editDistance = Dbg.normalizedLevenshteinDistance(f.Output, f0.Output);
					distances.Add(editDistance);
				}
				errorRates.Add(BuffUtils.median(distances));
			}

			return errorRates;
		}
	}

}