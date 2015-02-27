using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    public class BinarySearchTree<T> : BinaryTree<T> where T : IComparable<T>
    {
        public BinarySearchTreeNode<T> Head { get; protected set; }



        public BinarySearchTree()
        {
            
        }

        public BinarySearchTree(IEnumerable<T> collection)
            :base(collection)
        {

        }



        public override sealed void Add(T value)
        {
            if (Head == null)
            {
                Head = new BinarySearchTreeNode<T>(value);
            }
            else
            {
                AddTo(Head, value);
            }

            Count++;
        }

        protected void AddTo(BinarySearchTreeNode<T> node, T value)
        {
            if (value.CompareTo(node.Value) < 0)
            {
                if (node.Left == null)
                {
                    node.Left = new BinarySearchTreeNode<T>(value);
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
                    node.Right = new BinarySearchTreeNode<T>(value);
                }
                else
                {
                    AddTo(node.Right, value);
                }
            }
        }



        public override sealed bool Contains(T value)
        {
            BinarySearchTreeNode<T> parent;
            return FindWithParent(value, out parent) != null;
        }

        protected BinarySearchTreeNode<T> FindWithParent(T value, out BinarySearchTreeNode<T> parent)
        {
            BinarySearchTreeNode<T> current = Head;
            parent = null;

            while (current != null)
            {
                int compareResult = current.CompareTo(value);

                if (compareResult > 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (compareResult < 0)
                {
                    parent = current;
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
            BinarySearchTreeNode<T> parent;
            BinarySearchTreeNode<T> current = FindWithParent(value, out parent);

            if (current == null)
            {
                return false;
            }

            Count--;

            if (current.Right == null)
            {
                if (parent == null)
                {
                    Head = current.Left;
                }
                else
                {
                    int compareResult = parent.CompareTo(current.Value);

                    if (compareResult > 0)
                    {
                        parent.Left = current.Left;
                    }
                    else if (compareResult < 0)
                    {
                        parent.Right = current.Left;
                    }
                }
            }
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                if (parent == null)
                {
                    Head = current.Right;
                }
                else
                {
                    int compareResult = parent.CompareTo(current.Value);

                    if (compareResult > 0)
                    {
                        parent.Left = current.Right;
                    }
                    else if (compareResult < 0)
                    {
                        parent.Right = current.Right;
                    }
                }
            }
            else
            {
                BinarySearchTreeNode<T> leftmost = current.Right.Left;
                BinarySearchTreeNode<T> leftmostParent = current.Right;

                while (leftmost.Left != null)
                {
                    leftmostParent = leftmost;
                    leftmost = leftmost.Left;
                }

                leftmostParent.Left = leftmost.Right;

                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                if (parent == null)
                {
                    Head = leftmost;
                }
                else
                {
                    int compareResult = parent.CompareTo(current.Value);

                    if (compareResult > 0)
                    {
                        parent.Left = leftmost;
                    }
                    else if (compareResult < 0)
                    {
                        parent.Right = leftmost;
                    }
                }
            }

            return true;
        }

        public override sealed void Clear()
        {
            Head = null;
            Count = 0;
        }



        protected override sealed IEnumerable<T> InOrderTraversal()
        {
            if (Head != null)
            {
                Stack<BinarySearchTreeNode<T>> stack = new Stack<BinarySearchTreeNode<T>>();
                BinarySearchTreeNode<T> current = Head;

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

        protected override IEnumerable<T> PostOrderTraversal()
        {
            if (Head != null)
            {
                Stack<BinarySearchTreeNode<T>> stack = new Stack<BinarySearchTreeNode<T>>();
                BinarySearchTreeNode<T> current = Head;

                bool goLeftNext = true;
                bool goRightNext = true;

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


                    if (current.Right != null && goRightNext)
                    {
                        stack.Push(current);

                        current = current.Right;

                        goLeftNext = true;
                    }
                    else
                    {
                        yield return current.Value;

                        if (stack.First() != null && stack.First().Right == current)
                        {
                            goRightNext = false;
                        }
                        else
                        {
                            goRightNext = true;
                        }

                        current = stack.Pop();

                        goLeftNext = false;
                    }

                    
                }
            }
        }

        protected override IEnumerable<T> PreOrderTraversal()
        {
            if (Head != null)
            {
                Stack<BinarySearchTreeNode<T>> stack = new Stack<BinarySearchTreeNode<T>>();
                BinarySearchTreeNode<T> current = Head;

                bool goLeftNext = true;

                stack.Push(current);

                yield return current.Value;

                while (stack.Count > 0)
                {
                    if (goLeftNext)
                    {
                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;

                            yield return current.Value;
                        }
                    }

                    if (current.Right != null)
                    {
                        current = current.Right;

                        goLeftNext = true;

                        yield return current.Value;
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
