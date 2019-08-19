
namespace AntlrVSIX.Rename
{
    using Antlr4.Runtime;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.Package;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Linq;

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

        private IServiceProvider ServiceProvider
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

            string classification = AntlrLanguagePackage.Instance.Classification;
            SnapshotSpan span = AntlrLanguagePackage.Instance.Span;
            ITextView view = AntlrLanguagePackage.Instance.View;

            // First, find out what this view is, and what the file is.
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            string path = doc.FilePath;
            IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(path);

            List<IToken> where = new List<IToken>();
            List<ParserDetails> where_details = new List<ParserDetails>();
            foreach (var kvp in ParserDetails._per_file_parser_details)
            {
                string file_name = kvp.Key;
                ParserDetails details = kvp.Value;
                if (classification == AntlrVSIX.Constants.ClassificationNameNonterminal ||
                    classification == AntlrVSIX.Constants.ClassificationNameTerminal)
                {
                    var it = details._ant_applied_occurrence_classes.Where(
                        (t) => (t.Value == 0 || t.Value == 1) && t.Key.Text == span.GetText()).Select(t => t.Key);
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
            }
            if (!where.Any()) return;

            IWpfTextView wpftv = vstv.GetIWpfTextView();
            if (wpftv == null) return;

            // Create new span in the appropriate view.
            ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
            SnapshotSpan ss = new SnapshotSpan(cc, span.Start.Position, 1);
            SnapshotPoint sp = ss.Start;

            // Enable highlighter.
            RenameHighlightTagger.Update(view, sp);

            //AntlrVSIX.Package.Menus.ResetMenus();
        }
    }
}
