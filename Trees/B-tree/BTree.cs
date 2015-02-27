using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    public class BTree<TKey, TValue> : ICollection<KeyValuePair<TKey,TValue>>,IEnumerable<KeyValuePair<TKey, TValue>> where TKey : IComparable<TKey>
    {
        public BTreeMainNode<TKey, TValue> Head { get; set; }

        public int MaxDegree { get; set; }
        public int Alpha { get; set; }
        public int Count { get; set; }
        public bool IsReadOnly { get; private set; }




        public BTree(int maxDegree, int alpha)
            : this(null, maxDegree, alpha)
        {

        }

        public BTree(IEnumerable<KeyValuePair<TKey, TValue>> collection, int maxDegree, int alpha)
        {
            IsReadOnly = false;

            MaxDegree = maxDegree;
            Alpha = alpha;

            if (collection != null)
            {
                AddRange(collection);                
            }
        }

        


        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }
        //TODO
        public void Add(KeyValuePair<TKey, TValue> pair)
        {
            
        }

        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            foreach (KeyValuePair<TKey, TValue> pair in collection)
            {
                Add(pair);
            }
        }




        public void Clear()
        {
            Count = 0;
            Head = null;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
