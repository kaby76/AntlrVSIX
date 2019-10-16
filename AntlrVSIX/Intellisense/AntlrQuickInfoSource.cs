
namespace AntlrVSIX
{
    using AntlrVSIX.Extensions;
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

        public AntlrQuickInfoSource(ITextBuffer buffer, ITagAggregator<AntlrTokenTag> aggregator)
        {
            _aggregator = aggregator;
            _buffer = buffer;
            var doc = buffer.GetTextDocument();
            var path = doc.FilePath;
            _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(path);
        }

        public void Dispose()
        {
        }

        public Task<QuickInfoItem> GetQuickInfoItemAsync(IAsyncQuickInfoSession session, CancellationToken cancellationToken)
        {
            var trigger_point = (SnapshotPoint)session.GetTriggerPoint(_buffer.CurrentSnapshot);
            if (trigger_point == null) return Task.FromResult<QuickInfoItem>(null);
            ITextDocument doc = _buffer.GetTextDocument();
            string file_path = doc.FilePath;
            if (_grammar_description == null) return Task.FromResult<QuickInfoItem>(null);
            if (!_grammar_description.IsFileType(file_path)) Task.FromResult<QuickInfoItem>(null);
            var item = Workspaces.Workspace.Instance.FindDocument(file_path);
            if (item == null) return Task.FromResult<QuickInfoItem>(null);
            int index = trigger_point.Position;
            var sym = LanguageServer.Module.GetDocumentSymbol(index, item);
            if (sym == null) return Task.FromResult<QuickInfoItem>(null);
            var info = LanguageServer.Module.GetQuickInfo(index, item);
            if (info != null)
            {
                ITextView view = session.TextView;
                var len = 1 + sym.range.End.Value - sym.range.Start.Value;
                var start = sym.range.Start.Value;
                SnapshotSpan span = new SnapshotSpan(view.TextSnapshot, new Span(start, len));
                var tracking_span = _buffer.CurrentSnapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgeInclusive);
                return Task.FromResult(new QuickInfoItem(tracking_span, info));
            }
            return Task.FromResult<QuickInfoItem>(null);
        }
    }
}

