namespace AntlrVSIX.Grammar
{
    using Antlr4.Runtime.Tree;
    using Antlr4.Runtime;
    using AntlrVSIX.Tagger;
    using System.Collections.Generic;
    using System;

    class AntlrToClassifierName
    {
        public static List<string> Map = new List<string>()
        {
            AntlrVSIX.Constants.ClassificationNameNonterminal,
            AntlrVSIX.Constants.ClassificationNameTerminal,
            AntlrVSIX.Constants.ClassificationNameComment,
            AntlrVSIX.Constants.ClassificationNameKeyword,
            AntlrVSIX.Constants.ClassificationNameLiteral,
            AntlrVSIX.Constants.ClassificationNameMode,
            AntlrVSIX.Constants.ClassificationNameChannel,
        };
        public static Dictionary<string, int> InverseMap = new Dictionary<string, int>()
        {
            { AntlrVSIX.Constants.ClassificationNameNonterminal, 0 },
            { AntlrVSIX.Constants.ClassificationNameTerminal, 1 },
            { AntlrVSIX.Constants.ClassificationNameComment, 2 },
            { AntlrVSIX.Constants.ClassificationNameKeyword, 3 },
            { AntlrVSIX.Constants.ClassificationNameLiteral, 4 },
            { AntlrVSIX.Constants.ClassificationNameMode, 5 },
            { AntlrVSIX.Constants.ClassificationNameChannel, 6 },
        };
        public static List<System.Windows.Media.Color> MapColor = new List<System.Windows.Media.Color>()
        {
            AntlrVSIX.Constants.NormalColorTextForegroundNonterminal,
            AntlrVSIX.Constants.NormalColorTextForegroundTerminal,
            AntlrVSIX.Constants.NormalColorTextForegroundComment,
            AntlrVSIX.Constants.NormalColorTextForegroundKeyword,
            AntlrVSIX.Constants.NormalColorTextForegroundLiteral,
            AntlrVSIX.Constants.NormalColorTextForegroundMode,
            AntlrVSIX.Constants.NormalColorTextForegroundChannel,
        };
        public static List<System.Windows.Media.Color> MapInvertedColor = new List<System.Windows.Media.Color>()
        {
            AntlrVSIX.Constants.InvertedColorTextForegroundNonterminal,
            AntlrVSIX.Constants.InvertedColorTextForegroundTerminal,
            AntlrVSIX.Constants.InvertedColorTextForegroundComment,
            AntlrVSIX.Constants.InvertedColorTextForegroundKeyword,
            AntlrVSIX.Constants.InvertedColorTextForegroundLiteral,
            AntlrVSIX.Constants.InvertedColorTextForegroundMode,
            AntlrVSIX.Constants.InvertedColorTextForegroundChannel,
        };
        public static List<Func<IParseTree, bool>> MapTagPredicates = new List<Func<IParseTree, bool>>();
        public static List<bool> CanFindAllRefs = new List<bool>()
        {
            true, // nonterminal
            true, // Terminal
            false, // comment
            false, // keyword
            true, // literal
            true, // mode
            true, // channel
        };
        public static List<bool> CanRename = new List<bool>()
        {
            true, // nonterminal
            true, // Terminal
            false, // comment
            false, // keyword
            false, // literal
            true, // mode
            true, // channel
        };
        public static List<bool> CanGotodef = new List<bool>()
        {
            true, // nonterminal
            true, // Terminal
            false, // comment
            false, // keyword
            false, // literal
            true, // mode
            true, // channel
        };
        public static List<bool> CanGotovisitor = new List<bool>()
        {
            true, // nonterminal
            false, // Terminal
            false, // comment
            false, // keyword
            false, // literal
            false, // mode
            false, // channel
        };

        private static List<string> _antlr_keywords = new List<string>()
        {
            "options",
            "tokens",
            "channels",
            "import",
            "fragment",
            "lexer",
            "parser",
            "grammar",
            "protected",
            "public",
            "returns",
            "locals",
            "throws",
            "catch",
            "finally",
            "mode",
            "pushMode",
            "popMode",
            "type",
            "skip",
            "channel"
        };

