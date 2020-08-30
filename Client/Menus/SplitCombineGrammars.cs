namespace LspAntlr
{
    using LoggerNs;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;

    internal class SplitCombineGrammars
    {
        private readonly AntlrLanguageClient _package;
        private readonly MenuCommand _menu_item1;
        private readonly MenuCommand _menu_item2;

        private SplitCombineGrammars(AntlrLanguageClient package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            OleMenuCommandService commandService = ((IServiceProvider)ServiceProvider).GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService ?? throw new Exception("Command service not found.");

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
            ThreadHelper.ThrowIfNotOnUIThread();
            MenuItemCallback(true);
        }

        private void MenuItemCallbackCombine(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            MenuItemCallback(false);
        }

        private void MenuItemCallback(bool split)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            try
            {
                ////////////////////////
                /// Reorder parser productions.
                ////////////////////////

                if (!(((IServiceProvider)ServiceProvider).GetService(typeof(VsTextManagerClass)) is IVsTextManager manager)) return;
                manager.GetActiveView(1, null, out IVsTextView view);
                if (view == null) return;
                view.GetCaretPos(out int l, out int c);
                view.GetBuffer(out IVsTextLines buf);
                if (buf == null) return;
                ITextBuffer buffer = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(view)?.TextBuffer;
                string ffn = buffer.GetFFN();
                if (ffn == null) return;
                Workspaces.Document document = Workspaces.Workspace.Instance.FindDocument(ffn);
                if (document == null) return;
                int pos = new LanguageServer.Module().GetIndex(l, c, document);
                Dictionary<string, string> changes = AntlrLanguageClient.CMSplitCombineGrammars(ffn, split);
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
