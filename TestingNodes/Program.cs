using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MyBTreesLib;
using MyTreesLib;
using SortingAlgorithmsLib;

namespace TestingNodes
{
    class Program
    {
        static void Main(string[] args)
        {
            int j = 0;

            Random rand = new Random();
            Stopwatch sw = new Stopwatch();

            Console.Write("Enter count: ");
            int count = int.Parse(Console.ReadLine());
            Console.Write("Enter degree: ");
            int degree = int.Parse(Console.ReadLine());
            Console.Write("Enter alpha: ");
            double alpha = double.Parse(Console.ReadLine());


            BPlusTree<int, int> tree = new BPlusTree<int, int>(degree, alpha);




            Console.Clear();

            if (Console.ReadLine().ToLower() == "y")
            {
                sw.Reset();
                sw.Start();
                for (int i = 0; i < count; i++)
                {
                    tree.Add(i * 10, i * 10);
                }
                sw.Stop();
                Console.WriteLine("Adding ascending sequence takes {0:f3} s", (double)sw.ElapsedMilliseconds / 1000);


                sw.Reset();
                sw.Start();
                for (int i = 0; i < count; i++)
                {
                    tree.Find(rand.Next(count), out j);
                }
                sw.Stop();
                Console.WriteLine("Finding in ascending sequence takes {0:f3} s", (double)sw.ElapsedMilliseconds / 1000);

                GetObjectSize(tree);


                sw.Reset();
                sw.Start();
                for (int i = 0; i < count; i++)
                {
                    tree.Remove(i * 10);
                }
                sw.Stop();
                Console.WriteLine("Removing from ascending sequence takes {0:f3} s", (double)sw.ElapsedMilliseconds / 1000);
            }



            if (Console.ReadLine().ToLower() == "y")
            {
                List<int> elements = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    elements.Add(i * 10);
                }

                Sort<int>.SortOut(elements);
                Sort<int>.SortOut(elements);
                Sort<int>.SortOut(elements);
                Sort<int>.SortOut(elements);
                Sort<int>.SortOut(elements);

                sw.Reset();
                sw.Start();
                for (int i = 0; i < count; i++)
                {
                    tree.Add(elements[i], elements[i]);
                }
                sw.Stop();
                Console.WriteLine("Adding random sequence takes {0:f3} s", (double)sw.ElapsedMilliseconds / 1000);


                sw.Reset();
                sw.Start();
                for (int i = 0; i < count; i++)
                {
                    tree.Find(rand.Next(count), out j);
                }
                sw.Stop();
                Console.WriteLine("Finding in ascending sequence takes {0:f3} s", (double)sw.ElapsedMilliseconds / 1000);

                GetObjectSize(tree);


                sw.Reset();
                sw.Start();
                for (int i = 0; i < count; i++)
                {
                    tree.Remove(elements[i]);
                }
                sw.Stop();
                Console.WriteLine("Removing from ascending sequence takes {0:f3} s", (double)sw.ElapsedMilliseconds / 1000);





                Console.ReadLine();
            }
            
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
