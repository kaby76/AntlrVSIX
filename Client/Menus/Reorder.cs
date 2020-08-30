namespace LspAntlr
{
    using LoggerNs;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.ComponentModel.Design;

    internal class Reorder
    {
        private readonly AntlrLanguageClient _package;
        private readonly MenuCommand _menu_item1;
        private readonly MenuCommand _menu_item2;
        private readonly MenuCommand _menu_item3;

        private Reorder(AntlrLanguageClient package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            OleMenuCommandService commandService = ((IServiceProvider)ServiceProvider).GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService ?? throw new Exception("Command service not found.");

            {
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7014);
                _menu_item1 = new MenuCommand(MenuItemCallbackAlphabetically, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item1);
            }
            {
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7015);
                _menu_item2 = new MenuCommand(MenuItemCallbackBFS, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item2);
            }
            {
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7016);
                _menu_item3 = new MenuCommand(MenuItemCallbackDFS, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item3);
            }
        }

        public bool Enabled
        {
            set
            {
                if (_menu_item1 != null)
                {
                    _menu_item1.Enabled = value;
                }
            }
        }

        public bool Visible
        {
            set
            {
                if (_menu_item1 != null)
                {
                    _menu_item1.Visible = value;
                }
            }
        }

        public static Reorder Instance { get; private set; }

        private AntlrLanguageClient ServiceProvider => _package;

        public static void Initialize(AntlrLanguageClient package)
        {
            Instance = new Reorder(package);
        }

        private void MenuItemCallbackAlphabetically(object sender, EventArgs e)
        {
            MenuItemCallback(ReorderType.Alphabetically);
        }

        private void MenuItemCallbackBFS(object sender, EventArgs e)
        {
            MenuItemCallback(ReorderType.BFS);
        }

        private void MenuItemCallbackDFS(object sender, EventArgs e)
        {
            MenuItemCallback(ReorderType.DFS);
        }

        private void MenuItemCallback(ReorderType type)
        {
            try
            {
                ////////////////////////
                /// Reorder parser productions.
                ////////////////////////

                if (!(((IServiceProvider)ServiceProvider).GetService(typeof(VsTextManagerClass)) is IVsTextManager manager)) return;
                manager.GetActiveView(1, null, out IVsTextView view);
                if (view == null) return;
                view.GetCaretPos(out int l, out int c);
                view.GetBuffer(out IVsTextLines buf);
                if (buf == null) return;
                ITextBuffer buffer = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(view)?.TextBuffer;
                string ffn = buffer.GetFFN();
                if (ffn == null) return;
                Workspaces.Document document = Workspaces.Workspace.Instance.FindDocument(ffn);
                if (document == null) return;
                AntlrLanguageClient.CMReorderParserRules(ffn, type);
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }
        }
    }
}
