namespace AntlrLanguage
{
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("66b4f892-d9b8-42e7-9ee4-c7bdf455fa11")]
    public class FindRefsWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindRefsWindow"/> class.
        /// </summary>
        public FindRefsWindow() : base(null)
        {
            this.Caption = "Find Results of Antlr Symbols";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new FindRefsWindowControl();
        }
    }
}
