
namespace AntlrVSIX.Package
{
    using AntlrVSIX.About;
    using AntlrVSIX.Extensions;
    using AntlrVSIX.FindAllReferences;
    using AntlrVSIX.GoToDefinition;
    using AntlrVSIX.GoToVisitor;
    using AntlrVSIX.Grammar;
    using AntlrVSIX.NextSym;
    using AntlrVSIX.Options;
    using AntlrVSIX.Reformat;
    using AntlrVSIX.Rename;
    using EnvDTE;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System;
    using Microsoft.VisualStudio.Shell.Interop;

    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(AntlrVSIX.Constants.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(FindRefsWindow))]
    public sealed class AntlrLanguagePackage : AsyncPackage
    {
        public AntlrLanguagePackage()
        {
        }

        private IVsSolution solution;
        private uint solutionEventsCookie;

        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await base.InitializeAsync(cancellationToken, progress);

            // Attach to solution events
            solution = GetService(typeof(SVsSolution)) as IVsSolution;
            //if (solution == null) Common.Log("Could not get solution");
            var re = solution.AdviseSolutionEvents(new SolutionEventListener(), out solutionEventsCookie);

            FindAllReferencesCommand.Initialize(this);
            FindRefsWindowCommand.Initialize(this);
            GoToDefinitionCommand.Initialize(this);
            GoToVisitorCommand.Initialize(this);
            NextSymCommand.Initialize(this);
            OptionsCommand.Initialize(this);
            ReformatCommand.Initialize(this);
            RenameCommand.Initialize(this);
            AboutCommand.Initialize(this);
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
