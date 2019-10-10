namespace AntlrVSIX.File
{
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    internal static class FileAndContentTypeDefinitions
    {
        [Export]
        [Name("Antlr")]
        [BaseDefinition("text")]
        internal static ContentTypeDefinition hidingContentTypeDefinition;
    }
}
