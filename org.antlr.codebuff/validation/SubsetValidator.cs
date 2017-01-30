using System;
using System.Collections.Generic;
using System.Linq;
using org.antlr.codebuff.misc;

namespace org.antlr.codebuff.validation
{

    using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
    using Utils = Antlr4.Runtime.Misc.Utils;
    //using ST = org.stringtemplate.v4.ST;
    using Antlr4.Runtime.Misc;

    public class SubsetValidator
	{
		public const int DOCLIST_RANDOM_SEED = 777111333; // need randomness but use same seed to get reproducibility
		public const bool FORCE_SINGLE_THREADED = false;

		public const string outputDir = "/tmp";

		internal static readonly Random random = new Random();
		static SubsetValidator()
		{
			random = new Random(DOCLIST_RANDOM_SEED);
		}

		public string rootDir;
		public LangDescriptor language;
		internal IList<string> allFiles;

		public SubsetValidator(string rootDir, LangDescriptor language)
		{
			this.rootDir = rootDir;
			this.language = language;
			allFiles = Tool.getFilenames(rootDir, language.fileRegex);
		}

		public static void Main(string[] args)
		{
			LangDescriptor[] languages = new LangDescriptor[] { Tool.ANTLR4_DESCR };

			int maxNumFiles = 30;
			int trials = 50;
			IDictionary<string, float[]> results = new Dictionary<string, float[]>();
			foreach (LangDescriptor language in languages)
			{
				float[] medians = getMedianErrorRates(language, maxNumFiles, trials);
				results[language.name] = medians;
			}
			string python = "#\n" + "# AUTO-GENERATED FILE. DO NOT EDIT\n" + "# CodeBuff <version> '<date>'\n" + "#\n" + "import numpy as np\n" + "import matplotlib.pyplot as plt\n\n" + "fig = plt.figure()\n" + "ax = plt.subplot(111)\n" + "N = <maxNumFiles>\n" + "sizes = range(1,N+1)\n" + "<results:{r |\n" + "<r> = [<rest(results.(r)); separator={,}>]\n" + "ax.plot(range(1,len(<r>)+1), <r>, label=\"<r>\", marker='<markers.(r)>', color='<colors.(r)>')\n" + "}>\n" + "ax.yaxis.grid(True, linestyle='-', which='major', color='lightgrey', alpha=0.5)\n" + "ax.set_xlabel(\"Number of training files in sample corpus subset\", fontsize=14)\n" + "ax.set_ylabel(\"Median Error rate for <trials> trials\", fontsize=14)\n" + "ax.set_title(\"Effect of Corpus size on Median Leave-one-out Validation Error Rate\")\n" + "plt.legend()\n" + "plt.tight_layout()\n" + "fig.savefig('images/subset_validator.pdf', format='pdf')\n" + "plt.show()\n";
			ST pythonST = new ST(python);
			pythonST.add("results", results);
			pythonST.add("markers", LeaveOneOutValidator.nameToGraphMarker);
			pythonST.add("colors", LeaveOneOutValidator.nameToGraphColor);
			pythonST.add("version", version);
			pythonST.add("date", DateTime.Now);
			pythonST.add("trials", trials);
			pythonST.add("maxNumFiles", maxNumFiles);
			IList<string> corpusDirs = map(languages, l => l.corpusDir);
			string[] dirs = corpusDirs.ToArray();
			string fileName = "python/src/subset_validator.py";
            org.antlr.codebuff.misc.Utils.writeFile(fileName, pythonST.render());
			Console.WriteLine("wrote python code to " + fileName);
		}

