
namespace AntlrVSIX.Rename
{
    using Antlr4.Runtime;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.Model;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System;


    public class HighlightWordTag : TextMarkerTag, ITag
    {
        public HighlightWordTag() : base("blue") { }
    }

    public class RenameHighlightTagger : ITagger<HighlightWordTag>
    {
        public bool Enabled = false;
        private object updateLock = new object();
        private IClassifier _aggregator;

        private ITextView View { get; set; }
        private ITextBuffer SourceBuffer { get; set; }
        private ITextSearchService TextSearchService { get; set; }
        private ITextStructureNavigator TextStructureNavigator { get; set; }
        private NormalizedSnapshotSpanCollection WordSpans { get; set; }
        private SnapshotSpan? CurrentWord { get; set; }
        public SnapshotPoint RequestedPoint { get; set; }
        private static Dictionary<ITextView, RenameHighlightTagger> _view_to_tagger = new Dictionary<ITextView, RenameHighlightTagger>();

        public RenameHighlightTagger(ITextView view, ITextBuffer sourceBuffer, ITextSearchService textSearchService,
                                   ITextStructureNavigator textStructureNavigator,
                                   IClassifier aggregator)
        {
            View = view;
            SourceBuffer = sourceBuffer;
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
                UpdateAtCaretPosition(View.Caret.Position);
            }
        }

