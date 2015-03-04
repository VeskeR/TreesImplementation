using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBTreesLib
{
    [Serializable]
    public class BPlusTreeNodeSortedValues<TKey, TValue> : SortedList<TKey, TValue> where TKey : IComparable<TKey>
    {
        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            foreach (KeyValuePair<TKey, TValue> pair in values)
            {
                this.Add(pair.Key, pair.Value);
            }
        }



        public void CopyTo(BPlusTreeNodeSortedValues<TKey, TValue> links, int copyFromIndex, int copyToIndex)
        {
            try
            {
                for (int i = copyFromIndex; i < copyToIndex; i++)
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
