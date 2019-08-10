namespace AntlrVSIX.ErrorTagger
{
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    [Export(typeof(ITaggerProvider))]
    [ContentType(AntlrVSIX.Constants.ContentType)]
    [TagType(typeof(ErrorTag))]
    class ErrorTaggerProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return buffer.Properties.GetOrCreateSingletonProperty(() => new ErrorTagger(buffer)) as ITagger<T>;
        }
    }
}
