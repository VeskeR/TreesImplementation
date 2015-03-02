using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBTreesLib
{
    internal interface IBTree<TKey, TValue, TBTreeNode> : ICollection<KeyValuePair<TKey, TValue>>
        where TKey : IComparable<TKey>
    {
        TBTreeNode Head { get; set; }

        int MaxDegree { get; }
        double Alpha { get; }
        int MinDegree { get; }

        int KeysCount { get; }
        int NodesCount { get; }




        void Add(TKey key, TValue value);

        void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> collection);
    }
}