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

            IEnumerable<ProjectItem> iterator = DteExtensions.SolutionFiles(application);
            ProjectItem[] list = iterator.ToArray();
            foreach (var item in list)
            {
                //var doc = item.Document; CRASHES!!!! DO NOT USE!
                //var props = item.Properties;
                string file_name = item.Name;
                if (file_name != null)
                {
                    string prefix = file_name.TrimSuffix(".cs");
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
                            string code = sr.ReadToEnd();
                            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
                            trees[file_name] = tree;
                        }
                    }
                    catch (Exception eeks)
                    { }
                }
            }

            // Find first occurence of visitor for non-terminal pointed to.
            foreach (var kvp in trees)
            {
                var file_name = kvp.Key;
                var tree = kvp.Value;

                // Look for IParseTreeListener classes.
                // Look for IParseTreeVisitor classes.

                var root = (CompilationUnitSyntax)tree.GetRoot();
                foreach (var nm in root.Members)
                {
                    var namespace_member = (NamespaceDeclarationSyntax)nm;
                    foreach (var cm in namespace_member.Members)
                    {
                        var class_member = (ClassDeclarationSyntax)cm;
                        var bls = class_member.BaseList;
                        var types = bls.Types;
                        foreach (var type in types)
                        {

                        }
                    }
                }
                
                var firstMember = root.Members[0];
                var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;
                var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members[0];
                var mainDeclaration = (MethodDeclarationSyntax)programDeclaration.Members[0];
                var argsParameter = mainDeclaration.ParameterList.Parameters[0];
                var firstParameters = root.DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(methodDeclaration => methodDeclaration.Identifier.ValueText == "Main")
                    .Select(methodDeclaration => methodDeclaration.ParameterList.Parameters.First());
                var argsParameter2 = firstParameters.Single();

            }
        }
    }
}
