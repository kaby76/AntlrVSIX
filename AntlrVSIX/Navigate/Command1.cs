using AntlrVSIX.Extensions;
using AntlrVSIX.Grammar;

namespace AntlrVSIX.Navigate
{
    using System;
    using System.ComponentModel.Design;
    using System.Linq;
    using Antlr4.Runtime;
    using AntlrVSIX;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using System.Collections.Generic;
    using System.IO;
    using EnvDTE;
    using Microsoft.VisualStudio.TextManager.Interop;

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

            // First, open up every .g4 file in project.
            // Get VS solution, if any, and parse all grammars
            DTE application = ApplicationHelper.GetApplication();
            if (application != null)
            {
                IEnumerable<ProjectItem> iterator = ApplicationHelper.SolutionFiles(application);
                ProjectItem[] list = iterator.ToArray();
                foreach (var item in list)
                {
                    //var doc = item.Document; CRASHES!!!! DO NOT USE!
                    //var props = item.Properties;
                    string file_name = item.Name;
                    if (file_name != null)
                    {
                        string prefix = file_name.TrimSuffix(".g4");
                        if (prefix == file_name) continue;

                        try
                        {
                            object prop = item.Properties.Item("FullPath").Value;
                            string ffn = (string)prop;
                            if (!ParserDetails._per_file_parser_details.ContainsKey(ffn))
                            {
                                StreamReader sr = new StreamReader(ffn);
                                ParserDetails foo = new ParserDetails();
                                ParserDetails._per_file_parser_details[ffn] = foo;
                                foo.Parse(sr.ReadToEnd(), ffn);
                            }
                        } catch (Exception eeks)
                        { }
                    }
                }
            }

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
                if (classification == AntlrVSIX.Constants.ClassificationNameNonterminal)
                {
                    var it = details._ant_nonterminals_defining.Where(
                        (t) => t.Text == span.GetText());
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
                else if (classification == AntlrVSIX.Constants.ClassificationNameTerminal)
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
            IVsTextView vstv = full_file_name.GetIVsTextView();
            full_file_name.ShowFrame();
            vstv = where_token.full_file_name.GetIVsTextView();

            IWpfTextView wpftv = null;
            try
            {
                wpftv = VsTextViewCreationListener.to_wpftextview[vstv];
            }
            catch (Exception eeks)
            {
                return;
            }

            int line_number;
            int colum_number;
            vstv.GetLineAndColumn(token.StartIndex, out line_number, out colum_number);

            // Create new span in the appropriate view.
            ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
            SnapshotSpan ss = new SnapshotSpan(cc, token.StartIndex, 1);
            SnapshotPoint sp = ss.Start;
            // Put cursor on symbol.
            wpftv.Caret.MoveTo(sp);     // This sets cursor, bot does not center.
            // Center on cursor.
            //wpftv.Caret.EnsureVisible(); // This works, sort of. It moves the scroll bar, but it does not CENTER! Does not really work!
            if (line_number > 0)
                vstv.CenterLines(line_number - 1, 2);
            else
                vstv.CenterLines(line_number, 1);
        }
    }
}
