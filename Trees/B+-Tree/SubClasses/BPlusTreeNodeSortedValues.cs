using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBTreesLib
{
    public class BPlusTreeNodeSortedValues<TKey, TValue> : SortedList<TKey, TValue> where TKey : IComparable<TKey>
    {
        public void CopyTo(BPlusTreeNodeSortedValues<TKey, TValue> links, int indexFrom, int indexTo)
        {
            try
            {
                for (int i = indexFrom; i < indexTo; i++)
                {
                    links.Add(this.Keys[i], this.Values[i]);
                }
            }
            catch (Exception)
            {
                throw new IndexOutOfRangeException("Index run out of range while coping BPlusTreeNodeSortedValues.");
            }
        }
    }
}
