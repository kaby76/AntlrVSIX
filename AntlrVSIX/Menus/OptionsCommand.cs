namespace AntlrVSIX.Options
{
    using Basics;
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

        public int OptInLogging
        {
            get
            {
                var v = XOptions.GetProperty("OptInLogging");
                if (v == "1") return 1;
                else if (v == "2") return 2;
                else return 0;
            }
            set
            {
                if (value == 1)
                    XOptions.SetProperty("OptInLogging", "1");
                else if (value == 2)
                    XOptions.SetProperty("OptInLogging", "2");
                else
                    XOptions.SetProperty("OptInLogging", "0");
            }
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
            OptInLogging = 0;
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

                if (! POptions.EverInitialized())
                {
                    POptions.SetBoolean("RestrictedDirectory", RestrictedDirectory);
                    POptions.SetBoolean("NonInteractiveParse", NonInteractiveParse);
                    POptions.SetBoolean("GenerateVisitorListener", GenerateVisitorListener);
                    POptions.SetString("CorpusLocation", CorpusLocation);
                    POptions.SetBoolean("IncrementalReformat", IncrementalReformat);
                    POptions.SetBoolean("OverrideAntlrPluggins", OverrideAntlrPluggins);
                    POptions.SetBoolean("OverrideJavaPluggins", OverrideJavaPluggins);
                    POptions.SetBoolean("OverridePythonPluggins", OverridePythonPluggins);
                    POptions.SetBoolean("OverrideRustPluggins", OverrideRustPluggins);
                    POptions.SetInt32("OptInLogging", OptInLogging);
                }

                RestrictedDirectory = POptions.GetBoolean("RestrictedDirectory", RestrictedDirectory);
                NonInteractiveParse = POptions.GetBoolean("NonInteractiveParse", NonInteractiveParse);
                GenerateVisitorListener = POptions.GetBoolean("GenerateVisitorListener", GenerateVisitorListener);
                CorpusLocation = POptions.GetString("CorpusLocation", CorpusLocation);
                IncrementalReformat = POptions.GetBoolean("IncrementalReformat", IncrementalReformat);
                OverrideAntlrPluggins = POptions.GetBoolean("OverrideAntlrPluggins", OverrideAntlrPluggins);
                OverrideJavaPluggins = POptions.GetBoolean("OverrideJavaPluggins", OverrideJavaPluggins);
                OverridePythonPluggins = POptions.GetBoolean("OverridePythonPluggins", OverridePythonPluggins);
                OverrideRustPluggins = POptions.GetBoolean("OverrideRustPluggins", OverrideRustPluggins);
                OptInLogging = POptions.GetInt32("OptInLogging", OptInLogging);

                OptionsBox inputDialog = new OptionsBox(this);

                inputDialog.restricted_directory.IsChecked = RestrictedDirectory;
                inputDialog.noninteractive.IsChecked = NonInteractiveParse;
                inputDialog.generate_visitor_listener.IsChecked = GenerateVisitorListener;
                inputDialog.corpus_location.Text = CorpusLocation;
                inputDialog.incremental_reformat.IsChecked = IncrementalReformat;
                inputDialog.override_antlr.IsChecked = OverrideAntlrPluggins;
                inputDialog.override_java.IsChecked = OverrideJavaPluggins;
                inputDialog.override_python.IsChecked = OverridePythonPluggins;
                inputDialog.override_rust.IsChecked = OverrideRustPluggins;
                if (OptInLogging == 1) inputDialog.opt_in_reporting.IsChecked = true;
                else if (OptInLogging == 2) inputDialog.opt_in_reporting.IsChecked = false;
                else inputDialog.opt_in_reporting.IsChecked = null;
                inputDialog.Show();
            });
        }
    }
}
