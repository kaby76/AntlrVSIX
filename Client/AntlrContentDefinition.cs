namespace LspAntlr
{
    using Microsoft.VisualStudio.LanguageServer.Client;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    internal class AntlrContentDefinition
    {
        [Export]
        [Name("Antlr")]
        [BaseDefinition(CodeRemoteContentDefinition.CodeRemoteContentTypeName)]
        internal static ContentTypeDefinition JavaContentTypeDefinition;
    }
}