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
            {
                var c = Option.GetColor("AntlrNonterminal");
                nonterminal_color.SelectedColor = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
            }
            {
                var c = Option.GetColor("AntlrTerminal");
                terminal_color.SelectedColor = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
            }
            {
                var c = Option.GetColor("AntlrLiteral");
                literal_color.SelectedColor = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
            }
            {
                var c = Option.GetColor("AntlrMode");
                mode_color.SelectedColor = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
            }
            {
                var c = Option.GetColor("AntlrChannel");
                channel_color.SelectedColor = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
            }
        }

        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Drawing.Color> e)
        {
            // "#" + ClrPcker_Background.SelectedColor.R.ToString() + ClrPcker_Background.SelectedColor.G.ToString() + ClrPcker_Background.SelectedColor.B.ToString();
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
            {
                var c = System.Drawing.Color.FromArgb(
                    nonterminal_color.SelectedColor.Value.A,
                    nonterminal_color.SelectedColor.Value.R,
                    nonterminal_color.SelectedColor.Value.G,
                    nonterminal_color.SelectedColor.Value.B);
                Option.SetColor("AntlrNonterminal", c);
            }
            {
                var c = System.Drawing.Color.FromArgb(
                   terminal_color.SelectedColor.Value.A,
                   terminal_color.SelectedColor.Value.R,
                   terminal_color.SelectedColor.Value.G,
                   terminal_color.SelectedColor.Value.B);
                Option.SetColor("AntlrTerminal", c);
            }
            {
                var c = System.Drawing.Color.FromArgb(
                    literal_color.SelectedColor.Value.A,
                    literal_color.SelectedColor.Value.R,
                    literal_color.SelectedColor.Value.G,
                    literal_color.SelectedColor.Value.B);
                Option.SetColor("AntlrLiteral", c);
            }
            {
                var c = System.Drawing.Color.FromArgb(
                    mode_color.SelectedColor.Value.A,
                    mode_color.SelectedColor.Value.R,
                    mode_color.SelectedColor.Value.G,
                    mode_color.SelectedColor.Value.B);
                Option.SetColor("AntlrMode", c);
            }
            {
                var c = System.Drawing.Color.FromArgb(
                    channel_color.SelectedColor.Value.A,
                    channel_color.SelectedColor.Value.R,
                    channel_color.SelectedColor.Value.G,
                    channel_color.SelectedColor.Value.B);
                Option.SetColor("AntlrChannel", c);
            }
            Close();
        }

        private void btnDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
