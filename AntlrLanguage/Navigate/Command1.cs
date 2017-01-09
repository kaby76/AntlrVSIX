using System;
using System.ComponentModel.Design;
using System.Linq;
using Antlr4.Runtime;
using AntlrLanguage.Extensions;
using AntlrLanguage.Grammar;
using AntlrLanguage.Tag;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;
using Microsoft.VisualStudio.TextManager.Interop;

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
            ////////////////////////
            // Go to definition....
            ////////////////////////

            string classification = this.Classification;
            SnapshotSpan span = this.Symbol;
            ITextView view = this.View;
            
            // First, find out what this view is, and what the file is.
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            string path = doc.FilePath;

            //ParserDetails details = null;
            //bool found = ParserDetails._per_file_parser_details.TryGetValue(path, out details);
            //if (!found) return;

            List<IToken> where = new List<IToken>();
            List<ParserDetails> where_details = new List<ParserDetails>();
            IToken token = null;
            foreach (var kvp in ParserDetails._per_file_parser_details)
            {
                string file_name = kvp.Key;
                ParserDetails details = kvp.Value;
                if (classification == "nonterminal")
                {
                    var it = details._ant_nonterminals_defining.Where(
                        (t) => t.Text == span.GetText());
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
                else if (classification == "terminal")
                {
                    var it = details._ant_terminals_defining.Where(
                        (t) => t.Text == span.GetText());
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
            }
            if (where.Any()) token = where.First();
            else return;
            ParserDetails where_token = where_details.First();

            string full_file_name = where_token.full_file_name;
            IVsTextView w = where_token.full_file_name.GetIVsTextView();
            full_file_name.ShowFrame();
            w = where_token.full_file_name.GetIVsTextView();

            var v = VsTextViewCreationListener.to_wpftextview[w];

            // Create new span in the appropriate view.
            ITextSnapshot cc = v.TextBuffer.CurrentSnapshot;
            SnapshotSpan ss = new SnapshotSpan(cc, token.StartIndex, 1);
            var sp = ss.Start;
            // Put cursor on symbol.
            v.Caret.MoveTo(sp);     // This sets cursor, bot does not center.
            // Center on cursor.
            v.Caret.EnsureVisible(); // This works, sort of. It moves the scroll bar, but it does not CENTER! Does not really work!


            //var ok = w.SendExplicitFocus(); // This does not work, does not bring window to top.

            //Microsoft.VisualStudio.TextManager.Interop.IVsTextManager::NavigateToLineAndColumn();
        }
    }
}
