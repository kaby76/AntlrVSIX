
namespace AntlrVSIX.FindAllReferences
{
    using Antlr4.Runtime;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.GrammarDescription;
    using AntlrVSIX.Model;
    using AntlrVSIX.Package;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Linq;

    internal sealed class FindAllReferencesCommand
    {
        private readonly Package _package;
        private MenuCommand _menu_item1;
        private MenuCommand _menu_item2;

        private FindAllReferencesCommand(Package package)
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
                    var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7004);
                    _menu_item1 = new MenuCommand(this.MenuItemCallback, menuCommandID);
                    _menu_item1.Enabled = false;
                    _menu_item1.Visible = false;
                    commandService.AddCommand(_menu_item1);
                }
                {
                    var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidMenuAndCommandsCmdSet), 0x7004);
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

        public static FindAllReferencesCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return this._package; }
        }

        public static void Initialize(Package package)
        {
            Instance = new FindAllReferencesCommand(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            ////////////////////////
            // Find all references..
            ////////////////////////

            SnapshotSpan span = AntlrLanguagePackage.Instance.Span;
            ITextView view = AntlrLanguagePackage.Instance.View;
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            var file_name = doc.FilePath;
            var item = AntlrVSIX.GrammarDescription.Workspace.Instance.FindProjectFullName(file_name);
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
            var def_item = AntlrVSIX.GrammarDescription.Workspace.Instance.FindProjectFullName(def_file);
            if (def_item == null) return;
            FindAntlrSymbolsModel.Instance.Results.Clear();
            foreach (KeyValuePair<Antlr4.Runtime.Tree.IParseTree, Symtab.CombinedScopeSymbol> attr in ref_pd.Attributes)
            {
                if (!(attr.Key is Antlr4.Runtime.Tree.TerminalNodeImpl)) continue;
                if (!(attr.Value is Symtab.Symbol)) continue;
                var symbol = attr.Value as Symtab.Symbol;
                if (symbol.resolve() != def) continue;
                var w = new Entry() { FileName = symbol.Token.InputStream.SourceName, LineNumber = symbol.line, ColumnNumber = symbol.col, Token = symbol.Token };
                FindAntlrSymbolsModel.Instance.Results.Add(w);
            }
            FindRefsWindowCommand.Instance.Show();
        }
    }
}