        public static List<Func<IParseTree, bool>> Identify = new List<Func<IParseTree, bool>>()
        {
            (IParseTree n) => // nonterminal = 0
                {
                    TerminalNodeImpl term = n as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    // Make sure it's not a def.
                    var is_def = IdentifyDefinition[0](term);
                    if (is_def) return false;
                    if (_antlr_keywords.Contains(text)) return false;
                    if (n.Parent as ANTLRv4Parser.RulerefContext != null &&
                        term?.Symbol.Type == ANTLRv4Parser.RULE_REF)
                        return true;
                    if (n.Parent as ANTLRv4Parser.ActionBlockContext != null)
                        return false;
                    if (n.Parent as ANTLRv4Parser.ParserRuleSpecContext != null &&
                        term?.Symbol.Type == ANTLRv4Parser.RULE_REF)
                        return true;
                    return false;
                },
            (IParseTree n) => // terminal = 1
                {
                    TerminalNodeImpl term = n as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (!Char.IsUpper(text[0])) return false;
                    // Make sure it's not a def.
                    var is_def = IdentifyDefinition[1](term);
                    if (is_def) return false;
                       // special case--channels get their own classification.
                    var is_channel = IdentifyDefinition[6](term);
                    if (is_channel) return false;
                    if (Identify[5](term)) return false;
                    if (Identify[6](term)) return false;
                    if (term.Parent as ANTLRv4Parser.TerminalContext != null)
                        return true;
                    if (term.Parent as ANTLRv4Parser.LexerRuleSpecContext != null)
                        return true;
                    if (term.Parent?.Parent as ANTLRv4Parser.LexerCommandExprContext != null)
                        return true;
                    return false;
                },
            null, // comment = 2
            (IParseTree t) => // keyword = 3
                {
                    TerminalNodeImpl nonterm = t as TerminalNodeImpl;
                    if (nonterm == null) return false;
                    var text = nonterm.GetText();
                    if (!_antlr_keywords.Contains(text)) return false;
                    return true;
                },
            (IParseTree t) => // literal = 4
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    // Chicken/egg problem. Assume that literals are marked
                    // with the appropriate token type.
                    if (term.Symbol == null) return false;
                    if (!(term.Symbol.Type == ANTLRv4Parser.STRING_LITERAL
                        || term.Symbol.Type == ANTLRv4Parser.INT
                        || term.Symbol.Type == ANTLRv4Parser.LEXER_CHAR_SET))
                        return false;

                    // The token must be part of parserRuleSpec context.
                    for (var p = term.Parent; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.ParserRuleSpecContext ||
                            p is ANTLRv4Parser.LexerRuleSpecContext) return true;
                    }
                    return false;
                },
            (IParseTree t) => // mode = 5
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_antlr_keywords.Contains(text)) return false;
                    return false;
                },
            (IParseTree t) => // channel = 6
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_antlr_keywords.Contains(text)) return false;
                    // The channel def must be part of channelsSpec context.
                    for (var p = term.Parent; p != null; p = p.Parent)
                    {
                        if (p is ANTLRv4Parser.ChannelsSpecContext) return true;
                    }
                    return false;
                }
        };

        public static List<Func<IParseTree, bool>> IdentifyDefinition = new List<Func<IParseTree, bool>>()
        {
            (IParseTree t) => // nonterminal
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    if (term.Symbol.Type != ANTLRv4Parser.RULE_REF)
                        return false;
                    var text = term.GetText();
                    if (_antlr_keywords.Contains(text)) return false;
                    IRuleNode parent = term.Parent;
                    for (int i = 0; i < parent.ChildCount; ++i)
                    {
                        if (parent.GetChild(i) == term &&
                            i + 1 < parent.ChildCount &&
                            parent.GetChild(i + 1).GetText() == ":")
                            return true;
                    }
                    return false;
                },
            (IParseTree n) => // terminal
                {
                    TerminalNodeImpl term = n as TerminalNodeImpl;
                    if (term == null) return false;
                    if (term.Symbol.Type != ANTLRv4Parser.TOKEN_REF)
                        return false;
                    var text = term.GetText();
                    if (_antlr_keywords.Contains(text)) return false;
                    IRuleNode parent = term.Parent;
                    for (int i = 0; i < parent.ChildCount; ++i)
                    {
                        if (parent.GetChild(i) == term &&
                            i + 1 < parent.ChildCount &&
                            parent.GetChild(i + 1).GetText() == ":")
                            return true;
                    }
                    return false;
                },
            null, // comment
            null, // keyword
            null, // literal
            (IParseTree n) => // mode
                {
                    TerminalNodeImpl term = n as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_antlr_keywords.Contains(text)) return false;
                    if (!Char.IsUpper(text[0])) return false;
                    IRuleNode parent = term.Parent;
                    while (parent != null)
                    {
                        if (parent as ANTLRv4Parser.LexerRuleSpecContext != null)
                            return false;
                        if (parent as ANTLRv4Parser.ModeSpecContext != null)
                            return true;
                        parent = parent.Parent;
                    }
                    return false;
                },
            (IParseTree t) => // channel
                {
                    TerminalNodeImpl term = t as TerminalNodeImpl;
                    if (term == null) return false;
                    var text = term.GetText();
                    if (_antlr_keywords.Contains(text)) return false;
                    if (!Char.IsUpper(text[0])) return false;
                    IRuleNode parent = term.Parent;
                    while (parent != null)
                    {
                        if (parent as ANTLRv4Parser.LexerRuleSpecContext != null)
                            return false;
                        if (parent as ANTLRv4Parser.ChannelsSpecContext != null)
                            return true;
                        parent = parent.Parent;
                    }
                    return false;
                }
        };
    }
}
