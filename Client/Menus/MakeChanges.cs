namespace LspAntlr
{
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;

    internal class MakeChanges
    {
        public static (EnvDTE.Project, EnvDTE.ProjectItem) FindProjectAndItem(string fn)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string f = System.IO.Path.GetFileName(fn);
            EnvDTE.DTE dte = (EnvDTE.DTE)Package.GetGlobalService(typeof(EnvDTE.DTE));
            for (int i = 1; i <= dte.Solution.Projects.Count; ++i)
            {
                EnvDTE.Project project = dte.Solution.Projects.Item(i);
                for (int j = 1; j <= project.ProjectItems.Count; ++j)
                {
                    EnvDTE.ProjectItem item = project.ProjectItems.Item(j);
                    if (item.Name == f)
                    {
                        return (project, item);
                    }
                }
            }
            return (null, null);
        }

        public static void EnterChanges(Dictionary<string, string> changes, EnvDTE.Project project, string custom_namespace)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            foreach (KeyValuePair<string, string> pair in changes)
            {
                string fn = pair.Key;
                string new_code = pair.Value;
                if (new_code == null)
                {
                    continue;
                }
                {
                    // Create the file.
                    System.IO.File.WriteAllText(fn, new_code);
                    // Add to project.
                    project.ProjectItems.AddFromFile(fn);
                    // Find new item.
                    (EnvDTE.Project, EnvDTE.ProjectItem) new_item = FindProjectAndItem(fn);
                    if (fn.EndsWith(".g4"))
                    {
                        // Set attributes, but only for grammar file.
                        // Believe or not, something is wrong with VS in that
                        // properties when set get an exception thrown. The "cure"
                        // from my anecdotal evidence is to just keep repeating until
                        // it "sticks"! Really really bad, but it works.
                        bool again = true;
                        for (int times = 0; times < 10 && again; ++times)
                        {
                            again = false;
                            new_item.Item2.Properties.Item("ItemType").Value = "Antlr4";
                            try
                            {
                                if (!string.IsNullOrEmpty(custom_namespace))
                                {
                                    new_item.Item2.Properties.Item("CustomToolNamespace").Value = custom_namespace;
                                }
                            }
                            catch (Exception)
                            {
                                again = true;
                            }
                        }
                    }
                }
            }
            foreach (KeyValuePair<string, string> pair in changes)
            {
                string fn = pair.Key;
                string new_code = pair.Value;
                if (new_code == null)
                {
                    (EnvDTE.Project, EnvDTE.ProjectItem) p_f = FindProjectAndItem(fn);
                    if (p_f.Item1 != null && p_f.Item2 != null)
                    {
                        // Delete from project.
                        p_f.Item2.Delete();
                        // Delete the file.
                        System.IO.File.Delete(fn);
                    }
                }
            }
        }
    }
}
