namespace LanguageServer
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Atn;
    using Antlr4.Runtime.Misc;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    internal class LASets
    {
        private Parser _parser;
        private readonly AntlrInputStream _input_stream;
        private CommonTokenStream _token_stream;
        private List<IToken> _input;
        private int _cursor;
        private readonly Dictionary<Pair<ATNState, int>, bool> _visited = new Dictionary<Pair<ATNState, int>, bool>();
        private HashSet<ATNState> _stop_states;
        private HashSet<ATNState> _start_states;

        private class Edge
        {
            public ATNState _from;
            public ATNState _to;
            public TransitionType _type;
            public IntervalSet _label;
            public int _index_at_transition;
            public int _index; // Where we are in parse at _to state.
        }


        public LASets()
        {
        }

        public IntervalSet Compute(Parser parser, CommonTokenStream token_stream)
        {
            _input = new List<IToken>();
            _parser = parser;
            _token_stream = token_stream;
            //_cursor = _token_stream.GetTokens().Select(t => t.Text == "." ? t.TokenIndex : 0).Max();
            _stop_states = new HashSet<ATNState>();
            foreach (ATNState s in parser.Atn.ruleToStopState.Select(t => parser.Atn.states[t.stateNumber]))
            {
                _stop_states.Add(s);
            }
            _start_states = new HashSet<ATNState>();
            foreach (ATNState s in parser.Atn.ruleToStartState.Select(t => parser.Atn.states[t.stateNumber]))
            {
                _start_states.Add(s);
            }
            int currentIndex = _token_stream.Index;
            _token_stream.Seek(0);
            int offset = 1;
            while (true)
            {
                IToken token = _token_stream.LT(offset++);
                _input.Add(token);
                if (token.Type == TokenConstants.EOF)
                {
                    break;
                }
                _cursor = token.TokenIndex;
            }
            List<List<Edge>> all_parses = EnterState(null);

            IntervalSet result = new IntervalSet();
            foreach (List<Edge> p in all_parses)
            {
                HashSet<ATNState> set = ComputeSingle(p);
                foreach (ATNState s in set)
                {
                    foreach (Transition t in s.TransitionsArray)
                    {
                        switch (t.TransitionType)
                        {
                            case TransitionType.RULE:
                                break;

                            case TransitionType.PREDICATE:
                                break;

                            case TransitionType.WILDCARD:
                                break;

                            default:
                                if (!t.IsEpsilon)
                                {
                                    result.AddAll(t.Label);
                                }

                                break;
                        }
                    }
                }
            }
            return result;
        }

        private bool CheckPredicate(PredicateTransition transition)
        {
            return transition.Predicate.Eval(_parser, ParserRuleContext.EmptyContext);
        }

        private int entry_value;

        // Step to state and continue parsing input.
        // Returns a list of transitions leading to a state that accepts input.
        private List<List<Edge>> EnterState(Edge t)
        {
            int here = ++entry_value;

            int index_on_transition;
            int token_index;
            ATNState state;
            if (t == null)
            {
                token_index = 0;
                index_on_transition = 0;
                state = _parser.Atn.states[0];
            }
            else
            {
                token_index = t._index;
                index_on_transition = t._index_at_transition;
                state = t._to;
            }
            IToken input_token = _input[token_index];

            //System.Console.Error.WriteLine("Entry " + here
            //                        + " State " + state
            //                        + " tokenIndex " + token_index
            //                        + " " + input_token.Text
            //                        );

            // Upon reaching the cursor, return match.
            bool at_match = input_token.TokenIndex >= _cursor;
            if (at_match)
            {
                //System.Console.Error.Write("Entry " + here
                //                         + " return ");
                List<List<Edge>> res = new List<List<Edge>>() { new List<Edge>() { t } };
                //var str = PrintResult(res);
                //System.Console.Error.WriteLine(str);
                return res;
            }

            if (_visited.ContainsKey(new Pair<ATNState, int>(state, token_index)))
            {
                return null;
            }

            _visited[new Pair<ATNState, int>(state, token_index)] = true;

            List<List<Edge>> result = new List<List<Edge>>();

            if (_stop_states.Contains(state))
            {
                //System.Console.Error.Write("Entry " + here
                //                              + " return ");
                List<List<Edge>> res = new List<List<Edge>>() { new List<Edge>() { t } };
                //var str = PrintResult(res);
                //System.Console.Error.WriteLine(str);
                return res;
            }

            // Search all transitions from state.
            foreach (Transition transition in state.TransitionsArray)
            {
                List<List<Edge>> matches = null;
                switch (transition.TransitionType)
                {
                    case TransitionType.RULE:
                        {
                            RuleTransition rule = (RuleTransition)transition;
                            ATNState sub_state = rule.target;
                            matches = EnterState(new Edge()
                            {
                                _from = state,
                                _to = transition.target,
                                _label = transition.Label,
                                _type = transition.TransitionType,
                                _index = token_index,
                                _index_at_transition = token_index
                            });
                            if (matches != null && matches.Count == 0)
                            {
                                throw new Exception();
                            }

                            if (matches != null)
                            {
                                List<List<Edge>> new_matches = new List<List<Edge>>();
                                foreach (List<Edge> match in matches)
                                {
                                    Edge f = match.First(); // "to" is possibly final state of submachine.
                                    Edge l = match.Last(); // "to" is start state of submachine.
                                    bool is_final = _stop_states.Contains(f._to);
                                    bool is_at_caret = f._index >= _cursor;
                                    if (!is_final)
                                    {
                                        new_matches.Add(match);
                                    }
                                    else
                                    {
                                        List<List<Edge>> xxx = EnterState(new Edge()
                                        {
                                            _from = f._to,
                                            _to = rule.followState,
                                            _label = null,
                                            _type = TransitionType.EPSILON,
                                            _index = f._index,
                                            _index_at_transition = f._index
                                        });
                                        if (xxx != null && xxx.Count == 0)
                                        {
                                            throw new Exception();
                                        }

                                        if (xxx != null)
                                        {
                                            foreach (List<Edge> y in xxx)
                                            {
                                                List<Edge> copy = y.ToList();
                                                foreach (Edge q in match)
                                                {
                                                    copy.Add(q);
                                                }
                                                new_matches.Add(copy);
                                            }

                                        }
                                    }
                                }
                                matches = new_matches;
                            }
                        }
                        break;

                    case TransitionType.PREDICATE:
                        if (CheckPredicate((PredicateTransition)transition))
                        {
                            matches = EnterState(new Edge()
                            {
                                _from = state,
                                _to = transition.target,
                                _label = transition.Label,
                                _type = transition.TransitionType,
                                _index = token_index,
                                _index_at_transition = token_index
                            });
                            if (matches != null && matches.Count == 0)
                            {
                                throw new Exception();
                            }
                        }
                        break;

                    case TransitionType.WILDCARD:
                        matches = EnterState(new Edge()
                        {
                            _from = state,
                            _to = transition.target,
                            _label = transition.Label,
                            _type = transition.TransitionType,
                            _index = token_index + 1,
                            _index_at_transition = token_index
                        });
                        if (matches != null && matches.Count == 0)
                        {
                            throw new Exception();
                        }

                        break;

                    default:
                        if (transition.IsEpsilon)
                        {
                            matches = EnterState(new Edge()
                            {
                                _from = state,
                                _to = transition.target,
                                _label = transition.Label,
                                _type = transition.TransitionType,
                                _index = token_index,
                                _index_at_transition = token_index
                            });
                            if (matches != null && matches.Count == 0)
                            {
                                throw new Exception();
                            }
                        }
                        else
                        {
                            IntervalSet set = transition.Label;
                            if (set != null && set.Count > 0)
                            {
                                if (transition.TransitionType == TransitionType.NOT_SET)
                                {
                                    set = set.Complement(IntervalSet.Of(TokenConstants.MinUserTokenType, _parser.Atn.maxTokenType));
                                }

                                if (set.Contains(input_token.Type))
                                {
                                    matches = EnterState(new Edge()
                                    {
                                        _from = state,
                                        _to = transition.target,
                                        _label = transition.Label,
                                        _type = transition.TransitionType,
                                        _index = token_index + 1,
                                        _index_at_transition = token_index
                                    });
                                    if (matches != null && matches.Count == 0)
                                    {
                                        throw new Exception();
                                    }
                                }
                            }
                        }
                        break;
                }

                if (matches != null)
                {
                    foreach (List<Edge> match in matches)
                    {
                        List<Edge> x = match.ToList();
                        if (t != null)
                        {
                            x.Add(t);
                            Edge prev = null;
                            foreach (Edge z in x)
                            {
                                ATNState ff = z._to;
                                if (prev != null)
                                {
                                    if (prev._from != ff)
                                    {
                                        System.Console.Error.WriteLine("Fail " + PrintSingle(x));
                                        Debug.Assert(false);
                                    }
                                }

                                prev = z;
                            }
                        }
                        result.Add(x);
                    }
                }
            }
            if (result.Count == 0)
            {
                return null;
            }

            //{
            //    System.Console.Error.Write("Entry " + here
            //                                  + " return ");
            //    var str = PrintResult(result);
            //    System.Console.Error.WriteLine(str);
            //}
            return result;
        }

        private HashSet<ATNState> closure(ATNState start)
        {
            HashSet<ATNState> visited = new HashSet<ATNState>();
            Stack<ATNState> stack = new Stack<ATNState>();
            stack.Push(start);
            while (stack.Any())
            {
                ATNState state = stack.Pop();
                if (visited.Contains(state))
                {
                    continue;
                }

                visited.Add(state);
                foreach (Transition transition in state.TransitionsArray)
                {
                    switch (transition.TransitionType)
                    {
                        case TransitionType.RULE:
                            {
                                RuleTransition rule = (RuleTransition)transition;
                                ATNState sub_state = rule.target;
                                HashSet<ATNState> cl = closure(sub_state);
                                if (cl.Where(s => _stop_states.Contains(s) && s.atn == sub_state.atn).Any())
                                {
                                    HashSet<ATNState> cl2 = closure(rule.followState);
                                    cl.UnionWith(cl2);
                                }
                                foreach (ATNState c in cl)
                                {
                                    visited.Add(c);
                                }
                            }
                            break;

                        case TransitionType.PREDICATE:
                            if (CheckPredicate((PredicateTransition)transition))
                            {
                                stack.Push(transition.target);
                            }
                            break;

                        case TransitionType.WILDCARD:
                            break;

                        default:
                            if (transition.IsEpsilon)
                            {
                                stack.Push(transition.target);
                            }

                            break;
                    }
                }
            }
            return visited;
        }

        private HashSet<ATNState> ComputeSingle(List<Edge> parse)
        {
            List<Edge> copy = parse.ToList();
            HashSet<ATNState> result = new HashSet<ATNState>();
            for (; ; )
            {
                if (!copy.Any())
                {
                    break;
                }

                Edge last_transaction = copy.First();
                HashSet<ATNState> c = closure(last_transaction._to);
                foreach (ATNState s in c)
                {
                    result.Add(s);
                    if (_stop_states.Contains(s))
                    {
                    }
                }
                for (; ; )
                {
                    copy.RemoveAt(0);
                    if (!copy.Any())
                    {
                        break;
                    }

                    last_transaction = copy.First();
                    if (_start_states.Contains(last_transaction._from))
                    {
                        copy.RemoveAt(0);
                        if (!copy.Any())
                        {
                            break;
                        }
                    }
                }
            }
            return result;
        }

        private string PrintSingle(List<Edge> parse)
        {
            StringBuilder sb = new StringBuilder();
            List<Edge> q = parse.ToList();
            q.Reverse();
            foreach (Edge t in q)
            {
                string sym = "";
                switch (t._type)
                {
                    case TransitionType.ACTION:
                        sym = "on action (eps)";
                        break;
                    case TransitionType.ATOM:
                        sym = "on " + t._label.ToString() + " ('" + _input[t._index_at_transition].Text + "')";
                        break;
                    case TransitionType.EPSILON:
                        sym = "on eps";
                        break;
                    case TransitionType.INVALID:
                        sym = "invalid (eps)";
                        break;
                    case TransitionType.NOT_SET:
                        sym = "on not " + t._label.ToString();
                        break;
                    case TransitionType.PRECEDENCE:
                        sym = "on prec (eps)";
                        break;
                    case TransitionType.PREDICATE:
                        sym = "on pred (eps)";
                        break;
                    case TransitionType.RANGE:
                        sym = "on " + t._label.ToString() + " ('" + _input[t._index_at_transition].Text + "')";
                        break;
                    case TransitionType.RULE:
                        sym = "on " + _parser.RuleNames[t._to.ruleIndex] + " (eps)";
                        break;
                    case TransitionType.SET:
                        sym = "on " + t._label.ToString() + " ('" + _input[t._index_at_transition].Text + "')";
                        break;
                    case TransitionType.WILDCARD:
                        sym = "on wildcard ('" + _input[t._index_at_transition].Text + "')";
                        break;
                    default:
                        break;
                }
                sb.Append(" / " + t._from + " => " + t._to + " " + sym);
            }
            return sb.ToString();
        }

        private string PrintResult(List<List<Edge>> all_parses)
        {
            StringBuilder sb = new StringBuilder();
            foreach (List<Edge> p in all_parses)
            {
                sb.Append("||| " + PrintSingle(p));
            }
            return sb.ToString();
        }
    }
}
