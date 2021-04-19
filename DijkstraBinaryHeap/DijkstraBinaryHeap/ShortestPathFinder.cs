using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DijkstraBinaryHeap.BinaryHeapLib;

namespace DijkstraBinaryHeap
{
    public class ShortestPathFinder
    {
        private const int InfiniteInt = Int32.MaxValue;

        private string _outputFileName;
        private InputData _inputData;
        private readonly Dictionary<int, Dictionary<int, int>> _waysMap;
        private HashSet<int> _markedCities = new HashSet<int>();

        public ShortestPathFinder( InputData inputData )
        {
            _inputData = inputData;
            _waysMap = _inputData.GetWaysMap();
        }

        public OutputData GetShortestPathData( string outputFileName )
        {
            _outputFileName = outputFileName;

            _markedCities = new HashSet<int>();
            var unmarkedCities = new BinaryHeap<WayData>();
            unmarkedCities.Add( new WayData
            {
                CityStartIndex = _inputData.StartCityIndex, 
                CityEndIndex = _inputData.StartCityIndex, 
                WayDistance = 0
            } );

            while ( !_markedCities.Contains( _inputData.EndCityIndex ) )
            {
                if ( unmarkedCities.IsEmpty )
                {
                    return new OutputData { IsWayNotFound = true };
                }

                WayData wayData = unmarkedCities.First;
                _markedCities.Add( wayData.CityStartIndex );

                if ( !_waysMap.ContainsKey( wayData.CityStartIndex ) )
                {
                    continue;
                }

                var endCities = _waysMap[ wayData.CityStartIndex ];
                foreach ( var endCityData in endCities )
                {
                    
                }
            }

            return new OutputData
            {
                ShortestPathLength = 1,
                CitiesSequenceAlongPath = new List<int>()
            };
        }
    }
}
