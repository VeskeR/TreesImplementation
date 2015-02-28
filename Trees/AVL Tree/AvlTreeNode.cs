using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    [Serializable]
    public class AvlTreeNode<TNode> : BinaryTreeNode<TNode> where TNode : IComparable<TNode>
    {
        protected AvlTreeNode<TNode> _left;
        protected AvlTreeNode<TNode> _right;



        public AvlTreeNode<TNode> Left
        {
            get
            {
                return _left;
            }
            internal set
            {
                _left = value;

                if (_left != null)
                {
                    _left.Parent = this;
                }
            }
        }

        public AvlTreeNode<TNode> Right
        {
            get
            {
                return _right;
            }
            internal set
            {
                _right = value;

                if (_right != null)
                {
                    _right.Parent = this;
                }
            }
        }

        public AvlTreeNode<TNode> Parent { get; internal set; }

        public AvlTree<TNode> Tree { get; private set; }



        protected TreeState State
        {
            get
            {
                if ((LeftHeight - RightHeight) > 1)
                {
                    return TreeState.LeftHeavy;
                }

                if ((RightHeight - LeftHeight) > 1)
                {
                    return TreeState.RightHeavy;
                }

                return TreeState.Balanced;
            }
        }

        protected int RightHeight
        {
            get
            {
                return MaxChildHeight(Right);
            }
        }

        protected int LeftHeight
        {
            get
            {
                return MaxChildHeight(Left);
            }
        }

        protected int BalanceFactor
        {
            get
            {
                return RightHeight - LeftHeight;
            }
        }



        public AvlTreeNode(TNode value, AvlTreeNode<TNode> parent, AvlTree<TNode> tree)
            : base(value)
        {
            Parent = parent;
            Tree = tree;
        }



        internal void Balance()
        {
            if (State == TreeState.RightHeavy)
            {
                if (Right != null && Right.BalanceFactor < 0)
                {
                    RightLeftRotation();
                }
                else
                {
                    LeftRotation();
                }
            }
            else if (State == TreeState.LeftHeavy)
            {
                if (Left != null && Left.BalanceFactor > 0)
                {
                    LeftRightRotation();
                }
                else
                {
                    RightRotation();
                }
            }
        }

        protected int MaxChildHeight(AvlTreeNode<TNode> node)
        {
            if (node != null)
            {
                return 1 + Math.Max(MaxChildHeight(node.Left), MaxChildHeight(node.Right));
            }

            return 0;
        }

        protected void LeftRotation()
        {
            AvlTreeNode<TNode> newRoot = this.Right;
            
            ReplaceRoot(newRoot);

            this.Right = newRoot.Left;

            newRoot.Left = this;
        }

        protected void RightRotation()
        {
            AvlTreeNode<TNode> newRoot = this.Left;

            ReplaceRoot(newRoot);

            this.Left = newRoot.Right;

            newRoot.Right = this;
        }

        protected void LeftRightRotation()
        {
            Left.LeftRotation();

            RightRotation();
        }

        protected void RightLeftRotation()
        {
            Right.RightRotation();

            LeftRotation();
        }

        protected void ReplaceRoot(AvlTreeNode<TNode> newRoot)
        {
            if (this.Parent != null)
            {
                if (this.Parent.Right == this)
                {
                    this.Parent.Right = newRoot;
                }
                else if (this.Parent.Left == this)
                {
                    this.Parent.Left = newRoot;
                }
            }
            else
            {
                this.Tree.Head = newRoot;
            }

            newRoot.Parent = this.Parent;
            this.Parent = newRoot;
        }
    }
}
