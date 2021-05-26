using System;
using System.Collections.Generic;

namespace RabinKarp
{
	public class Position : IEquatable<Position>,IComparable<Position>
	{
		public int Line { get; set; }
		public int Column { get; set; }
		
		public class EqualityComparer : IEqualityComparer<Position> 
		{
			public bool Equals( Position positionThis, Position positionThere )
			{
				if ( ReferenceEquals( positionThis, positionThere ) ) return true;
				if ( ReferenceEquals( positionThis, null ) ) return false;
				if ( ReferenceEquals( positionThere, null ) ) return false;
				if ( positionThis.GetType() != positionThere.GetType() ) return false;
				return positionThis.Line == positionThere.Line && positionThis.Column == positionThere.Column;
			}

			public int GetHashCode( Position obj )
			{
				return HashCode.Combine( obj.Line, obj.Column );
			}
		}

		public bool Equals( Position other )
		{
			if ( ReferenceEquals( null, other ) ) return false;
			if ( ReferenceEquals( this, other ) ) return true;
			return Line == other.Line && Column == other.Column;
		}

		public override bool Equals( object obj )
		{
			if ( ReferenceEquals( null, obj ) ) return false;
			if ( ReferenceEquals( this, obj ) ) return true;
			if ( obj.GetType() != this.GetType() ) return false;
			return Equals( ( Position ) obj );
		}

		public override int GetHashCode()
		{
			return HashCode.Combine( Line, Column );
		}

		public int CompareTo( Position other )
		{
			if ( ReferenceEquals( this, other ) ) return 0;
			if ( ReferenceEquals( null, other ) ) return 1;
			int lineComparison = Line.CompareTo( other.Line );
			if ( lineComparison != 0 ) return lineComparison;
			return Column.CompareTo( other.Column );
		}
	}
}