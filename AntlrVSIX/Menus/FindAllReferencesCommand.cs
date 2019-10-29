
namespace AntlrVSIX.FindAllReferences
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Model;
    using AntlrVSIX.Package;
    using LanguageServer;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;

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
            int curLoc = span.Start.Position;
            var buf = span.Snapshot.TextBuffer;
            var file_name = buf.GetFFN().Result;
            if (file_name == null) return;
            var document = Workspaces.Workspace.Instance.FindDocument(file_name);
            var ref_pd = ParserDetailsFactory.Create(document);
            if (ref_pd == null) return;
            var locations = LanguageServer.Module.FindRefsAndDefs(curLoc, document);
            List<SnapshotSpan> wordSpans = new List<SnapshotSpan>();
            FindAntlrSymbolsModel.Instance.Results.Clear();
            foreach (var loc in locations)
            {
                var w = new Entry() { FileName = loc.uri.FullPath, Start = loc.range.Start.Value, End = loc.range.End.Value };
                FindAntlrSymbolsModel.Instance.Results.Add(w);
            }
            FindRefsWindowCommand.Instance.Show();
        }
    }
}
