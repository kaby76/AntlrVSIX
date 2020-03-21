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
        public (EnvDTE.Project, EnvDTE.ProjectItem) FindProjectAndItem(string fn)
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

        public void EnterChanges(string current_grammar_ffn, Dictionary<string, string> changes)
        {
            var p_f_original_grammar = FindProjectAndItem(current_grammar_ffn);
            foreach (var pair in changes)
            {
                string fn = pair.Key;
                string new_code = pair.Value;
                if (new_code == null)
                {
                    continue;
                }
                Workspaces.Document dd = Workspaces.Workspace.Instance.FindDocument(fn);
                if (dd == null)
                {
                    // Create the file.
                    System.IO.File.WriteAllText(fn, new_code);
                    // Add to project.
                    p_f_original_grammar.Item1.ProjectItems.AddFromFile(fn);
                    // Find new item.
                    var new_item = FindProjectAndItem(fn);
                    // Set attributes.
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
                            var prop = p_f_original_grammar.Item2.Properties.Item("CustomToolNamespace").Value;
                            if (prop.ToString() != "")
                                new_item.Item2.Properties.Item("CustomToolNamespace").Value = prop.ToString();
                        }
                        catch (Exception e)
                        {
                            again = true;
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
            //var edit = buffer.CreateEdit();
            //var diff = new LanguageServer.diff_match_patch();
            //var diffs = diff.diff_main(document.Code, new_code);
            //var patch = diff.patch_make(diffs);
            ////patch.Reverse();

            //// Start edit session.
            //int times = 0;
            //int delta = 0;
            //foreach (var p in patch)
            //{
            //    times++;
            //    var start = p.start1 - delta;

            //    var offset = 0;
            //    foreach (var ed in p.diffs)
            //    {
            //        if (ed.operation == LanguageServer.Operation.EQUAL)
            //        {
            //            // Let's verify that.
            //            var len = ed.text.Length;
            //            var tokenSpan = new SnapshotSpan(buffer.CurrentSnapshot,
            //              new Span(start + offset, len));
            //            var tt = tokenSpan.GetText();
            //            if (ed.text != tt)
            //            { }
            //            offset = offset + len;
            //        }
            //        else if (ed.operation == LanguageServer.Operation.DELETE)
            //        {
            //            var len = ed.text.Length;
            //            var tokenSpan = new SnapshotSpan(buffer.CurrentSnapshot,
            //              new Span(start + offset, len));
            //            var tt = tokenSpan.GetText();
            //            if (ed.text != tt)
            //            { }
            //            var sp = new Span(start + offset, len);
            //            offset = offset + len;
            //            edit.Delete(sp);
            //        }
            //        else if (ed.operation == LanguageServer.Operation.INSERT)
            //        {
            //            var len = ed.text.Length;
            //            edit.Insert(start + offset, ed.text);
            //        }
            //    }
            //    delta = delta + (p.length2 - p.length1);
            //}
            //edit.Apply();
        }

    }
}
