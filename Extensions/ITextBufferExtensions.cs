namespace AntlrVSIX.Extensions
{
    using Microsoft.VisualStudio.Text;

    static class ITextBufferExtensions
    {
        public static string GetBufferText(this ITextBuffer buffer)
        {
            return buffer.CurrentSnapshot.GetText();
        }

        public static string GetFilePath(this ITextBuffer text_buffer)
        {
            ITextDocument text_doc;
            return text_buffer.Properties.TryGetProperty(
                typeof(ITextDocument), out text_doc)
                ? text_doc.FilePath : null;
        }

        public static ITextDocument GetTextDocument(this ITextBuffer text_buffer)
        {
            ITextDocument text_doc;
            return text_buffer.Properties.TryGetProperty<ITextDocument>(
                typeof(ITextDocument), out text_doc)
                ? text_doc : null;
        }
    }
}
