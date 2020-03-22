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
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            _package = package;
            OleMenuCommandService commandService = ServiceProvider.GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService == null)
            {
                throw new ArgumentNullException("OleMenuCommandService");
            }

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

        private void MenuItemCallback(object sender, EventArgs e)
        {
            IVsTextManager manager = ((IServiceProvider)ServiceProvider).GetService(typeof(VsTextManagerClass)) as IVsTextManager;
            if (manager == null)
            {
                return;
            }

            manager.GetActiveView(1, null, out IVsTextView view);
            if (view == null)
            {
                return;
            }

            view.GetCaretPos(out int l, out int c);
            view.GetBuffer(out IVsTextLines buf);
            if (buf == null)
            {
                return;
            }

            IWpfTextView xxx = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(view);
            ITextBuffer buffer = xxx.TextBuffer;
            string ffn = buffer.GetFFN().Result;
            if (ffn == null)
            {
                return;
            }
            var current_grammar_ffn = ffn;

            ImportBox dialog_box = new ImportBox();
            Application.Current.Dispatcher.Invoke(delegate
            {
                dialog_box.ShowDialog();
                var xx = dialog_box.list;
                if (xx == null) return;
                AntlrLanguageClient alc = AntlrLanguageClient.Instance;
                if (alc == null) return;
                System.Collections.Generic.Dictionary<string, string> changes = alc.CMImportGrammarsServer(xx.Select(t => t._ffn).ToList());
                new LspAntlr.MakeChanges().EnterChanges(current_grammar_ffn, changes);
            });
        }
    }
}
