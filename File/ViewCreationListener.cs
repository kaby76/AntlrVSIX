namespace AntlrVSIX.File
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Microsoft.VisualStudio.Utilities;
    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics;

    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("code")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    public class ViewCreationListener : IVsTextViewCreationListener
    {
        private string document_file_path;

        [Import]
        public IVsEditorAdaptersFactoryService AdaptersFactory = null;

        [Import]
        public ITextDocumentFactoryService TextDocumentFactoryService { get; set; }

        [Import]
        public SVsServiceProvider GlobalServiceProvider = null;
        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            IWpfTextView view = AdaptersFactory.GetWpfTextView(textViewAdapter);
            if (view == null)
            {
                Debug.Fail("Unable to get IWpfTextView from text view adapter");
                return;
            }
            view.Closed += OnViewClosed;
            ITextBuffer doc = view.TextBuffer;
            string ffn = doc.GetFilePath();
            if (!ffn.IsAntlrSuffix()) return;
            document_file_path = ffn;
            var buffer = view.TextBuffer;
            var code = buffer.GetBufferText();
            ParserDetails pd = ParserDetails.Parse(code, ffn);
        }

        private void OnViewClosed(object sender, EventArgs e)
        {
            IWpfTextView view = sender as IWpfTextView;
            if (view == null) return;
            view.Closed -= OnViewClosed;
        }
    }
}
