using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    public abstract class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        public BinaryTreeNode<T> Head { get; protected set; }

        public virtual int Count { get; protected set; }


        public BinaryTree()
        {
            
        }

        public BinaryTree(IEnumerable<T> collection)
        {
            AddRange(collection);
        }


        public virtual void Add(T value)
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

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T value in collection)
            {
                Add(value);
            }
        }

        private void AddTo(BinaryTreeNode<T> node, T value)
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


        public virtual bool Contains(T value)
        {
            BinaryTreeNode<T> parent;
            return FindWithParent(value, out parent) != null;
        }


        private BinaryTreeNode<T> FindWithParent(T value, out BinaryTreeNode<T> parent)
        {
            BinaryTreeNode<T> current = Head;
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


        public bool Remove(T value)
        {
            BinaryTreeNode<T> current;
            BinaryTreeNode<T> parent;

            current = FindWithParent(value, out parent);

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
                BinaryTreeNode<T> leftmost = current.Right.Left;
                BinaryTreeNode<T> leftmostParent = current.Right;

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


        public virtual IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual IEnumerator<T> InOrderTraversal()
        {
            if (Head != null)
            {
                Stack<BinaryTreeNode<T>> stack = new Stack<BinaryTreeNode<T>>();
                BinaryTreeNode<T> current = Head;

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
