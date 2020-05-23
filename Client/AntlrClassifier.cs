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
        private readonly ITextBuffer _buffer;
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
                _to_classifiertype = new Dictionary<int, IClassificationType>();

                IClassificationFormatMap classificationFormatMap = ClassificationFormatMapService.GetClassificationFormatMap(category: "text");
                // hardwire colors to VS's colors of selected types.
                System.Collections.ObjectModel.ReadOnlyCollection<IClassificationType> list_of_formats = classificationFormatMap.CurrentPriorityOrder;

                {
                    int i = (int)LanguageServer.AntlrGrammarDescription.AntlrClassifications.ClassificationNonterminalDef;
                    int key = (int)SymbolKind.Variable;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationType t = list_of_formats.Where(f => f == null ? false : f.Classification == Options.Option.GetString("AntlrNonterminalDef")).FirstOrDefault();
                    if (t != null)
                    {
                        try
                        {
                            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                                .GetExplicitTextProperties(t);
                            classificationFormatMap.AddExplicitTextProperties(classificationType, identifierProperties);
                        }
#pragma warning disable 0168
                        catch (Exception eeks) { }
#pragma warning restore 0168
                    }
                    _to_classifiertype[i] = classificationType;
                }
                {
                    int i = (int)LanguageServer.AntlrGrammarDescription.AntlrClassifications.ClassificationNonterminalRef;
                    int key = (int)SymbolKind.Enum;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationType t = list_of_formats.Where(f => f == null ? false : f.Classification == Options.Option.GetString("AntlrNonterminalRef")).FirstOrDefault();
                    if (t != null)
                    {
                        try
                        {
                            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                                .GetExplicitTextProperties(t);
                            classificationFormatMap.AddExplicitTextProperties(classificationType, identifierProperties);
                        }
#pragma warning disable 0168
                        catch (Exception eeks) { }
#pragma warning restore 0168
                    }
                    _to_classifiertype[i] = classificationType;
                }
                {
                    int i = (int)LanguageServer.AntlrGrammarDescription.AntlrClassifications.ClassificationTerminalDef;
                    int key = (int)SymbolKind.Variable;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationType t = list_of_formats.Where(f => f == null ? false : f.Classification == Options.Option.GetString("AntlrTerminalDef")).FirstOrDefault();
                    if (t != null)
                    {
                        try
                        {
                            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                                .GetExplicitTextProperties(t);
                            classificationFormatMap.AddExplicitTextProperties(classificationType, identifierProperties);
                        }
#pragma warning disable 0168
                        catch (Exception eeks) { }
#pragma warning restore 0168
                    }
                    _to_classifiertype[i] = classificationType;
                }
                {
                    int i = (int)LanguageServer.AntlrGrammarDescription.AntlrClassifications.ClassificationTerminalRef;
                    int key = (int)SymbolKind.Enum;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationType t = list_of_formats.Where(f => f == null ? false : f.Classification == Options.Option.GetString("AntlrTerminalRef")).FirstOrDefault();
                    if (t != null)
                    {
                        try
                        {
                            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                                .GetExplicitTextProperties(t);
                            classificationFormatMap.AddExplicitTextProperties(classificationType, identifierProperties);
                        }
#pragma warning disable 0168
                        catch (Exception eeks) { }
#pragma warning restore 0168
                    }
                    _to_classifiertype[i] = classificationType;
                }
                {
                    int i = (int)LanguageServer.AntlrGrammarDescription.AntlrClassifications.ClassificationComment;
                    int key = (int)SymbolKind.String;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationType t = list_of_formats.Where(f => f == null ? false : f.Classification == Options.Option.GetString("AntlrComment")).FirstOrDefault();
                    if (t != null)
                    {
                        try
                        {
                            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                                .GetExplicitTextProperties(t);
                            classificationFormatMap.AddExplicitTextProperties(classificationType, identifierProperties);
                        }
#pragma warning disable 0168
                        catch (Exception eeks) { }
#pragma warning restore 0168
                    }
                    _to_classifiertype[i] = classificationType;
                }
                {
                    int i = (int)LanguageServer.AntlrGrammarDescription.AntlrClassifications.ClassificationKeyword;
                    int key = (int)SymbolKind.Key;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationType t = list_of_formats.Where(f => f == null ? false : f.Classification == Options.Option.GetString("AntlrKeyword")).FirstOrDefault();
                    if (t != null)
                    {
                        try
                        {
                            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                                .GetExplicitTextProperties(t);
                            classificationFormatMap.AddExplicitTextProperties(classificationType, identifierProperties);
                        }
#pragma warning disable 0168
                        catch (Exception eeks) { }
#pragma warning restore 0168
                    }
                    _to_classifiertype[i] = classificationType;
                }
                {
                    int i = (int)LanguageServer.AntlrGrammarDescription.AntlrClassifications.ClassificationLiteral;
                    int key = (int)SymbolKind.Constant;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationType t = list_of_formats.Where(f => f == null ? false : f.Classification == Options.Option.GetString("AntlrLiteral")).FirstOrDefault();
                    if (t != null)
                    {
                        try
                        {
                            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                                .GetExplicitTextProperties(t);
                            classificationFormatMap.AddExplicitTextProperties(classificationType, identifierProperties);
                        }
#pragma warning disable 0168
                        catch (Exception eeks) { }
#pragma warning restore 0168
                    }
                    _to_classifiertype[i] = classificationType;
                }
                {
                    int i = (int)LanguageServer.AntlrGrammarDescription.AntlrClassifications.ClassificationModeDef;
                    int key = (int)SymbolKind.Variable;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationType t = list_of_formats.Where(f => f == null ? false : f.Classification == Options.Option.GetString("AntlrModeDef")).FirstOrDefault();
                    if (t != null)
                    {
                        try
                        {
                            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                                .GetExplicitTextProperties(t);
                            classificationFormatMap.AddExplicitTextProperties(classificationType, identifierProperties);
                        }
#pragma warning disable 0168
                        catch (Exception eeks) { }
#pragma warning restore 0168
                    }
                    _to_classifiertype[i] = classificationType;
                }
                {
                    int i = (int)LanguageServer.AntlrGrammarDescription.AntlrClassifications.ClassificationModeRef;
                    int key = (int)SymbolKind.Enum;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationType t = list_of_formats.Where(f => f == null ? false : f.Classification == Options.Option.GetString("AntlrModeRef")).FirstOrDefault();
                    if (t != null)
                    {
                        try
                        {
                            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                                .GetExplicitTextProperties(t);
                            classificationFormatMap.AddExplicitTextProperties(classificationType, identifierProperties);
                        }
#pragma warning disable 0168
                        catch (Exception eeks) { }
#pragma warning restore 0168
                    }
                    _to_classifiertype[i] = classificationType;
                }
                {
                    int i = (int)LanguageServer.AntlrGrammarDescription.AntlrClassifications.ClassificationChannelDef;
                    int key = (int)SymbolKind.Variable;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationType t = list_of_formats.Where(f => f == null ? false : f.Classification == Options.Option.GetString("AntlrChannelDef")).FirstOrDefault();
                    if (t != null)
                    {
                        try
                        {
                            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                                .GetExplicitTextProperties(t);
                            classificationFormatMap.AddExplicitTextProperties(classificationType, identifierProperties);
                        }
#pragma warning disable 0168
                        catch (Exception eeks) { }
#pragma warning restore 0168
                    }
                    _to_classifiertype[i] = classificationType;
                }
                {
                    int i = (int)LanguageServer.AntlrGrammarDescription.AntlrClassifications.ClassificationChannelRef;
                    int key = (int)SymbolKind.Enum;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationType t = list_of_formats.Where(f => f == null ? false : f.Classification == Options.Option.GetString("AntlrChannelRef")).FirstOrDefault();
                    if (t != null)
                    {
                        try
                        {
                            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                                .GetExplicitTextProperties(t);
                            classificationFormatMap.AddExplicitTextProperties(classificationType, identifierProperties);
                        }
#pragma warning disable 0168
                        catch (Exception eeks) { }
#pragma warning restore 0168
                    }
                    _to_classifiertype[i] = classificationType;
                }
                {
                    int i = (int)LanguageServer.AntlrGrammarDescription.AntlrClassifications.ClassificationPunctuation;
                    int key = (int)SymbolKind.Event;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationType t = list_of_formats.Where(f => f == null ? false : f.Classification == Options.Option.GetString("AntlrPunctuation")).FirstOrDefault();
                    if (t != null)
                    {
                        try
                        {
                            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                                .GetExplicitTextProperties(t);
                            classificationFormatMap.AddExplicitTextProperties(classificationType, identifierProperties);
                        }
#pragma warning disable 0168
                        catch (Exception eeks) { }
#pragma warning restore 0168
                    }
                    _to_classifiertype[i] = classificationType;
                }
                {
                    int i = (int)LanguageServer.AntlrGrammarDescription.AntlrClassifications.ClassificationOperator;
                    int key = (int)SymbolKind.Object;
                    string val = _grammar_description.Map[i];
                    IClassificationType identiferClassificationType = service.GetClassificationType(val);
                    IClassificationType classificationType = identiferClassificationType == null ? service.CreateClassificationType(val, new IClassificationType[] { })
                            : identiferClassificationType;
                    IClassificationType t = list_of_formats.Where(f => f == null ? false : f.Classification == Options.Option.GetString("AntlrOperator")).FirstOrDefault();
                    if (t != null)
                    {
                        try
                        {
                            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap
                                .GetExplicitTextProperties(t);
                            classificationFormatMap.AddExplicitTextProperties(classificationType, identifierProperties);
                        }
#pragma warning disable 0168
                        catch (Exception eeks) { }
#pragma warning restore 0168
                    }
                    _to_classifiertype[i] = classificationType;
                }
                initialized = true;
            }
#pragma warning disable 0168
            catch (Exception eeks) { }
#pragma warning restore 0168
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
