using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    public class BTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        public BTreeNode<TKey, TValue> Head { get; set; }

        public int MaxDegree { get; set; }
        public int Alpha { get; set; }




        public BTree(int maxDegree, int alpha)
            : this(null, maxDegree, alpha)
        {

        }
        //TODO
        public BTree(IEnumerable<IDictionary<TKey, TValue>> collection, int maxDegree, int alpha)
        {
            MaxDegree = maxDegree;
            Alpha = alpha;
        }


    }
}
