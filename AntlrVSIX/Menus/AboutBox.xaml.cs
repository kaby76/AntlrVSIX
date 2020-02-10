using LspAntlr;

namespace AntlrVSIX.About
{
    using AntlrVSIX.Package;
    using System;
    using System.Diagnostics;
    using Window = System.Windows.Window;

    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            InitializeComponent();
            _info.Text = "Author: Ken Domino";
            _info.AppendText(Environment.NewLine);
            _info.AppendText("AntlrVSIX version: " + AntlrVSIX.Constants.Version);
            _info.AppendText(Environment.NewLine);
            _info.AppendText("NET Runtime: " + typeof(string).Assembly.ImageRuntimeVersion);
            _info.AppendText(Environment.NewLine);
            _info.AppendText("Running " + GetVSVersion.GetVisualStudioVersion());
            _info.AppendText(Environment.NewLine);
        }
    }
}
