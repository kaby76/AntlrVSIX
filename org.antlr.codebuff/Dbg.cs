using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using org.antlr.codebuff.misc;

namespace org.antlr.codebuff
{

	//using GUIController = org.antlr.codebuff.gui.GUIController;
	using CodeBuffTokenStream = org.antlr.codebuff.misc.CodeBuffTokenStream;
	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	using ClassificationAnalysis = org.antlr.codebuff.validation.ClassificationAnalysis;
	using LeaveOneOutValidator = org.antlr.codebuff.validation.LeaveOneOutValidator;
	using TokenPositionAnalysis = org.antlr.codebuff.validation.TokenPositionAnalysis;
	using ANTLRFileStream = Antlr4.Runtime.AntlrFileStream;
	using CommonToken = Antlr4.Runtime.CommonToken;
	using CommonTokenStream = Antlr4.Runtime.CommonTokenStream;
	using Lexer = Antlr4.Runtime.Lexer;
	using Token = Antlr4.Runtime.IToken;
	//using Pair = org.antlr.v4.runtime.misc.Pair;
	//using Triple = org.antlr.v4.runtime.misc.Triple;

	/// <summary>
	/// Grammar must have WS/comments on hidden channel
	/// 
	/// Testing:
	/// 
	/// Dbg  -antlr     corpus/antlr4/training      grammars/org/antlr/codebuff/tsql.g4
	/// Dbg  -antlr     corpus/antlr4/training      corpus/antlr4/training/MASM.g4
	/// Dbg  -quorum     corpus/quorum/training      corpus/quorum/training/Containers/List.quorum
	/// Dbg  -sqlite    corpus/sqlclean/training      corpus/sqlclean/training/dmart_bits.sql
	/// Dbg  -tsql      corpus/sqlclean/training        corpus/sqlclean/training/dmart_bits_PSQLRPT24.sql
	/// Dbg  -java_st      corpus/java/training/stringtemplate4/org/stringtemplate/v4/StringRenderer.java
	/// Dbg  -java_guava   corpus/java/training/guava/base/Absent.java
	/// </summary>
	public class Dbg
	{


		public static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.Error.WriteLine("Dbg [-leave-one-out] [-java|-java8|-antlr|-sqlite|-tsql] test-file");
			}

			int arg = 0;
			bool leaveOneOut = true;
			bool collectAnalysis = true;
			string language = args[arg++];
			language = language.Substring(1);
			string testFilename = args[arg];
			string output = "???";
			InputDocument testDoc = null;
			IList<TokenPositionAnalysis> analysisPerToken = null;
            org.antlr.codebuff.misc.Pair<string, IList<TokenPositionAnalysis>> results;
			LangDescriptor lang = null;
			System.DateTime start, stop;
			for (int i = 0; i < Tool.languages.Length; i++)
			{
				if (Tool.languages[i].name.Equals(language))
				{
					lang = Tool.languages[i];
					break;
				}
			}
			if (lang != null)
			{
				start  = System.DateTime.Now;
				LeaveOneOutValidator validator = new LeaveOneOutValidator(lang.corpusDir, lang);
				Triple<Formatter, float, float> val = validator.validateOneDocument(testFilename, null, collectAnalysis);
				testDoc = Tool.parse(testFilename, lang);
				stop = System.DateTime.Now;
				Formatter formatter = val.a;
				output = formatter.Output;
				Console.WriteLine("output len = " + output.Length);
				float editDistance = normalizedLevenshteinDistance(testDoc.content, output);
				Console.WriteLine("normalized Levenshtein distance: " + editDistance);
				analysisPerToken = formatter.AnalysisPerToken;

			    Regex rex = new Regex("^\\s+$");
				CommonTokenStream original_tokens = Tool.tokenize(testDoc.content, lang.lexerClass);
				IList<Token> wsTokens = BuffUtils.filter(original_tokens.GetTokens(), t => rex.IsMatch(t.Text));
				string originalWS = tokenText(wsTokens);
				Console.WriteLine("origin ws tokens len: " + originalWS.Length);
				CommonTokenStream formatted_tokens = Tool.tokenize(output, lang.lexerClass);
				wsTokens = BuffUtils.filter(formatted_tokens.GetTokens(), t => rex.IsMatch(t.Text));
				string formattedWS = tokenText(wsTokens);
				Console.WriteLine("formatted ws tokens len: " + formattedWS.Length);
				editDistance = levenshteinDistance(originalWS, formattedWS);
				editDistance /= Math.Max(testDoc.content.Length, output.Length);
				Console.WriteLine("Levenshtein distance of ws normalized to output len: " + editDistance);

				ClassificationAnalysis analysis = new ClassificationAnalysis(testDoc, analysisPerToken);
				Console.WriteLine(analysis);
			}

