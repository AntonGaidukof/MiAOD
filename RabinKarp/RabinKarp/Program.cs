using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RabinKarp
{
    class Program
    {
        private const ulong EmptyHash = 0;
        private const string OutputFileName = "out.txt";

        private static readonly HashSet<char> ForbiddenChars = new HashSet<char> { '.', ',', '!', '?', ':' };

        static void Main( string[] args )
        {
            string inputFileName = Console.ReadLine();
            string[] inputLines = File.ReadAllLines( $"{inputFileName}" );
            string[] targetLines = new string[ int.Parse( inputLines[ 0 ] ) ];

            Array.Copy( inputLines, 1, targetLines, 0, int.Parse( inputLines[ 0 ] ) );
            targetLines = targetLines.Select( tl => ReplaceForbiddenChars( tl ) ).ToArray();
            
            var startTime = System.Diagnostics.Stopwatch.StartNew();
            var inputData = ReadInputData( inputLines.Last() );

            var tempResult = startTime.Elapsed;
            string elapsedTime = String.Format( "{0:00}:{1:00}:{2:00}.{3:000}",
                tempResult.Hours,
                tempResult.Minutes,
                tempResult.Seconds,
                tempResult.Milliseconds );
            Console.WriteLine( $"ReadInputData time: {elapsedTime}" );

            var targetStringPositions = FindText( inputData.Item2, inputData.Item1.ToArray(), targetLines );

            tempResult = startTime.Elapsed;
            elapsedTime = String.Format( "{0:00}:{1:00}:{2:00}.{3:000}",
                tempResult.Hours,
                tempResult.Minutes,
                tempResult.Seconds,
                tempResult.Milliseconds );
            Console.WriteLine( $"FindText time: {elapsedTime}" );

            using ( StreamWriter sw = new StreamWriter( OutputFileName, false, Encoding.UTF8 ) )
            {
                foreach ( var targetStringPosition in targetStringPositions )
                {
                    sw.WriteLine( $"Line {targetStringPosition.Item2}, position {targetStringPosition.Item3}: {targetStringPosition.Item1}" );
                }
            }

            startTime.Stop();
            tempResult = startTime.Elapsed;
            elapsedTime = String.Format( "{0:00}:{1:00}:{2:00}.{3:000}",
                tempResult.Hours,
                tempResult.Minutes,
                tempResult.Seconds,
                tempResult.Milliseconds );
            Console.WriteLine( $"Write result time: {elapsedTime}" );
        }

        public static Tuple<List<int>, string> ReadInputData( string inputFileName )
        {
            string str;
            var text = new StringBuilder();
            var strLengths = new List<int>();

            using ( var f = new StreamReader( inputFileName, Encoding.UTF8 ) )
            {
                while ( ( str = f.ReadLine() ) != null )
                {
                    str = $"{ReplaceForbiddenChars( str )} ";
                    text.Append( str );
                    strLengths.Add( str.Length );
                }
            }

            return new Tuple<List<int>, string>( strLengths, text.ToString() );
        }

        public static List<Tuple<string, int, int>> FindText( string text, int[] strLengths, string[] targetStrs )
        {
            var targetStrHashsByStr = new Dictionary<string, ulong>();

            foreach ( var targetStr in targetStrs )
            {
                targetStrHashsByStr.TryAdd( targetStr, Hash.GetInitializeHashValue( targetStr ) );
            }
            return ScanStr( text, strLengths, targetStrHashsByStr );
        }

        public static List<Tuple<string, int, int>> ScanStr( string text, int[] strLengths, Dictionary<string, ulong> targetStrHashsByStr )
        {
            var scanStrHashsByTargetStr = targetStrHashsByStr.ToDictionary( s => s.Key, s => EmptyHash );
            var targetStringPositions = new List<Tuple<string, int, int>>();
            int currentLine = 0;
            int currentColumn = 0;

            for ( int i = 0; i < text.Length; i++ )
            {
                foreach ( var item in targetStrHashsByStr )
                {
                    if ( CompareHash( scanStrHashsByTargetStr, item.Value, text, item.Key, i ) )
                    {
                        targetStringPositions.Add( new Tuple<string, int, int>( item.Key, currentLine + 1, currentColumn + 1 )  );
                    }
                }

                if (currentColumn == strLengths[currentLine] - 1)
                {
                    currentLine++;
                    currentColumn = 0;
                }
                else
                {
                    currentColumn++;
                }
            }

            return targetStringPositions;
        }

        private static bool CompareHash( Dictionary<string, ulong> scanStrHashsByTargetStr, ulong targetStrHash, string scanStr, string targetStr, int currentPosition )
        {
            if ( currentPosition + targetStr.Length > scanStr.Length )
            {
                return false;
            }

            string subStr = scanStr.Substring( currentPosition, targetStr.Length );
            char previousChar = currentPosition == 0 ? default : scanStr[ currentPosition - 1];
            ulong scanStrHash = Hash.GetHashValue( scanStrHashsByTargetStr[ targetStr ], previousChar, subStr.Last(), subStr );
            scanStrHashsByTargetStr[ targetStr ] = scanStrHash;

            if ( scanStrHash == targetStrHash )
            {
                if ( subStr == targetStr )
                {
                    return true;
                }
            }

            return false;
        }

        private static string ReplaceForbiddenChars( string str )
        {
            var result = new StringBuilder();

            for ( int i = 0; i < str.Length; i++ )
            {
                if ( ForbiddenChars.Contains( str[ i ] ) )
                {
                    result.Append( str[ i ] );
                }
                else
                {
                    result.Append( Char.ToLower( str[ i ] ) );
                }
            }

            return result.ToString();
        }
    }
}
