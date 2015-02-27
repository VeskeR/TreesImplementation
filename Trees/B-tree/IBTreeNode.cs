using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    interface IBTreeNode<TKey,TValue> where TKey : IComparable<TKey>
    {
        BTreeMainNode<TKey, TValue> ParentNode { get; }

        BTree<TKey, TValue> ParentTree { get; }

        int MaxDegree { get; }
        int Alpha { get; }

        bool IsLeaf { get; }
        bool IsRoot { get; }
    }
}
