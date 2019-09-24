using System;
using System.Collections.Generic;
using System.Text;

namespace Symtab
{
    public class ModeSymbol : BaseSymbol, Symbol
    {
        public ModeSymbol(string n, int l, int c, string f) : base(n, l, c, f)
        {
        }
    }
}
