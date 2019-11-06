namespace AntlrVSIX.Options
{
    using System.Windows;
    using Basics;

    public partial class OptionsBox : Window
    {
        OptionsCommand _o;

        public OptionsBox(OptionsCommand o)
        {
            InitializeComponent();
            _o = o;
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
            _o.OptInLogging = inputDialog.opt_in_reporting.IsChecked switch
                {
                    true => 1,
                    false => 2,
                    _ => 0
                };

            POptions.SetBoolean("RestrictedDirectory", _o.RestrictedDirectory);
            POptions.SetBoolean("NonInteractiveParse", _o.NonInteractiveParse);
            POptions.SetBoolean("GenerateVisitorListener", _o.GenerateVisitorListener);
            POptions.SetString("CorpusLocation", _o.CorpusLocation);
            POptions.SetBoolean("IncrementalReformat", _o.IncrementalReformat);
            POptions.SetBoolean("OverrideAntlrPluggins", _o.OverrideAntlrPluggins);
            POptions.SetBoolean("OverrideJavaPluggins", _o.OverrideJavaPluggins);
            POptions.SetBoolean("OverridePythonPluggins", _o.OverridePythonPluggins);
            POptions.SetBoolean("OverrideRustPluggins", _o.OverrideRustPluggins);
            POptions.SetInt32("OptInLogging", _o.OptInLogging);
            this.Close();
        }

        private void btnDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
