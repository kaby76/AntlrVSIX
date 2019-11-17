namespace Workspaces
{
    using EnvDTE;
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

    public class Loader
    {
        private static bool finished = false;
        private static bool started = false;
        public static Loader Instance = new Loader();

        private static string GetPropertySolution(Solution solution, string name)
        {
            var count = solution.Properties.Count;
            for (int i = 0; i < count; ++i)
            {
                try
                {
                    var prop = solution.Properties.Item(i);
                    var prop_name = prop.Name;
                    if (name == prop_name)
                    {
                        object prop_value = prop.Value;
                        return prop_value as string;
                    }
                }
                catch (Exception)
                { }
            }
            return null;
        }

        private static string GetPropertyProject(EnvDTE.Project project, string name)
        {
            try
            {
                var count = project.Properties.Count;
                for (int i = 0; i < count; ++i)
                {
                    try
                    {
                        var prop = project.Properties.Item(i);
                        var prop_name = prop.Name;
                        //if (name == prop_name)
                        {
                            object prop_value = prop.Value;
                            //return prop_value as string;
                        }
                    }
                    catch (Exception)
                    { }
                }
            }
            catch (Exception) { }
            return null;
        }

        private static string GetPropertyProjectItem(EnvDTE.ProjectItem project_item, string name)
        {
            try
            {
                var count = project_item.Properties.Count;
                for (int i = 0; i < count; ++i)
                {
                    try
                    {
                        var prop = project_item.Properties.Item(i);
                        var prop_name = prop.Name;
                        //if (name == prop_name)
                        {
                            object prop_value = prop.Value;
                            //return prop_value as string;
                        }
                    }
                    catch (Exception)
                    { }
                }
            }
            catch (Exception) { }
            return null;
        }

        public static async System.Threading.Tasks.Task LoadAsync()
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                started = true;

                // Convert the entire solution into Project/Document workspace.

                DTE application = Help.GetApplication();
                if (application == null)
                {
                    started = false;
                    return;
                }
                Solution solution = application.Solution;
                Workspaces.Workspace.Initialize(solution.FullName, solution.FileName);
                var ws = Workspace.Instance;
                ws.Name = GetPropertySolution(solution, "Name");
                ws.FFN = ws.Name;
                HashSet<ProjectItem> visited = new HashSet<ProjectItem>();
                Stack<Tuple<Container, ProjectItem>> stack = new Stack<Tuple<Container, ProjectItem>>();
                Projects projects = null;
                try
                {
                    var ps = solution.Projects;
                    projects = ps as Projects;
                }
                catch (Exception)
                { }
                foreach (var p in projects)
                {
                    var q = p as ProjectItem;
                    if (q != null)
                    {
                        var tuple = new Tuple<Container, ProjectItem>(ws, q);
                        stack.Push(tuple);
                    }
                    else if (p is EnvDTE.Project)
                    {
                        var r = p as EnvDTE.Project;
                        string file_name = r.Name;
                        var project = ws.FindProject(file_name, file_name, file_name);
                        if (project == null)
                        {
                            project = new Workspaces.Project(file_name, file_name, file_name);
                            ws.AddChild(project);
                        }
                        foreach (var pi in r.ProjectItems)
                        {
                            var z = pi as ProjectItem;
                            var tuple = new Tuple<Container, ProjectItem>(project, z);
                            stack.Push(tuple);
                        }
                    }
                }
                while (stack.Any())
                {
                    var tuple = stack.Pop();
                    if (visited.Contains(tuple.Item2)) continue;
                    visited.Add(tuple.Item2);
                    var pi = tuple.Item2;
                    var parent = tuple.Item1;
                    string file_name = pi.Name;
                    var properties = pi.Properties;
                    if (pi as EnvDTE.Project != null)
                    {
                        var project = parent.FindProject(file_name, file_name, file_name);
                        if (project == null)
                        {
                            project = new Workspaces.Project(file_name, file_name, file_name);
                            parent.AddChild(project);
                        }
                        foreach (var p in pi.ProjectItems)
                        {
                            var q = p as ProjectItem;
                            if (q == null) continue;
                            var new_tuple = new Tuple<Container, ProjectItem>(project, q);
                            stack.Push(new_tuple);
                        }
                    }
                    else if (pi as EnvDTE.ProjectItem != null)
                    {
                        string fn = null;
                        try
                        {
                            if (pi.FileCount == 1)
                                fn = pi.FileNames[1];
                            if (fn != null && fn != "")
                                file_name = fn;
                        }
                        catch (Exception) { }
                        if (file_name == null) // Cannot do jack with this.
                            continue;
                        var document = parent.FindDocument(file_name);
                        if (document == null)
                        {
                            document = new Document(file_name, file_name);
                            var pr = parent as Project;
                            pr.AddDocument(document);
                        }
                    }
                }
                finished = true;
            }
            catch (Exception e)
            {
                Logger.Log.Notify(e.ToString());
            }
        }
    }
}
