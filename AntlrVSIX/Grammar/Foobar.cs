﻿namespace AntlrVSIX.Grammar
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using AntlrVSIX.GrammarDescription;
    using Microsoft.VisualStudio.Text;
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using AntlrVSIX.Extensions;

    static class Foobar
    {
        public static IParseTree Find(this SnapshotPoint point)
        {
            ITextBuffer tb = point.Snapshot.TextBuffer;
            string ffn = tb.GetFilePath();
            var item = AntlrVSIX.GrammarDescription.Workspace.Instance.FindProjectFullName(ffn);
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
