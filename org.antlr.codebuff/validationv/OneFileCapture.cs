using System;
using System.Collections.Generic;

namespace org.antlr.codebuff.validation
{

	using BuffUtils = org.antlr.codebuff.misc.BuffUtils;
	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	using Utils = Antlr4.Runtime.Misc.Utils;
	using ST = org.stringtemplate.v4.ST;


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Dbg.normalizedLevenshteinDistance;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.ANTLR4_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.JAVA8_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.JAVA_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.QUORUM_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.SQLITE_CLEAN_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.SQLITE_NOISY_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.TSQL_CLEAN_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.TSQL_NOISY_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.version;

	public class OneFileCapture
	{
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws Exception
		public static void Main(string[] args)
		{
			LangDescriptor[] languages = new LangDescriptor[] {QUORUM_DESCR, JAVA_DESCR, JAVA8_DESCR, ANTLR4_DESCR, SQLITE_NOISY_DESCR, SQLITE_CLEAN_DESCR, TSQL_NOISY_DESCR, TSQL_CLEAN_DESCR};
			for (int i = 0; i < languages.Length; i++)
			{
				LangDescriptor language = languages[i];
				runCaptureForOneLanguage(language);
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void runCaptureForOneLanguage(org.antlr.codebuff.misc.LangDescriptor language) throws Exception
		public static void runCaptureForOneLanguage(LangDescriptor language)
		{
			IList<string> filenames = Tool.getFilenames(new File(language.corpusDir), language.fileRegex);
			IList<float?> selfEditDistances = new List<float?>();
			foreach (string fileName in filenames)
			{
				Corpus corpus = new Corpus(fileName, language);
				corpus.train();
				InputDocument testDoc = Tool.parse(fileName, corpus.language);
				Formatter formatter = new Formatter(corpus, language.indentSize);
				string output = formatter.format(testDoc, false);
				//		System.out.println(output);
				float editDistance = normalizedLevenshteinDistance(testDoc.content, output);
				Console.WriteLine(fileName + " edit distance " + editDistance);
				selfEditDistances.Add(editDistance);
			}

			Corpus corpus = new Corpus(language.corpusDir, language);
			corpus.train();

			IList<float?> corpusEditDistances = new List<float?>();
			foreach (string fileName in filenames)
			{
				InputDocument testDoc = Tool.parse(fileName, corpus.language);
				Formatter formatter = new Formatter(corpus, language.indentSize);
				string output = formatter.format(testDoc, false);
				//		System.out.println(output);
				float editDistance = normalizedLevenshteinDistance(testDoc.content, output);
				Console.WriteLine(fileName + "+corpus edit distance " + editDistance);
				corpusEditDistances.Add(editDistance);
			}
			// heh this gives info on within-corpus variability. i.e., how good/consistent is my corpus?
			// those files with big difference are candidates for dropping from corpus or for cleanup.
			IList<string> labels = BuffUtils.map(filenames, f => '"' + System.IO.Path.GetFileName(f) + '"');

			string python = "#\n" + "# AUTO-GENERATED FILE. DO NOT EDIT\n" + "# CodeBuff <version> '<date>'\n" + "#\n" + "import numpy as np\n" + "import matplotlib.pyplot as plt\n\n" + "fig = plt.figure()\n" + "ax = plt.subplot(111)\n" + "labels = <labels>\n" + "N = len(labels)\n\n" + "featureIndexes = range(0,N)\n" + "<lang>_self = <selfEditDistances>\n" + "<lang>_corpus = <corpusEditDistances>\n" + "<lang>_diff = np.abs(np.subtract(<lang>_self, <lang>_corpus))\n\n" + "all = zip(<lang>_self, <lang>_corpus, <lang>_diff, labels)\n" + "all = sorted(all, key=lambda x : x[2], reverse=True)\n" + "<lang>_self, <lang>_corpus, <lang>_diff, labels = zip(*all)\n\n" + "ax.plot(featureIndexes, <lang>_self, label=\"<lang>_self\")\n" + "#ax.plot(featureIndexes, <lang>_corpus, label=\"<lang>_corpus\")\n" + "ax.plot(featureIndexes, <lang>_diff, label=\"<lang>_diff\")\n" + "ax.set_xticklabels(labels, rotation=60, fontsize=8)\n" + "plt.xticks(featureIndexes, labels, rotation=60)\n" + "ax.yaxis.grid(True, linestyle='-', which='major', color='lightgrey', alpha=0.5)\n\n" + "ax.text(1, .25, 'median $f$ self distance = %5.3f, corpus+$f$ distance = %5.3f' %" + "    (np.median(<lang>_self),np.median(<lang>_corpus)))\n" + "ax.set_xlabel(\"File Name\")\n" + "ax.set_ylabel(\"Edit Distance\")\n" + "ax.set_title(\"Difference between Formatting File <lang> $f$\\nwith Training=$f$ and Training=$f$+Corpus\")\n" + "plt.legend()\n" + "plt.tight_layout()\n" + "fig.savefig(\"images/" + language.name + "_one_file_capture.pdf\", format='pdf')\n" + "plt.show()\n";
			ST pythonST = new ST(python);

			pythonST.add("lang", language.name);
			pythonST.add("version", version);
			pythonST.add("date", DateTime.Now);
			pythonST.add("labels", labels.ToString());
			pythonST.add("selfEditDistances", selfEditDistances.ToString());
			pythonST.add("corpusEditDistances", corpusEditDistances.ToString());

			string code = pythonST.render();

			string fileName = "python/src/" + language.name + "_one_file_capture.py";
			Utils.writeFile(fileName, code);
			Console.WriteLine("wrote python code to " + fileName);
		}
	}

}