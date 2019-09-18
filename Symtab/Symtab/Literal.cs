using System;
using System.Collections.Generic;
using System.Text;

namespace Symtab.Symtab
{
    public class Literal : SymbolWithScope, Symbol
    {
        public Literal(string name) : base(name)
        {
        }

        public int TypeIndex { get; }
    }
}
