using System;
using System.Collections.Generic;
using System.Linq;

namespace DijkstraBinaryHeap.BinaryHeapLib
{
    public class BinaryHeap<T> : IBinaryHeap<T> where T : IComparable
    {
        private List<T> _heap;
        private BinaryHeapKind _kind;

        public int Length => _heap.Count;
        public bool IsEmpty => _heap.Count == 0;

        private int ParerntComparingResult => _kind == BinaryHeapKind.Desc ? 1 : -1;
        private int ChildComparingResult => _kind == BinaryHeapKind.Desc ? -1 : 1;

        public BinaryHeap( BinaryHeapKind kind = BinaryHeapKind.Desc )
        {
            _kind = kind;
            _heap = new List<T>();
        }

        public BinaryHeap( IEnumerable<T> elements, BinaryHeapKind kind = BinaryHeapKind.Desc )
        {
            _kind = kind;
            _heap = new List<T>();
            foreach ( var element in elements )
            {
                Add( element );
            }
        }

        public void Delete( T element )
        {
            int elementIndex = _heap.FindIndex( e => e.Equals( element ) );
            if ( elementIndex != -1 )
            {
                _heap[ elementIndex ] = _heap.Last();
                _heap.RemoveAt( _heap.Count - 1 );
                Sifting( elementIndex );
            }
        }

        public void Add( T element )
        {
            _heap.Add( element );
            int index = _heap.Count - 1;

            if ( NeedSiftingUp( index ) )
            {
                SiftingUp( index );
            }
        }

        public void Update( T currentValue, T newValue )
        {
            int currentValueIndex = _heap.FindIndex( e => e.Equals( currentValue ) );

            if ( currentValueIndex != -1 )
            {
                _heap[ currentValueIndex ] = newValue;
                Sifting( currentValueIndex );
            }
        }

        public void Update( int index, T newValue )
        {
            if ( index > 0 && index < _heap.Count )
            {
                _heap[ index ] = newValue;
                Sifting( index );
            }
        }

        public void Print()
        {
            PrintHeap( 0 );
        }

        private void PrintHeap( int index )
        {
            int leftChildIndex = GetLeftChildIndex( index );
            int rightChildIndex = GetRightChildIndex( index );

            if ( leftChildIndex > 0 )
            {
                PrintHeap( leftChildIndex );
                Console.Write( $"{_heap[ leftChildIndex ]} " );
            }
            else if ( rightChildIndex > 0 )
            {
                PrintHeap( rightChildIndex );
                Console.Write( $"{_heap[ rightChildIndex ]} " );
            }
            Console.WriteLine( "" );
        }

        private void Sifting( int index )
        {
            if ( index == 0 || index >= _heap.Count )
            {
                return;
            }

            if ( NeedSiftingUp( index ) )
            {
                SiftingUp( index );
            }
            else if ( NeedSiftingDown( index ) )
            {
                SiftingDown( index );
            }
        }

        private bool NeedSiftingUp( int index )
        {
            var element = _heap[ index ];
            int parentIndex = GetParentInndex( index );

            return parentIndex > 0 && element.CompareTo( _heap[ parentIndex ] ) == ParerntComparingResult;
        }

        private void SiftingUp( int index )
        {
            int parentIndex = GetParentInndex( index );
            var parent = _heap[ parentIndex ];
            var element = _heap[ index ];

            while ( element.CompareTo( parent ) == ParerntComparingResult )
            {
                _heap[ parentIndex ] = element;
                _heap[ index ] = parent;
                index = parentIndex;
                parentIndex = GetParentInndex( index );
                element = _heap[ index ];
                parent = _heap[ parentIndex ];
            }
        }

        private bool NeedSiftingDown( int index )
        {
            var element = _heap[ index ];
            int leftChildIndex = GetLeftChildIndex( index );
            int rightChildIndex = GetRightChildIndex( index );

            return ( leftChildIndex > 0 && element.CompareTo( _heap[ leftChildIndex ] ) == ChildComparingResult )
                || ( rightChildIndex > 0 && element.CompareTo( _heap[ rightChildIndex ] ) == ChildComparingResult );
        }

        private void SiftingDown( int index )
        {
            int leftChildIndex = GetLeftChildIndex( index );
            int rightChildIndex = GetRightChildIndex( index );
            var element = _heap[ index ];
            var leftChild = _heap[ leftChildIndex ];
            var rightChild = _heap[ rightChildIndex ];

            while ( element.CompareTo( leftChild ) == ChildComparingResult || element.CompareTo( rightChild ) == ChildComparingResult )
            {
                if ( element.CompareTo( leftChild ) == ChildComparingResult )
                {
                    _heap[ leftChildIndex ] = element;
                    _heap[ index ] = leftChild;
                    index = leftChildIndex;
                }
                else
                {
                    _heap[ rightChildIndex ] = element;
                    _heap[ index ] = leftChild;
                    index = rightChildIndex;
                }

                leftChildIndex = GetLeftChildIndex( index );
                rightChildIndex = GetRightChildIndex( index );
            }
        }

        private int GetParentInndex( int elementIndex )
        {
            return ( elementIndex - 1 ) / 2;
        }

        private int GetLeftChildIndex( int elementIndex )
        {
            return 2 * elementIndex + 1;
        }

        private int GetRightChildIndex( int elementIndex )
        {
            return 2 * elementIndex + 2;
        }
    }
}
