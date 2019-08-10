namespace AntlrVSIX.AggregateTagger
{
    using AntlrVSIX;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    internal static class ClassificationTypes
    {
        public const string AntlrTerminalTypeName = AntlrVSIX.Constants.ClassificationNameTerminal;
        public const string AntlrNonterminalTypeName = AntlrVSIX.Constants.ClassificationNameNonterminal;
        public const string AntlrCommentTypeName = AntlrVSIX.Constants.ClassificationNameComment;
        public const string AntlrKeywordTypeName = AntlrVSIX.Constants.ClassificationNameKeyword;
        public const string AntlrLiteralTypeName = AntlrVSIX.Constants.ClassificationNameLiteral;

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
