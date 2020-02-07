namespace LspAntlr
{
    using Microsoft.VisualStudio.LanguageServer.Client;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    public class AntlrContentDefinition
    {
        [Export]
        [Name("Antlr")]
        [BaseDefinition(CodeRemoteContentDefinition.CodeRemoteContentTypeName)]
        internal static ContentTypeDefinition JavaContentTypeDefinition;

        [Export]
        [FileExtension(".g4")]
        [ContentType("Antlr")]
        internal static FileExtensionToContentTypeDefinition JavaFileExtensionDefinition;
    }
}