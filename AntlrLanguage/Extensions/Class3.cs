using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace AntlrLanguage.Extensions
{
    public static class TextExtensions
    {
        public static string GetFilePath(this ITextView textView)
        {
            return textView.TextBuffer.GetFilePath();
        }

        public static string GetFilePath(this ITextBuffer textBuffer)
        {
            ITextDocument textDocument;
            if (textBuffer.Properties.TryGetProperty(typeof(ITextDocument), out textDocument))
            {
                return textDocument.FilePath;
            }
            else
            {
                return null;
            }
        }
    }
}
