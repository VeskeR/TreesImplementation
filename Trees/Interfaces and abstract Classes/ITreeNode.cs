using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    interface ITreeNode<TNode> : IComparable<TNode> where TNode : IComparable<TNode>
    {
        TNode Value { get; set; }
    }
}
