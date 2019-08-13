namespace AntlrVSIX.NextSym
{
    using Antlr4.Runtime;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.Package;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System;

    internal sealed class NextSymCommand
    {
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
                var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7002);
                _menu_item1 = new MenuCommand(this.MenuItemCallbackFor, menuCommandID);
                _menu_item1.Enabled = true;
                _menu_item1.Visible = true;
                commandService.AddCommand(_menu_item1);
            }
            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7003);
                _menu_item2 = new MenuCommand(this.MenuItemCallbackRev, menuCommandID);
                _menu_item2.Enabled = true;
                _menu_item2.Visible = true;
                commandService.AddCommand(_menu_item2);
            }
            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidMenuAndCommandsCmdSet), 0x7002);
                _menu_item3 = new MenuCommand(this.MenuItemCallbackFor, menuCommandID);
                _menu_item3.Enabled = true;
                _menu_item3.Visible = true;
                commandService.AddCommand(_menu_item3);
            }
            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidMenuAndCommandsCmdSet), 0x7003);
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
            ////////////////////////
            /// Next rule.
            ////////////////////////


            string classification = AntlrLanguagePackage.Instance.Classification;
            SnapshotSpan span = AntlrLanguagePackage.Instance.Span;
            ITextView view = AntlrLanguagePackage.Instance.View;

            view = AntlrLanguagePackage.Instance.GetActiveView();
            if (view == null) return;

            ITextCaret car = view.Caret;
            CaretPosition cp = car.Position;
            SnapshotPoint bp = cp.BufferPosition;
            int pos = bp.Position;

            // First, find out what this view is, and what the file is.
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            string path = doc.FilePath;

            //ParserDetails details = null;
            //bool found = ParserDetails._per_file_parser_details.TryGetValue(path, out details);
            //if (!found) return;

            List<IToken> where = new List<IToken>();
            List<ParserDetails> where_details = new List<ParserDetails>();
            int next_sym = forward ? Int32.MaxValue : -1;
            foreach (var kvp in ParserDetails._per_file_parser_details)
            {
                string file_name = kvp.Key;
                if (file_name != path)
                    continue;
                ParserDetails details = kvp.Value;
                foreach (var t in details._ant_nonterminals_defining)
                {
                    if (forward)
                    {
                        if (t.StartIndex > pos && t.StartIndex < next_sym)
                            next_sym = t.StartIndex;
                    }
                    else
                    {
                        if (t.StartIndex < pos && t.StartIndex > next_sym)
                            next_sym = t.StartIndex;
                    }
                }

                foreach (var t in details._ant_terminals_defining)
                {
                    if (forward)
                    {
                        if (t.StartIndex > pos && t.StartIndex < next_sym)
                            next_sym = t.StartIndex;
                    }
                    else
                    {
                        if (t.StartIndex < pos && t.StartIndex > next_sym)
                            next_sym = t.StartIndex;
                    }
                }

                break;
            }

            if (next_sym == Int32.MaxValue || next_sym < 0) return;

            string full_file_name = path;
            IVsTextView vstv = IVsTextViewExtensions.GetIVsTextView(full_file_name);
            if (vstv == null)
            {
                IVsTextViewExtensions.ShowFrame(full_file_name);
                vstv = IVsTextViewExtensions.GetIVsTextView(full_file_name);
            }

            IWpfTextView wpftv = vstv.GetIWpfTextView();
            if (wpftv == null) return;

            int line_number;
            int colum_number;
            vstv.GetLineAndColumn(next_sym, out line_number, out colum_number);

            // Create new span in the appropriate view.
            ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
            SnapshotSpan ss = new SnapshotSpan(cc, next_sym, 1);
            SnapshotPoint sp = ss.Start;
            // Put cursor on symbol.
            wpftv.Caret.MoveTo(sp); // This sets cursor, bot does not center.
            // Center on cursor.
            if (line_number > 0)
                vstv.CenterLines(line_number - 1, 2);
            else
                vstv.CenterLines(line_number, 1);
            AntlrVSIX.Package.Menus.ResetMenus();
        }
    }
}
