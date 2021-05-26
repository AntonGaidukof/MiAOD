using System;

namespace RabinKarp
{
    public static class Hash
    {
        private static readonly ulong Constant = 12;
        //private readonly long ModConstant = 

        public static ulong GetHashValue( ulong previousHashValue, char previousChar, char newChar, string str )
        {
            return ( ulong )str.GetHashCode();
            /*return previousHashValue != 0
                ? previousHashValue - GetCharHashValue( previousChar, 0 ) + GetCharHashValue( newChar, str.Length - 1 )
                : GetInitializeHashValue( str );
            //return GetInitializeHashValue( str );
            /*return previousHashValue != 0 
                ? ( previousHashValue - previousChar * ( ulong )Math.Pow( Constant, str.Length - 1 ) ) * Constant + newChar
                : GetInitializeHashValue( str );*/
        }

        public static ulong GetInitializeHashValue( string str )
        {
            return ( ulong )str.GetHashCode();
            /*
            ulong result = 0;
            for ( int i = 0; i < str.Length; i++ )
            {
                result += GetCharHashValue( str[ i ], i );
               // result += str[ i ] + ( ulong )Math.Pow( Constant, str.Length - i - 1 );
            }
            return result;*/
        }

        private static ulong GetCharHashValue( char item, int position )
        {
            //return ( ulong )( item.GetHashCode() ) * ( ulong )Math.Pow( Constant, position );
            return ( ulong )Math.Pow( Constant, item.GetHashCode() );
        }
    }
}
