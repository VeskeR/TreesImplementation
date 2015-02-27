using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    public class BTreeMainNode<TKey, TValue> : IBTreeNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        protected List<BTreeMainNode<TKey, TValue>> _links;
        protected List<TKey> _keys; 

        public List<BTreeMainNode<TKey, TValue>> Links
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

        public BTreeMainNode<TKey, TValue> ParentNode { get; internal set; }
        public BTreeMainNode<TKey, TValue> NeighbourNode { get; internal set; }

        public BTree<TKey, TValue> ParentTree { get; internal set; }

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

        public bool IsLeaf { get; protected set; }
        public bool IsRoot { get; internal set; }




        public BTreeMainNode(List<TKey> keys, BTreeMainNode<TKey, TValue> parentNode, BTreeMainNode<TKey, TValue> neighbour,
            BTree<TKey, TValue> parentTree, bool isRoot)
        {
            Keys = keys;
            ParentNode = parentNode;
            NeighbourNode = neighbour;
            ParentTree = parentTree;

            IsLeaf = false;
            IsRoot = isRoot;
        }
    }
}
