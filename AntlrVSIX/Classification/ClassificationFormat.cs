using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace AntlrLanguage
{
    #region Format definition

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "terminal")]
    [Name("terminal")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class Terminal : ClassificationFormatDefinition
    {
        public Terminal()
        {
            DisplayName = "terminal"; //human readable version of the name
            ForegroundColor = Colors.Lime;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nonterminal")]
    [Name("nonterminal")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class Nonterminal : ClassificationFormatDefinition
    {
        public Nonterminal()
        {
            DisplayName = "nonterminal"; //human readable version of the name
            ForegroundColor = Colors.Purple;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "acomment")]
    [Name("acomment")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class AComment : ClassificationFormatDefinition
    {
        public AComment()
        {
            DisplayName = "acomment"; //human readable version of the name
            ForegroundColor = Colors.Green;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "akeyword")]
    [Name("akeyword")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class Keyword : ClassificationFormatDefinition
    {
        public Keyword()
        {
            DisplayName = "akeyword"; //human readable version of the name
            ForegroundColor = Colors.Blue;
        }
    }

    #endregion //Format definition
}
