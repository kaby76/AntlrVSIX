namespace AntlrVSIX.Tagger
{
    using Microsoft.VisualStudio.Text.Tagging;

    public class AntlrTokenTag : ITag
    {
        public int TagType { get; private set; }

        public AntlrTokenTag(int token_type)
        {
            this.TagType = token_type;
        }
    }
}
