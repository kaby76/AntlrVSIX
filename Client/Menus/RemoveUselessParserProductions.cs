namespace LspAntlr
{
    using LanguageServer;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;

    internal class RemoveUselessParserProductions
    {
        private readonly AntlrLanguageClient _package;
        private readonly MenuCommand _menu_item1;

        private RemoveUselessParserProductions(AntlrLanguageClient package)
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
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7012);
                _menu_item1 = new MenuCommand(MenuItemCallbackFor, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item1);
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

        public static RemoveUselessParserProductions Instance { get; private set; }

        private AntlrLanguageClient ServiceProvider => _package;

        public static void Initialize(AntlrLanguageClient package)
        {
            Instance = new RemoveUselessParserProductions(package);
        }

        private void MenuItemCallbackFor(object sender, EventArgs e)
        {
            MenuItemCallback(sender, e, true);
        }

        private void MenuItemCallbackRev(object sender, EventArgs e)
        {
            MenuItemCallback(sender, e, false);
        }

        private void MenuItemCallback(object sender, EventArgs e, bool forward)
        {
            try
            {
                ////////////////////////
                /// Remove useless parser productions.
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
                Dictionary<string, string> changes = alc.CMRemoveUselessParserProductions(ffn, pos);
                if (changes == null) return;
                foreach (KeyValuePair<string, string> pair in changes)
                {
                    string fn = pair.Key;
                    Workspaces.Document dd = Workspaces.Workspace.Instance.FindDocument(fn);
                    if (dd == null)
                    {
                        return;
                    }
                    string new_code = pair.Value;
                    ITextEdit edit = buffer.CreateEdit();
                    diff_match_patch diff = new LanguageServer.diff_match_patch();
                    List<Diff> diffs = diff.diff_main(document.Code, new_code);
                    List<Patch> patch = diff.patch_make(diffs);
                    //patch.Reverse();

                    // Start edit session.
                    int times = 0;
                    int delta = 0;
                    foreach (Patch p in patch)
                    {
                        times++;
                        int start = p.start1 - delta;

                        int offset = 0;
                        foreach (Diff ed in p.diffs)
                        {
                            if (ed.operation == LanguageServer.Operation.EQUAL)
                            {
                                // Let's verify that.
                                int len = ed.text.Length;
                                SnapshotSpan tokenSpan = new SnapshotSpan(buffer.CurrentSnapshot,
                                  new Span(start + offset, len));
                                string tt = tokenSpan.GetText();
                                if (ed.text != tt)
                                { }
                                offset = offset + len;
                            }
                            else if (ed.operation == LanguageServer.Operation.DELETE)
                            {
                                int len = ed.text.Length;
                                SnapshotSpan tokenSpan = new SnapshotSpan(buffer.CurrentSnapshot,
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
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }
        }
    }
}
