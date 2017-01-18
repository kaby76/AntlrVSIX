namespace AntlrVSIX.Extensions
{
    using Microsoft.VisualStudio.Text;

    internal static class ITextBufferExtentions
    {
        public static ITextDocument GetTextDocument(this ITextBuffer TextBuffer)
        {
            ITextDocument textDoc;
            var rc = TextBuffer.Properties.TryGetProperty<ITextDocument>(
                typeof(ITextDocument), out textDoc);
            if (rc == true)
                return textDoc;
            else
                return null;
        }
    }
}
