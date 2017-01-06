using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using AntlrLanguage.Key;
using AntlrLanguage.Tag;
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

            _state.CtrlKeyStateChanged += (sender, args) =>
            {
                if (_state.Enabled)
                    this.TryHighlightItemUnderMouse(RelativeToView(System.Windows.Input.Mouse.PrimaryDevice.GetPosition(_view.VisualElement)));
                else
                    this.SetHighlightSpan(null);
            };

            // Some other points to clear the highlight span:

            _view.LostAggregateFocus += (sender, args) => this.SetHighlightSpan(null);
            _view.VisualElement.MouseLeave += (sender, args) => this.SetHighlightSpan(null);
        }

        // Remember the location of the mouse on left button down, so we only handle left button up
        // if the mouse has stayed in a single location.
        System.Windows.Point? _mouseDownAnchorPoint;

        //public override void PostprocessMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    _mouseDownAnchorPoint = RelativeToView(e.GetPosition(_view.VisualElement));
        //}

        //public override void PostprocessMouseRightButtonUp(MouseButtonEventArgs e)
        //{
        //    base.PostprocessMouseRightButtonUp(e);
        //}

        public override void PreprocessMouseMove(MouseEventArgs e)
        {
            //if (!_mouseDownAnchorPoint.HasValue && _state.Enabled && e.LeftButton == MouseButtonState.Released)
            //{
            //    TryHighlightItemUnderMouse(RelativeToView(e.GetPosition(_view.VisualElement)));
            //}
            //else if (_mouseDownAnchorPoint.HasValue)
            //{
            //    // Check and see if this is a drag; if so, clear out the highlight.
            //    var currentMousePosition = RelativeToView(e.GetPosition(_view.VisualElement));
            //    if (InDragOperation(_mouseDownAnchorPoint.Value, currentMousePosition))
            //    {
            //        _mouseDownAnchorPoint = null;
            //        this.SetHighlightSpan(null);
            //    }
            //}
        }

        private bool InDragOperation(System.Windows.Point anchorPoint, System.Windows.Point currentPoint)
        {
            // If the mouse up is more than a drag away from the mouse down, this is a drag
            return Math.Abs(anchorPoint.X - currentPoint.X) >= SystemParameters.MinimumHorizontalDragDistance ||
                   Math.Abs(anchorPoint.Y - currentPoint.Y) >= SystemParameters.MinimumVerticalDragDistance;
        }

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
            e.Handled = true;
            _mouseDownAnchorPoint = RelativeToView(e.GetPosition(_view.VisualElement));
            System.Windows.Point p = _mouseDownAnchorPoint ?? default(System.Windows.Point);
            if (p == default(System.Windows.Point)) return;
            SnapshotPoint? where = ConvertPointToBufferIndex(p);
            SnapshotPoint wheren = where ?? default(SnapshotPoint);
            if (wheren == default(SnapshotPoint)) return;
            ITextSnapshot sn = wheren.Snapshot;
            
            TextExtent extent = _navigator.GetExtentOfWord(wheren);
            SnapshotSpan span = extent.Span;
            SnapshotSpan sp2 = _navigator.GetSpanOfEnclosing(span);           

            //  Now, check for valid classification type.
            Command1 cmd = Command1.Instance;

            var c1 = _aggregator.GetClassificationSpans(span).ToArray();

            Command1.Instance.Enable = false;
            Command1.Instance.Symbol = default(SnapshotSpan);
            Command1.Instance.Classification = default(string);
            Command1.Instance.View = default(ITextView);

            // classification aggregator passed here does not work. Pick up view classifier.
            foreach (ClassificationSpan classification in c1)
            {
                var name = classification.ClassificationType.Classification.ToLower();
                if (name == "terminal")
                {
                    TextExtent ee = extent;
                    Command1.Instance.Enable = true;
                    Command1.Instance.Symbol = span;
                    Command1.Instance.Classification = "terminal";
                    Command1.Instance.View = this._view;
                }
                else if (name == "nonterminal")
                {
                    TextExtent ee = extent;
                    Command1.Instance.Enable = true;
                    Command1.Instance.Symbol = span;
                    Command1.Instance.Classification = "nonterminal";
                    Command1.Instance.View = this._view;
                }
            }
        }

        //public override void PreprocessMouseUp(MouseButtonEventArgs e)
        //{
        //    if (_mouseDownAnchorPoint.HasValue && _state.Enabled)
        //    {
        //        var currentMousePosition = RelativeToView(e.GetPosition(_view.VisualElement));

        //        if (!InDragOperation(_mouseDownAnchorPoint.Value, currentMousePosition))
        //        {
        //            _state.Enabled = false;

        //            this.SetHighlightSpan(null);
        //            _view.Selection.Clear();
        //            this.DispatchGoToDef();

        //            e.Handled = true;
        //        }
        //    }

        //    _mouseDownAnchorPoint = null;
        //}

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

                // Quick check - if the mouse is still inside the current underline span, we're already set
                var currentSpan = CurrentUnderlineSpan;
                if (currentSpan.HasValue && currentSpan.Value.Contains(bufferPosition.Value))
                {
                    return bufferPosition;
                }

                var extent = _navigator.GetExtentOfWord(bufferPosition.Value);
                if (!extent.IsSignificant)
                    return default(SnapshotPoint?);
                return bufferPosition;
            }
            catch (Exception) { }
            return default(SnapshotPoint?);
        }

        bool TryHighlightItemUnderMouse(Point position)
        {
            bool updated = false;

            try
            {
                var line = _view.TextViewLines.GetTextViewLineContainingYCoordinate(position.Y);
                if (line == null)
                    return false;

                var bufferPosition = line.GetBufferPositionFromXCoordinate(position.X);

                if (!bufferPosition.HasValue)
                    return false;

                // Quick check - if the mouse is still inside the current underline span, we're already set
                var currentSpan = CurrentUnderlineSpan;
                if (currentSpan.HasValue && currentSpan.Value.Contains(bufferPosition.Value))
                {
                    updated = true;
                    return true;
                }

                var extent = _navigator.GetExtentOfWord(bufferPosition.Value);
                if (!extent.IsSignificant)
                    return false;

                // For C#, we ignore namespaces after using statements - GoToDef will fail for those
                if (_view.TextBuffer.ContentType.IsOfType("csharp"))
                {
                    string lineText = bufferPosition.Value.GetContainingLine().GetText().Trim();
                    if (lineText.StartsWith("using", StringComparison.OrdinalIgnoreCase))
                        return false;
                }

                //  Now, check for valid classification type.  C# and C++ (at least) classify the things we are interested
                // in as either "identifier" or "user types" (though "identifier" will yield some false positives).  VB, unfortunately,
                // doesn't classify identifiers.
                foreach (var classification in _aggregator.GetClassificationSpans(extent.Span))
                {
                    var name = classification.ClassificationType.Classification.ToLower();
                    if ((name.Contains("identifier") || name.Contains("user types")) &&
                        SetHighlightSpan(classification.Span))
                    {
                        updated = true;
                        return true;
                    }
                }

                // No update occurred, so return false
                return false;
            }
            finally
            {
                if (!updated)
                    SetHighlightSpan(null);
            }
        }

        SnapshotSpan? CurrentUnderlineSpan
        {
            get
            {
                //var classifier = UnderlineClassifierProvider.GetClassifierForView(_view);
                //if (classifier != null && classifier.CurrentUnderlineSpan.HasValue)
                //    return classifier.CurrentUnderlineSpan.Value.TranslateTo(_view.TextSnapshot, SpanTrackingMode.EdgeExclusive);
                //else
                return null;
            }
        }

        bool SetHighlightSpan(SnapshotSpan? span)
        {
            //var classifier = UnderlineClassifierProvider.GetClassifierForView(_view);
            //if (classifier != null)
            //{
            //    if (span.HasValue)
            //        System.Windows.Input.Mouse.OverrideCursor = Cursors.Hand;
            //    else
            //        System.Windows.Input.Mouse.OverrideCursor = null;

            //    classifier.SetUnderlineSpan(span);
            //    return true;
            //}

            return false;
        }

        bool DispatchGoToDef()
        {
            Guid cmdGroup = VSConstants.GUID_VSStandardCommandSet97;
            int hr = _commandTarget.Exec(ref cmdGroup,
                                         (uint)VSConstants.VSStd97CmdID.GotoDefn,
                                         (uint)OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT,
                                         System.IntPtr.Zero,
                                         System.IntPtr.Zero);
            return ErrorHandler.Succeeded(hr);
        }
    }
}
