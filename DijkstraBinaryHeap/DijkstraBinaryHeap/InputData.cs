using System;
using System.Collections.Generic;

namespace DijkstraBinaryHeap
{
    public struct InputData
    {
        public Node[] Nodes { get; set; }

        public Dictionary<int, int> DistancesByNodeName { get; set; }

        public Node StartNode { get; set; }

        public Node DistanceNode { get; set; }
    }
}
