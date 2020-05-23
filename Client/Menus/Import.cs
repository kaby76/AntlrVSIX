namespace LspAntlr
{
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.ComponentModel.Design;
    using System.Linq;
    using System.Windows;

    public class Import
    {
        private readonly Microsoft.VisualStudio.Shell.Package _package;
        private readonly MenuCommand _menu_item1;
        //private readonly MenuCommand _menu_item2;

        private Import(Microsoft.VisualStudio.Shell.Package package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            OleMenuCommandService commandService = ((IServiceProvider)ServiceProvider).GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService ?? throw new Exception("Command service not found.");

            {
                // Set up hook for context menu.
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7019);
                _menu_item1 = new MenuCommand(MenuItemCallback, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item1);
            }
        }
        public static Import Instance { get; private set; }

        private IServiceProvider ServiceProvider => _package;

        public static void Initialize(Microsoft.VisualStudio.Shell.Package package)
        {
            Instance = new Import(package);
        }

        private async void MenuItemCallback(object sender, EventArgs e)
        {
            IVsTextManager manager = ServiceProvider.GetService(typeof(VsTextManagerClass)) as IVsTextManager;
            if (manager == null)
            {
                return;
            }
            // If we have a view, we'll place the imported file in the parent project.
            // Otherwise the "misc project".
            EnvDTE.Project project = null;
            string the_namespace = "";
            for (; ; )
            {
                manager.GetActiveView(1, null, out IVsTextView view);
                if (view == null)
                {
                    break;
                }

                view.GetBuffer(out IVsTextLines buf);
                if (buf == null)
                {
                    break;
                }

                IWpfTextView xxx = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(view);
                ITextBuffer buffer = xxx.TextBuffer;
                string ffn = buffer.GetFFN().Result;
                if (ffn == null)
                {
                    break;
                }

                string current_grammar_ffn = ffn;
                (EnvDTE.Project, EnvDTE.ProjectItem) p_f_original_grammar = LspAntlr.MakeChanges.FindProjectAndItem(current_grammar_ffn);
                project = p_f_original_grammar.Item1;
                try
                {
                    object prop = p_f_original_grammar.Item2.Properties.Item("CustomToolNamespace").Value;
                    the_namespace = prop.ToString();
                }
                catch (Exception)
                {
                }
                break;
            }
            if (project == null)
            {
                EnvDTE.DTE dte = (EnvDTE.DTE)Package.GetGlobalService(typeof(EnvDTE.DTE));
                EnvDTE.Projects projects = dte.Solution.Projects;
                project = projects.Item(EnvDTE.Constants.vsMiscFilesProjectUniqueName);
            }

            ImportBox dialog_box = new ImportBox();
            Application.Current.Dispatcher.Invoke(delegate
            {
                dialog_box.ShowDialog();
                System.Collections.Generic.List<ImportBox.StringValue> xx = dialog_box.list;
                if (xx == null)
                {
                    return;
                }
                // Note, the language client cannot be applied here because we
                // aren't focused on a grammar file, and may not even have a window
                // open!
                System.Collections.Generic.Dictionary<string, string> changes = LanguageServer.BisonImport.ImportGrammars(xx.Select(t => t._ffn).ToList());
                LspAntlr.MakeChanges.EnterChanges(changes, project, the_namespace);
            });
        }
    }
}
