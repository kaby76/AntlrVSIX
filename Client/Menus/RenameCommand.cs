namespace LspAntlr
{
    using LanguageServer;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Linq;
    using System.Windows;

    internal class Entry : INotifyPropertyChanged
    {
        public Entry() { }
        public string FileName { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        //void OnPropertyChanged([CallerMemberName]string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }

    internal sealed class RenameCommand
    {
        private readonly AntlrLanguageClient _package;
        //private MenuCommand _menu_item1;
        private MenuCommand _menu_item2;

        private RenameCommand(AntlrLanguageClient package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            _package = package;
            OleMenuCommandService commandService = ((IServiceProvider)ServiceProvider).GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService == null)
            {
                throw new ArgumentNullException("OleMenuCommandService");
            }


            //{
            //    CommandID menu_command_id = new CommandID(new Guid(LspAntlr.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7009);
            //    _menu_item1 = new MenuCommand(RenameCallback, menu_command_id);
            //    _menu_item1.Enabled = false;
            //    commandService.AddCommand(_menu_item1);
            //}
            {
                CommandID menu_command_id = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7009);
                _menu_item2 = new MenuCommand(RenameCallback, menu_command_id);
                _menu_item2.Enabled = false;
                commandService.AddCommand(_menu_item2);
            }
        }

        public bool Enabled
        {
            set
            {
                //_menu_item1.Enabled = value;
                _menu_item2.Enabled = value;
            }
        }

        public bool Visible
        {
            set { }
        }

        public static RenameCommand Instance { get; private set; }

        private AntlrLanguageClient ServiceProvider => _package;

        public static void Initialize(AntlrLanguageClient package)
        {
            Instance = new RenameCommand(package);
        }

        private void RenameCallback(object sender, EventArgs e)
        {
            try
            {
                // Highlight the symbol, reposition it to the beginning of it.
                // Every character changes all occurrences of the symbol.

                IVsTextManager manager = ((IServiceProvider)ServiceProvider).GetService(typeof(VsTextManagerClass)) as IVsTextManager;
                if (manager == null)
                {
                    return;
                }

                manager.GetActiveView(1, null, out IVsTextView view);
                if (view == null)
                {
                    return;
                }

                view.GetCaretPos(out int l, out int c);
                view.GetBuffer(out IVsTextLines buf);
                if (buf == null)
                {
                    return;
                }

                IWpfTextView xxx = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(view);
                ITextBuffer buffer = xxx.TextBuffer;
                string orig_ffn = buffer.GetFFN().Result;
                if (orig_ffn == null)
                {
                    return;
                }

                Workspaces.Document document = Workspaces.Workspace.Instance.FindDocument(orig_ffn);
                if (document == null)
                {
                    return;
                }

                var ref_pd = ParserDetailsFactory.Create(document);
                if (ref_pd == null) return;
                int pos = LanguageServer.Module.GetIndex(l, c, document);
                AntlrLanguageClient alc = AntlrLanguageClient.Instance;
                if (alc == null)
                {
                    return;
                }

                var sym = LanguageServer.Module.GetDocumentSymbol(pos, document);
                if (sym == null) return;
                var locations = LanguageServer.Module.FindRefsAndDefs(pos, document);
                List<SnapshotSpan> wordSpans = new List<SnapshotSpan>();
                var results = new List<Entry>();
                foreach (Location loc in locations)
                {
                    var w = new Entry() { FileName = loc.Uri.FullPath, Start = loc.Range.Start.Value, End = loc.Range.End.Value };
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
                        foreach (string f in files)
                        {
                            var per_file_results = results.Where(r => r.FileName == f);
                            per_file_results.Reverse();
                            var fitem = Workspaces.Workspace.Instance.FindDocument(f);
                            var pd = ParserDetailsFactory.Create(fitem);
                            IVsTextView vstv2 = IVsTextViewExtensions.FindTextViewFor(f);
                            if (vstv2 == null)
                            {
                                IVsTextViewExtensions.ShowFrame(ServiceProvider, f);
                                vstv2 = IVsTextViewExtensions.FindTextViewFor(f);
                            }
                            IWpfTextView wpftv2 = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(vstv2); 
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
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }
        }
    }
}
