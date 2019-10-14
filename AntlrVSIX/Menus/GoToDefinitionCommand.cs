
namespace AntlrVSIX.GoToDefinition
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Package;
    using LanguageServer;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.ComponentModel.Design;

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
            var file_name = doc.FilePath;
            var item = Workspaces.Workspace.Instance.FindDocument(file_name);
            var ref_pd = ParserDetailsFactory.Create(item);
            var location = LanguageServer.Module.FindDef(span.Start.Position, item);
            if (location == null) return;
            var sym = LanguageServer.Module.GetDocumentSymbol(location.range.Start.Value, location.uri);
            if (sym == null) return;
            string full_file_name = location.uri.FullPath;
            IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(full_file_name);
            {
                IVsTextViewExtensions.ShowFrame(full_file_name);
                vstv = IVsTextViewExtensions.FindTextViewFor(full_file_name);
            }
            if (vstv == null) return;
            IWpfTextView wpftv = vstv.GetIWpfTextView();
            if (wpftv == null) return;
            vstv.GetLineAndColumn(sym.range.Start.Value, out int line_number, out int column_number);
            ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
            SnapshotSpan ss = new SnapshotSpan(cc, sym.range.Start.Value, 1);
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
