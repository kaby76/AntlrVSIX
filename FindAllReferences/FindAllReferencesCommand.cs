using AntlrVSIX.Navigate;

namespace AntlrVSIX.FindAllReferences
{
    using Antlr4.Runtime;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.Model;
    using EnvDTE;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Linq;
    using System;

    internal sealed class FindAllReferencesCommand
    {
        private readonly Package _package;
        private MenuCommand _menu_item1;
        private MenuCommand _menu_item2;


        private FindAllReferencesCommand(Package package)
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
                {
                    var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x0101);
                    _menu_item1 = new MenuCommand(this.MenuItemCallback, menuCommandID);
                    _menu_item1.Enabled = false;
                    _menu_item1.Visible = false;
                    commandService.AddCommand(_menu_item1);
                }
                {
                    var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidMenuAndCommandsCmdSet), 0x7004);
                    _menu_item2 = new MenuCommand(this.MenuItemCallback, menuCommandID);
                    _menu_item2.Enabled = false;
                    _menu_item2.Visible = true;
                    commandService.AddCommand(_menu_item2);
                }
            }
        }

        public bool Enabled
        {
            set
            {
                if (_menu_item1 != null) _menu_item1.Enabled = value;
                if (_menu_item2 != null) _menu_item2.Enabled = value;
            }
        }

        public bool Visible
        {
            set
            {
                if (_menu_item1 != null) _menu_item1.Visible = value;
                //    if (_menu_item2 != null) _menu_item2.Visible = value;
            }
        }

        public static FindAllReferencesCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return this._package; }
        }

        public static void Initialize(Package package)
        {
            Instance = new FindAllReferencesCommand(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            ////////////////////////
            // Find all references..
            ////////////////////////

            // First, open up every .g4 file in project and parse.
            DTE application = DteExtensions.GetApplication();
            if (application != null)
            {
                IEnumerable<ProjectItem> iterator = DteExtensions.SolutionFiles(application);
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

            string classification = AntlrLanguagePackage.Instance.Classification;
            SnapshotSpan span = AntlrLanguagePackage.Instance.Symbol;
            ITextView view = AntlrLanguagePackage.Instance.View;

            // First, find out what this view is, and what the file is.
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            string path = doc.FilePath;

            List<IToken> where = new List<IToken>();
            List<ParserDetails> where_details = new List<ParserDetails>();
            foreach (var kvp in ParserDetails._per_file_parser_details)
            {
                string file_name = kvp.Key;
                ParserDetails details = kvp.Value;
                if (classification == AntlrVSIX.Constants.ClassificationNameNonterminal)
                {
                    var it = details._ant_nonterminals.Where(
                        (t) => t.Text == span.GetText());
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
                else if (classification == AntlrVSIX.Constants.ClassificationNameTerminal)
                {
                    var it = details._ant_terminals.Where(
                        (t) => t.Text == span.GetText());
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
                else if (classification == AntlrVSIX.Constants.ClassificationNameLiteral)
                {
                    var it = details._ant_literals.Where(
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
