namespace AntlrVSIX.About
{
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.ComponentModel.Design;
    using System.Windows;

    public class AboutCommand
    {
        private readonly Microsoft.VisualStudio.Shell.Package _package;
        private MenuCommand _menu_item1;
        private MenuCommand _menu_item2;

        private AboutCommand(Microsoft.VisualStudio.Shell.Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            _package = package;
            OleMenuCommandService commandService = this.ServiceProvider.GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService == null)
            {
                throw new ArgumentNullException("OleMenuCommandService");
            }

            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7010);
                _menu_item1 = new MenuCommand(this.MenuItemCallback, menuCommandID);
                _menu_item1.Enabled = true;
                _menu_item1.Visible = true;
                commandService.AddCommand(_menu_item1);
            }
            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidMenuAndCommandsCmdSet), 0x7010);
                _menu_item2 = new MenuCommand(this.MenuItemCallback, menuCommandID);
                _menu_item2.Enabled = true;
                _menu_item2.Visible = true;
                commandService.AddCommand(_menu_item2);
            }
        }
        public static AboutCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return this._package; }
        }

        public static void Initialize(Microsoft.VisualStudio.Shell.Package package)
        {
            Instance = new AboutCommand(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                AboutBox inputDialog = new AboutBox();
                inputDialog.ShowDialog();
            });
        }
    }
}
