
namespace AntlrVSIX.AggregateTagger
{
    using Microsoft.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    class Themes
    {
        public static bool IsInvertedTheme()
        {
            return VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey).GetBrightness() < 0.5;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = AntlrVSIX.Constants.ClassificationNameTerminal)]
    [Name(AntlrVSIX.Constants.ClassificationNameTerminal)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Terminal : ClassificationFormatDefinition
    {
        public Terminal()
        {
            DisplayName = "Antlr Terminal"; //human readable version of the name
            if (Themes.IsInvertedTheme())
                ForegroundColor = AntlrVSIX.Constants.InvertedColorTextForegroundTerminal;
            else
                ForegroundColor = AntlrVSIX.Constants.NormalColorTextForegroundTerminal;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = AntlrVSIX.Constants.ClassificationNameNonterminal)]
    [Name(AntlrVSIX.Constants.ClassificationNameNonterminal)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Nonterminal : ClassificationFormatDefinition
    {
        public Nonterminal()
        {
            DisplayName = "Antlr Nonterminal"; //human readable version of the name
            if (Themes.IsInvertedTheme())
                ForegroundColor = AntlrVSIX.Constants.InvertedColorTextForegroundNonterminal;
            else
                ForegroundColor = AntlrVSIX.Constants.NormalColorTextForegroundNonterminal;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = AntlrVSIX.Constants.ClassificationNameComment)]
    [Name(AntlrVSIX.Constants.ClassificationNameComment)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class AComment : ClassificationFormatDefinition
    {
        public AComment()
        {
            DisplayName = "Antlr Comment"; //human readable version of the name
            if (Themes.IsInvertedTheme())
                ForegroundColor = AntlrVSIX.Constants.InvertedColorTextForegroundComment;
            else
                ForegroundColor = AntlrVSIX.Constants.NormalColorTextForegroundComment;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = AntlrVSIX.Constants.ClassificationNameKeyword)]
    [Name(AntlrVSIX.Constants.ClassificationNameKeyword)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Keyword : ClassificationFormatDefinition
    {
        public Keyword()
        {
            DisplayName = "Antlr Keyword"; //human readable version of the name
            if (Themes.IsInvertedTheme())
                ForegroundColor = AntlrVSIX.Constants.InvertedColorTextForegroundKeyword;
            else
                ForegroundColor = AntlrVSIX.Constants.NormalColorTextForegroundKeyword;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = AntlrVSIX.Constants.ClassificationNameLiteral)]
    [Name(AntlrVSIX.Constants.ClassificationNameLiteral)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Literal : ClassificationFormatDefinition
    {
        public Literal()
        {
            DisplayName = "Antlr Literal"; //human readable version of the name
            if (Themes.IsInvertedTheme())
                ForegroundColor = AntlrVSIX.Constants.InvertedColorTextForegroundLiteral;
            else
                ForegroundColor = AntlrVSIX.Constants.NormalColorTextForegroundLiteral;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = AntlrVSIX.Constants.ClassificationNameMode)]
    [Name(AntlrVSIX.Constants.ClassificationNameMode)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Mode : ClassificationFormatDefinition
    {
        public Mode()
        {
            DisplayName = "Antlr Mode"; //human readable version of the name
            if (Themes.IsInvertedTheme())
                ForegroundColor = AntlrVSIX.Constants.InvertedColorTextForegroundMode;
            else
                ForegroundColor = AntlrVSIX.Constants.NormalColorTextForegroundMode;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = AntlrVSIX.Constants.ClassificationNameChannel)]
    [Name(AntlrVSIX.Constants.ClassificationNameChannel)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Channel : ClassificationFormatDefinition
    {
        public Channel()
        {
            DisplayName = "Antlr Channel"; //human readable version of the name
            if (Themes.IsInvertedTheme())
                ForegroundColor = AntlrVSIX.Constants.InvertedColorTextForegroundChannel;
            else
                ForegroundColor = AntlrVSIX.Constants.NormalColorTextForegroundChannel;
        }
    }
}
