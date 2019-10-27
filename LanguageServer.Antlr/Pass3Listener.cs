namespace LanguageServer.Antlr
{
    using Antlr4.Runtime.Misc;

    public class Pass3Listener : ANTLRv4ParserBaseListener
    {
        private AntlrParserDetails _pd;

        public Pass3Listener(AntlrParserDetails pd)
        {
            _pd = pd;
            AntlrParserDetails._dependent_grammars.Add(_pd.FullFileName);
        }

        public override void EnterOption([NotNull] ANTLRv4Parser.OptionContext context)
        {
            if (context.ChildCount < 3) return;
            if (context.GetChild(0) == null) return;
            if (context.GetChild(0).GetText() != "tokenVocab") return;
            var dep_grammar = context.GetChild(2).GetText();
            var file = _pd.Item.FullPath;
            var dir = System.IO.Path.GetDirectoryName(file);
            var dep = dir + System.IO.Path.DirectorySeparatorChar + dep_grammar + ".g4";
            dep = Workspaces.Util.GetProperFilePathCapitalization(dep);
            if (dep == null) return;
            _pd.Imports.Add(dep);
            AntlrParserDetails._dependent_grammars.Add(dep, file);
        }

        public override void EnterDelegateGrammar([NotNull] ANTLRv4Parser.DelegateGrammarContext context)
        {
            if (context.ChildCount < 1) return;
            if (context.GetChild(0) == null) return;
            var dep_grammar = context.GetChild(0).GetText();
            var file = _pd.Item.FullPath;
            var dir = System.IO.Path.GetDirectoryName(file);
            var dep = dir + System.IO.Path.DirectorySeparatorChar + dep_grammar + ".g4";
            dep = Workspaces.Util.GetProperFilePathCapitalization(dep);
            if (dep == null) return;
            _pd.Imports.Add(dep);
            AntlrParserDetails._dependent_grammars.Add(dep, file);
        }
    }
}
