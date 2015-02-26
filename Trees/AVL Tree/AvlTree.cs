using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    public class AvlTree<T> : BinaryTree<T> where T : IComparable<T>
    {
        public AvlTreeNode<T> Head { get; set; }



        public AvlTree()
        {
            
        }

        public AvlTree(IEnumerable<T> collection) : base(collection)
        {
            
        }



        public override sealed void Add(T value)
        {
            if (Head == null)
            {
                Head = new AvlTreeNode<T>(value, null, this);
            }
            else
            {
                AddTo(Head, value);
            }

            Count++;
        }

        protected void AddTo(AvlTreeNode<T> node, T value)
        {
            if (value.CompareTo(node.Value) < 0)
            {
                if (node.Left == null)
                {
                    node.Left = new AvlTreeNode<T>(value, node, this);
                }
                else
                {
                    AddTo(node.Left, value);
                }
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = new AvlTreeNode<T>(value, node, this);
                }
                else
                {
                    AddTo(node.Right, value);
                }
            }
        }



        public override sealed bool Contains(T value)
        {
            return Find(value) != null;
        }

        protected AvlTreeNode<T> Find(T value)
        {
            AvlTreeNode<T> current = Head;

            while (current != null)
            {
                int compareResult = current.CompareTo(value);

                if (compareResult > 0)
                {
                    current = current.Left;
                }
                else if (compareResult < 0)
                {
                    current = current.Right;                    
                }
                else
                {
                    break;
                }
            }

            return current;
        }



        public override sealed bool Remove(T value)
        {
            throw new NotImplementedException();
        }

        public override sealed void Clear()
        {
            Head = null;
            Count = 0;
        }



        public override IEnumerator<T> InOrderTraversal()
        {
            if (Head != null)
            {
                Stack<AvlTreeNode<T>> stack = new Stack<AvlTreeNode<T>>();
                AvlTreeNode<T> current = Head;

                bool goLeftNext = true;

                stack.Push(current);

                while (stack.Count > 0)
                {
                    if (goLeftNext)
                    {
                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;
                        }
                    }

                    yield return current.Value;

                    if (current.Right != null)
                    {
                        current = current.Right;

                        goLeftNext = true;
                    }
                    else
                    {
                        current = stack.Pop();

                        goLeftNext = false;
                    }
                }
            }
        }
    }
}
