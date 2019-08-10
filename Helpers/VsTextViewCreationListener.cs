
namespace AntlrVSIX.Extensions
{
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Microsoft.VisualStudio.Utilities;
    using System;
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
            IWpfTextView wpftv = AdaptersFactory.GetWpfTextView(textViewAdapter);
            if (wpftv == null)
            {
                Debug.Fail("Unable to get IWpfTextView from text view adapter");
                return;
            }
            wpftv.Closed += OnViewClosed;
            to_wpftextview[textViewAdapter] = wpftv;
            to_ivstextview[wpftv] = textViewAdapter;
        }

        private void OnViewClosed(object sender, EventArgs e)
        {
            IWpfTextView view = sender as IWpfTextView;
            if (view == null) return;
            view.Closed -= OnViewClosed;

            if (to_ivstextview.ContainsKey(view))
            {
                var textViewAdapter = to_ivstextview[view];
                to_ivstextview.Remove(view);
                if (to_wpftextview.ContainsKey(textViewAdapter)) to_wpftextview.Remove(textViewAdapter);
            }
        }
    }
}
