using System;
using System.ComponentModel.Design;
using System.Linq;
using Antlr4.Runtime;
using AntlrLanguage.Tag;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace AntlrLanguage
{
    internal sealed class Command1
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("0c1acc31-15ac-417c-86b2-eefdc669e8bf");
        private readonly Package package;
        private MenuCommand menuItem;

        private Command1(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            this.package = package;
            OleMenuCommandService commandService =
                this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                menuItem.Enabled = false;
                commandService.AddCommand(menuItem);
            }
        }

        public bool Enable
        {
            set { menuItem.Enabled = value; }
        }

        public SnapshotSpan Symbol { get; set; }
        public string Classification { get; set; }
        public ITextView View { get; set; }

        public static Command1 Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return this.package; }
        }

        public static void Initialize(Package package)
        {
            Instance = new Command1(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            // Find.
            AntlrTokenTagger tt = AntlrTokenTagger.Instance;
            string classification = this.Classification;
            SnapshotSpan span = this.Symbol;

            IToken token = null;
            if (classification == "nonterminal")
            {
                var it = tt._ant_nonterminals_defining.Where(
                    (t) => t.Text == span.GetText());
                if (it.Any()) token = it.First();
                else return;
            }
            else if (classification == "terminal")
            {
                var it = tt._ant_terminals_defining.Where(
                    (t) =>
                    {
                        return t.Text == span.GetText();
                    }).ToArray();
                if (it.Any()) token = it.First();
                else return;
            }

            // Create new span.
            ITextSnapshot cc = View.TextBuffer.CurrentSnapshot;
            SnapshotSpan ss = new SnapshotSpan(cc, token.StartIndex, 1);
            var sp = ss.Start;
            // Put cursor on symbol.
            View.Caret.MoveTo(sp);
            // Center on cursor.
            View.Caret.EnsureVisible();
        }
    }
}
