namespace AntlrVSIX
{
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Utilities;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System;

    [Export(typeof(ICompletionSourceProvider))]
    [ContentType(AntlrVSIX.Constants.ContentType)]
    [Name("AntlrCompletion")]
    class AntlrCompletionSourceProvider : ICompletionSourceProvider
    {
        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return new AntlrCompletionSource(textBuffer);
        }
    }

    class AntlrCompletionSource : ICompletionSource
    {
        private ITextBuffer _buffer;
        private bool _disposed;
        
        public AntlrCompletionSource(ITextBuffer buffer)
        {
            _buffer = buffer;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            if (_disposed)
                throw new ObjectDisposedException("AntlrCompletionSource");

            List<Completion> completions = new List<Completion>()
            {
                new Completion(AntlrVSIX.Constants.ClassificationNameTerminal),
                new Completion(AntlrVSIX.Constants.ClassificationNameNonterminal),
                new Completion(AntlrVSIX.Constants.ClassificationNameComment),
                new Completion(AntlrVSIX.Constants.ClassificationNameKeyword)
            };
            
            ITextSnapshot snapshot = _buffer.CurrentSnapshot;
            SnapshotPoint snapshot_point = (SnapshotPoint)session.GetTriggerPoint(snapshot);

            if (snapshot_point == null)
                return;

            var line = snapshot_point.GetContainingLine();
            SnapshotPoint start = snapshot_point;

            while (start > line.Start && !char.IsWhiteSpace((start - 1).GetChar()))
            {
                start -= 1;
            }

            ITrackingSpan tracking_span = snapshot.CreateTrackingSpan(new SnapshotSpan(start, snapshot_point), SpanTrackingMode.EdgeInclusive);

            completionSets.Add(new CompletionSet("All", "All", tracking_span, completions, Enumerable.Empty<Completion>()));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}

