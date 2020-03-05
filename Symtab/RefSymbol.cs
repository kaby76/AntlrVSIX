namespace Symtab
{
    using Antlr4.Runtime;

    // All symbols are either "refs" or "defs". This class implements a ref symbol which
    // contains a pointer to the def. When the symbol is resolved, the def is returned.
    public class RefSymbol : BaseSymbol
    {
        public ISymbol Def { get; protected set; }

        public override ISymbol resolve()
        {
            return Def;
        }

        public RefSymbol(IToken t, ISymbol def)
            : base(def.Name, t)
        {
            Def = def;
        }
    }
}
