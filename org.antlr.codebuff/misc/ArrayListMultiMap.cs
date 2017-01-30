using System;
using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using org.antlr.codebuff.misc;

namespace com.google.common.collect
{
    public sealed class ArrayListMultiMap<K, V> : MyMultiMap<K, V>, IEnumerable
    {
        private List<KeyValuePair<K, V>> the_order;

        public static ArrayListMultiMap<K, V> create()
        {
            return new ArrayListMultiMap<K, V>();
        }

        public static ArrayListMultiMap<K, V> create(MyMultiMap<K, V> multimap)
        {
            return new ArrayListMultiMap<K, V>(multimap);
        }

        private ArrayListMultiMap()
        {
            the_order = new List<KeyValuePair<K, V>>();
        }

        private ArrayListMultiMap(MyMultiMap<K, V> multimap)
        {
            the_order = new List<KeyValuePair<K, V>>();
            foreach (KeyValuePair<K, MyHashSet<V>> kv in multimap)
            {
                foreach (object v in kv.Value)
                {
                    V vv = (V)v;
                    this[kv.Key] = kv.Value;
                    the_order.Add(new KeyValuePair<K,V>(kv.Key, vv));
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
