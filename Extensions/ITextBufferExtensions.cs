namespace AntlrVSIX.Extensions
{
    using Microsoft.VisualStudio.Text;

    static class ITextBufferExtensions
    {
        public static string GetBufferText(this ITextBuffer buffer)
        {
            return buffer.CurrentSnapshot.GetText();
        }
    }
}
