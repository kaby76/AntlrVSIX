using System;
using System.Collections.Generic;
using System.Text;

namespace Symtab
{
    // All symbols are either "refs" or "defs". This class implements a ref symbol which
    // contains a pointer to the def. When the symbol is resolved, the def is returned.
    public class RefSymbol : BaseSymbol
    {
        public Symbol Def { get; protected set; }

        public override Symbol resolve()
        {
            return this.Def;
        }

        public RefSymbol(Symbol def)
            : base(def.Name, def.Token)
        {
            Def = def;
        }
    }
}
