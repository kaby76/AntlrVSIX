namespace org.antlr.symtab
{

    public class InvalidType : Type
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