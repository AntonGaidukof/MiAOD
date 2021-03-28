using DijkstraBinaryHeap.BinaryHeapLib;
using System.Collections.Generic;

namespace DijkstraBinaryHeap
{
    class Program
    {
        static void Main( string[] args )
        {
            var testList = new List<int> { 5, 11, 14, 7, 3, 8, 1, 9, 18, 6 };
            var binaryHeap = new BinaryHeap<int>( testList, BinaryHeapKind.Asc );

            binaryHeap.Print();
        }
    }
}
