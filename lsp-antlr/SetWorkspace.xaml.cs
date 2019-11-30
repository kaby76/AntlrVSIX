using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using Path = System.IO.Path;

namespace VSIXProject1
{
    public partial class SetWorkspace : Window
    {
        public string _java_home = null;
        public string _java_executable = null;

        public SetWorkspace()
        {
            InitializeComponent();
            string javaHome = System.Environment.GetEnvironmentVariable("JAVA_HOME");
            this.java_home_path.Text = javaHome == null || javaHome == "" ? "not set" : javaHome;
            this.eclipse_jdt.Text = "jdt-language-server-0.47.0-201911150945.tar.gz";
        }

        private void btnOK_click(object sender, RoutedEventArgs e)
        {
            string javaHome = this.java_home_path.Text;
            if (!Directory.Exists(javaHome))
            {
                string message = "Cannot find Java home, currently set to "
                                 + "'" + javaHome + "'"
                                 + " Please set either the JAVA_HOME environment variable, "
                                 + "or set a property for JAVA_HOME in your CSPROJ file.";
                string caption = "Error Detected in Java Home";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = System.Windows.Forms.MessageBox.Show(message, caption, buttons);
                return;
            }

            // Find Java.
            string java_executable = null;
            string java_name = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                java_name = "java";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                java_name = "java.exe";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                java_name = "java";
            else
            {
                string message = "Java home is invalid. Set java home to java installation directory, which contains bin/.";
                string caption = "Error Detected in Java Home";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = System.Windows.Forms.MessageBox.Show(message, caption, buttons);
                return;
            }
            java_executable = Path.Combine(Path.Combine(javaHome, "bin"), java_name);
            if (!File.Exists(java_executable))
            {
                string message = "Java home is invalid. Set java home to java installation directory, which contains bin/.";
                string caption = "Error Detected in Java Home";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = System.Windows.Forms.MessageBox.Show(message, caption, buttons);
                return;
            }

            _java_home = javaHome;
            _java_executable = java_executable;

            this.DialogResult = true;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
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
    }
}
