using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using org.antlr.codebuff.misc;

namespace org.antlr.codebuff
{

    using CodeBuffTokenStream = org.antlr.codebuff.misc.CodeBuffTokenStream;
    using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
    //using ANTLRErrorListener = Antlr4.Runtime.IAntlrErrorListener;
    using ANTLRInputStream = Antlr4.Runtime.AntlrInputStream;
    using BailErrorStrategy = Antlr4.Runtime.BailErrorStrategy;
    using CharStream = Antlr4.Runtime.ICharStream;
    using CommonToken = Antlr4.Runtime.CommonToken;
    using CommonTokenStream = Antlr4.Runtime.CommonTokenStream;
    using DefaultErrorStrategy = Antlr4.Runtime.DefaultErrorStrategy;
    using Lexer = Antlr4.Runtime.Lexer;
    using Parser = Antlr4.Runtime.Parser;
    using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;
    using RecognitionException = Antlr4.Runtime.RecognitionException;
    using Recognizer = Antlr4.Runtime.IRecognizer;
    using Token = Antlr4.Runtime.IToken;
    using TokenStream = Antlr4.Runtime.ITokenStream;
    using ATNConfigSet = Antlr4.Runtime.Atn.ATNConfigSet;
    using PredictionMode = Antlr4.Runtime.Atn.PredictionMode;
    using DFA = Antlr4.Runtime.Dfa.DFA;
    //using ParseCancellationException = Antlr4.Runtime.Misc.ParseCancellationException;
    using Utils = Antlr4.Runtime.Misc.Utils;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The main CodeBuff tool used to format files. Examples:
    /// 
    ///   $ java -jar target/codebuff-1.4.19.jar  \
    ///        -g org.antlr.codebuff.ANTLRv4 -rule grammarSpec -corpus corpus/antlr4/training \
    ///        -files g4 -indent 4 -comment LINE_COMMENT T.g4
    /// 
    ///   $ java -jar codebuff-1.4.19 \
    ///        -g org.antlr.codebuff.Java -rule compilationUnit \
    ///        -corpus corpus/java/training/stringtemplate4  -files java \
    ///        -comment LINE_COMMENT T.java
    /// 
    /// You have to have some libs in your CLASSPATH. See pom.xml, but it's
    /// ANTLR 4, Apache commons-lang3, Google guava, and StringTemplate 4.
    /// 
    /// The grammar must be run through ANTLR and be compiled (and in the CLASSPATH).
    /// For Java8.g4, use "-g Java8", not the filename. For separated
    /// grammar files, like ANTLRv4Parser.g4 and ANTLRv4Lexer.g4, use "-g ANTLRv4".
    /// If the grammar is in a package, use fully-qualified like
    /// "-g org.antlr.codebuff.ANTLRv4"
    /// 
    /// Output goes to stdout if no -o option used.
    /// </summary>
    public class Tool
	{
		public static bool showFileNames = false;
		public static bool showTokens = false;

		public static readonly LangDescriptor ANTLR4_DESCR = new LangDescriptor("antlr", "corpus/antlr4/training", ".*\\.g4", typeof(ANTLRv4Lexer), typeof(ANTLRv4Parser), "grammarSpec", 4, ANTLRv4Lexer.LINE_COMMENT);

		public static LangDescriptor[] languages = new LangDescriptor[] { ANTLR4_DESCR };

		public static string version;

		static Tool()
		{
			try
			{
				Tool.setToolVersion();
			}
			catch (Exception ioe)
			{
                System.Console.WriteLine(ioe.StackTrace);
			}
		}

        public static string formatted_output = null;
        public static string unformatted_input = null;

