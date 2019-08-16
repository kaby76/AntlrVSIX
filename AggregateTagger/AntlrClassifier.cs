namespace AntlrVSIX.AggregateTagger
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.Package;
    using AntlrVSIX.Tagger;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text;
    using System.Collections.Generic;
    using System;
    using Microsoft.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    class Themes
    {
        public static bool IsInvertedTheme()
        {
            return VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey).GetBrightness() < 0.5;
        }
    }

    internal sealed class AntlrClassifier : ITagger<ClassificationTag>
    {
        public ITextBuffer _buffer;
        public ITagAggregator<AntlrTokenTag> _aggregator;
        private IDictionary<AntlrTagTypes, IClassificationType> _antlrtype_to_classifiertype;

        internal AntlrClassifier(
            ITextBuffer buffer,
            ITagAggregator<AntlrTokenTag> aggregator,
            IClassificationTypeRegistryService service,
            IClassificationFormatMapService ClassificationFormatMapService)
        {
            foreach (var kvp in AntlrToClassifierName.Map)
            {
                var key = kvp.Key;
                var val = kvp.Value;
                var identiferClassificationType = service.GetClassificationType(val);
                var classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                        : identiferClassificationType;
                var classificationFormatMap = ClassificationFormatMapService.GetClassificationFormatMap(category: "text");
                var identifierProperties = classificationFormatMap
                    .GetExplicitTextProperties(classificationType);
                var color =  ! Themes.IsInvertedTheme() ? AntlrToClassifierName.MapColor[key]
                    : AntlrToClassifierName.MapInvertedColor[key];
                var newProperties = identifierProperties.SetForeground(color);
                classificationFormatMap.AddExplicitTextProperties(classificationType, newProperties);
            }
            
            _buffer = buffer;
            _aggregator = aggregator;
            _antlrtype_to_classifiertype = new Dictionary<AntlrTagTypes, IClassificationType>();
            foreach (var kvp in AntlrToClassifierName.Map)
            {
                var key = kvp.Key;
                var val = kvp.Value;
                _antlrtype_to_classifiertype[key] = service.GetClassificationType(val);
            }            
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
