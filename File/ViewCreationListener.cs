using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using AntlrVSIX.Extensions;
using AntlrVSIX.Grammar;

namespace AntlrVSIX.File
{

    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("code")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    public class ViewCreationListener : IVsTextViewCreationListener
    {
        [Import]
        public IVsEditorAdaptersFactoryService AdaptersFactory = null;

        [Import]
        public ITextDocumentFactoryService TextDocumentFactoryService { get; set; }

        [Import]
        public SVsServiceProvider GlobalServiceProvider = null;
        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            var wpftv = AdaptersFactory.GetWpfTextView(textViewAdapter);
            if (wpftv == null)
            {
                Debug.Fail("Unable to get IWpfTextView from text view adapter");
                return;
            }
            IVsTextView vstv = AdaptersFactory.GetViewAdapter(wpftv);
            ITextBuffer doc = wpftv.TextBuffer;
            string ffn = doc.GetFilePath();
            if (Path.GetExtension(ffn).ToLower() != ".g4") return;

            if (!ParserDetails._per_file_parser_details.ContainsKey(ffn))
            {
                StreamReader sr = new StreamReader(ffn);
                ParserDetails foo = new ParserDetails();
                ParserDetails._per_file_parser_details[ffn] = foo;
                foo.Parse(sr.ReadToEnd(), ffn);
            }
        }
    }
}
