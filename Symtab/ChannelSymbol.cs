using System;
using System.Collections.Generic;
using System.Text;

namespace Symtab
{
    public class ChannelSymbol : BaseSymbol, Symbol
    {
        public ChannelSymbol(string name, int l, int c, string f) : base(name, l, c, f)
        {
        }
    }
}
