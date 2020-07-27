namespace XPathHelpers
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ObserverParserRuleContext : ParserRuleContext, IObservable<ObserverParserRuleContext>
    {
        private List<IObserver<ObserverParserRuleContext>> observers;

        public ObserverParserRuleContext(ParserRuleContext parent, int invokingState)
            : base(parent, invokingState)
        {
            observers = new List<IObserver<ObserverParserRuleContext>>();
        }

        public ObserverParserRuleContext()
            : base()
        {
            observers = new List<IObserver<ObserverParserRuleContext>>();
        }

        public override void AddChild(ITerminalNode t)
        {
            base.AddChild(t);
        }

        public override ITerminalNode AddChild(IToken matchedToken)
        {
            return base.AddChild(matchedToken);
        }

        public override void AddChild(RuleContext ruleInvocation)
        {
            base.AddChild(ruleInvocation);
        }

        public IDisposable Subscribe(IObserver<ObserverParserRuleContext> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<ObserverParserRuleContext>> _observers;
            private IObserver<ObserverParserRuleContext> _observer;

            public Unsubscriber(List<IObserver<ObserverParserRuleContext>> observers, IObserver<ObserverParserRuleContext> observer)
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

        public void EndTransmission()
        {
            foreach (var observer in observers.ToArray())
                if (observers.Contains(observer))
                    observer.OnCompleted();

            observers.Clear();
        }
    }
}
