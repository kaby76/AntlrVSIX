namespace AntlrVSIX.Options
{
    using LanguageServer;
    using Microsoft.VisualStudio.Settings;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Settings;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;

    public class OptionsCommand
    {
        private readonly Microsoft.VisualStudio.Shell.Package _package;
        private MenuCommand _menu_item1;
        public bool IncrementalReformat
        {
            get { return XOptions.GetProperty("IncrementalReformat") == "true"; }
            set { XOptions.SetProperty("IncrementalReformat", value ? "true" : "false"); }
        }
        public bool NonInteractiveParse
        {
            get { return XOptions.GetProperty("NonInteractiveParse") == "true"; }
            set { XOptions.SetProperty("NonInteractiveParse", value ? "true" : "false"); }
        }
        public bool RestrictedDirectory
        {
            get { return XOptions.GetProperty("RestrictedDirectory") == "true"; }
            set { XOptions.SetProperty("RestrictedDirectory", value ? "true" : "false"); }
        }
        public bool GenerateVisitorListener
        {
            get { return XOptions.GetProperty("GenerateVisitorListener") == "true"; }
            set { XOptions.SetProperty("GenerateVisitorListener", value ? "true" : "false"); }
        }
        public string CorpusLocation
        {
            get { return XOptions.GetProperty("CorpusLocation"); }
            set { XOptions.SetProperty("CorpusLocation", value); }
        }
        public bool OverrideAntlrPluggins
        {
            get { return XOptions.GetProperty("OverrideAntlrPluggins") == "true"; }
            set { XOptions.SetProperty("OverrideAntlrPluggins", value ? "true" : "false"); }
        }
        public bool OverrideJavaPluggins
        {
            get { return XOptions.GetProperty("OverrideJavaPluggins") == "true"; }
            set { XOptions.SetProperty("OverrideJavaPluggins", value ? "true" : "false"); }
        }
        public bool OverridePythonPluggins
        {
            get { return XOptions.GetProperty("OverridePythonPluggins") == "true"; }
            set { XOptions.SetProperty("OverridePythonPluggins", value ? "true" : "false"); }
        }
        public bool OverrideRustPluggins
        {
            get { return XOptions.GetProperty("OverrideRustPluggins") == "true"; }
            set { XOptions.SetProperty("OverrideRustPluggins", value ? "true" : "false"); }
        }

        public bool OptInLogging
        {
            get { return XOptions.GetProperty("OptInLogging") == "true"; }
            set { XOptions.SetProperty("OptInLogging", value ? "true" : "false"); }
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

            IncrementalReformat = true;
            NonInteractiveParse = false;
            RestrictedDirectory = true;
            GenerateVisitorListener = false;
            OverrideAntlrPluggins = true;
            OverrideJavaPluggins = true;
            OverridePythonPluggins = true;
            OverrideRustPluggins = true;
            OptInLogging = false;
            var s = System.Environment.GetEnvironmentVariable("CORPUS_LOCATION");
            CorpusLocation = s == null ? "" : s;
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
            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {

                SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider);
                WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
                try
                {
                    IEnumerable<string> collection = userSettingsStore.GetSubCollectionNames("AntlrVSIX");
                }
                catch (Exception)
                {
                    userSettingsStore.CreateCollection("AntlrVSIX");
                    IEnumerable<string> collection = userSettingsStore.GetSubCollectionNames("AntlrVSIX");
                    {
                        userSettingsStore.SetBoolean("AntlrVSIX", "RestrictedDirectory", RestrictedDirectory);
                        userSettingsStore.SetBoolean("AntlrVSIX", "NonInteractiveParse", NonInteractiveParse);
                        userSettingsStore.SetBoolean("AntlrVSIX", "GenerateVisitorListener", GenerateVisitorListener);
                        userSettingsStore.SetString("AntlrVSIX", "CorpusLocation", CorpusLocation);
                        userSettingsStore.SetBoolean("AntlrVSIX", "IncrementalReformat", IncrementalReformat);
                        userSettingsStore.SetBoolean("AntlrVSIX", "OverrideAntlrPluggins", OverrideAntlrPluggins);
                        userSettingsStore.SetBoolean("AntlrVSIX", "OverrideJavaPluggins", OverrideJavaPluggins);
                        userSettingsStore.SetBoolean("AntlrVSIX", "OverridePythonPluggins", OverridePythonPluggins);
                        userSettingsStore.SetBoolean("AntlrVSIX", "OverrideRustPluggins", OverrideRustPluggins);
                        userSettingsStore.SetBoolean("AntlrVSIX", "OptInLogging", OptInLogging);
                    }
                }

                RestrictedDirectory = userSettingsStore.GetBoolean("AntlrVSIX", "RestrictedDirectory", RestrictedDirectory);
                NonInteractiveParse = userSettingsStore.GetBoolean("AntlrVSIX", "NonInteractiveParse", NonInteractiveParse);
                GenerateVisitorListener = userSettingsStore.GetBoolean("AntlrVSIX", "GenerateVisitorListener", GenerateVisitorListener);
                CorpusLocation = userSettingsStore.GetString("AntlrVSIX", "CorpusLocation", CorpusLocation);
                IncrementalReformat = userSettingsStore.GetBoolean("AntlrVSIX", "IncrementalReformat", IncrementalReformat);
                OverrideAntlrPluggins = userSettingsStore.GetBoolean("AntlrVSIX", "OverrideAntlrPluggins", OverrideAntlrPluggins);
                OverrideJavaPluggins = userSettingsStore.GetBoolean("AntlrVSIX", "OverrideJavaPluggins", OverrideJavaPluggins);
                OverridePythonPluggins = userSettingsStore.GetBoolean("AntlrVSIX", "OverridePythonPluggins", OverridePythonPluggins);
                OverrideRustPluggins = userSettingsStore.GetBoolean("AntlrVSIX", "OverrideRustPluggins", OverrideRustPluggins);
                OptInLogging = userSettingsStore.GetBoolean("AntlrVSIX", "OptInLogging", OptInLogging);

                OptionsBox inputDialog = new OptionsBox(this, userSettingsStore);

                inputDialog.restricted_directory.IsChecked = RestrictedDirectory;
                inputDialog.noninteractive.IsChecked = NonInteractiveParse;
                inputDialog.generate_visitor_listener.IsChecked = GenerateVisitorListener;
                inputDialog.corpus_location.Text = CorpusLocation;
                inputDialog.incremental_reformat.IsChecked = IncrementalReformat;
                inputDialog.override_antlr.IsChecked = OverrideAntlrPluggins;
                inputDialog.override_java.IsChecked = OverrideJavaPluggins;
                inputDialog.override_python.IsChecked = OverridePythonPluggins;
                inputDialog.override_rust.IsChecked = OverrideRustPluggins;
                inputDialog.opt_in_reporting.IsChecked = OptInLogging;
                inputDialog.Show();
            });
        }
    }
}
