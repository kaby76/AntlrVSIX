using System;
using System.Windows;
using System.Windows.Controls;

namespace AntlrVSIX.Options
{
    public partial class OptionsBox : Window
    {
        public OptionsBox()
        {
            InitializeComponent();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
        }

    }
}
