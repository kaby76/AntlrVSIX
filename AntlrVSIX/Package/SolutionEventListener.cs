using AntlrVSIX.Extensions;
using AntlrVSIX.Grammar;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VSLangProj;

namespace AntlrVSIX.Package
{
    class SolutionEventListener : IVsSolutionEvents, IVsSolutionLoadEvents
    {
        private void ParseAllFiles()
        {
            // Convert the entire solution into Project/Document workspace.

            // First, open up every .g4 file in project and parse.
            DTE application = DteExtensions.GetApplication();
            if (application == null) return;

            Solution solution = (Solution)application.Solution;
            string solution_full_name = solution.FullName;
            string solution_file_name = solution.FileName;
            var ws = AntlrVSIX.GrammarDescription.Workspace.Instance;
            ws.Name = solution_full_name;
            ws.FFN = solution_file_name;

            Properties solution_properties = solution.Properties;
            foreach (Project project in solution.Projects)
            {
                string project_name = project.Name;
                string project_file_name = project.FileName;

                var ws_project = new AntlrVSIX.GrammarDescription.Project(project_name, project_file_name);
                ws.AddProject(ws_project);

                {
                    var properties = project.Properties;
                    if (properties == null) continue;
                    var count = properties.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        try
                        {
                            var prop = properties.Item(i);
                            var name = prop.Name;
                            var value = prop.Value;
                            ws_project.AddProperty(name, value == null ? null : value.ToString());
                        }
                        catch (Exception _)
                        { }
                    }

                }
                foreach (ProjectItem item in project.ProjectItems)
                {
                    string file_name = item.Name;
                    if (file_name == null) return;
                    var doc = new AntlrVSIX.GrammarDescription.Document(file_name);
                    ws_project.AddDocument(doc);
                    var properties = item.Properties;
                    if (properties == null) continue;
                    var count = properties.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        try
                        {
                            var prop = properties.Item(i);
                            var name = prop.Name;
                            var value = prop.Value;
                            doc.AddProperty(name, value == null? null : value.ToString());
                        }
                        catch (Exception _)
                        { }
                    }
                }
                Properties project_properties = project.Properties;
            }

            foreach (var project in ws.Projects)
            {
                foreach (var document in project.Documents)
                {
                    var ffn = document.GetProperty("FullPath");
                    if (ffn != null) document.FullPath = ffn;
                }
            }

            // Parse.

            foreach (var project in ws.Projects)
            {
                foreach (var document in project.Documents)
                {
                    string file_name = document.FullPath;
                    if (file_name == null) continue;
                    var gd = GrammarDescriptionFactory.Create(file_name);
                    if (gd == null) continue;
                    if (!System.IO.File.Exists(file_name)) continue;


                    gd.Parse(file_name, document.Code,
                        out IParseTree parse_tree,
                        out Dictionary<IParseTree, Symtab.CombinedScopeSymbol> st);
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
