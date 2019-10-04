namespace AntlrVSIX.AggregateTagger
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.GrammarDescription;
    using AntlrVSIX.Package;
    using AntlrVSIX.Tagger;
    using Microsoft.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

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
            var xxx = System.Windows.Media.Colors.Red;
            _buffer = buffer;
            _aggregator = aggregator;
            var doc = _buffer.GetTextDocument();
            if (doc == null) return;
            var ffn = doc.FilePath;
            if (ffn == null) return;
            _grammar_description = GrammarDescriptionFactory.Create(ffn);
            if (_grammar_description == null) return;
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
            if (_grammar_description == null) yield break;
            if (_grammar_description == null) throw new Exception();
            ITextDocument doc = _buffer.GetTextDocument();
            string f = doc.FilePath;

            var item = AntlrVSIX.GrammarDescription.Workspace.Instance.FindDocumentFullName(f);
            if (item == null) yield break;
            item.Code = _buffer.GetBufferText();
            var pd = ParserDetailsFactory.Create(item);
            pd.Parse(item);

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
