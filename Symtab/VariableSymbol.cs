namespace Symtab
{
    using Antlr4.Runtime;

    public class VariableSymbol : BaseSymbol, ITypedSymbol
    {
        public VariableSymbol(string n, IToken t) : base(n, t)
        {
        }

        public override IType Type
        {
            set
            {
                base.Type = value;
            }
        }
    }

}