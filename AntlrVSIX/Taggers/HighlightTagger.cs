
namespace AntlrVSIX.Taggers
{
    using Antlr4.Runtime;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.GrammarDescription;
    using AntlrVSIX.Model;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System;


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
            var doc = _buffer.GetTextDocument();
            var ffn = doc.FilePath;
            _grammar_description = GrammarDescriptionFactory.Create(ffn);
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
            List<SnapshotSpan> wordSpans = new List<SnapshotSpan>();
            SnapshotPoint start = currentRequest;
            int curLoc = start.Position;
            SnapshotPoint end = currentRequest;
            var snapshot = currentRequest.Snapshot;
            ITextBuffer buf = currentRequest.Snapshot.TextBuffer;
            var doc = buf.GetTextDocument();
            string file_name = doc.FilePath;
            Document item = AntlrVSIX.GrammarDescription.Workspace.Instance.FindProjectFullName(file_name);
            var ref_pd = ParserDetailsFactory.Create(item);
            if (ref_pd == null) return;
            SnapshotSpan span = new SnapshotSpan(start, 0);
            Antlr4.Runtime.Tree.IParseTree ref_pt = span.Start.Find();
            if (ref_pt == null) return;
            ref_pd.Attributes.TryGetValue(ref_pt, out Symtab.CombinedScopeSymbol value);
            if (value == null) return;
            var @ref = value as Symtab.Symbol;
            if (@ref == null) return;
            var def = @ref.resolve();
            if (def == null) return;
            var def_file = def.file;
            if (def_file == null) return;
            var def_item = AntlrVSIX.GrammarDescription.Workspace.Instance.FindProjectFullName(def_file);
            if (def_item == null) return;
            var def_pd = ParserDetailsFactory.Create(def_item);
            List<Antlr4.Runtime.Tree.TerminalNodeImpl> where = new List<Antlr4.Runtime.Tree.TerminalNodeImpl>();
            where.AddRange(ref_pd.Refs.Where(
                (t) =>
                {
                    Antlr4.Runtime.Tree.TerminalNodeImpl x = t.Key;
                    if (x == @ref.Token) return true;
                    ref_pd.Attributes.TryGetValue(x, out Symtab.CombinedScopeSymbol v);
                    var vv = v as Symtab.Symbol;
                    if (vv == null) return false;
                    if (vv.resolve() == def) return true;
                    return false;
                }).Select(t => t.Key));

            where.AddRange(ref_pd.Defs.Where(
                (t) =>
                {
                    Antlr4.Runtime.Tree.TerminalNodeImpl x = t.Key;
                    if (x == @ref.Token) return true;
                    ref_pd.Attributes.TryGetValue(x, out Symtab.CombinedScopeSymbol v);
                    var vv = v as Symtab.Symbol;
                    if (vv == null) return false;
                    if (vv.resolve() == def) return true;
                    return false;
                }).Select(t => t.Key));

            if (!where.Any()) return;

            var results = new List<Entry>();
            for (int i = 0; i < where.Count; ++i)
            {
                Antlr4.Runtime.Tree.TerminalNodeImpl x = where[i];
                ParserDetails y = ref_pd;
                var w = new Entry() { FileName = y.FullFileName, LineNumber = x.Symbol.Line, ColumnNumber = x.Symbol.Column, Token = x.Symbol };
                results.Add(w);
            }
            for (int i = 0; i < results.Count; ++i)
            {
                var w = results[i];
                // Create new span in the appropriate view.
                ITextSnapshot cc = buf.CurrentSnapshot;
                SnapshotSpan ss = new SnapshotSpan(cc, w.Token.StartIndex, 1 + w.Token.StopIndex - w.Token.StartIndex);
                SnapshotPoint sp = ss.Start;
                wordSpans.Add(ss);
            }

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
