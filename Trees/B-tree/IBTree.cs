using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    interface IBTree<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>> where TKey : IComparable<TKey>
    {
        int MaxDegree { get; }
        double Alpha { get; }
    }
}
