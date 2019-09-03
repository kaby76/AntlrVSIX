
namespace AntlrVSIX.Grammar
{
    using Antlr4.Runtime.Tree;
    using Antlr4.Runtime;
    using AntlrVSIX.GrammarDescription;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System;

    public class ParserDetails : IObservable<ParserDetails>
    {
        public static Dictionary<string, ParserDetails> _per_file_parser_details = new Dictionary<string, ParserDetails>();
        public string FullFileName;
        public string Code;
        public Dictionary<IToken, int> _ant_applied_occurrence_classes = new Dictionary<IToken, int>();
        public Dictionary<IToken, int> _ant_defining_occurrence_classes = new Dictionary<IToken, int>();
        public Dictionary<IToken, int> _ant_comments = new Dictionary<IToken, int>();
        public Dictionary<IParseTree, org.antlr.symtab.Symbol> _ant_symtab = new Dictionary<IParseTree, org.antlr.symtab.Symbol>();
        private List<IObserver<ParserDetails>> _observers = new List<IObserver<ParserDetails>>();
        public IParseTree _ant_tree = null;
        private IEnumerable<IParseTree>_all_nodes = null;

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

            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null) throw new Exception();
            gd.Parse(ffn, code, out pd._ant_tree, out pd._ant_symtab);

            pd._all_nodes = DFSVisitor.DFS(pd._ant_tree as ParserRuleContext);

            pd._ant_defining_occurrence_classes = new Dictionary<IToken, int>();
            pd._ant_applied_occurrence_classes = new Dictionary<IToken, int>();

            // Order of finding stuff dependent here. First find defs, then refs.
            for (int classification = 0; classification < gd.IdentifyDefinition.Count; ++classification)
            {
                var fun = gd.IdentifyDefinition[classification];
                if (fun == null) continue;
                var it = pd._all_nodes.Where(t => fun(gd, t));
                foreach (var t in it)
                {
                    var itoken = (t as TerminalNodeImpl).Symbol;
                    pd._ant_defining_occurrence_classes.Add(itoken, classification);
                }
            }
            for (int classification = 0; classification < gd.Identify.Count; ++classification)
            {
                var fun = gd.Identify[classification];
                if (fun == null) continue;
                var it = pd._all_nodes.Where(t => fun(gd, t));
                foreach (var t in it)
                {
                    var itoken = (t as TerminalNodeImpl).Symbol;
                    pd._ant_applied_occurrence_classes.Add(itoken, classification);
                }
            }

            pd._ant_comments = gd.ExtractComments(code);

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
