namespace AntlrVSIX.Tag
{
    using Microsoft.VisualStudio.Text.Tagging;

    public class AntlrTokenTag : ITag
    {
        public AntlrTokenTypes type { get; private set; }

        public AntlrTokenTag(AntlrTokenTypes type)
        {
            this.type = type;
        }
    }
}
