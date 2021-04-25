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
            Console.WriteLine( "print input file name and output file name: " );
            string[] inputLine = Console.ReadLine()?.Split( ' ' );
            if ( inputLine == null || inputLine.Length < 2 )
            {
                Console.WriteLine( "you must input file name and output file name!" );

                return 1;
            }

            string inputFileName = inputLine[ 0 ];
            if ( string.IsNullOrEmpty( inputFileName ) )
            {
                Console.WriteLine( "Error input file name is empty!" );

                return 1;
            }

            string outputFileName = inputLine[ 0 ];
            if ( string.IsNullOrEmpty( outputFileName ) )
            {
                Console.WriteLine( "Error output file name is empty!" );

                return 1;
            }

            var inputData = GetInputData( inputFileName );
            var shortWay = GetShortWay( inputData );

            Console.WriteLine( $"Distance: {shortWay?.ShortestPathLength.ToString() ?? "false"}" );
            Console.WriteLine( shortWay.DestinationPath );

            return 0;
        }

        private static InputData GetInputData( string inputFileName )
        {
            string[] lines = File.ReadAllLines( $"{inputFileName}.txt" );
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
                DistancesByNodeName = nodesByName.ToDictionary( n => n.Key, n => int.MaxValue )
            };
            inputData.DistancesByNodeName[ inputData.StartNode.Name ] = 0;
            inputData.StartNode.Distance = 0;

            return inputData;
        }

        private static ShortWay GetShortWay( InputData inputData )
        {
            var distancesByNodeName = inputData.DistancesByNodeName;
            var heap = new BinaryHeap<Node>();

            heap.Push( inputData.StartNode );

            while ( !heap.IsEmpty )
            {
                var currentNode = heap.First;
                heap.Pop();

                if ( distancesByNodeName[ currentNode.Name ] < currentNode.Distance )
                {
                    continue;
                }

                foreach ( var relatedNode in currentNode.RelatedNodes )
                {
                    if ( relatedNode.Item2.Distance > relatedNode.Item1 + currentNode.Distance )
                    {
                        distancesByNodeName[ relatedNode.Item2.Name ] = relatedNode.Item1 + currentNode.Distance;
                        relatedNode.Item2.Distance = relatedNode.Item1 + currentNode.Distance;
                        relatedNode.Item2.LastNode = currentNode;
                        heap.Push( relatedNode.Item2 );
                    }
                }
            }


            return distancesByNodeName[ inputData.DistanceNode.Name ] == int.MaxValue
                ? null
                : new ShortWay
                {
                    ShortestPathLength = distancesByNodeName[ inputData.DistanceNode.Name ],
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
