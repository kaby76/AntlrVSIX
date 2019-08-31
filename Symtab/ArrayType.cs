namespace org.antlr.symtab
{

    /// <summary>
    /// An element within a type type such is used in C or Java where we need to
    ///  indicate the type is an array of some element type like float[] or User[].
    ///  It also tracks the size as some types indicate the size of the array.
    /// </summary>
    public class ArrayType : Type
    {
        protected internal readonly Type elemType;
        protected internal readonly int numElems; // some languages allow you to point at arrays of a specific size

        public ArrayType(Type elemType)
        {
            this.elemType = elemType;
            this.numElems = -1;
        }

        public ArrayType(Type elemType, int numElems)
        {
            this.elemType = elemType;
            this.numElems = numElems;
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
            return elemType + "[]";
        }
    }

}