using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Workspaces;

namespace LspAntlr
{
    using LanguageServer;
    using Microsoft.CodeAnalysis;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class GoToVisitorCommand
    {
        private readonly AntlrLanguageClient _package;
        private MenuCommand _menu_item1;
        private MenuCommand _menu_item2;
        private MenuCommand _menu_item3;
        private MenuCommand _menu_item4;

        private GoToVisitorCommand(AntlrLanguageClient package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            _package = package;
            OleMenuCommandService commandService = ((IServiceProvider)this.ServiceProvider).GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService == null)
            {
                throw new ArgumentNullException("OleMenuCommandService");
            }

            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7005);
                _menu_item1 = new MenuCommand(this.MenuItemCallbackListener, menuCommandID);
                _menu_item1.Enabled = true;
                _menu_item1.Visible = true;
                commandService.AddCommand(_menu_item1);
            }
            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7006);
                _menu_item2 = new MenuCommand(this.MenuItemCallbackVisitor, menuCommandID);
                _menu_item2.Enabled = true;
                _menu_item2.Visible = true;
                commandService.AddCommand(_menu_item2);
            }
            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7005);
                _menu_item3 = new MenuCommand(this.MenuItemCallbackListener, menuCommandID);
                _menu_item3.Enabled = true;
                _menu_item3.Visible = true;
                commandService.AddCommand(_menu_item3);
            }
            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7006);
                _menu_item4 = new MenuCommand(this.MenuItemCallbackVisitor, menuCommandID);
                _menu_item4.Enabled = true;
                _menu_item4.Visible = true;
                commandService.AddCommand(_menu_item4);
            }
        }

        public bool Enabled
        {
            set
            {
                if (_menu_item1 != null) _menu_item1.Enabled = value;
                if (_menu_item2 != null) _menu_item2.Enabled = value;
                if (_menu_item3 != null) _menu_item3.Enabled = value;
                if (_menu_item4 != null) _menu_item4.Enabled = value;
            }
        }

        public bool Visible
        {
            set { }
        }

        public static GoToVisitorCommand Instance { get; private set; }

        private AntlrLanguageClient ServiceProvider
        {
            get { return this._package; }
        }

        public static void Initialize(AntlrLanguageClient package)
        {
            Instance = new GoToVisitorCommand(package);
        }

        private void MenuItemCallbackVisitor(object sender, EventArgs e)
        {
            MenuItemCallback(sender, e, true);
        }

        private void MenuItemCallbackListener(object sender, EventArgs e)
        {
            MenuItemCallback(sender, e, false);
        }

        private void MenuItemCallback(object sender, EventArgs e, bool visitor)
        {
            try
            {
                ////////////////////////
                /// Go to visitor.
                ////////////////////////

                var manager = ((IServiceProvider)this.ServiceProvider).GetService(typeof(VsTextManagerClass)) as IVsTextManager;
                if (manager == null) return;
                manager.GetActiveView(1, null, out IVsTextView view);
                if (view == null) return;
                view.GetCaretPos(out int l, out int c);
                view.GetBuffer(out IVsTextLines buf);
                if (buf == null) return;
                IWpfTextView xxx = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(view);
                var buffer = xxx.TextBuffer;
                string orig_ffn = buffer.GetFFN().Result;
                if (orig_ffn == null) return;
                var document = Workspaces.Workspace.Instance.FindDocument(orig_ffn);
                if (document == null) return;
                var pos = LanguageServer.Module.GetIndex(l, c, document);
                var alc = AntlrLanguageClient.Instance;
                if (alc == null) return;
                CustomMessage3Result symbol = alc.SendServerCustomMessage3(orig_ffn, pos);
                if (symbol == null) return;

                {
                    var class_file_path = symbol.TextDocument.LocalPath;
                    var index = symbol.Start;
                    // Open to this line in editor.
                    IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(class_file_path);
                    {
                        IVsTextViewExtensions.ShowFrame(this.ServiceProvider, class_file_path);
                        vstv = IVsTextViewExtensions.FindTextViewFor(class_file_path);
                    }

                    IWpfTextView wpftv = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(vstv);
                    if (wpftv == null) return;

                    // Create new span in the appropriate view.
                    ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
                    int line_number;
                    int colum_number;
                    vstv.GetLineAndColumn(index, out line_number, out colum_number);

                    // Put cursor on symbol.
                    wpftv.Caret.MoveTo(new SnapshotPoint(cc, index)); // This sets cursor, bot does not center.
                                            // Center on cursor.
                                            //wpftv.Caret.EnsureVisible(); // This works, sort of. It moves the scroll bar, but it does not CENTER! Does not really work!
                    if (line_number > 0)
                        vstv.CenterLines(line_number - 1, 2);
                    else
                        vstv.CenterLines(line_number, 1);
                    return;
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }
        }
    }
}
