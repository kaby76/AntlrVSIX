using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntlrVSIX.About;
using AntlrVSIX.Extensions;
using AntlrVSIX.FindAllReferences;
using AntlrVSIX.GoToDefinition;
using AntlrVSIX.GoToVisitor;
using AntlrVSIX.Grammar;
using AntlrVSIX.NextSym;
using AntlrVSIX.Options;
using AntlrVSIX.Reformat;
using AntlrVSIX.Rename;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace AntlrVSIX.Package
{
    class SolutionEventListener : IVsSolutionEvents, IVsSolutionLoadEvents
    {
        private void ParseAllFiles()
        {
            // First, open up every .g4 file in project and parse.
            DTE application = DteExtensions.GetApplication();
            if (application == null) return;

            IEnumerable<ProjectItem> iterator = DteExtensions.SolutionFiles(application);
            ProjectItem[] list = iterator.ToArray();
            foreach (var item in list)
            {
                string file_name = item.Name;
                if (file_name != null)
                {
                    var gd = GrammarDescriptionFactory.Create(file_name);
                    if (gd == null) continue;
                    try
                    {
                        if (item.Properties == null) continue;
                        object prop = item.Properties.Item("FullPath").Value;
                        string ffn = (string)prop;
                        if (!ParserDetails._per_file_parser_details.ContainsKey(ffn))
                        {
                            StreamReader sr = new StreamReader(ffn);
                            ParserDetails.Parse(sr.ReadToEnd(), ffn);
                        }
                    }
                    catch (Exception eeks)
                    {
                    }
                }
            }
        }


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
            ParseAllFiles();
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
            ParseAllFiles();
            return VSConstants.S_OK;
        }
    }
}
