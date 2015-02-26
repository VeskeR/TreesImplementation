using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
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



        public abstract IEnumerator<T> InOrderTraversal();

        public virtual IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
