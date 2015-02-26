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
            AvlTree<int> avlTree = new AvlTree<int>(new[] {10,3,2,4,12,15,11,25});

            avlTree.Remove(11);

            avlTree.Clear();

            avlTree.AddRange(Enumerable.Range(1, 256));

            printTree(avlTree);

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
