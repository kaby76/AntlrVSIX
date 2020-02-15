namespace LspAntlr
{
    using System.Windows;
    using Options;

    public partial class OptionsBox : Window
    {
        public OptionsBox()
        {
            InitializeComponent();
            this.visible_server_window.IsChecked = Option.GetBoolean("VisibleServerWindow");
            this.restricted_directory.IsChecked = Option.GetBoolean("RestrictedDirectory");
            this.generate_visitor_listener.IsChecked = Option.GetBoolean("GenerateVisitorListener");
            this.corpus_location.Text = Option.GetString("CorpusLocation");
            this.incremental_reformat.IsChecked = Option.GetBoolean("IncrementalReformat");
            this.override_antlr.IsChecked = Option.GetBoolean("OverrideAntlrPluggins");
            this.opt_in_reporting.IsChecked = Option.GetBoolean("OptInLogging");
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = this;
            Option.SetBoolean("VisibleServerWindow", inputDialog.visible_server_window.IsChecked ?? false);
            Option.SetBoolean("RestrictedDirectory", inputDialog.restricted_directory.IsChecked ?? false);
            Option.SetBoolean("GenerateVisitorListener", inputDialog.generate_visitor_listener.IsChecked ?? false);
            Option.SetString("CorpusLocation", inputDialog.corpus_location.Text);
            Option.SetBoolean("IncrementalReformat", inputDialog.incremental_reformat.IsChecked ?? false);
            Option.SetBoolean("OverrideAntlrPluggins", inputDialog.override_antlr.IsChecked ?? false);
            Option.SetBoolean("OptInLogging", inputDialog.opt_in_reporting.IsChecked ?? false);
            this.Close();
        }

        private void btnDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
