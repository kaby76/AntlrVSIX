namespace AntlrVSIX.Classification
{
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Utilities;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;


    // Please refer to Language Service and Editor Extension Points,
    // https://msdn.microsoft.com/en-us/library/dd885244.aspx,
    // for information on how this Managed Extensiblility Framework (MEF)
    // extension hooks into Visual Studio 2015.

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ClassificationNameTerminal)]
    [Name(Constants.ClassificationNameTerminal)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Terminal : ClassificationFormatDefinition
    {
        public Terminal()
        {
            DisplayName = "Antlr Terminal"; //human readable version of the name
            ForegroundColor = Constants.ColorTextForegroundTerminal;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ClassificationNameNonterminal)]
    [Name(Constants.ClassificationNameNonterminal)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Nonterminal : ClassificationFormatDefinition
    {
        public Nonterminal()
        {
            DisplayName = "Antlr Nonterminal"; //human readable version of the name
            ForegroundColor = Constants.ColorTextForegroundNonterminal;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ClassificationNameComment)]
    [Name(Constants.ClassificationNameComment)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class AComment : ClassificationFormatDefinition
    {
        public AComment()
        {
            DisplayName = "Antlr Comment"; //human readable version of the name
            ForegroundColor = Constants.ColorTextForegroundComment;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ClassificationNameKeyword)]
    [Name(Constants.ClassificationNameKeyword)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Keyword : ClassificationFormatDefinition
    {
        public Keyword()
        {
            DisplayName = "Antlr Keyword"; //human readable version of the name
            ForegroundColor = Constants.ColorTextForegroundKeyword;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ClassificationNameLiteral)]
    [Name(Constants.ClassificationNameLiteral)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Literal : ClassificationFormatDefinition
    {
        public Literal()
        {
            DisplayName = "Antlr Literal"; //human readable version of the name
            ForegroundColor = Constants.ColorTextForegroundLiteral;
        }
    }
}
