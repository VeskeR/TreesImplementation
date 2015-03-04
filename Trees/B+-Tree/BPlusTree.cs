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
            protected set { _maxDegree = value < 4 ? 4 : value; }
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
                return ((int) (MaxDegree*Alpha) >= 2) ? (int) (MaxDegree*Alpha) : 2;
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

                    Head = leaf;

                    NodesCount--;
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

                    Head = node;

                    NodesCount--;
                }
            }
        }

        protected void MergeLeaves(BPlusTreeNode<TKey, TValue> leftLeafToMerge,
            BPlusTreeNode<TKey, TValue> rightLeafToMerge)
        {
            NodesCount--;

            BPlusTreeNode<TKey, TValue> newLeaf =
                new BPlusTreeNode<TKey, TValue>(new BPlusTreeNodeSortedValues<TKey, TValue>(), null, null, null,
                    leftLeafToMerge.ParentTree, false);

            BPlusTreeNodeSortedValues<TKey, TValue> mergedValues = new BPlusTreeNodeSortedValues<TKey, TValue>();

            mergedValues.AddRange(leftLeafToMerge.Values);
            mergedValues.AddRange(rightLeafToMerge.Values);

            newLeaf.Values = mergedValues;

            newLeaf.ParentNode = rightLeafToMerge.ParentNode;

            newLeaf.LeftLeafNode = leftLeafToMerge.LeftLeafNode;
            newLeaf.RightLeafNode = rightLeafToMerge.RightLeafNode;

            if (newLeaf.LeftLeafNode != null)
            {
                newLeaf.LeftLeafNode.RightLeafNode = newLeaf;
            }
            if (newLeaf.RightLeafNode != null)
            {
                newLeaf.RightLeafNode.LeftLeafNode = newLeaf;
            }

            newLeaf.ParentNode.Links.Remove(rightLeafToMerge.Values.Keys.Max());
            newLeaf.ParentNode.Links.Add(newLeaf.Values.Keys.Max(), newLeaf);

            leftLeafToMerge.ParentNode.Links.Remove(leftLeafToMerge.Values.Keys.Max());

            if (leftLeafToMerge.ParentNode != rightLeafToMerge.ParentNode)
            {
                leftLeafToMerge.ParentNode.ParentNode.Links.Remove(leftLeafToMerge.Values.Keys.Max());
                leftLeafToMerge.ParentNode.ParentNode.Links.Add(leftLeafToMerge.ParentNode.Links.Keys.Max(),
                    leftLeafToMerge.ParentNode);

                if (leftLeafToMerge.ParentNode.KeysCount < MinDegree && !leftLeafToMerge.ParentNode.IsRoot)
                {
                    MergeOrPullNode(leftLeafToMerge.ParentNode);
                }
            }

            if (newLeaf.ParentNode.KeysCount < MinDegree && !newLeaf.ParentNode.IsRoot)
            {
                MergeOrPullNode(newLeaf.ParentNode);
            }
            else if (newLeaf.ParentNode.IsRoot && newLeaf.ParentNode.KeysCount == 1)
            {
                newLeaf.IsRoot = true;
                newLeaf.ParentNode = null;

                Head = newLeaf;

                NodesCount--;
            }
        }
        protected void MergeNodes(BPlusTreeNode<TKey, TValue> leftNodeToMerge,
            BPlusTreeNode<TKey, TValue> rightNodeToMerge)
        {
            NodesCount--;

            BPlusTreeNode<TKey, TValue> newNode =
                new BPlusTreeNode<TKey, TValue>(new BPlusTreeNodeSortedLinks<TKey, TValue>(), null, null, null,
                    leftNodeToMerge.ParentTree, false);

            BPlusTreeNodeSortedLinks<TKey, TValue> mergedLinks = new BPlusTreeNodeSortedLinks<TKey, TValue>();

            mergedLinks.AddRange(newNode, leftNodeToMerge.Links);
            mergedLinks.AddRange(newNode, rightNodeToMerge.Links);

            newNode.Links = mergedLinks;

            newNode.ParentNode = rightNodeToMerge.ParentNode;

            newNode.LeftNode = leftNodeToMerge.LeftNode;
            newNode.RightNode = rightNodeToMerge.RightNode;

            if (newNode.RightNode != null)
            {
                newNode.RightNode.LeftNode = newNode;
            }
            if (newNode.LeftNode != null)
            {
                newNode.LeftNode.RightNode = newNode;
            }

            newNode.ParentNode.Links.Remove(rightNodeToMerge.Links.Keys.Max());
            newNode.ParentNode.Links.Add(newNode.Links.Keys.Max(), newNode);

            leftNodeToMerge.ParentNode.Links.Remove(leftNodeToMerge.Links.Keys.Max());

            if (leftNodeToMerge.ParentNode != rightNodeToMerge.ParentNode)
            {
                leftNodeToMerge.ParentNode.ParentNode.Links.Remove(leftNodeToMerge.Links.Keys.Max());
                leftNodeToMerge.ParentNode.ParentNode.Links.Add(leftNodeToMerge.ParentNode.Links.Keys.Max(),
                    leftNodeToMerge.ParentNode);

                if (leftNodeToMerge.ParentNode.KeysCount < MinDegree && !leftNodeToMerge.ParentNode.IsRoot)
                {
                    MergeOrPullNode(leftNodeToMerge.ParentNode);
                }
            }

            if (newNode.ParentNode.KeysCount < MinDegree && !newNode.ParentNode.IsRoot)
            {
                MergeOrPullNode(newNode.ParentNode);
            }
            else if (newNode.ParentNode.IsRoot && newNode.ParentNode.KeysCount == 1)
            {
                newNode.IsRoot = true;
                newNode.ParentNode = null;

                Head = newNode;

                NodesCount--;
            }
        }

        protected void PullLeaves(BPlusTreeNode<TKey, TValue> leafToPull, BPlusTreeNode<TKey, TValue> leafFromPull)
        {
            int numberOfValuesToPull = leafToPull.KeysCount + (leafFromPull.KeysCount - leafToPull.KeysCount)/2;

            if (leafToPull.RightLeafNode == leafFromPull)
            {
                BPlusTreeNodeSortedValues<TKey, TValue> pulledValues = new BPlusTreeNodeSortedValues<TKey, TValue>();

                for (int i = 0; i < numberOfValuesToPull; i++)
                {
                    pulledValues.Add(leafFromPull.Values.Keys[0], leafFromPull.Values.Values[0]);
                    leafFromPull.Values.RemoveAt(0);
                }

                TKey oldKey = leafToPull.Values.Keys.Max();

                leafToPull.Values.AddRange(pulledValues);

                leafToPull.ParentNode.Links.Remove(oldKey);
                leafToPull.ParentNode.Links.Add(leafToPull.Values.Keys.Max(), leafToPull);
            }
            else
            {
                BPlusTreeNodeSortedValues<TKey, TValue> pulledValues = new BPlusTreeNodeSortedValues<TKey, TValue>();

                TKey oldKey = leafFromPull.Values.Keys.Max();

                for (int i = 0; i < numberOfValuesToPull; i++)
                {
                    pulledValues.Add(leafFromPull.Values.Keys[leafFromPull.KeysCount - 1],
                        leafFromPull.Values.Values[leafFromPull.KeysCount - 1]);
                    leafFromPull.Values.RemoveAt(leafFromPull.KeysCount - 1);
                }

                leafToPull.Values.AddRange(pulledValues);

                leafFromPull.ParentNode.Links.Remove(oldKey);
                leafFromPull.ParentNode.Links.Add(leafFromPull.Values.Keys.Max(), leafFromPull);
            }
        }
        protected void PullNodes(BPlusTreeNode<TKey, TValue> nodeToPull, BPlusTreeNode<TKey, TValue> nodeFromPull)
        {
            int numberOfLinksToPull = nodeToPull.KeysCount + (nodeFromPull.KeysCount - nodeToPull.KeysCount) / 2;

            if (nodeToPull.RightNode == nodeFromPull)
            {
                BPlusTreeNodeSortedLinks<TKey, TValue> pulledLinks = new BPlusTreeNodeSortedLinks<TKey, TValue>();

                for (int i = 0; i < numberOfLinksToPull; i++)
                {
                    pulledLinks.Add(nodeFromPull.Links.Keys[0], nodeFromPull.Links.Values[0]);
                    nodeFromPull.Links.RemoveAt(0);
                }

                TKey oldKey = nodeToPull.Links.Keys.Max();

                nodeToPull.Links.AddRange(nodeToPull, pulledLinks);

                nodeToPull.ParentNode.Links.Remove(oldKey);
                nodeToPull.ParentNode.Links.Add(nodeToPull.Links.Keys.Max(), nodeToPull);
            }
            else
            {
                BPlusTreeNodeSortedLinks<TKey, TValue> pulledLinks = new BPlusTreeNodeSortedLinks<TKey, TValue>();

                TKey oldKey = nodeFromPull.Links.Keys.Max();

                for (int i = 0; i < numberOfLinksToPull; i++)
                {
                    pulledLinks.Add(nodeFromPull.Links.Keys[nodeFromPull.KeysCount - 1],
                        nodeFromPull.Links.Values[nodeFromPull.KeysCount - 1]);
                    nodeFromPull.Links.RemoveAt(nodeFromPull.KeysCount - 1);
                }

                nodeToPull.Links.AddRange(nodeToPull, pulledLinks);

                nodeFromPull.ParentNode.Links.Remove(oldKey);
                nodeFromPull.ParentNode.Links.Add(nodeFromPull.Links.Keys.Max(), nodeFromPull);
            }
        }



        public bool Remove(TKey key)
        {
            BPlusTreeNode<TKey, TValue> current = FindLeafThatMightContainKey(key);

            if (current != null)
            {
                if (current.Values.ContainsKey(key))
                {
                    Count--;

                    if (current.Values.Keys.Max().CompareTo(key) == 0 && current.ParentNode != null)
                    {
                        current.Values.Remove(key);

                        current.ParentNode.Links.Remove(key);
                        current.ParentNode.Links.Add(current.Values.Keys.Max(), current);

                        BPlusTreeNode<TKey, TValue> parent = current.ParentNode;

                        while (parent.Links.Keys.Max().CompareTo(current.Values.Keys.Max()) == 0 &&
                               parent.ParentNode != null)
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

                    if (current.IsRoot && current.KeysCount == 0)
                    {
                        NodesCount--;
                        Head = null;
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