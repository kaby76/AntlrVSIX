using System;
using System.IO;
using AntlrVSIX.Grammar;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace AntlrVSIX.File
{
    internal class FileSaveListener : IVsRunningDocTableEvents3
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
            FileChangeListener.Instance.RunningDocumentTable.GetDocumentInfo(docCookie, out flags, out readlocks, out editlocks, out name, out hier, out itemid, out docData);

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
}
