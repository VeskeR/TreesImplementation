using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    public abstract class BinaryTreeNode<TNode> : ITreeNode<TNode> where TNode : IComparable<TNode>
    {
        public TNode Value { get; private set; }



        protected BinaryTreeNode(TNode value)
        {
            Value = value;
        }



        public int CompareTo(TNode other)
        {
            return Value.CompareTo(other);
        }

        public int CompareNode(BinaryTreeNode<TNode> other)
        {
            return Value.CompareTo(other.Value);
        }



        public static bool operator >(BinaryTreeNode<TNode> node1, BinaryTreeNode<TNode> node2)
        {
            return node1.CompareNode(node2) > 0;
        }

        public static bool operator <(BinaryTreeNode<TNode> node1, BinaryTreeNode<TNode> node2)
        {
            return node1.CompareNode(node2) < 0;
        }

        public static bool operator >=(BinaryTreeNode<TNode> node1, BinaryTreeNode<TNode> node2)
        {
            return node1.CompareNode(node2) >= 0;
        }

        public static bool operator <=(BinaryTreeNode<TNode> node1, BinaryTreeNode<TNode> node2)
        {
            return node1.CompareNode(node2) <= 0;
        }
    }
}
