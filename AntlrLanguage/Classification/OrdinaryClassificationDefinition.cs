namespace AntlrLanguage.Classification
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Utilities;

    // Please refer to Language Service and Editor Extension Points,
    // https://msdn.microsoft.com/en-us/library/dd885244.aspx,
    // for information on how this Managed Extensiblility Framework (MEF)
    // extension hooks into Visual Studio 2015.

    internal static class OrdinaryClassificationDefinition
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.ClassificationNameTerminal)]
        internal static ClassificationTypeDefinition antlrTerminal = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.ClassificationNameNonterminal)]
        internal static ClassificationTypeDefinition antlrNonterminal = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.ClassificationNameComment)]
        internal static ClassificationTypeDefinition antlrComment = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.ClassificationNameKeyword)]
        internal static ClassificationTypeDefinition antlrKeyword = null;
    }
}
