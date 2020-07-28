using System;
using System.Collections.Generic;
using System.Text;

namespace AntlrTreeEditing.AntlrDOM
{
    interface IAntlrObserver : IObserver<ObserverParserRuleContext>
    {
        void OnParentDisconnect(ObserverParserRuleContext value);
        void OnParentConnect(ObserverParserRuleContext value);
        void OnChildDisconnect(ObserverParserRuleContext value);
        void OnChildConnect(ObserverParserRuleContext value);
    }
}
