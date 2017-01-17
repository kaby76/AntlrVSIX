namespace AntlrVSIX.Tagger
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(ITaggerProvider))]
    [ContentType(Constants.ContentType)]
    [TagType(typeof(AntlrTokenTag))]
    internal sealed class AntlrTokenTagProvider : ITaggerProvider
    {
        [Import]
        SVsServiceProvider GlobalServiceProvider = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new AntlrTokenTagger(null, buffer, GlobalServiceProvider) as ITagger<T>;
        }
    }
}
