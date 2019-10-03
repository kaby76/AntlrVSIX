using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;

namespace Symtab
{
    public class ChannelSymbol : BaseSymbol, Symbol
    {
        public ChannelSymbol(string name, IToken token) : base(name, token)
        {
        }
    }
}
