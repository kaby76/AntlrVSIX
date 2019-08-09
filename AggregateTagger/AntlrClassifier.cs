namespace AntlrVSIX.AggregateTagger
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.Package;
    using AntlrVSIX.Tagger;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text;
    using System.Collections.Generic;
    using System;

    internal sealed class AntlrClassifier : ITagger<ClassificationTag>, IObserver<ParserDetails>
    {
        public ITextBuffer _buffer = null;
        public ITextView _view = null;
        public ITagAggregator<AntlrTokenTag> _aggregator;
        private IDictionary<AntlrTagTypes, IClassificationType> _antlrtype_to_classifiertype;
        public static Dictionary<ITextBuffer, AntlrClassifier> _buffer_to_classifier = new Dictionary<ITextBuffer, AntlrClassifier>();
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        internal AntlrClassifier(
            ITextView view,
            ITextBuffer buffer,
            ITagAggregator<AntlrTokenTag> aggregator,
            IClassificationTypeRegistryService service)
        {
            _view = view;
            _buffer = buffer;
            _buffer_to_classifier[buffer] = this;

            _aggregator = aggregator;
            _antlrtype_to_classifiertype = new Dictionary<AntlrTagTypes, IClassificationType>();
            _antlrtype_to_classifiertype[AntlrTagTypes.Nonterminal] = service.GetClassificationType(Constants.ClassificationNameNonterminal);
            _antlrtype_to_classifiertype[AntlrTagTypes.Terminal] = service.GetClassificationType(Constants.ClassificationNameTerminal);
            _antlrtype_to_classifiertype[AntlrTagTypes.Comment] = service.GetClassificationType(Constants.ClassificationNameComment);
            _antlrtype_to_classifiertype[AntlrTagTypes.Keyword] = service.GetClassificationType(Constants.ClassificationNameKeyword);
            _antlrtype_to_classifiertype[AntlrTagTypes.Literal] = service.GetClassificationType(Constants.ClassificationNameLiteral);
            _antlrtype_to_classifiertype[AntlrTagTypes.Other] = service.GetClassificationType("other");
            
            // Ensure package is loaded.
            AntlrLanguagePackage package = AntlrLanguagePackage.Instance;

            ITextDocument doc = _buffer.GetTextDocument();
            string f = doc.FilePath;
            var pd = ParserDetails.Parse(_buffer.GetBufferText(), f);
            // pd.Subscribe(this);
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            ITextDocument doc = _buffer.GetTextDocument();
            string f = doc.FilePath;
            var pd = ParserDetails.Parse(_buffer.GetBufferText(), f);

            foreach (IMappingTagSpan<AntlrTokenTag> tag_span in _aggregator.GetTags(spans))
            {
                NormalizedSnapshotSpanCollection tag_spans = tag_span.Span.GetSpans(spans[0].Snapshot);
                yield return
                    new TagSpan<ClassificationTag>(tag_spans[0],
                        new ClassificationTag(_antlrtype_to_classifiertype[tag_span.Tag.TagType]));
            }
        }


        public void OnNext(ParserDetails value)
        {
            return;
            // This code does not work...
            ITextSnapshot snapshot = _buffer.CurrentSnapshot;
            ITextDocument doc = _buffer.GetTextDocument();
            string f = doc.FilePath;
            if (TagsChanged != null)
            {
                TagsChanged(this,
                    new SnapshotSpanEventArgs(
                        new SnapshotSpan(snapshot, new Span(0, snapshot.Length))));
            }
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}
