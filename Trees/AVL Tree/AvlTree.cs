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
                AddTo(Head, value, true);
            }

            Count++;
        }

        public void AddWithoutBalance(T value)
        {
            if (Head == null)
            {
                Head = new AvlTreeNode<T>(value, null, this);
            }
            else
            {
                AddTo(Head, value, false);
            }

            Count++;
        }

        public void AddRangeWithoutBalance(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                AddWithoutBalance(item);
            }
        }

        protected void AddTo(AvlTreeNode<T> node, T value, bool balance)
        {
            if (value.CompareTo(node.Value) < 0)
            {
                if (node.Left == null)
                {
                    node.Left = new AvlTreeNode<T>(value, node, this);
                }
                else
                {
                    AddTo(node.Left, value, balance);
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
                    AddTo(node.Right, value, balance);
                }
            }

            if (balance) node.Balance();
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
            return _remove(value, true);
        }

        public bool RemoveWithoutBalance(T value, bool balance)
        {
            return _remove(value, false);
        }

        protected bool _remove(T value, bool balance)
        {
            AvlTreeNode<T> current = Find(value);

            if (current == null)
            {
                return false;
            }

            AvlTreeNode<T> treeToBalance = current.Parent;
            Count--;

            if (current.Right == null)
            {
                if (current.Parent == null)
                {
                    Head = current.Left;

                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    int compareResult = current.Parent.CompareNode(current);

                    if (compareResult > 0)
                    {
                        current.Parent.Left = current.Left;
                    }
                    else if (compareResult < 0)
                    {
                        current.Parent.Right = current.Left;
                    }
                }
            }
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                if (current.Parent == null)
                {
                    Head = current.Right;

                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    int compareResult = current.Parent.CompareNode(current);

                    if (compareResult > 0)
                    {
                        current.Parent.Left = current.Right;
                    }
                    else if (compareResult < 0)
                    {
                        current.Parent.Right = current.Right;
                    }
                }
            }
            else
            {
                AvlTreeNode<T> leftmost = current.Right.Left;

                while (leftmost.Left != null)
                {
                    leftmost = leftmost.Left;
                }

                leftmost.Parent.Left = leftmost.Right;

                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                if (current.Parent == null)
                {
                    Head = leftmost;

                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    int compareResult = current.Parent.CompareNode(current);

                    if (compareResult > 0)
                    {
                        current.Parent.Left = leftmost;
                    }
                    else if (compareResult < 0)
                    {
                        current.Parent.Right = leftmost;
                    }
                }
            }

            if (balance)
            {
                if (treeToBalance != null)
                {
                    treeToBalance.Balance();
                }
                else
                {
                    if (Head != null)
                    {
                        Head.Balance();
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



        protected override IEnumerable<T> InOrderTraversal()
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

        protected override IEnumerable<T> PostOrderTraversal()
        {
            if (Head != null)
            {
                Stack<AvlTreeNode<T>> stack = new Stack<AvlTreeNode<T>>();
                AvlTreeNode<T> current = Head;

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

                        if (current.Parent != null && current.Parent.Right == current)
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
                Stack<AvlTreeNode<T>> stack = new Stack<AvlTreeNode<T>>();
                AvlTreeNode<T> current = Head;

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
