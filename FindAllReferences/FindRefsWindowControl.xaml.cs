namespace AntlrVSIX
{
    using System.Windows.Controls;

    public partial class FindRefsWindowControl : UserControl
    {
        public FindRefsWindowControl()
        {
            this.InitializeComponent();
            DataContext = FindAntlrSymbolsModel.Instance;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb == null) return;
            object item = lb.SelectedItem;
            Entry entry = item as Entry;
            if (entry == null) return;
            FindAntlrSymbolsModel.Instance.ItemSelected = entry;
        }
    }
}