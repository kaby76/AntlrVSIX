namespace AntlrVSIX.Classification
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.Navigate;
    using AntlrVSIX.Tagger;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System.Collections.Generic;
    using System;

    internal sealed class AntlrClassifier : ITagger<ClassificationTag>
    {
        public ITextBuffer _buffer = null;
        public ITextView _view = null;
        public ITagAggregator<AntlrTokenTag> _aggregator;
        IDictionary<AntlrTokenTypes, IClassificationType> _antlrTypes;
        public static Dictionary<ITextBuffer, AntlrClassifier> _buffer_to_classifier = new Dictionary<ITextBuffer, AntlrClassifier>();

        internal AntlrClassifier(
            ITextView view,
            ITextBuffer buffer,
            ITagAggregator<AntlrTokenTag> antlrTagAggregator,
            IClassificationTypeRegistryService typeService)
        {
            _view = view;
            _buffer = buffer;
            _buffer_to_classifier[buffer] = this;

            _aggregator = antlrTagAggregator;
            _antlrTypes = new Dictionary<AntlrTokenTypes, IClassificationType>();
            _antlrTypes[AntlrTokenTypes.Nonterminal] = typeService.GetClassificationType(Constants.ClassificationNameNonterminal);
            _antlrTypes[AntlrTokenTypes.Terminal] = typeService.GetClassificationType(Constants.ClassificationNameTerminal);
            _antlrTypes[AntlrTokenTypes.Comment] = typeService.GetClassificationType(Constants.ClassificationNameComment);
            _antlrTypes[AntlrTokenTypes.Keyword] = typeService.GetClassificationType(Constants.ClassificationNameKeyword);
            _antlrTypes[AntlrTokenTypes.Literal] = typeService.GetClassificationType(Constants.ClassificationNameLiteral);
            _antlrTypes[AntlrTokenTypes.Other] = typeService.GetClassificationType("other");
            
            // Ensure package is loaded.
            var package = AntlrLanguagePackage.Instance;

            buffer.Changed += BufferChanged;
           // buffer.ContentTypeChanged += BufferContentTypeChanged;
        }

        private void BufferChanged(object sender, TextContentChangedEventArgs e)
        {
            // Non-incremental parse. Future work: if performance becomes a problem, it would
            // probably be best to make the lexical analyzer incremental, then
            // do a full parse.
            ITextSnapshot snapshot = _buffer.CurrentSnapshot;
            EventHandler<SnapshotSpanEventArgs> tagsChanged = TagsChanged;
            if (tagsChanged != null)
            {
                ParserDetails foo = new ParserDetails();
                ITextDocument doc = _buffer.GetTextDocument();
                string f = doc.FilePath;
                ParserDetails._per_file_parser_details[f] = foo;
                IVsTextView vstv = IVsTextViewExtensions.GetIVsTextView(f);
                IWpfTextView wpftv = vstv.GetIWpfTextView();
                if (wpftv == null) return;
                ITextBuffer tb = wpftv.TextBuffer;
                foo.Parse(tb.GetBufferText(), f);
                tagsChanged(this, new SnapshotSpanEventArgs(new SnapshotSpan(snapshot, new Span(0, snapshot.Length))));
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (var tagSpan in _aggregator.GetTags(spans))
            {
                var tagSpans = tagSpan.Span.GetSpans(spans[0].Snapshot);
                yield return
                    new TagSpan<ClassificationTag>(tagSpans[0],
                        new ClassificationTag(_antlrTypes[tagSpan.Tag.type]));
            }
        }
    }
}
