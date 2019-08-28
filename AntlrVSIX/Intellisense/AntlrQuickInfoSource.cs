﻿
namespace AntlrVSIX
{
    using AntlrVSIX.Package;
    using AntlrVSIX.Tagger;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.GrammarDescription;
    using AntlrVSIX.Extensions;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Utilities;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System;

    [Export(typeof(IQuickInfoSourceProvider))]
    [ContentType(AntlrVSIX.Constants.ContentType)]
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

    class AntlrQuickInfoSource : IQuickInfoSource
    {
        private ITagAggregator<AntlrTokenTag> _aggregator;
        private ITextBuffer _buffer;
        private IGrammarDescription _grammar_description;
        private bool _disposed = false;

        public AntlrQuickInfoSource(ITextBuffer buffer, ITagAggregator<AntlrTokenTag> aggregator)
        {
            _aggregator = aggregator;
            _buffer = buffer;
            var doc = buffer.GetTextDocument();
            var path = doc.FilePath;
            _grammar_description = GrammarDescriptionFactory.Create(path);
        }

        public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> quick_info_content, out ITrackingSpan tracking_span)
        {
            tracking_span = null;

            if (_disposed)
                throw new ObjectDisposedException("TestQuickInfoSource");

            var trigger_point = (SnapshotPoint) session.GetTriggerPoint(_buffer.CurrentSnapshot);

            if (trigger_point == null)
                return;

            IWpfTextView view = AntlrLanguagePackage.Instance.GetActiveView();
            if (view == null) return;

            TextExtent extent = AntlrVSIX.Package.AntlrLanguagePackage.Instance.Navigator[view].GetExtentOfWord(trigger_point);
            SnapshotSpan span = extent.Span;

            foreach (IMappingTagSpan<AntlrTokenTag> cur_tag in _aggregator.GetTags(span))
            {
                int tag_type = (int)cur_tag.Tag.TagType;
                SnapshotSpan tag_span = cur_tag.Span.GetSpans(_buffer).First();
                tracking_span = _buffer.CurrentSnapshot.CreateTrackingSpan(tag_span, SpanTrackingMode.EdgeExclusive);
                quick_info_content.Add(_grammar_description.Map[tag_type]);
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
