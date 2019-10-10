namespace AntlrVSIX.FindAllReferences
{
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using System;
    using System.ComponentModel.Design;

    internal sealed class FindRefsWindowCommand
    {
        private readonly Package package;
        private MenuCommand _menu_item1;
        private MenuCommand _menu_item2;

        private FindRefsWindowCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                {
                    var menuCommandID = new CommandID(new Guid(AntlrVSIX.Constants.guidFindAllReferences), 256);
                    _menu_item1 = new MenuCommand(this.ShowToolWindow, menuCommandID);
                    commandService.AddCommand(_menu_item1);
                }
            }
        }

        public static FindRefsWindowCommand Instance
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
            Instance = new FindRefsWindowCommand(package);
        }

        private void ShowToolWindow(object sender, EventArgs e)
        {
            ToolWindowPane window = this.package.FindToolWindow(typeof(FindRefsWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        public void Show()
        {
            ShowToolWindow(null, null);
       }
    }
}