			if (lang != null)
			{
    //            GUIController controller;
    //            controller = new GUIController(analysisPerToken, testDoc, output, lang.lexerClass);
				//controller.show();
	//			System.out.println(output);
				//Console.Write("formatting time {0:D}s\n", (stop - start) / 1000000);
				Console.Write("classify calls {0:D}, hits {1:D} rate {2:F}\n", kNNClassifier.nClassifyCalls, kNNClassifier.nClassifyCacheHits, kNNClassifier.nClassifyCacheHits / (float) kNNClassifier.nClassifyCalls);
				Console.Write("kNN calls {0:D}, hits {1:D} rate {2:F}\n", kNNClassifier.nNNCalls, kNNClassifier.nNNCacheHits, kNNClassifier.nNNCacheHits / (float) kNNClassifier.nNNCalls);
			}
		}

		/// <summary>
		/// from https://en.wikipedia.org/wiki/Levenshtein_distance
		///  "It is always at least the difference of the sizes of the two strings."
		///  "It is at most the length of the longer string."
		/// </summary>
		public static float normalizedLevenshteinDistance(string s, string t)
		{
			float d = levenshteinDistance(s, t);
			int max = Math.Max(s.Length, t.Length);
			return d / (float)max;
		}

		public static float levenshteinDistance(string s, string t)
		{
			// degenerate cases
			if (s.Equals(t))
			{
				return 0;
			}
			if (s.Length == 0)
			{
				return t.Length;
			}
			if (t.Length == 0)
			{
				return s.Length;
			}

			// create two work vectors of integer distances
			int[] v0 = new int[t.Length + 1];
			int[] v1 = new int[t.Length + 1];

			// initialize v0 (the previous row of distances)
			// this row is A[0][i]: edit distance for an empty s
			// the distance is just the number of characters to delete from t
			for (int i = 0; i < v0.Length; i++)
			{
				v0[i] = i;
			}

			for (int i = 0; i < s.Length; i++)
			{
				// calculate v1 (current row distances) from the previous row v0

				// first element of v1 is A[i+1][0]
				//   edit distance is delete (i+1) chars from s to match empty t
				v1[0] = i + 1;

				// use formula to fill in the rest of the row
				for (int j = 0; j < t.Length; j++)
				{
					int cost = s[i] == t[j] ? 0 : 1;
					v1[j + 1] = Math.Min(Math.Min(v1[j] + 1, v0[j + 1] + 1), v0[j] + cost);
				}

				// copy v1 (current row) to v0 (previous row) for next iteration
				Array.Copy(v1, 0, v0, 0, v0.Length);
			}

			int d = v1[t.Length];
			return d;
		}

		/* Compare whitespace and give an approximate Levenshtein distance /
		   edit distance. MUCH faster to use this than pure Levenshtein which
		   must consider all of the "real" text that is in common.
	
			when only 1 kind of char, just substract lengths
			Orig    Altered Distance
			AB      A B     1
			AB      A  B    2
			AB      A   B   3
			A B     A  B    1
	
			A B     AB      1
			A  B    AB      2
			A   B   AB      3
	
			when ' ' and '\n', we count separately.
	
			A\nB    A B     spaces delta=1, newline delete=1, distance = 2
			A\nB    A  B    spaces delta=2, newline delete=1, distance = 3
			A\n\nB  A B     spaces delta=1, newline delete=2, distance = 3
			A\n \nB A B     spaces delta=0, newline delete=2, distance = 2
			A\n \nB A\nB    spaces delta=1, newline delete=1, distance = 2
			A \nB   A\n B   spaces delta=0, newline delete=0, distance = 0
							levenshtein would count this as 2 I think but
							for our doc distance, I think it's ok to measure as same
		 */
	//	public static int editDistance(String s, String t) {
	//	}

		/*
				A \nB   A\n B   spaces delta=0, newline delete=0, distance = 0
							levenshtein would count this as 2 I think but
							for our doc distance, I think it's ok to measure as same
		 */
		public static int whitespaceEditDistance(string s, string t)
		{
			int s_spaces = Tool.count(s, ' ');
			int s_nls = Tool.count(s, '\n');
			int t_spaces = Tool.count(t, ' ');
			int t_nls = Tool.count(t, '\n');
			return Math.Abs(s_spaces - t_spaces) + Math.Abs(s_nls - t_nls);
		}

		/// <summary>
		/// Compute a document difference metric 0-1.0 between two documents that
		///  are identical other than (likely) the whitespace and comments.
		/// 
		///  1.0 means the docs are maximally different and 0 means docs are identical.
		/// 
		///  The Levenshtein distance between the docs counts only
		///  whitespace diffs as the non-WS content is identical.
		///  Levenshtein distance is bounded by 0..max(len(doc1),len(doc2)) so
		///  we normalize the distance by dividing by max WS count.
		/// 
		///  TODO: can we simplify this to a simple walk with two
		///  cursors through the original vs formatted counting
		///  mismatched whitespace? real text are like anchors.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static double docDiff(String original, String formatted, Class lexerClass) throws Exception
		public static double docDiff(string original, string formatted, Type lexerClass)
		{
			// Grammar must strip all but real tokens and whitespace (and put that on hidden channel)
			CodeBuffTokenStream original_tokens = Tool.tokenize(original, lexerClass);
	//		String s = original_tokens.getText();
			CodeBuffTokenStream formatted_tokens = Tool.tokenize(formatted, lexerClass);
	//		String t = formatted_tokens.getText();

			// walk token streams and examine whitespace in between tokens
			int i = -1;
			int ws_distance = 0;
			int original_ws = 0;
			int formatted_ws = 0;
			while (true)
			{
				Token ot = original_tokens.LT(i); // TODO: FIX THIS! can't use LT()
				if (ot == null || ot.Type == TokenConstants.EOF)
				{
					break;
				}
				IList<Token> ows = original_tokens.GetHiddenTokensToLeft(ot.TokenIndex);
				original_ws += tokenText(ows).Length;

				Token ft = formatted_tokens.LT(i); // TODO: FIX THIS! can't use LT()
				if (ft == null || ft.Type == TokenConstants.EOF)
				{
					break;
				}
				IList<Token> fws = formatted_tokens.GetHiddenTokensToLeft(ft.TokenIndex);
				formatted_ws += tokenText(fws).Length;

				ws_distance += whitespaceEditDistance(tokenText(ows), tokenText(fws));
				i++;
			}
			// it's probably ok to ignore ws diffs after last real token

			int max_ws = Math.Max(original_ws, formatted_ws);
			double normalized_ws_distance = ((float) ws_distance) / max_ws;
			return normalized_ws_distance;
		}

		/// <summary>
		/// Compare an input document's original text with its formatted output
		///  and return the ratio of the incorrectWhiteSpaceCount to total whitespace
		///  count in the original document text. It is a measure of document
		///  similarity.
		/// </summary>
	//	public static double compare(InputDocument doc,
	//	                             String formatted,
	//	                             Class<? extends Lexer> lexerClass)
	//		throws Exception {
	//	}

		public static string tokenText(IList<Token> tokens)
		{
			return tokenText(tokens, null);
		}

		public static string tokenText(IList<Token> tokens, string separator)
		{
			if (tokens == null)
			{
				return "";
			}
			StringBuilder buf = new StringBuilder();
			bool first = true;
			foreach (Token t in tokens)
			{
				if (!string.ReferenceEquals(separator, null) && !first)
				{
					buf.Append(separator);
				}
				buf.Append(t.Text);
				first = false;
			}
			return buf.ToString();
		}

		public static void printOriginalFilePiece(InputDocument doc, CommonToken originalCurToken)
		{
			Console.WriteLine(doc.getLine(originalCurToken.Line-1));
			Console.WriteLine(doc.getLine(originalCurToken.Line));
			Console.Write(Tool.spaces(originalCurToken.Column));
			Console.WriteLine("^");
		}

		public class Foo
		{
			public static void Main(string[] args)
			{
				ANTLRv4Lexer lexer = new ANTLRv4Lexer(new ANTLRFileStream("grammars/org/antlr/codebuff/ANTLRv4Lexer.g4"));
				CommonTokenStream tokens = new CodeBuffTokenStream(lexer);
				ANTLRv4Parser parser = new ANTLRv4Parser(tokens);
				ANTLRv4Parser.GrammarSpecContext tree = parser.grammarSpec();
				Console.WriteLine(tree.ToStringTree(parser));
			}
		}
	}

}