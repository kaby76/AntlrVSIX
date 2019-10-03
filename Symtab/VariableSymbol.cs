namespace Symtab
{
    using Antlr4.Runtime;

    public class VariableSymbol : BaseSymbol, TypedSymbol
    {
        public VariableSymbol(string n, IToken t) : base(n, t)
        {
        }

        public override Type Type
        {
            set
            {
                base.Type = value;
            }
        }
    }

}