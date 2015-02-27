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
            BinarySearchTree<int> avlTree = new BinarySearchTree<int>();

            avlTree.AddRange(new[] { 4, 2, 5, 1, 3, 7, 6, 8 });

            printTree(avlTree);

            foreach (int i in avlTree.GetEnumerable(TreeTraversalOrder.PreOrder))
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
