namespace LspAntlr
{
    using Options;
    using System.Windows;

    public partial class OptionsBox : Window
    {
        public OptionsBox()
        {
            InitializeComponent();
            visible_server_window.IsChecked = Option.GetBoolean("VisibleServerWindow");
            restricted_directory.IsChecked = Option.GetBoolean("RestrictedDirectory");
            generate_visitor_listener.IsChecked = Option.GetBoolean("GenerateVisitorListener");
            corpus_location.Text = Option.GetString("CorpusLocation");
            incremental_reformat.IsChecked = Option.GetBoolean("IncrementalReformat");
            override_antlr.IsChecked = Option.GetBoolean("OverrideAntlrPluggins");
            opt_in_reporting.IsChecked = Option.GetBoolean("OptInLogging");
            enable_completion.IsChecked = Option.GetBoolean("EnableCompletion");
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            OptionsBox inputDialog = this;
            Option.SetBoolean("VisibleServerWindow", inputDialog.visible_server_window.IsChecked ?? false);
            Option.SetBoolean("RestrictedDirectory", inputDialog.restricted_directory.IsChecked ?? false);
            Option.SetBoolean("GenerateVisitorListener", inputDialog.generate_visitor_listener.IsChecked ?? false);
            Option.SetString("CorpusLocation", inputDialog.corpus_location.Text);
            Option.SetBoolean("IncrementalReformat", inputDialog.incremental_reformat.IsChecked ?? false);
            Option.SetBoolean("OverrideAntlrPluggins", inputDialog.override_antlr.IsChecked ?? false);
            Option.SetBoolean("OptInLogging", inputDialog.opt_in_reporting.IsChecked ?? false);
            Option.SetBoolean("EnableCompletion", inputDialog.enable_completion.IsChecked ?? false);
            Close();
        }

        private void btnDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
