namespace LspAntlr
{
    using System;
    using System.Windows;

    public partial class RenameDialogBox : Window
    {
        public RenameDialogBox(string symbol)
        {
            InitializeComponent();
            txtAnswer.Text = symbol;
        }
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        public string Answer
        {
            get { return txtAnswer.Text; }
        }
    }
}
