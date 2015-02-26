using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    public abstract class BinaryTreeNode<TNode> : IComparable<TNode> where TNode : IComparable<TNode>
    {
        protected TNode _value;

        protected BinaryTreeNode<TNode> _left;
        protected BinaryTreeNode<TNode> _right;

        public virtual TNode Value
        {
            get
            {
                return _value;
            }
            protected set
            {
                _value = value;
            }
        }

        public virtual BinaryTreeNode<TNode> Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
            }
        }

        public virtual BinaryTreeNode<TNode> Right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;
            }
        }


        public BinaryTreeNode(TNode value)
        {
            _value = value;
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
