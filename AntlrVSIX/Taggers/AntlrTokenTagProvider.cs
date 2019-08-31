namespace AntlrVSIX.Tagger
{
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    [Export(typeof(ITaggerProvider))]
    [ContentType("any")]
    [TagType(typeof(AntlrTokenTag))]
    internal sealed class AntlrTokenTagProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return buffer.Properties.GetOrCreateSingletonProperty(() => new AntlrTokenTagger(buffer)) as ITagger<T>;
        }
    }
}
