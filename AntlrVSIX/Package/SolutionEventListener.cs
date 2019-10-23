namespace AntlrVSIX.Package
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Tagger;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;

    class SolutionEventListener : IVsSolutionEvents, IVsSolutionLoadEvents
    {

        public int OnAfterOpenProject(IVsHierarchy aPHierarchy, int aFAdded)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryCloseProject(IVsHierarchy aPHierarchy, int aFRemoving, ref int aPfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseProject(IVsHierarchy aPHierarchy, int aFRemoved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProject(IVsHierarchy aPStubHierarchy, IVsHierarchy aPRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryUnloadProject(IVsHierarchy aPRealHierarchy, ref int aPfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeUnloadProject(IVsHierarchy aPRealHierarchy, IVsHierarchy aPStubHierarchy)
        {
            return VSConstants.S_OK;
        }


        public int OnAfterOpenSolution(object aPUnkReserved, int aFNewSolution)
        {
            Workspaces.Loader.LoadAsync().Wait();
            var to_do = LanguageServer.Module.Compile();
            foreach (var t in to_do)
            {
                var w = t.FullFileName;
                IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(w);
                if (vstv == null) continue;
                IWpfTextView wpftv = vstv.GetIWpfTextView();
                if (wpftv == null) continue;
                var buffer = wpftv.TextBuffer;
                var att = buffer.Properties.GetOrCreateSingletonProperty(() => new AntlrVSIX.AggregateTagger.AntlrClassifier(buffer));
                att.Raise();
            }
            return VSConstants.S_OK;
        }


        public int OnQueryCloseSolution(object aPUnkReserved, ref int aPfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseSolution(object aPUnkReserved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterCloseSolution(object aPUnkReserved)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeOpenSolution(string pszSolutionFilename)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeBackgroundSolutionLoadBegins()
        {
            return VSConstants.S_OK;
        }

        public int OnQueryBackgroundLoadProjectBatch(out bool pfShouldDelayLoadToNextIdle)
        {
            pfShouldDelayLoadToNextIdle = false;
            return VSConstants.S_OK;
        }

        public int OnBeforeLoadProjectBatch(bool fIsBackgroundIdleBatch)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProjectBatch(bool fIsBackgroundIdleBatch)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterBackgroundSolutionLoadComplete()
        {
            Workspaces.Loader.LoadAsync().Wait();
            var to_do = LanguageServer.Module.Compile();
            foreach (var t in to_do)
            {
                var w = t.FullFileName;
                IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(w);
                if (vstv == null) continue;
                IWpfTextView wpftv = vstv.GetIWpfTextView();
                if (wpftv == null) continue;
                var buffer = wpftv.TextBuffer;
                var att = buffer.Properties.GetOrCreateSingletonProperty(() => new AntlrVSIX.AggregateTagger.AntlrClassifier(buffer));
                att.Raise();
            }
            return VSConstants.S_OK;
        }
    }
}
