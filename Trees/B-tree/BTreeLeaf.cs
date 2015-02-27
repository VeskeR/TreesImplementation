using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    internal class BTreeLeaf<TKey, TValue> : IBTreeNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        public SortedDictionary<TKey, TValue> Values { get; internal set; }

        public BTreeMainNode<TKey, TValue> ParentNode { get; internal set; }
        public BTree<TKey, TValue> ParentTree { get; internal set; }
        public BTreeLeaf<TKey,TValue> NextLeafNode { get; internal set; }

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
        public bool IsRoot { get; protected set; }




        public BTreeLeaf(SortedDictionary<TKey, TValue> values, BTreeMainNode<TKey, TValue> parentNode,
            BTreeLeaf<TKey, TValue> nextLeafNode, BTree<TKey, TValue> parentTree)
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
