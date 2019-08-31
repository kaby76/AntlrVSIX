using System;
using System.Collections.Generic;
using System.Text;

namespace org.antlr.symtab
{
    public class EnumSymbol : SymbolWithScope, Symbol, Type
    {
        public EnumSymbol(string name) : base(name)
        {
        }

        public int TypeIndex { get; }
    }
}
