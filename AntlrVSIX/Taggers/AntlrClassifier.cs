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
    using System.Linq;

    class Themes
    {
        public static bool IsInvertedTheme()
        {
            return VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey).GetBrightness() < 0.5;
        }
    }

    internal sealed class AntlrClassifier : ITagger<ClassificationTag>
    {
        private bool initialized = false;
        private ITextBuffer _buffer;
        private IGrammarDescription _grammar_description;
        private ITagAggregator<AntlrTokenTag> _aggregator;
        private IDictionary<int, IClassificationType> _antlrtype_to_classifiertype;

        internal void Initialize(
            IBufferTagAggregatorFactoryService aggregatorFactory,
            IClassificationTypeRegistryService service,
            IClassificationFormatMapService ClassificationFormatMapService)
        {
            try
            {
                if (initialized) return;
                var ffn = _buffer.GetFFN().Result;
                if (ffn == null) return;
                _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
                if (_grammar_description == null) return;
                var document = Workspaces.Workspace.Instance.FindDocument(ffn);
                if (document == null)
                {
                    Workspaces.Loader.LoadAsync().Wait();
                    var to_do = LanguageServer.Module.Compile();
                    document = Workspaces.Workspace.Instance.FindDocument(ffn);
                }
                _aggregator = aggregatorFactory.CreateTagAggregator<AntlrTokenTag>(_buffer);
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
                    var color = !Themes.IsInvertedTheme() ? _grammar_description.MapColor[key]
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
                initialized = true;
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }
        }

        internal AntlrClassifier(ITextBuffer buffer)
        {
            try
            {
                _buffer = buffer;
                _buffer.Changed += new EventHandler<TextContentChangedEventArgs>(OnTextChanged);

                var ffn = _buffer.GetFFN().Result;
                if (ffn == null) return;
                _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
                if (_grammar_description == null) return;
                var document = Workspaces.Workspace.Instance.FindDocument(ffn);
                if (document == null)
                {
                    Workspaces.Loader.LoadAsync().Wait();
                    var to_do = LanguageServer.Module.Compile();
                    document = Workspaces.Workspace.Instance.FindDocument(ffn);
                }
                AntlrLanguagePackage package = AntlrLanguagePackage.Instance;
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.ToString());
            }
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            try
            {
                if (_grammar_description == null) yield break;
                if (_grammar_description == null) throw new Exception();
                string f = _buffer.GetFFN().Result;
                var document = Workspaces.Workspace.Instance.FindDocument(f);
                if (document == null) yield break;
                document.Code = _buffer.GetBufferText();
                var pd = ParserDetailsFactory.Create(document);
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }

            List<IMappingTagSpan<AntlrTokenTag>> span_list = null;
            try
            {
                span_list = _aggregator.GetTags(spans).ToList();
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }

            if (span_list != null)
            {
                foreach (IMappingTagSpan<AntlrTokenTag> tag_span in span_list)
                {
                    TagSpan<ClassificationTag> result = null;
                    try
                    {
                        NormalizedSnapshotSpanCollection tag_spans = tag_span.Span.GetSpans(spans[0].Snapshot);
                        result = new TagSpan<ClassificationTag>(tag_spans[0],
                                new ClassificationTag(_antlrtype_to_classifiertype[(int)tag_span.Tag.TagType]));
                    }
                    catch (Exception exception)
                    {
                        Logger.Log.Notify(exception.StackTrace);
                    }
                    if (result != null)
                        yield return result;
                }
            }
        }

#pragma warning disable CS0067
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore CS0067

        static CancellationTokenSource source;
        static Task<int> task;

        async void OnTextChanged(object sender, TextContentChangedEventArgs args)
        {
            try
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
            } catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }
        }

        void ReparseFile(object sender, TextContentChangedEventArgs args)
        {
            ITextBuffer buffer = sender as ITextBuffer;
            ITextSnapshot snapshot = buffer.CurrentSnapshot;
            string code = buffer.GetBufferText();
            string ffn = buffer.GetFFN().Result;
            if (ffn == null) return;
            var document = Workspaces.Workspace.Instance.FindDocument(ffn);
            if (document == null) return;
            document.Code = code;
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
