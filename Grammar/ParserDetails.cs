
namespace AntlrVSIX.Grammar
{
    using Antlr4.Runtime.Tree;
    using Antlr4.Runtime;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System;

    public class ParserDetails : IObservable<ParserDetails>
    {
        public static Dictionary<string, ParserDetails> _per_file_parser_details =
            new Dictionary<string, ParserDetails>();

        public string FullFileName;
        public string Code;
        private List<IObserver<ParserDetails>> _observers = new List<IObserver<ParserDetails>>();

        // Parser and parse tree.
        private ANTLRv4Parser _ant_parser = null;
        private IParseTree _ant_tree = null;
        private IEnumerable<IParseTree>_all_nodes = null;

        // List of all comments, terminal, nonterminal, and keyword tokens in the grammar.
        public Dictionary<IToken, int> _ant_applied_occurrence_classes = new Dictionary<IToken, int>();
        public Dictionary<IToken, int> _ant_defining_occurrence_classes = new Dictionary<IToken, int>();

        public static ParserDetails Parse(string code, string ffn)
        {
            bool has_entry = _per_file_parser_details.ContainsKey(ffn);
            ParserDetails pd;
            if (!has_entry)
            {
                pd = new ParserDetails();
                _per_file_parser_details[ffn] = pd;
            }
            else
            {
                pd = _per_file_parser_details[ffn];
            }

            bool has_changed = false;
            if (pd.Code == code) return pd;
            else if (pd.Code != null) has_changed = true;

            pd.Code = code;
            pd.FullFileName = ffn;

            // Set up Antlr to parse input grammar.
            byte[] byteArray = Encoding.UTF8.GetBytes(code);
            CommonTokenStream cts = new CommonTokenStream(
                new ANTLRv4Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())));
            pd._ant_parser = new ANTLRv4Parser(cts);

            // Set up another token stream containing comments. This might be
            // problematic as the parser influences the lexer.
            CommonTokenStream cts_off_channel = new CommonTokenStream(
                new ANTLRv4Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            new MemoryStream(byteArray)).ReadToEnd())),
                ANTLRv4Lexer.OFF_CHANNEL);


            try
            {
                pd._ant_tree = pd._ant_parser.grammarSpec();
            }
            catch (Exception e)
            {
                // Parsing error.
            }

            pd._all_nodes = DFSVisitor.DFS(pd._ant_tree as ParserRuleContext);

            //StringBuilder sb = new StringBuilder();
            //Class1.ParenthesizedAST(pd._ant_tree, sb, "", cts);
            //string fn = System.IO.Path.GetFileName(pd.FullFileName);
            //fn = "c:\\temp\\" + fn;
            //System.IO.File.WriteAllText(fn, sb.ToString());

            pd._ant_defining_occurrence_classes = new Dictionary<IToken, int>();
            pd._ant_applied_occurrence_classes = new Dictionary<IToken, int>();

            // Order of finding stuff dependent here. First find defs, then refs.
            for (int classification = 0; classification < AntlrVSIX.Grammar.AntlrToClassifierName.IdentifyDefinition.Count; ++classification)
            {
                var fun = AntlrVSIX.Grammar.AntlrToClassifierName.IdentifyDefinition[classification];
                if (fun == null) continue;
                var it = pd._all_nodes.Where(t => fun(t));
                foreach (var t in it)
                {
                    var itoken = (t as TerminalNodeImpl).Symbol;
                    pd._ant_defining_occurrence_classes.Add(itoken, classification);
                }
            }
            for (int classification = 0; classification < AntlrVSIX.Grammar.AntlrToClassifierName.Identify.Count; ++classification)
            {
                var fun = AntlrVSIX.Grammar.AntlrToClassifierName.Identify[classification];
                if (fun == null) continue;
                var it = pd._all_nodes.Where(t => fun(t));
                foreach (var t in it)
                {
                    var itoken = (t as TerminalNodeImpl).Symbol;
                    pd._ant_applied_occurrence_classes.Add(itoken, classification);
                }
            }

            // Get all comments.
            var new_list = new List<IToken>();
            while (cts_off_channel.LA(1) != ANTLRv4Parser.Eof)
            {
                IToken token = cts_off_channel.LT(1);
                if (token.Type == ANTLRv4Parser.BLOCK_COMMENT
                    || token.Type == ANTLRv4Parser.LINE_COMMENT)
                {
                    new_list.Add(token);
                }
                cts_off_channel.Consume();
            }
           // pd._ant_comments = new_list;

            if (has_changed)
            {
                foreach (var observer in pd._observers)
                {
                    observer.OnNext(pd);
                }
            }

            return pd;
        }

        public IDisposable Subscribe(IObserver<ParserDetails> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<ParserDetails>> _observers;
            private IObserver<ParserDetails> _observer;

            public Unsubscriber(List<IObserver<ParserDetails>> observers, IObserver<ParserDetails> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
