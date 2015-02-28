using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    public class BPlusTreeMainNode<TKey, TValue> : IBTreeNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        protected List<BPlusTreeMainNode<TKey, TValue>> _links;
        protected List<TKey> _keys; 

        public List<BPlusTreeMainNode<TKey, TValue>> Links
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
            set
            {
                _keys = value;
                
                _keys.Sort();
            }
        }

        public BPlusTreeMainNode<TKey, TValue> ParentNode { get; internal set; }
        public BPlusTreeMainNode<TKey, TValue> NeighbourNode { get; internal set; }

        public BPlusTree<TKey, TValue> ParentPlusTree { get; internal set; }

        public int MaxDegree
        {
            get
            {
                return ParentPlusTree.MaxDegree;
            }
        }
        public int Alpha
        {
            get
            {
                return ParentPlusTree.Alpha;
            }
        }

        public bool IsLeaf { get; protected set; }
        public bool IsRoot { get; internal set; }




        public BPlusTreeMainNode(List<TKey> keys, BPlusTreeMainNode<TKey, TValue> parentNode, BPlusTreeMainNode<TKey, TValue> neighbour,
            BPlusTree<TKey, TValue> parentPlusTree, bool isRoot)
        {
            Keys = keys;
            ParentNode = parentNode;
            NeighbourNode = neighbour;
            ParentPlusTree = parentPlusTree;

            IsLeaf = false;
            IsRoot = isRoot;
        }
    }
}
