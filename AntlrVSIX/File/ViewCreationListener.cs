namespace AntlrVSIX.File
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Tagger;
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Microsoft.VisualStudio.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
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
            IWpfTextView view = AdaptersFactory.GetWpfTextView(textViewAdapter);
            if (view == null)
            {
                Debug.Fail("Unable to get IWpfTextView from text view adapter");
                return;
            }
            view.Closed += OnViewClosed;
            var buffer = view.TextBuffer;
            string ffn = await buffer.GetFFN();
            var grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
            if (grammar_description == null) return;
            var content_type = buffer.ContentType;
            System.Collections.Generic.List<IContentType> content_types = ContentTypeRegistryService.ContentTypes.ToList();
            var new_content_type = content_types.Find(ct => ct.TypeName == "Antlr");
            var type_of_content_type = new_content_type.GetType();
            var assembly = type_of_content_type.Assembly;
            buffer.ChangeContentType(new_content_type, null);
            if (!PreviousContentType.ContainsKey(ffn))
                PreviousContentType[ffn] = content_type;
            var att = buffer.Properties.GetOrCreateSingletonProperty(() => new AntlrVSIX.AggregateTagger.AntlrClassifier(buffer));
            att.Raise();
            
        }

        private void OnViewClosed(object sender, EventArgs e)
        {
            IWpfTextView view = sender as IWpfTextView;
            if (view == null) return;
            view.Closed -= OnViewClosed;
        }
    }
}
