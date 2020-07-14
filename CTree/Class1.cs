using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CTree
{
    public class Class1 : AstParserBaseVisitor<IParseTree>
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
            IParseTree convert = Convert(ast);
            return convert;
        }

        public IParseTree Convert(IParseTree tree)
        {
            var result = this.Visit(tree);
            return result;
        }

        public override IParseTree VisitAst(AstParserParser.AstContext context)
        {
            return VisitNode(context.node());
        }

        public override IParseTree VisitNode(AstParserParser.NodeContext context)
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
            var mapped_node = Activator.CreateInstance(mapped_type, new object[] {null, 0}) as IParseTree;
            foreach (var c in context.children)
            {
                if (c is AstParserParser.NodeContext)
                {
                    var mc = VisitNode(c as AstParserParser.NodeContext);
                    if (mc is TerminalNodeImpl)
                    {
                        var _mc = mc as TerminalNodeImpl;
                        var _mapped_node = mapped_node as ParserRuleContext;
                        _mc.Parent = _mapped_node;
                        _mapped_node.AddChild(_mc);
                    }
                    else
                    {
                        var _mc = mc as ParserRuleContext;
                        var _mapped_node = mapped_node as ParserRuleContext;
                        _mc.Parent = _mapped_node;
                        _mapped_node.AddChild(_mc);
                    }
                }
                else if (c is AstParserParser.ValContext)
                {
                    var mc = VisitVal(c as AstParserParser.ValContext);
                    if (mc is TerminalNodeImpl)
                    {
                        var _mc = mc as TerminalNodeImpl;
                        var _mapped_node = mapped_node as ParserRuleContext;
                        _mc.Parent = _mapped_node;
                        _mapped_node.AddChild(_mc);
                    }
                    else
                    {
                        var _mc = mc as ParserRuleContext;
                        var _mapped_node = mapped_node as ParserRuleContext;
                        _mc.Parent = _mapped_node;
                        _mapped_node.AddChild(_mc);
                    }
                }
            }
            return mapped_node;
        }

        public override IParseTree VisitVal(AstParserParser.ValContext context)
        {
            var id = context.ID();
            var id_name = id.GetText();
            return _env[id_name];
        }
    }
}
