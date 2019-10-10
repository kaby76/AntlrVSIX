using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Linq;
using Project = EnvDTE.Project;
using System.IO;

namespace ClassLibrary1
{
    public class Help
    {
        public static DTE GetApplication()
        {
            DTE dte = (DTE)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE));
            DTE application = null;
            if (dte != null)
                application = dte.Application;
            return application;
        }
    }

    public class Class1
    {
        private static bool finished = false;
        private static bool started = false;

        private static void ProcessHierarchy(IVsHierarchy parent, IVsHierarchy hierarchy)
        {
            // Traverse the nodes of the hierarchy from the root node
            ProcessHierarchyNodeRecursively(parent, hierarchy, VSConstants.VSITEMID_ROOT);
        }

        private static void ProcessHierarchyNodeRecursively(IVsHierarchy parent, IVsHierarchy hierarchy, uint itemId)
        {
            int result;
            IntPtr nestedHiearchyValue = IntPtr.Zero;
            uint nestedItemIdValue = 0;
            object value = null;
            uint visibleChildNode;
            Guid nestedHierarchyGuid;
            IVsHierarchy nestedHierarchy;

            // First, guess if the node is actually the root of another hierarchy (a project, for example)
            nestedHierarchyGuid = typeof(IVsHierarchy).GUID;
            result = hierarchy.GetNestedHierarchy(itemId, ref nestedHierarchyGuid, out nestedHiearchyValue, out nestedItemIdValue);

            if (result == VSConstants.S_OK && nestedHiearchyValue != IntPtr.Zero && nestedItemIdValue == VSConstants.VSITEMID_ROOT)
            {
                // Get the new hierarchy
                nestedHierarchy = System.Runtime.InteropServices.Marshal.GetObjectForIUnknown(nestedHiearchyValue) as IVsHierarchy;
                System.Runtime.InteropServices.Marshal.Release(nestedHiearchyValue);

                if (nestedHierarchy != null)
                {
                    ProcessHierarchy(hierarchy, nestedHierarchy);
                }
            }
            else // The node is not the root of another hierarchy, it is a regular node
            {
                ShowNodeName(parent, hierarchy, itemId);

                result = hierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_FirstChild, out value);

                while (result == VSConstants.S_OK && value != null)
                {
                    if (value is int && (uint)(int)value == VSConstants.VSITEMID_NIL)
                    {
                        // No more nodes
                        break;
                    }
                    else
                    {
                        visibleChildNode = Convert.ToUInt32(value);

                        // Enter in recursion
                        ProcessHierarchyNodeRecursively(parent, hierarchy, visibleChildNode);

                        // Get the next visible sibling node
                        value = null;
                        result = hierarchy.GetProperty(visibleChildNode, (int)__VSHPROPID.VSHPROPID_NextVisibleSibling, out value);
                    }
                }
            }
        }
        static string GetProperDirectoryCapitalization(DirectoryInfo dirInfo)
        {
            DirectoryInfo parentDirInfo = dirInfo.Parent;
            if (null == parentDirInfo)
                return dirInfo.Name.ToUpper();
            return Path.Combine(GetProperDirectoryCapitalization(parentDirInfo),
                                parentDirInfo.GetDirectories(dirInfo.Name)[0].Name);
        }

        static string GetProperFilePathCapitalization(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            DirectoryInfo dirInfo = fileInfo.Directory;
            return Path.Combine(GetProperDirectoryCapitalization(dirInfo),
                                dirInfo.GetFiles(fileInfo.Name)[0].Name);
        }
        private static void ShowNodeName(IVsHierarchy parent, IVsHierarchy hierarchy, uint itemId)
        {
            int result;
            object value = null;
            string n1 = "";
            string c1 = "";
            string n2 = "";
            string c2 = "";
            string n3 = "";
            string c3 = "";

            IVsSolution t1 = hierarchy as IVsSolution;
            IVsProject t2 = hierarchy as IVsProject;
            Solution as_solution = null;
            Project as_project = null;
            ProjectItem as_projectitem = null;

            result = hierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_ExtObject, out value);
            if (result == VSConstants.S_OK && value != null)
            {
                as_solution = value as Solution;
                as_project = value as Project;
                as_projectitem = value as ProjectItem;
            }

            result = hierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_Name, out value);
            if (result == VSConstants.S_OK && value != null)
            {
                n1 = value.ToString();
            }
            result = hierarchy.GetCanonicalName(itemId, out c1);

            if (as_solution != null)
            {
                AntlrVSIX.GrammarDescription.Workspace.Initialize(t1,
                    as_solution.FullName,
                    as_solution.FileName);
            } else if (as_project != null)
            {
                var ws = AntlrVSIX.GrammarDescription.Workspace.Instance;
                var j_name = as_project.Name;
                var j_fullname = as_project.FullName;
                var j_uniquename = as_project.UniqueName;
                var j_filename = as_project.FileName;
                var find_project = ws.FindProject(j_filename, j_uniquename);
                if (find_project == null)
                {
                    var ws_project = new AntlrVSIX.GrammarDescription.Project(hierarchy, itemId,
                        as_project.UniqueName,
                        as_project.Name,
                        as_project.FileName);
                    ws.AddProject(ws_project);
                    find_project = ws_project;
                }
                {
                    var properties = as_project.Properties;
                    if (properties != null)
                    {
                        var count = properties.Count;
                        for (int i = 0; i < count; ++i)
                        {
                            try
                            {
                                var prop = properties.Item(i);
                                var name = prop.Name;
                                find_project.AddProperty(name);
                            }
                            catch (Exception)
                            { }
                        }
                    }
                }
            }
            else if (as_projectitem != null)
            {
                var ws = AntlrVSIX.GrammarDescription.Workspace.Instance;
                Project containing_project = as_projectitem.ContainingProject;
                AntlrVSIX.GrammarDescription.Project find_project = ws.FindProject(
                    containing_project.FileName,
                    containing_project.Name);
                if (c1 != null)
                {
                    try
                    {
                        var real_name = GetProperFilePathCapitalization(c1);
                        c1 = real_name == null ? c1 : real_name;
                    }
                    catch (Exception)
                    { }
                }
                var find_document = find_project.FindDocument(c1, n1);
                if (find_document == null)
                {
                    var doc = new AntlrVSIX.GrammarDescription.Document(hierarchy, c1, n1);
                    find_project.AddDocument(doc);
                    find_document = doc;
                }
                {
                    var properties = as_projectitem.Properties;
                    if (properties != null)
                    {
                        var count = properties.Count;
                        for (int i = 0; i < count; ++i)
                        {
                            try
                            {
                                var prop = properties.Item(i);
                                var pname = prop.Name;
                                find_document.AddProperty(pname);
                            }
                            catch (Exception _)
                            { }
                        }
                    }
                }
            }
        }

        public static void Load()
        {
            started = true;

            // Convert the entire solution into Project/Document workspace.

            // First, open up every .g4 file in project and parse.
            DTE application = Help.GetApplication();
            if (application == null)
            {
                started = false;
                return;
            }

            IVsSolution ivs_solution = (IVsSolution)Package.GetGlobalService(typeof(SVsSolution));

            ProcessHierarchy(null, ivs_solution as IVsHierarchy);

            finished = true;
        }

        public static string lazy_eval(string name, object props)
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
    }
}
