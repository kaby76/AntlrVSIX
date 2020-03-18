namespace LspAntlr
{
    using Antlr4.Runtime;
    using LanguageServer;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.LanguageServer.Client;
    using Microsoft.VisualStudio.LanguageServer.Protocol;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Threading;
    using Microsoft.VisualStudio.Utilities;
    using System.Linq;

    class SplitCombineGrammars
    {
        private readonly AntlrLanguageClient _package;
        private readonly MenuCommand _menu_item1;
        private readonly MenuCommand _menu_item2;
        private string current_grammar_ffn;

        private (EnvDTE.Project, EnvDTE.ProjectItem) FindProjectAndItem(string fn)
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

        private void EnterChanges(Dictionary<string, string> changes)
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

        private SplitCombineGrammars(AntlrLanguageClient package)
        {
            _package = package ?? throw new ArgumentNullException("package");
            OleMenuCommandService commandService = ((IServiceProvider)ServiceProvider).GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService == null)
            {
                throw new ArgumentNullException("OleMenuCommandService");
            }

            //{
            //    // Set up hook for context menu.
            //    CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7002);
            //    _menu_item1 = new MenuCommand(MenuItemCallbackFor, menuCommandID)
            //    {
            //        Enabled = true,
            //        Visible = true
            //    };
            //    commandService.AddCommand(_menu_item1);
            //}
            {
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7017);
                _menu_item1 = new MenuCommand(MenuItemCallbackSplit, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item1);
            }
            {
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7018);
                _menu_item2 = new MenuCommand(MenuItemCallbackCombine, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item2);
            }
        }

        public bool Enabled
        {
            set
            {
                if (_menu_item1 != null)
                {
                    _menu_item1.Enabled = value;
                }
            }
        }

        public bool Visible
        {
            set
            {
                if (_menu_item1 != null)
                {
                    _menu_item1.Visible = value;
                }
            }
        }

        public static SplitCombineGrammars Instance { get; private set; }

        private AntlrLanguageClient ServiceProvider => _package;

        public static void Initialize(AntlrLanguageClient package)
        {
            Instance = new SplitCombineGrammars(package);
        }

        private void MenuItemCallbackSplit(object sender, EventArgs e)
        {
            MenuItemCallback(sender, e, true);
        }

        private void MenuItemCallbackCombine(object sender, EventArgs e)
        {
            MenuItemCallback(sender, e, false);
        }

        private void MenuItemCallback(object sender, EventArgs e, bool split)
        {
            try
            {
                ////////////////////////
                /// Reorder parser productions.
                ////////////////////////

                IVsTextManager manager = ((IServiceProvider)ServiceProvider).GetService(typeof(VsTextManagerClass)) as IVsTextManager;
                if (manager == null)
                {
                    return;
                }

                manager.GetActiveView(1, null, out IVsTextView view);
                if (view == null)
                {
                    return;
                }

                view.GetCaretPos(out int l, out int c);
                view.GetBuffer(out IVsTextLines buf);
                if (buf == null)
                {
                    return;
                }

                IWpfTextView xxx = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(view);
                ITextBuffer buffer = xxx.TextBuffer;
                string ffn = buffer.GetFFN().Result;
                if (ffn == null)
                {
                    return;
                }
                current_grammar_ffn = ffn;

                Workspaces.Document document = Workspaces.Workspace.Instance.FindDocument(ffn);
                if (document == null)
                {
                    return;
                }

                int pos = LanguageServer.Module.GetIndex(l, c, document);
                AntlrLanguageClient alc = AntlrLanguageClient.Instance;
                if (alc == null)
                {
                    return;
                }
                Dictionary<string, string> changes = alc.CMSplitCombineGrammarsServer(ffn, pos, split);
                EnterChanges(changes);
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }
        }
    }
}
