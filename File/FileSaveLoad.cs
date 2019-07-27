using System;
using System.Collections.Generic;
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
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace AntlrVSIX.File
{
    internal class RunningDocTableEventsHandler : IVsRunningDocTableEvents3
    {
        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining,
            uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining,
            uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
            uint flags, readlocks, editlocks;
            string name; IVsHierarchy hier;
            uint itemid; IntPtr docData;
            FileSaveLoad.Instance.RunningDocumentTable.GetDocumentInfo(docCookie, out flags, out readlocks, out editlocks, out name, out hier, out itemid, out docData);

            string ffn = name;
            if (Path.GetExtension(ffn).ToLower() != ".g4") return VSConstants.S_OK;

            StreamReader sr = new StreamReader(ffn);
            ParserDetails foo = new ParserDetails();
            ParserDetails._per_file_parser_details[ffn] = foo;
            foo.Parse(sr.ReadToEnd(), ffn);
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChangeEx(uint docCookie, uint grfAttribs, IVsHierarchy pHierOld, uint itemidOld,
            string pszMkDocumentOld, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeSave(uint docCookie)
        {
            /////// MY CODE ////////
            return VSConstants.S_OK;
        }
    }

    class FileSaveLoad
    {
        private readonly Microsoft.VisualStudio.Shell.Package _package;
        private IVsRunningDocumentTable _runningDocumentTable;

        private FileSaveLoad(Microsoft.VisualStudio.Shell.Package package)
        {
            _package = package;
            uint cookie;
            _runningDocumentTable = (IVsRunningDocumentTable)this.ServiceProvider.GetService(typeof(SVsRunningDocumentTable));
            _runningDocumentTable.AdviseRunningDocTableEvents(new RunningDocTableEventsHandler(), out cookie);
        }

        public static void Initialize(Microsoft.VisualStudio.Shell.Package package)
        {
            Instance = new FileSaveLoad(package);
        }

        public static FileSaveLoad Instance { get; private set; }

        private System.IServiceProvider ServiceProvider
        {
            get { return this._package; }
        }

        public IVsRunningDocumentTable RunningDocumentTable
        {
            get { return this._runningDocumentTable; }
        }
    }


    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("code")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    public class xxxxx : IVsTextViewCreationListener
    {
        [Import]
        public IVsEditorAdaptersFactoryService AdaptersFactory = null;

        [Import]
        public ITextDocumentFactoryService TextDocumentFactoryService { get; set; }

        [Import]
        public SVsServiceProvider GlobalServiceProvider = null;
        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            var wpftv = AdaptersFactory.GetWpfTextView(textViewAdapter);
            if (wpftv == null)
            {
                Debug.Fail("Unable to get IWpfTextView from text view adapter");
                return;
            }
            IVsTextView vstv = AdaptersFactory.GetViewAdapter(wpftv);
            ITextBuffer doc = wpftv.TextBuffer;
            string ffn = doc.GetFilePath();
            if (Path.GetExtension(ffn).ToLower() != ".g4") return;

            if (!ParserDetails._per_file_parser_details.ContainsKey(ffn))
            {
                StreamReader sr = new StreamReader(ffn);
                ParserDetails foo = new ParserDetails();
                ParserDetails._per_file_parser_details[ffn] = foo;
                foo.Parse(sr.ReadToEnd(), ffn);
            }
        }
    }
}
