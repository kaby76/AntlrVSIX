namespace org.antlr.symtab
{
    using System;
    using System.Collections.Generic;

    internal class LinkedHashMap<T, U>
    {
        Dictionary<T, LinkedListNode<Tuple<U, T>>> D = new Dictionary<T, LinkedListNode<Tuple<U, T>>>();
        LinkedList<Tuple<U, T>> LL = new LinkedList<Tuple<U, T>>();

        public U this[T c]
        {
            get
            {
                D.TryGetValue(c, out LinkedListNode<Tuple<U, T>> v);
                if (v != null)
                    return D[c].Value.Item1;
                else
                    return default(U);
            }

            set
            {
                if (D.ContainsKey(c))
                {
                    LL.Remove(D[c]);
                }

                D[c] = new LinkedListNode<Tuple<U, T>>(Tuple.Create(value, c));
                LL.AddLast(D[c]);
            }
        }

        public bool ContainsKey(T k)
        {
            return D.ContainsKey(k);
        }

        public U PopFirst()
        {
            var node = LL.First;
            LL.Remove(node);
            D.Remove(node.Value.Item2);
            return node.Value.Item1;
        }

        public int Count
        {
            get { return D.Count; }
        }
    }
}