using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    class AvlTreeNode<TNode> : BinaryTreeNode<TNode> where TNode : IComparable<TNode>
    {
        protected AvlTree<TNode> _tree;

        protected new AvlTreeNode<TNode> _left;
        protected new AvlTreeNode<TNode> _right;


        public new AvlTreeNode<TNode> Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;

                if (_left != null)
                {
                    _left.Parent = this;
                }
            }
        }

        public new AvlTreeNode<TNode> Right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;

                if (_right != null)
                {
                    _right.Parent = this;
                }
            }
        }

        public AvlTreeNode<TNode> Parent { get; protected set; }


        public AvlTreeNode(TNode value, AvlTreeNode<TNode> parent, AvlTree<TNode> tree)
            : base(value)
        {
            Parent = parent;
            _tree = tree;
        }
    }
}
