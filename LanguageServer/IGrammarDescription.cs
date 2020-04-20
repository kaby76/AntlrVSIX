namespace LanguageServer
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using System;
    using System.Collections.Generic;

    public interface IGrammarDescription
    {
        string Name { get; }
        ParserDetails CreateParserDetails(Workspaces.Document item);
        System.Type Parser { get; }
        System.Type Lexer { get; }
        void Parse(ParserDetails pd);
        void Parse(string code,
            out CommonTokenStream TokStream,
            out Parser Parser,
            out Lexer Lexer,
            out IParseTree ParseTree);
        Dictionary<IToken, int> ExtractComments(string code);
        bool CanNextRule { get; }
        string[] Map { get; }
        List<bool> CanFindAllRefs { get; }
        List<bool> CanRename { get; }
        List<bool> CanGotodef { get; }
        List<bool> CanGotovisitor { get; }
        bool CanReformat { get; }
        Func<IGrammarDescription, Dictionary<IParseTree, IList<Symtab.CombinedScopeSymbol>>, IParseTree, int> Classify { get; }
        bool IsFileType(string ffn);
        string FileExtension { get; }
        string StartRule { get; }
        bool DoErrorSquiggles { get; }
        List<Func<ParserDetails, IParseTree, string>> PopUpDefinition { get; }

    }
}
