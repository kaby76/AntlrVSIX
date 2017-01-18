namespace AntlrVSIX.Navigate
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.FindAllReferences;
    using AntlrVSIX.GoToDefintion;
    using AntlrVSIX.Rename;
    using AntlrVSIX;
    using Microsoft.VisualStudio.Shell;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(Constants.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(FindRefsWindow))]
    public sealed class AntlrLanguagePackage : Package
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
    }
}
