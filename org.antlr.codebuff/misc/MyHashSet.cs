using System;
using System.Collections.Generic;
using System.Collections;

/*
 * Copyright 1997-2006 Sun Microsystems, Inc.  All Rights Reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Sun designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Sun in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Sun Microsystems, Inc., 4150 Network Circle, Santa Clara,
 * CA 95054 USA or visit www.sun.com if you need additional information or
 * have any questions.
 */

namespace org.antlr.codebuff.misc
{

    /// <summary>
    /// This class implements the <tt>Set</tt> interface, backed by a hash table
    /// (actually a <tt>HashMap</tt> instance).  It makes no guarantees as to the
    /// iteration order of the set; in particular, it does not guarantee that the
    /// order will remain constant over time.  This class permits the <tt>null</tt>
    /// element.
    /// 
    /// <para>This class offers constant time performance for the basic operations
    /// (<tt>add</tt>, <tt>remove</tt>, <tt>contains</tt> and <tt>size</tt>),
    /// assuming the hash function disperses the elements properly among the
    /// buckets.  Iterating over this set requires time proportional to the sum of
    /// the <tt>HashSet</tt> instance's size (the number of elements) plus the
    /// "capacity" of the backing <tt>HashMap</tt> instance (the number of
    /// buckets).  Thus, it's very important not to set the initial capacity too
    /// high (or the load factor too low) if iteration performance is important.
    /// 
    /// </para>
    /// <para><strong>Note that this implementation is not synchronized.</strong>
    /// If multiple threads access a hash set concurrently, and at least one of
    /// the threads modifies the set, it <i>must</i> be synchronized externally.
    /// This is typically accomplished by synchronizing on some object that
    /// naturally encapsulates the set.
    /// 
    /// If no such object exists, the set should be "wrapped" using the
    /// <seealso cref="Collections#synchronizedSet Collections.synchronizedSet"/>
    /// method.  This is best done at creation time, to prevent accidental
    /// unsynchronized access to the set:<pre>
    ///   Set s = Collections.synchronizedSet(new HashSet(...));</pre>
    /// 
    /// </para>
    /// <para>The iterators returned by this class's <tt>iterator</tt> method are
    /// <i>fail-fast</i>: if the set is modified at any time after the iterator is
    /// created, in any way except through the iterator's own <tt>remove</tt>
    /// method, the Iterator throws a <seealso cref="ConcurrentModificationException"/>.
    /// Thus, in the face of concurrent modification, the iterator fails quickly
    /// and cleanly, rather than risking arbitrary, non-deterministic behavior at
    /// an undetermined time in the future.
    /// 
    /// </para>
    /// <para>Note that the fail-fast behavior of an iterator cannot be guaranteed
    /// as it is, generally speaking, impossible to make any hard guarantees in the
    /// presence of unsynchronized concurrent modification.  Fail-fast iterators
    /// throw <tt>ConcurrentModificationException</tt> on a best-effort basis.
    /// Therefore, it would be wrong to write a program that depended on this
    /// exception for its correctness: <i>the fail-fast behavior of iterators
    /// should be used only to detect bugs.</i>
    /// 
    /// </para>
    /// <para>This class is a member of the
    /// <a href="{@docRoot}/../technotes/guides/collections/index.html">
    /// Java Collections Framework</a>.
    /// 
    /// </para>
    /// </summary>
    /// @param <E> the type of elements maintained by this set
    /// 
    /// @author  Josh Bloch
    /// @author  Neal Gafter </param>
    /// <seealso cref=     Collection </seealso>
    /// <seealso cref=     Set </seealso>
    /// <seealso cref=     TreeSet </seealso>
    /// <seealso cref=     HashMap
    /// @since   1.2 </seealso>

    [Serializable]
    public class MyHashSet<E> : System.Collections.IEnumerable, ICollection<E>
    {
        internal const long serialVersionUID = -5024744406713321676L;

        [NonSerialized] private Dictionary<E, object> map;

        // Dummy value to associate with an Object in the backing Map
        private static readonly object PRESENT = new object();

        /// <summary>
        /// Constructs a new, empty set; the backing <tt>HashMap</tt> instance has
        /// default initial capacity (16) and load factor (0.75).
        /// </summary>
        public MyHashSet()
        {
            map = new Dictionary<E, object>();
        }

