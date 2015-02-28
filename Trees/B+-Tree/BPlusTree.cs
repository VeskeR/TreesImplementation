﻿using System;
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