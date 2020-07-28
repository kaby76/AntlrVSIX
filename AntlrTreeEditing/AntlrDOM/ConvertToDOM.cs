namespace AntlrTreeEditing.AntlrDOM
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using org.w3c.dom;
    using System.Text.RegularExpressions;

    public class ConvertToDOM
    {
        public static AntlrDynamicContext Try(IParseTree tree, Parser parser)
        {
            // Perform bottom up traversal to derive equivalent tree in "dom".
            var converted_tree = BottomUpConvert(tree, parser);
            var document = new AntlrDocument();
            document.NodeType = NodeConstants.DOCUMENT_NODE;
            AntlrNodeList nl = new AntlrNodeList();
            nl.Add(converted_tree);
            document.ChildNodes = nl;
            AntlrDynamicContext result = new AntlrDynamicContext();
            result.Document = document;
            return result;
        }

        private static AntlrNode BottomUpConvert(IParseTree tree, Parser parser)
        {
            if (tree is TerminalNodeImpl)
            {
                var result = new AntlrElement();
                result.AntlrIParseTree = tree;
                TerminalNodeImpl t = tree as TerminalNodeImpl;
                Interval interval = t.SourceInterval;
                result.NodeType = NodeConstants.ELEMENT_NODE;
                var common_token_stream = parser.InputStream as CommonTokenStream;
                var lexer = common_token_stream.TokenSource as Lexer;
                var fixed_name = parser.Vocabulary.GetSymbolicName(t.Symbol.Type);
                result.LocalName = fixed_name;
                var nl = new AntlrNodeList();
                result.ChildNodes = nl;

                var child = new AntlrText();
                child.AntlrIParseTree = tree;
                child.NodeType = NodeConstants.TEXT_NODE;
                child.Data = new xpath.org.eclipse.wst.xml.xpath2.processor.@internal.OutputParseTree().PerformEscapes(/*"'" + */ tree.GetText() /*+ "'"*/);
                child.ParentNode = result;
                nl.Add(child);

                {
                    var attr = new AntlrAttr();
                    var child_count = t.ChildCount;
                    attr.NodeType = NodeConstants.ATTRIBUTE_NODE;
                    attr.Name = "ChildCount";
                    attr.Value = child_count.ToString();
                    attr.ParentNode = result;
                    nl.Add(attr);
                }

                {
                    var attr = new AntlrAttr();
                    attr.NodeType = NodeConstants.ATTRIBUTE_NODE;
                    var source_interval = t.SourceInterval;
                    var a = source_interval.a;
                    var b = source_interval.b;
                    attr.Name = "SourceInterval";
                    attr.Value = "[" + a + "," + b + "]";
                    attr.ParentNode = result;
                    nl.Add(attr);
                }

                return result;
            }
            else
            {
                var result = new AntlrElement();
                var t = tree as ObserverParserRuleContext;
                t.Subscribe(result);
                result.AntlrIParseTree = tree;
                result.NodeType = NodeConstants.ELEMENT_NODE;
                var fixed_name = tree.GetType().ToString()
                    .Replace("Antlr4.Runtime.Tree.", "");
                fixed_name = Regex.Replace(fixed_name, "^.*[+]", "");
                fixed_name = fixed_name.Substring(0, fixed_name.Length - "Context".Length);
                fixed_name = fixed_name[0].ToString().ToLower()
                             + fixed_name.Substring(1);
                result.LocalName = fixed_name;
                var nl = new AntlrNodeList();
                result.ChildNodes = nl;

                var map = new AntlrNamedNodeMap();
                result.Attributes = map;

                {
                    var attr = new AntlrAttr();
                    var child_count = t.ChildCount;
                    attr.NodeType = NodeConstants.ATTRIBUTE_NODE;
                    attr.Name = "ChildCount";
                    attr.LocalName = "ChildCount";
                    attr.Value = child_count.ToString();
                    attr.ParentNode = result;
                    nl.Add(attr);
                    map.Add(attr);
                }

                {
                    var attr = new AntlrAttr();
                    attr.NodeType = NodeConstants.ATTRIBUTE_NODE;
                    var source_interval = t.SourceInterval;
                    var a = source_interval.a;
                    var b = source_interval.b;
                    attr.Name = "Start";
                    attr.LocalName = "Start";
                    attr.Value = a.ToString();
                    attr.ParentNode = result;
                    nl.Add(attr);
                    map.Add(attr);
                }
                {
                    var attr = new AntlrAttr();
                    attr.NodeType = NodeConstants.ATTRIBUTE_NODE;
                    var source_interval = t.SourceInterval;
                    var a = source_interval.a;
                    var b = source_interval.b;
                    attr.Name = "End";
                    attr.LocalName = "End";
                    attr.Value = b.ToString();
                    attr.ParentNode = result;
                    nl.Add(attr);
                    map.Add(attr);
                }

                for (int i = 0; i < tree.ChildCount; ++i)
                {
                    var child = tree.GetChild(i);
                    var convert = BottomUpConvert(child, parser);
                    nl.Add(convert);
                    convert.ParentNode = result;
                }

                return result;
            }
        }
    }
}
