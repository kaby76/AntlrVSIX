using Nerdbank.Streams;

namespace LspAntlr
{
    using System.Windows;
    using Options;

    public partial class OptionsBox : Window
    {
        OptionsCommand _o;

        public OptionsBox()
        {
            InitializeComponent();
            this.visible_server_window.IsChecked = POptions.GetBoolean("VisibleServerWindow");
            this.restricted_directory.IsChecked = POptions.GetBoolean("RestrictedDirectory");
            this.generate_visitor_listener.IsChecked = POptions.GetBoolean("GenerateVisitorListener");
            this.corpus_location.Text = POptions.GetString("CorpusLocation");
            this.incremental_reformat.IsChecked = POptions.GetBoolean("IncrementalReformat");
            this.override_antlr.IsChecked = POptions.GetBoolean("OverrideAntlrPluggins");
            if (POptions.GetInt32("OptInLogging") == 1) this.opt_in_reporting.IsChecked = true;
            else if (POptions.GetInt32("OptInLogging") == 2) this.opt_in_reporting.IsChecked = false;
            else this.opt_in_reporting.IsChecked = null;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = this;
            POptions.SetBoolean("VisibleServerWindow", inputDialog.visible_server_window.IsChecked ?? false);
            POptions.SetBoolean("RestrictedDirectory", inputDialog.restricted_directory.IsChecked ?? false);
            POptions.SetBoolean("GenerateVisitorListener", inputDialog.generate_visitor_listener.IsChecked ?? false);
            POptions.SetString("CorpusLocation", inputDialog.corpus_location.Text);
            POptions.SetBoolean("IncrementalReformat", inputDialog.incremental_reformat.IsChecked ?? false);
            POptions.SetBoolean("OverrideAntlrPluggins", inputDialog.override_antlr.IsChecked ?? false);
            this.Close();
        }

        private void btnDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
