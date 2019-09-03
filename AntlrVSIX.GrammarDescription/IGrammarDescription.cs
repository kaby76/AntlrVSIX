using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using org.antlr.symtab;

namespace AntlrVSIX.GrammarDescription
{
    public interface IGrammarDescription
    {
        void Parse(string ffn, string code, out IParseTree parse_tree, out Dictionary<IParseTree, Symbol> symbols);
        Dictionary<IToken, int> ExtractComments(string code);
        bool CanNextRule { get; }
        string[] Map { get; }
        Dictionary<string, int> InverseMap { get; }
        List<System.Windows.Media.Color> MapColor { get; }
        List<System.Windows.Media.Color> MapInvertedColor { get; }
        List<bool> CanFindAllRefs { get; }
        List<bool> CanRename { get; }
        List<bool> CanGotodef { get; }
        List<bool> CanGotovisitor { get; }
        List<Func<IGrammarDescription, Dictionary<IParseTree, org.antlr.symtab.Symbol>, IParseTree, bool>> Identify { get; }
        List<Func<IGrammarDescription, Dictionary<IParseTree, org.antlr.symtab.Symbol>, IParseTree, bool>> IdentifyDefinition { get; }
        bool IsFileType(string ffn);
        bool DoErrorSquiggles { get; }
    }
}
