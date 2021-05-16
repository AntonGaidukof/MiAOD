using System;
using DijkstraBinaryHeap.BinaryHeapLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DijkstraBinaryHeap
{
    class Program
    {
        static int Main( string[] args )
        {
            Console.WriteLine( "Введите название входного и выходного файла через пробел" );
            string[] inputLine = Console.ReadLine()?.Split( ' ' );
            
            if ( inputLine == null || inputLine.Length < 2 )
            {
                Console.WriteLine( "Нужно ввести название обоих файлов" );

                return 1;
            }

            string inputFileName = inputLine[ 0 ];
            string outputFileName = inputLine[ 1 ];

            var startTime = System.Diagnostics.Stopwatch.StartNew();
            var inputData = GetInputData( inputFileName );
            
            var tempResult = startTime.Elapsed;
            string elapsedTime = String.Format( "{0:00}:{1:00}:{2:00}.{3:000}",
                tempResult.Hours,
                tempResult.Minutes,
                tempResult.Seconds,
                tempResult.Milliseconds );
            Console.WriteLine( elapsedTime );
           
            var shortWay = GetShortWay( inputData );
            
            startTime.Stop();
            tempResult = startTime.Elapsed;
            elapsedTime = String.Format( "{0:00}:{1:00}:{2:00}.{3:000}",
                tempResult.Hours,
                tempResult.Minutes,
                tempResult.Seconds,
                tempResult.Milliseconds );
            Console.WriteLine( elapsedTime );

            using ( StreamWriter sw = new StreamWriter( outputFileName, false, Encoding.Default ) )
            {
                if ( shortWay != null)
                {
                    sw.WriteLine( shortWay?.ShortestPathLength.ToString() );
                    sw.WriteLine( shortWay.DestinationPath );
                }
                else
                {
                    sw.WriteLine( "false" );
                }
            }

            return 0;
        }

        private static InputData GetInputData( string inputFileName )
        {
            string[] lines = File.ReadAllLines( $"{inputFileName}" );
            var nodesByName = new Dictionary<int, Node>();

            for ( int i = 1; i < lines.Length; ++i )
            {
                string[] line = lines[ i ].Split( ' ' );
                int nodeName = int.Parse( line[ 0 ] );
                int relatedNodeName = int.Parse( line[ 1 ] );
                int distance = int.Parse( line[ 2 ] );
                if ( !nodesByName.ContainsKey( relatedNodeName ) )
                {
                    nodesByName.Add( relatedNodeName, new Node( relatedNodeName ) );
                }
                if ( nodesByName.ContainsKey( nodeName ) )
                {

                    nodesByName[ nodeName ].RelatedNodes.Add( new Tuple<int, Node>( distance, nodesByName[ relatedNodeName ] ) );
                    nodesByName[ relatedNodeName ].RelatedNodes.Add( new Tuple<int, Node>( distance, nodesByName[ nodeName ] ) );
                }
                else
                {
                    nodesByName.Add( nodeName, new Node( nodeName ) );
                    nodesByName[ nodeName ].RelatedNodes.Add( new Tuple<int, Node>( distance, nodesByName[ relatedNodeName ] ) );
                    nodesByName[ relatedNodeName ].RelatedNodes.Add( new Tuple<int, Node>( distance, nodesByName[ nodeName ] ) );
                }
            }

            string[] firstLine = lines[ 0 ].Split( ' ' );
            var inputData = new InputData
            {
                Nodes = nodesByName.Values.ToArray(),
                StartNode = nodesByName[ int.Parse( firstLine[ 2 ] ) ],
                DistanceNode = nodesByName[ int.Parse( firstLine[ 3 ] ) ],
            };
            inputData.StartNode.Distance = 0;

            return inputData;
        }

        private static ShortWay GetShortWay( InputData inputData )
        {
            var heap = new BinaryHeap<Node>( inputData.Nodes.Count() );

            heap.Push( inputData.StartNode );

            while ( !heap.IsEmpty )
            {
                var currentNode = heap.First;
                heap.Pop();

                foreach ( var relatedNode in currentNode.RelatedNodes )
                {
                    if ( relatedNode.Item2.Distance > relatedNode.Item1 + currentNode.Distance )
                    {
                        relatedNode.Item2.Distance = relatedNode.Item1 + currentNode.Distance;
                        relatedNode.Item2.LastNode = currentNode;
                        heap.Push( relatedNode.Item2 );
                    }
                }
            }

            return inputData.DistanceNode.Distance == int.MaxValue
                ? null
                : new ShortWay
                {
                    ShortestPathLength = inputData.DistanceNode.Distance,
                    DestinationPath = GetMinDestinationPath( inputData.DistanceNode )
                };
        }

        private static string GetMinDestinationPath( Node destinationNode )
        {
            var currentNode = destinationNode;
            var destinationPathNodes = new List<Node>();
            while ( currentNode != null )
            {
                destinationPathNodes.Add( currentNode );
                currentNode = currentNode.LastNode;
            }
            var result = new StringBuilder();
            for ( int i = destinationPathNodes.Count - 1; i >= 0; i-- )
            {
                result.Append( $"{destinationPathNodes[ i ].Name} " );
            }

            return result.ToString();
        }
    }
}
