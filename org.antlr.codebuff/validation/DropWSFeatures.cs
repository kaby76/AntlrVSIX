using System;
using System.Collections.Generic;
using System.Linq;
using org.antlr.codebuff.misc;

namespace org.antlr.codebuff.validation
{

	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	//using Triple = Antlr4.Runtime.Misc.Triple;
	using Utils = Antlr4.Runtime.Misc.Utils;
	//using ST = org.stringtemplate.v4.ST;

	public class DropWSFeatures
	{
		public static void Main(string[] args)
		{
			LangDescriptor[] languages = new LangDescriptor[] { Tool.ANTLR4_DESCR };
			testFeatures(languages, false);
		}

		public static void testFeatures(LangDescriptor[] languages, bool includeAllFeatures)
		{
			IDictionary<string, IDictionary<string, float>> langToFeatureMedians = new Dictionary<string, IDictionary<string, float>>();

			FeatureMetaData[] whichFeatures = Trainer.FEATURES_INJECT_WS;
			if (includeAllFeatures)
			{
				whichFeatures = Trainer.FEATURES_ALL;
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
				Dictionary<string, float> featureToErrors = new Dictionary<string, float>();

				FeatureMetaData[] injectWSFeatures = deepCopy(whichFeatures);

				// do it first to get answer with curated features
				IList<float> errors = getWSErrorRates(language, injectWSFeatures, Trainer.FEATURES_HPOS);
                errors = errors.OrderBy(e => e).ToList();
                int n = errors.Count;
				float quart = errors[(int)(0.27 * n)];
				float median = errors[n / 2];
				float quart3 = errors[(int)(0.75 * n)];
				Console.WriteLine("curated error median " + median);
				featureToErrors["curated"] = median;

				// do it again to get answer with all features if they want
				if (includeAllFeatures)
				{
					errors = getWSErrorRates(language, Trainer.FEATURES_ALL, Trainer.FEATURES_HPOS);
                    errors = errors.OrderBy(e => e).ToList();
                    n = errors.Count;
					median = errors[n / 2];
					Console.WriteLine("all-in error median " + median);
					featureToErrors["all-in"] = median;
				}

				foreach (FeatureMetaData feature in injectWSFeatures)
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

					errors = getWSErrorRates(language, injectWSFeatures, Trainer.FEATURES_HPOS);
                    errors = errors.OrderBy(e => e).ToList();
                    n = errors.Count;
					median = errors[n / 2];
					featureToErrors[name] = median;
					Console.WriteLine("median error rates " + median);

					// reset feature
					feature.mismatchCost = saveCost;
				}
				Console.WriteLine(featureToErrors);
				langToFeatureMedians[language.name] = featureToErrors;
			}

			string python = "#\n" + "# AUTO-GENERATED FILE. DO NOT EDIT\n" + "# CodeBuff <version> '<date>'\n" + "#\n" + "import numpy as np\n" + "import matplotlib.pyplot as plt\n\n" + "fig = plt.figure()\n" + "ax = plt.subplot(111)\n" + "N = <numFeatures>\n" + "featureIndexes = range(0,N)\n" + "<langToMedians:{r |\n" + "<r> = [<langToMedians.(r); separator={, }>]\n" + "ax.plot(featureIndexes, <r>, label=\"<r>\")\n" + "}>\n" + "labels = [<labels:{l | '<l>'}; separator={, }>]\n" + "ax.set_xticklabels(labels, rotation=60, fontsize=8)\n" + "plt.xticks(featureIndexes, labels, rotation=60)\n" + "ax.yaxis.grid(True, linestyle='-', which='major', color='lightgrey', alpha=0.5)\n\n" + "ax.set_xlabel(\"Inject Whitespace Feature\")\n" + "ax.set_ylabel(\"Median Error rate\")\n" + "ax.set_title(\"Effect of Dropping One Feature on Whitespace Decision\\nMedian Leave-one-out Validation Error Rate\")\n" + "plt.legend()\n" + "plt.tight_layout()\n" + "fig.savefig(\"images/drop_one_ws_feature" + (includeAllFeatures?"_from_all":"") + ".pdf\", format='pdf')\n" + "plt.show()\n";
			ST pythonST = new ST(python);
			IDictionary<string, ICollection<float?>> langToMedians = new Dictionary<string, ICollection<float?>>();
			int numFeatures = 0;
			foreach (string s in langToFeatureMedians.Keys)
			{
				IDictionary<string, float> featureToErrors = langToFeatureMedians[s];
				langToMedians[s] = featureToErrors.Values;
				numFeatures = featureToErrors.Values.Count;
			}
			pythonST.add("langToMedians", langToMedians);
			pythonST.add("version", Tool.version);
			pythonST.add("date", DateTime.Now);
			pythonST.add("numFeatures", numFeatures);
			pythonST.add("labels", labels);

			string code = pythonST.render();

			string fileName = "python/src/drop_one_ws_feature.py";
			if (includeAllFeatures)
			{
				fileName = "python/src/drop_one_ws_feature_from_all.py";
			}
			org.antlr.codebuff.misc.Utils.writeFile(fileName, code);
			Console.WriteLine("wrote python code to " + fileName);
		}

		private static IList<float> getWSErrorRates(LangDescriptor language, FeatureMetaData[] injectWSFeatures, FeatureMetaData[] alignmentFeatures)
		{
			LeaveOneOutValidator validator = new LeaveOneOutValidator(language.corpusDir, language);
			Triple<IList<Formatter>, IList<float>, IList<float>> results = validator.validateDocuments(injectWSFeatures, alignmentFeatures, false, null);
			IList<Formatter> formatters = results.a;
			IList<float> wsErrorRates = new List<float>(); // don't include align errors
			foreach (Formatter formatter in formatters)
			{
				ClassificationAnalysis analysis = new ClassificationAnalysis(formatter.testDoc, formatter.AnalysisPerToken);
				wsErrorRates.Add(analysis.WSErrorRate);
			}
	//		System.out.println(results.c);
	//		System.out.println("vs");
	//		System.out.println(wsErrorRates);
			return wsErrorRates;
		}

	    private static FeatureMetaData[] deepCopy(FeatureMetaData[] features)
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