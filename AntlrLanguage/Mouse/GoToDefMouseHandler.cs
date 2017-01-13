using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using AntlrLanguage.Key;
using AntlrLanguage.Navigate;
using Point = System.Windows.Point;

namespace AntlrLanguage.Mouse
{
    internal sealed class GoToDefMouseHandler : MouseProcessorBase
    {
        IWpfTextView _view;
        CtrlKeyState _state;
        IClassifier _aggregator;
        ITextStructureNavigator _navigator;
        IOleCommandTarget _commandTarget;

        public GoToDefMouseHandler(IWpfTextView view, IOleCommandTarget commandTarget, IClassifier aggregator,
                                   ITextStructureNavigator navigator, CtrlKeyState state)
        {
            _view = view;
            _commandTarget = commandTarget;
            _state = state;
            _aggregator = aggregator;
            _navigator = navigator;
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
            // Whack any old values.
            Command1.Instance.Enable = false;
            Command1.Instance.Symbol = default(SnapshotSpan);
            Command1.Instance.Classification = default(string);
            Command1.Instance.View = default(ITextView);

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
                if (name == "terminal")
                {
                    Command1.Instance.Enable = true;
                    Command1.Instance.Symbol = span;
                    Command1.Instance.Classification = "terminal";
                    Command1.Instance.View = _view;
                }
                else if (name == "nonterminal")
                {
                    Command1.Instance.Enable = true;
                    Command1.Instance.Symbol = span;
                    Command1.Instance.Classification = "nonterminal";
                    Command1.Instance.View = _view;
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
