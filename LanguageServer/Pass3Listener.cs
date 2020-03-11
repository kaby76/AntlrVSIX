﻿namespace LanguageServer
{
    using Antlr4.Runtime.Misc;

    public class Pass3Listener : ANTLRv4ParserBaseListener
    {
        private readonly AntlrParserDetails _pd;

        public Pass3Listener(AntlrParserDetails pd)
        {
            _pd = pd;
            if (!AntlrParserDetails._dependent_grammars.ContainsKey(_pd.FullFileName))
            {
                AntlrParserDetails._dependent_grammars.Add(_pd.FullFileName);
            }
        }

        public override void EnterOption([NotNull] ANTLRv4Parser.OptionContext context)
        {
            if (context.ChildCount < 3)
            {
                return;
            }

            if (context.GetChild(0) == null)
            {
                return;
            }

            if (context.GetChild(0).GetText() != "tokenVocab")
            {
                return;
            }

            string dep_grammar = context.GetChild(2).GetText();
            string file = _pd.Item.FullPath;
            string dir = System.IO.Path.GetDirectoryName(file);
            string dep = dir + System.IO.Path.DirectorySeparatorChar + dep_grammar + ".g4";
            dep = Workspaces.Util.GetProperFilePathCapitalization(dep);
            if (dep == null)
            {
                return;
            }

            _pd.Imports.Add(dep);
            if (!AntlrParserDetails._dependent_grammars.ContainsKey(dep))
            {
                AntlrParserDetails._dependent_grammars.Add(dep);
            }

            bool found = false;
            foreach (string f in AntlrParserDetails._dependent_grammars[dep])
            {
                if (f == file)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                AntlrParserDetails._dependent_grammars.Add(dep, file);
            }
        }

        public override void EnterDelegateGrammar([NotNull] ANTLRv4Parser.DelegateGrammarContext context)
        {
            if (context.ChildCount < 1)
            {
                return;
            }

            if (context.GetChild(0) == null)
            {
                return;
            }

            string dep_grammar = context.GetChild(0).GetText();
            string file = _pd.Item.FullPath;
            string dir = System.IO.Path.GetDirectoryName(file);
            string dep = dir + System.IO.Path.DirectorySeparatorChar + dep_grammar + ".g4";
            dep = Workspaces.Util.GetProperFilePathCapitalization(dep);
            if (dep == null)
            {
                return;
            }

            _pd.Imports.Add(dep);
            if (!AntlrParserDetails._dependent_grammars.ContainsKey(dep))
            {
                AntlrParserDetails._dependent_grammars.Add(dep);
            }

            bool found = false;
            foreach (string f in AntlrParserDetails._dependent_grammars[dep])
            {
                if (f == file)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                AntlrParserDetails._dependent_grammars.Add(dep, file);
            }
        }
    }
}