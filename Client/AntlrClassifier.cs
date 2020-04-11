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
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class AntlrClassifier : ITagger<ClassificationTag>
    {
        private static readonly CancellationTokenSource source;
        private static readonly Task<int> task;
        private readonly ITextBuffer _buffer;
        private IDictionary<int, IClassificationType> _lsptype_to_classifiertype;
        private IDictionary<int, IClassificationType> _to_classifiertype;
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
            // Checks to partially fix https://github.com/kaby76/AntlrVSIX/issues/31#.
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
                LanguageServer.CMClassifierInformation[] sorted_combined_tokens = alc.CMGetClassifiers(curLocStart, curLocEnd - 1, ffn);
                if (sorted_combined_tokens == null)
                {
                    continue;
                }

                foreach (LanguageServer.CMClassifierInformation r in sorted_combined_tokens)
                {
                    int type = (int)r.Kind;
                    if (type < 0)
                        continue;
                    int i = r.start;
                    int i2 = r.end;
                    int length = i2 - i;
                    if (length < 0)
                    {
                        continue;
                    }
                    TagSpan<ClassificationTag> result = null;
                    try
                    {
                        ITextSnapshot a = curSpan.Snapshot.TextBuffer.CurrentSnapshot;
                        ITextSnapshot b = curSpan.Snapshot;

                        SnapshotSpan tokenSpan = new SnapshotSpan(
                            curSpan.Snapshot.TextBuffer.CurrentSnapshot,
                            //curSpan.Snapshot,
                            new Span(i, length));

                        result = new TagSpan<ClassificationTag>(tokenSpan,
                            new ClassificationTag(_to_classifiertype[type]));
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

        internal string Capitalize(string str)
        {
            if (str.Length == 0)
                return str;
            else if (str.Length == 1)
                return char.ToUpper(str[0]).ToString();
            else
                return char.ToUpper(str[0]) + str.Substring(1);
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
                _to_classifiertype = new Dictionary<int, IClassificationType>();

                System.Drawing.Color[] colors = new System.Drawing.Color[_grammar_description.Map.Length];
                for (int i = 0; i < _grammar_description.Map.Length; ++i)
                {
                    string[] n = _grammar_description.Map[i].Replace(" - ", " ").Split(' ');
                    string option_name = String.Join("", n.Select(p => Capitalize(p)));
                    var w = Options.Option.GetColor(option_name);
                    colors[i] = w;
                }
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
                    //System.Drawing.Color color = !Themes.IsInvertedTheme()
                    //    ? _grammar_description.MapColor[key]
                    //    : _grammar_description.MapInvertedColor[key];
                    var color = colors[i];
                    System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                    Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties newProperties = identifierProperties.SetForeground(newColor);
                    classificationFormatMap.AddExplicitTextProperties(classificationType, newProperties);
                }
                {
                    int key = (int)SymbolKind.Variable;
                    string val = _grammar_description.Map[0];
                    var t = service.GetClassificationType(val);
                    _lsptype_to_classifiertype[key] = t;
                    _to_classifiertype[0] = t;
                }
                {
                    int key = (int)SymbolKind.Enum;
                    string val = _grammar_description.Map[1];
                    var t = service.GetClassificationType(val);
                    _lsptype_to_classifiertype[key] = t;
                    _to_classifiertype[1] = t;
                }
                {
                    int key = (int)SymbolKind.String;
                    string val = _grammar_description.Map[2];
                    var t = service.GetClassificationType(val);
                    _lsptype_to_classifiertype[key] = t;
                    _to_classifiertype[2] = t;
                }
                {
                    int key = (int)SymbolKind.Key;
                    string val = _grammar_description.Map[3];
                    var t = service.GetClassificationType(val);
                    _lsptype_to_classifiertype[key] = t;
                    _to_classifiertype[3] = t;
                }
                {
                    int key = (int)SymbolKind.Constant;
                    string val = _grammar_description.Map[4];
                    var t = service.GetClassificationType(val);
                    _lsptype_to_classifiertype[key] = t;
                    _to_classifiertype[4] = t;
                }
                {
                    int key = (int)SymbolKind.Event;
                    string val = _grammar_description.Map[5];
                    var t = service.GetClassificationType(val);
                    _lsptype_to_classifiertype[key] = t;
                    _to_classifiertype[5] = t;
                }
                {
                    int key = (int)SymbolKind.Object;
                    string val = _grammar_description.Map[6];
                    var t = service.GetClassificationType(val);
                    _lsptype_to_classifiertype[key] = t;
                    _to_classifiertype[6] = t;
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
