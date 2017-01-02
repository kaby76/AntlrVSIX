using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace AntlrLanguage
{
    internal static class OrdinaryClassificationDefinition
    {
        #region Type definition

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("terminal")]
        internal static ClassificationTypeDefinition antlrTerminal = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("nonterminal")]
        internal static ClassificationTypeDefinition antlrNonterminal = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("acomment")]
        internal static ClassificationTypeDefinition antlrComment = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("akeyword")]
        internal static ClassificationTypeDefinition antlrKeyword = null;

        #endregion
    }
}
