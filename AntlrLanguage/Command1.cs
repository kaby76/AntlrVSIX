using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace AntlrLanguage
{
   internal sealed class Command1
   {
      public const int CommandId = 0x0100;

      public static readonly Guid CommandSet = new Guid("0c1acc31-15ac-417c-86b2-eefdc669e8bf");

      private readonly Package package;

      private Command1(Package package)
      {
         if (package == null)
         {
            throw new ArgumentNullException("package");
         }

         this.package = package;

         OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
         if (commandService != null)
         {
            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
            commandService.AddCommand(menuItem);
         }
      }

      public static Command1 Instance
      {
         get;
         private set;
      }

      private IServiceProvider ServiceProvider
      {
         get
         {
            return this.package;
         }
      }

      public static void Initialize(Package package)
      {
         Instance = new Command1(package);
      }

      private void MenuItemCallback(object sender, EventArgs e)
      {
         string message;
         string title = "Command1";
         EnvDTE.DTE dte;
         EnvDTE.Document activeDocument;

         dte = (EnvDTE.DTE)this.ServiceProvider.GetService(typeof(EnvDTE.DTE));
         activeDocument = dte.ActiveDocument;
         message = $"Called on {activeDocument.FullName}";

         // Show a message box to prove we were here
         VsShellUtilities.ShowMessageBox(
             this.ServiceProvider,
             message,
             title,
             OLEMSGICON.OLEMSGICON_INFO,
             OLEMSGBUTTON.OLEMSGBUTTON_OK,
             OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
      }
   }
}
