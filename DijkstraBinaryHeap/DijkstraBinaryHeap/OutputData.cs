using System.Collections.Generic;

namespace DijkstraBinaryHeap
{
    public struct OutputData
    {
        public int ShortestPathLength { get; set; }
        public List<int> CitiesSequenceAlongPath { get; set; }

        public bool IsWayNotFound { get; set; }
    }
}