        public static void Main(string[] args)
		{
            if (args.Length < 7)
            {
                Console.Error.WriteLine("org.antlr.codebuff.Tool -g grammar-name -rule start-rule -corpus root-dir-of-samples \\\n" + "   [-files file-extension] [-indent num-spaces] \\" + "   [-comment line-comment-name] [-o output-file] file-to-format");
                return;
            }

            formatted_output = null;
            string outputFileName = "";
            string grammarName = null;
			string startRule = null;
			string corpusDir = null;
			string indentS = "4";
			string commentS = null;
			string input_file_name = null;
			string fileExtension = null;
			int i = 0;
			while (i < args.Length && args[i].StartsWith("-", StringComparison.Ordinal))
			{
				switch (args[i])
				{
					case "-g":
						i++;
						grammarName = args[i++];
						break;
					case "-rule" :
						i++;
						startRule = args[i++];
						break;
					case "-corpus" :
						i++;
						corpusDir = args[i++];
						break;
					case "-files" :
						i++;
						fileExtension = args[i++];
						break;
					case "-indent" :
						i++;
						indentS = args[i++];
						break;
					case "-comment" :
						i++;
						commentS = args[i++];
						break;
					case "-o" :
						i++;
						outputFileName = args[i++];
						break;
                    case "-inoutstring":
                        i++;
                        formatted_output = "";
                        outputFileName = null;
                        break;
				}
			}
			input_file_name = args[i]; // must be last

			Console.WriteLine("gramm: " + grammarName);
			string parserClassName = grammarName + "Parser";
			string lexerClassName = grammarName + "Lexer";
			Type parserClass = null;
			Type lexerClass = null;
			Lexer lexer = null;
			try
			{
				parserClass = (Type)Type.GetType(parserClassName);
				lexerClass = (Type)Type.GetType(lexerClassName);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("Can't load " + parserClassName + " or maybe " + lexerClassName);
				Console.Error.WriteLine("Make sure they are generated by ANTLR, compiled, and in CLASSPATH");
                System.Console.WriteLine(e.StackTrace);
			}
			if (parserClass == null | lexerClass == null)
			{
				return; // don't return from catch!
			}
			int indentSize = int.Parse(indentS);
			int singleLineCommentType = -1;
			if (!string.ReferenceEquals(commentS, null))
			{
				try
				{
					lexer = getLexer(lexerClass, null);
				}
				catch (Exception e)
				{
					Console.Error.WriteLine("Can't instantiate lexer " + lexerClassName);
                    System.Console.WriteLine(e.StackTrace);
                }
                if (lexer == null)
				{
					return;
				}
				IDictionary<string, int> tokenTypeMap = lexer.TokenTypeMap;
				if (tokenTypeMap.ContainsKey(commentS))
				{
					singleLineCommentType = tokenTypeMap[commentS];
				}
			}
			string fileRegex = null;
			if (!string.ReferenceEquals(fileExtension, null))
			{
				fileRegex = ".*\\." + fileExtension;
			}
			LangDescriptor language = new LangDescriptor(grammarName, corpusDir, fileRegex, lexerClass, parserClass, startRule, indentSize, singleLineCommentType);

            ////////
            // load all corpus files up front
            IList<string> allFiles = getFilenames(language.corpusDir, language.fileRegex);
            IList<InputDocument> documents = load(allFiles, language);

            // Handle formatting of document if it's passed as a string or not.
            if (unformatted_input == null)
            {
                // Don't include file to format in corpus itself.
                string path = System.IO.Path.GetFullPath(input_file_name);
                IList<InputDocument> others = BuffUtils.filter(documents, d => !d.fileName.Equals(path));
                // Perform training of formatter.
                Corpus corpus = new Corpus(others, language);
                corpus.train();

                // Parse code contained in file.
                InputDocument unformatted_document = parse(input_file_name, language);

                // Format document.
                Formatter formatter = new Formatter(corpus, language.indentSize, Formatter.DEFAULT_K, Trainer.FEATURES_INJECT_WS, Trainer.FEATURES_HPOS);
                formatted_output = formatter.format(unformatted_document, false);
            }
            else
            {
                // Perform training of formatter.
                Corpus corpus = new Corpus(documents, language);
                corpus.train();

                // Parse code that was represented as a string.
                InputDocument unformatted_document = parse(input_file_name, unformatted_input, language);

                // Format document.
                Formatter formatter = new Formatter(corpus, language.indentSize, Formatter.DEFAULT_K, Trainer.FEATURES_INJECT_WS, Trainer.FEATURES_HPOS);
                formatted_output = formatter.format(unformatted_document, false);
            }
            ///////
            if (outputFileName != null && outputFileName == "")
		    {
		        System.Console.WriteLine(formatted_output);
		    }
            else if (!string.IsNullOrEmpty(outputFileName))
            {
                org.antlr.codebuff.misc.Utils.writeFile(outputFileName, formatted_output);
            }
        }

		public static void setToolVersion()
		{
			version = "v1";
		}

		public static CodeBuffTokenStream tokenize(string doc, Type lexerClass)
		{
			ANTLRInputStream input = new ANTLRInputStream(doc);
			Lexer lexer = getLexer(lexerClass, input);

			CodeBuffTokenStream tokens = new CodeBuffTokenStream(lexer);
			tokens.Fill();
			return tokens;
		}

