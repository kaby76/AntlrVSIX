using System;
using System.Collections.Generic;

namespace org.antlr.codebuff.validation
{

	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	using Triple = Antlr4.Runtime.Misc.Triple;
	using Utils = Antlr4.Runtime.Misc.Utils;
	using ST = org.stringtemplate.v4.ST;


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
//	import static org.antlr.codebuff.Tool.TSQL_CLEAN_DESCR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Tool.version;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Trainer.FEATURES_ALL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Trainer.FEATURES_HPOS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.antlr.codebuff.Trainer.FEATURES_INJECT_WS;

	// hideous cut/paste from WS version
	public class DropAlignFeatures
	{
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws Exception
		public static void Main(string[] args)
		{
			LangDescriptor[] languages = new LangDescriptor[] {QUORUM_DESCR, JAVA_DESCR, JAVA8_DESCR, ANTLR4_DESCR, SQLITE_CLEAN_DESCR, TSQL_CLEAN_DESCR};
			testFeatures(languages, false);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void testFeatures(org.antlr.codebuff.misc.LangDescriptor[] languages, boolean includeAllFeatures) throws Exception
		public static void testFeatures(LangDescriptor[] languages, bool includeAllFeatures)
		{
			IDictionary<string, IDictionary<string, float?>> langToFeatureMedians = new Dictionary<string, IDictionary<string, float?>>();

			FeatureMetaData[] whichFeatures = FEATURES_HPOS;
			if (includeAllFeatures)
			{
				whichFeatures = FEATURES_ALL;
			}

			IList<string> labels = new List<string>();
			labels.Add("curated");
			if (includeAllFeatures)
			{
				labels.Add("all-in");
			}
			foreach (FeatureMetaData f in whichFeatures)
			{
				if (f == FeatureMetaData.UNUSED || f.type.ToString().StartsWith("INFO_", StringComparison.Ordinal))
				{
					continue;
				}
				labels.Add(Utils.join(f.abbrevHeaderRows, " "));
			}

			foreach (LangDescriptor language in languages)
			{
				Console.WriteLine("###### " + language.name);
				IDictionary<string, float?> featureToErrors = new LinkedHashMap<string, float?>();

				FeatureMetaData[] alignFeatures = deepCopy(whichFeatures);

				// do it first to get answer with curated features
				IList<float?> errors = getAlignmentErrorRates(language, FEATURES_INJECT_WS, alignFeatures);
				errors.Sort();
				int n = errors.Count;
				float quart = errors[(int)(0.27 * n)].Value;
				float median = errors[n / 2].Value;
				float quart3 = errors[(int)(0.75 * n)].Value;
				Console.WriteLine("curated error median " + median);
				featureToErrors["curated"] = median;

				// do it again to get answer with all features if they want
				if (includeAllFeatures)
				{
					errors = getAlignmentErrorRates(language, FEATURES_INJECT_WS, FEATURES_ALL);
					errors.Sort();
					n = errors.Count;
					median = errors[n / 2].Value;
					Console.WriteLine("all-in error median " + median);
					featureToErrors["all-in"] = median;
				}

				foreach (FeatureMetaData feature in alignFeatures)
				{
					if (feature == FeatureMetaData.UNUSED || feature.type.ToString().StartsWith("INFO_", StringComparison.Ordinal))
					{
						continue;
					}
					string name = Utils.join(feature.abbrevHeaderRows, " ");
					labels.Add(name.Trim());
					Console.WriteLine("wack " + name);
					double saveCost = feature.mismatchCost;
					feature.mismatchCost = 0; // wack this feature

					errors = getAlignmentErrorRates(language, FEATURES_INJECT_WS, alignFeatures);
					errors.Sort();
					n = errors.Count;
					median = errors[n / 2].Value;
					featureToErrors[name] = median;
					Console.WriteLine("median error rates " + median);

					// reset feature
					feature.mismatchCost = saveCost;
				}
				Console.WriteLine(featureToErrors);
				langToFeatureMedians[language.name] = featureToErrors;
			}

			string python = "#\n" + "# AUTO-GENERATED FILE. DO NOT EDIT\n" + "# CodeBuff <version> '<date>'\n" + "#\n" + "import numpy as np\n" + "import matplotlib.pyplot as plt\n\n" + "fig = plt.figure()\n" + "ax = plt.subplot(111)\n" + "N = <numFeatures>\n" + "featureIndexes = range(0,N)\n" + "<langToMedians:{r |\n" + "<r> = [<langToMedians.(r); separator={, }>]\n" + "ax.plot(featureIndexes, <r>, label=\"<r>\")\n" + "}>\n" + "labels = [<labels:{l | '<l>'}; separator={, }>]\n" + "ax.set_xticklabels(labels, rotation=60, fontsize=8)\n" + "plt.xticks(featureIndexes, labels, rotation=60)\n" + "ax.yaxis.grid(True, linestyle='-', which='major', color='lightgrey', alpha=0.5)\n\n" + "ax.set_xlabel(\"Alignment Feature\")\n" + "ax.set_ylabel(\"Median Error rate\")\n" + "ax.set_title(\"Effect of Dropping One Feature on Alignment Decision\\nMedian Leave-one-out Validation Error Rate\")\n" + "plt.legend()\n" + "plt.tight_layout()\n" + "fig.savefig(\"images/drop_one_align_feature" + (includeAllFeatures?"_from_all":"") + ".pdf\", format='pdf')\n" + "plt.show()\n";
			string fileName = "python/src/drop_one_align_feature.py";
			if (includeAllFeatures)
			{
				fileName = "python/src/drop_one_align_feature_from_all.py";
			}
			ST pythonST = new ST(python);
			IDictionary<string, ICollection<float?>> langToMedians = new Dictionary<string, ICollection<float?>>();
			int numFeatures = 0;
			foreach (string s in langToFeatureMedians.Keys)
			{
				IDictionary<string, float?> featureToErrors = langToFeatureMedians[s];
				langToMedians[s] = featureToErrors.Values;
				numFeatures = featureToErrors.Values.Count;
			}
			pythonST.add("langToMedians", langToMedians);
			pythonST.add("version", version);
			pythonST.add("date", DateTime.Now);
			pythonST.add("numFeatures", numFeatures);
			pythonST.add("labels", labels);

			string code = pythonST.render();

			Utils.writeFile(fileName, code);
			Console.WriteLine("wrote python code to " + fileName);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static java.util.List<Nullable<float>> getAlignmentErrorRates(org.antlr.codebuff.misc.LangDescriptor language, org.antlr.codebuff.FeatureMetaData[] injectWSFeatures, org.antlr.codebuff.FeatureMetaData[] alignmentFeatures) throws Exception
		public static IList<float?> getAlignmentErrorRates(LangDescriptor language, FeatureMetaData[] injectWSFeatures, FeatureMetaData[] alignmentFeatures)
		{
			LeaveOneOutValidator validator = new LeaveOneOutValidator(language.corpusDir, language);
			Triple<IList<Formatter>, IList<float?>, IList<float?>> results = validator.validateDocuments(injectWSFeatures, alignmentFeatures, false, null);
			IList<Formatter> formatters = results.a;
			IList<float?> alignErrorRates = new List<float?>(); // don't include align errors
			foreach (Formatter formatter in formatters)
			{
				ClassificationAnalysis analysis = new ClassificationAnalysis(formatter.testDoc, formatter.AnalysisPerToken);
				alignErrorRates.Add(analysis.AlignmentErrorRate);
			}
	//		System.out.println(results.c);
	//		System.out.println("vs");
	//		System.out.println(alignErrorRates);
			return alignErrorRates;
		}

		public static FeatureMetaData[] deepCopy(FeatureMetaData[] features)
		{
			FeatureMetaData[] dup = new FeatureMetaData[features.Length];
			for (int i = 0; i < dup.Length; i++)
			{
				if (features[i] == FeatureMetaData.UNUSED)
				{
					dup[i] = FeatureMetaData.UNUSED;
				}
				else
				{
					dup[i] = new FeatureMetaData(features[i]);
				}
			}
			return dup;
		}
	}

}