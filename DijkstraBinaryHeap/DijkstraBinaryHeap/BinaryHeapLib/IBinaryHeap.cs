using System.Collections.Generic;

namespace DijkstraBinaryHeap.BinaryHeapLib
{
    public interface IBinaryHeap<T>
    {
        void Add( T element );
        void Delete( T element );
    }
}
