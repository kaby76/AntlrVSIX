using System;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using AntlrVSIX.Extensions;
using AntlrVSIX.FindAllReferences;
using AntlrVSIX.GoToDefintion;
using AntlrVSIX.Rename;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AntlrVSIX.File;
using AntlrVSIX.GoToVisitor;
using AntlrVSIX.Grammar;
using AntlrVSIX.NextSym;
using AntlrVSIX.Options;
using AntlrVSIX.Reformat;
using EnvDTE;

namespace AntlrVSIX.Package
{

    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(Constants.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(FindRefsWindow))]
    public sealed class AntlrLanguagePackage : Microsoft.VisualStudio.Shell.Package
    {
        public AntlrLanguagePackage()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            ParseAllFiles();
            FileSaveLoad.Initialize(this);
            FindAllReferencesCommand.Initialize(this);
            FindRefsWindowCommand.Initialize(this);
            GoToDefinitionCommand.Initialize(this);
            GoToVisitorCommand.Initialize(this);
            NextSymCommand.Initialize(this);
            OptionsCommand.Initialize(this);
            ReformatCommand.Initialize(this);
            RenameCommand.Initialize(this);
        }

        private static AntlrLanguagePackage _instance;
        public static AntlrLanguagePackage Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                _instance = ((Microsoft.VisualStudio.Shell.Interop.IVsShell)GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SVsShell))).LoadPackage<AntlrLanguagePackage>();
                return _instance;
            }
        }

        public ITextView View { get; set; }
        public SnapshotSpan Span { get; set; }
        public string Classification { get; set; }

        public Dictionary<IWpfTextView, IClassifier> Aggregator { get; } = new Dictionary<IWpfTextView, IClassifier>();
        public Dictionary<IWpfTextView, ITextStructureNavigator> Navigator { get; } = new Dictionary<IWpfTextView, ITextStructureNavigator>();
        public Dictionary<IWpfTextView, SVsServiceProvider> ServiceProvider { get; } = new Dictionary<IWpfTextView, SVsServiceProvider>();

        public IWpfTextView GetActiveView()
        {
            IWpfTextView view = null;
            // Look for currently active view. I don't know if the SVsTextManager
            // provider will do this correctly, so I make sure it returns a consistent
            // view with that stored for the provider.
            foreach (var kvp in AntlrVSIX.Package.AntlrLanguagePackage.Instance.ServiceProvider)
            {
                var k = kvp.Key;
                var v = kvp.Value;
                var service = v.GetService(typeof(SVsTextManager));
                var textManager = service as IVsTextManager2;
                IVsTextView view2;
                int result = textManager.GetActiveView2(1, null, (uint)_VIEWFRAMETYPE.vftCodeWindow, out view2);
                var xxx = view2.GetIWpfTextView();
                if (xxx == k)
                {
                    view = xxx;
                    break;
                }
            }
            return view;
        }

        private void ParseAllFiles()
        {
            // First, open up every .g4 file in project and parse.
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
        }
    }
}
