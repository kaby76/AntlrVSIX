namespace AntlrVSIX.Package
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.FindAllReferences;
    using AntlrVSIX.GoToDefinition;
    using AntlrVSIX.GoToVisitor;
    using AntlrVSIX.Grammar;
    using LanguageServer;
    using AntlrVSIX.NextSym;
    using AntlrVSIX.Reformat;
    using AntlrVSIX.Rename;
    using AntlrVSIX.Taggers;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Text;
    using System.Linq;

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
            IGrammarDescription grammar_description = LanguageServer.GrammarDescriptionFactory.Create(path);

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

            if (grammar_description == null) return;

            var fp = view.GetFilePath();
            if (fp != null)
            {
                var gd = LanguageServer.GrammarDescriptionFactory.Create(fp);
                if (gd != null && gd.CanNextRule)
                {
                    NextSymCommand.Instance.Enabled = true;
                }
                if (gd != null && gd.CanReformat)
                {
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

            HighlightTagger.Update(view, bp);

            //  Now, check for valid classification type.
            ClassificationSpan[] c1 = AntlrVSIX.Package.AntlrLanguagePackage.Instance.Aggregator[view].GetClassificationSpans(span).ToArray();
            foreach (ClassificationSpan classification in c1)
            {
                var name = classification.ClassificationType.Classification;
                if (!grammar_description.InverseMap.TryGetValue(name, out int type))
                    continue;
                if (grammar_description.CanFindAllRefs[type])
                    FindAllReferencesCommand.Instance.Enabled = true;
                if (grammar_description.CanRename[type])
                    RenameCommand.Instance.Enabled = true;
                if (grammar_description.CanGotodef[type])
                    GoToDefinitionCommand.Instance.Enabled = true;
                if (grammar_description.CanGotovisitor[type])
                    GoToVisitorCommand.Instance.Enabled = true;
                if (grammar_description.CanReformat)
                    ReformatCommand.Instance.Enabled = true;
            }
        }
    }
}
