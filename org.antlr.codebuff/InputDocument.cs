using System.Collections.Generic;
using System.Linq;

namespace org.antlr.codebuff
{

	using CodeBuffTokenStream = org.antlr.codebuff.misc.CodeBuffTokenStream;
	using LangDescriptor = org.antlr.codebuff.misc.LangDescriptor;
	using Parser = Antlr4.Runtime.Parser;
	using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;
	using Token = Antlr4.Runtime.IToken;
	using TerminalNode = Antlr4.Runtime.Tree.ITerminalNode;


	public class InputDocument
	{
		public LangDescriptor language;
		public string fileName;
		public string content;
		public IList<string> lines; // used for debugging; a cache of lines in this.content
		public int index;
		public ParserRuleContext tree;
		public IDictionary<Token, TerminalNode> tokenToNodeMap = null;

		public Parser parser;
		public CodeBuffTokenStream tokens;

		public static InputDocument dup(InputDocument old)
		{
		    if (!string.IsNullOrEmpty(old.fileName))
		        // reparse to get new tokens, tree
		        return Tool.parse(old.fileName, old.language);
		    else
		        return Tool.parse(old.fileName, Tool.unformatted_input, old.language);
		}

		public InputDocument(string fileName, string content, LangDescriptor language)
		{
			this.content = content;
			this.fileName = fileName;
			this.language = language;
		}

		public virtual string getLine(int line)
		{
			if (lines == null)
			{
			    string[] v = content.Split('\n');
			    lines = v.ToList();
			}
			if (line > 0)
			{
				return lines[line-1];
			}
			return null;
		}

		public virtual ParserRuleContext Tree
		{
			set
			{
				this.tree = value;
				if (value != null)
				{
					tokenToNodeMap = Trainer.indexTree(value);
				}
			}
		}

		public override string ToString()
		{
			return fileName + "[" + content.Length + "]" + "@" + index;
		}
	}


}