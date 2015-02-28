using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    interface ITree<T> : IEnumerable<T> where T : IComparable<T>
    {
        int Count { get; }



        void Add(T value);

        void AddRange(IEnumerable<T> collection);



        bool Contains(T value);



        bool Remove(T value);

        void Clear();
    }
}
