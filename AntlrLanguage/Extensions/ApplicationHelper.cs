namespace AntlrLanguage.Extensions
{
    using System.Collections.Generic;
    using EnvDTE;

    internal class ApplicationHelper
    {
        private static IEnumerable<ProjectItem> Recurse(ProjectItems i)
        {
            if (i != null)
            {
                foreach (ProjectItem j in i)
                {
                    foreach (ProjectItem k in Recurse(j))
                    {
                        yield return k;
                    }
                }
            }
        }

        private static IEnumerable<ProjectItem> Recurse(ProjectItem i)
        {
            yield return i;
            foreach (ProjectItem j in Recurse(i.ProjectItems))
            {
                yield return j;
            }
        }

        public static IEnumerable<ProjectItem> SolutionFiles(DTE application)
        {
            Solution solution = (Solution)application.Solution;
            string solution_full_name = solution.FullName;
            string solution_file_name = solution.FileName;
            Properties solution_properties = solution.Properties;
            foreach (Project project in solution.Projects)
            {
                string project_full_name = project.FullName;
                string project_file_name = project.FileName;
                Properties project_properties = project.Properties;
                if (solution_properties != null)
                    foreach (ProjectItem item in Recurse(project.ProjectItems))
                    {
                        yield return item;
                    }
            }
        }

        internal static DTE GetApplication()
        {
            DTE dte = (DTE)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE));
            DTE application = null;
            if (dte != null)
                application = dte.Application;
            return application;
        }
    }
}
