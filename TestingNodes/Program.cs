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
            BinarySearchTree<int> binSearchTree = new BinarySearchTree<int>();
            binSearchTree.AddRange(new[] {4,6,2,8,7,1,9,3,12,16,10,25,11,13,17,20,25,19});

            printTree(binSearchTree);

            binSearchTree.Remove(10);
            binSearchTree.Remove(25);

            printTree(binSearchTree);

            binSearchTree.AddRange(new[] {100,50,25,75});

            binSearchTree.Add(-5);
            binSearchTree.Add(-10);
            binSearchTree.Add(-25);

            printTree(binSearchTree);

            binSearchTree.Clear();

            printTree(binSearchTree);

            binSearchTree.AddRange(new[] {100,50,25,75});

            printTree(binSearchTree);

            Console.WriteLine(binSearchTree.Contains(100));
            Console.WriteLine(binSearchTree.Contains(200));

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
