using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using MyBTreesLib;
using MyTreesLib;

namespace TestingNodes
{
    class Program
    {
        static void Main(string[] args)
        {
            BPlusTree<int, int> bTree = new BPlusTree<int, int>(1000, 0.5);

            for (int i = 0; i < 20; i++)
            {
                bTree.Add(i, i);
            }

            for (int i = 0; i < 15; i++)
            {
                bTree.Remove(i);
            }

            foreach (KeyValuePair<int, int> pair in bTree)
            {
                Console.WriteLine(pair);
            }

            //List<int> arr = Enumerable.Range(0, 10000).ToList();

            //List<int> list = new List<int>(arr);
            //Console.WriteLine("List finished");
            //var bst = new BinarySearchTree<int>();

            //bst.AddRange(arr);

            //Console.WriteLine("Tree finished");

            //GetObjectSize(list);
            //GetObjectSize(bst);

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

        static long GetObjectSize(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, obj);

            Console.WriteLine("{0} of type {1} consumes {2} bytes", obj.GetType().Name, obj.GetType(), ms.Length);

            return ms.Length;
        }
    }
}
