namespace AntlrVSIX.Options
{
    using System.Windows;
    using global::Options;

    public partial class OptionsBox : Window
    {
        OptionsCommand _o;

        public OptionsBox()
        {
            InitializeComponent();
            this.restricted_directory.IsChecked = POptions.GetBoolean("RestrictedDirectory");
            this.noninteractive.IsChecked = POptions.GetBoolean("NonInteractiveParse");
            this.generate_visitor_listener.IsChecked = POptions.GetBoolean("GenerateVisitorListener");
            this.corpus_location.Text = POptions.GetString("CorpusLocation");
            this.incremental_reformat.IsChecked = POptions.GetBoolean("IncrementalReformat");
            this.override_antlr.IsChecked = POptions.GetBoolean("OverrideAntlrPluggins");
            this.override_java.IsChecked = POptions.GetBoolean("OverrideJavaPluggins");
            this.override_python.IsChecked = POptions.GetBoolean("OverridePythonPluggins");
            this.override_rust.IsChecked = POptions.GetBoolean("OverrideRustPluggins");
            if (POptions.GetInt32("OptInLogging") == 1) this.opt_in_reporting.IsChecked = true;
            else if (POptions.GetInt32("OptInLogging") == 2) this.opt_in_reporting.IsChecked = false;
            else this.opt_in_reporting.IsChecked = null;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = this;
            POptions.SetBoolean("RestrictedDirectory", inputDialog.restricted_directory.IsChecked ?? false);
            POptions.SetBoolean("NonInteractiveParse", inputDialog.noninteractive.IsChecked ?? false);
            POptions.SetBoolean("GenerateVisitorListener", inputDialog.generate_visitor_listener.IsChecked ?? false);
            POptions.SetString("CorpusLocation", inputDialog.corpus_location.Text);
            POptions.SetBoolean("IncrementalReformat", inputDialog.incremental_reformat.IsChecked ?? false);
            POptions.SetBoolean("OverrideAntlrPluggins", inputDialog.override_antlr.IsChecked ?? false);
            POptions.SetBoolean("OverrideJavaPluggins", inputDialog.override_java.IsChecked ?? false);
            POptions.SetBoolean("OverridePythonPluggins", inputDialog.override_python.IsChecked ?? false);
            POptions.SetBoolean("OverrideRustPluggins", inputDialog.override_rust.IsChecked ?? false);
            POptions.SetInt32("OptInLogging", inputDialog.opt_in_reporting.IsChecked switch
            {
                true => 1,
                false => 2,
                _ => 0
            });
            this.Close();
        }

        private void btnDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
