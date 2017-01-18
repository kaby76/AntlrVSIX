namespace AntlrVSIX.Tagger
{
    using Microsoft.VisualStudio.Text.Tagging;

    public class AntlrTokenTag : ITag
    {
        public AntlrTokenTypes TokenType { get; private set; }

        public AntlrTokenTag(AntlrTokenTypes token_type)
        {
            this.TokenType = token_type;
        }
    }
}
