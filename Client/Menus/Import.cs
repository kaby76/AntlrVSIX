namespace LspAntlr
{
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.ComponentModel.Design;
    using System.Windows;

    public class Import
    {
        private readonly Microsoft.VisualStudio.Shell.Package _package;
        private readonly MenuCommand _menu_item1;
        private readonly MenuCommand _menu_item2;

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
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7010);
                _menu_item1 = new MenuCommand(MenuItemCallback, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item1);
            }
            {
                // Set up hook for context menu.
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7010);
                _menu_item2 = new MenuCommand(MenuItemCallback, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item2);
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
            Application.Current.Dispatcher.Invoke(delegate
            {
                AboutBox inputDialog = new AboutBox();
                inputDialog.Show();
            });
        }
    }
}
