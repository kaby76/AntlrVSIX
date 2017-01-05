using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace AntlrLanguage.Tag
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("Antlr")]
    [TagType(typeof(AntlrTokenTag))]
    internal sealed class AntlrTokenTagProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new AntlrTokenTagger(buffer) as ITagger<T>;
        }
    }
}
