﻿namespace LanguageServer
{
    using LspAntlr;

    public class CMGetClassifiersParams
    {
        public CMGetClassifiersParams() { }
        public System.Uri TextDocument;
        public int Start;
        public int End;
    }

    public class CMClassifierInformation
    {
        public CMClassifierInformation() { }
        public int start { get; set; }
        public int end { get; set; }
        public int Kind { get; set; }
    }

    public class CMNextSymbolParams
    {
        public CMNextSymbolParams() { }
        public System.Uri TextDocument;
        public int Pos;
        public bool Forward;
    }

    public class CMGotoParams
    {
        public CMGotoParams() { }
        public System.Uri TextDocument;
        public int Pos;
        public bool IsEnter;
    }

    public class CMGotoResult
    {
        public CMGotoResult() { }
        public System.Uri TextDocument;
        public int Start;
    }

    public class CMReplaceLiteralsParams
    {
        public CMReplaceLiteralsParams() { }
        public System.Uri TextDocument;
        public int Pos;
    }

    public class CMRemoveUselessParserProductionsParams
    {
        public CMRemoveUselessParserProductionsParams() { }
        public System.Uri TextDocument;
        public int Pos;
    }

    public class CMMoveStartRuleToTopParams
    {
        public CMMoveStartRuleToTopParams() { }
        public System.Uri TextDocument;
        public int Pos;
    }

    public class CMReorderParserRulesParams
    {
        public CMReorderParserRulesParams() { }
        public System.Uri TextDocument;
        public ReorderType Type;
    }

    public class CMSplitCombineGrammarsParams
    {
        public CMSplitCombineGrammarsParams() { }
        public System.Uri TextDocument;
        public int Pos;
        public bool Split;
    }

    public class CMEliminateDirectLeftRecursionParams
    {
        public CMEliminateDirectLeftRecursionParams() { }
        public System.Uri TextDocument;
        public int Pos;
    }

}
