using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using Symtab;

namespace AntlrVSIX.GrammarDescription
{
    public interface IGrammarDescription
    {
        string Name { get; }
        ParserDetails CreateParserDetails(Document item);
        System.Type Parser { get; }
        System.Type Lexer { get; }
        void Parse(ParserDetails pd);
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
        bool CanReformat { get; }
        List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>, IParseTree, bool>> Identify { get; }
        List<Func<IGrammarDescription, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>, IParseTree, bool>> IdentifyDefinition { get; }
        bool IsFileType(string ffn);
        string FileExtension { get; }
        string StartRule { get; }
        bool DoErrorSquiggles { get; }
        List<Func<ParserDetails, IParseTree, string>> PopUpDefinition { get; }
    }
}
