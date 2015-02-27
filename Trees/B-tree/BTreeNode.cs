using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    public class BTreeNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        protected List<BTreeNode<TKey, TValue>> _links;
        protected SortedDictionary<TKey, TValue> _values;
        protected List<TKey> _keys; 

        public List<BTreeNode<TKey, TValue>> Links
        {
            get
            {
                return _links;
            }
            set
            {
                _links = value;

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
        public List<TKey> Keys { get; internal set; }
        public SortedDictionary<TKey, TValue> Values
        {
            get
            {
                if (IsLeaf)
                {
                    return _values;
                }

                return null;
            }
            set
            {
                if (IsLeaf)
                {
                    _values = value;
                    _keys = value.Keys.ToList();
                }
            }
        }

        public BTreeNode<TKey, TValue> ParentNode { get; internal set; }
        public BTreeNode<TKey, TValue> NeighbourNode { get; internal set; }

        public BTree<TKey, TValue> ParentTree { get; internal set; }
        public BTreeNode<TKey,TValue> NextLeafNode { get; internal set; }

        public int MaxDegree
        {
            get
            {
                return ParentTree.MaxDegree;
            }
        }
        public int Alpha
        {
            get
            {
                return ParentTree.Alpha;
            }
        }

        public bool IsLeaf { get; internal set; }
        public bool IsRoot { get; internal set; }




        public BTreeNode(List<TKey> keys, BTreeNode<TKey, TValue> parent, BTreeNode<TKey, TValue> neighbour,
            BTree<TKey, TValue> parentTree, bool isRoot)
        {
            Keys = keys;
            ParentNode = parent;
            NeighbourNode = neighbour;
            ParentTree = parentTree;

            IsLeaf = false;
            IsRoot = isRoot;
        }

        public BTreeNode(SortedDictionary<TKey, TValue> values, BTreeNode<TKey, TValue> parent,
            BTreeNode<TKey, TValue> neighbour, BTreeNode<TKey, TValue> nextLeafNode, BTree<TKey, TValue> parentTree)
        {
            Values = values;
            ParentNode = parent;
            NeighbourNode = neighbour;
            NextLeafNode = nextLeafNode;
            ParentTree = parentTree;

            IsLeaf = true;
            IsRoot = false;
        }
    }
}
