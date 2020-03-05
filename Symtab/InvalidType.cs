namespace Symtab
{

    public class InvalidType : IType
    {
        public virtual string Name => "INVALID";

        public virtual int TypeIndex => -1;
    }

}