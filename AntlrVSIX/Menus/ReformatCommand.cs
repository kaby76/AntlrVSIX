
namespace AntlrVSIX.Reformat
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.Package;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using System;
    using System.ComponentModel.Design;

    internal sealed class ReformatCommand
    {
        private readonly Package _package;
        private MenuCommand _menu_item1;
        private MenuCommand _menu_item2;

        private ReformatCommand(Package package)
        {
            _package = package ?? throw new ArgumentNullException("package");
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
            if (view == null) return;

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
                string ffn = buffer.GetFilePath();
                var grammar_description = GrammarDescriptionFactory.Create(ffn);
                if (grammar_description == null) return;
                org.antlr.codebuff.Tool.unformatted_input = text;
                try
                {
                    var result = org.antlr.codebuff.Tool.Main(
                        new object[]
                        {
                        "-g", grammar_description.Name,
                        "-lexer", grammar_description.Lexer,
                        "-parser", grammar_description.Parser,
                        "-rule", grammar_description.StartRule,
                        "-files", grammar_description.FileExtension,
                        "-corpus", corpus_location,
                        "-inoutstring",
                        ""
                        });

                    var edit = buffer.CreateEdit();
                    if (Options.OptionsCommand.Instance.IncrementalReformat)
                    {
                        var diff = new Diff.diff_match_patch();
                        var diffs = diff.diff_main(text, org.antlr.codebuff.Tool.formatted_output);
                        var patch = diff.patch_make(diffs);
                        //patch.Reverse();

                        // Start edit session.
                        int times = 0;
                        int delta = 0;
                        foreach (var p in patch)
                        {
                            times++;
                            var start = p.start1 - delta;

                            var offset = 0;
                            foreach (var ed in p.diffs)
                            {
                                if (ed.operation == Diff.Operation.EQUAL)
                                {
                                    // Let's verify that.
                                    var len = ed.text.Length;
                                    var tokenSpan = new SnapshotSpan(buffer.CurrentSnapshot,
                                      new Span(start + offset, len));
                                    var tt = tokenSpan.GetText();
                                    if (ed.text != tt)
                                    { }
                                    offset = offset + len;
                                }
                                else if (ed.operation == Diff.Operation.DELETE)
                                {
                                    var len = ed.text.Length;
                                    var tokenSpan = new SnapshotSpan(buffer.CurrentSnapshot,
                                      new Span(start + offset, len));
                                    var tt = tokenSpan.GetText();
                                    if (ed.text != tt)
                                    { }
                                    var sp = new Span(start + offset, len);
                                    offset = offset + len;
                                    edit.Delete(sp);
                                }
                                else if (ed.operation == Diff.Operation.INSERT)
                                {
                                    var len = ed.text.Length;
                                    edit.Insert(start + offset, ed.text);
                                }
                            }
                            delta = delta + (p.length2 - p.length1);
                        }
                    }
                    else
                    {
                        edit.Replace(0, buffer.GetBufferText().Length, org.antlr.codebuff.Tool.formatted_output);
                    }
                    edit.Apply();
                }
                catch (Exception eeks)
                {
                    var result = org.antlr.codebuff.Log.Message();
                    System.Windows.Forms.MessageBox.Show(result);
                    return;
                }
            }
        }
    }
}
