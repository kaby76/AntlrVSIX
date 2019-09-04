using System;
using System.Collections.Generic;
using System.Text;

namespace Symtab
{
    public class EnumSymbol : SymbolWithScope, Symbol, Type
    {
        public Mono.Cecil.MemberReference MonoType { get; set; }
        public EnumSymbol(string name) : base(name)
        {
        }

        public int TypeIndex { get; }
    }
}
