using System;
using System.Collections.Generic;
using System.Text;

namespace Symtab
{
    // All symbols are either "refs" or "defs". This class implements a ref symbol which
    // contains a pointer to the def. When the symbol is resolved, the def is returned.
    public class RefSymbol : CombinedScopeSymbol, Symbol
    {
        public Symbol Def { get; protected set; }

        public string Name => Def.Name;

        public Scope Scope { get => Def.Scope; set => Def.Scope = value; }
        public int InsertionOrderNumber { get => Def.InsertionOrderNumber; set => Def.InsertionOrderNumber = value; }

        public virtual Symbol resolve()
        {
            return this.Def;
        }

        public RefSymbol(Symbol def)
        {
            Def = def;
        }
    }
}
