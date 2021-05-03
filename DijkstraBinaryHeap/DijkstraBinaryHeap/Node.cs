
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DijkstraBinaryHeap
{
    public class Node : IComparable<Node>
    {
        public int Name { get; }

        public int Distance { get; set; }

        public List<Tuple<int, Node>> RelatedNodes { get; set; }

        public Node LastNode { get; set; }

        public Node( int name, int distance = int.MaxValue )
        {
            Name = name;
            Distance = distance;
            RelatedNodes = new List<Tuple<int, Node>>();
        }

        public int CompareTo( [AllowNull] Node other )
        {
            return other.Distance.CompareTo( Distance );
        }
    }
}
