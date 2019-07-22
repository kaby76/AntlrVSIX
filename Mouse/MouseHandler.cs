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

    internal sealed class MouseHandler : MouseProcessorBase
    {
        private IWpfTextView _view;
        private IClassifier _aggregator;
        private ITextStructureNavigator _navigator;
        private SVsServiceProvider _service_provider;

        public MouseHandler(IWpfTextView view,
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
            AntlrVSIX.Navigate.AntlrLanguagePackage.Instance.Aggregator[view] = aggregator;
            AntlrVSIX.Navigate.AntlrLanguagePackage.Instance.Navigator[view] = navigator;
            AntlrVSIX.Navigate.AntlrLanguagePackage.Instance.ServiceProvider[view] = serviceProvider;
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
            AntlrVSIX.Navigate.MenuEnableProvider.ResetMenus();
        }

        public override void PostprocessMouseDown(MouseButtonEventArgs e)
        {
            AntlrVSIX.Navigate.MenuEnableProvider.ResetMenus();
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
