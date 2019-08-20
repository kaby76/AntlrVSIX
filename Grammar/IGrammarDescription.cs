using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntlrVSIX.Grammar
{
    interface IGrammarDescription
    {
        IParseTree Parse(string ffn);
        string[] Map { get; }
        Dictionary<string, int> InverseMap { get; }
        List<System.Windows.Media.Color> MapColor { get; }
        List<System.Windows.Media.Color> MapInvertedColor { get; }
        List<bool> CanFindAllRefs { get; }
        List<bool> CanRename { get; }
        List<bool> CanGotodef { get; }
        List<bool> CanGotovisitor { get; }
        List<Func<IGrammarDescription, IParseTree, bool>> Identify { get; }
        List<Func<IGrammarDescription, IParseTree, bool>> IdentifyDefinition { get; }
    }
}
