using System;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;

namespace org.antlr.codebuff.validation
{

	using BuffUtils = org.antlr.codebuff.misc.BuffUtils;
	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	//using Pair = Antlr4.Runtime.Misc.Pair;

	/// <summary>
	/// Test the speed of loading (parsing), training on corpus - doc, and formatting one doc.
	/// 
	///  Sample runs:
	/// 
	///      -antlr corpus/antlr4/training/Java8.g4
	///      -java_guava corpus/java/training/guava/cache/LocalCache.java
	///      -java8_guava corpus/java/training/guava/cache/LocalCache.java
	/// </summary>

	public class Speed
	{
		public const int TRIALS = 20;

        public static void Main(string[] args)
		{
			string langname = args[0].Substring(1);
			string testFilename = args[1];
			LangDescriptor language = null;
			for (int i = 0; i < languages.length; i++)
			{
				if (languages[i].name.Equals(langname))
				{
					language = languages[i];
					break;
				}
			}
			if (language == null)
			{
				Console.Error.WriteLine("Language " + langname + " unknown");
				return;
			}

			// load all files up front
			DateTime load_start = System.DateTime.Now;
			IList<string> allFiles = Tool.getFilenames(language.corpusDir, language.fileRegex);
			IList<InputDocument> documents = Tool.load(allFiles, language);
            DateTime load_stop = System.DateTime.Now;
            DateTime load_time = (load_stop - load_start) / 1000000;
			Console.Write("Loaded {0:D} files in {1:D}ms\n", documents.Count, load_time);

			string path = System.IO.Path.GetFullPath(testFilename);
			IList<InputDocument> others = BuffUtils.filter(documents, d => !d.fileName.Equals(path));
			IList<InputDocument> excluded = BuffUtils.filter(documents, d => d.fileName.Equals(path));
			Debug.Assert(others.Count == documents.Count - 1);
			if (excluded.Count == 0)
			{
				Console.Error.WriteLine("Doc not in corpus: " + path);
				return;
			}
			InputDocument testDoc = excluded[0];

			IList<int> training = new List<int>();
			IList<int> formatting = new List<int>();
			for (int i = 1; i <= TRIALS; i++)
			{
                org.antlr.codebuff.misc.Pair<int, int> timing = test(language, others, testDoc);
				training.Add(timing.a);
				formatting.Add(timing.b);
			}
			// drop first four
			training = training.subList(5,training.Count);
			formatting = formatting.subList(5,formatting.Count);
			Console.Write("median of [5:{0:D}] training {1:D}ms\n", TRIALS - 1, BuffUtils.median(training));
			Console.Write("median of [5:{0:D}] formatting {1:D}ms\n", TRIALS - 1, BuffUtils.median(formatting));
		}

		public static org.antlr.codebuff.misc.Pair<int, int> test(LangDescriptor language, IList<InputDocument> others, InputDocument testDoc)
		{
			var train_start = System.DateTime.Now;
			Corpus corpus = new Corpus(others, language);
			corpus.train();
            var train_stop = System.DateTime.Now;

            var format_start = System.DateTime.Now;
			Formatter formatter = new Formatter(corpus, language.indentSize, Formatter.DEFAULT_K, FEATURES_INJECT_WS, FEATURES_HPOS);
			formatter.format(testDoc, false);
			var format_stop = System.DateTime.Now;

			var train_time = (train_stop - train_start) / 1000000;
			var format_time = (format_stop - format_start) / 1000000;

			Console.Write("{0} training of {1} = {2:D}ms formatting = {3:D}ms\n", language.name, testDoc.fileName, train_time, format_time);

			return new org.antlr.codebuff.misc.Pair<int, int>((int)train_time, (int)format_time);
		}
	}

}