		public static Parser getParser(Type parserClass, CommonTokenStream tokens)
		{
            object o = Activator.CreateInstance(parserClass, new object[] { tokens });
            return (Parser)o;
		}

		public static Lexer getLexer(Type lexerClass, ANTLRInputStream input)
		{
		    object o = Activator.CreateInstance(lexerClass, new object[] { input });
			return (Lexer)o;
		}

		/// <summary>
		/// Get all file contents into input doc list </summary>
		public static IList<InputDocument> load(IList<string> fileNames, LangDescriptor language)
		{
			IList<InputDocument> documents = new List<InputDocument>();
			foreach (string fileName in fileNames)
			{
				documents.Add(parse(fileName, language));
			}
			if (documents.Count > 0)
			{
				documents[0].parser.Interpreter.ClearDFA(); // free up memory
			}

			return documents;
		}

		public static string load(string fileName, int tabSize)
		{
			string path = System.IO.Path.GetFullPath(fileName);
		    string content = new StreamReader(path).ReadToEnd();
			string notabs = expandTabs(content, tabSize);
			return notabs;
		}

		/// <summary>
		/// Parse doc and fill tree and tokens fields
		/// </summary>
		public static InputDocument parse(string fileName, LangDescriptor language)
		{
			string content = load(fileName, language.indentSize);
			return parse(fileName, content, language);
		}

		public static InputDocument parse(string fileName, string content, LangDescriptor language)
		{
			ANTLRInputStream input = new ANTLRInputStream(content);
			Lexer lexer = getLexer(language.lexerClass, input);
			input.name = fileName;

			InputDocument doc = new InputDocument(fileName, content, language);

			doc.tokens = new CodeBuffTokenStream(lexer);

			doc.parser = getParser(language.parserClass, doc.tokens);
			doc.parser.BuildParseTree = true;

			// two-stage parsing. Try with SLL first
			doc.parser.Interpreter.PredictionMode = Antlr4.Runtime.Atn.PredictionMode.SLL;
			doc.parser.ErrorHandler = new BailErrorStrategy();
			doc.parser.RemoveErrorListeners();

			MethodInfo startRule = language.parserClass.GetMethod(language.startRuleName);
			try
			{
				doc.Tree = (ParserRuleContext) startRule.Invoke(doc.parser, (object[]) null);
			}
			catch (Exception ex)
			{
				if (ex.InnerException is ParseCanceledException)
				{
					doc.parser.Reset();
					doc.tokens.Reset(); // rewind input stream
					// back to standard listeners/handlers
					doc.parser.AddErrorListener(new ANTLRErrorListenerAnonymousInnerClass());
					doc.parser.ErrorHandler = new DefaultErrorStrategy();
					doc.parser.Interpreter.PredictionMode = PredictionMode.LL;
					doc.Tree = (ParserRuleContext) startRule.Invoke(doc.parser, (object[]) null);
					if (doc.parser.NumberOfSyntaxErrors > 0)
					{
						doc.Tree = null;
					}
				}
			}

			return doc;
		}

		private class ANTLRErrorListenerAnonymousInnerClass : Antlr4.Runtime.IAntlrErrorListener<IToken>
		{
			public ANTLRErrorListenerAnonymousInnerClass()
			{
			}

		    public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
		        RecognitionException e)
		    {
                Console.Error.WriteLine(recognizer.InputStream.SourceName + " line " + line + ":" + charPositionInLine + " " + msg);
            }
        }

		public static IList<string> getFilenames(string dir_or_file, string inputFilePattern)
		{
			IList<string> files = new List<string>();
			getFilenames_(dir_or_file, inputFilePattern, files);
			return files;
		}

		public static void getFilenames_(string dir_or_file, string inputFilePattern, IList<string> files)
		{
            // If this is a directory, walk each file/dir in that directory
            FileAttributes attr = File.GetAttributes(dir_or_file);

            //detect whether its a directory or file
            if (attr.HasFlag(FileAttributes.Directory))
            {
                IEnumerable<string> dlist = Directory.EnumerateDirectories(dir_or_file);
                foreach (string d in dlist)
                {
                    getFilenames_(d, inputFilePattern, files);
                }
                var flist = Directory.EnumerateFiles(dir_or_file);
                foreach (string f in flist)
                {
                    getFilenames_(f, inputFilePattern, files);
                }
            }
            else if ( inputFilePattern == null || new Regex(inputFilePattern).IsMatch(dir_or_file))
            {
                string p = Path.GetFullPath(dir_or_file);
                files.Add(p);
			}
		}

