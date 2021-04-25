using System.Collections.Generic;

namespace DijkstraBinaryHeap
{
    public class ShortWay
    {
        public int ShortestPathLength { get; set; }
        public List<int> CitiesSequenceAlongPath { get; set; }

        public string DestinationPath { get; set; }

        public bool IsWayNotFound { get; set; }
    }
}
