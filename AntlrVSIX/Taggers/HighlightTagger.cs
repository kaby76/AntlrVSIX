
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
            ParserDetails details = null;
            bool found = ParserDetailsFactory.AllParserDetails.Where(pair => pair.Key == file_name).Any();
            if (!found) return;

            List<KeyValuePair<Antlr4.Runtime.Tree.TerminalNodeImpl, int>> combined_tokens = new List<KeyValuePair<Antlr4.Runtime.Tree.TerminalNodeImpl, int>>();

            combined_tokens.AddRange(
                details.Refs.Where((pair) =>
                {
                    var token = pair.Key;
                    int start_token_start = token.Symbol.StartIndex;
                    int end_token_end = token.Symbol.StopIndex;
                    if (curLoc < start_token_start) return false;
                    if (end_token_end < curLoc) return false;
                    return true;
                }));

            combined_tokens.AddRange(details.Defs.Where((pair) =>
            {
                var token = pair.Key;
                int start_token_start = token.Symbol.StartIndex;
                int end_token_end = token.Symbol.StopIndex;
                if (curLoc < start_token_start) return false;
                if (end_token_end < curLoc) return false;
                return true;
            }));

            if (combined_tokens.Count > 1 || combined_tokens.Count == 0)
            {
                SynchronousUpdate(currentRequest, new NormalizedSnapshotSpanCollection(), null);
                return;
            }

            var the_symbol = combined_tokens.First();
            
            var node = (Antlr4.Runtime.Tree.IParseTree)the_symbol.Key;
            if (! details.Attributes.TryGetValue(node, out Symtab.CombinedScopeSymbol value))
            {
                return;
            }
            var a1 = the_symbol.Key.Symbol.StartIndex;
            var a2 = the_symbol.Key.Symbol.StopIndex;
            var a3 = new SnapshotPoint(snapshot, a1);
            var a4 = new SnapshotPoint(snapshot, a2);
            var currentWord = new SnapshotSpan(a3, a4);
            bool can_rename = _aggregator.GetClassificationSpans(currentWord).Where(
                classification =>
                {
                    var name = classification.ClassificationType.Classification;
                    if (!_grammar_description.InverseMap.TryGetValue(name, out int type))
                        return false;
                    return _grammar_description.CanRename[type];
                }).Any();
            if (! can_rename) return;

            SnapshotSpan span = currentWord;
            ITextView view = this._view;
            ITextBuffer buffer = view.TextBuffer;
            string path = doc.FilePath;
            List<Antlr4.Runtime.Tree.TerminalNodeImpl> where = new List<Antlr4.Runtime.Tree.TerminalNodeImpl>();
            where.AddRange(details.Refs.Where(
                (t) =>
                {
                    if (!_grammar_description.CanRename[t.Value])
                        return false;
                    Antlr4.Runtime.Tree.TerminalNodeImpl x = t.Key;
                    if (x == the_symbol.Key)
                        return true;
                    var nn = (Antlr4.Runtime.Tree.IParseTree)t.Key;
                    if (! details.Attributes.TryGetValue(nn, out Symtab.CombinedScopeSymbol value2))
                    {
                        return false;
                    }
                    if (value2 == value)
                        return true;
                    if (!(value2 is Symtab.Symbol))
                        return false;
                    if (!(value is Symtab.Symbol))
                        return false;
                    if ((value as Symtab.Symbol).resolve() == (value2 as Symtab.Symbol).resolve())
                        return true;
                    return false;
                }).Select(t => t.Key));

            where.AddRange(details.Defs.Where(
                (t) =>
                {
                    if (!_grammar_description.CanRename[t.Value])
                        return false;
                    Antlr4.Runtime.Tree.TerminalNodeImpl x = t.Key;
                    if (x == the_symbol.Key)
                        return true;
                    var nn = (Antlr4.Runtime.Tree.IParseTree)t.Key;
                    if (! details.Attributes.TryGetValue(nn, out Symtab.CombinedScopeSymbol value2))
                    {
                        return false;
                    }
                    if (value2 == value)
                        return true;
                    if (!(value2 is Symtab.Symbol))
                        return false;
                    if (!(value is Symtab.Symbol))
                        return false;
                    if ((value as Symtab.Symbol).resolve() == (value2 as Symtab.Symbol).resolve())
                        return true;
                    return false;
                }).Select(t => t.Key));

            if (!where.Any()) return;

            var results = new List<Entry>();
            for (int i = 0; i < where.Count; ++i)
            {
                Antlr4.Runtime.Tree.TerminalNodeImpl x = where[i];
                ParserDetails y = details;
                var w = new Entry() { FileName = y.FullFileName, LineNumber = x.Symbol.Line, ColumnNumber = x.Symbol.Column, Token = x.Symbol };
                results.Add(w);
            }
            for (int i = 0; i < results.Count; ++i)
            {
                var w = results[i];
                if (w.FileName == path)
                {
                    // Create new span in the appropriate view.
                    ITextSnapshot cc = buffer.CurrentSnapshot;
                    SnapshotSpan ss = new SnapshotSpan(cc, w.Token.StartIndex, 1 + w.Token.StopIndex - w.Token.StartIndex);
                    SnapshotPoint sp = ss.Start;
                    wordSpans.Add(ss);
                }
            }

            // If we are still up-to-date (another change hasn't happened yet), do a real update
            if (currentRequest == RequestedPoint)
                SynchronousUpdate(currentRequest, new NormalizedSnapshotSpanCollection(wordSpans), currentWord);

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
