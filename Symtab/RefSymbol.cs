using System;

namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;
    using System.Linq;

    // All symbols are either "refs" or "defs". This class implements a ref symbol which
    // contains a pointer to the def. When the symbol is resolved, the def is returned.
    public class RefSymbol : BaseSymbol
    {
        public List<ISymbol> Def { get; protected set; }

        public override List<ISymbol> resolve()
        {
            return Def;
        }

        public RefSymbol(IList<IToken> t, List<ISymbol> def)
            : base(def.First().Name, t)
        {
            Def = def;
            foreach (var d in def)
            {
                var dd = d as BaseSymbol;
                if (dd != null)
                {
                    dd.Refs.Add(this);
                }
            }
        }

        public override bool Equals(object obj)
        {
            return Object.ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
        }

    }
}
