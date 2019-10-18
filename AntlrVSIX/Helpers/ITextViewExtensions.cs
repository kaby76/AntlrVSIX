namespace AntlrVSIX.Extensions
{
    using Microsoft.VisualStudio.Text.Editor;
    using System.Threading.Tasks;

    internal static class ITextViewExtensions
    {
        public static async Task<string> GetFilePath(this ITextView textView)
        {
            return await textView.TextBuffer.GetFFN();
        }
    }
}
