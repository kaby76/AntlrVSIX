namespace AntlrVSIX.Intellisense
{
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text;
    using System.Collections.Generic;

    internal class TemplateQuickInfoController : IIntellisenseController
    {
        private ITextView _text_view;
        private IList<ITextBuffer> _subject_buffers;
        private TemplateQuickInfoControllerProvider _component_context;
        private IQuickInfoSession _session;

        internal TemplateQuickInfoController(ITextView text_view, IList<ITextBuffer> subject_buffers, TemplateQuickInfoControllerProvider component_context)
        {
            _text_view = text_view;
            _subject_buffers = subject_buffers;
            _component_context = component_context;

            _text_view.MouseHover += OnTextViewMouseHover;
        }

        public void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
        }

        public void DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
        }

        public void Detach(ITextView text_view)
        {
            if (_text_view == text_view)
            {
                _text_view.MouseHover -= OnTextViewMouseHover;
                _text_view = null;
            }
        }

        private void OnTextViewMouseHover(object sender, MouseHoverEventArgs e)
        {
            SnapshotPoint? point = GetMousePosition(new SnapshotPoint(_text_view.TextSnapshot, e.Position));

            if (point != null)
            {
                ITrackingPoint triggerPoint = point.Value.Snapshot.CreateTrackingPoint(point.Value.Position,
                    PointTrackingMode.Positive);

                // Find the broker for this buffer

                if (!_component_context.QuickInfoBroker.IsQuickInfoActive(_text_view))
                {
                    _session = _component_context.QuickInfoBroker.CreateQuickInfoSession(_text_view, triggerPoint, true);
                    _session.Start();
                }
            }
        }

        private SnapshotPoint? GetMousePosition(SnapshotPoint topPosition)
        {
            return _text_view.BufferGraph.MapDownToFirstMatch
                (
                topPosition,
                PointTrackingMode.Positive,
                snapshot => _subject_buffers.Contains(snapshot.TextBuffer),
                PositionAffinity.Predecessor
                );
        }
    }
}