using System.Collections.Generic;

namespace org.antlr.codebuff.misc
{
    /// <summary>
    /// Extension to the normal Dictionary. This class can store more than one value for every key. It keeps a HashSet for every Key value.
    /// Calling Add with the same Key and multiple values will store each value under the same Key in the Dictionary. Obtaining the values
    /// for a Key will return the HashSet with the Values of the Key. 
    /// </summary>
    /// <typeparam name="K">The type of the key.</typeparam>
    /// <typeparam name="V">The type of the value.</typeparam>
    public class MyMultiMap<K, V> : Dictionary<K, MyHashSet<V>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyMultiMap&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public MyMultiMap()
            : base()
        {
        }


        /// <summary>
        /// Adds the specified value under the specified key
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(K key, V value)
        {
            MyHashSet<V> container = null;
            if (!this.TryGetValue(key, out container))
            {
                container = new MyHashSet<V>();
                base.Add(key, container);
            }
            container.add(value);
        }


        /// <summary>
        /// Determines whether this dictionary contains the specified value for the specified key 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the value is stored for the specified key in this dictionary, false otherwise</returns>
        public bool ContainsValue(K key, V value)
        {
            bool toReturn = false;
            MyHashSet<V> values = null;
            if (this.TryGetValue(key, out values))
            {
                toReturn = values.contains(value);
            }
            return toReturn;
        }


        /// <summary>
        /// Removes the specified value for the specified key. It will leave the key in the dictionary.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Remove(K key, V value)
        {
            MyHashSet<V> container = null;
            if (this.TryGetValue(key, out container))
            {
                container.remove(value);
                if (container.size() <= 0)
                {
                    this.Remove(key);
                }
            }
        }


        /// <summary>
        /// Merges the specified multivaluedictionary into this instance.
        /// </summary>
        /// <param name="toMergeWith">To merge with.</param>
        public void Merge(MyMultiMap<K, V> toMergeWith)
        {
            if (toMergeWith == null)
            {
                return;
            }

            foreach (KeyValuePair<K, MyHashSet<V>> pair in toMergeWith)
            {
                foreach (V value in pair.Value)
                {
                    this.Add(pair.Key, value);
                }
            }
        }


        /// <summary>
        /// Gets the values for the key specified. This method is useful if you want to avoid an exception for key value retrieval and you can't use TryGetValue
        /// (e.g. in lambdas)
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="returnEmptySet">if set to true and the key isn't found, an empty hashset is returned, otherwise, if the key isn't found, null is returned</param>
        /// <returns>
        /// This method will return null (or an empty set if returnEmptySet is true) if the key wasn't found, or
        /// the values if key was found.
        /// </returns>
        public MyHashSet<V> GetValues(K key, bool returnEmptySet)
        {
            MyHashSet<V> toReturn = null;
            if (!base.TryGetValue(key, out toReturn) && returnEmptySet)
            {
                toReturn = new MyHashSet<V>();
            }
            return toReturn;
        }

        public virtual void Map(K key, V value)
        {
            MyHashSet<V> elementsForKey;
            if (!TryGetValue(key, out elementsForKey))
            {
                elementsForKey = new MyHashSet<V>();
                this[key] = elementsForKey;
            }
            elementsForKey.add(value);
        }
    }
}