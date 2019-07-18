using AntlrVSIX.Navigate;
using AntlrVSIX.NextSym;
using AntlrVSIX.Reformat;

namespace AntlrVSIX.Mouse
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.FindAllReferences;
    using AntlrVSIX.GoToDefintion;
    using AntlrVSIX.Keyboard;
    using AntlrVSIX.Rename;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Text;
    using Point = System.Windows.Point;
    using System.Linq;
    using System.Windows.Input;
    using System;

    internal sealed class GoToDefMouseHandler : MouseProcessorBase
    {
        private IWpfTextView _view;
        private IClassifier _aggregator;
        private ITextStructureNavigator _navigator;
        private SVsServiceProvider _service_provider;

        public GoToDefMouseHandler(IWpfTextView view,
            IOleCommandTarget commandTarget,
            SVsServiceProvider serviceProvider,
            IClassifier aggregator,
            ITextStructureNavigator navigator,
            CtrlKeyState state)
        {
            _view = view;
            _aggregator = aggregator;
            _navigator = navigator;
            _service_provider = serviceProvider;
        }

        // Remember the location of the mouse on left button down, so we only handle left button up
        // if the mouse has stayed in a single location.
        System.Windows.Point? _mouseDownAnchorPoint;

        public override void PreprocessMouseLeave(MouseEventArgs e)
        {
            _mouseDownAnchorPoint = null;
        }

        public override void PreprocessMouseUp(MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        public override void PostprocessMouseRightButtonDown(MouseButtonEventArgs e)
        {
            if (_view == null)
                return;

            if (GoToDefinitionCommand.Instance == null ||
                FindAllReferencesCommand.Instance == null ||
                RenameCommand.Instance == null ||
                Reformat.ReformatCommand.Instance == null)
                return;

            AntlrLanguagePackage.Instance.Span = default(SnapshotSpan);
            AntlrLanguagePackage.Instance.Classification = default(string);
            AntlrLanguagePackage.Instance.View = default(ITextView);

            // Whack any old values that cursor points to.
            GoToDefinitionCommand.Instance.Enabled = false;
            GoToDefinitionCommand.Instance.Visible = false;

            FindAllReferencesCommand.Instance.Enabled = false;
            FindAllReferencesCommand.Instance.Visible = false;

            RenameCommand.Instance.Enabled = false;
            RenameCommand.Instance.Visible = false;

            Reformat.ReformatCommand.Instance.Enabled = false;
            Reformat.ReformatCommand.Instance.Visible = false;

            var fp = _view.GetFilePath();
            if (fp != null)
            {
                string suffix = System.IO.Path.GetExtension(fp);
                if (suffix.Equals(".g4") || suffix.Equals(".G4"))
                {
                    GoToDefinitionCommand.Instance.Visible = true;
                    FindAllReferencesCommand.Instance.Visible = true;
                    RenameCommand.Instance.Visible = true;
                    Reformat.ReformatCommand.Instance.Enabled = true;
                    Reformat.ReformatCommand.Instance.Visible = true;
                }
            }

            // No matter what, say we handled event, otherwise we get all sorts of pop-up errors.
            e.Handled = true;

            AntlrLanguagePackage.Instance.View = _view;

            // Find details of the Antlr symbol pointed to.
            _mouseDownAnchorPoint = RelativeToView(e.GetPosition(_view.VisualElement));
            System.Windows.Point point = _mouseDownAnchorPoint ?? default(System.Windows.Point);
            if (point == default(System.Windows.Point)) return;
            SnapshotPoint? nullable_where = ConvertPointToBufferIndex(point);
            SnapshotPoint where = nullable_where ?? default(SnapshotPoint);
            if (where == default(SnapshotPoint)) return;
            TextExtent extent = _navigator.GetExtentOfWord(where);
            SnapshotSpan span = extent.Span;

            //  Now, check for valid classification type.
            ClassificationSpan[] c1 = _aggregator.GetClassificationSpans(span).ToArray();
            foreach (ClassificationSpan classification in c1)
            {
                var name = classification.ClassificationType.Classification.ToLower();
                if (name == AntlrVSIX.Constants.ClassificationNameTerminal)
                {

                    AntlrLanguagePackage.Instance.Span = span;
                    AntlrLanguagePackage.Instance.Classification = AntlrVSIX.Constants.ClassificationNameTerminal;

                    GoToDefinitionCommand.Instance.Enabled = true;
                    GoToDefinitionCommand.Instance.Visible = true;

                    FindAllReferencesCommand.Instance.Enabled = true;
                    FindAllReferencesCommand.Instance.Visible = true;

                    RenameCommand.Instance.Enabled = true;
                    RenameCommand.Instance.Visible = true;

                    Reformat.ReformatCommand.Instance.Enabled = true;
                    Reformat.ReformatCommand.Instance.Visible = true;

                    NextSymCommand.Instance.Enabled = true;
                }
                else if (name == AntlrVSIX.Constants.ClassificationNameNonterminal)
                {
                    AntlrLanguagePackage.Instance.Span = span;
                    AntlrLanguagePackage.Instance.Classification = AntlrVSIX.Constants.ClassificationNameNonterminal;

                    GoToDefinitionCommand.Instance.Enabled = true;
                    GoToDefinitionCommand.Instance.Visible = true;

                    FindAllReferencesCommand.Instance.Enabled = true;
                    FindAllReferencesCommand.Instance.Visible = true;

                    RenameCommand.Instance.Enabled = true;
                    RenameCommand.Instance.Visible = true;

                    Reformat.ReformatCommand.Instance.Enabled = true;
                    Reformat.ReformatCommand.Instance.Visible = true;

                    NextSymCommand.Instance.Enabled = true;
                }
                else if (name == AntlrVSIX.Constants.ClassificationNameLiteral)
                {
                    AntlrLanguagePackage.Instance.Span = span;
                    AntlrLanguagePackage.Instance.Classification = AntlrVSIX.Constants.ClassificationNameLiteral;

                    GoToDefinitionCommand.Instance.Enabled = true;
                    GoToDefinitionCommand.Instance.Visible = true;

                    FindAllReferencesCommand.Instance.Enabled = true;
                    FindAllReferencesCommand.Instance.Visible = true;

                    RenameCommand.Instance.Enabled = true;
                    RenameCommand.Instance.Visible = true;

                    Reformat.ReformatCommand.Instance.Enabled = true;
                    Reformat.ReformatCommand.Instance.Visible = true;

                    NextSymCommand.Instance.Enabled = true;
                }
            }
        }

        public override void PostprocessMouseDown(MouseButtonEventArgs e)
        {
            if (_view == null)
                return;

            if (GoToDefinitionCommand.Instance == null ||
                FindAllReferencesCommand.Instance == null ||
                RenameCommand.Instance == null ||
                Reformat.ReformatCommand.Instance == null)
                return;

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

            // No matter what, say we handled event, otherwise we get all sorts of pop-up errors.
            e.Handled = true;

            AntlrLanguagePackage.Instance.View = _view;

            var fp = _view.GetFilePath();
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
            _mouseDownAnchorPoint = RelativeToView(e.GetPosition(_view.VisualElement));
            System.Windows.Point point = _mouseDownAnchorPoint ?? default(System.Windows.Point);
            if (point == default(System.Windows.Point)) return;
            SnapshotPoint? nullable_where = ConvertPointToBufferIndex(point);
            SnapshotPoint where = nullable_where ?? default(SnapshotPoint);
            if (where == default(SnapshotPoint)) return;
            TextExtent extent = _navigator.GetExtentOfWord(where);
            SnapshotSpan span = extent.Span;

            AntlrLanguagePackage.Instance.Span = span;

            //  Now, check for valid classification type.
            ClassificationSpan[] c1 = _aggregator.GetClassificationSpans(span).ToArray();
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

        Point RelativeToView(Point position)
        {
            return new Point(position.X + _view.ViewportLeft, position.Y + _view.ViewportTop);
        }

        public SnapshotPoint? ConvertPointToBufferIndex(Point position)
        {
            try
            {
                var line = _view.TextViewLines.GetTextViewLineContainingYCoordinate(position.Y);
                if (line == null)
                    return default(SnapshotPoint?);

                SnapshotPoint? bufferPosition = line.GetBufferPositionFromXCoordinate(position.X);

                if (!bufferPosition.HasValue)
                {
                    // Assume beginning of line.
                    return new SnapshotPoint(line.Snapshot, line.Start);
                }

                var extent = _navigator.GetExtentOfWord(bufferPosition.Value);
                if (!extent.IsSignificant)
                    return default(SnapshotPoint?);
                return bufferPosition;
            }
            catch (Exception) { }
            return default(SnapshotPoint?);
        }
    }
}
