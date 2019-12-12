using System.IO;

namespace LspAntlr
{
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Microsoft.VisualStudio.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;


    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("any")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    public class ViewCreationListener : IVsTextViewCreationListener
    {

        public static Dictionary<string, IContentType> PreviousContentType = new Dictionary<string, IContentType>();

        [Import]
        public IVsEditorAdaptersFactoryService AdaptersFactory = null;

        [Import]
        internal IContentTypeRegistryService ContentTypeRegistryService { get; set; }

        public async void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            try
            {
                IWpfTextView view = AdaptersFactory.GetWpfTextView(textViewAdapter);
                if (view == null) return;
                view.Closed += OnViewClosed;
                var buffer = view.TextBuffer;
                if (buffer == null) return;
                string ffn = await buffer.GetFFN();
                if (ffn == null) return;
                if (!(Path.GetExtension(ffn) == ".g4"))
                    return;
                var content_type = buffer.ContentType;
                System.Collections.Generic.List<IContentType> content_types = ContentTypeRegistryService.ContentTypes.ToList();
                var new_content_type = content_types.Find(ct => ct.TypeName == "Antlr");
                var type_of_content_type = new_content_type.GetType();
                var assembly = type_of_content_type.Assembly;
                buffer.ChangeContentType(new_content_type, null);
                if (!PreviousContentType.ContainsKey(ffn))
                    PreviousContentType[ffn] = content_type;
                
            }
            catch (Exception e)
            {
            }
        }

        private void OnViewClosed(object sender, EventArgs e)
        {
            IWpfTextView view = sender as IWpfTextView;
            if (view == null) return;
            view.Closed -= OnViewClosed;
        }
    }
}
