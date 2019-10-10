namespace AntlrVSIX.Options
{
    using System;
    using System.Windows;

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
