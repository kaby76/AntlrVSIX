namespace Symtab
{

    public class VariableSymbol : BaseSymbol, TypedSymbol
    {
        protected VariableSymbol(string name) : base(name)
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