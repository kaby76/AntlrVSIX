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

            // Whack any old values that cursor points to.
            GoToDefinitionCommand.Instance.Enabled = false;
            GoToDefinitionCommand.Instance.Visible = false;
            GoToDefinitionCommand.Instance.Symbol = default(SnapshotSpan);
            GoToDefinitionCommand.Instance.Classification = default(string);
            GoToDefinitionCommand.Instance.View = default(ITextView);

            FindAllReferencesCommand.Instance.Enabled = false;
            FindAllReferencesCommand.Instance.Visible = false;
            FindAllReferencesCommand.Instance.Symbol = default(SnapshotSpan);
            FindAllReferencesCommand.Instance.Classification = default(string);
            FindAllReferencesCommand.Instance.View = default(ITextView);

            RenameCommand.Instance.Enabled = false;
            RenameCommand.Instance.Visible = false;
            RenameCommand.Instance.Symbol = default(SnapshotSpan);
            RenameCommand.Instance.Classification = default(string);
            RenameCommand.Instance.View = default(ITextView);

            Reformat.ReformatCommand.Instance.Enabled = false;
            Reformat.ReformatCommand.Instance.Visible = false;
            RenameCommand.Instance.View = default(ITextView);

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
                    GoToDefinitionCommand.Instance.Enabled = true;
                    GoToDefinitionCommand.Instance.Visible = true;
                    GoToDefinitionCommand.Instance.Symbol = span;
                    GoToDefinitionCommand.Instance.Classification = AntlrVSIX.Constants.ClassificationNameTerminal;
                    GoToDefinitionCommand.Instance.View = _view;

                    FindAllReferencesCommand.Instance.Enabled = true;
                    FindAllReferencesCommand.Instance.Visible = true;
                    FindAllReferencesCommand.Instance.Symbol = span;
                    FindAllReferencesCommand.Instance.Classification = AntlrVSIX.Constants.ClassificationNameTerminal;
                    FindAllReferencesCommand.Instance.View = _view;

                    RenameCommand.Instance.Enabled = true;
                    RenameCommand.Instance.Visible = true;
                    RenameCommand.Instance.Symbol = span;
                    RenameCommand.Instance.Classification = AntlrVSIX.Constants.ClassificationNameTerminal;
                    RenameCommand.Instance.View = _view;

                    Reformat.ReformatCommand.Instance.Enabled = true;
                    Reformat.ReformatCommand.Instance.Visible = true;
                    Reformat.ReformatCommand.Instance.Symbol = span;
                    Reformat.ReformatCommand.Instance.Classification = AntlrVSIX.Constants.ClassificationNameTerminal;
                    Reformat.ReformatCommand.Instance.View = _view;
                }
                else if (name == AntlrVSIX.Constants.ClassificationNameNonterminal)
                {
                    GoToDefinitionCommand.Instance.Enabled = true;
                    GoToDefinitionCommand.Instance.Visible = true;
                    GoToDefinitionCommand.Instance.Symbol = span;
                    GoToDefinitionCommand.Instance.Classification = AntlrVSIX.Constants.ClassificationNameNonterminal;
                    GoToDefinitionCommand.Instance.View = _view;

                    FindAllReferencesCommand.Instance.Enabled = true;
                    FindAllReferencesCommand.Instance.Visible = true;
                    FindAllReferencesCommand.Instance.Symbol = span;
                    FindAllReferencesCommand.Instance.Classification = AntlrVSIX.Constants.ClassificationNameNonterminal;
                    FindAllReferencesCommand.Instance.View = _view;

                    RenameCommand.Instance.Enabled = true;
                    RenameCommand.Instance.Visible = true;
                    RenameCommand.Instance.Symbol = span;
                    RenameCommand.Instance.Classification = AntlrVSIX.Constants.ClassificationNameNonterminal;
                    RenameCommand.Instance.View = _view;

                    Reformat.ReformatCommand.Instance.Enabled = true;
                    Reformat.ReformatCommand.Instance.Visible = true;
                    Reformat.ReformatCommand.Instance.Symbol = span;
                    Reformat.ReformatCommand.Instance.Classification = AntlrVSIX.Constants.ClassificationNameNonterminal;
                    Reformat.ReformatCommand.Instance.View = _view;
                }
                else if (name == AntlrVSIX.Constants.ClassificationNameLiteral)
                {
                    GoToDefinitionCommand.Instance.Enabled = true;
                    GoToDefinitionCommand.Instance.Visible = true;
                    GoToDefinitionCommand.Instance.Symbol = span;
                    GoToDefinitionCommand.Instance.Classification = AntlrVSIX.Constants.ClassificationNameLiteral;
                    GoToDefinitionCommand.Instance.View = _view;

                    FindAllReferencesCommand.Instance.Enabled = true;
                    FindAllReferencesCommand.Instance.Visible = true;
                    FindAllReferencesCommand.Instance.Symbol = span;
                    FindAllReferencesCommand.Instance.Classification = AntlrVSIX.Constants.ClassificationNameLiteral;
                    FindAllReferencesCommand.Instance.View = _view;

                    RenameCommand.Instance.Enabled = true;
                    RenameCommand.Instance.Visible = true;
                    RenameCommand.Instance.Symbol = span;
                    RenameCommand.Instance.Classification = AntlrVSIX.Constants.ClassificationNameLiteral;
                    RenameCommand.Instance.View = _view;

                    Reformat.ReformatCommand.Instance.Enabled = true;
                    Reformat.ReformatCommand.Instance.Visible = true;
                    Reformat.ReformatCommand.Instance.Symbol = span;
                    Reformat.ReformatCommand.Instance.Classification = AntlrVSIX.Constants.ClassificationNameLiteral;
                    Reformat.ReformatCommand.Instance.View = _view;
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
                    return default(SnapshotPoint?);

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
