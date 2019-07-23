using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using AntlrVSIX.Extensions;
using AntlrVSIX.FindAllReferences;
using AntlrVSIX.GoToDefintion;
using AntlrVSIX.Rename;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using AntlrVSIX.Extensions;
using AntlrVSIX.FindAllReferences;
using AntlrVSIX.GoToDefintion;
using AntlrVSIX.Rename;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using AntlrVSIX.NextSym;
using AntlrVSIX.Keyboard;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;
using System.Collections.Generic;

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
            GoToDefinitionCommand.Initialize(this);
            FindAllReferencesCommand.Initialize(this);
            RenameCommand.Initialize(this);
            FindRefsWindowCommand.Initialize(this);
            Reformat.ReformatCommand.Initialize(this);
            AntlrVSIX.NextSym.NextSymCommand.Initialize(this);
            AntlrVSIX.GoToVisitor.GoToVisitorCommand.Initialize(this);
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
    }
}
