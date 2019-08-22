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
    using System.Linq;
    using AntlrVSIX.Package;
    using AntlrVSIX.Tagger;
    using Microsoft.VisualStudio.Text.Classification;


    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("any")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    public class ViewCreationListener : IVsTextViewCreationListener
    {
        [Import]
        public IVsEditorAdaptersFactoryService AdaptersFactory = null;

        [Import]
        internal IContentTypeRegistryService ContentTypeRegistryService { get; set; }

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
            var grammar_description = GrammarDescriptionFactory.Create(ffn);
            if (grammar_description == null) return;
            var buffer = view.TextBuffer;
            var content_type = buffer.ContentType;
            System.Collections.Generic.List<IContentType> content_types = ContentTypeRegistryService.ContentTypes.ToList();
            var found = content_types.Find(ct => ct.TypeName == "Antlr");
            var type_of_content_type = found.GetType();
            var assembly = type_of_content_type.Assembly;
            buffer.ChangeContentType(found, null);
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
