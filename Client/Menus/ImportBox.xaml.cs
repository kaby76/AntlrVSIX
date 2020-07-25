namespace LspAntlr
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Window = System.Windows.Window;

    public partial class ImportBox : Window
    {
        private readonly List<StringValue> list = new List<StringValue>();

        public List<StringValue> List => list;

        public ImportBox()
        {
            InitializeComponent();
            Files.ItemsSource = list;
        }

        private void BtnDialogFind_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string[] files = null;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Input Files|*.y;*.g3;*.g|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.Multiselect = true;
                openFileDialog.RestoreDirectory = true;
                DialogResult result = openFileDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    //Get the path of specified file
                    files = openFileDialog.FileNames;
                }
            }
            List.Clear();
            if (files != null)
            {
                foreach (string r in files)
                {
                    List.Add(new StringValue(r));
                }
            }
            //Files.ItemsSource = list;
            Files.Items.Refresh();
        }

        private void BtnDialogOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Import files.
            Close();
        }

        private void BtnDialogCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}
