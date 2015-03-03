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
        public BPlusTree(int maxDegree, double alpha)
            : this(null, maxDegree, alpha)
        {

        }
        public BPlusTree(IEnumerable<KeyValuePair<TKey, TValue>> collection, int maxDegree, double alpha)
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

                NodesCount++;
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

                NodesCount++;
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
        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            foreach (KeyValuePair<TKey, TValue> pair in collection)
            {
                Add(pair);
            }
        }

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



        protected void MergeOrPullLeaf(BPlusTreeNode<TKey, TValue> leaf)
        {
            if (leaf.RightLeafNode != null && leaf.LeftLeafNode != null)
            {
                BPlusTreeNode<TKey, TValue> minLeaf = (leaf.LeftLeafNode.KeysCount < leaf.RightLeafNode.KeysCount)
                ? leaf.LeftLeafNode
                : leaf.RightLeafNode;
                BPlusTreeNode<TKey, TValue> maxLeaf = (leaf.LeftLeafNode.KeysCount >= leaf.RightLeafNode.KeysCount)
                    ? leaf.LeftLeafNode
                    : leaf.RightLeafNode;

                if (leaf.KeysCount + minLeaf.KeysCount <= MaxDegree)
                {
                    if (leaf.RightLeafNode == minLeaf)
                    {
                        MergeLeaves(leaf, minLeaf);
                    }
                    else
                    {
                        MergeLeaves(minLeaf, leaf);
                    }
                }
                else
                {
                    PullLeaves(leaf, maxLeaf);                    
                }
            }
            else if (leaf.LeftLeafNode != null)
            {
                if (leaf.KeysCount + leaf.LeftLeafNode.KeysCount <= MaxDegree)
                {
                    MergeLeaves(leaf.LeftLeafNode, leaf);
                }
                else
                {
                    PullLeaves(leaf, leaf.LeftLeafNode);
                }
            }
            else if (leaf.RightLeafNode != null)
            {
                if (leaf.KeysCount + leaf.RightLeafNode.KeysCount <= MaxDegree)
                {
                    MergeLeaves(leaf, leaf.RightLeafNode);
                }
                else
                {
                    PullLeaves(leaf, leaf.RightLeafNode);
                }
            }
            else
            {
                if (!leaf.IsRoot)
                {
                    leaf.IsRoot = true;
                    leaf.ParentNode = null;
                }
            }
        }
        protected void MergeOrPullNode(BPlusTreeNode<TKey, TValue> node)
        {
            if (node.RightNode != null && node.LeftNode != null)
            {
                BPlusTreeNode<TKey, TValue> minNode = (node.LeftNode.KeysCount < node.RightNode.KeysCount)
                ? node.LeftNode
                : node.RightNode;
                BPlusTreeNode<TKey, TValue> maxNode = (node.LeftNode.KeysCount >= node.RightNode.KeysCount)
                    ? node.LeftNode
                    : node.RightNode;

                if (node.KeysCount + minNode.KeysCount <= MaxDegree)
                {
                    if (node.RightNode == minNode)
                    {
                        MergeNodes(node, minNode);
                    }
                    else
                    {
                        MergeNodes(minNode, node);
                    }
                }
                else
                {
                    PullNodes(node, maxNode);
                }
            }
            else if (node.LeftNode != null)
            {
                if (node.KeysCount + node.LeftNode.KeysCount <= MaxDegree)
                {
                    MergeNodes(node.LeftNode, node);
                }
                else
                {
                    PullNodes(node, node.LeftNode);
                }
            }
            else if (node.RightNode != null)
            {
                if (node.KeysCount + node.RightNode.KeysCount <= MaxDegree)
                {
                    MergeNodes(node, node.RightNode);
                }
                else
                {
                    PullNodes(node, node.RightNode);
                }
            }
            else
            {
                if (!node.IsRoot)
                {
                    node.IsRoot = true;
                    node.ParentNode = null;
                }
            }
        }

        protected void MergeLeaves(BPlusTreeNode<TKey, TValue> leftLeafToMerge,
            BPlusTreeNode<TKey, TValue> rightLeafToMerge)
        {

        }
        protected void MergeNodes(BPlusTreeNode<TKey, TValue> leftNodeToMerge,
            BPlusTreeNode<TKey, TValue> rightNodeToMerge)
        {

        }

        protected void PullLeaves(BPlusTreeNode<TKey, TValue> leafToPull, BPlusTreeNode<TKey, TValue> leafFromPull)
        {
            
        }
        protected void PullNodes(BPlusTreeNode<TKey, TValue> nodeToPull, BPlusTreeNode<TKey, TValue> nodeFromPull)
        {
            
        }



        public bool Remove(TKey key)
        {
            BPlusTreeNode<TKey, TValue> current = FindLeafThatMightContainKey(key);

            if (current != null)
            {
                if (current.Values.ContainsKey(key))
                {
                    if (current.Values.Keys.Max().CompareTo(key) == 0 && current.ParentNode != null)
                    {
                        current.Values.Remove(key);

                        current.ParentNode.Links.Remove(key);
                        current.ParentNode.Links.Add(current.Values.Keys.Max(), current);

                        BPlusTreeNode<TKey, TValue> parent = current.ParentNode;

                        while (parent.ParentNode != null)
                        {
                            parent.ParentNode.Links.Remove(key);
                            parent.ParentNode.Links.Add(current.Values.Keys.Max(), parent);

                            parent = parent.ParentNode;
                        }
                    }
                    else
                    {
                        current.Values.Remove(key);
                    }

                    if (current.KeysCount < MinDegree && !current.IsRoot)
                    {
                        if (current.IsLeaf)
                        {
                            MergeOrPullLeaf(current);
                        }
                        else
                        {
                            MergeOrPullNode(current);
                        }
                    }

                    return true;
                }
            }

            return false;
        }
        public bool Remove(KeyValuePair<TKey, TValue> pair)
        {
            if (Contains(pair))
            {
                Remove(pair.Key);
                return true;
            }

            return false;
        }


        public void Clear()
        {
            Count = 0;
            NodesCount = 0;
            Head = null;
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