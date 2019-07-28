using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using AntlrVSIX.Extensions;
using AntlrVSIX.Grammar;
using Microsoft.VisualStudio.Shell.Interop;

namespace AntlrVSIX.File
{

    class FileChangeListener
    {
        private readonly Microsoft.VisualStudio.Shell.Package _package;
        private IVsRunningDocumentTable _runningDocumentTable;

        private FileChangeListener(Microsoft.VisualStudio.Shell.Package package)
        {
            _package = package;
            uint cookie;
            _runningDocumentTable = (IVsRunningDocumentTable)this.ServiceProvider.GetService(typeof(SVsRunningDocumentTable));
            _runningDocumentTable.AdviseRunningDocTableEvents(new FileSaveListener(), out cookie);
        }

        public static void Initialize(Microsoft.VisualStudio.Shell.Package package)
        {
            Instance = new FileChangeListener(package);
        }

        public static FileChangeListener Instance { get; private set; }

        private System.IServiceProvider ServiceProvider
        {
            get { return this._package; }
        }

        public IVsRunningDocumentTable RunningDocumentTable
        {
            get { return this._runningDocumentTable; }
        }
    }
}
