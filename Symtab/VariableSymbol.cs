namespace Symtab
{

    public class VariableSymbol : BaseSymbol, TypedSymbol
    {
        public VariableSymbol(string n, int l, int c, string f) : base(n, l, c, f)
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