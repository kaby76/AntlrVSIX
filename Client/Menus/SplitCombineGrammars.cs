namespace LspAntlr
{
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;

    internal class SplitCombineGrammars
    {
        private readonly AntlrLanguageClient _package;
        private readonly MenuCommand _menu_item1;
        private readonly MenuCommand _menu_item2;
        private string current_grammar_ffn;


        private SplitCombineGrammars(AntlrLanguageClient package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            OleMenuCommandService commandService = ((IServiceProvider)ServiceProvider).GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService ?? throw new Exception("Command service not found.");

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

#pragma warning disable VSTHRD100
        private async void MenuItemCallback(object sender, EventArgs e, bool split)
#pragma warning restore VSTHRD100
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
                string ffn = await buffer.GetFFN().ConfigureAwait(false);
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
                Dictionary<string, string> changes = alc.CMSplitCombineGrammars(ffn, pos, split);
                EnvDTE.Project project = null;
                string the_namespace = "";
                for (; ; )
                {
                    string current_grammar_ffn = ffn;
                    (EnvDTE.Project, EnvDTE.ProjectItem) p_f_original_grammar = LspAntlr.MakeChanges.FindProjectAndItem(current_grammar_ffn);
                    project = p_f_original_grammar.Item1;
                    try
                    {
                        object prop = p_f_original_grammar.Item2.Properties.Item("CustomToolNamespace").Value;
                        the_namespace = prop.ToString();
                    }
                    catch (Exception)
                    {
                    }
                    break;
                }
                if (project == null)
                {
                    EnvDTE.DTE dte = (EnvDTE.DTE)Package.GetGlobalService(typeof(EnvDTE.DTE));
                    EnvDTE.Projects projects = dte.Solution.Projects;
                    project = projects.Item(EnvDTE.Constants.vsMiscFilesProjectUniqueName);
                }
                if (changes == null) return;
                MakeChanges.EnterChanges(changes, project, the_namespace);
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }
        }
    }
}
