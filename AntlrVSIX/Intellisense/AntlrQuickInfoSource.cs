
namespace AntlrVSIX
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.Package;
    using AntlrVSIX.Tagger;
    using LanguageServer;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    [Export(typeof(IAsyncQuickInfoSourceProvider))]
    [ContentType(AntlrVSIX.Constants.ContentType)]
    [Name("AntlrQuickInfo")]
    class AntlrQuickInfoSourceProvider : IAsyncQuickInfoSourceProvider
    {

        [Import]
        IBufferTagAggregatorFactoryService aggService = null;

        public IAsyncQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer)
        {
             return new AntlrQuickInfoSource(textBuffer, aggService.CreateTagAggregator<AntlrTokenTag>(textBuffer));
        }

        //public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer)
        //{
        //    return new AntlrQuickInfoSource(textBuffer, aggService.CreateTagAggregator<AntlrTokenTag>(textBuffer));
        //}

        //IAsyncQuickInfoSource IAsyncQuickInfoSourceProvider.TryCreateQuickInfoSource(ITextBuffer textBuffer)
        //{
        //    return new AntlrQuickInfoSource(textBuffer, aggService.CreateTagAggregator<AntlrTokenTag>(textBuffer));
        //}
    }

    class AntlrQuickInfoSource : IAsyncQuickInfoSource
    {
        private ITagAggregator<AntlrTokenTag> _aggregator;
        private ITextBuffer _buffer;
        private IGrammarDescription _grammar_description;
        private bool _disposed = false;
        private volatile IAsyncQuickInfoSession _curSession;
        IWpfTextView view;

        public AntlrQuickInfoSource(ITextBuffer buffer, ITagAggregator<AntlrTokenTag> aggregator)
        {
            _aggregator = aggregator;
            _buffer = buffer;
            var doc = buffer.GetTextDocument();
            view = AntlrLanguagePackage.Instance.GetActiveView();
            var path = doc.FilePath;
            _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(path);
        }

        //public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> quick_info_content, out ITrackingSpan tracking_span)
        //{
        //    tracking_span = null;

        //    if (_disposed)
        //        throw new ObjectDisposedException("TestQuickInfoSource");

        //    var trigger_point = (SnapshotPoint) session.GetTriggerPoint(_buffer.CurrentSnapshot);

        //    if (trigger_point == null)
        //        return;

        //    IWpfTextView view = AntlrLanguagePackage.Instance.GetActiveView();
        //    if (view == null) return;

        //    TextExtent extent = AntlrVSIX.Package.AntlrLanguagePackage.Instance.Navigator[view].GetExtentOfWord(trigger_point);
        //    SnapshotSpan span = extent.Span;
        //    ITextBuffer buffer = view.TextBuffer;
        //    ITextDocument doc = buffer.GetTextDocument();
        //    string file_path = doc.FilePath;
        //    IGrammarDescription grammar_description = GrammarDescriptionFactory.Create(file_path);
        //    if (!grammar_description.IsFileType(file_path)) return;
        //    ParserDetails pd = ParserDetails._per_file_parser_details[file_path];

        //    foreach (IMappingTagSpan<AntlrTokenTag> cur_tag in _aggregator.GetTags(span))
        //    {
        //        int tag_type = (int)cur_tag.Tag.TagType;
        //        SnapshotSpan tag_span = cur_tag.Span.GetSpans(_buffer).First();
        //        tracking_span = _buffer.CurrentSnapshot.CreateTrackingSpan(tag_span, SpanTrackingMode.EdgeExclusive);
        //        Antlr4.Runtime.Tree.IParseTree pt = tag_span.Start.Find();
        //        bool found = false;
        //        if (pt != null)
        //        {
        //            Antlr4.Runtime.Tree.IParseTree p = pt;
        //            for (; p != null; p = p.Parent)
        //            {
        //                pd._ant_symtab.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
        //                if (value != null)
        //                {
        //                    var name = value as Symtab.Symbol;
        //                    string show = name?.Name;
        //                    if (value is Symtab.Literal)
        //                    {
        //                        show = ((Symtab.Literal)value).Cleaned;
        //                    }
        //                    if (grammar_description.PopUpDefinition[tag_type] != null)
        //                    {
        //                        var fun = grammar_description.PopUpDefinition[tag_type];
        //                        show = fun(grammar_description, pd._ant_symtab, p);
        //                        quick_info_content.Add(
        //                          show);
        //                    }
        //                    else
        //                    {
        //                        quick_info_content.Add(
        //                            _grammar_description.Map[tag_type]
        //                            + "\n"
        //                            + show);
        //                    }
        //                    found = true;
        //                    break;
        //                }
        //            }
        //        }
        //        if (! found)
        //            quick_info_content.Add(
        //            _grammar_description.Map[tag_type]);
        //    }
        //}

        public void Dispose()
        {
            _disposed = true;
        }

        public async Task<QuickInfoItem> GetQuickInfoItemAsync(IAsyncQuickInfoSession session, CancellationToken cancellationToken)
        {
            if (_curSession != null && _curSession.State != QuickInfoSessionState.Dismissed)
            {
                await _curSession.DismissAsync();
                _curSession = null;
            }
            _curSession = session;
            _curSession.StateChanged += CurSessionStateChanged;
            var trigger_point = (SnapshotPoint)session.GetTriggerPoint(_buffer.CurrentSnapshot);
            if (trigger_point == null) return null;
            ITextView vv = session.TextView;
            int asdf = trigger_point.Position;
            ITextDocument doc = _buffer.GetTextDocument();
            SnapshotSpan span = new SnapshotSpan(vv.TextSnapshot, new Span(asdf, 0));
            string file_path = doc.FilePath;
            IGrammarDescription grammar_description = LanguageServer.GrammarDescriptionFactory.Create(file_path);
            if (!grammar_description.IsFileType(file_path)) return null;
            var item = Workspaces.Workspace.Instance.FindDocument(file_path);
            var pd = ParserDetailsFactory.Create(item);

            foreach (IMappingTagSpan<AntlrTokenTag> cur_tag in _aggregator.GetTags(span))
            {
                int tag_type = (int)cur_tag.Tag.TagType;
                SnapshotSpan tag_span = cur_tag.Span.GetSpans(_buffer).First();
                var tracking_span = _buffer.CurrentSnapshot.CreateTrackingSpan(tag_span, SpanTrackingMode.EdgeExclusive);
                Antlr4.Runtime.Tree.IParseTree pt = tag_span.Start.Find();
                bool found = false;
                if (pt != null)
                {
                    Antlr4.Runtime.Tree.IParseTree p = pt;
                    {
                        pd.Attributes.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
                        if (value != null)
                        {
                            var name = value as Symtab.Symbol;
                            string show = name?.Name;
                            if (value is Symtab.Literal)
                            {
                                show = ((Symtab.Literal)value).Cleaned;
                            }
                            if (grammar_description.PopUpDefinition[tag_type] != null)
                            {
                                var fun = grammar_description.PopUpDefinition[tag_type];
                                var mess = fun(pd, p);
                                if (mess != null)
                                    return new QuickInfoItem(tracking_span, mess);
                            }
                            return new QuickInfoItem(
                                tracking_span, _grammar_description.Map[tag_type]
                                + "\n"
                                + show);
                        }
                    }
                }
                if (!found)
                    return new QuickInfoItem(
                         tracking_span,
                            _grammar_description.Map[tag_type]);
            }
            return null;
        }

        private void CurSessionStateChanged(object sender, QuickInfoSessionStateChangedEventArgs e)
            => _curSession = null;
        
    }
}

