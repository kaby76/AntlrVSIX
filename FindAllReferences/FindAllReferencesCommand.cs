
namespace AntlrVSIX.FindAllReferences
{
    using Antlr4.Runtime;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.GrammarDescription;
    using AntlrVSIX.Model;
    using AntlrVSIX.Package;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Linq;

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
                    var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7004);
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

            string classification = AntlrLanguagePackage.Instance.Classification;
            SnapshotSpan span = AntlrLanguagePackage.Instance.Span;
            ITextView view = AntlrLanguagePackage.Instance.View;
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            string path = doc.FilePath;
            IGrammarDescription grammar_description = GrammarDescriptionFactory.Create(path);
            List<IToken> where = new List<IToken>();
            List<ParserDetails> where_details = new List<ParserDetails>();
            foreach (var kvp in ParserDetails._per_file_parser_details)
            {
                string file_name = kvp.Key;
                ParserDetails details = kvp.Value;
                {
                    var it = details._ant_applied_occurrence_classes.Where(
                        (t) => grammar_description.CanFindAllRefs[t.Value]
                            && t.Key.Text == span.GetText()).Select(t => t.Key);
                    where.AddRange(it);
                    foreach (var j in it) where_details.Add(details);
                }
                {
                    var it = details._ant_defining_occurrence_classes.Where(
                        (t) => grammar_description.CanFindAllRefs[t.Value]
                            && t.Key.Text == span.GetText()).Select(t => t.Key);
                    where.AddRange(it);
                    foreach (var j in it) where_details.Add(details);
                }
            }
            if (!where.Any()) return;
            FindAntlrSymbolsModel.Instance.Results.Clear();
            for (int i = 0; i < where.Count; ++i)
            {
                IToken x = where[i];
                ParserDetails y = where_details[i];
                var w = new Entry() { FileName = y.FullFileName, LineNumber = x.Line, ColumnNumber = x.Column, Token = x };
                FindAntlrSymbolsModel.Instance.Results.Add(w);
            }
            FindRefsWindowCommand.Instance.Show();
        }
    }
}
