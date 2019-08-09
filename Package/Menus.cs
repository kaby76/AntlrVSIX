using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using AntlrVSIX.Extensions;
using AntlrVSIX.FindAllReferences;
using AntlrVSIX.GoToDefintion;
using AntlrVSIX.Rename;
using AntlrVSIX.NextSym;
using AntlrVSIX.GoToVisitor;
using AntlrVSIX.Reformat;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;

namespace AntlrVSIX.Package
{
    public class Menus
    {
        public static void ResetMenus()
        {
            if (GoToDefinitionCommand.Instance == null ||
                FindAllReferencesCommand.Instance == null ||
                RenameCommand.Instance == null ||
                Reformat.ReformatCommand.Instance == null)
                return;

            IWpfTextView view = AntlrLanguagePackage.Instance.GetActiveView();
            if (view == null) return;

            // First, find out what this view is, and what the file is.
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            string path = doc.FilePath;


            // Whack any old values that cursor points to.
            GoToDefinitionCommand.Instance.Enabled = false;
            FindAllReferencesCommand.Instance.Enabled = false;
            NextSymCommand.Instance.Enabled = true;
            RenameCommand.Instance.Enabled = false;
            ReformatCommand.Instance.Enabled = false;
            GoToVisitorCommand.Instance.Enabled = false;

            AntlrLanguagePackage.Instance.Span = default(SnapshotSpan);
            AntlrLanguagePackage.Instance.Classification = default(string);
            AntlrLanguagePackage.Instance.View = view;

            var fp = view.GetFilePath();
            if (fp != null)
            {
                if (fp.IsAntlrSuffix())
                {
                    NextSymCommand.Instance.Enabled = true;
                    Reformat.ReformatCommand.Instance.Enabled = true;
                }
            }
            else return;

            // Find details of the Antlr symbol pointed to.
            ITextCaret car = view.Caret;
            CaretPosition cp = car.Position;
            SnapshotPoint bp = cp.BufferPosition;
            TextExtent extent = AntlrVSIX.Package.AntlrLanguagePackage.Instance.Navigator[view].GetExtentOfWord(bp);
            SnapshotSpan span = extent.Span;
            AntlrLanguagePackage.Instance.Span = span;

            //  Now, check for valid classification type.
            ClassificationSpan[] c1 = AntlrVSIX.Package.AntlrLanguagePackage.Instance.Aggregator[view].GetClassificationSpans(span).ToArray();
            foreach (ClassificationSpan classification in c1)
            {
                var name = classification.ClassificationType.Classification.ToLower();
                if (name == AntlrVSIX.Constants.ClassificationNameTerminal)
                {
                    AntlrLanguagePackage.Instance.Classification = AntlrVSIX.Constants.ClassificationNameTerminal;

                    GoToDefinitionCommand.Instance.Enabled = true;
                    FindAllReferencesCommand.Instance.Enabled = true;
                    RenameCommand.Instance.Enabled = true;
                    ReformatCommand.Instance.Enabled = true;
                }
                else if (name == AntlrVSIX.Constants.ClassificationNameNonterminal)
                {
                    AntlrLanguagePackage.Instance.Classification = AntlrVSIX.Constants.ClassificationNameNonterminal;

                    GoToDefinitionCommand.Instance.Enabled = true;
                    FindAllReferencesCommand.Instance.Enabled = true;
                    RenameCommand.Instance.Enabled = true;
                    ReformatCommand.Instance.Enabled = true;
                    GoToVisitorCommand.Instance.Enabled = true;
                }
                else if (name == AntlrVSIX.Constants.ClassificationNameLiteral)
                {
                    AntlrLanguagePackage.Instance.Classification = AntlrVSIX.Constants.ClassificationNameLiteral;

                    GoToDefinitionCommand.Instance.Enabled = true;
                    FindAllReferencesCommand.Instance.Enabled = true;
                    RenameCommand.Instance.Enabled = true;
                    ReformatCommand.Instance.Enabled = true;
                }
            }
        }
    }
}
