namespace AntlrVSIX.Rename
{
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    [Export(typeof(IViewTaggerProvider))]
    [ContentType(Constants.ContentType)]
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

            ITextStructureNavigator textStructureNavigator =
                TextStructureNavigatorSelector.GetTextStructureNavigator(buffer);
            
            IClassifier ag1 = AggregatorFactory.GetClassifier(buffer);

            return new RenameHighlightTagger(textView, buffer, TextSearchService, textStructureNavigator, ag1) as ITagger<T>;
        }
    }
}
