namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    public class VariableSymbol : BaseSymbol, ITypedSymbol
    {
        public VariableSymbol(string n, IList<IToken> t) : base(n, t)
        {
        }

        public override IType Type
        {
            set => base.Type = value;
        }
    }

}