		public static string join(int[] array, string separator)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				builder.Append(array[i]);
				if (i < array.Length - 1)
				{
					builder.Append(separator);
				}
			}

			return builder.ToString();
		}

		public static string join(string[] array, string separator)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				builder.Append(array[i]);
				if (i < array.Length - 1)
				{
					builder.Append(separator);
				}
			}

			return builder.ToString();
		}

		public static IList<CommonToken> copy(CommonTokenStream tokens)
		{
			IList<CommonToken> copy = new List<CommonToken>();
		    tokens.Fill();
			foreach (Token t in tokens.GetTokens())
			{
				copy.Add(new CommonToken(t));
			}
			return copy;
		}

		public static int L0_Distance(bool[] categorical, int[] A, int[] B)
		{
			int count = 0; // count how many mismatched categories there are
			for (int i = 0; i < A.Length; i++)
			{
				if (categorical[i])
				{
					if (A[i] != B[i])
					{
						count++;
					}
				}
			}
			return count;
		}

		/// <summary>
		/// A distance of 0 should count much more than non-0. Also, penalize
		///  mismatches closer to current token than those farther away.
		/// </summary>
		public static double weightedL0_Distance(FeatureMetaData[] featureTypes, int[] A, int[] B)
		{
			double count = 0; // count how many mismatched categories there are
			for (int i = 0; i < A.Length; i++)
			{
				FeatureType type = featureTypes[i].type;
				if (type == FeatureType.TOKEN || type == FeatureType.RULE || type == FeatureType.INT || type == FeatureType.BOOL)
				{
					if (A[i] != B[i])
					{
						count += featureTypes[i].mismatchCost;
					}
				}
			}
			return count;
		}

		public static double sigmoid(int x, float center)
		{
			return 1.0 / (1.0 + Math.Exp(-0.9 * (x - center)));
		}

		public static int max(IList<int?> Y)
		{
			int max = 0;
			foreach (int y in Y)
			{
				max = Math.Max(max, y);
			}
			return max;
		}

		public static int sum(int[] a)
		{
			int s = 0;
			foreach (int x in a)
			{
				s += x;
			}
			return s;
		}

		public static string spaces(int n)
		{
			return sequence(n, " ");
	//		StringBuilder buf = new StringBuilder();
	//		for (int sp=1; sp<=n; sp++) buf.append(" ");
	//		return buf.toString();
		}

		public static string newlines(int n)
		{
			return sequence(n, "\r\n"); //KED KED KED = needs to be platform independent
	//		StringBuilder buf = new StringBuilder();
	//		for (int sp=1; sp<=n; sp++) buf.append("\n");
	//		return buf.toString();
		}

		public static string sequence(int n, string s)
		{
			StringBuilder buf = new StringBuilder();
			for (int sp = 1; sp <= n; sp++)
			{
				buf.Append(s);
			}
			return buf.ToString();
		}

		public static int count(string s, char x)
		{
			int n = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == x)
				{
					n++;
				}
			}
			return n;
		}

		public static string expandTabs(string s, int tabSize)
		{
			if (string.ReferenceEquals(s, null))
			{
				return null;
			}
			StringBuilder buf = new StringBuilder();
			int col = 0;
			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];
				switch (c)
				{
					case '\n' :
						col = 0;
						buf.Append(c);
						break;
					case '\t' :
						int n = tabSize - col % tabSize;
						col += n;
						buf.Append(spaces(n));
						break;
					default :
						col++;
						buf.Append(c);
						break;
				}
			}
			return buf.ToString();
		}

		public static string dumpWhiteSpace(string s)
		{
			string[] whiteSpaces = new string[s.Length];
			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];
				switch (c)
				{
					case '\n' :
						whiteSpaces[i] = "\\n";
						break;
					case '\t' :
						whiteSpaces[i] = "\\t";
						break;
					case '\r' :
						whiteSpaces[i] = "\\r";
						break;
					case '\u000C' :
						whiteSpaces[i] = "\\u000C";
						break;
					case ' ' :
						whiteSpaces[i] = "ws";
						break;
					default :
						whiteSpaces[i] = c.ToString();
						break;
				}
			}
			return join(whiteSpaces, " | ");
		}
	}

}