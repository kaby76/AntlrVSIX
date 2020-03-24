namespace LspAntlr
{
    using Microsoft.VisualStudio.LanguageServer.Protocol;
    using Microsoft.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class AntlrClassifier : ITagger<ClassificationTag>
    {
        private static readonly CancellationTokenSource source;
        private static readonly Task<int> task;
        private readonly ITextBuffer _buffer;
        private IDictionary<int, IClassificationType> _lsptype_to_classifiertype;
        private bool initialized = false;
        private readonly object updateLock = new object();

        internal AntlrClassifier(ITextBuffer buffer)
        {
            try
            {
                _buffer = buffer;
                _buffer.Changed += new EventHandler<TextContentChangedEventArgs>(OnTextChanged);
                string ffn = _buffer.GetFFN().Result;
            }
            catch (Exception)
            {
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            AntlrLanguageClient alc = AntlrLanguageClient.Instance;
            if (alc == null)
            {
                yield break;
            }
            string ffn = _buffer.GetFFN().Result;
            if (ffn == null)
            {
                yield break;
            }
            LanguageServer.IGrammarDescription _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
            if (_grammar_description == null)
            {
                yield break;
            }
            Workspaces.Document document = Workspaces.Workspace.Instance.FindDocument(ffn);
            IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(ffn);
            foreach (SnapshotSpan curSpan in spans)
            {
                SnapshotPoint start = curSpan.Start;
                SnapshotPoint end = curSpan.End;
                string text = curSpan.GetText();
                int curLocStart = start.Position;
                int curLocEnd = end.Position;
                SymbolInformation[] sorted_combined_tokens = alc.CMGetClassifiersSendServer(curLocStart, curLocEnd - 1, ffn);
                if (sorted_combined_tokens == null)
                {
                    continue;
                }

                foreach (SymbolInformation r in sorted_combined_tokens)
                {
                    int l = r.Location.Range.Start.Line;
                    int c = r.Location.Range.Start.Character;
                    int i = LanguageServer.Module.GetIndex(l, c, document);
                    int l2 = r.Location.Range.End.Line;
                    int c2 = r.Location.Range.End.Character;
                    int i2 = LanguageServer.Module.GetIndex(l2, c2, document);
                    int start_token_start = i;
                    int end_token_end = i2;
                    int type = (int)r.Kind;
                    int length = end_token_end - start_token_start + 1;
                    if (length < 0)
                    {
                        continue;
                    }
                    TagSpan<ClassificationTag> result = null;
                    try
                    {
                        if (type >= 0)
                        {
                            // Make sure the length doesn't go past the end of the current span.
                            if (start_token_start + length > curLocEnd)
                            {
                                var new_length = curLocEnd - start_token_start;
                                if (new_length >= 0) length = new_length;
                            }

                            ITextSnapshot a = curSpan.Snapshot.TextBuffer.CurrentSnapshot;
                            ITextSnapshot b = curSpan.Snapshot;

                            SnapshotSpan tokenSpan = new SnapshotSpan(
                                curSpan.Snapshot.TextBuffer.CurrentSnapshot,
                                //curSpan.Snapshot,
                                new Span(start_token_start, length));

                            result = new TagSpan<ClassificationTag>(tokenSpan,
                                new ClassificationTag(_lsptype_to_classifiertype[type]));
                        }
                    }
                    catch (Exception)
                    {
                        //Logger.Log.Notify(exception.StackTrace);
                    }
                    if (result != null)
                    {
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
                {
                    return;
                }

                temp(this, new SnapshotSpanEventArgs(span));
            }
        }

        internal void Initialize(IClassificationTypeRegistryService service,
            IClassificationFormatMapService ClassificationFormatMapService)
        {
            try
            {
                if (initialized)
                {
                    return;
                }

                string ffn = _buffer.GetFFN().Result;
                if (ffn == null)
                {
                    return;
                }

                LanguageServer.IGrammarDescription _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
                if (_grammar_description == null)
                {
                    return;
                }

                Workspaces.Document document = Workspaces.Workspace.Instance.FindDocument(ffn);
                _lsptype_to_classifiertype = new Dictionary<int, IClassificationType>();
                for (int i = 0; i < _grammar_description.Map.Length; ++i)
                {
                    int key = i;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationFormatMap classificationFormatMap = ClassificationFormatMapService.GetClassificationFormatMap(category: "text");
                    Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                        .GetExplicitTextProperties(classificationType);
                    System.Drawing.Color color = !Themes.IsInvertedTheme() ? _grammar_description.MapColor[key]
                        : _grammar_description.MapInvertedColor[key];
                    System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                    Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties newProperties = identifierProperties.SetForeground(newColor);
                    classificationFormatMap.AddExplicitTextProperties(classificationType, newProperties);
                }
                {
                    int key = (int)SymbolKind.Variable;
                    string val = _grammar_description.Map[0];
                    _lsptype_to_classifiertype[key] = service.GetClassificationType(val);
                }
                {
                    int key = (int)SymbolKind.Enum;
                    string val = _grammar_description.Map[1];
                    _lsptype_to_classifiertype[key] = service.GetClassificationType(val);
                }
                initialized = true;
            }
            catch (Exception)
            {
                //Logger.Log.Notify(exception.StackTrace);
            }
        }

        private async void OnTextChanged(object sender, TextContentChangedEventArgs args)
        {
            AntlrLanguageClient alc = AntlrLanguageClient.Instance;
            if (alc == null)
            {
                return;
            }

            string ffn = _buffer.GetFFN().Result;
            if (ffn == null)
            {
                return;
            }

            LanguageServer.IGrammarDescription _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
            if (_grammar_description == null)
            {
                return;
            }

            Workspaces.Document document = Workspaces.Workspace.Instance.FindDocument(ffn);
            if (document == null)
            {
                return;
            }

            if (_buffer.CurrentSnapshot == null)
            {
                return;
            }

            document.Code = _buffer.CurrentSnapshot.GetText();
        }
    }

    internal class Themes
    {
        public static bool IsInvertedTheme()
        {
            return VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey).GetBrightness() < 0.5;
        }
    }
}
