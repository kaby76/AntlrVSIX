
namespace AntlrVSIX.Options
{
    using Microsoft.VisualStudio.Shell;
    using System.ComponentModel.Design;
    using System.Windows;
    using System;

    public class OptionsCommand
    {
        private readonly Microsoft.VisualStudio.Shell.Package _package;
        private MenuCommand _menu_item1;
        public bool IncrementalReformat { get; private set; }
        public bool NonInteractiveParse { get; private set; }
        public bool RestrictedDirectory { get; private set; }
        public bool GenerateVisitorListener { get; private set; }
        public string CorpusLocation { get; private set; }
        public bool OverrideAntlrPluggins { get; private set; }
        public bool OverrideJavaPluggins { get; private set; }
        public bool OverridePythonPluggins { get; private set; }
        public bool OverrideRustPluggins { get; private set; }

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

            IncrementalReformat = true;
            NonInteractiveParse = false;
            RestrictedDirectory = false;
            GenerateVisitorListener = false;
            OverrideAntlrPluggins = true;
            OverrideJavaPluggins = true;
            OverridePythonPluggins = true;
            OverrideRustPluggins = true;
            CorpusLocation = System.Environment.GetEnvironmentVariable("CORPUS_LOCATION");
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
                inputDialog.incremental_reformat.IsChecked = IncrementalReformat;
                inputDialog.override_antlr.IsChecked = OverrideAntlrPluggins;
                inputDialog.override_java.IsChecked = OverrideJavaPluggins;
                inputDialog.override_python.IsChecked = OverridePythonPluggins;
                inputDialog.override_rust.IsChecked = OverrideRustPluggins;
                if (inputDialog.ShowDialog() == true)
                {
                    RestrictedDirectory = inputDialog.restricted_directory.IsChecked ?? false;
                    NonInteractiveParse = inputDialog.noninteractive.IsChecked ?? false;
                    GenerateVisitorListener = inputDialog.generate_visitor_listener.IsChecked ?? false;
                    CorpusLocation = inputDialog.corpus_location.Text;
                    IncrementalReformat = inputDialog.incremental_reformat.IsChecked ?? false;
                    OverrideAntlrPluggins = inputDialog.override_antlr.IsChecked ?? false;
                    OverrideJavaPluggins = inputDialog.override_java.IsChecked ?? false;
                    OverridePythonPluggins = inputDialog.override_python.IsChecked ?? false;
                    OverrideRustPluggins = inputDialog.override_rust.IsChecked ?? false;
                }
            });
        }
    }
}
