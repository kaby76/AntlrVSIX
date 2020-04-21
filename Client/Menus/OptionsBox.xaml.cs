namespace LspAntlr
{
    using Options;
    using System.Windows;
    using System.Linq;
    using System.Collections.Generic;
    using LanguageServer;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.LanguageServer.Client;
    using Microsoft.VisualStudio.LanguageServer.Protocol;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Threading;
    using Microsoft.VisualStudio.Utilities;
    using StreamJsonRpc;
    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using Color = System.Drawing.Color;
    using Task = System.Threading.Tasks.Task;
    using System.Windows.Controls;
    using System.Windows.Media;

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

            // Get list of all equivalence classes from VS.
            IComponentModel componentModel = Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;
            var service = componentModel.GetService<Microsoft.VisualStudio.Text.Classification.IClassificationFormatMapService>();
            var classificationFormatMap = service.GetClassificationFormatMap(category: "text");
            var list_of_formats = classificationFormatMap.CurrentPriorityOrder;
            var list_of_names = new List<string>();
            foreach (var p in list_of_formats)
            {
                if (p == null) continue;
                var name = p.Classification;
                list_of_names.Add(name);
            }

            {
                var c = Option.GetString("AntlrNonterminalDef");
                foreach (var n in list_of_names) nonterminal_def_color.Items.Add(n);
                nonterminal_def_color.SelectedValue = c;
                c1.SelectedColor = GetColor(c);
            }
            {
                var c = Option.GetString("AntlrNonterminalRef");
                foreach (var n in list_of_names) nonterminal_ref_color.Items.Add(n);
                nonterminal_ref_color.SelectedValue = c;
                c2.SelectedColor = GetColor(c);
            }
            {
                var c = Option.GetString("AntlrTerminalDef");
                foreach (var n in list_of_names) terminal_def_color.Items.Add(n);
                terminal_def_color.SelectedValue = c;
                c3.SelectedColor = GetColor(c);
            }
            {
                var c = Option.GetString("AntlrTerminalRef");
                foreach (var n in list_of_names) terminal_ref_color.Items.Add(n);
                terminal_ref_color.SelectedValue = c;
                c4.SelectedColor = GetColor(c);
            }
            {
                var c = Option.GetString("AntlrComment");
                foreach (var n in list_of_names) comment_color.Items.Add(n);
                comment_color.SelectedValue = c;
                c5.SelectedColor = GetColor(c);
            }
            {
                var c = Option.GetString("AntlrKeyword");
                foreach (var n in list_of_names) keyword_color.Items.Add(n);
                keyword_color.SelectedValue = c;
                c6.SelectedColor = GetColor(c);
            }
            {
                var c = Option.GetString("AntlrLiteral");
                foreach (var n in list_of_names) literal_color.Items.Add(n);
                literal_color.SelectedValue = c;
                c7.SelectedColor = GetColor(c);
            }
            {
                var c = Option.GetString("AntlrModeDef");
                foreach (var n in list_of_names) mode_def_color.Items.Add(n);
                mode_def_color.SelectedValue = c;
                c8.SelectedColor = GetColor(c);
            }
            {
                var c = Option.GetString("AntlrModeRef");
                foreach (var n in list_of_names) mode_ref_color.Items.Add(n);
                mode_ref_color.SelectedValue = c;
                c9.SelectedColor = GetColor(c);
            }
            {
                var c = Option.GetString("AntlrChannelDef");
                foreach (var n in list_of_names) channel_def_color.Items.Add(n);
                mode_ref_color.SelectedValue = c;
                c10.SelectedColor = GetColor(c);
            }
            {
                var c = Option.GetString("AntlrChannelRef");
                foreach (var n in list_of_names) channel_ref_color.Items.Add(n);
                channel_ref_color.SelectedValue = c;
                c11.SelectedColor = GetColor(c);
            }
            {
                var c = Option.GetString("AntlrPunctuation");
                foreach (var n in list_of_names) punctuation_color.Items.Add(n);
                punctuation_color.SelectedValue = c;
                c12.SelectedColor = GetColor(c);
            }
            {
                var c = Option.GetString("AntlrOperator");
                foreach (var n in list_of_names) operator_color.Items.Add(n);
                operator_color.SelectedValue = c;
                c13.SelectedColor = GetColor(c);
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

            Option.SetString("AntlrNonterminalDef", inputDialog.nonterminal_def_color.Text);
            Option.SetString("AntlrNonterminalRef", inputDialog.nonterminal_ref_color.Text);
            Option.SetString("AntlrTerminalDef", inputDialog.terminal_def_color.Text);
            Option.SetString("AntlrTerminalRef", inputDialog.terminal_ref_color.Text);
            Option.SetString("AntlrComment", inputDialog.comment_color.Text);
            Option.SetString("AntlrKeyword", inputDialog.keyword_color.Text);
            Option.SetString("AntlrLiteral", inputDialog.literal_color.Text);
            Option.SetString("AntlrTerminalRef", inputDialog.terminal_ref_color.Text);
            Option.SetString("AntlrModeDef", inputDialog.mode_def_color.Text);
            Option.SetString("AntlrModeRef", inputDialog.mode_ref_color.Text);
            Option.SetString("AntlrChannelDef", inputDialog.channel_def_color.Text);
            Option.SetString("AntlrChannelRef", inputDialog.channel_ref_color.Text);
            Option.SetString("AntlrPunctuation", inputDialog.punctuation_color.Text);
            Option.SetString("AntlrOperator", inputDialog.operator_color.Text);

            Close();
        }

        private void btnDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private System.Windows.Media.Color GetColor(string text)
        {
            IComponentModel componentModel = Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;
            var service = componentModel.GetService<Microsoft.VisualStudio.Text.Classification.IClassificationFormatMapService>();
            var classificationFormatMap = service.GetClassificationFormatMap(category: "text");
            var list_of_formats = classificationFormatMap.CurrentPriorityOrder;
            IClassificationType selected = list_of_formats.Where(t => { if (t != null) return t.Classification == text; return false; }).FirstOrDefault();
            if (selected == null) return System.Windows.Media.Color.FromArgb(0,0,0,0);
            Microsoft.VisualStudio.Text.Formatting.TextFormattingRunProperties identifierProperties = classificationFormatMap.GetExplicitTextProperties(selected);
            var brush = identifierProperties.ForegroundBrush;
            var tttt = brush.GetType();
            if (brush is SolidColorBrush)
            {
                var b = brush as SolidColorBrush;
                System.Windows.Media.Color color = b.Color;
                return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
            }
            return System.Windows.Media.Color.FromArgb(0, 0, 0, 0);
        }

        private void nonterminal_def_color_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            var color = GetColor(text);
            c1.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void nonterminal_ref_color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            var color = GetColor(text);
            c2.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void terminal_def_color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            var color = GetColor(text);
            c3.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void terminal_ref_color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            var color = GetColor(text);
            c4.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void comment_color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            var color = GetColor(text);
            c5.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void keyword_color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            var color = GetColor(text);
            c6.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void literal_color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            var color = GetColor(text);
            c7.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void mode_def_color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            var color = GetColor(text);
            c8.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void mode_ref_color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            var color = GetColor(text);
            c9.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void channel_def_color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            var color = GetColor(text);
            c10.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void channel_ref_color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            var color = GetColor(text);
            c11.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void punctuation_color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            var color = GetColor(text);
            c12.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void operator_color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            var color = GetColor(text);
            c13.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
