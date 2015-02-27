using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTreesLib;

namespace TestingNodes
{
    static class MyList
    {
        static public void AddSorted<TValue>(this List<TValue> list, TValue item)
        {
            list.Add(item);
            list.Sort();
        }

        static public void AddRangeSorted<TValue>(this List<TValue> list, IEnumerable<TValue> collection)
        {
            list.AddRange(collection);
            list.Sort();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int>(new[] {651,3,534,652,346,23,46,234,5,143,51,4,6523,6,2});

            list.AddSorted(5);

            foreach (var i in list)
            {
                Console.WriteLine(i);
            }

            Console.ReadLine();
        }

        static void printTree<T>(IEnumerable<T> searchTree) where T: IComparable<T>
        {
            foreach (T item in searchTree)
            {
                Console.Write("{0}, ", item.ToString());
            }
            Console.WriteLine();
        }
    }
}
