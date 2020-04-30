namespace LspAntlr
{
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;

    internal class ConvertRecursionToKleeneOperator
    {
        private readonly AntlrLanguageClient _package;
        private readonly MenuCommand _menu_item1;
        private string current_grammar_ffn;


        private ConvertRecursionToKleeneOperator(AntlrLanguageClient package)
        {
            _package = package ?? throw new ArgumentNullException("package");
            OleMenuCommandService commandService = ((IServiceProvider)ServiceProvider).GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService == null)
            {
                throw new ArgumentNullException("OleMenuCommandService");
            }
            {
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7025);
                _menu_item1 = new MenuCommand(MenuItemCallbackSplit, menuCommandID)
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

        public static ConvertRecursionToKleeneOperator Instance { get; private set; }

        private AntlrLanguageClient ServiceProvider => _package;

        public static void Initialize(AntlrLanguageClient package)
        {
            Instance = new ConvertRecursionToKleeneOperator(package);
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
                /// Convert recursion to Kleene operator.
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
                Dictionary<string, string> changes = alc.CMConvertRecursionToKleeneOperator(ffn, pos);
                MakeChanges.EnterIncrementalChanges(ServiceProvider, changes, buffer);
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }
        }
    }
}
