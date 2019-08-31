namespace AntlrVSIX.FindAllReferences
{
    using Microsoft.VisualStudio.Shell;
    using System.Runtime.InteropServices;

    [Guid("66b4f892-d9b8-42e7-9ee4-c7bdf455fa11")]
    public class FindRefsWindow : ToolWindowPane
    {
        public FindRefsWindow() : base(null)
        {
            Caption = "Find Results of Antlr Symbols";
            Content = new FindRefsWindowControl();
        }
    }
}
