using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using AntlrVSIX.GrammarDescription;
using Symtab;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;


namespace AntlrVSIX.GrammarDescription.Antlr
{
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
