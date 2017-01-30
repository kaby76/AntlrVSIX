using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace org.antlr.codebuff.misc
{


	public class HashBag<T> : IDictionary<T, int>
	{
		protected internal IDictionary<T, MutableInt> data = new Dictionary<T, MutableInt>();

		public virtual void Clear()
		{
			data.Clear();
		}

		public virtual int Count
		{
			get
			{
				return data.Count;
			}
		}
        
		public virtual bool Empty
		{
			get
			{
				return data.Count == 0;
			}
		}

		public virtual bool ContainsKey(object key)
		{
			return data.ContainsKey((T)key);
		}

		public virtual bool containsValue(object value)
		{
		    var f = data.Select(t =>
		    {
		        if (value == t.Value) return true;
		        else return false;
		    });
		    return f.Any();
		}

		public virtual int? get(object key)
		{
		    MutableInt I = null;
            if (key.GetType() != typeof(T))
                return null;
            data.TryGetValue((T)key, out I);
			if (I != null)
			{
				return I.asInt();
			}
			return null;
		}

		public int? put(T key, int value)
		{
			data[key] = new MutableInt(value);
			return null; // violates Map<> contract
		}

		public virtual int? add(T key)
		{
            MutableInt I = null;
            data.TryGetValue(key, out I);
			if (I == null)
			{
				data[key] = new MutableInt(1);
			}
			else
			{
				I.inc();
			}
			return get(key);
		}

		public virtual int? remove(object key)
		{
			int? I = get(key);
			data.Remove((T)key);
			return I;
		}

		public void putAll<T1>(IDictionary<T1, int> m) where T1 : T
		{
			foreach (T1 key in m.Keys)
			{
				put(key, m[key]);
			}
		}

		//public virtual ISet<T> keySet()
		//{
		//    ICollection<T> keys = data.Keys;
		//    var rv = new MyHashSet<T>();
		//    foreach (T t in keys) rv.add(t);
  //          return rv;
		//}

		public virtual ICollection<int> Values
		{
			get
			{
				IList<int> v = new List<int>();
				foreach (MutableInt I in data.Values)
				{
					v.Add(I.asInt());
				}
				return v;
			}
		}

		public virtual ISet<KeyValuePair<T, int>> entrySet()
		{
			throw new System.NotSupportedException();
		}

		public override string ToString()
		{
			return data.ToString();
		}

        public virtual T MostFrequent
		{
			get
			{
				T t = default(T);
				int max = 0;
				foreach (T key in data.Keys)
				{
					MutableInt count = data[key];
					if (count.asInt() > max)
					{
						max = count.asInt();
						t = key;
					}
				}
				return t;
			}
		}

        public bool ContainsKey(T key)
        {
            throw new NotImplementedException();
        }

        public void Add(T key, int value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(T key, out int value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<T, int> item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<T, int> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<T, int>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<T, int> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public ICollection<T> Keys
        {
            get
            {
                IList<T> v = new List<T>();
                foreach (T k in data.Keys)
                {
                    v.Add(k);
                }
                return v;
            }
        }

        ICollection<int> IDictionary<T, int>.Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int this[T key]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }

}