using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace AntlrLanguage
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("Antlr")]
    [TagType(typeof(ClassificationTag))]
    internal sealed class AntlrClassifierProvider : ITaggerProvider
    {
        [Export]
        [Name("Antlr")]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition AntlrContentType = null;

        [Export]
        [FileExtension(".g4")]
        [ContentType("Antlr")]
        internal static FileExtensionToContentTypeDefinition AntlrFileType = null;

        [Import]
        internal IClassificationTypeRegistryService ClassificationTypeRegistry = null;

        [Import]
        internal IBufferTagAggregatorFactoryService aggregatorFactory = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            ITagAggregator<AntlrTokenTag> antlrTagAggregator = 
                                            aggregatorFactory.CreateTagAggregator<AntlrTokenTag>(buffer);

            return new AntlrClassifier(buffer, antlrTagAggregator, ClassificationTypeRegistry) as ITagger<T>;
        }
    }

    internal sealed class AntlrClassifier : ITagger<ClassificationTag>
    {
        ITextBuffer _buffer = null;
        ITagAggregator<AntlrTokenTag> _aggregator;
        IDictionary<AntlrTokenTypes, IClassificationType> _antlrTypes;

        /// <summary>
        /// Construct the classifier and define search tokens
        /// </summary>
        internal AntlrClassifier(ITextBuffer buffer, 
                               ITagAggregator<AntlrTokenTag> antlrTagAggregator, 
                               IClassificationTypeRegistryService typeService)
        {
            _buffer = buffer;
            _aggregator = antlrTagAggregator;
            _antlrTypes = new Dictionary<AntlrTokenTypes, IClassificationType>();
            _antlrTypes[AntlrTokenTypes.Nonterminal] = typeService.GetClassificationType("nonterminal");
            _antlrTypes[AntlrTokenTypes.Terminal] = typeService.GetClassificationType("terminal");
            _antlrTypes[AntlrTokenTypes.Comment] = typeService.GetClassificationType("acomment");
            _antlrTypes[AntlrTokenTypes.Keyword] = typeService.GetClassificationType("akeyword");
            _antlrTypes[AntlrTokenTypes.Other] = typeService.GetClassificationType("other");
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }


        // For each span given, classify the tag.
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
