namespace LanguageServer.Java
{
    using Antlr4.Runtime.Misc;

    class Pass3Listener : Java9ParserBaseListener
    {
        private JavaParserDetails _pd;

        public Pass3Listener(JavaParserDetails pd)
        {
            _pd = pd;
            JavaParserDetails._dependent_grammars.Add(_pd.FullFileName);
        }

        public override void EnterSingleTypeImportDeclaration([NotNull] Java9Parser.SingleTypeImportDeclarationContext context)
        {
            var name = context.GetChild(1).GetText();
            _pd.Imports.Add(name);
        }

        public override void EnterTypeImportOnDemandDeclaration([NotNull] Java9Parser.TypeImportOnDemandDeclarationContext context)
        {
            var name = context.GetChild(1).GetText();
            _pd.Imports.Add(name);
        }
    }
}
