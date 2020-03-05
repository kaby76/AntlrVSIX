namespace LspAntlr
{
    using Microsoft.VisualStudio.Text;
    using System.Threading.Tasks;

    public static class Helpers
    {
        public static async Task<string> GetFFN(this ITextBuffer buffer)
        {
            if (buffer == null)
            {
                return null;
            }

            Microsoft.VisualStudio.Text.Projection.IElisionBuffer projection = buffer as Microsoft.VisualStudio.Text.Projection.IElisionBuffer;
            if (projection != null)
            {
                ITextBuffer source_buffer = projection.SourceBuffer;
                return await source_buffer.GetFFN();
            }
            buffer.Properties.TryGetProperty(typeof(Microsoft.VisualStudio.TextManager.Interop.IVsTextBuffer), out Microsoft.VisualStudio.TextManager.Interop.IVsTextBuffer bufferAdapter);
            if (bufferAdapter != null)
            {
                await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                Microsoft.VisualStudio.Shell.Interop.IPersistFileFormat persistFileFormat = bufferAdapter as Microsoft.VisualStudio.Shell.Interop.IPersistFileFormat;
                string ppzsFilename = null;
                if (persistFileFormat != null)
                {
                    persistFileFormat.GetCurFile(out ppzsFilename, out uint iii);
                }

                return ppzsFilename;
            }
            return null;
        }
    }
}