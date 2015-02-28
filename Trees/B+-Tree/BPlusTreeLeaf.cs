using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBTreesLib
{
    [Obsolete("All functionality ob B+-Trees nodes have been moved to BPlusTreeNode class. Use BPlusTreeNode class.")]
    public class BPlusTreeLeaf<TKey, TValue> :
        IBTreeNode<TKey, TValue, BPlusTreeLeaf<TKey, TValue>, BPlusTree<TKey, TValue>> where TKey : IComparable<TKey>
    {
        //public SortedDictionary<TKey, TValue> Values { get; internal set; }

        //public BPlusTreeNode<TKey, TValue> ParentNode { get; internal set; }
        //public BPlusTree<TKey, TValue> ParentTree { get; internal set; }
        //public BPlusTreeLeaf<TKey,TValue> NextLeafNode { get; internal set; }

        //public int MaxDegree
        //{
        //    get
        //    {
        //        return ParentTree.MaxDegree;
        //    }
        //}
        //public double Alpha
        //{
        //    get
        //    {
        //        return ParentTree.Alpha;
        //    }
        //}

        //public bool IsLeaf { get; protected set; }
        //public bool IsRoot { get; protected set; }




        //public BPlusTreeLeaf(SortedDictionary<TKey, TValue> values, BPlusTreeNode<TKey, TValue> parentNode,
        //    BPlusTreeLeaf<TKey, TValue> nextLeafNode, BPlusTree<TKey, TValue> parentTree)
        //{
        //    Values = values;
        //    ParentNode = parentNode;
        //    NextLeafNode = nextLeafNode;
        //    ParentTree = parentTree;

        //    IsLeaf = true;
        //    IsRoot = false;
        //}
        public List<BPlusTreeLeaf<TKey, TValue>> Links { get; private set; }
        public SortedDictionary<TKey, TValue> Values { get; private set; }
        public BPlusTreeLeaf<TKey, TValue> ParentNode { get; private set; }
        public BPlusTree<TKey, TValue> ParentTree { get; private set; }
        public int MaxDegree { get; private set; }
        public double Alpha { get; private set; }
        public bool IsLeaf { get; private set; }
        public bool IsRoot { get; private set; }
    }
}