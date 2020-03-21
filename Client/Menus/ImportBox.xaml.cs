namespace LspAntlr
{
	using System;
    using System.Windows.Forms;
    using Window = System.Windows.Window;
	using System.Linq;
    using System.Collections.Generic;

    public partial class ImportBox : Window
	{
		public List<StringValue> list = new List<StringValue>();

		public ImportBox()
		{
			InitializeComponent();
			Files.ItemsSource = list;
		}

		public class StringValue
		{

			public StringValue(string s)
			{
				_ffn = s;
				_bison = System.IO.Path.GetFileName(_ffn);
				_antlr = System.IO.Path.GetFileName(_ffn).Replace(".y", ".g4");
			}
			//public string FFN { get { return _ffn; } set { _ffn = value; } }
			public string Bison { get { return _bison; } set { _bison = value; } }
			public string Antlr { get { return _antlr; } set { _antlr = value; } }
			public string _ffn;
			string _antlr;
			string _bison;
		}

		private void btnDialogFind_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var fileContent = string.Empty;
			string[] files = null;
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.InitialDirectory = "c:\\";
				openFileDialog.Filter = "Bison/Yacc files (*.y)|*.y|All files (*.*)|*.*";
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
			list.Clear();
			foreach (var r in files) list.Add(new StringValue(r));
			//Files.ItemsSource = list;
			Files.Items.Refresh();
		}

		private void btnDialogOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// Import files.
			Close();
		}

		private void btnDialogCancel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Close();
		}
	}
}
