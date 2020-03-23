using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace LspAntlr
{
    class MakeChanges
    {
        public static (EnvDTE.Project, EnvDTE.ProjectItem) FindProjectAndItem(string fn)
        {
            var f = System.IO.Path.GetFileName(fn);
            EnvDTE.DTE dte = (EnvDTE.DTE)Package.GetGlobalService(typeof(EnvDTE.DTE));
            for (int i = 1; i <= dte.Solution.Projects.Count; ++i)
            {
                EnvDTE.Project project = dte.Solution.Projects.Item(i);
                for (int j = 1; j <= project.ProjectItems.Count; ++j)
                {
                    var item = project.ProjectItems.Item(j);
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
            foreach (var pair in changes)
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
                    var new_item = FindProjectAndItem(fn);
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
                                if (custom_namespace != "")
                                    new_item.Item2.Properties.Item("CustomToolNamespace").Value = custom_namespace;
                            }
                            catch (Exception e)
                            {
                                again = true;
                            }
                        }
                    }
                }
            }
            foreach (var pair in changes)
            {
                string fn = pair.Key;
                string new_code = pair.Value;
                if (new_code == null)
                {
                    var p_f = FindProjectAndItem(fn);
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
