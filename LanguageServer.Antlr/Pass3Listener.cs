namespace LanguageServer.Antlr
{
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Symtab;

    public class Pass3Listener : ANTLRv4ParserBaseListener
    {
        private AntlrParserDetails _pd;

        public Pass3Listener(AntlrParserDetails pd)
        {
            _pd = pd;
        }

        public override void EnterOption([NotNull] ANTLRv4Parser.OptionContext context)
        {
            if (context.ChildCount < 3) return;
            if (context.GetChild(0) == null) return;
            if (context.GetChild(0).GetText() != "tokenVocab") return;
            var dep_grammar = context.GetChild(2).GetText();

        }

        public override void EnterDelegateGrammar([NotNull] ANTLRv4Parser.DelegateGrammarContext context)
        {

        }
    }
}
