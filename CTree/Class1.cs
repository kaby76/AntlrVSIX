using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CTree
{
    public class Class1 : AstParserBaseVisitor<ParserRuleContext>
    {
        private Parser _parser;
        private Lexer _lexer;
        private Dictionary<string, IParseTree> _env;

        public Class1(Parser parser, Lexer lexer, Dictionary<string, IParseTree> env)
        {
            _parser = parser;
            _lexer = lexer;
            _env = env;
        }

        public IParseTree CreateTree(string ast_string)
        {
            IParseTree result = null;
            var ast_stream = CharStreams.fromstring(ast_string);
            var ast_lexer = new AstLexer(ast_stream);
            var ast_tokens = new CommonTokenStream(ast_lexer);
            var ast_parser = new AstParserParser(ast_tokens);
            ast_parser.BuildParseTree = true;
            var listener = new ErrorListener<IToken>();
            ast_parser.AddErrorListener(listener);
            IParseTree ast = ast_parser.ast();
            if (listener.had_error) throw new Exception();
            RuleContext convert = Convert(ast);
            return convert;
        }

        public RuleContext Convert(IParseTree tree)
        {
            var result = this.Visit(tree);
            return result;
        }

        public override ParserRuleContext VisitAst(AstParserParser.AstContext context)
        {
            return VisitNode(context.node());
        }

        public override ParserRuleContext VisitNode(AstParserParser.NodeContext context)
        {
            var id = context.ID();
            var id_name = id.GetText().ToLower() + "context";
            var pt = _parser.GetType();
            var list = pt.GetTypeInfo().DeclaredNestedTypes.Where(t => t.Name.ToLower() == id_name);
            if (list.Count() != 1)
            {
                throw new Exception();
            }
            TypeInfo mapped_type = list.First();
            var mapped_node = (ParserRuleContext) Activator.CreateInstance(mapped_type, new object[] {null, 0});
            foreach (var c in context.children)
            {
                if (c is AstParserParser.NodeContext)
                {
                    var mc = VisitNode(c as AstParserParser.NodeContext);
                    mc.Parent = mapped_node;
                    mapped_node.AddChild(mc);
                }
                else if (c is AstParserParser.ValContext)
                {
                    var mc = VisitVal(c as AstParserParser.ValContext);
                    mc.Parent = mapped_node;
                    mapped_node.AddChild(mc);
                }
            }
            return mapped_node;
        }

        public override ParserRuleContext VisitVal(AstParserParser.ValContext context)
        {
            var id = context.ID();
            var id_name = id.GetText();
            return (ParserRuleContext)_env[id_name];
        }
    }
}
