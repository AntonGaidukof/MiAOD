using System;
using System.Linq;

namespace RabinKarp
{
    public static class Hash
    {
        public static readonly long PrimeMod = 15487469;
        public static readonly long PrimeBase = 13;
        //private readonly long ModConstant = 


        public static long GetHashValue( long previousHashValue, char previousChar, char newChar, string str )
        {
            long hashCode = previousHashValue;
            // предварительно вычисляем множитель
            long multiplier = 1;
            var strLength = str.Count();
            for (var i = 1; i < strLength; ++i) {
                multiplier *= PrimeBase;
                multiplier %= PrimeMod;
            }
            
            // добавляем модуль для получения неотрицательного хэша
            hashCode += PrimeMod;
            
            hashCode -= (multiplier * previousChar) % PrimeMod;
            hashCode *= PrimeBase;
            hashCode += newChar;
            hashCode %= PrimeMod;
            
            return hashCode;
            
            //return GetInitializeHashValue( str );
            /*return previousHashValue != 0 
                ? ( previousHashValue - previousChar * ( ulong )Math.Pow( Constant, str.Length - 1 ) ) * Constant + newChar
                : GetInitializeHashValue( str );*/
        }

        public static long GetInitializeHashValue( string str )
        {
            // long ret = 0;
            // for (var i = 0; i < str.Count(); i++)
            // {
            //     ret = ret * PrimeBase + str[i];
            //     ret %= PrimeMod; //don't overflow
            // }
            //
            // return ret;

            var stringLength = str.Count();
            long hash = 0;
            for (var i = 0; i < stringLength; i++)
            {
                //hash = GetHashValue( 0, i == 0 ? 0 : str[0], str[i], string str );
                hash = ( hash * PrimeBase + str[i] ) % PrimeMod;
            }
            
            return hash;
            
            // ulong result = 0;
            // for ( int i = 0; i < str.Length; i++ )
            // {
            //     result += str[ i ] + ( ulong )Math.Pow( Constant, str.Length - i - 1 );
            // }
            // return result;
        }
    }
}
