namespace AntlrVSIX.Rename
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Model;
    using AntlrVSIX.Package;
    using LanguageServer;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Linq;
    using System.Windows;

    internal sealed class RenameCommand
    {
        private readonly Package _package;
        private MenuCommand _menu_item1;
        private MenuCommand _menu_item2;

        private RenameCommand(Package package)
        {
            _package = package ?? throw new ArgumentNullException("package");
            OleMenuCommandService command_service = this.ServiceProvider.GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService;
            if (command_service == null) return;

            {
                CommandID menu_command_id = new CommandID(new Guid(AntlrVSIX.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7009);
                _menu_item1 = new MenuCommand(RenameCallback, menu_command_id);
                _menu_item1.Enabled = false;
                command_service.AddCommand(_menu_item1);
            }
            {
                CommandID menu_command_id = new CommandID(new Guid(AntlrVSIX.Constants.guidMenuAndCommandsCmdSet), 0x7009);
                _menu_item2 = new MenuCommand(RenameCallback, menu_command_id);
                _menu_item2.Enabled = false;
                command_service.AddCommand(_menu_item2);
            }
        }

        public bool Enabled
        {
            set
            {
                _menu_item1.Enabled = value;
                _menu_item2.Enabled = value;
            }
        }

        public bool Visible
        {
            set { }
        }

        public static RenameCommand Instance { get; private set; }

        private System.IServiceProvider ServiceProvider
        {
            get { return this._package; }
        }

        public static void Initialize(Package package)
        {
            Instance = new RenameCommand(package);
        }

        private void RenameCallback(object sender, EventArgs e)
        {
            // Highlight the symbol, reposition it to the beginning of it.
            // Every character changes all occurrences of the symbol.

            SnapshotSpan span = AntlrLanguagePackage.Instance.Span;
            int curLoc = span.Start.Position;
            var buf = span.Snapshot.TextBuffer;
            var doc = buf.GetTextDocument();
            var file_name = doc.FilePath;
            var item = Workspaces.Workspace.Instance.FindDocument(file_name);
            var ref_pd = ParserDetailsFactory.Create(item);
            if (ref_pd == null) return;
            var sym = LanguageServer.Module.GetDocumentSymbol(curLoc, item);
            if (sym == null) return;
            var locations = LanguageServer.Module.FindRefsAndDefs(curLoc, item);
            List<SnapshotSpan> wordSpans = new List<SnapshotSpan>();
            var results = new List<Entry>();
            foreach (var loc in locations)
            {
                if (Options.OptionsCommand.Instance.RestrictedDirectory)
                {
                    string p1 = System.IO.Path.GetDirectoryName(file_name);
                    string p2 = System.IO.Path.GetDirectoryName(loc.uri.FullPath);
                    if (p1 != p2) continue;
                }
                var w = new Entry() { FileName = loc.uri.FullPath, Start = loc.range.Start.Value, End = loc.range.End.Value };
                results.Add(w);
            }

            // Call up the rename dialog box. In another thread because
            // of "The calling thread must be STA, because many UI components require this."
            // error.
            Application.Current.Dispatcher.Invoke((Action)delegate {

                RenameDialogBox inputDialog = new RenameDialogBox(sym.name);
                if (inputDialog.ShowDialog() == true)
                {
                    var new_name = inputDialog.Answer;
                    var files = results.Select(r => r.FileName).OrderBy(q => q).Distinct();
                    foreach (var f in files)
                    {
                        var per_file_results = results.Where(r => r.FileName == f);
                        per_file_results.Reverse();
                        var fitem = Workspaces.Workspace.Instance.FindDocument(f);
                        var pd = ParserDetailsFactory.Create(fitem);
                        IVsTextView vstv2 = IVsTextViewExtensions.FindTextViewFor(f);
                        if (vstv2 == null)
                        {
                            // File has not been opened before! Open file in editor.
                            IVsTextViewExtensions.ShowFrame(f);
                            vstv2 = IVsTextViewExtensions.FindTextViewFor(f);
                        }
                        IWpfTextView wpftv2 = vstv2.GetIWpfTextView();
                        ITextBuffer tb = wpftv2.TextBuffer;
                        using (var edit = tb.CreateEdit())
                        {
                            ITextSnapshot cc2 = tb.CurrentSnapshot;
                            foreach (var e2 in per_file_results)
                            {
                                SnapshotSpan ss2 = new SnapshotSpan(cc2, e2.Start, 1 + e2.End - e2.Start);
                                SnapshotPoint sp2 = ss2.Start;
                                edit.Replace(ss2, new_name);
                            }
                            edit.Apply();
                        }
                    }
                }
            });

            //AntlrVSIX.Package.Menus.ResetMenus();
        }
    }
}
