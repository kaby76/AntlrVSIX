namespace AntlrLanguage.Navigate
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    //using Microsoft.VisualStudio.Shell.Interop;
    using AntlrLanguage.Extensions;
    using AntlrLanguage;

    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(Constants.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(AntlrLanguage.FindRefsWindow))]
    public sealed class AntlrLanguagePackage : Package
    {
        public AntlrLanguagePackage()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            Command1.Initialize(this);
            Command2.Initialize(this);
            AntlrLanguage.FindRefsWindowCommand.Initialize(this);
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
