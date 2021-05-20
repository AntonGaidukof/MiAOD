using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RabinKarp
{
    class Program
    {
        private const int EmptyValue = -1;
        private const ulong EmptyHash = 0;

        private static readonly HashSet<char> ForbiddenChars = new HashSet<char> { '.', ',', '!', '?', ':' };

        static void Main( string[] args )
        {
            string inputFileName = Console.ReadLine();
            string[] inputLines = File.ReadAllLines( $"{inputFileName}" );
            string[] targetLines = new string[ int.Parse( inputLines[ 0 ] ) ];

            Array.Copy( inputLines, 1, targetLines, 0, int.Parse( inputLines[ 0 ] ) );
            
            var inputText = ReadInputData( inputLines.Last() );
            var positionsByTargetStrings = FindText( inputText, targetLines );

            foreach ( var positionsByTargetString in positionsByTargetStrings )
            {
                foreach ( var position in positionsByTargetString.Value )
                {
                    Console.WriteLine( $"{positionsByTargetString.Key}: Line: {position.Item1}, column: {position.Item2}" );
                }
            }
        }

       /* public List<string> GetTargetStrings( string[] inputLines )
        {
            int amountLines = int.Parse( inputLines[ 0 ] );
            var targetStrings = new List<string>();

            for ( int i = 1; i <= amountLines; i++ )
            {
                targetStrings.Add()
            }
        }*/

        public static string[] ReadInputData( string inputFileName )
        {
            string str;
            List<string> result = new List<string>();

            using ( var f = new StreamReader( inputFileName, Encoding.UTF8 ) )
            {
                while ( ( str = f.ReadLine() ) != null )
                {
                    result.Add( ReplaceForbiddenChars( str ) );
                }
            }

            return result.ToArray();
        }

        public static Dictionary<string, List<Tuple<int, int>>> FindText( string[] strArr, string[] targetStrs )
        {
            var targetStrHashsByStr = new Dictionary<string, ulong>();
            var positionsByTargetString = new Dictionary<string, List<Tuple<int, int>>>();

            foreach ( var targetStr in targetStrs )
            {
                targetStrHashsByStr.TryAdd( targetStr, Hash.GetInitializeHashValue( targetStr ) );
                positionsByTargetString.TryAdd( targetStr, new List<Tuple<int, int>>() );
            }
            for ( int i = 0; i < strArr.Length; i++ )
            {
                ScanStr( strArr[ i ], targetStrHashsByStr, i, positionsByTargetString );
            }

            return positionsByTargetString;
        }

        public static void ScanStr( string str, Dictionary<string, ulong> targetStrHashsByStr, int currentLine, Dictionary<string, List<Tuple<int, int>>> positionsByTargetString )
        {
            var scanStrHashsByTargetStr = targetStrHashsByStr.ToDictionary( s => s.Key, s => EmptyHash );

            for ( int i = 0; i < str.Length; i++ )
            {
                foreach ( var item in targetStrHashsByStr )
                {
                    if ( CompareHash( scanStrHashsByTargetStr, item.Value, str, item.Key, i ) )
                    {
                        positionsByTargetString[ item.Key ].Add( new Tuple<int, int>( currentLine + 1, i + 1 ) );
                    }
                }
            }
        }

        private static bool CompareHash( Dictionary<string, ulong> scanStrHashsByTargetStr, ulong targetStrHash, string scanStr, string targetStr, int currentPosition )
        {
            if ( currentPosition + targetStr.Length > scanStr.Length )
            {
                return false;
            }

            string subStr = scanStr.Substring( currentPosition, targetStr.Length );
            char previousChar = currentPosition == 0 ? default : scanStr[ currentPosition ];
            ulong scanStrHash = Hash.GetHashValue( scanStrHashsByTargetStr[ targetStr ], previousChar, subStr[ targetStr.Length - 1 ], subStr );
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
