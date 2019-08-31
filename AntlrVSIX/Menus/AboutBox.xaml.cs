using System;
using System.Windows;
using AntlrVSIX.Extensions;
using EnvDTE;
using Window = System.Windows.Window;
using System.Diagnostics;
using AntlrVSIX.Package;
using AntlrVSIX.File;

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
            var grammar_view = AntlrLanguagePackage.Instance.GetActiveView();
            if (grammar_view != null)
            {
                var buf = grammar_view.TextBuffer;
                var active = buf.GetFilePath();
                var content_type = buf.ContentType;
                _info.AppendText("Active view: " + active);
                _info.AppendText(Environment.NewLine);
                _info.AppendText("Content type: " + content_type.DisplayName);
                _info.AppendText(Environment.NewLine);
                if (ViewCreationListener.PreviousContentType.ContainsKey(active))
                {
                    _info.AppendText("Old content type: " + ViewCreationListener.PreviousContentType[active].DisplayName);
                    _info.AppendText(Environment.NewLine);
                }
            }
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
