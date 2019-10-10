namespace LanguageServer.Antlr
{
    using Antlr4.Runtime.Tree;
    using System.Text;

    class Reconstruct
    {
        public static void Doit(StringBuilder sb, IParseTree node)
        {
            if (node is TerminalNodeImpl)
            {
                sb.Append(" " + node.GetText());
            }
            else
            {
                for (int i = 0; i < node.ChildCount; ++i)
                {
                    var c = node.GetChild(i);
                    Doit(sb, c);
                }
            }
        }
    }
}
