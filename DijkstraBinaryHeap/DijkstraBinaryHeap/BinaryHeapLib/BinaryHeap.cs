using System.Collections.Generic;

namespace DijkstraBinaryHeap.BinaryHeapLib
{
    public class BinaryHeap : IBinaryHeap
    {
        private List<int> _heap = new List<int>();

        public int Length => _heap.Count;

        public bool IsEmpty => _heap.Count == 0;

        public BinaryHeap()
        {

        }

        public BinaryHeap(IEnumerable<int> elements)
        {

        }

        public void Delete(int element)
        {

        }

        public void Add(int element)
        {

        }

        private void BubbleUp()
        {

        }

        private void BubbleDown()
        {

        }
    }
}
