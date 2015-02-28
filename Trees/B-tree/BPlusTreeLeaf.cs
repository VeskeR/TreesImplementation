using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    internal class BPlusTreeLeaf<TKey, TValue> : IBTreeNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        public SortedDictionary<TKey, TValue> Values { get; internal set; }

        public BPlusTreeMainNode<TKey, TValue> ParentNode { get; internal set; }
        public BPlusTree<TKey, TValue> ParentPlusTree { get; internal set; }
        public BPlusTreeLeaf<TKey,TValue> NextLeafNode { get; internal set; }

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
        public bool IsRoot { get; protected set; }




        public BPlusTreeLeaf(SortedDictionary<TKey, TValue> values, BPlusTreeMainNode<TKey, TValue> parentNode,
            BPlusTreeLeaf<TKey, TValue> nextLeafNode, BPlusTree<TKey, TValue> parentPlusTree)
        {
            Values = values;
            ParentNode = parentNode;
            NextLeafNode = nextLeafNode;
            ParentPlusTree = parentPlusTree;

            IsLeaf = true;
            IsRoot = false;
        }
    }
}
