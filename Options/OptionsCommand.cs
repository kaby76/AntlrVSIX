using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualStudio.Shell;

namespace AntlrVSIX.Options
{
    public class OptionsCommand
    {
        private readonly Microsoft.VisualStudio.Shell.Package _package;
        private MenuCommand _menu_item1;
        public bool InteractiveParse = false;
        public bool RestrictedDirectory = false;

        private OptionsCommand(Microsoft.VisualStudio.Shell.Package package)
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
                var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidMenuAndCommandsCmdSet), 0x7007);
                _menu_item1 = new MenuCommand(this.MenuItemCallback, menuCommandID);
                _menu_item1.Enabled = true;
                _menu_item1.Visible = true;
                commandService.AddCommand(_menu_item1);
            }
        }
        public static OptionsCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return this._package; }
        }

        public static void Initialize(Microsoft.VisualStudio.Shell.Package package)
        {
            Instance = new OptionsCommand(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {

                OptionsBox inputDialog = new OptionsBox();
                if (inputDialog.ShowDialog() == true)
                {
                    RestrictedDirectory = inputDialog.restricted_directory.IsChecked ?? false;
                    InteractiveParse = inputDialog.interactive.IsChecked ?? false;
                }
            });
        }
    }
}
