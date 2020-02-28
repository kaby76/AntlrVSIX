namespace LspAntlr
{
    using Microsoft.VisualStudio.LanguageServer.Protocol;
    using Microsoft.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class AntlrClassifier : ITagger<ClassificationTag>
    {
        static CancellationTokenSource source;
        static Task<int> task;
        private ITextBuffer _buffer;
        private IDictionary<int, IClassificationType> _lsptype_to_classifiertype;
        private bool initialized = false;
        object updateLock = new object();

        internal AntlrClassifier(ITextBuffer buffer)
        {
            try
            {
                _buffer = buffer;
                _buffer.Changed += new EventHandler<TextContentChangedEventArgs>(OnTextChanged);
                var ffn = _buffer.GetFFN().Result;
                if (ffn == null) return;
            }
            catch (Exception exception)
            {
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            var alc = AntlrLanguageClient.Instance;
            if (alc == null) yield break;
            var ffn = _buffer.GetFFN().Result;
            var document = Workspaces.Workspace.Instance.FindDocument(ffn);
            IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(ffn);
            foreach (SnapshotSpan curSpan in spans)
            {
                SnapshotPoint start = curSpan.Start;
                SnapshotPoint end = curSpan.End;
                var text = curSpan.GetText();
                int curLocStart = start.Position;
                int curLocEnd = end.Position;
                var sorted_combined_tokens = alc.SendServerCustomMessage(curLocStart, curLocEnd - 1, ffn);
                if (sorted_combined_tokens == null) continue;
                foreach (var r in sorted_combined_tokens)
                {
                    var l = r.Location.Range.Start.Line;
                    var c = r.Location.Range.Start.Character;
                    var i = LanguageServer.Module.GetIndex(l, c, document);
                    var l2 = r.Location.Range.End.Line;
                    var c2 = r.Location.Range.End.Character;
                    var i2 = LanguageServer.Module.GetIndex(l2, c2, document);
                    int start_token_start = i;
                    int end_token_end = i2;
                    int type = (int)r.Kind;
                    int length = end_token_end - start_token_start + 1;

                    if (type >= 0)
                    {
                        // Make sure the length doesn't go past the end of the current span.
                        if (start_token_start + length > curLocEnd)
                            length = curLocEnd - start_token_start;

                        var a = curSpan.Snapshot.TextBuffer.CurrentSnapshot;
                        var b = curSpan.Snapshot;

                        var tokenSpan = new SnapshotSpan(
                            curSpan.Snapshot.TextBuffer.CurrentSnapshot,
                            //curSpan.Snapshot,
                            new Span(start_token_start, length));

                        TagSpan<ClassificationTag> result = null;
                        try
                        {
                            result = new TagSpan<ClassificationTag>(tokenSpan,
                                new ClassificationTag(_lsptype_to_classifiertype[type]));
                        }
                        catch (Exception exception)
                        {
                            //Logger.Log.Notify(exception.StackTrace);
                        }
                        if (result != null)
                            yield return result;
                    }
                }
            }
        }

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

        internal void Initialize(IClassificationTypeRegistryService service,
            IClassificationFormatMapService ClassificationFormatMapService)
        {
            try
            {
                if (initialized) return;
                var ffn = _buffer.GetFFN().Result;
                if (ffn == null) return;
                var _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
                if (_grammar_description == null) return;
                var document = Workspaces.Workspace.Instance.FindDocument(ffn);
                _lsptype_to_classifiertype = new Dictionary<int, IClassificationType>();
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
                {
                    var key = (int)SymbolKind.Variable;
                    var val = _grammar_description.Map[0];
                    _lsptype_to_classifiertype[key] = service.GetClassificationType(val);
                }
                {
                    var key = (int)SymbolKind.Enum;
                    var val = _grammar_description.Map[1];
                    _lsptype_to_classifiertype[key] = service.GetClassificationType(val);
                }
                initialized = true;
            }
            catch (Exception exception)
            {
                //Logger.Log.Notify(exception.StackTrace);
            }
        }

        async void OnTextChanged(object sender, TextContentChangedEventArgs args)
        {
            var alc = AntlrLanguageClient.Instance;
            if (alc == null) return;
            var ffn = _buffer.GetFFN().Result;
            var document = Workspaces.Workspace.Instance.FindDocument(ffn);
            if (document == null) return;
            if (_buffer.CurrentSnapshot == null) return;
            document.Code = _buffer.CurrentSnapshot.GetText();
        }
    }

    class Themes
    {
        public static bool IsInvertedTheme()
        {
            return VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey).GetBrightness() < 0.5;
        }
    }
}
