namespace AntlrVSIX.Extensions
{
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Microsoft.VisualStudio.Utilities;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;

    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("code")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    public class VsTextViewCreationListener : IVsTextViewCreationListener
    {
        [Import]
        public IVsEditorAdaptersFactoryService AdaptersFactory = null;

        [Import]
        public ITextDocumentFactoryService TextDocumentFactoryService { get; set; }

        [Import]
        public SVsServiceProvider GlobalServiceProvider = null;

        public static Dictionary<IVsTextView, IWpfTextView> to_wpftextview = new Dictionary<IVsTextView, IWpfTextView>();
        public static Dictionary<IWpfTextView, IVsTextView> to_ivstextview = new Dictionary<IWpfTextView, IVsTextView>();

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            var wpftv = AdaptersFactory.GetWpfTextView(textViewAdapter);
            if (wpftv == null)
            {
                Debug.Fail("Unable to get IWpfTextView from text view adapter");
                return;
            }
            to_wpftextview[textViewAdapter] = wpftv;
            to_ivstextview[wpftv] = textViewAdapter;

            IVsTextView vstv = AdaptersFactory.GetViewAdapter(wpftv);
            // vstv should be equal to textViewAdapter.
        }
    }
}
