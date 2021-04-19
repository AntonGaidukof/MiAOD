using System;
using System.Collections.Generic;

namespace DijkstraBinaryHeap
{
    public struct InputData
    {
        public int CitiesAmount { get; set; }
        public int RoadsAmount { get; set; }
        public int StartCityIndex { get; set; }
        public int EndCityIndex { get; set; }
        public List<Tuple<int, int, int>> WayData { get; set; }

        public Dictionary<int, Dictionary<int, int>> GetWaysMap()
        {
            Dictionary<int, Dictionary<int, int>> waysMap = new Dictionary<int, Dictionary<int, int>>();
            
            foreach (var (startCityIndex, endCityIndex, wayDistance) in WayData)
            {
                if ( !waysMap.ContainsKey( startCityIndex ) )
                {
                    waysMap[ startCityIndex ] = new Dictionary<int, int>();
                }

                waysMap[ startCityIndex ][ endCityIndex ] = wayDistance;
            }

            return waysMap;
        }
    }
}
