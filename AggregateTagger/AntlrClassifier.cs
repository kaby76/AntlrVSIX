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
        private ITextBuffer _buffer;
        private IGrammarDescription _grammar_description;
        private ITagAggregator<AntlrTokenTag> _aggregator;
        private IDictionary<int, IClassificationType> _antlrtype_to_classifiertype;

        internal AntlrClassifier(
            ITextBuffer buffer,
            ITagAggregator<AntlrTokenTag> aggregator,
            IClassificationTypeRegistryService service,
            IClassificationFormatMapService ClassificationFormatMapService)
        {
            _buffer = buffer;
            _grammar_description = AntlrToClassifierName.Instance;
            _aggregator = aggregator;
            _antlrtype_to_classifiertype = new Dictionary<int, IClassificationType>();

            for (int i = 0; i < _grammar_description.Map.Length; ++i)
            {
                var key = i;
                var val = _grammar_description.Map[i];
                var identiferClassificationType = service.GetClassificationType(val);
                var classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                        : identiferClassificationType;
                var classificationFormatMap = ClassificationFormatMapService.GetClassificationFormatMap(category: "text");
                var identifierProperties = classificationFormatMap
                    .GetExplicitTextProperties(classificationType);
                var color =  ! Themes.IsInvertedTheme() ? _grammar_description.MapColor[key]
                    : _grammar_description.MapInvertedColor[key];
                var newProperties = identifierProperties.SetForeground(color);
                classificationFormatMap.AddExplicitTextProperties(classificationType, newProperties);
            }
            
            for (int i = 0; i < _grammar_description.Map.Length; ++i)
            {
                var key = i;
                var val = _grammar_description.Map[i];
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
                        new ClassificationTag(_antlrtype_to_classifiertype[(int)tag_span.Tag.TagType]));
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
    }
}
