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
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(ObserverParserRuleContext value)
        {
            throw new NotImplementedException();
        }

        public void OnParentDisconnect(ObserverParserRuleContext value)
        {
            throw new NotImplementedException();
        }

        public void OnParentConnect(ObserverParserRuleContext value)
        {
            throw new NotImplementedException();
        }

        public void OnChildDisconnect(ObserverParserRuleContext value)
        {
            throw new NotImplementedException();
        }

        public void OnChildConnect(ObserverParserRuleContext value)
        {
            throw new NotImplementedException();
        }

        public void OnChildConnect(ITerminalNode value)
        {
            throw new NotImplementedException();
        }
    }
}
