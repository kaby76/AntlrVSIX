using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace AntlrLanguage
{
    // Please refer to Language Service and Editor Extension Points,
    // https://msdn.microsoft.com/en-us/library/dd885244.aspx,
    // for information on how this Managed Extensiblility Framework (MEF)
    // extension hooks into Visual Studio 2015.

    internal static class OrdinaryClassificationDefinition
    {
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
    }
}
