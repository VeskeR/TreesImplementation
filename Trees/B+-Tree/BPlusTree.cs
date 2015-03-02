using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBTreesLib
{
    [Serializable]
    public class BPlusTree<TKey, TValue> : IBTree<TKey, TValue, BPlusTreeNode<TKey, TValue>>
        where TKey : IComparable<TKey>
    {
        private int _maxDegree;
        private double _alpha;

        public BPlusTreeNode<TKey, TValue> Head { get; set; }

        public int MaxDegree
        {
            get { return _maxDegree; }
            protected set { _maxDegree = value < 2 ? 2 : value; }
        }

        public double Alpha
        {
            get { return _alpha; }
            protected set { _alpha = (value <= 0.5 && value > 0) ? value : 0.5; }
        }

        public int MinDegree
        {
            get
            {
                return ((int) (MaxDegree*Alpha) > 0) ? (int) (MaxDegree*Alpha) : 1;
            }
        }

        public int Count { get; protected set; }

        public int KeysCount
        {
            get
            {
                return Count;
            }
        }

        public int NodesCount { get; protected set; }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }




        public BPlusTree()
            :this(null, 0, 0)
        {
            
        }
        public BPlusTree(int maxDegree, int alpha)
            : this(null, maxDegree, alpha)
        {

        }
        public BPlusTree(IEnumerable<KeyValuePair<TKey, TValue>> collection, int maxDegree, int alpha)
        {
            MaxDegree = maxDegree;
            Alpha = alpha;

            if (collection != null)
            {
                AddRange(collection);
            }
        }




        protected BPlusTreeNode<TKey, TValue> FindLeafThatMightContainKey(TKey key)
        {
            if (Head != null)
            {
                BPlusTreeNode<TKey, TValue> current = Head;

                while (!current.IsLeaf)
                {
                    current = current.Links.Values[FindIndexOfLinkToGo(current, key)];
                }

                return current;
            }

            return null;
        }

        protected int FindIndexOfLinkToGo(BPlusTreeNode<TKey, TValue> node, TKey key)
        {
            for (int i = 0; i < node.Links.Count; i++)
            {
                if (node.Links.Keys[i].CompareTo(key) >= 0)
                {
                    return i;
                }
            }

            //Если нужный ключ больше всех в даной ноде, тогда возвращается индекс последнего сына даного нода
            return node.Links.Count - 1;
        }

        protected bool FindValueByKeyInLeaf(BPlusTreeNode<TKey, TValue> leaf, TKey key, out TValue value)
        {
            return leaf.Values.TryGetValue(key, out value);
        }



        protected void Split(BPlusTreeNode<TKey, TValue> nodeToSplit)
        {
            NodesCount++;

            if (nodeToSplit.IsLeaf)
            {
                SplitLeaf(nodeToSplit);
            }
            else
            {
                SplitNode(nodeToSplit);
            }
        }

        protected void SplitLeaf(BPlusTreeNode<TKey, TValue> leafToSplit)
        {
            int pivotIndex = leafToSplit.Values.Count / 2;

            BPlusTreeNodeSortedValues<TKey, TValue> leftValues = new BPlusTreeNodeSortedValues<TKey, TValue>();
            BPlusTreeNodeSortedValues<TKey, TValue> rightValues = new BPlusTreeNodeSortedValues<TKey, TValue>();

            leafToSplit.Values.CopyTo(leftValues, 0, pivotIndex + 1);
            leafToSplit.Values.CopyTo(rightValues, pivotIndex + 1, leafToSplit.Values.Count);

            BPlusTreeNode<TKey, TValue> leftNode = new BPlusTreeNode<TKey, TValue>(leftValues,
                leafToSplit.ParentNode, null, null, leafToSplit.ParentTree, false);
            BPlusTreeNode<TKey, TValue> rightNode = new BPlusTreeNode<TKey, TValue>(rightValues,
                leafToSplit.ParentNode, null, null, leafToSplit.ParentTree, false);

            leftNode.RightLeafNode = rightNode;
            leftNode.LeftLeafNode = leafToSplit.LeftLeafNode;

            rightNode.LeftLeafNode = leftNode;
            rightNode.RightLeafNode = leafToSplit.RightLeafNode;

            if (leafToSplit.LeftLeafNode != null)
            {
                leafToSplit.LeftLeafNode.RightLeafNode = leftNode;
            }
            if (leafToSplit.RightLeafNode != null)
            {
                leafToSplit.RightLeafNode.LeftLeafNode = rightNode;
            }



            if (!leafToSplit.IsRoot)
            {
                leafToSplit.ParentNode.Links.Remove(leafToSplit.Values.Keys.Max());

                leafToSplit.ParentNode.Links.Add(rightNode.Values.Keys.Last(), rightNode);
                leafToSplit.ParentNode.Links.Add(leftNode.Values.Keys.Last(), leftNode);
            }
            else
            {
                BPlusTreeNode<TKey, TValue> newRoot =
                    new BPlusTreeNode<TKey, TValue>(new BPlusTreeNodeSortedLinks<TKey, TValue>(), null, null, null,
                        leafToSplit.ParentTree, true);

                leftNode.ParentNode = newRoot;
                rightNode.ParentNode = newRoot;

                newRoot.Links.Add(leftNode.Values.Keys.Last(), leftNode);
                newRoot.Links.Add(rightNode.Values.Keys.Last(), rightNode);

                Head = newRoot;
            }

            if (leftNode.ParentNode.KeysCount > MaxDegree)
            {
                Split(leftNode.ParentNode);
            }
        }
        protected void SplitNode(BPlusTreeNode<TKey, TValue> nodeToSplit)
        {
            int pivotIndex = nodeToSplit.Links.Count / 2;

            BPlusTreeNodeSortedLinks<TKey, TValue> leftLinks = new BPlusTreeNodeSortedLinks<TKey, TValue>();
            BPlusTreeNodeSortedLinks<TKey, TValue> rightLinks = new BPlusTreeNodeSortedLinks<TKey, TValue>();

            nodeToSplit.Links.CopyTo(leftLinks, 0, pivotIndex + 1);
            nodeToSplit.Links.CopyTo(rightLinks, pivotIndex + 1, nodeToSplit.Links.Count);

            BPlusTreeNode<TKey, TValue> leftNode = new BPlusTreeNode<TKey, TValue>(leftLinks, nodeToSplit.ParentNode,
                null, null, nodeToSplit.ParentTree, false);
            BPlusTreeNode<TKey, TValue> rightNode = new BPlusTreeNode<TKey, TValue>(rightLinks, nodeToSplit.ParentNode,
                null, null, nodeToSplit.ParentTree, false);

            leftNode.RightNode = rightNode;
            leftNode.LeftNode = nodeToSplit.LeftNode;

            rightNode.LeftNode = leftNode;
            rightNode.RightNode = nodeToSplit.RightNode;

            if (nodeToSplit.LeftNode != null)
            {
                nodeToSplit.LeftNode.RightNode = leftNode;
            }
            if (nodeToSplit.RightNode != null)
            {
                nodeToSplit.RightNode.LeftNode = rightNode;
            }



            if (!nodeToSplit.IsRoot)
            {
                nodeToSplit.ParentNode.Links.Remove(nodeToSplit.Links.Keys.Max());

                nodeToSplit.ParentNode.Links.Add(rightNode.Links.Keys.Last(), rightNode);
                nodeToSplit.ParentNode.Links.Add(leftNode.Links.Keys.Last(), leftNode);
            }
            else
            {
                BPlusTreeNode<TKey, TValue> newRoot =
                    new BPlusTreeNode<TKey, TValue>(new BPlusTreeNodeSortedLinks<TKey, TValue>(), null, null, null,
                        nodeToSplit.ParentTree, true);

                leftNode.ParentNode = newRoot;
                rightNode.ParentNode = newRoot;

                newRoot.Links.Add(leftNode.Links.Keys.Last(), leftNode);
                newRoot.Links.Add(rightNode.Links.Keys.Last(), rightNode);

                Head = newRoot;
            }

            if (leftNode.ParentNode.KeysCount > MaxDegree)
            {
                Split(leftNode.ParentNode);
            }
        }


        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        //TODO
        public void Add(KeyValuePair<TKey, TValue> pair)
        {
            BPlusTreeNode<TKey, TValue> current = FindLeafThatMightContainKey(pair.Key);

            if (current != null)
            {
                if (current.Values.ContainsKey(pair.Key))
                {
                    throw new ArgumentException("An entry with the same key already exists.");                    
                }


                if (current.Values.Last().Key.CompareTo(pair.Key) < 0 && current.ParentNode != null)
                {
                    TKey oldKey = current.Values.Keys.Last();
                    current.Values.Add(pair.Key, pair.Value);

                    //BPlusTreeNode<TKey, TValue> oldLinkValue;
                    //current.ParentNode.Links.TryGetValue(oldKey, out oldLinkValue);

                    current.ParentNode.Links.Remove(oldKey);
                    current.ParentNode.Links.Add(pair.Key, current);

                    BPlusTreeNode<TKey, TValue> parent = current.ParentNode;

                    while (parent.ParentNode != null)
                    {
                        parent.ParentNode.Links.Remove(oldKey);
                        parent.ParentNode.Links.Add(pair.Key, parent);

                        parent = parent.ParentNode;
                    }
                }
                else
                {
                    current.Values.Add(pair.Key, pair.Value);
                }

                if (current.KeysCount > MaxDegree)
                {
                    Split(current);
                }
            }
            else
            {
                BPlusTreeNodeSortedValues<TKey, TValue> list = new BPlusTreeNodeSortedValues<TKey, TValue>();
                list.Add(pair.Key, pair.Value);
                Head = new BPlusTreeNode<TKey, TValue>(list, null, null, null, this, true);

                NodesCount++;
            }

            Count++;
        }

        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            foreach (KeyValuePair<TKey, TValue> pair in collection)
            {
                Add(pair);
            }
        }



        public void Clear()
        {
            Count = 0;
            NodesCount = 0;
            Head = null;
        }
        //TODO
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }



        public bool Find(TKey key , out TValue value)
        {
            BPlusTreeNode<TKey, TValue> leaf = FindLeafThatMightContainKey(key);

            if (leaf != null && FindValueByKeyInLeaf(leaf, key, out value))
            {
                return true;
            }

            value = default (TValue);
            return false;
        }

        public bool Contains(KeyValuePair<TKey, TValue> pair)
        {
            TValue tempValue;

            if (Find(pair.Key, out tempValue) && EqualityComparer<TValue>.Default.Equals(tempValue, pair.Value))
            {
                return true;
            }

            return false;
        }



        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (Head != null)
            {
                BPlusTreeNode<TKey, TValue> current = Head;

                while (!current.IsLeaf)
                {
                    current = current.Links.Values[0];
                }

                do
                {
                    foreach (KeyValuePair<TKey, TValue> pair in current.Values)
                    {
                        array[arrayIndex++] = pair;
                    }

                    current = current.RightLeafNode;
                } while (current != null);
            }
        }



        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (Head != null)
            {
                BPlusTreeNode<TKey, TValue> current = Head;

                while (!current.IsLeaf)
                {
                    current = current.Links.Values[0];
                }

                do
                {
                    foreach (KeyValuePair<TKey, TValue> pair in current.Values)
                    {
                        yield return pair;
                    }

                    current = current.RightLeafNode;
                } while (current != null);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}