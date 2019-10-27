using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;

namespace LanguageServer.Java
{
    class Pass3Listener : Java9ParserBaseListener
    {
        private JavaParserDetails _pd;

        public Pass3Listener(JavaParserDetails pd)
        {
            _pd = pd;
        }

        public override void EnterSingleTypeImportDeclaration([NotNull] Java9Parser.SingleTypeImportDeclarationContext context)
        {
            var name = context.GetChild(1).GetText();
            JavaParserDetails.imports.Add(_pd.Item.FullPath, name);
        }

        public override void EnterTypeImportOnDemandDeclaration([NotNull] Java9Parser.TypeImportOnDemandDeclarationContext context)
        {
            var name = context.GetChild(1).GetText();
            JavaParserDetails.imports.Add(_pd.Item.FullPath, name);
        }
    }
}
