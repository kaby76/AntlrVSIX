namespace AntlrVSIX.Rename
{
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    [Export(typeof(IViewTaggerProvider))]
    [ContentType(AntlrVSIX.Constants.ContentType)]
    [TagType(typeof(HighlightWordTag))]
    public class HighlightWordTaggerProvider : IViewTaggerProvider
    {
        [Import]
        IClassifierAggregatorService AggregatorFactory = null;

        [Import]
        internal ITextSearchService TextSearchService { get; set; }

        [Import]
        internal ITextStructureNavigatorSelectorService TextStructureNavigatorSelector { get; set; }

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            if (textView.TextBuffer != buffer)
                return null;
            return new HighlightTagger(textView, buffer, TextSearchService,
                TextStructureNavigatorSelector.GetTextStructureNavigator(buffer),
                AggregatorFactory.GetClassifier(buffer)) as ITagger<T>;
        }
    }
}
