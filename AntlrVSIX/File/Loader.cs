namespace AntlrVSIX.File
{
    using EnvDTE;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using System;
    using System.IO;
    using Workspaces;

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

    public class Loader : IVsRunningDocTableEvents3
    {
        private static bool finished = false;
        private static bool started = false;
        public static Loader Instance = new Loader();
        private uint _cookie;

        private static void ProcessHierarchy(Container parent, IVsHierarchy hierarchy)
        {
            // Traverse the nodes of the hierarchy from the root node
            ProcessHierarchyNodeRecursively(parent, hierarchy, VSConstants.VSITEMID_ROOT);
        }

        private static void ProcessHierarchyNodeRecursively(Container parent, IVsHierarchy hierarchy, uint itemId)
        {
            int result;
            IntPtr nestedHiearchyValue = IntPtr.Zero;
            uint nestedItemIdValue;
            object value;
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
                    ProcessHierarchy(parent, nestedHierarchy);
                }
            }
            else // The node is not the root of another hierarchy, it is a regular node
            {
                var new_container = ShowNodeName(parent, hierarchy, itemId);

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
                        ProcessHierarchyNodeRecursively(new_container, hierarchy, visibleChildNode);

                        // Get the next visible sibling node
                        value = null;
                        result = hierarchy.GetProperty(visibleChildNode, (int)__VSHPROPID.VSHPROPID_NextSibling, out value);
                    }
                }
            }
        }

        private static string GetProperDirectoryCapitalization(DirectoryInfo dirInfo)
        {
            DirectoryInfo parentDirInfo = dirInfo.Parent;
            if (null == parentDirInfo)
                return dirInfo.Name.ToUpper();
            return Path.Combine(GetProperDirectoryCapitalization(parentDirInfo),
                                parentDirInfo.GetDirectories(dirInfo.Name)[0].Name);
        }

        private static string GetProperFilePathCapitalization(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            DirectoryInfo dirInfo = fileInfo.Directory;
            if (dirInfo.GetFiles(fileInfo.Name).Length != 0)
                return Path.Combine(GetProperDirectoryCapitalization(dirInfo),
                                dirInfo.GetFiles(fileInfo.Name)[0].Name);
            else
                return GetProperDirectoryCapitalization(dirInfo);
        }
        
        private static Container ShowNodeName(Container parent, IVsHierarchy hierarchy, uint itemId)
        {
            int result;
            object value = null;
            string n1 = "";
            string c1;

            IVsSolution t1 = hierarchy as IVsSolution;
            IVsProject t2 = hierarchy as IVsProject;
            EnvDTE.Solution as_solution = null;
            EnvDTE.Project as_project = null;
            EnvDTE.ProjectItem as_projectitem = null;

            result = hierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_ExtObject, out value);
            if (result == VSConstants.S_OK && value != null)
            {
                as_solution = value as EnvDTE.Solution;
                as_project = value as EnvDTE.Project;
                as_projectitem = value as EnvDTE.ProjectItem;
            }

            result = hierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_Name, out value);
            if (result == VSConstants.S_OK && value != null)
            {
                n1 = value.ToString();
            }
            result = hierarchy.GetCanonicalName(itemId, out c1);

            if (as_solution != null)
            {
                return Workspaces.Workspace.Initialize(t1,
                    as_solution.FullName,
                    as_solution.FileName);
            }
            else if (as_project != null)
            {
                var ws = Workspaces.Workspace.Instance;
                var j_name = as_project.Name;
                var j_fullname = as_project.FullName;
                var j_uniquename = as_project.UniqueName;
                var j_filename = as_project.FileName;
                var ws_project = new Workspaces.Project(hierarchy, itemId,
                    as_project.UniqueName,
                    as_project.Name,
                    as_project.FileName);
                parent.AddChild(ws_project);
                var find_project = ws_project;
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
                return find_project;
            }
            else if (as_projectitem != null)
            {
                var ws = Workspaces.Workspace.Instance;
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
                if (System.IO.Directory.Exists(c1))
                {
                    var project = new Workspaces.Project(hierarchy, itemId,
                        c1,
                        c1,
                        c1);
                    parent.AddChild(project);
                    return project;
                }
                else
                {
                    var doc = new Workspaces.Document(hierarchy, c1, n1);
                    parent.AddChild(doc);
                    var find_document = doc;
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
                            catch (Exception)
                            { }
                        }
                    }
                    return find_document;
                }
            }
            else
                return null;
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

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
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

        public int OnAfterAttributeChangeEx(uint docCookie, uint grfAttribs, IVsHierarchy pHierOld, uint itemidOld, string pszMkDocumentOld, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeSave(uint docCookie)
        {
            AntlrVSIX.File.Loader.Load();
            LanguageServer.Compiler.Compile();
            return VSConstants.S_OK;
        }
    }
}
