namespace AntlrVSIX.Extensions
{
    using EnvDTE;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;

    public static class ProjectHelpers
    {
        public static object GetSelectedItem()
        {
            object selectedObject = null;

            var monitorSelection = (IVsMonitorSelection)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SVsShellMonitorSelection));

            try
            {
                monitorSelection.GetCurrentSelection(out IntPtr hierarchyPointer,
                    out uint itemId,
                    out IVsMultiItemSelect multiItemSelect,
                    out IntPtr selectionContainerPointer);


                if (Marshal.GetTypedObjectForIUnknown(
                    hierarchyPointer,
                    typeof(IVsHierarchy)) is IVsHierarchy selectedHierarchy)
                {
                    ErrorHandler.ThrowOnFailure(selectedHierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_ExtObject, out selectedObject));
                }

                Marshal.Release(hierarchyPointer);
                Marshal.Release(selectionContainerPointer);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }

            return selectedObject;
        }

        public static string GetRootFolder(this Project project)
        {
            if (project == null)
                return null;

            DTE application = DteExtensions.GetApplication();
            if (application == null) return null;

            if (project.IsKind("{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")) //ProjectKinds.vsProjectKindSolutionFolder
                return Path.GetDirectoryName(application.Solution.FullName);

            if (string.IsNullOrEmpty(project.FullName))
                return null;

            string fullPath;

            try
            {
                fullPath = project.Properties.Item("FullPath").Value as string;
            }
            catch (ArgumentException)
            {
                try
                {
                    // MFC projects don't have FullPath, and there seems to be no way to query existence
                    fullPath = project.Properties.Item("ProjectDirectory").Value as string;
                }
                catch (ArgumentException)
                {
                    // Installer projects have a ProjectPath.
                    fullPath = project.Properties.Item("ProjectPath").Value as string;
                }
            }

            if (string.IsNullOrEmpty(fullPath))
                return System.IO.File.Exists(project.FullName) ? Path.GetDirectoryName(project.FullName) : null;

            if (Directory.Exists(fullPath))
                return fullPath;

            if (System.IO.File.Exists(fullPath))
                return Path.GetDirectoryName(fullPath);

            return null;
        }
        public static bool IsKind(this Project project, params string[] kindGuids)
        {
            foreach (string guid in kindGuids)
            {
                if (project.Kind.Equals(guid, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public static ProjectItem AddFileToProject(this Project project, FileInfo file, string itemType = null)
        {
            string root = project.GetRootFolder();

            if (string.IsNullOrEmpty(root) || !file.FullName.StartsWith(root, StringComparison.OrdinalIgnoreCase))
                return null;

            ProjectItem item = project.ProjectItems.AddFromFile(file.FullName);
            item.SetItemType(itemType);
            return item;
        }
        public static void SetItemType(this ProjectItem item, string itemType)
        {
            try
            {
                if (item == null || item.ContainingProject == null)
                    return;

                if (string.IsNullOrEmpty(itemType)
                    || item.ContainingProject.IsKind(ProjectTypes.WEBSITE_PROJECT)
                    || item.ContainingProject.IsKind(ProjectTypes.UNIVERSAL_APP))
                    return;

                item.Properties.Item("ItemType").Value = itemType;
            }
            catch (Exception ex)
            {
            }
        }
    }
    public static class ProjectTypes
    {
        public const string ASPNET_5 = "{8BB2217D-0F2D-49D1-97BC-3654ED321F3B}";
        public const string DOTNET_Core = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";
        public const string WEBSITE_PROJECT = "{E24C65DC-7377-472B-9ABA-BC803B73C61A}";
        public const string UNIVERSAL_APP = "{262852C6-CD72-467D-83FE-5EEB1973A190}";
        public const string NODE_JS = "{9092AA53-FB77-4645-B42D-1CCCA6BD08BD}";
        public const string SSDT = "{00d1a9c2-b5f0-4af3-8072-f6c62b433612}";
    }

    public static class FilePathExtension
    {
        public static bool IsAntlrSuffix(this string file_name)
        {
            if (file_name == null) return false;
            var allowable_suffices = Constants.FileExtension.Split(';').ToList<string>();
            var suffix = Path.GetExtension(file_name).ToLower();
            foreach (var s in allowable_suffices)
                if (suffix == s)
                    return true;
            return false;
        }
    }
}
