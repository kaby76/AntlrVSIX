using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntlrVSIX.Reformat
{
    using Antlr4.Runtime;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using EnvDTE;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Linq;
    using System;

    internal sealed class ReformatCommand
    {
        public const int _command_id = 0x0103;
        public static readonly Guid _command_set = new Guid("0c1acc31-15ac-417c-86b2-eefdc669e8bf");
        private readonly Package _package;
        private MenuCommand _menu_item;

        private ReformatCommand(Package package)
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
                var menuCommandID = new CommandID(_command_set, _command_id);
                _menu_item = new MenuCommand(this.MenuItemCallback, menuCommandID);
                _menu_item.Enabled = false;
                _menu_item.Visible = false;
                commandService.AddCommand(_menu_item);
            }
        }

        public bool Enabled
        {
            set { _menu_item.Enabled = value; }
        }

        public bool Visible
        {
            set { _menu_item.Visible = value; }
        }

        public SnapshotSpan Symbol { get; set; }
        public string Classification { get; set; }
        public ITextView View { get; set; }
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

            string classification = this.Classification;
            SnapshotSpan span = this.Symbol;
            ITextView view = this.View;

            // First, find out what this view is, and what the file is.
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            string path = doc.FilePath;

            string corpus_location = System.Environment.GetEnvironmentVariable("CORPUS_LOCATION");
            if (corpus_location != null)
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
