
namespace AntlrVSIX.Taggers
{
    using AntlrVSIX.Extensions;
    using LanguageServer;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Text.Tagging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class HighlightWordTag : TextMarkerTag, ITag
    {
        public HighlightWordTag() : base("blue") { }
    }

    public class HighlightTagger : ITagger<HighlightWordTag>
    {
        private object updateLock = new object();
        private IClassifier _aggregator;

        private ITextView _view;
        private IGrammarDescription _grammar_description;
        private ITextBuffer _buffer;
        private ITextSearchService TextSearchService { get; set; }
        private ITextStructureNavigator TextStructureNavigator { get; set; }
        private NormalizedSnapshotSpanCollection WordSpans { get; set; }
        public static SnapshotSpan? CurrentWord { get; set; }
        public SnapshotPoint RequestedPoint { get; set; }
        private static Dictionary<ITextView, HighlightTagger> _view_to_tagger = new Dictionary<ITextView, HighlightTagger>();

        public HighlightTagger(ITextView view, ITextBuffer sourceBuffer, ITextSearchService textSearchService,
                                   ITextStructureNavigator textStructureNavigator,
                                   IClassifier aggregator)
        {
            _view = view;
            _buffer = sourceBuffer;
            var ffn = _buffer.GetFFN().Result;
            _grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
            TextSearchService = textSearchService;
            TextStructureNavigator = textStructureNavigator;
            _aggregator = aggregator;
            WordSpans = new NormalizedSnapshotSpanCollection();
            CurrentWord = null;
            _view_to_tagger[view] = this;
        }

        private void ViewLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            // If a new snapshot wasn't generated, then skip this layout
            if (e.NewViewState.EditSnapshot != e.OldViewState.EditSnapshot)
            {
                UpdateAtCaretPosition(_view.Caret.Position);
            }
        }

        private void CaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            UpdateAtCaretPosition(e.NewPosition);
        }

        private void UpdateAtCaretPosition(CaretPosition caretPoisition)
        {
            SnapshotPoint? point = caretPoisition.Point.GetPoint(_buffer, caretPoisition.Affinity);

            if (!point.HasValue)
                return;

            // If the new cursor position is still within the current word (and on the same snapshot),
            // we don't need to check it.
            if (CurrentWord.HasValue &&
                CurrentWord.Value.Snapshot == _view.TextSnapshot &&
                point.Value >= CurrentWord.Value.Start &&
                point.Value <= CurrentWord.Value.End)
            {
                return;
            }

            RequestedPoint = point.Value;

            ThreadPool.QueueUserWorkItem(UpdateWordAdornments);
        }

        public static void Update(ITextView view, SnapshotPoint point)
        {
            foreach (var kvp in _view_to_tagger)
                if (view == kvp.Key)
                {
                    kvp.Value.RequestedPoint = point;
                    kvp.Value.Update();
                    break;
                }
        }

        public void Update()
        {
            SnapshotPoint currentRequest = RequestedPoint;
            int curLoc = currentRequest.Position;
            var buf = currentRequest.Snapshot.TextBuffer;
            var file_name = buf.GetFFN().Result;
            var document = Workspaces.Workspace.Instance.FindDocument(file_name);
            if (document == null) return;
            var ref_pd = ParserDetailsFactory.Create(document);
            if (ref_pd == null) return;
            var location = LanguageServer.Module.GetDocumentSymbol(curLoc, document);
            if (location == null) return;
            var locations = LanguageServer.Module.FindRefsAndDefs(curLoc, document);
            List<SnapshotSpan> wordSpans = new List<SnapshotSpan>();
            foreach (var loc in locations)
            {
                if (loc.uri != document) continue;
                ITextSnapshot cc = buf.CurrentSnapshot;
                SnapshotSpan ss = new SnapshotSpan(cc, loc.range.Start.Value,
                    1 + loc.range.End.Value - loc.range.Start.Value);
                SnapshotPoint sp = ss.Start;
                wordSpans.Add(ss);
            }
            SnapshotSpan span = new SnapshotSpan(currentRequest, 0);
            // If we are still up-to-date (another change hasn't happened yet), do a real update
            if (currentRequest == RequestedPoint)
                SynchronousUpdate(currentRequest, new NormalizedSnapshotSpanCollection(wordSpans), span);

        }

        private void UpdateWordAdornments(object threadContext)
        {
            Update();
        }

        static bool WordExtentIsValid(SnapshotPoint currentRequest, TextExtent word)
        {
            return word.IsSignificant && currentRequest.Snapshot.GetText(word.Span).Any(c => char.IsLetter(c));
        }

        private void SynchronousUpdate(SnapshotPoint currentRequest, NormalizedSnapshotSpanCollection newSpans, SnapshotSpan? newCurrentWord)
        {
            lock (updateLock)
            {
                if (currentRequest != RequestedPoint)
                    return;
                WordSpans = newSpans;
                CurrentWord = newCurrentWord;
                var tempEvent = TagsChanged;
                if (tempEvent != null)
                    tempEvent(this, new SnapshotSpanEventArgs(new SnapshotSpan(_buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length)));
            }
        }

        public IEnumerable<ITagSpan<HighlightWordTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (CurrentWord == null)
                yield break;
            SnapshotSpan currentWord = CurrentWord.Value;
            NormalizedSnapshotSpanCollection wordSpans = WordSpans;
            if (spans.Count == 0 || WordSpans.Count == 0)
                yield break;
            if (spans[0].Snapshot != wordSpans[0].Snapshot)
            {
                wordSpans = new NormalizedSnapshotSpanCollection(
                    wordSpans.Select(span => span.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive)));
                currentWord = currentWord.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive);
            }
            foreach (SnapshotSpan span in NormalizedSnapshotSpanCollection.Overlap(spans, wordSpans))
            {
                yield return new TagSpan<HighlightWordTag>(span, new HighlightWordTag());
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
    }
}
