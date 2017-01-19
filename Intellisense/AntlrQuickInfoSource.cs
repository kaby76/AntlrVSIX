namespace AntlrVSIX
{
    using AntlrVSIX.Tagger;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Utilities;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System;

    /// <summary>
    /// Factory for quick info sources
    /// </summary>
    [Export(typeof(IQuickInfoSourceProvider))]
    [ContentType(Constants.ContentType)]
    [Name("AntlrQuickInfo")]
    class AntlrQuickInfoSourceProvider : IQuickInfoSourceProvider
    {

        [Import]
        IBufferTagAggregatorFactoryService aggService = null;

        public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer)
        {
            return new AntlrQuickInfoSource(textBuffer, aggService.CreateTagAggregator<AntlrTokenTag>(textBuffer));
        }
    }

    /// <summary>
    /// Provides QuickInfo information to be displayed in a text buffer
    /// </summary>
    class AntlrQuickInfoSource : IQuickInfoSource
    {
        private ITagAggregator<AntlrTokenTag> _aggregator;
        private ITextBuffer _buffer;
        private bool _disposed = false;


        public AntlrQuickInfoSource(ITextBuffer buffer, ITagAggregator<AntlrTokenTag> aggregator)
        {
            _aggregator = aggregator;
            _buffer = buffer;
        }

        /// <summary>
        /// Determine which pieces of Quickinfo content should be displayed
        /// </summary>
        public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> quick_info_content, out ITrackingSpan tracking_span)
        {
            tracking_span = null;

            if (_disposed)
                throw new ObjectDisposedException("TestQuickInfoSource");

            var trigger_point = (SnapshotPoint) session.GetTriggerPoint(_buffer.CurrentSnapshot);

            if (trigger_point == null)
                return;

            foreach (IMappingTagSpan<AntlrTokenTag> cur_tag in _aggregator.GetTags(new SnapshotSpan(trigger_point, trigger_point)))
            {
                if (cur_tag.Tag.TagType == AntlrTagTypes.Terminal)
                {
                    SnapshotSpan tag_span = cur_tag.Span.GetSpans(_buffer).First();
                    tracking_span = _buffer.CurrentSnapshot.CreateTrackingSpan(tag_span, SpanTrackingMode.EdgeExclusive);
                    quick_info_content.Add("Terminal");
                }
                else if (cur_tag.Tag.TagType == AntlrTagTypes.Nonterminal)
                {
                    SnapshotSpan tag_span = cur_tag.Span.GetSpans(_buffer).First();
                    tracking_span = _buffer.CurrentSnapshot.CreateTrackingSpan(tag_span, SpanTrackingMode.EdgeExclusive);
                    quick_info_content.Add("Nonterminal");
                }
                else if (cur_tag.Tag.TagType == AntlrTagTypes.Comment)
                {
                    SnapshotSpan tag_span = cur_tag.Span.GetSpans(_buffer).First();
                    tracking_span = _buffer.CurrentSnapshot.CreateTrackingSpan(tag_span, SpanTrackingMode.EdgeExclusive);
                    quick_info_content.Add("Comment");
                }
                else if (cur_tag.Tag.TagType == AntlrTagTypes.Keyword)
                {
                    SnapshotSpan tag_span = cur_tag.Span.GetSpans(_buffer).First();
                    tracking_span = _buffer.CurrentSnapshot.CreateTrackingSpan(tag_span, SpanTrackingMode.EdgeExclusive);
                    quick_info_content.Add("Keyword");
                }
                else if (cur_tag.Tag.TagType == AntlrTagTypes.Literal)
                {
                    SnapshotSpan tag_span = cur_tag.Span.GetSpans(_buffer).First();
                    tracking_span = _buffer.CurrentSnapshot.CreateTrackingSpan(tag_span, SpanTrackingMode.EdgeExclusive);
                    quick_info_content.Add("Literal");
                }
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}

