namespace AntlrVSIX.Classification
{
    using System;
    using System.Collections.Generic;
    using AntlrVSIX.Navigate;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using AntlrVSIX.Tagger;
    using Microsoft.VisualStudio.Text.Editor;

    internal sealed class AntlrClassifier : ITagger<ClassificationTag>
    {
        public ITextBuffer _buffer = null;
        public ITextView _view = null;
        public ITagAggregator<AntlrTokenTag> _aggregator;
        IDictionary<AntlrTokenTypes, IClassificationType> _antlrTypes;
        public static Dictionary<ITextBuffer, AntlrClassifier> _buffer_to_classifier = new Dictionary<ITextBuffer, AntlrClassifier>();

        internal AntlrClassifier(
            ITextView view,
            ITextBuffer buffer,
            ITagAggregator<AntlrTokenTag> antlrTagAggregator,
            IClassificationTypeRegistryService typeService)
        {
            _view = view;
            _buffer = buffer;
            _buffer_to_classifier[buffer] = this;

            _aggregator = antlrTagAggregator;
            _antlrTypes = new Dictionary<AntlrTokenTypes, IClassificationType>();
            _antlrTypes[AntlrTokenTypes.Nonterminal] = typeService.GetClassificationType(Constants.ClassificationNameNonterminal);
            _antlrTypes[AntlrTokenTypes.Terminal] = typeService.GetClassificationType(Constants.ClassificationNameTerminal);
            _antlrTypes[AntlrTokenTypes.Comment] = typeService.GetClassificationType(Constants.ClassificationNameComment);
            _antlrTypes[AntlrTokenTypes.Keyword] = typeService.GetClassificationType(Constants.ClassificationNameKeyword);
            _antlrTypes[AntlrTokenTypes.Literal] = typeService.GetClassificationType(Constants.ClassificationNameLiteral);
            _antlrTypes[AntlrTokenTypes.Other] = typeService.GetClassificationType("other");
            // Ensure package is loaded.
            var package = AntlrLanguagePackage.Instance;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (var tagSpan in _aggregator.GetTags(spans))
            {
                var tagSpans = tagSpan.Span.GetSpans(spans[0].Snapshot);
                yield return
                    new TagSpan<ClassificationTag>(tagSpans[0],
                        new ClassificationTag(_antlrTypes[tagSpan.Tag.type]));
            }
        }
    }
}
