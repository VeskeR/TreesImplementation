using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    [Serializable]
    public abstract class BinaryTree<T> : ITree<T> where T : IComparable<T>
    {
        public int Count { get; protected set; }



        protected BinaryTree()
        {
            
        }

        protected BinaryTree(IEnumerable<T> collection)
        {
            AddRange(collection);
        }



        public abstract void Add(T value);

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T value in collection)
            {
                Add(value);
            }
        }



        public abstract bool Contains(T value);



        public abstract bool Remove(T value);

        public abstract void Clear();



        protected abstract IEnumerable<T> PostOrderTraversal();

        protected abstract IEnumerable<T> PreOrderTraversal();

        protected abstract IEnumerable<T> InOrderTraversal();

        public IEnumerable<T> GetEnumerable(TreeTraversalOrder order)
        {
            switch (order)
            {
                case TreeTraversalOrder.InOrder:
                    return InOrderTraversal();
                case TreeTraversalOrder.PostOrder:
                    return PostOrderTraversal();
                case TreeTraversalOrder.PreOrder:
                    return PreOrderTraversal();
            }

            return null;
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
