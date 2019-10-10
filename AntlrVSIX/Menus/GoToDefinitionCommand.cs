
namespace AntlrVSIX.GoToDefinition
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
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
            var item = Workspaces.Workspace.Instance.FindDocumentFullName(file_name);
            var ref_pd = ParserDetailsFactory.Create(item);
            Antlr4.Runtime.Tree.IParseTree ref_pt = span.Start.Find();
            if (ref_pt == null) return;
            ref_pd.Attributes.TryGetValue(ref_pt, out Symtab.CombinedScopeSymbol value);
            if (value == null) return;
            var @ref = value as Symtab.Symbol;
            if (@ref == null) return;
            var def = @ref.resolve();
            if (def == null) return;
            var def_file = def.file;
            if (def_file == null) return;
            var def_item = Workspaces.Workspace.Instance.FindDocumentFullName(def_file);
            if (def_item == null) return;
            var def_pd = ParserDetailsFactory.Create(def_item);
            string full_file_name = def_pd.FullFileName;
            IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(full_file_name);
            {
                IVsTextViewExtensions.ShowFrame(full_file_name);
                vstv = IVsTextViewExtensions.FindTextViewFor(def_pd.FullFileName);
            }
            IWpfTextView wpftv = vstv.GetIWpfTextView();
            if (wpftv == null) return;
            int line_number = def.line;
            int column_number = def.col;
            vstv.GetLineAndColumn(def.Token.StartIndex, out line_number, out column_number);
            ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
            SnapshotSpan ss = new SnapshotSpan(cc, def.Token.StartIndex, 1);
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
