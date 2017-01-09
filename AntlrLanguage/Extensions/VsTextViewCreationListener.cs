using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text.Editor;

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using System.Diagnostics;

namespace AntlrLanguage.Extensions
{
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("code")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    public class VsTextViewCreationListener : IVsTextViewCreationListener
    {
        [Import]
        IVsEditorAdaptersFactoryService AdaptersFactory = null;

        public static Dictionary<IVsTextView, IWpfTextView> to_wpftextview = new Dictionary<IVsTextView, IWpfTextView>();
        public static Dictionary<IWpfTextView, IVsTextView> to_ivstextview = new Dictionary<IWpfTextView, IVsTextView>();
        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            var wpfTextView = AdaptersFactory.GetWpfTextView(textViewAdapter);
            if (wpfTextView == null)
            {
                Debug.Fail("Unable to get IWpfTextView from text view adapter");
                return;
            }
            to_wpftextview[textViewAdapter] = wpfTextView;
            to_ivstextview[wpfTextView] = textViewAdapter;

            Debug.Write(wpfTextView.TextBuffer.ContentType.TypeName);
        }
    }
}
