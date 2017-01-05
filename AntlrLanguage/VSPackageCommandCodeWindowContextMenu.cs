using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace AntlrLanguage
{
   [PackageRegistration(UseManagedResourcesOnly = true)]
   [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
   [Guid(VSPackageCommandCodeWindowContextMenu.PackageGuidString)]
   [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
   [ProvideMenuResource("Menus.ctmenu", 1)]
   public sealed class VSPackageCommandCodeWindowContextMenu : Package
   {
      public const string PackageGuidString = "7e37eef9-8cbe-4b10-81f7-66413cd2c9d3";

      public VSPackageCommandCodeWindowContextMenu()
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         Command1.Initialize(this);
      }
   }
}