        /// <summary>
        /// Constructs a new set containing the elements in the specified
        /// collection.  The <tt>HashMap</tt> is created with default load factor
        /// (0.75) and an initial capacity sufficient to contain the elements in
        /// the specified collection.
        /// </summary>
        /// <param name="c"> the collection whose elements are to be placed into this set </param>
        /// <exception cref="NullPointerException"> if the specified collection is null </exception>
        public MyHashSet(ICollection<E> c)
        {
            map = new Dictionary<E, object>(Math.Max((int) (c.Count/.75f) + 1, 16));
            foreach (E e in c)
                map.Add(e, PRESENT);
        }

        /// <summary>
        /// Constructs a new, empty set; the backing <tt>HashMap</tt> instance has
        /// the specified initial capacity and the specified load factor.
        /// </summary>
        /// <param name="initialCapacity">   the initial capacity of the hash map </param>
        /// <param name="loadFactor">        the load factor of the hash map </param>
        /// <exception cref="IllegalArgumentException"> if the initial capacity is less
        ///             than zero, or if the load factor is nonpositive </exception>
        public MyHashSet(int initialCapacity, float loadFactor)
        {
            map = new Dictionary<E, object>(initialCapacity);
        }

        /// <summary>
        /// Constructs a new, empty set; the backing <tt>HashMap</tt> instance has
        /// the specified initial capacity and default load factor (0.75).
        /// </summary>
        /// <param name="initialCapacity">   the initial capacity of the hash table </param>
        /// <exception cref="IllegalArgumentException"> if the initial capacity is less
        ///             than zero </exception>
        public MyHashSet(int initialCapacity)
        {
            map = new Dictionary<E, object>(initialCapacity);
        }

        /// <summary>
        /// Constructs a new, empty linked hash set.  (This package private
        /// constructor is only used by LinkedHashSet.) The backing
        /// HashMap instance is a LinkedHashMap with the specified initial
        /// capacity and the specified load factor.
        /// </summary>
        /// <param name="initialCapacity">   the initial capacity of the hash map </param>
        /// <param name="loadFactor">        the load factor of the hash map </param>
        /// <param name="dummy">             ignored (distinguishes this
        ///             constructor from other int, float constructor.) </param>
        /// <exception cref="IllegalArgumentException"> if the initial capacity is less
        ///             than zero, or if the load factor is nonpositive </exception>
        internal MyHashSet(int initialCapacity, float loadFactor, bool dummy)
        {
            map = new Dictionary<E, object>(initialCapacity);
        }

        /// <summary>
        /// Returns an iterator over the elements in this set.  The elements
        /// are returned in no particular order.
        /// </summary>
        /// <returns> an Iterator over the elements in this set </returns>
        /// <seealso cref= ConcurrentModificationException </seealso>
        public virtual IEnumerator<E> iterator()
        {
            return map.Keys.GetEnumerator();
        }

        /// <summary>
        /// Returns the number of elements in this set (its cardinality).
        /// </summary>
        /// <returns> the number of elements in this set (its cardinality) </returns>
        public virtual int size()
        {
            return map.Count;
        }

        /// <summary>
        /// Returns <tt>true</tt> if this set contains no elements.
        /// </summary>
        /// <returns> <tt>true</tt> if this set contains no elements </returns>
        public virtual bool Empty
        {
            get { return map.Count == 0; }
        }

        /// <summary>
        /// Returns <tt>true</tt> if this set contains the specified element.
        /// More formally, returns <tt>true</tt> if and only if this set
        /// contains an element <tt>e</tt> such that
        /// <tt>(o==null&nbsp;?&nbsp;e==null&nbsp;:&nbsp;o.equals(e))</tt>.
        /// </summary>
        /// <param name="o"> element whose presence in this set is to be tested </param>
        /// <returns> <tt>true</tt> if this set contains the specified element </returns>
        public virtual bool contains(object o)
        {
            E e = (E)o;
            return map.ContainsKey(e);
        }

