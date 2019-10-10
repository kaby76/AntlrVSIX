namespace AntlrVSIX.Grammar
{
    using Antlr4.Runtime.Tree;
    using AntlrVSIX.Extensions;
    using Microsoft.VisualStudio.Text;

    static class FindSymbol
    {
        public static IParseTree Find(this SnapshotPoint point)
        {
            ITextBuffer tb = point.Snapshot.TextBuffer;
            string ffn = tb.GetFilePath();
            var item = Workspaces.Workspace.Instance.FindDocument(ffn);
            return LanguageServer.Util.Find(point.Position, item);
        }
    }
}
