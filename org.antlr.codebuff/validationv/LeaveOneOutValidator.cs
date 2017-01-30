using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace org.antlr.codebuff.validation
{

	using BuffUtils = org.antlr.codebuff.misc.BuffUtils;
	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	using Triple = Antlr4.Runtime.Misc.Triple;
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

		internal IList<double?> trainingTimes = new List<double?>();
		internal IList<double?> formattingTokensPerMS = new List<double?>();

		public LeaveOneOutValidator(string rootDir, LangDescriptor language)
		{
			this.rootDir = rootDir;
			this.language = language;
			random.Seed = DOCLIST_RANDOM_SEED;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public org.antlr.v4.runtime.misc.Triple<org.antlr.codebuff.Formatter,Nullable<float>,Nullable<float>> validateOneDocument(String fileToExclude, String outputDir, boolean collectAnalysis) throws Exception
		public virtual Triple<Formatter, float?, float?> validateOneDocument(string fileToExclude, string outputDir, bool collectAnalysis)
		{
			IList<string> allFiles = getFilenames(new File(rootDir), language.fileRegex);
			IList<InputDocument> documents = Tool.load(allFiles, language);
			return validate(language, documents, fileToExclude, Formatter.DEFAULT_K, outputDir, false, collectAnalysis);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public org.antlr.v4.runtime.misc.Triple<java.util.List<org.antlr.codebuff.Formatter>,java.util.List<Nullable<float>>,java.util.List<Nullable<float>>> validateDocuments(boolean computeEditDistance, String outputDir) throws Exception
		public virtual Triple<IList<Formatter>, IList<float?>, IList<float?>> validateDocuments(bool computeEditDistance, string outputDir)
		{
			return validateDocuments(Trainer.FEATURES_INJECT_WS, Trainer.FEATURES_HPOS, computeEditDistance, outputDir);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public org.antlr.v4.runtime.misc.Triple<java.util.List<org.antlr.codebuff.Formatter>,java.util.List<Nullable<float>>,java.util.List<Nullable<float>>> validateDocuments(org.antlr.codebuff.FeatureMetaData[] injectWSFeatures, org.antlr.codebuff.FeatureMetaData[] alignmentFeatures, boolean computeEditDistance, String outputDir) throws Exception
		public virtual Triple<IList<Formatter>, IList<float?>, IList<float?>> validateDocuments(FeatureMetaData[] injectWSFeatures, FeatureMetaData[] alignmentFeatures, bool computeEditDistance, string outputDir)
		{
			IList<Formatter> formatters = Collections.synchronizedList(new List<Formatter>());
			IList<float?> distances = Collections.synchronizedList(new List<float?>());
			IList<float?> errors = Collections.synchronizedList(new List<float?>());
			long start = System.nanoTime();
			try
			{
				IList<string> allFiles = getFilenames(new File(rootDir), language.fileRegex);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<org.antlr.codebuff.InputDocument> documents = org.antlr.codebuff.Tool.load(allFiles, language);
				IList<InputDocument> documents = Tool.load(allFiles, language);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<org.antlr.codebuff.InputDocument> parsableDocuments = filter(documents, d -> d.tree!=null);
				IList<InputDocument> parsableDocuments = filter(documents, d => d.tree != null);
				long stop = System.nanoTime();
				Console.Write("Load/parse all docs from {0} time {1:D} ms\n", rootDir, (stop - start) / 1_000_000);

				int ncpu = Runtime.Runtime.availableProcessors();
				if (FORCE_SINGLE_THREADED)
				{
					ncpu = 2;
				}
				ExecutorService pool = Executors.newFixedThreadPool(ncpu - 1);
				IList<Callable<Void>> jobs = new List<Callable<Void>>();

				for (int i = 0; i < parsableDocuments.Count; i++)
				{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String fileName = parsableDocuments.get(i).fileName;
					string fileName = parsableDocuments[i].fileName;
					Callable<Void> job = () =>
				{
					try
					{
						Triple<Formatter, float?, float?> results = validate(language, parsableDocuments, fileName, Formatter.DEFAULT_K, injectWSFeatures, alignmentFeatures, outputDir, computeEditDistance, false);
						formatters.Add(results.a);
						float editDistance = results.b;
						distances.Add(editDistance);
						float? errorRate = results.c;
						errors.Add(errorRate);
					}
					catch (Exception t)
					{
						t.printStackTrace(System.err);
					}
					return null;
				};
					jobs.Add(job);
				}

				pool.invokeAll(jobs);
				pool.shutdown();
				pool.awaitTermination(60, TimeUnit.MINUTES);
			}
			finally
			{
				long final_stop = System.nanoTime();
				double? medianTrainingTime = median(trainingTimes);
				double medianFormattingPerMS = median(formattingTokensPerMS);
				Console.Write("Total time {0:D}ms\n", (final_stop - start) / 1_000_000);
				Console.Write("Median training time {0:D}ms\n", medianTrainingTime.Value);
				Console.Write("Median formatting time tokens per ms {0,5:F4}ms, min {1,5:F4} max {2,5:F4}\n", medianFormattingPerMS, BuffUtils.min(formattingTokensPerMS), BuffUtils.max(formattingTokensPerMS));
			}
			return new Triple<IList<Formatter>, IList<float?>, IList<float?>>(formatters,distances,errors);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public org.antlr.v4.runtime.misc.Triple<org.antlr.codebuff.Formatter,Nullable<float>,Nullable<float>> validate(org.antlr.codebuff.misc.LangDescriptor language, java.util.List<org.antlr.codebuff.InputDocument> documents, String fileToExclude, int k, String outputDir, boolean computeEditDistance, boolean collectAnalysis) throws Exception
		public virtual Triple<Formatter, float?, float?> validate(LangDescriptor language, IList<InputDocument> documents, string fileToExclude, int k, string outputDir, bool computeEditDistance, bool collectAnalysis)
		{
			return validate(language, documents, fileToExclude, k, Trainer.FEATURES_INJECT_WS, Trainer.FEATURES_HPOS, outputDir, computeEditDistance, collectAnalysis);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public org.antlr.v4.runtime.misc.Triple<org.antlr.codebuff.Formatter,Nullable<float>,Nullable<float>> validate(org.antlr.codebuff.misc.LangDescriptor language, java.util.List<org.antlr.codebuff.InputDocument> documents, String fileToExclude, int k, org.antlr.codebuff.FeatureMetaData[] injectWSFeatures, org.antlr.codebuff.FeatureMetaData[] alignmentFeatures, String outputDir, boolean computeEditDistance, boolean collectAnalysis) throws Exception
		public virtual Triple<Formatter, float?, float?> validate(LangDescriptor language, IList<InputDocument> documents, string fileToExclude, int k, FeatureMetaData[] injectWSFeatures, FeatureMetaData[] alignmentFeatures, string outputDir, bool computeEditDistance, bool collectAnalysis)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String path = new java.io.File(fileToExclude).getAbsolutePath();
			string path = System.IO.Path.GetFullPath(fileToExclude);
			IList<InputDocument> others = filter(documents, d => !d.fileName.Equals(path));
			IList<InputDocument> excluded = filter(documents, d => d.fileName.Equals(path));
			Debug.Assert(others.Count == documents.Count - 1);
	//		kNNClassifier.resetCache();
			if (excluded.Count == 0)
			{
				Console.Error.WriteLine("Doc not in corpus: " + path);
				return null;
			}
			InputDocument testDoc = excluded[0];
			long start = System.nanoTime();
			Corpus corpus = new Corpus(others, language);
			corpus.train();
			long stop = System.nanoTime();
			Formatter formatter = new Formatter(corpus, language.indentSize, k, injectWSFeatures, alignmentFeatures);
			InputDocument originalDoc = testDoc;
			long format_start = System.nanoTime();
			string output = formatter.format(testDoc, collectAnalysis);
			long format_stop = System.nanoTime();
			float editDistance = 0;
			if (computeEditDistance)
			{
				editDistance = normalizedLevenshteinDistance(testDoc.content, output);
			}
			ClassificationAnalysis analysis = new ClassificationAnalysis(originalDoc, formatter.AnalysisPerToken);
			Console.WriteLine(testDoc.fileName + ": edit distance = " + editDistance + ", error rate = " + analysis.ErrorRate);
			if (!string.ReferenceEquals(outputDir, null))
			{
				File dir = new File(outputDir + "/" + language.name + "/" + Tool.version);
				if (!dir.exists())
				{
					dir.mkdirs();
				}
				Utils.writeFile(dir.Path + "/" + System.IO.Path.GetFileName(testDoc.fileName), output);
			}
			long tms = (stop - start) / 1_000_000;
			long fms = (format_stop - format_start) / 1_000_000;
			trainingTimes.Add((double)tms);
			float tokensPerMS = testDoc.tokens.size() / (float) fms;
			formattingTokensPerMS.Add((double)tokensPerMS);
			Console.Write("Training time = {0:D} ms, formatting {1:D} ms, {2,5:F3} tokens/ms ({3:D} tokens)\n", tms, fms, tokensPerMS, testDoc.tokens.size());
	//		System.out.printf("classify calls %d, hits %d rate %f\n",
	//		                  kNNClassifier.nClassifyCalls, kNNClassifier.nClassifyCacheHits,
	//		                  kNNClassifier.nClassifyCacheHits/(float) kNNClassifier.nClassifyCalls);
	//		System.out.printf("kNN calls %d, hits %d rate %f\n",
	//						  kNNClassifier.nNNCalls, kNNClassifier.nNNCacheHits,
	//						  kNNClassifier.nNNCacheHits/(float) kNNClassifier.nNNCalls);
			return new Triple<Formatter, float?, float?>(formatter, editDistance, analysis.ErrorRate);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static String testAllLanguages(org.antlr.codebuff.misc.LangDescriptor[] languages, String[] corpusDirs, String imageFileName) throws Exception
		public static string testAllLanguages(LangDescriptor[] languages, string[] corpusDirs, string imageFileName)
		{
			IList<string> languageNames = map(languages, l => l.name + "_err");
	//		Collections.sort(languageNames);
			IDictionary<string, int?> corpusSizes = new Dictionary<string, int?>();
			for (int i = 0; i < languages.Length; i++)
			{
				LangDescriptor language = languages[i];
				IList<string> filenames = Tool.getFilenames(new File(corpusDirs[i]), language.fileRegex);
				corpusSizes[language.name] = filenames.Count;
			}
			IList<string> languageNamesAsStr = map(languages, l => '"' + l.name + "\\nn=" + corpusSizes[l.name] + '"');
	//		Collections.sort(languageNamesAsStr);

			StringBuilder data = new StringBuilder();
			for (int i = 0; i < languages.Length; i++)
			{
				LangDescriptor language = languages[i];
				string corpus = corpusDirs[i];
				LeaveOneOutValidator validator = new LeaveOneOutValidator(corpus, language);
				Triple<IList<Formatter>, IList<float?>, IList<float?>> results = validator.validateDocuments(true, "/tmp");
				IList<Formatter> formatters = results.a;
				IList<float?> distances = results.b;
				IList<float?> errors = results.c;
	//			data.append(language.name+"_dist = "+distances+"\n");
				data.Append(language.name + "_err = " + errors + "\n");
			}

			string python = "#\n" + "# AUTO-GENERATED FILE. DO NOT EDIT\n" + "# CodeBuff %s '%s'\n" + "#\n" + "import numpy as np\n" + "import pylab\n" + "import matplotlib.pyplot as plt\n\n" + "%s\n" + "language_data = %s\n" + "labels = %s\n" + "fig = plt.figure()\n" + "ax = plt.subplot(111)\n" + "ax.boxplot(language_data,\n" + "           whis=[10, 90], # 10 and 90 %% whiskers\n" + "           widths=.35,\n" + "           labels=labels,\n" + "           showfliers=False)\n" + "ax.set_xticklabels(labels, rotation=60, fontsize=18)\n" + "ax.tick_params(axis='both', which='major', labelsize=18)\n" + "plt.xticks(range(1,len(labels)+1), labels, rotation=60, fontsize=18)\n" + "pylab.ylim([0,.28])\n" + "ax.yaxis.grid(True, linestyle='-', which='major', color='lightgrey', alpha=0.5)\n" + "ax.set_xlabel(\"Grammar and corpus size\", fontsize=20)\n" + "ax.set_ylabel(\"Misclassification Error Rate\", fontsize=20)\n" + "# ax.set_title(\"Leave-one-out Validation Using Error Rate\\nBetween Formatted and Original File\")\n" + "plt.tight_layout()\n" + "fig.savefig('images/%s', format='pdf')\n" + "plt.show()\n";
			return string.format(python, Tool.version, DateTime.Now, data, languageNames, languageNamesAsStr, imageFileName);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws Exception
		public static void Main(string[] args)
		{
			LangDescriptor[] languages = new LangDescriptor[] {JAVA_DESCR, JAVA8_DESCR, JAVA_GUAVA_DESCR, JAVA8_GUAVA_DESCR, ANTLR4_DESCR, SQLITE_CLEAN_DESCR, TSQL_CLEAN_DESCR, SQLITE_NOISY_DESCR, TSQL_NOISY_DESCR};
			IList<string> corpusDirs = map(languages, l => l.corpusDir);
			string[] dirs = corpusDirs.ToArray();
			string python = testAllLanguages(languages, dirs, "leave_one_out.pdf");
			string fileName = "python/src/leave_one_out.py";
			Utils.writeFile(fileName, python);
			Console.WriteLine("wrote python code to " + fileName);
		}
	}

}