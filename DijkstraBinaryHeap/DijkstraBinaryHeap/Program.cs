using System;
using DijkstraBinaryHeap.BinaryHeapLib;
using System.Collections.Generic;

namespace DijkstraBinaryHeap
{
    class Program
    {
        static int Main( string[] args )
        {
            var testList = new List<int> { 5, 11, 14, 7, 3, 8, 1, 9, 18, 6 };
            var binaryHeap = new BinaryHeap<int>( testList, BinaryHeapKind.Asc );

            binaryHeap.Print();
            binaryHeap.Delete();
         //   binaryHeap.Update( 1, 10 );
            Console.WriteLine( "--------------" );
            binaryHeap.Print();

            Console.WriteLine( "--------------" );
            Console.WriteLine("print input file name and output file name: ");
            string[] inputLine = Console.ReadLine()?.Split(' ');
            if ( inputLine == null || inputLine.Length < 2 )
            {
                Console.WriteLine( "you must input file name and output file name!" );

                return 1;
            }

            string inputFileName = inputLine[ 0 ];
            if (string.IsNullOrEmpty(inputFileName))
            {
                Console.WriteLine("Error input file name is empty!");

                return 1;
            }

            string outputFileName = inputLine[ 0 ];
            if (string.IsNullOrEmpty(outputFileName))
            {
                Console.WriteLine("Error output file name is empty!");

                return 1;
            }

            string[] lines = System.IO.File.ReadAllLines($"{inputFileName}.txt");

            
            string[] firstLine = lines[0].Split(' ');
            var inputData = new InputData
            {
                CitiesAmount = int.Parse(firstLine[0]),
                RoadsAmount = int.Parse(firstLine[1]),
                StartCityIndex = int.Parse(firstLine[2]),
                EndCityIndex = int.Parse(firstLine[3]),
                WayData = new List<Tuple<int, int, int>>()
            };

            for (int i = 1; i < lines.Length; ++i)
            {
                string[] line = lines[i].Split(' ');
                var way = new Tuple<int, int, int>(int.Parse(line[0]), int.Parse(line[1]), int.Parse(line[2]));
                inputData.WayData.Add(way);
            }

            var shortestPathFinder = new ShortestPathFinder( inputData );
            OutputData outputData = shortestPathFinder.GetShortestPathData(outputFileName);

            Console.WriteLine( outputData.ShortestPathLength );
            Console.WriteLine( outputData.CitiesSequenceAlongPath );

            return 0;
        }
    }
}
