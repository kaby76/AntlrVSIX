namespace org.antlr.symtab
{

    /// <summary>
    /// An element in a type tree that represents a pointer to some type,
    ///  such as we need for C.  "int *" would need a PointerType(intType) object.
    /// </summary>
    public class PointerType : Type
    {
        protected internal Type targetType;
        public PointerType(Type targetType)
        {
            this.targetType = targetType;
        }

        public virtual string Name
        {
            get
            {
                return ToString();
            }
        }

        public virtual int TypeIndex
        {
            get
            {
                return -1;
            }
        }
        public override string ToString()
        {
            return "*" + targetType;
        }
    }

}