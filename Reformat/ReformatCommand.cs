
namespace AntlrVSIX.Reformat
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Package;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text;
    using System.ComponentModel.Design;
    using System;

    internal sealed class ReformatCommand
    {
        private readonly Package _package;
        private MenuCommand _menu_item1;
        private MenuCommand _menu_item2;

        private ReformatCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            _package = package;
            OleMenuCommandService commandService = this.ServiceProvider.GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService == null) return;

            {
                var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7008);
                _menu_item1 = new MenuCommand(this.MenuItemCallback, menuCommandID);
                _menu_item1.Enabled = false;
                _menu_item1.Visible = true;
                commandService.AddCommand(_menu_item1);
            }
            {
                var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidMenuAndCommandsCmdSet), 0x7008);
                _menu_item2 = new MenuCommand(this.MenuItemCallback, menuCommandID);
                _menu_item2.Enabled = false;
                _menu_item2.Visible = true;
                commandService.AddCommand(_menu_item2);
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

        public static ReformatCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return this._package; }
        }

        public static void Initialize(Package package)
        {
            Instance = new ReformatCommand(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            ////////////////////////
            // Reformat code.
            ////////////////////////

            string classification = AntlrLanguagePackage.Instance.Classification;
            SnapshotSpan span = AntlrLanguagePackage.Instance.Span;
            ITextView view = AntlrLanguagePackage.Instance.View;

            // First, find out what this view is, and what the file is.
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            string path = doc.FilePath;

            string corpus_location = Options.OptionsCommand.Instance.CorpusLocation;
            if (corpus_location == null)
            {
                System.Windows.Forms.MessageBox.Show(
                    "CORPUS_LOCATION is not set. Set location using AntlrVSIX -> Options.");
                return;
            }
            {
                // Get reformated text.
                string text = buffer.GetBufferText();
                org.antlr.codebuff.Tool.unformatted_input = text;
                org.antlr.codebuff.Tool.Main(
                    new string[]
                    {
                        "-g", " ANTLRv4",
                        "-rule", "grammarSpec",
                        "-files", "g4",
                        $@"-corpus", corpus_location,
                        "-inoutstring",
                        ""
                    });

                // Start edit session.
                var edit = buffer.CreateEdit();
                edit.Replace(0, buffer.GetBufferText().Length, org.antlr.codebuff.Tool.formatted_output);
                edit.Apply();
            }
        }
    }
}
