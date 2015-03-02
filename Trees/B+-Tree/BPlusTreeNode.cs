using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace MyBTreesLib
{
    [Serializable]
    public class BPlusTreeNode<TKey, TValue> :
        IBTreeNode<TKey, TValue, BPlusTreeNode<TKey, TValue>, BPlusTree<TKey, TValue>> where TKey : IComparable<TKey>
    {
        protected BPlusTreeNodeSortedLinks<TKey, TValue> _links;



        //TODO
        public BPlusTreeNodeSortedLinks<TKey, TValue> Links
        {
            get
            {
                return _links;
            }

            internal set
            {
                _links = value;

                if (_links != null)
                {
                    foreach (KeyValuePair<TKey, BPlusTreeNode<TKey, TValue>> pair in _links)
                    {
                        pair.Value.ParentNode = this;
                    }
                }
            }
        }

        public BPlusTreeNodeSortedValues<TKey, TValue> Values { get; internal set; }

        public int KeysCount
        {
            get
            {
                if (this.IsLeaf)
                {
                    return this.Values.Keys.Count;
                }

                return this.Links.Keys.Count;
            }
        }

        public BPlusTreeNode<TKey, TValue> ParentNode { get; internal set; }
        public BPlusTreeNode<TKey, TValue> RightNode { get; internal set; }
        public BPlusTreeNode<TKey, TValue> LeftNode { get; internal set; } 
        public BPlusTreeNode<TKey, TValue> RightLeafNode { get; internal set; }
        public BPlusTreeNode<TKey, TValue> LeftLeafNode { get; internal set; } 

        public BPlusTree<TKey, TValue> ParentTree { get; internal set; }

        public int MaxDegree
        {
            get { return ParentTree.MaxDegree; }
        }

        public double Alpha
        {
            get { return ParentTree.Alpha; }
        }

        public int MinDegree
        {
            get
            {
                return ParentTree.MinDegree;
            }
        }

        public bool IsLeaf { get; protected set; }
        public bool IsRoot { get; internal set; }




        public BPlusTreeNode(BPlusTreeNodeSortedLinks<TKey, TValue> links, BPlusTreeNode<TKey, TValue> parentNode,
            BPlusTreeNode<TKey, TValue> rightNode, BPlusTreeNode<TKey, TValue> leftNode,
            BPlusTree<TKey, TValue> parentTree, bool isRoot)
        {
            Links = links;
            ParentNode = parentNode;
            RightNode = rightNode;
            LeftNode = leftNode;
            ParentTree = parentTree;

            IsLeaf = false;
            IsRoot = isRoot;
        }

        public BPlusTreeNode(BPlusTreeNodeSortedValues<TKey, TValue> values, BPlusTreeNode<TKey, TValue> parentNode,
            BPlusTreeNode<TKey, TValue> rightLeafNode, BPlusTreeNode<TKey, TValue> leftLeafNode,
            BPlusTree<TKey, TValue> parentTree, bool isRoot)
        {
            Values = values;
            ParentNode = parentNode;
            RightLeafNode = rightLeafNode;
            LeftLeafNode = leftLeafNode;
            ParentTree = parentTree;

            IsLeaf = true;
            IsRoot = isRoot;
        }
    }
}