using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace AntlrLanguage.Tag
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("Antlr")]
    [TagType(typeof(AntlrTokenTag))]
    internal sealed class AntlrTokenTagProvider : ITaggerProvider
    {
        [Import]
        SVsServiceProvider GlobalServiceProvider = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new AntlrTokenTagger(null, buffer) as ITagger<T>;
        }
    }
}