        /// <summary>
        /// Adds the specified element to this set if it is not already present.
        /// More formally, adds the specified element <tt>e</tt> to this set if
        /// this set contains no element <tt>e2</tt> such that
        /// <tt>(e==null&nbsp;?&nbsp;e2==null&nbsp;:&nbsp;e.equals(e2))</tt>.
        /// If this set already contains the element, the call leaves the set
        /// unchanged and returns <tt>false</tt>.
        /// </summary>
        /// <param name="e"> element to be added to this set </param>
        /// <returns> <tt>true</tt> if this set did not already contain the specified
        /// element </returns>
        public virtual bool add(E e)
        {
            bool result = map.ContainsKey(e);
            map[e] = PRESENT; // no throw if already in dictionary.
            return result;
        }

        /// <summary>
        /// Removes the specified element from this set if it is present.
        /// More formally, removes an element <tt>e</tt> such that
        /// <tt>(o==null&nbsp;?&nbsp;e==null&nbsp;:&nbsp;o.equals(e))</tt>,
        /// if this set contains such an element.  Returns <tt>true</tt> if
        /// this set contained the element (or equivalently, if this set
        /// changed as a result of the call).  (This set will not contain the
        /// element once the call returns.)
        /// </summary>
        /// <param name="o"> object to be removed from this set, if present </param>
        /// <returns> <tt>true</tt> if the set contained the specified element </returns>
        public virtual bool remove(object o)
        {
            E e = (E) o;
            bool result = map.ContainsKey(e);
            map.Remove(e);
            return result;
        }

        /// <summary>
        /// Removes all of the elements from this set.
        /// The set will be empty after this call returns.
        /// </summary>
        public virtual void clear()
        {
            map.Clear();
        }

        /// <summary>
        /// Returns a shallow copy of this <tt>HashSet</tt> instance: the elements
        /// themselves are not cloned.
        /// </summary>
        /// <returns> a shallow copy of this set </returns>
        public virtual object clone()
        {
            try
            {
                MyHashSet<E> newSet = new MyHashSet<E>();
                newSet.map = new Dictionary<E, object>(this.map);
                return newSet;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        IEnumerator<E> IEnumerable<E>.GetEnumerator()
        {
            return map.Keys.GetEnumerator();
        }

        /// <summary>
        /// Save the state of this <tt>HashSet</tt> instance to a stream (that is,
        /// serialize it).
        /// 
        /// @serialData The capacity of the backing <tt>HashMap</tt> instance
        ///             (int), and its load factor (float) are emitted, followed by
        ///             the size of the set (the number of elements it contains)
        ///             (int), followed by all of its elements (each an Object) in
        ///             no particular order.
        /// </summary>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: private void writeObject(java.io.ObjectOutputStream s) throws java.io.IOException
        //private void writeObject(java.io.ObjectOutputStream s)
        //{
        //    // Write out any hidden serialization magic
        //    s.defaultWriteObject();

        //    // Write out HashMap capacity and load factor
        //    s.writeInt(map.capacity());
        //    s.writeFloat(map.loadFactor());

        //    // Write out size
        //    s.writeInt(map.Count);

        //    // Write out all elements in the proper order.
        //    for (IEnumerator i = map.Keys.GetEnumerator(); i.MoveNext();)
        //    {
        //        s.writeObject(i.Current);
        //    }
        //}

        /// <summary>
        /// Reconstitute the <tt>HashSet</tt> instance from a stream (that is,
        /// deserialize it).
        /// </summary>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: private void readObject(java.io.ObjectInputStream s) throws java.io.IOException, ClassNotFoundException
        //private void readObject(java.io.ObjectInputStream s)
        //{
        //    // Read in any hidden serialization magic
        //    s.defaultReadObject();

        //    // Read in HashMap capacity and load factor and create backing HashMap
        //    int capacity = s.readInt();
        //    float loadFactor = s.readFloat();
        //    map = (((HashSet) this) is LinkedHashSet
        //        ? new LinkedHashMap<E, object>(capacity, loadFactor)
        //        : new Dictionary<E, object>(capacity, loadFactor));

        //    // Read in size
        //    int size = s.readInt();

        //    // Read in all elements in the proper order.
        //    for (int i = 0; i < size; i++)
        //    {
        //        E e = (E) s.readObject();
        //        map[e] = PRESENT;
        //    }
        //}



        public IEnumerator GetEnumerator()
        {
            return map.Keys.GetEnumerator();
        }

        public void Add(E item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(E item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(E[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(E item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; }
        public bool IsReadOnly { get; }
    }
}
