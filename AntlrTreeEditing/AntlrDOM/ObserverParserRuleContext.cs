namespace AntlrTreeEditing.AntlrDOM
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using System;
    using System.Collections.Generic;

    public class ObserverParserRuleContext : ParserRuleContext, IAntlrObservable
    {
        private List<IAntlrObserver> observers;

        public ObserverParserRuleContext(ParserRuleContext parent, int invokingState)
            : base(parent, invokingState)
        {
            observers = new List<IAntlrObserver>();
        }

        public ObserverParserRuleContext()
            : base()
        {
            observers = new List<IAntlrObserver>();
        }

        public override void AddChild(ITerminalNode t)
        {
            base.AddChild(t);

            this.NotifyAddChild(t);
            var o = t as ObserverParserRuleContext;
            if (o != null)
            {
                o.NotifyAddParent(this);
            }
        }

        public override ITerminalNode AddChild(IToken matchedToken)
        {
            return base.AddChild(matchedToken);
        }

        public override void AddChild(RuleContext ruleInvocation)
        {
            base.AddChild(ruleInvocation);
        }


        public virtual void RemoveChild(ITerminalNode t)
        {
        }

        public virtual void RemoveChild(IToken matchedToken)
        {
        }

        public virtual void RemoveChild(RuleContext ruleInvocation)
        {
        }

        public virtual void ReplaceChild(IParseTree t)
        {
        }


        public override RuleContext Parent
        {
            get
            {
                return base.Parent;
            }
            set
            {
                base.Parent = value;
                var o = value as ObserverParserRuleContext;
                if (o != null)
                {
                    o.NotifyAddParent(this);
                    this.NotifyAddChild(o);
                }
            }
        }

        public IDisposable Subscribe(IAntlrObserver observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IAntlrObserver> _observers;
            private IAntlrObserver _observer;

            public Unsubscriber(List<IAntlrObserver> observers, IAntlrObserver observer)
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

        public void SendMessage(ObserverParserRuleContext loc)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(loc);
            }
        }

        public void NotifyAddParent(ObserverParserRuleContext loc)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(loc);
            }
        }

        public void NotifyRemoveParent(ObserverParserRuleContext loc)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(loc);
            }
        }

        public void NotifyAddChild(ObserverParserRuleContext loc)
        {
            foreach (var observer in observers)
            {
                observer.OnChildConnect(loc);
            }
        }

        public void NotifyAddChild(ITerminalNode loc)
        {
            foreach (var observer in observers)
            {
                observer.OnChildConnect(loc);
            }
        }

        public void NotifyRemoveChild(ObserverParserRuleContext loc)
        {
            foreach (var observer in observers)
            {
                observer.OnChildDisconnect(loc);
            }
        }

        public void EndTransmission()
        {
            foreach (var observer in observers.ToArray())
                if (observers.Contains(observer))
                    observer.OnCompleted();

            observers.Clear();
        }

        public IDisposable Subscribe(IObserver<ObserverParserRuleContext> observer)
        {
            throw new NotImplementedException();
        }
    }
}
