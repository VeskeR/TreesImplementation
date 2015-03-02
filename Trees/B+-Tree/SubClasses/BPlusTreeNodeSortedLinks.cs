using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBTreesLib
{
    [Serializable]
    public class BPlusTreeNodeSortedLinks<TKey, TValue> : SortedList<TKey, BPlusTreeNode<TKey, TValue>>
        where TKey : IComparable<TKey>
    {
        public void AddWithParent(BPlusTreeNode<TKey, TValue> parent, TKey key, BPlusTreeNode<TKey, TValue> node)
        {
            AddWithParentAndNeighbours(parent, null, null, key, node);
        }

        public void AddWithParentAndNeighbours(BPlusTreeNode<TKey, TValue> parent, BPlusTreeNode<TKey, TValue> leftNode,
            BPlusTreeNode<TKey, TValue> rightNode, TKey key, BPlusTreeNode<TKey, TValue> node)
        {
            Add(key, node);

            node.ParentNode = parent;
            node.ParentTree = parent.ParentTree;
            node.RightNode = rightNode;
            node.LeftNode = leftNode;
        }




        public void AddRangeToEnd(BPlusTreeNode<TKey, TValue> parent,
            IEnumerable<KeyValuePair<TKey, BPlusTreeNode<TKey, TValue>>> collection)
        {
            if (collection != null)
            {
                int countBeforeInserting = this.Count;

                foreach (KeyValuePair<TKey, BPlusTreeNode<TKey, TValue>> pair in collection)
                {
                    this.Add(pair.Key, pair.Value);
                }

                for (int i = countBeforeInserting; i < this.Count; i++)
                {
                    this.Values[i].ParentNode = parent;
                    this.Values[i].ParentTree = parent.ParentTree;

                    if (i != 0)
                    {
                        this.Values[i].LeftNode = this.Values[i - 1];
                        this.Values[i - 1].RightNode = this.Values[i];
                    }
                }

                this.Values[this.Values.Count - 1].RightNode = null;
            }
        }

        public void AddRangeToBegin(BPlusTreeNode<TKey, TValue> parent,
            IEnumerable<KeyValuePair<TKey, BPlusTreeNode<TKey, TValue>>> collection)
        {
            if (collection != null)
            {
                int countBeforeInserting = this.Count;

                foreach (KeyValuePair<TKey, BPlusTreeNode<TKey, TValue>> pair in collection)
                {
                    this.Add(pair.Key, pair.Value);
                }

                for (int i = 0; i < this.Count - countBeforeInserting; i++)
                {
                    this.Values[i].ParentNode = parent;
                    this.Values[i].ParentTree = parent.ParentTree;

                    if (i != this.Count - 1)
                    {
                        this.Values[i].RightNode = this.Values[i + 1];
                        this.Values[i + 1].LeftNode = this.Values[i];
                    }
                }

                this.Values[0].LeftNode = null;
            }
        }




        public void CopyTo(BPlusTreeNodeSortedLinks<TKey, TValue> links, int indexFrom, int indexTo)
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
                throw new IndexOutOfRangeException("Index run out of range while coping BPlusTreeNodeSortedLinks.");
            }
        }
    }
}
