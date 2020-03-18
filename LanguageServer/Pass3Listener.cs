namespace LanguageServer
{
    using Antlr4.Runtime.Misc;

    public class Pass3Listener : ANTLRv4ParserBaseListener
    {
        private readonly AntlrGrammarDetails _pd;
        private bool saw_tokenVocab_option = false;
        private enum GrammarType
        {
            Combined,
            Parser,
            Lexer
        }

        private GrammarType Type;

        public Pass3Listener(AntlrGrammarDetails pd)
        {
            _pd = pd;
            if (!AntlrGrammarDetails._dependent_grammars.ContainsKey(_pd.FullFileName))
            {
                AntlrGrammarDetails._dependent_grammars.Add(_pd.FullFileName);
            }
        }

        public override void EnterGrammarType([NotNull] ANTLRv4Parser.GrammarTypeContext context)
        {
            if (context.GetChild(0).GetText() == "parser")
            {
                Type = GrammarType.Parser;
            }
            else if (context.GetChild(0).GetText() == "lexer")
            {
                Type = GrammarType.Lexer;
            }
            else
            {
                Type = GrammarType.Combined;
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
            if (!AntlrGrammarDetails._dependent_grammars.ContainsKey(dep))
            {
                AntlrGrammarDetails._dependent_grammars.Add(dep);
            }

            bool found = false;
            foreach (string f in AntlrGrammarDetails._dependent_grammars[dep])
            {
                if (f == file)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                AntlrGrammarDetails._dependent_grammars.Add(dep, file);
            }
            saw_tokenVocab_option = true;
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
            if (!AntlrGrammarDetails._dependent_grammars.ContainsKey(dep))
            {
                AntlrGrammarDetails._dependent_grammars.Add(dep);
            }

            bool found = false;
            foreach (string f in AntlrGrammarDetails._dependent_grammars[dep])
            {
                if (f == file)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                AntlrGrammarDetails._dependent_grammars.Add(dep, file);
            }
        }

        public override void EnterRules([NotNull] ANTLRv4Parser.RulesContext context)
        {
            if (saw_tokenVocab_option)
                return;

            // We didn't see an option to include lexer grammar.

            if (Type != GrammarType.Parser)
                return;

            // It's a parser grammar, but we didn't see the tokenVocab option for the lexer.
            // We must assume a lexer grammar in this directory.

            string file = _pd.Item.FullPath;
            string dir = System.IO.Path.GetDirectoryName(file);
            string dep = file.Replace("Parser.g4","Lexer.g4");
            _pd.Imports.Add(dep);
            if (!AntlrGrammarDetails._dependent_grammars.ContainsKey(dep))
            {
                AntlrGrammarDetails._dependent_grammars.Add(dep);
            }

            bool found = false;
            foreach (string f in AntlrGrammarDetails._dependent_grammars[dep])
            {
                if (f == file)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                AntlrGrammarDetails._dependent_grammars.Add(dep, file);
            }
        }
    }
}
