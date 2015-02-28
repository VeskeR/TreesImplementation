using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBTreesLib
{
    public class BPlusTree<TKey, TValue> : IBTree<TKey, TValue, BPlusTreeNode<TKey, TValue>>
        where TKey : IComparable<TKey>
    {
        private int _maxDegree;
        private double _alpha;

        public BPlusTreeNode<TKey, TValue> Head { get; set; }

        public int MaxDegree
        {
            get { return _maxDegree; }
            protected set { _maxDegree = value < 2 ? 2 : value; }
        }

        public double Alpha
        {
            get { return _alpha; }
            protected set { _alpha = (value <= 0.5 && value > 0) ? value : 0.5; }
        }

        public int Count { get; protected set; }
        public bool IsReadOnly { get; protected set; }




        public BPlusTree()
            :this(null, 0, 0)
        {
            
        }
        public BPlusTree(int maxDegree, int alpha)
            : this(null, maxDegree, alpha)
        {

        }
        public BPlusTree(IEnumerable<KeyValuePair<TKey, TValue>> collection, int maxDegree, int alpha)
        {
            IsReadOnly = false;

            MaxDegree = maxDegree;
            Alpha = alpha;

            if (collection != null)
            {
                AddRange(collection);
            }
        }




        protected BPlusTreeNode<TKey, TValue> FindLeafThatMightContainKey(TKey key)
        {
            if (Head != null)
            {
                BPlusTreeNode<TKey, TValue> current = Head;

                while (!current.IsLeaf)
                {
                    current = current.Links[FindIndexOfLinkToGo(current, key)];
                }

                return current;
            }

            return null;
        }

        protected int FindIndexOfLinkToGo(BPlusTreeNode<TKey, TValue> node, TKey key)
        {
            for (int i = 0; i < node.Keys.Count; i++)
            {
                if (node.Keys[i].CompareTo(key) >= 0)
                {
                    return i;
                }
            }

            return node.Keys.Count;
        }

        protected bool FindValueByKeyInLeaf(BPlusTreeNode<TKey, TValue> leaf, TKey key, out TValue value)
        {
            return leaf.Values.TryGetValue(key, out value);
        }



        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        //TODO
        public void Add(KeyValuePair<TKey, TValue> pair)
        {
            BPlusTreeNode<TKey, TValue> current = FindLeafThatMightContainKey(pair.Key);

            if (current != null)
            {
                if (current.Values.Count < MaxDegree)
                {
                    current.Values.Add(pair.Key, pair.Value);


                }
            }
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
        //TODO
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }



        public bool Find(TKey key , out TValue value)
        {
            BPlusTreeNode<TKey, TValue> leaf = FindLeafThatMightContainKey(key);

            if (FindValueByKeyInLeaf(leaf, key, out value))
            {
                return true;
            }

            value = default (TValue);
            return false;
        }

        public bool Contains(KeyValuePair<TKey, TValue> pair)
        {
            TValue tempValue;

            if (Find(pair.Key, out tempValue) && EqualityComparer<TValue>.Default.Equals(tempValue, pair.Value))
            {
                return true;
            }

            return false;
        }



        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (Head != null)
            {
                BPlusTreeNode<TKey, TValue> current = Head;

                while (!current.IsLeaf)
                {
                    current = current.Links[0];
                }

                do
                {
                    foreach (KeyValuePair<TKey, TValue> pair in current.Values)
                    {
                        array[arrayIndex++] = pair;
                    }

                    current = current.NextLeafNode;
                } while (current != null);
            }
        }



        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (Head != null)
            {
                BPlusTreeNode<TKey, TValue> current = Head;

                while (!current.IsLeaf)
                {
                    current = current.Links[0];
                }

                do
                {
                    foreach (KeyValuePair<TKey, TValue> pair in current.Values)
                    {
                        yield return pair;
                    }

                    current = current.NextLeafNode;
                } while (current != null);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}