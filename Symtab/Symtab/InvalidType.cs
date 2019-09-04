namespace Symtab
{

    public class InvalidType : Type
    {
        public Mono.Cecil.MemberReference MonoType { get; set; }
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