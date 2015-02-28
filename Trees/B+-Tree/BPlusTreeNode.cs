using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace MyBTreesLib
{
    public class BPlusTreeNode<TKey, TValue> :
        IBTreeNode<TKey, TValue, BPlusTreeNode<TKey, TValue>, BPlusTree<TKey, TValue>> where TKey : IComparable<TKey>
    {
        protected List<BPlusTreeNode<TKey, TValue>> _links;
        protected List<TKey> _keys;

        public List<BPlusTreeNode<TKey, TValue>> Links
        {
            get
            {
                return _links;
            }

            internal set
            {
                _links = value;

                _links.Sort();

                if (_links != null)
                {
                    int lc = _links.Count;

                    for (int i = 0; i < lc - 1; i++)
                    {
                        _links[i].ParentNode = this;
                        _links[i].NeighbourNode = _links[i + 1];
                    }

                    _links[lc - 1].ParentNode = this;

                    if (lc > 1)
                    {
                        _links[lc - 1].NeighbourNode = _links[lc - 2];
                    }
                }
            }
        }

        public List<TKey> Keys
        {
            get
            {
                return _keys; 
            }
            internal set
            {
                _keys = value;

                _keys.Sort();
            }
        }

        public SortedDictionary<TKey, TValue> Values { get; internal set; }

        public BPlusTreeNode<TKey, TValue> ParentNode { get; internal set; }
        public BPlusTreeNode<TKey, TValue> NeighbourNode { get; internal set; }
        public BPlusTreeNode<TKey, TValue> NextLeafNode { get; internal set; }

        public BPlusTree<TKey, TValue> ParentTree { get; internal set; }

        public int MaxDegree
        {
            get { return ParentTree.MaxDegree; }
        }

        public double Alpha
        {
            get { return ParentTree.Alpha; }
        }

        public bool IsLeaf { get; protected set; }
        public bool IsRoot { get; internal set; }




        public BPlusTreeNode(List<TKey> keys, BPlusTreeNode<TKey, TValue> parentNode,
            BPlusTreeNode<TKey, TValue> neighbour,
            BPlusTree<TKey, TValue> parentTree, bool isRoot)
        {
            Keys = keys;
            ParentNode = parentNode;
            NeighbourNode = neighbour;
            ParentTree = parentTree;

            IsLeaf = false;
            IsRoot = isRoot;
        }

        public BPlusTreeNode(SortedDictionary<TKey, TValue> values, BPlusTreeNode<TKey, TValue> parentNode,
            BPlusTreeNode<TKey, TValue> nextLeafNode, BPlusTree<TKey, TValue> parentTree)
        {
            Values = values;
            ParentNode = parentNode;
            NextLeafNode = nextLeafNode;
            ParentTree = parentTree;

            IsLeaf = true;
            IsRoot = false;
        }
    }
}