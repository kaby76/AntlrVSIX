using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace org.antlr.codebuff.validation
{

	using BuffUtils = org.antlr.codebuff.misc.BuffUtils;
	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	using Pair = Antlr4.Runtime.Misc.Pair;


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.getFilenames;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.languages;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Trainer.FEATURES_HPOS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Trainer.FEATURES_INJECT_WS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.misc.BuffUtils.filter;

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
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws Exception
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
			long load_start = System.nanoTime();
			IList<string> allFiles = getFilenames(new File(language.corpusDir), language.fileRegex);
			IList<InputDocument> documents = Tool.load(allFiles, language);
			long load_stop = System.nanoTime();
			long load_time = (load_stop - load_start) / 1_000_000;
			Console.Write("Loaded {0:D} files in {1:D}ms\n", documents.Count, load_time);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String path = new java.io.File(testFilename).getAbsolutePath();
			string path = System.IO.Path.GetFullPath(testFilename);
			IList<InputDocument> others = filter(documents, d => !d.fileName.Equals(path));
			IList<InputDocument> excluded = filter(documents, d => d.fileName.Equals(path));
			Debug.Assert(others.Count == documents.Count - 1);
			if (excluded.Count == 0)
			{
				Console.Error.WriteLine("Doc not in corpus: " + path);
				return;
			}
			InputDocument testDoc = excluded[0];

			IList<int?> training = new List<int?>();
			IList<int?> formatting = new List<int?>();
			for (int i = 1; i <= TRIALS; i++)
			{
				Pair<int?, int?> timing = test(language, others, testDoc);
				training.Add(timing.a);
				formatting.Add(timing.b);
			}
			// drop first four
			training = training.subList(5,training.Count);
			formatting = formatting.subList(5,formatting.Count);
			Console.Write("median of [5:{0:D}] training {1:D}ms\n", TRIALS - 1, BuffUtils.median(training));
			Console.Write("median of [5:{0:D}] formatting {1:D}ms\n", TRIALS - 1, BuffUtils.median(formatting));
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.antlr.v4.runtime.misc.Pair<Nullable<int>,Nullable<int>> test(org.antlr.codebuff.misc.LangDescriptor language, java.util.List<org.antlr.codebuff.InputDocument> others, org.antlr.codebuff.InputDocument testDoc) throws Exception
		public static Pair<int?, int?> test(LangDescriptor language, IList<InputDocument> others, InputDocument testDoc)
		{
			long train_start = System.nanoTime();
			Corpus corpus = new Corpus(others, language);
			corpus.train();
			long train_stop = System.nanoTime();

			long format_start = System.nanoTime();
			Formatter formatter = new Formatter(corpus, language.indentSize, Formatter.DEFAULT_K, FEATURES_INJECT_WS, FEATURES_HPOS);
			formatter.format(testDoc, false);
			long format_stop = System.nanoTime();

			long train_time = (train_stop - train_start) / 1_000_000;
			long format_time = (format_stop - format_start) / 1_000_000;

			Console.Write("{0} training of {1} = {2:D}ms formatting = {3:D}ms\n", language.name, testDoc.fileName, train_time, format_time);

			return new Pair<int?, int?>((int)train_time, (int)format_time);
		}
	}

}