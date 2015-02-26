using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    public class BinarySearchTreeNode<TNode> : BinaryTreeNode<TNode> where TNode : IComparable<TNode>
    {
        public BinarySearchTreeNode<TNode> Left { get; set; }

        public BinarySearchTreeNode<TNode> Right { get; set; }



        public BinarySearchTreeNode(TNode value)
            : base(value)
        {

        }
    }
}
