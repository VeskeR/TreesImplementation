using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTreesLib;

namespace TestingNodes
{
    class Program
    {
        static void Main(string[] args)
        {

            SortedDictionary<int, int> dict = new SortedDictionary<int, int>();
            dict.Add(5, 5);
            dict.Add(4, 4);
            dict.Add(2, 2);
            dict.Add(9, 9);
            dict.Add(15, 15);
            dict.Add(0, 0);
            dict.Add(10, 10);
            dict.Add(-9, -9);
            dict.Add(25, 25);

            foreach (KeyValuePair<int, int> pair in dict)
            {
                Console.WriteLine("'{0}' = {1}", pair.Key, pair.Value);
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
