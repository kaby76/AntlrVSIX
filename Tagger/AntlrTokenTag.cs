namespace AntlrVSIX.Tagger
{
    using Microsoft.VisualStudio.Text.Tagging;

    public class AntlrTokenTag : ITag
    {
        public AntlrTagTypes TagType { get; private set; }

        public AntlrTokenTag(AntlrTagTypes token_type)
        {
            this.TagType = token_type;
        }
    }
}
