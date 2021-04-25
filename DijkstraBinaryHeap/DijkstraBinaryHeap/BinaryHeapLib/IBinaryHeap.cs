using System.Collections.Generic;

namespace DijkstraBinaryHeap.BinaryHeapLib
{
    public interface IBinaryHeap<T>
    {
        void Push( T element );
        void Pop();
    }
}
