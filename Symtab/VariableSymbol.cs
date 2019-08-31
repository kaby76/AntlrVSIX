namespace org.antlr.symtab
{

    public class VariableSymbol : BaseSymbol, TypedSymbol
    {
        public VariableSymbol(string name) : base(name)
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