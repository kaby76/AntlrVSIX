namespace AntlrVSIX.Options
{
    using System;
    using System.Windows;

    public partial class OptionsBox : Window
    {
        OptionsCommand _o;
        Microsoft.VisualStudio.Settings.WritableSettingsStore _s;

        public OptionsBox(OptionsCommand o, Microsoft.VisualStudio.Settings.WritableSettingsStore s)
        {
            InitializeComponent();
            _o = o;
            _s = s;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = this;
            _o.RestrictedDirectory = inputDialog.restricted_directory.IsChecked ?? false;
            _o.NonInteractiveParse = inputDialog.noninteractive.IsChecked ?? false;
            _o.GenerateVisitorListener = inputDialog.generate_visitor_listener.IsChecked ?? false;
            _o.CorpusLocation = inputDialog.corpus_location.Text;
            _o.IncrementalReformat = inputDialog.incremental_reformat.IsChecked ?? false;
            _o.OverrideAntlrPluggins = inputDialog.override_antlr.IsChecked ?? false;
            _o.OverrideJavaPluggins = inputDialog.override_java.IsChecked ?? false;
            _o.OverridePythonPluggins = inputDialog.override_python.IsChecked ?? false;
            _o.OverrideRustPluggins = inputDialog.override_rust.IsChecked ?? false;
            _o.OptInLogging = inputDialog.opt_in_reporting.IsChecked ?? false;

            _s.SetBoolean("AntlrVSIX", "RestrictedDirectory", _o.RestrictedDirectory);
            _s.SetBoolean("AntlrVSIX", "NonInteractiveParse", _o.NonInteractiveParse);
            _s.SetBoolean("AntlrVSIX", "GenerateVisitorListener", _o.GenerateVisitorListener);
            _s.SetString("AntlrVSIX", "CorpusLocation", _o.CorpusLocation);
            _s.SetBoolean("AntlrVSIX", "IncrementalReformat", _o.IncrementalReformat);
            _s.SetBoolean("AntlrVSIX", "OverrideAntlrPluggins", _o.OverrideAntlrPluggins);
            _s.SetBoolean("AntlrVSIX", "OverrideJavaPluggins", _o.OverrideJavaPluggins);
            _s.SetBoolean("AntlrVSIX", "OverridePythonPluggins", _o.OverridePythonPluggins);
            _s.SetBoolean("AntlrVSIX", "OverrideRustPluggins", _o.OverrideRustPluggins);
            _s.SetBoolean("AntlrVSIX", "OptInLogging", _o.OptInLogging);
            this.Close();
        }

        private void btnDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
