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
    }
}