        private void CaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            UpdateAtCaretPosition(e.NewPosition);
        }

        private void UpdateAtCaretPosition(CaretPosition caretPoisition)
        {
            SnapshotPoint? point = caretPoisition.Point.GetPoint(SourceBuffer, caretPoisition.Affinity);

            if (!point.HasValue)
                return;

            // If the new cursor position is still within the current word (and on the same snapshot),
            // we don't need to check it.
            if (CurrentWord.HasValue &&
                CurrentWord.Value.Snapshot == View.TextSnapshot &&
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
                    kvp.Value.Enabled = true;
                    kvp.Value.RequestedPoint = point;
                    kvp.Value.Update();
                    break;
                }
        }

        public void Update()
        {
            if (!Enabled) return;
            Enabled = false;

            SnapshotPoint currentRequest = RequestedPoint;

            List<SnapshotSpan> wordSpans = new List<SnapshotSpan>();

            // Find all words in the buffer like the one the caret is on
            TextExtent word = TextStructureNavigator.GetExtentOfWord(currentRequest);

            bool foundWord = true;

            // If we've selected something not worth highlighting, we might have
            // missed a "word" by a little bit
            if (!WordExtentIsValid(currentRequest, word))
            {
                // Before we retry, make sure it is worthwhile
                if (word.Span.Start != currentRequest ||
                    currentRequest == currentRequest.GetContainingLine().Start ||
                    char.IsWhiteSpace((currentRequest - 1).GetChar()))
                {
                    foundWord = false;
                }
                else
                {
                    // Try again, one character previous.  If the caret is at the end of a word, then
                    // this will pick up the word we are at the end of.
                    word = TextStructureNavigator.GetExtentOfWord(currentRequest - 1);

                    // If we still aren't valid the second time around, we're done
                    if (!WordExtentIsValid(currentRequest, word))
                        foundWord = false;
                }
            }

            if (!foundWord)
            {
                // If we couldn't find a word, just clear out the existing markers
                SynchronousUpdate(currentRequest, new NormalizedSnapshotSpanCollection(), null);
                return;
            }

            SnapshotSpan currentWord = word.Span;

            // If this is the same word we currently have, we're done (e.g. caret moved within a word).
            if (CurrentWord.HasValue && currentWord == CurrentWord)
                return;

            /* Find spans using simple text search....
            FindData findData = new FindData(currentWord.GetText(), currentWord.Snapshot);
            findData.FindOptions = FindOptions.WholeWord | FindOptions.MatchCase;
            wordSpans.AddRange(TextSearchService.FindAll(findData));
            */

            // Verify current word is a grammar symbol.
            //  Now, check for valid classification type.
            string classification = null;
            ClassificationSpan[] c1 = _aggregator.GetClassificationSpans(currentWord).ToArray();
            foreach (ClassificationSpan cl in c1)
            {
                classification = cl.ClassificationType.Classification.ToLower();
                if (classification == AntlrVSIX.Constants.ClassificationNameTerminal)
                {
                    break;
                }
                else if (classification == AntlrVSIX.Constants.ClassificationNameNonterminal)
                {
                    break;
                }
            }
            if (classification == null) return;

            SnapshotSpan span = currentWord;
            ITextView view = this.View;

            // First, find out what this view is, and what the file is.
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            string path = doc.FilePath;

            List<IToken> where = new List<IToken>();
            List<ParserDetails> where_details = new List<ParserDetails>();
            foreach (var kvp in ParserDetails._per_file_parser_details)
            {
                string file_name = kvp.Key;
                ParserDetails details = kvp.Value;
                if (classification == AntlrVSIX.Constants.ClassificationNameNonterminal)
                {
                    var it = details._ant_nonterminals.Where(
                        (t) => t.Text == span.GetText());
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
                else if (classification == AntlrVSIX.Constants.ClassificationNameTerminal)
                {
                    var it = details._ant_terminals.Where(
                        (t) => t.Text == span.GetText());
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
                else if (classification == AntlrVSIX.Constants.ClassificationNameLiteral)
                {
                    var it = details._ant_literals.Where(
                        (t) => t.Text == span.GetText());
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
            }
            if (!where.Any()) return;

            // Populate the Antlr find results model/window with file/line/col info
            // for each occurrence.
            var results = new List<Entry>();
            for (int i = 0; i < where.Count; ++i)
            {
                IToken x = where[i];
                ParserDetails y = where_details[i];
                var w = new Entry() { FileName = y.FullFileName, LineNumber = x.Line, ColumnNumber = x.Column, Token = x };
                results.Add(w);
            }

            // Now, for all entries which are in this buffer, highlight.
            //wordSpans.Add(currentWord);
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

            // Call up the rename dialog box. In another thread because
            // of "The calling thread must be STA, because many UI components require this."
            // error.
            Application.Current.Dispatcher.Invoke((Action)delegate {

                RenameDialogBox inputDialog = new RenameDialogBox(currentWord.GetText());
                if (inputDialog.ShowDialog() == true)
                {
                    var new_name = inputDialog.Answer;
                    var files = results.Select(r => r.FileName).OrderBy(q => q).Distinct();
                    foreach (var f in files)
                    {
                        var per_file_results = results.Where(r => r.FileName == f);
                        per_file_results.Reverse();
                        var pd = ParserDetails._per_file_parser_details[f];
                        IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(f);
                        if (vstv == null)
                        {
                            // File has not been opened before! Open file in editor.
                            IVsTextViewExtensions.ShowFrame(f);
                            vstv = IVsTextViewExtensions.FindTextViewFor(f);
                        }
                        IWpfTextView wpftv = vstv.GetIWpfTextView();
                        ITextBuffer tb = wpftv.TextBuffer;
                        using (var edit = tb.CreateEdit())
                        {
                            ITextSnapshot cc = tb.CurrentSnapshot;
                            foreach (var e in per_file_results)
                            {
                                SnapshotSpan ss = new SnapshotSpan(cc, e.Token.StartIndex, 1 + e.Token.StopIndex - e.Token.StartIndex);
                                SnapshotPoint sp = ss.Start;
                                edit.Replace(ss, new_name);
                            }
                            edit.Apply();
                            ParserDetails.Parse(tb.GetBufferText(), f);
                        }
                    }

                    // Reparse everything.
                    //foreach (string f in results.Select((e) => e.FileName).Distinct())
                    //{
                    //    IVsTextView vstv = IVsTextViewExtensions.GetIVsTextView(f);
                    //    IWpfTextView wpftv = vstv.GetIWpfTextView();
                    //    if (wpftv == null) continue;
                    //    ITextBuffer tb = wpftv.TextBuffer;
                    //    ParserDetails.Parse(tb.GetBufferText(), f);
                    //}
                }
                // Get rid of replace tagging.
                //foreach (var kvp in _view_to_tagger)
                //{
                //    kvp.Value.WordSpans = new NormalizedSnapshotSpanCollection();
                //    kvp.Value.CurrentWord = null;
                //    var clear_view = kvp.Key;
                //    var tb = clear_view.TextBuffer;
                //    ITextSnapshot snapshot = tb.CurrentSnapshot;
                //    TagsChanged(this, new SnapshotSpanEventArgs(new SnapshotSpan(snapshot, new Span(0, snapshot.Length))));
                //}
                // Update Antlr tagging.
                //foreach (var kvp in _view_to_tagger)
                //{
                //    kvp.Value.WordSpans = new NormalizedSnapshotSpanCollection();
                //    kvp.Value.CurrentWord = null;
                //    var clear_view = kvp.Key;
                //    var tb = clear_view.TextBuffer;
                //    ITextSnapshot snapshot = tb.CurrentSnapshot;
                //    TagsChanged(this, new SnapshotSpanEventArgs(new SnapshotSpan(snapshot, new Span(0, snapshot.Length))));
                //}
            });
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
                    tempEvent(this, new SnapshotSpanEventArgs(new SnapshotSpan(SourceBuffer.CurrentSnapshot, 0, SourceBuffer.CurrentSnapshot.Length)));
            }
        }

        public IEnumerable<ITagSpan<HighlightWordTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (CurrentWord == null)
                yield break;

            // Hold on to a "snapshot" of the word spans and current word, so that we maintain the same
            // collection throughout
            SnapshotSpan currentWord = CurrentWord.Value;
            NormalizedSnapshotSpanCollection wordSpans = WordSpans;

            if (spans.Count == 0 || WordSpans.Count == 0)
                yield break;

            // If the requested snapshot isn't the same as the one our words are on, translate our spans
            // to the expected snapshot
            if (spans[0].Snapshot != wordSpans[0].Snapshot)
            {
                wordSpans = new NormalizedSnapshotSpanCollection(
                    wordSpans.Select(span => span.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive)));

                currentWord = currentWord.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive);
            }

            // First, yield back the word the cursor is under (if it overlaps)
            // Note that we'll yield back the same word again in the wordspans collection;
            // the duplication here is expected.
            if (spans.OverlapsWith(new NormalizedSnapshotSpanCollection(currentWord)))
                yield return new TagSpan<HighlightWordTag>(currentWord, new HighlightWordTag());

            // Second, yield all the other words in the file
            foreach (SnapshotSpan span in NormalizedSnapshotSpanCollection.Overlap(spans, wordSpans))
            {
                yield return new TagSpan<HighlightWordTag>(span, new HighlightWordTag());
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
    }
}
