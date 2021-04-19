using System;

namespace DijkstraBinaryHeap
{
    public class WayData : IComparable<WayData>
    {
        public int CityStartIndex { get; set; }
        public int CityEndIndex { get; set; }
        public int WayDistance { get; set; }
        
        public int CompareTo(WayData other)
        {
            if ( ReferenceEquals( this, other ) ) return 0;
            if ( ReferenceEquals( null, other ) ) return 1;

            return WayDistance.CompareTo( other.WayDistance );
        }
    }
}
