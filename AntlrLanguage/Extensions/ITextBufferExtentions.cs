using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;

namespace AntlrLanguage.Extensions
{
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
