namespace AntlrVSIX.AggregateTagger
{
    using AntlrVSIX;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    // Please refer to Language Service and Editor Extension Points,
    // https://msdn.microsoft.com/en-us/library/dd885244.aspx,
    // for information on how this Managed Extensiblility Framework (MEF)
    // extension hooks into Visual Studio 2015.

    internal static class ClassificationTypes
    {
        public const string AntlrTerminalTypeName = Constants.ClassificationNameTerminal;
        public const string AntlrNonterminalTypeName = Constants.ClassificationNameNonterminal;
        public const string AntlrCommentTypeName = Constants.ClassificationNameComment;
        public const string AntlrKeywordTypeName = Constants.ClassificationNameKeyword;
        public const string AntlrLiteralTypeName = Constants.ClassificationNameLiteral;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.ClassificationNameTerminal)]
        internal static ClassificationTypeDefinition _antlr_terminal = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.ClassificationNameNonterminal)]
        internal static ClassificationTypeDefinition _antlr_nonterminal = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.ClassificationNameComment)]
        internal static ClassificationTypeDefinition _antlr_comment = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.ClassificationNameKeyword)]
        internal static ClassificationTypeDefinition _antlr_keyword = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.ClassificationNameLiteral)]
        internal static ClassificationTypeDefinition _antlr_literal = null;
    }
}
