namespace AntlrTreeEditing.AntlrDOM
{
    using Antlr4.Runtime.Tree;
    using org.w3c.dom;
    using System;

    public class AntlrElement : AntlrNode, Element, IAntlrObserver
    {
        public object getAttributeNS(string sCHEMA_INSTANCE, string nIL_ATTRIBUTE)
        {
            return null;
        }

        public string Prefix { get; set; }
        public TypeInfo SchemaTypeInfo { get; set; }
        public string lookupNamespaceURI(string prefix)
        {
            throw new NotImplementedException();
        }

        public bool isDefaultNamespace(object elementNamespaceUri)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(ObserverParserRuleContext value)
        {
        }

        public void OnParentDisconnect(IParseTree value)
        {
        }

        public void OnParentConnect(IParseTree value)
        {
        }

        public void OnChildDisconnect(IParseTree value)
        {
        }

        public void OnChildConnect(IParseTree value)
        {
        }
    }
}
