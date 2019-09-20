
namespace AntlrVSIX.Grammar
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using AntlrVSIX.GrammarDescription;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ParserDetails : IObservable<ParserDetails>
    {
        public static Dictionary<string, ParserDetails> _per_file_parser_details = new Dictionary<string, ParserDetails>();
        public string FullFileName;
        public string Code;
        public Dictionary<TerminalNodeImpl, int> _ant_applied_occurrence_classes = new Dictionary<TerminalNodeImpl, int>();
        public Dictionary<TerminalNodeImpl, int> _ant_defining_occurrence_classes = new Dictionary<TerminalNodeImpl, int>();
        public Dictionary<IToken, int> _ant_comments = new Dictionary<IToken, int>();
        public Dictionary<IParseTree, Symtab.CombinedScopeSymbol> _ant_symtab = new Dictionary<IParseTree, Symtab.CombinedScopeSymbol>();
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

            pd._ant_defining_occurrence_classes = new Dictionary<TerminalNodeImpl, int>();
            pd._ant_applied_occurrence_classes = new Dictionary<TerminalNodeImpl, int>();

            // Order of finding stuff dependent here. First find defs, then refs.
            for (int classification = 0; classification < gd.IdentifyDefinition.Count; ++classification)
            {
                var fun = gd.IdentifyDefinition[classification];
                if (fun == null) continue;
                var it = pd._all_nodes.Where(t => fun(gd, pd._ant_symtab, t));
                foreach (var t in it)
                {
                    var x = (t as TerminalNodeImpl);
                    try
                    {
                        pd._ant_defining_occurrence_classes.Add(x, classification);
                    }
                    catch (ArgumentException e)
                    {
                        // Duplicate
                    }
                }
            }
            for (int classification = 0; classification < gd.Identify.Count; ++classification)
            {
                var fun = gd.Identify[classification];
                if (fun == null) continue;
                var it = pd._all_nodes.Where(t => fun(gd, pd._ant_symtab, t));
                foreach (var t in it)
                {
                    var x = (t as TerminalNodeImpl);
                    try
                    {
                        pd._ant_applied_occurrence_classes.Add(x, classification);
                    }
                    catch (ArgumentException e)
                    {
                        // Duplicate
                    }
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
