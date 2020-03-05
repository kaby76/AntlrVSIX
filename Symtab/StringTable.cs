namespace Symtab
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A unique set of strings mapped to a monotonically increasing index.
    ///  These indexes often useful to bytecode interpreters that have instructions
    ///  referring to strings by unique integer. Indexing is from 0.
    /// 
    ///  We can also get them back out in original order.
    /// 
    ///  Yes, I know that this is similar to <seealso cref="string#intern()"/> but in this
    ///  case, I need the index out not just to make these strings unique.
    /// </summary>
    public class StringTable
    {
        internal LinkedHashMap<string, int?> table = new LinkedHashMap<string, int?>();
        protected internal int index = -1; // index we have just written
        protected internal IList<string> strings = new List<string>();

        public virtual int add(string s)
        {
            int? I = table[s];
            if (I != null)
            {
                return I.Value;
            }
            index++;
            table[s] = index;
            strings.Add(s);
            return index;
        }

        /// <summary>
        /// Get the ith string or null if out of range </summary>
        public virtual string get(int i)
        {
            if (i < size() && i >= 0)
            {
                return strings[i];
            }
            return null;
        }

        public virtual int size()
        {
            return table.Count;
        }

        /// <summary>
        /// Return an array, possibly of length zero, with all strings
        ///  sitting at their appropriate index within the array.
        /// </summary>
        public virtual string[] toArray()
        {
            return strings.ToArray();
        }

        /// <summary>
        /// Return a List, possibly of length zero, with all strings
        ///  sitting at their appropriate index within the array.
        /// </summary>
        public virtual IList<string> toList()
        {
            return strings;
        }

        public virtual int NumberOfStrings => index + 1;

        public override string ToString()
        {
            return table.ToString();
        }
    }

}