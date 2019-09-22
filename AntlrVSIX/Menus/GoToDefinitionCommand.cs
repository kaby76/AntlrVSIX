
namespace AntlrVSIX.GoToDefinition
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
    using System.IO;
    using System.Linq;
    using System;

    internal sealed class GoToDefinitionCommand
    {
        private readonly Package _package;
        private MenuCommand _menu_item1;
        private MenuCommand _menu_item2;

        private GoToDefinitionCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            _package = package;
            OleMenuCommandService commandService = this.ServiceProvider.GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                {
                    // Set up hook for context menu.
                    var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x0100);
                    _menu_item1 = new MenuCommand(this.MenuItemCallback, menuCommandID);
                    _menu_item1.Enabled = false;
                    _menu_item1.Visible = false;
                    commandService.AddCommand(_menu_item1);
                }

                {
                    // Set up hook for context menu.
                    var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidMenuAndCommandsCmdSet), 0x0100);
                    _menu_item2 = new MenuCommand(this.MenuItemCallback, menuCommandID);
                    _menu_item2.Enabled = false;
                    _menu_item2.Visible = true;
                    commandService.AddCommand(_menu_item2);
                }
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
            //    if (_menu_item2 != null) _menu_item2.Visible = value;
            }
        }

        public static GoToDefinitionCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return this._package; }
        }

        public static void Initialize(Package package)
        {
            Instance = new GoToDefinitionCommand(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            ////////////////////////
            // Go to definition....
            ////////////////////////

            SnapshotSpan span = AntlrLanguagePackage.Instance.Span;
            ITextView view = AntlrLanguagePackage.Instance.View;
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            string path_containing_applied_occurrence = Path.GetDirectoryName(doc.FilePath);
            List<Antlr4.Runtime.Tree.TerminalNodeImpl> where = new List<Antlr4.Runtime.Tree.TerminalNodeImpl>();
            List<ParserDetails> where_details = new List<ParserDetails>();
            Antlr4.Runtime.Tree.TerminalNodeImpl token = null;
            foreach (var kvp in ParserDetails._per_file_parser_details)
            {
                string file_name = kvp.Key;
                string path_containing_defining_occurrence = Path.GetDirectoryName(file_name);
                ParserDetails details = kvp.Value;
                {
                    var it = details._defs.Where(
                        (t) => t.Key.Symbol.Text == span.GetText()
                            && path_containing_applied_occurrence
                            == path_containing_defining_occurrence).Select(t => t.Key);
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
            }
            if (where.Any()) token = where.First();
            else return;
            ParserDetails where_token = where_details.First();
            string full_file_name = where_token.FullFileName;
            IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(full_file_name);
            {
                IVsTextViewExtensions.ShowFrame(full_file_name);
                vstv = IVsTextViewExtensions.FindTextViewFor(where_token.FullFileName);
            }
            IWpfTextView wpftv = vstv.GetIWpfTextView();
            if (wpftv == null) return;

            int line_number;
            int colum_number;
            vstv.GetLineAndColumn(token.Symbol.StartIndex, out line_number, out colum_number);
            ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
            SnapshotSpan ss = new SnapshotSpan(cc, token.Symbol.StartIndex, 1);
            SnapshotPoint sp = ss.Start;
            wpftv.Caret.MoveTo(sp);
            if (line_number > 0)
                vstv.CenterLines(line_number - 1, 2);
            else
                vstv.CenterLines(line_number, 1);
            AntlrVSIX.Package.Menus.ResetMenus();
        }
    }
}
