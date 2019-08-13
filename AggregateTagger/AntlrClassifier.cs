namespace AntlrVSIX.AggregateTagger
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.Package;
    using AntlrVSIX.Tagger;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text;
    using System.Collections.Generic;
    using System;

    internal sealed class AntlrClassifier : ITagger<ClassificationTag>
    {
        public ITextBuffer _buffer;
        public ITagAggregator<AntlrTokenTag> _aggregator;
        private IDictionary<AntlrTagTypes, IClassificationType> _antlrtype_to_classifiertype;

        internal AntlrClassifier(
            ITextBuffer buffer,
            ITagAggregator<AntlrTokenTag> aggregator,
            IClassificationTypeRegistryService service)
        {
            _buffer = buffer;
            _aggregator = aggregator;

            _antlrtype_to_classifiertype = new Dictionary<AntlrTagTypes, IClassificationType>();
            _antlrtype_to_classifiertype[AntlrTagTypes.Nonterminal] = service.GetClassificationType(AntlrVSIX.Constants.ClassificationNameNonterminal);
            _antlrtype_to_classifiertype[AntlrTagTypes.Terminal] = service.GetClassificationType(AntlrVSIX.Constants.ClassificationNameTerminal);
            _antlrtype_to_classifiertype[AntlrTagTypes.Comment] = service.GetClassificationType(AntlrVSIX.Constants.ClassificationNameComment);
            _antlrtype_to_classifiertype[AntlrTagTypes.Keyword] = service.GetClassificationType(AntlrVSIX.Constants.ClassificationNameKeyword);
            _antlrtype_to_classifiertype[AntlrTagTypes.Literal] = service.GetClassificationType(AntlrVSIX.Constants.ClassificationNameLiteral);
            _antlrtype_to_classifiertype[AntlrTagTypes.Other] = service.GetClassificationType("other");
            
            // Ensure package is loaded.
            AntlrLanguagePackage package = AntlrLanguagePackage.Instance;
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            ITextDocument doc = _buffer.GetTextDocument();
            string f = doc.FilePath;
            var pd = ParserDetails.Parse(_buffer.GetBufferText(), f);

            foreach (IMappingTagSpan<AntlrTokenTag> tag_span in _aggregator.GetTags(spans))
            {
                NormalizedSnapshotSpanCollection tag_spans = tag_span.Span.GetSpans(spans[0].Snapshot);
                yield return
                    new TagSpan<ClassificationTag>(tag_spans[0],
                        new ClassificationTag(_antlrtype_to_classifiertype[tag_span.Tag.TagType]));
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
    }
}
