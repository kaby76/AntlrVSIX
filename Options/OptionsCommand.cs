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
        public bool NonInteractiveParse
        {
            get; private set;
        }
        public bool RestrictedDirectory
        {
            get; private set;
        }
        public bool GenerateVisitorListener
        {
            get; private set;
        }
        public string CorpusLocation
        {
            get; private set;
        }

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

            NonInteractiveParse = false;
            RestrictedDirectory = false;
            GenerateVisitorListener = false;
            string path = System.Environment.GetEnvironmentVariable("CORPUS_LOCATION");
            CorpusLocation = path;
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
                inputDialog.restricted_directory.IsChecked = RestrictedDirectory;
                inputDialog.noninteractive.IsChecked = NonInteractiveParse;
                inputDialog.generate_visitor_listener.IsChecked = GenerateVisitorListener;
                inputDialog.corpus_location.Text = CorpusLocation;
                if (inputDialog.ShowDialog() == true)
                {
                    RestrictedDirectory = inputDialog.restricted_directory.IsChecked ?? false;
                    NonInteractiveParse = inputDialog.noninteractive.IsChecked ?? false;
                    GenerateVisitorListener = inputDialog.generate_visitor_listener.IsChecked ?? false;
                    CorpusLocation = inputDialog.corpus_location.Text;
                }
            });
        }
    }
}
