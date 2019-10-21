namespace AntlrVSIX.AggregateTagger
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Package;
    using AntlrVSIX.Tagger;
    using LanguageServer;
    using Microsoft.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

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
            _buffer.Changed += new EventHandler<TextContentChangedEventArgs>(OnTextChanged);
            _aggregator = aggregator;
            var ffn = _buffer.GetFFN().Result;
            if (ffn == null) return;
            _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
            if (_grammar_description == null) return;
            var item = Workspaces.Workspace.Instance.FindDocument(ffn);
            if (item == null)
            {
                AntlrVSIX.File.Loader.LoadAsync().Wait();
                var to_do = LanguageServer.Module.Compile();
                item = Workspaces.Workspace.Instance.FindDocument(ffn);
            }
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
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                var newProperties = identifierProperties.SetForeground(newColor);
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
            string f = _buffer.GetFFN().Result;
            var item = Workspaces.Workspace.Instance.FindDocument(f);
            if (item == null) yield break;
            item.Code = _buffer.GetBufferText();
            var pd = ParserDetailsFactory.Create(item);

            foreach (IMappingTagSpan<AntlrTokenTag> tag_span in _aggregator.GetTags(spans))
            {
                NormalizedSnapshotSpanCollection tag_spans = tag_span.Span.GetSpans(spans[0].Snapshot);
                yield return
                    new TagSpan<ClassificationTag>(tag_spans[0],
                        new ClassificationTag(_antlrtype_to_classifiertype[(int)tag_span.Tag.TagType]));
            }
        }

#pragma warning disable CS0067
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore CS0067

        static CancellationTokenSource source;
        static Task<int> task;

        async void OnTextChanged(object sender, TextContentChangedEventArgs args)
        {
            var s = source;
            var t = task;
            if (s != null)
            {
                s.Cancel();
            }
            {
                s = source = new CancellationTokenSource();
                t = task = Task.Run(async delegate
                {
                    await Task.Delay(TimeSpan.FromSeconds(3), s.Token);
                    return 42;
                });
                try
                {
                    await t;
                }
                catch (Exception)
                {
                }
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    ReparseFile(sender, args);
                }
                source = null;
                task = null;
            }
        }

        void ReparseFile(object sender, TextContentChangedEventArgs args)
        {
            ITextBuffer buffer = sender as ITextBuffer;
            ITextSnapshot snapshot = buffer.CurrentSnapshot;
            string code = buffer.GetBufferText();
            string ffn = buffer.GetFFN().Result;
            var item = Workspaces.Workspace.Instance.FindDocument(ffn);
            if (item == null) return;
            item.Code = code;
            var to_do = LanguageServer.Module.Compile();
            lock (updateLock)
            {
                SnapshotSpan span = new SnapshotSpan(_buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length);
                EventHandler<SnapshotSpanEventArgs> temp = TagsChanged;
                var i = _buffer as Microsoft.VisualStudio.Text.Projection.IElisionBuffer;
                if (i != null)
                {
                    var j = i.SourceBuffer;
                }
                else
                {

                }
                if (temp == null)
                    return;
                temp(this, new SnapshotSpanEventArgs(span));
            }
        }

        object updateLock = new object();

        public void Raise()
        {
            lock (updateLock)
            {
                SnapshotSpan span = new SnapshotSpan(_buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length);
                EventHandler<SnapshotSpanEventArgs> temp = TagsChanged;
                if (temp == null)
                    return;
                temp(this, new SnapshotSpanEventArgs(span));
            }
        }


    }
}