		public static float[] getMedianErrorRates(LangDescriptor language, int maxNumFiles, int trials)
		{
			SubsetValidator validator = new SubsetValidator(language.corpusDir, language);
			IList<InputDocument> documents = Tool.load(validator.allFiles, language);
			float[] medians = new float[Math.Min(documents.Count,maxNumFiles) + 1];

			int ncpu = Runtime.Runtime.availableProcessors();
			if (FORCE_SINGLE_THREADED)
			{
				ncpu = 2;
			}
			ExecutorService pool = Executors.newFixedThreadPool(ncpu - 1);
			IList<Callable<Void>> jobs = new List<Callable<Void>>();

			for (int i = 1; i <= Math.Min(validator.allFiles.Count, maxNumFiles); i++)
			{ // i is corpus subset size
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int corpusSubsetSize = i;
				int corpusSubsetSize = i;
				Callable<Void> job = () =>
			{
				try
				{
					IList<float?> errorRates = new List<float?>();
					for (int trial = 1; trial <= trials; trial++)
					{ // multiple trials per subset size
                        org.antlr.codebuff.misc.Pair<InputDocument, IList<InputDocument>> sample = validator.selectSample(documents, corpusSubsetSize);
						Triple<Formatter, float?, float?> results = validate(language, sample.b, sample.a, true, false);
//					System.out.println(sample.a.fileName+" n="+corpusSubsetSize+": error="+results.c);
//				System.out.println("\tcorpus =\n\t\t"+Utils.join(sample.b.iterator(), "\n\t\t"));
						errorRates.Add(results.c);
					}
					errorRates.Sort();
					int n = errorRates.Count;
					float median = errorRates[n / 2].Value;
					Console.WriteLine("median " + language.name + " error rate for n=" + corpusSubsetSize + " is " + median);
					medians[corpusSubsetSize] = median;
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
			bool terminated = pool.awaitTermination(60, TimeUnit.MINUTES);
			return medians;
		}

		/// <summary>
		/// Select one document at random, then n others w/o replacement as corpus </summary>
		public virtual org.antlr.codebuff.misc.Pair<InputDocument, IList<InputDocument>> selectSample(IList<InputDocument> documents, int n)
		{
			int i = random.Next(documents.Count);
			InputDocument testDoc = documents[i];
			IList<InputDocument> others = BuffUtils.filter(documents, d => d != testDoc);
			IList<InputDocument> corpusSubset = getRandomDocuments(others, n);
			return new org.antlr.codebuff.misc.Pair<InputDocument, IList<InputDocument>>(testDoc, corpusSubset);
		}

		public static Triple<Formatter, float, float> validate(LangDescriptor language, IList<InputDocument> documents, InputDocument testDoc, bool saveOutput, bool computeEditDistance)
		{
	//		kNNClassifier.resetCache();
			Corpus corpus = new Corpus(documents, language);
			corpus.train();
	//		System.out.printf("%d feature vectors\n", corpus.featureVectors.size());
			Formatter formatter = new Formatter(corpus, language.indentSize);
			string output = formatter.format(testDoc, false);
			float editDistance = 0;
			if (computeEditDistance)
			{
				editDistance = Dbg.normalizedLevenshteinDistance(testDoc.content, output);
			}
			ClassificationAnalysis analysis = new ClassificationAnalysis(testDoc, formatter.AnalysisPerToken);
	//		System.out.println(testDoc.fileName+": edit distance = "+editDistance+", error rate = "+analysis.getErrorRate());
			if (saveOutput)
			{
				File dir = new File(outputDir + "/" + language.name);
				if (saveOutput)
				{
					dir = new File(outputDir + "/" + language.name);
					dir.mkdir();
				}
                org.antlr.codebuff.misc.Utils.writeFile(dir.Path + "/" + System.IO.Path.GetFileName(testDoc.fileName), output);
			}
			return new Triple<Formatter, float?, float?>(formatter, editDistance, analysis.ErrorRate);
		}


		/// <summary>
		/// From input documents, grab n in random order w/o replacement </summary>
		public static IList<InputDocument> getRandomDocuments(IList<InputDocument> documents, int n)
		{
			IList<InputDocument> documents_ = new List<InputDocument>(documents);
			Collections.shuffle(documents_, random);
			IList<InputDocument> contentList = new List<InputDocument>(n);
			// get first n files from shuffle and set file index for it
			for (int i = 0; i < Math.Min(documents_.Count,n); i++)
			{
				contentList.Add(documents_[i]);
			}
			return contentList;
		}

		/// <summary>
		/// From input documents, grab n in random order w replacement </summary>
		public static IList<InputDocument> getRandomDocumentsWithRepl(IList<InputDocument> documents, int n)
		{
			IList<InputDocument> contentList = new List<InputDocument>(n);
			for (int i = 1; i <= n; i++)
			{
				int r = random.Next(documents.Count); // get random index from 0..|inputfiles|-1
				contentList.Add(documents[r]);
			}
			return contentList;
		}
	}

}