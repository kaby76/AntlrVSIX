﻿namespace LspAntlr
{
    using LoggerNs;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.ComponentModel.Design;

    internal class ConvertRecursionToKleeneOperator
    {
        private readonly AntlrLanguageClient _package;
        private readonly MenuCommand _menu_item1;

        private ConvertRecursionToKleeneOperator(AntlrLanguageClient package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            OleMenuCommandService commandService = ((IServiceProvider)ServiceProvider).GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService ?? throw new Exception("Command service not found.");

            {
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7025);
                _menu_item1 = new MenuCommand(MenuItemCallback, menuCommandID)
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

        private void MenuItemCallback(object sender, EventArgs e)
        {
            try
            {
                ////////////////////////
                /// Convert recursion to Kleene operator.
                ////////////////////////

                if (!(((IServiceProvider)ServiceProvider).GetService(typeof(VsTextManagerClass)) is IVsTextManager manager)) return;
                manager.GetActiveView(1, null, out IVsTextView view);
                if (view == null) return;
                view.GetCaretPos(out int l, out int c);
                view.GetSelection(out int ls, out int cs, out int le, out int ce);
                view.GetBuffer(out IVsTextLines buf);
                if (buf == null) return;
                ITextBuffer buffer = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(view)?.TextBuffer;
                string ffn = buffer?.GetFFN();
                if (ffn == null) return;
                Workspaces.Document document = Workspaces.Workspace.Instance.FindDocument(ffn);
                if (document == null) return;
                int start = new LanguageServer.Module().GetIndex(ls, cs, document);
                int end = new LanguageServer.Module().GetIndex(le, ce, document);
                AntlrLanguageClient.CMConvertRecursionToKleeneOperator(ffn, start, end);
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }
        }
    }
}
