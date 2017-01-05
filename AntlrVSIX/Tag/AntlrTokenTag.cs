using Microsoft.VisualStudio.Text.Tagging;

namespace AntlrLanguage.Tag
{
    public class AntlrTokenTag : ITag
    {
        public AntlrTokenTypes type { get; private set; }

        public AntlrTokenTag(AntlrTokenTypes type)
        {
            this.type = type;
        }
    }
}
