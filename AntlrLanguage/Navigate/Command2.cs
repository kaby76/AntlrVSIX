using System.Collections.ObjectModel;

namespace AntlrLanguage.Navigate
{
    using System;
    using System.ComponentModel.Design;
    using System.Linq;
    using Antlr4.Runtime;
    using AntlrLanguage.Extensions;
    using AntlrLanguage.Grammar;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using System.Collections.Generic;
    using System.IO;
    using EnvDTE;
    using Microsoft.VisualStudio.TextManager.Interop;

    internal sealed class Command2
    {
        public const int CommandId = 0x0101;
        public static readonly Guid CommandSet = new Guid("0c1acc31-15ac-417c-86b2-eefdc669e8bf");
        private readonly Package package;
        private MenuCommand menuItem;

        private Command2(Package package)
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
        public static Command2 Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return this.package; }
        }

        public static void Initialize(Package package)
        {
            Instance = new Command2(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            ////////////////////////
            // Find all references..
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
                        }
                        catch (Exception eeks)
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

            List<IToken> where = new List<IToken>();
            List<ParserDetails> where_details = new List<ParserDetails>();
            IToken token = null;
            foreach (var kvp in ParserDetails._per_file_parser_details)
            {
                string file_name = kvp.Key;
                ParserDetails details = kvp.Value;
                if (classification == AntlrLanguage.Constants.ClassificationNameNonterminal)
                {
                    var it = details._ant_nonterminals.Where(
                        (t) => t.Text == span.GetText());
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
                else if (classification == AntlrLanguage.Constants.ClassificationNameTerminal)
                {
                    var it = details._ant_terminals.Where(
                        (t) => t.Text == span.GetText());
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
            }
            if (!where.Any()) return;

            // Populate the Antlr find results model/window with file/line/col info
            // for each occurrence.
            FindAntlrSymbolsModel.Instance.Results.Clear();
            for (int i = 0; i < where.Count; ++i)
            {
                IToken x = where[i];
                ParserDetails y = where_details[i];
                var w = new Entry() { FileName = y.full_file_name, LineNumber = x.Line, ColumnNumber = x.Column, Token = x };
                FindAntlrSymbolsModel.Instance.Results.Add(w);
            }
        }
    }
}
