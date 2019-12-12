using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace LspClangd
{
    public class ContentDefinition
    {
        [Export]
        [Name("clangd")]
        [BaseDefinition(CodeRemoteContentDefinition.CodeRemoteContentTypeName)]
        internal static ContentTypeDefinition ContentTypeDefinition;

        [Export]
        [FileExtension(".cpp")]
        [ContentType("clangd")]
        internal static FileExtensionToContentTypeDefinition FileExtensionDefinition;
    }
}