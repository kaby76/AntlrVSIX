using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Antlr4.Runtime;
using AntlrVSIX.Extensions;
using AntlrVSIX.Grammar;
using AntlrVSIX.Package;
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
using AntlrVSIX.FindAllReferences;
using AntlrVSIX.GoToDefintion;
using AntlrVSIX.Rename;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;

namespace AntlrVSIX.GoToVisitor
{
    public class GoToVisitorCommand
    {
        private readonly Microsoft.VisualStudio.Shell.Package _package;
        private MenuCommand _menu_item1;
        private MenuCommand _menu_item2;

        public GoToVisitorCommand(Microsoft.VisualStudio.Shell.Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            _package = package;
            OleMenuCommandService commandService = this.ServiceProvider.GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService == null)
            {
                throw new ArgumentNullException("OleMenuCommandService");
            }

            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidMenuAndCommandsCmdSet), 0x7005);
                _menu_item1 = new MenuCommand(this.MenuItemCallbackVisitor, menuCommandID);
                _menu_item1.Enabled = false;
                _menu_item1.Visible = true;
                commandService.AddCommand(_menu_item1);
            }
            {
                // Set up hook for context menu.
                var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidMenuAndCommandsCmdSet), 0x7006);
                _menu_item2 = new MenuCommand(this.MenuItemCallbackListener, menuCommandID);
                _menu_item2.Enabled = false;
                _menu_item2.Visible = true;
                commandService.AddCommand(_menu_item2);
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

        public static GoToVisitorCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return this._package; }
        }

        public static void Initialize(Microsoft.VisualStudio.Shell.Package package)
        {
            Instance = new GoToVisitorCommand(package);
        }

        private void MenuItemCallbackVisitor(object sender, EventArgs e)
        {
            MenuItemCallback(sender, e, true);
        }

        private void MenuItemCallbackListener(object sender, EventArgs e)
        {
            MenuItemCallback(sender, e, false);
        }

        private void MenuItemCallback(object sender, EventArgs e, bool visitor)
        {
            Dictionary<string, SyntaxTree> trees = new Dictionary<string, SyntaxTree>();
            DTE application = DteExtensions.GetApplication();
            if (application == null) return;

            foreach (var item in DteExtensions.SolutionFiles(application))
            {
                string file_name = item.Name;
                if (file_name != null)
                {
                    string prefix = file_name.TrimSuffix(".cs");
                    if (prefix == file_name) continue;

                    try
                    {
                        object prop = item.Properties.Item("FullPath").Value;
                        string ffn = (string)prop;
                        StreamReader sr = new StreamReader(ffn);
                        string code = sr.ReadToEnd();
                        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
                        trees[ffn] = tree;
                    }
                    catch (Exception eeks)
                    { }
                }
            }
            foreach (var item in DteExtensions.SolutionFiles(application))
            {
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
            // Get active view.
            var view = AntlrLanguagePackage.Instance.GetActiveView();
            ITextCaret car = view.Caret;
            CaretPosition cp = car.Position;
            SnapshotPoint bp = cp.BufferPosition;
            int pos = bp.Position;
            ITextBuffer buffer = view.TextBuffer;
            ITextDocument doc = buffer.GetTextDocument();
            string path = doc.FilePath;

            // Find details of the Antlr symbol pointed to.
            TextExtent extent = AntlrVSIX.Package.AntlrLanguagePackage.Instance.Navigator[view].GetExtentOfWord(bp);
            SnapshotSpan span = extent.Span;
            AntlrLanguagePackage.Instance.Span = span;

            //  Now, check for valid classification type.
            ClassificationSpan[] c1 = AntlrVSIX.Package.AntlrLanguagePackage.Instance.Aggregator[view].GetClassificationSpans(span).ToArray();
            foreach (ClassificationSpan classification in c1)
            {
                var cname = classification.ClassificationType.Classification.ToLower();
                if (cname == AntlrVSIX.Constants.ClassificationNameTerminal)
                {
                    AntlrLanguagePackage.Instance.Classification = AntlrVSIX.Constants.ClassificationNameTerminal;
                }
                else if (cname == AntlrVSIX.Constants.ClassificationNameNonterminal)
                {
                    AntlrLanguagePackage.Instance.Classification = AntlrVSIX.Constants.ClassificationNameNonterminal;
                }
                else if (cname == AntlrVSIX.Constants.ClassificationNameLiteral)
                {
                    AntlrLanguagePackage.Instance.Classification = AntlrVSIX.Constants.ClassificationNameLiteral;
                }
            }

            List<IToken> where = new List<IToken>();
            List<ParserDetails> where_details = new List<ParserDetails>();
            IToken token = null;
            foreach (var kvp in ParserDetails._per_file_parser_details)
            {
                string file_name = kvp.Key;
                ParserDetails details = kvp.Value;
                if (AntlrLanguagePackage.Instance.Classification == AntlrVSIX.Constants.ClassificationNameNonterminal)
                {
                    var it = details._ant_nonterminals_defining.Where(
                        (t) => t.Text == span.GetText());
                    where.AddRange(it);
                    foreach (var i in it) where_details.Add(details);
                }
                else if (AntlrLanguagePackage.Instance.Classification == AntlrVSIX.Constants.ClassificationNameTerminal)
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

            // Symbol
            var sym = token.Text;

            // Assumed name...
            var name = Path.GetFileName(doc.FilePath);
            name = Path.GetFileNameWithoutExtension(name);
            var listener_name = name + "BaseListener";
            var visitor_name = name + "BaseVisitor";

            // Find first occurence of visitor for non-terminal pointed to.
            SyntaxTree found = null;
            foreach (var kvp in trees)
            {
                var file_name = kvp.Key;
                var tree = kvp.Value;

                // Look for IParseTreeListener classes.
                // Look for IParseTreeVisitor classes.

                var root = (CompilationUnitSyntax)tree.GetRoot();
                if (root == null) continue;
                foreach (var nm in root.Members)
                {
                    var namespace_member = nm as NamespaceDeclarationSyntax;
                    if (namespace_member == null) continue;
                    foreach (var cm in namespace_member.Members)
                    {
                        var class_member = cm as ClassDeclarationSyntax;
                        if (class_member == null) continue;
                        var bls = class_member.BaseList;
                        if (bls == null) continue;
                        var types = bls.Types;
                        foreach (var type in types)
                        {
                            var s = type.ToString();
                            if (s.ToString() == listener_name ||
                                s.ToString() == visitor_name)
                            {
                                // Found the right class.
                                // Look for enter or exit method for symbol.
                                var to_find = "Enter" + sym;

                                foreach (var me in class_member.Members)
                                {
                                    var method_member = me as MethodDeclarationSyntax;
                                    if (method_member == null) continue;

                                    if (method_member.Identifier.ValueText.ToLower() == to_find.ToLower())
                                    {
                                        // Open to this line in editor.
                                        IVsTextView vstv = IVsTextViewExtensions.GetIVsTextView(file_name);
                                        IVsTextViewExtensions.ShowFrame(file_name);
                                        vstv = IVsTextViewExtensions.GetIVsTextView(file_name);

                                        IWpfTextView wpftv = vstv.GetIWpfTextView();
                                        if (wpftv == null) return;

                                        int line_number;
                                        int colum_number;
                                        var txt_span = method_member.Identifier.Span;
                                        vstv.GetLineAndColumn(txt_span.Start, out line_number, out colum_number);

                                        // Create new span in the appropriate view.
                                        ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
                                        SnapshotSpan ss = new SnapshotSpan(cc, txt_span.Start, txt_span.Length);
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
                        }
                    }
                }
                
                //var firstMember = root.Members[0];
                //var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;
                //var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members[0];
                //var mainDeclaration = (MethodDeclarationSyntax)programDeclaration.Members[0];
                //var argsParameter = mainDeclaration.ParameterList.Parameters[0];
                //var firstParameters = root.DescendantNodes()
                //    .OfType<MethodDeclarationSyntax>()
                //    .Where(methodDeclaration => methodDeclaration.Identifier.ValueText == "Main")
                //    .Select(methodDeclaration => methodDeclaration.ParameterList.Parameters.First());
                //var argsParameter2 = firstParameters.Single();

            }
        }
    }
}
