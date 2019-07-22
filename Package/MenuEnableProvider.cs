using System;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using AntlrVSIX.Extensions;
using AntlrVSIX.FindAllReferences;
using AntlrVSIX.GoToDefintion;
using AntlrVSIX.Rename;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using AntlrVSIX.NextSym;
using AntlrVSIX.Keyboard;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;

namespace AntlrVSIX.Navigate
{
    public class MenuEnableProvider
    {
        public static void ResetMenus()
        {
            if (GoToDefinitionCommand.Instance == null ||
                FindAllReferencesCommand.Instance == null ||
                RenameCommand.Instance == null ||
                Reformat.ReformatCommand.Instance == null)
                return;

            IWpfTextView view = null;

            foreach (var kvp in AntlrVSIX.Navigate.AntlrLanguagePackage.Instance.ServiceProvider)
            {
                var k = kvp.Key;
                var v = kvp.Value;
                var service = v.GetService(typeof(SVsTextManager));
                var textManager = service as IVsTextManager2;
                IVsTextView view2;
                int result = textManager.GetActiveView2(1, null, (uint)_VIEWFRAMETYPE.vftCodeWindow, out view2);
                var xxx = view2.GetIWpfTextView();
                if (xxx == k)
                {
                    view = xxx;
                    break;
                }
            }

            if (view == null)
                return;

            // First, find out what this view is, and what the file is.
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            string path = doc.FilePath;

            AntlrLanguagePackage.Instance.Span = default(SnapshotSpan);
            AntlrLanguagePackage.Instance.Classification = default(string);
            AntlrLanguagePackage.Instance.View = default(ITextView);

            // Whack any old values that cursor points to.
            GoToDefinitionCommand.Instance.Enabled = false;
            GoToDefinitionCommand.Instance.Visible = false;

            FindAllReferencesCommand.Instance.Enabled = false;
            FindAllReferencesCommand.Instance.Visible = false;

            NextSymCommand.Instance.Enabled = true;

            RenameCommand.Instance.Enabled = false;
            RenameCommand.Instance.Visible = false;

            Reformat.ReformatCommand.Instance.Enabled = false;
            Reformat.ReformatCommand.Instance.Visible = false;

            AntlrLanguagePackage.Instance.View = view;

            var fp = view.GetFilePath();
            if (fp != null)
            {
                string suffix = System.IO.Path.GetExtension(fp);
                if (suffix.Equals(".g4") || suffix.Equals(".G4"))
                {
                    GoToDefinitionCommand.Instance.Visible = true;
                    FindAllReferencesCommand.Instance.Visible = true;
                    RenameCommand.Instance.Visible = true;
                    NextSymCommand.Instance.Enabled = true;
                    Reformat.ReformatCommand.Instance.Enabled = true;
                    Reformat.ReformatCommand.Instance.Visible = true;
                }
            }
            else return;

            // Find details of the Antlr symbol pointed to.
            ITextCaret car = view.Caret;
            CaretPosition cp = car.Position;
            SnapshotPoint bp = cp.BufferPosition;
            int pos = bp.Position;
            SnapshotSpan span2 = new SnapshotSpan(bp, 0);
            TextExtent extent = AntlrVSIX.Navigate.AntlrLanguagePackage.Instance.Navigator[view].GetExtentOfWord(bp);
            SnapshotSpan span = extent.Span;

            //SnapshotPoint where = bp;
            //if (where == default(SnapshotPoint)) return;
            //TextExtent extent = _navigator.GetExtentOfWord(where);
            //SnapshotSpan span = extent.Span;

            //  Now, check for valid classification type.
            ClassificationSpan[] c1 = AntlrVSIX.Navigate.AntlrLanguagePackage.Instance.Aggregator[view].GetClassificationSpans(span).ToArray();
            foreach (ClassificationSpan classification in c1)
            {
                var name = classification.ClassificationType.Classification.ToLower();
                if (name == AntlrVSIX.Constants.ClassificationNameTerminal)
                {
                    AntlrLanguagePackage.Instance.Classification = AntlrVSIX.Constants.ClassificationNameTerminal;

                    GoToDefinitionCommand.Instance.Enabled = true;
                    GoToDefinitionCommand.Instance.Visible = true;

                    FindAllReferencesCommand.Instance.Enabled = true;
                    FindAllReferencesCommand.Instance.Visible = true;

                    RenameCommand.Instance.Enabled = true;
                    RenameCommand.Instance.Visible = true;

                    Reformat.ReformatCommand.Instance.Enabled = true;
                    Reformat.ReformatCommand.Instance.Visible = true;
                }
                else if (name == AntlrVSIX.Constants.ClassificationNameNonterminal)
                {
                    AntlrLanguagePackage.Instance.Classification = AntlrVSIX.Constants.ClassificationNameNonterminal;

                    GoToDefinitionCommand.Instance.Enabled = true;
                    GoToDefinitionCommand.Instance.Visible = true;

                    FindAllReferencesCommand.Instance.Enabled = true;
                    FindAllReferencesCommand.Instance.Visible = true;

                    RenameCommand.Instance.Enabled = true;
                    RenameCommand.Instance.Visible = true;

                    Reformat.ReformatCommand.Instance.Enabled = true;
                    Reformat.ReformatCommand.Instance.Visible = true;
                }
                else if (name == AntlrVSIX.Constants.ClassificationNameLiteral)
                {
                    AntlrLanguagePackage.Instance.Classification = AntlrVSIX.Constants.ClassificationNameLiteral;

                    GoToDefinitionCommand.Instance.Enabled = true;
                    GoToDefinitionCommand.Instance.Visible = true;

                    FindAllReferencesCommand.Instance.Enabled = true;
                    FindAllReferencesCommand.Instance.Visible = true;

                    RenameCommand.Instance.Enabled = true;
                    RenameCommand.Instance.Visible = true;

                    Reformat.ReformatCommand.Instance.Enabled = true;
                    Reformat.ReformatCommand.Instance.Visible = true;
                }
            }
        }
    }
}
