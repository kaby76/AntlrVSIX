using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor;

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

    internal sealed class NextSymCommand
    {
        [Import]
        public IVsEditorAdaptersFactoryService AdaptersFactory = null;

        private readonly Microsoft.VisualStudio.Shell.Package _package;
        private MenuCommand _menu_item1;
        private MenuCommand _menu_item2;
        private MenuCommand _menu_item3;
        private MenuCommand _menu_item4;

        private NextSymCommand(Microsoft.VisualStudio.Shell.Package package)
        {
            _package = package ?? throw new ArgumentNullException("package");
            OleMenuCommandService commandService = this.ServiceProvider.GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService == null)
            {
                throw new ArgumentNullException("OleMenuCommandService");
            }

            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7002);
                _menu_item1 = new MenuCommand(this.MenuItemCallbackFor, menuCommandID);
                _menu_item1.Enabled = true;
                _menu_item1.Visible = true;
                commandService.AddCommand(_menu_item1);
            }
            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7003);
                _menu_item2 = new MenuCommand(this.MenuItemCallbackRev, menuCommandID);
                _menu_item2.Enabled = true;
                _menu_item2.Visible = true;
                commandService.AddCommand(_menu_item2);
            }
            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7002);
                _menu_item3 = new MenuCommand(this.MenuItemCallbackFor, menuCommandID);
                _menu_item3.Enabled = true;
                _menu_item3.Visible = true;
                commandService.AddCommand(_menu_item3);
            }
            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7003);
                _menu_item4 = new MenuCommand(this.MenuItemCallbackRev, menuCommandID);
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
            }
        }

        public bool Visible
        {
            set
            {
                if (_menu_item1 != null) _menu_item1.Visible = value;
                if (_menu_item2 != null) _menu_item2.Visible = value;
            }
        }

        public static NextSymCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return this._package; }
        }

        public static void Initialize(Microsoft.VisualStudio.Shell.Package package)
        {
            Instance = new NextSymCommand(package);
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
                /// Next rule.
                ////////////////////////

                var manager = this.ServiceProvider.GetService(typeof(VsTextManagerClass)) as IVsTextManager;
                if (manager == null) return;
                manager.GetActiveView(1, null, out IVsTextView view);
                if (view == null) return;
                view.GetCaretPos(out int l, out int c);
                view.GetBuffer(out IVsTextLines buf);
                if (buf == null) return;
                IWpfTextView xxx = AdaptersFactory.GetWpfTextView(view);
                var buffer = xxx.TextBuffer;
                string ffn = buffer.GetFFN().Result;
                if (ffn == null) return;
                var document = Workspaces.Workspace.Instance.FindDocument(ffn);
                if (document == null) return;
                var pos = LanguageServer.Module.GetIndex(l, c, document);
                var alc = AntlrLanguageClient.Instance;
                if (alc == null) return;
                var new_pos = alc.SendServerCustomMessage2(pos, ffn);
                if (new_pos < 0) return;

                List<IToken> where = new List<IToken>();
                List<ParserDetails> where_details = new List<ParserDetails>();

                IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(ffn);
                if (vstv == null) return;
                IWpfTextView wpftv = this.AdaptersFactory.GetWpfTextView(vstv);
                if (wpftv == null) return;
                int line_number;
                int colum_number;
                vstv.GetLineAndColumn(new_pos, out line_number, out colum_number);
                ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
                SnapshotSpan ss = new SnapshotSpan(cc, new_pos, 1);
                SnapshotPoint sp = ss.Start;
                // Put cursor on symbol.
                wpftv.Caret.MoveTo(sp);
                if (line_number > 0)
                    vstv.CenterLines(line_number - 1, 2);
                else
                    vstv.CenterLines(line_number, 1);
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }
        }
    }
}
