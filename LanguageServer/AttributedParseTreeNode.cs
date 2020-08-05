namespace LanguageServer
{
    using Antlr4.Runtime;

    public class AttributedParseTreeNode : AntlrTreeEditing.AntlrDOM.ObserverParserRuleContext
    {
        public AttributedParseTreeNode()
            : base()
        {
        }

        public AttributedParseTreeNode(ParserRuleContext parent, int invokingState)
            : base(parent, invokingState)
        {
        }

        public ParsingResults ParserDetails { get; set; }
    }
}
