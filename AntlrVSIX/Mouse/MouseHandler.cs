namespace AntlrVSIX.Mouse
{
    using AntlrVSIX.Keyboard;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Text;
    using Point = System.Windows.Point;
    using System.Windows.Input;
    using System;

    internal sealed class MouseHandler : MouseProcessorBase
    {
        private IWpfTextView _view;
        private ITextStructureNavigator _navigator;

        public MouseHandler(IWpfTextView view,
            IOleCommandTarget commandTarget,
            SVsServiceProvider serviceProvider,
            IClassifier aggregator,
            ITextStructureNavigator navigator,
            CtrlKeyState state)
        {
            _view = view;
            _navigator = navigator;
            AntlrVSIX.Package.AntlrLanguagePackage.Instance.Aggregator[view] = aggregator;
            AntlrVSIX.Package.AntlrLanguagePackage.Instance.Navigator[view] = navigator;
            AntlrVSIX.Package.AntlrLanguagePackage.Instance.ServiceProvider[view] = serviceProvider;
        }

        public override void PreprocessMouseUp(MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        public override void PostprocessMouseRightButtonDown(MouseButtonEventArgs e)
        {
            AntlrVSIX.Package.Menus.ResetMenus();
        }

        public override void PostprocessMouseDown(MouseButtonEventArgs e)
        {
            AntlrVSIX.Package.Menus.ResetMenus();
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
