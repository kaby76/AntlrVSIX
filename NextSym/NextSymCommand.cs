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
using AntlrVSIX.Navigate;

namespace AntlrVSIX.NextSym
{
    internal sealed class NextSymCommand
    {
        private readonly Package _package;
        private MenuCommand _menu_item1;
        private MenuCommand _menu_item2;

        private NextSymCommand(Package package)
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
                    // Set up hook for context menu.
                    var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidMenuAndCommandsCmdSet), 0x7002);
                    _menu_item1 = new MenuCommand(this.MenuItemCallback, menuCommandID);
                    _menu_item1.Enabled = true;
                    _menu_item1.Visible = true;
                    commandService.AddCommand(_menu_item1);
                }
                {
                    // Set up hook for context menu.
                    var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidMenuAndCommandsCmdSet), 0x7003);
                    _menu_item2 = new MenuCommand(this.MenuItemCallback, menuCommandID);
                    _menu_item2.Enabled = true;
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
                if (_menu_item2 != null) _menu_item2.Visible = value;
            }
        }

        public static NextSymCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return this._package; }
        }

        public static void Initialize(Package package)
        {
            Instance = new NextSymCommand(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            ////////////////////////
            /// Next rule.
            ////////////////////////

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
                            string ffn = (string) prop;
                            if (!ParserDetails._per_file_parser_details.ContainsKey(ffn))
                            {
                                StreamReader sr = new StreamReader(ffn);
                                ParserDetails foo = new ParserDetails();
                                ParserDetails._per_file_parser_details[ffn] = foo;
                                foo.Parse(sr.ReadToEnd(), ffn);
                            }
                        }
                        catch (Exception eeks)
                        {
                        }
                    }
                }

                string classification = AntlrLanguagePackage.Instance.Classification;
                SnapshotSpan span = AntlrLanguagePackage.Instance.Span;
                ITextView view = AntlrLanguagePackage.Instance.View;

                var service = ServiceProvider.GetService(typeof(SVsTextManager));
                var textManager = service as IVsTextManager2;
                IVsTextView view2;
                int result = textManager.GetActiveView2(1, null, (uint)_VIEWFRAMETYPE.vftCodeWindow, out view2);
                var xxx = view2.GetIWpfTextView();
                view = xxx;

                ITextCaret car = view.Caret;
                CaretPosition cp = car.Position;
                SnapshotPoint bp = cp.BufferPosition;
                int pos = bp.Position;

                // First, find out what this view is, and what the file is.
                ITextBuffer buffer = view.TextBuffer;
                ITextDocument doc = buffer.GetTextDocument();
                string path = doc.FilePath;

                //ParserDetails details = null;
                //bool found = ParserDetails._per_file_parser_details.TryGetValue(path, out details);
                //if (!found) return;

                List<IToken> where = new List<IToken>();
                List<ParserDetails> where_details = new List<ParserDetails>();
                int next_sym = Int32.MaxValue;
                foreach (var kvp in ParserDetails._per_file_parser_details)
                {
                    string file_name = kvp.Key;
                    if (file_name != path)
                        continue;

                    ParserDetails details = kvp.Value;
                    foreach (var t in details._ant_nonterminals_defining)
                    {
                        if (t.StartIndex > pos && t.StartIndex < next_sym)
                            next_sym = t.StartIndex;
                    }

                    break;
                }

                if (next_sym == Int32.MaxValue) return;

                string full_file_name = path;
                IVsTextView vstv = IVsTextViewExtensions.GetIVsTextView(full_file_name);
                IVsTextViewExtensions.ShowFrame(full_file_name);

                IWpfTextView wpftv = vstv.GetIWpfTextView();
                if (wpftv == null) return;

                int line_number;
                int colum_number;
                vstv.GetLineAndColumn(next_sym, out line_number, out colum_number);

                // Create new span in the appropriate view.
                ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
                SnapshotSpan ss = new SnapshotSpan(cc, next_sym, 1);
                SnapshotPoint sp = ss.Start;
                // Put cursor on symbol.
                wpftv.Caret.MoveTo(sp); // This sets cursor, bot does not center.
                // Center on cursor.
                //wpftv.Caret.EnsureVisible(); // This works, sort of. It moves the scroll bar, but it does not CENTER! Does not really work!
                if (line_number > 0)
                    vstv.CenterLines(line_number - 1, 2);
                else
                    vstv.CenterLines(line_number, 1);
            }
        }
    }
}
