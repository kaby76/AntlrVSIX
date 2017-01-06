using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using AntlrLanguage.Tag;

namespace AntlrLanguage
{
    internal sealed class AntlrClassifier : ITagger<ClassificationTag>
    {
        ITextBuffer _buffer = null;
        ITagAggregator<AntlrTokenTag> _aggregator;
        IDictionary<AntlrTokenTypes, IClassificationType> _antlrTypes;

        internal AntlrClassifier(ITextBuffer buffer,
                               ITagAggregator<AntlrTokenTag> antlrTagAggregator,
                               IClassificationTypeRegistryService typeService)
        {
            _buffer = buffer;
            _aggregator = antlrTagAggregator;
            _antlrTypes = new Dictionary<AntlrTokenTypes, IClassificationType>();
            _antlrTypes[AntlrTokenTypes.Nonterminal] = typeService.GetClassificationType("nonterminal");
            _antlrTypes[AntlrTokenTypes.Terminal] = typeService.GetClassificationType("terminal");
            _antlrTypes[AntlrTokenTypes.Comment] = typeService.GetClassificationType("acomment");
            _antlrTypes[AntlrTokenTypes.Keyword] = typeService.GetClassificationType("akeyword");
            _antlrTypes[AntlrTokenTypes.Other] = typeService.GetClassificationType("other");
            // Ensure package is loaded.
            var package = VSPackageCommandCodeWindowContextMenu.Instance;
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
