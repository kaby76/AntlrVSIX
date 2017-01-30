using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.antlr.codebuff.validation
{
    using misc;
    using BuffUtils = org.antlr.codebuff.misc.BuffUtils;
    using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
    //using Triple = Antlr4.Runtime.Misc.Triple;
    using Utils = Antlr4.Runtime.Misc.Utils;


    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.Dbg.normalizedLevenshteinDistance;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.Tool.ANTLR4_DESCR;
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
    //	import static org.antlr.codebuff.Tool.SQLITE_NOISY_DESCR;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.Tool.TSQL_CLEAN_DESCR;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.Tool.TSQL_NOISY_DESCR;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.Tool.getFilenames;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.misc.BuffUtils.filter;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.misc.BuffUtils.map;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //	import static org.antlr.codebuff.misc.BuffUtils.median;

    public class LeaveOneOutValidator
	{
		public const int DOCLIST_RANDOM_SEED = 951413; // need randomness but use same seed to get reproducibility
		internal readonly Random random = new Random();

		public static bool FORCE_SINGLE_THREADED = false;

		public static readonly IDictionary<string, string> nameToGraphMarker = new HashMapAnonymousInnerClass();

		private class HashMapAnonymousInnerClass : Dictionary<string, string>
		{
			public HashMapAnonymousInnerClass()
			{
			}

	//		{
	//		put("antlr", ".");
	//		put("java_st", "s");
	//		put("java8_st", "+");
	//		put("java_guava", ">");
	//		put("java8_guava", "d");
	//		put("sqlite", "o");
	//		put("tsql", "p");
	//	}
		}

		public static readonly IDictionary<string, string> nameToGraphColor = new HashMapAnonymousInnerClass2();

		private class HashMapAnonymousInnerClass2 : Dictionary<string, string>
		{
			public HashMapAnonymousInnerClass2()
			{
			}

	//		{
	//		put("antlr", "k");
	//		put("java_st", "g");
	//		put("java8_st", "b");
	//		put("java_guava", "m");
	//		put("java8_guava", "c");
	//		put("sqlite", "y");
	//		put("tsql", "r");
	//	}
		}

		public string rootDir;
		public LangDescriptor language;

		internal IList<double> trainingTimes = new List<double>();
		internal IList<double> formattingTokensPerMS = new List<double>();

		public LeaveOneOutValidator(string rootDir, LangDescriptor language)
		{
			this.rootDir = rootDir;
			this.language = language;
            random = new Random(DOCLIST_RANDOM_SEED);
		}

		public virtual Triple<Formatter, float, float> validateOneDocument(string fileToExclude, string outputDir, bool collectAnalysis)
		{
			IList<string> allFiles = Tool.getFilenames(rootDir, language.fileRegex);
			IList<InputDocument> documents = Tool.load(allFiles, language);
			return validate(language, documents, fileToExclude, Formatter.DEFAULT_K, outputDir, false, collectAnalysis);
		}

		public virtual Triple<IList<Formatter>, IList<float>, IList<float>> validateDocuments(bool computeEditDistance, string outputDir)
		{
			return validateDocuments(Trainer.FEATURES_INJECT_WS, Trainer.FEATURES_HPOS, computeEditDistance, outputDir);
		}

		public virtual Triple<IList<Formatter>, IList<float>, IList<float>> validateDocuments(FeatureMetaData[] injectWSFeatures, FeatureMetaData[] alignmentFeatures, bool computeEditDistance, string outputDir)
		{
			IList<Formatter> formatters = new List<Formatter>();
			IList<float> distances = new List<float>();
			IList<float> errors = new List<float>();
			System.DateTime start = System.DateTime.Now;
			try
			{
				IList<string> allFiles = Tool.getFilenames(rootDir, language.fileRegex);
				IList<InputDocument> documents = Tool.load(allFiles, language);
				IList<InputDocument> parsableDocuments = BuffUtils.filter(documents, d => d.tree != null);
                System.DateTime stop = System.DateTime.Now;
				//Console.Write("Load/parse all docs from {0} time {1:D} ms\n", rootDir, (stop - start) / 1000000);

				int ncpu = 1;
				if (FORCE_SINGLE_THREADED)
				{
					ncpu = 2;
				}

			    for (int i = 0; i < parsableDocuments.Count; i++)
			    {
			        string fileName = parsableDocuments[i].fileName;

			        {
			            try
			            {
			                Triple<Formatter, float, float> results = validate(language, parsableDocuments, fileName,
			                    Formatter.DEFAULT_K, injectWSFeatures, alignmentFeatures, outputDir, computeEditDistance, false);
			                formatters.Add(results.a);
			                float editDistance = results.b;
			                distances.Add(editDistance);
			                float errorRate = results.c;
			                errors.Add(errorRate);
			            }
			            catch (Exception t)
			            {
                            System.Console.WriteLine(t.StackTrace);
			            }
			            return null;
			        }
			    }
			}
			finally
			{
				DateTime final_stop = System.DateTime.Now;
				double medianTrainingTime = BuffUtils.median(trainingTimes);
				double medianFormattingPerMS = BuffUtils.median(formattingTokensPerMS);
				Console.Write("Total time {0:D}ms\n", final_stop - start);
				Console.Write("Median training time {0:D}ms\n", medianTrainingTime);
				Console.Write("Median formatting time tokens per ms {0,5:F4}ms, min {1,5:F4} max {2,5:F4}\n", medianFormattingPerMS, BuffUtils.min(formattingTokensPerMS), BuffUtils.max(formattingTokensPerMS));
			}
			return new Triple<IList<Formatter>, IList<float>, IList<float>>(formatters,distances,errors);
		}

		public virtual Triple<Formatter, float, float> validate(LangDescriptor language, IList<InputDocument> documents, string fileToExclude, int k, string outputDir, bool computeEditDistance, bool collectAnalysis)
		{
			return validate(language, documents, fileToExclude, k, Trainer.FEATURES_INJECT_WS, Trainer.FEATURES_HPOS, outputDir, computeEditDistance, collectAnalysis);
		}

		public virtual Triple<Formatter, float, float> validate(LangDescriptor language, IList<InputDocument> documents, string fileToExclude, int k, FeatureMetaData[] injectWSFeatures, FeatureMetaData[] alignmentFeatures, string outputDir, bool computeEditDistance, bool collectAnalysis)
		{
			string path = System.IO.Path.GetFullPath(fileToExclude);
			IList<InputDocument> others = BuffUtils.filter(documents, d => !d.fileName.Equals(path));
			IList<InputDocument> excluded = BuffUtils.filter(documents, d => d.fileName.Equals(path));
			Debug.Assert(others.Count == documents.Count - 1);
	//		kNNClassifier.resetCache();
			if (excluded.Count == 0)
			{
				Console.Error.WriteLine("Doc not in corpus: " + path);
				return null;
			}
			InputDocument testDoc = excluded[0];
			DateTime start = System.DateTime.Now;
			Corpus corpus = new Corpus(others, language);
			corpus.train();
			DateTime stop = System.DateTime.Now;
			Formatter formatter = new Formatter(corpus, language.indentSize, k, injectWSFeatures, alignmentFeatures);
			InputDocument originalDoc = testDoc;
			DateTime format_start = System.DateTime.Now;
			string output = formatter.format(testDoc, collectAnalysis);
			DateTime format_stop = System.DateTime.Now;
			float editDistance = 0;
			if (computeEditDistance)
			{
				editDistance = Dbg.normalizedLevenshteinDistance(testDoc.content, output);
			}
			ClassificationAnalysis analysis = new ClassificationAnalysis(originalDoc, formatter.AnalysisPerToken);
			Console.WriteLine(testDoc.fileName + ": edit distance = " + editDistance + ", error rate = " + analysis.ErrorRate);
			if (!string.ReferenceEquals(outputDir, null))
			{
				string dir = outputDir + "/" + language.name + "/" + Tool.version;
				if (!System.IO.Directory.Exists(dir))
				{
					System.IO.Directory.CreateDirectory(dir);
				}
                org.antlr.codebuff.misc.Utils.writeFile(dir + "/" + System.IO.Path.GetFileName(testDoc.fileName), output);
			}
			var tms = (stop - start);
			var fms = format_stop - format_start;
			trainingTimes.Add((double)tms.Milliseconds);
			float tokensPerMS = testDoc.tokens.Size / (float) fms.TotalMilliseconds;
			formattingTokensPerMS.Add((double)tokensPerMS);
			Console.Write("Training time = {0:D} ms, formatting {1:D} ms, {2,5:F3} tokens/ms ({3:D} tokens)\n", tms, fms, tokensPerMS, testDoc.tokens.Size);
	//		System.out.printf("classify calls %d, hits %d rate %f\n",
	//		                  kNNClassifier.nClassifyCalls, kNNClassifier.nClassifyCacheHits,
	//		                  kNNClassifier.nClassifyCacheHits/(float) kNNClassifier.nClassifyCalls);
	//		System.out.printf("kNN calls %d, hits %d rate %f\n",
	//						  kNNClassifier.nNNCalls, kNNClassifier.nNNCacheHits,
	//						  kNNClassifier.nNNCacheHits/(float) kNNClassifier.nNNCalls);
			return new Triple<Formatter, float, float>(formatter, editDistance, analysis.ErrorRate);
		}

		public static string testAllLanguages(LangDescriptor[] languages, string[] corpusDirs, string imageFileName)
		{
			IList<string> languageNames = BuffUtils.map(languages, l => l.name + "_err");
	//		Collections.sort(languageNames);
			IDictionary<string, int?> corpusSizes = new Dictionary<string, int?>();
			for (int i = 0; i < languages.Length; i++)
			{
				LangDescriptor language = languages[i];
				IList<string> filenames = Tool.getFilenames(corpusDirs[i], language.fileRegex);
				corpusSizes[language.name] = filenames.Count;
			}
			IList<string> languageNamesAsStr = BuffUtils.map(languages, l => '"' + l.name + "\\nn=" + corpusSizes[l.name] + '"');
	//		Collections.sort(languageNamesAsStr);

			StringBuilder data = new StringBuilder();
			for (int i = 0; i < languages.Length; i++)
			{
				LangDescriptor language = languages[i];
				string corpus = corpusDirs[i];
				LeaveOneOutValidator validator = new LeaveOneOutValidator(corpus, language);
				Triple<IList<Formatter>, IList<float>, IList<float>> results = validator.validateDocuments(true, "/tmp");
				IList<Formatter> formatters = results.a;
				IList<float> distances = results.b;
				IList<float> errors = results.c;
	//			data.append(language.name+"_dist = "+distances+"\n");
				data.Append(language.name + "_err = " + errors + "\n");
			}

			string python = "#\n" + "# AUTO-GENERATED FILE. DO NOT EDIT\n" + "# CodeBuff %s '%s'\n" + "#\n" + "import numpy as np\n" + "import pylab\n" + "import matplotlib.pyplot as plt\n\n" + "%s\n" + "language_data = %s\n" + "labels = %s\n" + "fig = plt.figure()\n" + "ax = plt.subplot(111)\n" + "ax.boxplot(language_data,\n" + "           whis=[10, 90], # 10 and 90 %% whiskers\n" + "           widths=.35,\n" + "           labels=labels,\n" + "           showfliers=False)\n" + "ax.set_xticklabels(labels, rotation=60, fontsize=18)\n" + "ax.tick_params(axis='both', which='major', labelsize=18)\n" + "plt.xticks(range(1,len(labels)+1), labels, rotation=60, fontsize=18)\n" + "pylab.ylim([0,.28])\n" + "ax.yaxis.grid(True, linestyle='-', which='major', color='lightgrey', alpha=0.5)\n" + "ax.set_xlabel(\"Grammar and corpus size\", fontsize=20)\n" + "ax.set_ylabel(\"Misclassification Error Rate\", fontsize=20)\n" + "# ax.set_title(\"Leave-one-out Validation Using Error Rate\\nBetween Formatted and Original File\")\n" + "plt.tight_layout()\n" + "fig.savefig('images/%s', format='pdf')\n" + "plt.show()\n";
			return string.Format(python, Tool.version, DateTime.Now, data, languageNames, languageNamesAsStr, imageFileName);
		}

		public static void Main(string[] args)
		{
			LangDescriptor[] languages = new LangDescriptor[] { Tool.ANTLR4_DESCR };
			IList<string> corpusDirs = BuffUtils.map(languages, l => l.corpusDir);
			string[] dirs = corpusDirs.ToArray();
			string python = testAllLanguages(languages, dirs, "leave_one_out.pdf");
			string fileName = "python/src/leave_one_out.py";
            org.antlr.codebuff.misc.Utils.writeFile(fileName, python);
			Console.WriteLine("wrote python code to " + fileName);
		}
	}

}