using System;
using System.Windows;
using AntlrVSIX.Extensions;
using EnvDTE;
using Window = System.Windows.Window;
using System.Diagnostics;

namespace AntlrVSIX.About
{
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            InitializeComponent();
            DTE application = DteExtensions.GetApplication();
            _info.Text = "Author: Ken Domino";
            _info.AppendText(Environment.NewLine);
            _info.AppendText("AntlrVSIX version: " + AntlrVSIX.Constants.Version);
            _info.AppendText(Environment.NewLine);
            _info.AppendText("Runtime: " + typeof(string).Assembly.ImageRuntimeVersion);
            _info.AppendText(Environment.NewLine);
            _info.AppendText("Running " + application?.Name + " " + application.Version);
            _info.AppendText(Environment.NewLine);
            _info.AppendText(application.FullName);
            _info.AppendText(Environment.NewLine);
            FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(application.FullName);
            _info.AppendText("File version: " + fileVersion.FileVersion);
            _info.AppendText(Environment.NewLine);
            var cla = application.CommandLineArguments;
            _info.AppendText("Command line args: " + cla);
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
