using System;

namespace org.antlr.codebuff.misc
{

	using Lexer = Antlr4.Runtime.Lexer;
	using Parser = Antlr4.Runtime.Parser;

	public class LangDescriptor
	{
		public string name;
		public string corpusDir; // dir under "corpus/"
		public string fileRegex;
		public Type lexerClass;
		public Type parserClass;
		public string startRuleName;
		public int indentSize;
		/// <summary>
		/// token type of single comment, if any. If your single-comment lexer
		///  rule matches newline, then this is optional.
		/// </summary>
		public int singleLineCommentType;

		public LangDescriptor(string name, string corpusDir, string fileRegex, Type lexerClass, Type parserClass, string startRuleName, int indentSize, int singleLineCommentType)
		{
			this.name = name;
			this.corpusDir = corpusDir;
			this.fileRegex = fileRegex;
			this.lexerClass = lexerClass;
			this.parserClass = parserClass;
			this.startRuleName = startRuleName;
			this.indentSize = indentSize;
			this.singleLineCommentType = singleLineCommentType;
		}
	}

}