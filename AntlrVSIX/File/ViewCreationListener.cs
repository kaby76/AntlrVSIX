namespace AntlrVSIX.File
{
    using AntlrVSIX.Extensions;
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
                var grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
                if (grammar_description == null) return;
                var document = Workspaces.Workspace.Instance.FindDocument(ffn);
                if (document == null)
                {
                    var name = "Miscellaneous Files";
                    var project = Workspaces.Workspace.Instance.FindProject(name);
                    if (project == null)
                    {
                        project = new Workspaces.Project(name, name, name);
                        Workspaces.Workspace.Instance.AddChild(project);
                    }
                    document = new Workspaces.Document(ffn, ffn);
                    project.AddDocument(document);
                }
                Workspaces.Loader.LoadAsync().Wait();
                var content_type = buffer.ContentType;
                System.Collections.Generic.List<IContentType> content_types = ContentTypeRegistryService.ContentTypes.ToList();
                var new_content_type = content_types.Find(ct => ct.TypeName == "Antlr");
                var type_of_content_type = new_content_type.GetType();
                var assembly = type_of_content_type.Assembly;
                buffer.ChangeContentType(new_content_type, null);
                if (!PreviousContentType.ContainsKey(ffn))
                    PreviousContentType[ffn] = content_type;
                var to_do = LanguageServer.Module.Compile();
                var att = buffer.Properties.GetOrCreateSingletonProperty(() => new AntlrVSIX.AggregateTagger.AntlrClassifier(buffer));
                att.Raise();
            }
            catch (Exception e)
            {
                Logger.Log.Notify(e.ToString());
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
