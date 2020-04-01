using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using LanguageServer;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace LspAntlr
{
    internal class MakeChanges
    {
        public static (EnvDTE.Project, EnvDTE.ProjectItem) FindProjectAndItem(string fn)
        {
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
                                if (custom_namespace != "")
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

        public static void EnterIncrementalChanges(AntlrLanguageClient ServiceProvider, Dictionary<string, string> changes, ITextBuffer buffsssser)
        {
            if (changes == null)
            {
                return;
            }
            foreach (KeyValuePair<string, string> pair in changes)
            {
                string fn = pair.Key;
                string new_code = pair.Value;
                if (new_code == null)
                {
                    continue;
                }
                {
                    Workspaces.Document document = Workspaces.Workspace.Instance.FindDocument(fn);
                    if (document == null)
                    {
                        return;
                    }
                    ITextEdit edit = null;
                    try
                    {
                        // Find buffer for document.
                        // Open to this line in editor.
                        IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(fn);
                        {
                            IVsTextViewExtensions.ShowFrame(ServiceProvider, fn);
                            vstv = IVsTextViewExtensions.FindTextViewFor(fn);
                        }
                        IWpfTextView wpftv = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(vstv);
                        if (wpftv == null)
                        {
                            return;
                        }
                        ITextBuffer the_buffer = wpftv.TextBuffer;

                        edit = the_buffer.CreateEdit();
                        LanguageServer.diff_match_patch diff = new LanguageServer.diff_match_patch();
                        List<LanguageServer.Diff> diffs = diff.diff_main(document.Code, new_code);
                        List<LanguageServer.Patch> patch = diff.patch_make(diffs);
                        //patch.Reverse();

                        // Start edit session.
                        int times = 0;
                        int delta = 0;
                        foreach (LanguageServer.Patch p in patch)
                        {
                            times++;
                            int start = p.start1 - delta;

                            int offset = 0;
                            foreach (LanguageServer.Diff ed in p.diffs)
                            {
                                if (ed.operation == LanguageServer.Operation.EQUAL)
                                {
                                    // Let's verify that.
                                    int len = ed.text.Length;
                                    SnapshotSpan tokenSpan = new SnapshotSpan(the_buffer.CurrentSnapshot,
                                      new Span(start + offset, len));
                                    string tt = tokenSpan.GetText();
                                    if (ed.text != tt)
                                    { }
                                    offset = offset + len;
                                }
                                else if (ed.operation == LanguageServer.Operation.DELETE)
                                {
                                    int len = ed.text.Length;
                                    SnapshotSpan tokenSpan = new SnapshotSpan(the_buffer.CurrentSnapshot,
                                      new Span(start + offset, len));
                                    string tt = tokenSpan.GetText();
                                    if (ed.text != tt)
                                    { }
                                    Span sp = new Span(start + offset, len);
                                    offset = offset + len;
                                    edit.Delete(sp);
                                }
                                else if (ed.operation == LanguageServer.Operation.INSERT)
                                {
                                    int len = ed.text.Length;
                                    edit.Insert(start + offset, ed.text);
                                }
                            }
                            delta = delta + (p.length2 - p.length1);
                        }
                        edit.Apply();
                    }
                    catch (Exception eeks)
                    {
                        edit.Cancel();
                    }
                }
            }
        }
    }
}
