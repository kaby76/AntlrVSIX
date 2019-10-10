namespace AntlrVSIX.Grammar
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using AntlrVSIX.Extensions;
    using LanguageServer;
    using Microsoft.VisualStudio.Text;

    static class Foobar
    {
        public static IParseTree Find(this SnapshotPoint point)
        {
            ITextBuffer tb = point.Snapshot.TextBuffer;
            string ffn = tb.GetFilePath();
            var item = Workspaces.Workspace.Instance.FindDocumentFullName(ffn);
            var pd = ParserDetailsFactory.Create(item);
            foreach (var node in DFSVisitor.DFS(pd.ParseTree as ParserRuleContext))
            {
                if (node as TerminalNodeImpl == null)
                    continue;
                var leaf = node as TerminalNodeImpl;
                if (leaf.Symbol.StartIndex <= point.Position && point.Position <= leaf.Symbol.StopIndex)
                    return leaf;
            }
            return null;
        }
    }
}
