using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace VSIXProject1
{
    public class JavaContentDefinition
    {
        [Export]
        [Name("antlr")]
        [BaseDefinition(CodeRemoteContentDefinition.CodeRemoteContentTypeName)]
        internal static ContentTypeDefinition JavaContentTypeDefinition;

        [Export]
        [FileExtension(".g4")]
        [ContentType("antlr")]
        internal static FileExtensionToContentTypeDefinition JavaFileExtensionDefinition;
    }
}