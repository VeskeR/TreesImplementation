using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBTreesLib
{
    internal interface IBTreeNode<TKey, TValue, TBTreeNode, TBTree> where TKey : IComparable<TKey>
    {
        BPlusTreeNodeSortedLinks<TKey, TValue> Links { get; }
        BPlusTreeNodeSortedValues<TKey, TValue> Values { get; }

        TBTreeNode ParentNode { get; }
        TBTree ParentTree { get; }

        int MaxDegree { get; }
        double Alpha { get; }
        int MinDegree { get; }

        bool IsLeaf { get; }
        bool IsRoot { get; }
    }
}
