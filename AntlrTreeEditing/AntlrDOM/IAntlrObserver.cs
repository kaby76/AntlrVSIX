using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntlrTreeEditing.AntlrDOM
{
    public interface IAntlrObserver : IObserver<ObserverParserRuleContext>
    {
        void OnParentDisconnect(ObserverParserRuleContext value);
        void OnParentConnect(ObserverParserRuleContext value);
        void OnChildDisconnect(ObserverParserRuleContext value);
        void OnChildConnect(ObserverParserRuleContext value);
        void OnChildConnect(ITerminalNode value);
    }
}
