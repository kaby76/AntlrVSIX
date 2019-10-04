using Antlr4.Runtime.Tree;
using AntlrVSIX.Extensions;
using AntlrVSIX.Grammar;
using AntlrVSIX.GrammarDescription;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using Project = EnvDTE.Project;

namespace AntlrVSIX.Package
{
    class SolutionEventListener : IVsSolutionEvents, IVsSolutionLoadEvents
    {
        private static bool finished = false;
        private static bool started = false;

        public static void Load()
        {
            if (!started)
            {
                started = true;

                // Convert the entire solution into Project/Document workspace.

                // First, open up every .g4 file in project and parse.
                DTE application = DteExtensions.GetApplication();
                if (application == null)
                {
                    started = false;
                    return;
                }

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

                    var ws_project = new AntlrVSIX.GrammarDescription.Project(project_name, project_file_name, lazy_eval, project.Properties);
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
                                object value = null;
                                //if (name == "FullPath")
                                //{
                                //    value = prop.Value;
                                //    doc.AddProperty(name, value == null ? null : value.ToString());
                                //}
                                //else
                                {
                                    ws_project.AddProperty(name);
                                }
                            }
                            catch (Exception)
                            { }
                        }

                    }
                    foreach (ProjectItem item in project.ProjectItems)
                    {
                        string file_name = item.Name;
                        if (file_name == null) return;
                        var properties = item.Properties;
                        if (properties == null) continue;
                        var doc = new AntlrVSIX.GrammarDescription.Document(file_name, lazy_eval, properties);
                        ws_project.AddDocument(doc);
                        if (AntlrVSIX.Grammar.GrammarDescriptionFactory.Create(file_name) == null)
                            continue;
                        var count = properties.Count;
                        for (int i = 0; i < count; ++i)
                        {
                            try
                            {
                                var prop = properties.Item(i);
                                var name = prop.Name;
                                object value = null;
                                //if (name == "FullPath")
                                //{
                                //    value = prop.Value;
                                //    doc.AddProperty(name, value == null ? null : value.ToString());
                                //}
                                //else
                                {
                                    doc.AddProperty(name);
                                }
                            }
                            catch (Exception _)
                            { }
                        }
                    }
                }

                foreach (var project in ws.Projects)
                {
                    foreach (var document in project.Documents)
                    {
                        var ffn = document.GetProperty("FullPath");
                        if (ffn != null) document.FullPath = ffn;
                    }
                }

                foreach (var project in ws.Projects)
                {
                    foreach (var document in project.Documents)
                    {
                        string file_name = document.FullPath;
                        if (file_name == null) continue;
                        var gd = GrammarDescriptionFactory.Create(file_name);
                        if (gd == null) continue;
                        if (!System.IO.File.Exists(file_name)) continue;
                        var item = AntlrVSIX.GrammarDescription.Workspace.Instance.FindProjectFullName(file_name);
                        var pd = ParserDetailsFactory.Create(item);
                        pd.Parse(document);
                    }
                }

                foreach (var project in ws.Projects)
                {
                    foreach (var document in project.Documents)
                    {
                        string file_name = document.FullPath;
                        if (file_name == null) continue;
                        var gd = GrammarDescriptionFactory.Create(file_name);
                        if (gd == null) continue;
                        if (!System.IO.File.Exists(file_name)) continue;
                        var item = AntlrVSIX.GrammarDescription.Workspace.Instance.FindProjectFullName(file_name);
                        var pd = ParserDetailsFactory.Create(item);
                        pd.Pass1(pd);
                    }
                }

                foreach (var project in ws.Projects)
                {
                    foreach (var document in project.Documents)
                    {
                        string file_name = document.FullPath;
                        if (file_name == null) continue;
                        var gd = GrammarDescriptionFactory.Create(file_name);
                        if (gd == null) continue;
                        if (!System.IO.File.Exists(file_name)) continue;
                        var item = AntlrVSIX.GrammarDescription.Workspace.Instance.FindProjectFullName(file_name);
                        var pd = ParserDetailsFactory.Create(item);
                        pd.Pass2(pd);
                    }
                }

                foreach (var project in ws.Projects)
                {
                    foreach (var document in project.Documents)
                    {
                        string file_name = document.FullPath;
                        if (file_name == null) continue;
                        var gd = GrammarDescriptionFactory.Create(file_name);
                        if (gd == null) continue;
                        if (!System.IO.File.Exists(file_name)) continue;
                        var item = AntlrVSIX.GrammarDescription.Workspace.Instance.FindProjectFullName(file_name);
                        var pd = ParserDetailsFactory.Create(item);
                        pd.GatherDefs(document);
                    }
                }

                foreach (var project in ws.Projects)
                {
                    foreach (var document in project.Documents)
                    {
                        string file_name = document.FullPath;
                        if (file_name == null) continue;
                        var gd = GrammarDescriptionFactory.Create(file_name);
                        if (gd == null) continue;
                        if (!System.IO.File.Exists(file_name)) continue;
                        var item = AntlrVSIX.GrammarDescription.Workspace.Instance.FindProjectFullName(file_name);
                        var pd = ParserDetailsFactory.Create(item);
                        pd.GatherRefs(document);
                    }
                }
                finished = true;
            }
        }

        static string lazy_eval(string name, object props)
        {
            string result = null;
            try
            {
                var properties = (Properties)props;
                var prop = properties.Item(name);
                object value = null;
                value = prop.Value;
                result = value == null ? null : value.ToString();
            }
            catch (Exception)
            { }
            return result;
        }

        private void ParseAllFiles()
        {
            Load();
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
