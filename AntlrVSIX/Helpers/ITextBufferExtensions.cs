namespace AntlrVSIX.Extensions
{
    using Microsoft.VisualStudio.Text;
    using System.Threading.Tasks;

    static class ITextBufferExtensions
    {
        public static string GetBufferText(this ITextBuffer buffer)
        {
            return buffer.CurrentSnapshot.GetText();
        }

        public static async Task<string> GetFFN(this ITextBuffer text_buffer)
        {
            if (text_buffer == null) return null;
            Microsoft.VisualStudio.TextManager.Interop.IVsTextBuffer bufferAdapter;
            text_buffer.Properties.TryGetProperty(typeof(Microsoft.VisualStudio.TextManager.Interop.IVsTextBuffer), out bufferAdapter);
            if (bufferAdapter != null)
            {
                await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                var persistFileFormat = bufferAdapter as Microsoft.VisualStudio.Shell.Interop.IPersistFileFormat;
                string ppzsFilename = null;
                uint iii;
                if (persistFileFormat != null) persistFileFormat.GetCurFile(out ppzsFilename, out iii);
                return ppzsFilename;
            }
            return null;
        }
    }
}
