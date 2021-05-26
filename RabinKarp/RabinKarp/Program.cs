using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RabinKarp
{
    class Program
    {
        private const long EmptyHash = 0;
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

            Dictionary<Position, List<string>> targetStringPositions = FindText( inputData.Item2, inputData.Item1.ToArray(), targetLines );

            tempResult = startTime.Elapsed;
            elapsedTime = String.Format( "{0:00}:{1:00}:{2:00}.{3:000}",
                tempResult.Hours,
                tempResult.Minutes,
                tempResult.Seconds,
                tempResult.Milliseconds );
            Console.WriteLine( $"FindText time: {elapsedTime}" );
            
            var s = new SortedDictionary<Position, List<string>>(targetStringPositions);
            using ( StreamWriter sw = new StreamWriter( OutputFileName, false, Encoding.UTF8 ) )
            {
                foreach ( KeyValuePair<Position, List<string>> targetStringPosition in s )
                {
                    foreach ( string target in targetStringPosition.Value )
                    {
                        sw.WriteLine( $"Line {targetStringPosition.Key.Line}, position {targetStringPosition.Key.Column}: {target}" );    
                    }
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

        public static Dictionary<Position, List<string>> FindText( string text, int[] strLengths, string[] targetStrs )
        {
            var targetStrHashsByStr = new Dictionary<string, HashData>();

            foreach ( var targetStr in targetStrs )
            {
                targetStrHashsByStr.TryAdd( targetStr, new HashData( Hash.GetInitializeHashValue( targetStr ), CalculatePower( targetStr ) ) );
            }
            return ScanStr( text, strLengths, targetStrHashsByStr );
        }

        private static long CalculatePower( string str )
        {
            long power = 1;
            for ( int i = 0; i < str.Count(); i++ )
            {
                power = (power * Hash.PrimeBase) % Hash.PrimeMod;
            }

            return power;
        }

        public static Dictionary<Position, List<string>> ScanStr( string text, int[] strLengths, Dictionary<string, HashData> targetStrHashsByStr )
        {
            Dictionary<string, long> scanStrHashsByTargetStr = targetStrHashsByStr
                .ToDictionary( s => s.Key, s => EmptyHash );
            var targetStringPositions = new Dictionary<Position, List<string>>();//<Tuple<string, int, int>>();
            int currentLine = 0;
            int currentColumn = 0;

            for ( int i = 0; i < text.Length; i++ )
            {
                foreach ( var item in targetStrHashsByStr )
                {
                    if ( CompareHash( scanStrHashsByTargetStr, item.Value, text, item.Key, i ) )
                    {
                        var itemPosition = GenerateValue(item.Key.Length, currentLine, currentColumn, strLengths );

                        if ( !targetStringPositions.ContainsKey( itemPosition ) )
                        {
                            targetStringPositions.Add( itemPosition, new List<string>() );
                        }
                        
                        targetStringPositions[itemPosition].Add( item.Key );
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

        private static Position GenerateValue( int targetStrLen, int currentLine, int currentColumn, int[] strLengths )
        {
            var x = currentColumn - targetStrLen + 2;
            while ( x < 0 )
            {
                currentLine--;
                var y = strLengths[currentLine];
                x = y + x;
            }

            return new Position { Column = x, Line = currentLine + 1 };
        }

        private static bool CompareHash( Dictionary<string, long> scanStrHashsByTargetStr, HashData targetStrHashData, string scanStr, string targetStr, int currentPosition )
        {
            // add the last symbol
            var strHash = scanStrHashsByTargetStr[ targetStr ];
            strHash = strHash * Hash.PrimeBase + scanStr[currentPosition];
            strHash %= Hash.PrimeMod;

            // remove the first symbol
            if ( currentPosition >= targetStr.Count() )
            {
                strHash -= targetStrHashData.Power * scanStr[currentPosition - targetStr.Count()] % Hash.PrimeMod;
                if ( strHash < 0 ) // negative to positive
                {
                    strHash += Hash.PrimeMod;
                }
            }

            scanStrHashsByTargetStr[ targetStr ] = strHash;

            return currentPosition >= targetStr.Count() - 1 && targetStrHashData.Hash == strHash;
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
