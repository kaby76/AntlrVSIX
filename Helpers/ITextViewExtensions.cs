namespace AntlrVSIX.Extensions
{
    using Microsoft.VisualStudio.Text.Editor;

    internal static class ITextViewExtensions
    {
        public static string GetFilePath(this ITextView textView)
        {
            return textView.TextBuffer.GetFilePath();
        }
    }
}
