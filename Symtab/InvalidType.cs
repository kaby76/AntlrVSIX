namespace Symtab
{

    public class InvalidType : IType
    {
        public virtual string Name
        {
            get
            {
                return "INVALID";
            }
        }

        public virtual int TypeIndex
        {
            get
            {
                return -1;
            }
        }
    }

}