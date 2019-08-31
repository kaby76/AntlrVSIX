
namespace AntlrVSIX.Rename
{
    using Antlr4.Runtime;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.GrammarDescription;
    using AntlrVSIX.Package;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Linq;
    using System.Windows;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.Model;

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
            ITextView view = AntlrLanguagePackage.Instance.View;
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            string path = doc.FilePath;
            IGrammarDescription grammar_description = GrammarDescriptionFactory.Create(path);
            IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(path);
            List<IToken> where = new List<IToken>();
            List<ParserDetails> where_details = new List<ParserDetails>();
            foreach (var kvp in ParserDetails._per_file_parser_details)
            {
                string file_name = kvp.Key;
                ParserDetails details = kvp.Value;
                {
                    var it = details._ant_applied_occurrence_classes.Where(
                        (t) => grammar_description.CanRename[t.Value]
                            && t.Key.Text == span.GetText()).Select(t => t.Key);
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
                {
                    var it = details._ant_defining_occurrence_classes.Where(
                        (t) => grammar_description.CanRename[t.Value]
                            && t.Key.Text == span.GetText()).Select(t => t.Key);
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
            }
            if (!where.Any()) return;
            IWpfTextView wpftv = vstv.GetIWpfTextView();
            if (wpftv == null) return;
            ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
            SnapshotSpan ss = new SnapshotSpan(cc, span.Start.Position, 1);
            SnapshotPoint sp = ss.Start;
            SnapshotSpan? currentWord = RenameHighlightTagger.CurrentWord;

            var results = new List<Entry>();
            for (int i = 0; i < where.Count; ++i)
            {
                IToken x = where[i];
                ParserDetails y = where_details[i];
                var w = new Entry() { FileName = y.FullFileName, LineNumber = x.Line, ColumnNumber = x.Column, Token = x };
                results.Add(w);
            }

            List<SnapshotSpan> wordSpans = new List<SnapshotSpan>();

            for (int i = 0; i < results.Count; ++i)
            {
                var w = results[i];
                if (w.FileName == path)
                {
                    // Create new span in the appropriate view.
                    ITextSnapshot cc2 = buffer.CurrentSnapshot;
                    SnapshotSpan ss2 = new SnapshotSpan(cc2, w.Token.StartIndex, 1 + w.Token.StopIndex - w.Token.StartIndex);
                    SnapshotPoint sp2 = ss2.Start;
                    wordSpans.Add(ss2);
                }
            }

            // Call up the rename dialog box. In another thread because
            // of "The calling thread must be STA, because many UI components require this."
            // error.
            Application.Current.Dispatcher.Invoke((Action)delegate {

                RenameDialogBox inputDialog = new RenameDialogBox(currentWord?.GetText());
                if (inputDialog.ShowDialog() == true)
                {
                    var new_name = inputDialog.Answer;
                    var files = results.Select(r => r.FileName).OrderBy(q => q).Distinct();
                    foreach (var f in files)
                    {
                        var per_file_results = results.Where(r => r.FileName == f);
                        per_file_results.Reverse();
                        var pd = ParserDetails._per_file_parser_details[f];
                        IVsTextView vstv2 = IVsTextViewExtensions.FindTextViewFor(f);
                        if (vstv2 == null)
                        {
                            // File has not been opened before! Open file in editor.
                            IVsTextViewExtensions.ShowFrame(f);
                            vstv2 = IVsTextViewExtensions.FindTextViewFor(f);
                        }
                        IWpfTextView wpftv2 = vstv.GetIWpfTextView();
                        ITextBuffer tb = wpftv2.TextBuffer;
                        using (var edit = tb.CreateEdit())
                        {
                            ITextSnapshot cc2 = tb.CurrentSnapshot;
                            foreach (var e2 in per_file_results)
                            {
                                SnapshotSpan ss2 = new SnapshotSpan(cc2, e2.Token.StartIndex, 1 + e2.Token.StopIndex - e2.Token.StartIndex);
                                SnapshotPoint sp2 = ss2.Start;
                                edit.Replace(ss2, new_name);
                            }
                            edit.Apply();
                            ParserDetails.Parse(tb.GetBufferText(), f);
                        }
                    }
                }
            });

            //AntlrVSIX.Package.Menus.ResetMenus();
        }
    }
}
