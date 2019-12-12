using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace LspClangd
{
    public partial class SetWorkspace : Window
    {
        public string _clangd_executable = null;

        public SetWorkspace()
        {
            InitializeComponent();
            this.clangd_executable.Text = @"C:\Program Files\LLVM\bin\clangd.exe";
        }

        private void btnOK_click(object sender, RoutedEventArgs e)
        {
            string clangd_executable = this.clangd_executable.Text;
            if (!File.Exists(clangd_executable))
            {
                string message = "Cannot find Clangd executable, currently set to "
                                 + "'" + clangd_executable + "'";
                string caption = "Error Detected in Clangd executable";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = System.Windows.Forms.MessageBox.Show(message, caption, buttons);
                return;
            }
            _clangd_executable = clangd_executable;
            this.DialogResult = true;
        }

        private void btnBrowseWorkspace_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    var directory = fbd.SelectedPath;
                    workspace_path.Text = directory;
                }
            }
        }

        private void btnBrowseClang_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new OpenFileDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.FileName))
                {
                    var fn = fbd.FileName;
                    _clangd_executable = fn;
                }
            }
        }
    }
}
