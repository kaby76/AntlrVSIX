using System;
using System.Collections.Generic;
using System.Text;

namespace Symtab
{
    public class EnumSymbol : SymbolWithScope, Symbol, TypeReference
    {
        public EnumSymbol(string name) : base(name)
        {
        }

        public int TypeIndex { get; }
    }
}
