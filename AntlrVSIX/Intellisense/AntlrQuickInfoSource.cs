
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
            var path = buffer.GetFFN().Result;
            _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(path);
        }

        public void Dispose()
        {
        }

        public Task<QuickInfoItem> GetQuickInfoItemAsync(IAsyncQuickInfoSession session, CancellationToken cancellationToken)
        {
            var trigger_point = (SnapshotPoint)session.GetTriggerPoint(_buffer.CurrentSnapshot);
            if (trigger_point == null) return Task.FromResult<QuickInfoItem>(null);
            string file_path = _buffer.GetFFN().Result;
            if (file_path == null) return Task.FromResult<QuickInfoItem>(null);
            if (_grammar_description == null) return Task.FromResult<QuickInfoItem>(null);
            if (!_grammar_description.IsFileType(file_path)) Task.FromResult<QuickInfoItem>(null);
            var document = Workspaces.Workspace.Instance.FindDocument(file_path);
            if (document == null) return Task.FromResult<QuickInfoItem>(null);
            int index = trigger_point.Position;
            var info = LanguageServer.Module.GetQuickInfo(index, document);
            if (info == null || info.Display == null || info.Display == "") return Task.FromResult<QuickInfoItem>(null);
            ITextView view = session.TextView;
            var len = 1 + info.Range.End.Value - info.Range.Start.Value;
            var start = info.Range.Start.Value;
            if (len + start > view.TextSnapshot.Length)
                len = len - (len + start - view.TextSnapshot.Length);
            SnapshotSpan span = new SnapshotSpan(view.TextSnapshot, new Span(start, len));
            var tracking_span = _buffer.CurrentSnapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgeInclusive);
            return Task.FromResult(new QuickInfoItem(tracking_span, info.Display));
        }
    }
}

