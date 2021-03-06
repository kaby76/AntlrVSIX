﻿namespace LspAntlr
{
    using LoggerNs;
    using Antlr4.Runtime;
    using LanguageServer;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;

    internal sealed class NextSymCommand
    {
        private readonly AntlrLanguageClient _package;
        private readonly MenuCommand _menu_item1;
        private readonly MenuCommand _menu_item2;
        private readonly MenuCommand _menu_item3;
        private readonly MenuCommand _menu_item4;

        private NextSymCommand(AntlrLanguageClient package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            OleMenuCommandService commandService = ((IServiceProvider)ServiceProvider).GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService ?? throw new Exception("Command service not found.");

            {
                // Set up hook for context menu.
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7002);
                _menu_item1 = new MenuCommand(MenuItemCallbackFor, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item1);
            }
            {
                // Set up hook for context menu.
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7003);
                _menu_item2 = new MenuCommand(MenuItemCallbackRev, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item2);
            }
            {
                // Set up hook for context menu.
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7002);
                _menu_item3 = new MenuCommand(MenuItemCallbackFor, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item3);
            }
            {
                // Set up hook for context menu.
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7003);
                _menu_item4 = new MenuCommand(MenuItemCallbackRev, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item4);
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

                if (_menu_item2 != null)
                {
                    _menu_item2.Enabled = value;
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

                if (_menu_item2 != null)
                {
                    _menu_item2.Visible = value;
                }
            }
        }

        public static NextSymCommand Instance { get; private set; }

        private AntlrLanguageClient ServiceProvider => _package;

        public static void Initialize(AntlrLanguageClient package)
        {
            Instance = new NextSymCommand(package);
        }

        private void MenuItemCallbackFor(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            MenuItemCallback(true);
        }

        private void MenuItemCallbackRev(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            MenuItemCallback(false);
        }

        private void MenuItemCallback(bool forward)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            try
            {
                ////////////////////////
                /// Next rule.
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
                int new_pos = AntlrLanguageClient.CMNextSymbol(ffn, pos, forward);
                if (new_pos < 0) return;
                List<IToken> where = new List<IToken>();
                List<ParsingResults> where_details = new List<ParsingResults>();
                IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(ffn);
                if (vstv == null) return;
                IWpfTextView wpftv = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(vstv);
                if (wpftv == null) return;
                vstv.GetLineAndColumn(new_pos, out int line_number, out int colum_number);
                ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
                SnapshotSpan ss = new SnapshotSpan(cc, new_pos, 1);
                SnapshotPoint sp = ss.Start;
                // Put cursor on symbol.
                wpftv.Caret.MoveTo(sp);
                if (line_number > 0)
                {
                    vstv.CenterLines(line_number - 1, 2);
                }
                else
                {
                    vstv.CenterLines(line_number, 1);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }
        }
    }
}
