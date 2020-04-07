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

        public RefSymbol(IToken t, List<ISymbol> def)
            : base(def.First().Name, t)
        {
            Def = def;
        }
    }
}
