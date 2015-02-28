using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using MyBTreesLib;
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
            SortedDictionary<int, int> dict = new SortedDictionary<int, int>();

            dict.Add(1,0);
            dict.Add(12,0);
            dict.Add(13,0);
            dict.Add(4,0);
            dict.Add(5,0);
            dict.Add(6,0);
            dict.Add(10,0);
            dict.Add(20,0);
            dict.Add(0,0);
            dict.Add(7,0);
            dict.Add(15,0);

            Console.WriteLine(dict.Keys.Max());
            

            printDict(dict);

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

        static void printDict<TKey, TValue>(SortedDictionary<TKey, TValue> dict)
        {
            foreach (var pair in dict)
            {
                Console.WriteLine("'{0}' = {1}", pair.Key, pair.Value);
            }
        }
    }
}
