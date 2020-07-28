namespace AntlrTreeEditing.AntlrDOM
{
    using Antlr4.Runtime.Tree;
    using org.w3c.dom;
    using System;

    public class AntlrNode : Node
    {
        public IParseTree AntlrIParseTree { get; set; }
        public short NodeType { get; set; }
        public string LocalName { get; set; }
        public Document OwnerDocument { get; set; }
        public NodeList ChildNodes { get; set; }
        public Node NextSibling { get; set; }
        public string BaseURI { get; set; }
        public NamedNodeMap Attributes { get; set; }
        public object NodeValue { get; set; }
        public string NamespaceURI { get; set; }
        public object NodeName { get; set; }
        public Node ParentNode { get; set; }
        public Node PreviousSibling { get; set; }
        public bool isSameNode(Node nodeValue)
        {
            throw new NotImplementedException();
        }

        public short compareDocumentPosition(Node nodeB)
        {
            throw new NotImplementedException();
        }

        public bool isEqualNode(Node node)
        {
            throw new NotImplementedException();
        }

        public bool hasChildNodes()
        {
            throw new NotImplementedException();
        }

        public bool hasAttributes()
        {
            throw new NotImplementedException();
        }
    }
}
