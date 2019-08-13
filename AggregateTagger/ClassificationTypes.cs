namespace AntlrVSIX.AggregateTagger
{
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    internal static class ClassificationTypes
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name(AntlrVSIX.Constants.ClassificationNameTerminal)]
        internal static ClassificationTypeDefinition _antlr_terminal = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(AntlrVSIX.Constants.ClassificationNameNonterminal)]
        internal static ClassificationTypeDefinition _antlr_nonterminal = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(AntlrVSIX.Constants.ClassificationNameComment)]
        internal static ClassificationTypeDefinition _antlr_comment = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(AntlrVSIX.Constants.ClassificationNameKeyword)]
        internal static ClassificationTypeDefinition _antlr_keyword = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(AntlrVSIX.Constants.ClassificationNameLiteral)]
        internal static ClassificationTypeDefinition _antlr_literal = null;
    }
}
