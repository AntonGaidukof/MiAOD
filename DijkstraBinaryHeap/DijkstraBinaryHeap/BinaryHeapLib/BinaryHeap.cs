using System;
using System.Collections.Generic;
using System.Linq;

namespace DijkstraBinaryHeap.BinaryHeapLib
{
    public class BinaryHeap<T> : IBinaryHeap<T> where T : IComparable<T>
    {
        private readonly List<T> _heap;
        private readonly BinaryHeapKind _kind;

        public int Length => _heap.Count;
        public bool IsEmpty => _heap.Count == 0;
        public T First => _heap.First();

        private int ParentComparingResult => _kind == BinaryHeapKind.Desc ? 1 : -1;
        private int ChildComparingResult => _kind == BinaryHeapKind.Desc ? -1 : 1;

        public BinaryHeap( int capacity, BinaryHeapKind kind = BinaryHeapKind.Asc )
        {
            _kind = kind;
            _heap = new List<T>();
            _heap.Capacity = capacity;
        }

        public BinaryHeap( IEnumerable<T> elements, BinaryHeapKind kind = BinaryHeapKind.Asc )
        {
            _kind = kind;
            _heap = new List<T>();
            foreach ( var element in elements )
            {
                Push( element );
            }
        }

        public void Pop()
        {
            if ( _heap.Count == 0 )
            {
                return;
            }

            if ( _heap.Count == 1 )
            {
                _heap.RemoveAt( 0 );
                return;
            }

            _heap[ 0 ] = _heap.Last();
            _heap.RemoveAt( _heap.Count - 1 );
            Sifting( 0 );
        }

        public void Push( T element )
        {
            _heap.Add( element );
            int index = _heap.Count - 1;

            if ( NeedSiftingUp( index ) )
            {
                SiftingUp( index );
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
            int newStringLimit = 2;
            Console.WriteLine( _heap.First() );
            for ( int i = 1; i < _heap.Count; i++ )
            {
                Console.Write( $"{_heap[ i ]} " );
                if ( newStringLimit == i )
                {
                    Console.WriteLine();
                    newStringLimit = 2 * newStringLimit + 2;
                }
            }
        }

        private void Sifting( int index )
        {
            if ( index == -1 || index >= _heap.Count )
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

        private void SiftingUp( int index )
        {
            T element = _heap[ index ];
            T parent;

            while ( NeedSiftingUp( index ) )
            {
                int parentIndex = GetParentIndex( index );
                parent = _heap[ parentIndex ];
                _heap[ parentIndex ] = element;
                _heap[ index ] = parent;
                index = parentIndex;
            }
        }

        private bool NeedSiftingUp( int index )
        {
            T element = _heap[ index ];
            int parentIndex = GetParentIndex( index );

            return parentIndex >= 0 && element.CompareTo( _heap[ parentIndex ] ) == ParentComparingResult;
        }

        private void SiftingDown( int index )
        {
            T element = _heap[ index ];
            T leftChild;
            T rightChild;

            while ( NeedSiftingDown( index ) )
            {
                int leftChildIndex = GetLeftChildIndex( index );
                if ( leftChildIndex > 0 )
                {
                    leftChild = _heap[ leftChildIndex ];
                    if ( element.CompareTo( leftChild ) == ChildComparingResult )
                    {
                        _heap[ leftChildIndex ] = element;
                        _heap[ index ] = leftChild;
                        index = leftChildIndex;
                        continue;
                    }
                }

                int rightChildIndex = GetRightChildIndex( index );
                if ( rightChildIndex > 0 )
                {
                    rightChild = _heap[ rightChildIndex ];
                    if ( element.CompareTo( rightChild ) == ChildComparingResult )
                    {
                        _heap[ rightChildIndex ] = element;
                        _heap[ index ] = rightChild;
                        index = rightChildIndex;
                    }
                }
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

        private int GetParentIndex( int elementIndex )
        {
            int parentIndex = ( elementIndex - 1 ) / 2;
            return ( _heap.Count - 1 ) >= parentIndex ? parentIndex : -1;
        }

        private int GetLeftChildIndex( int elementIndex )
        {
            int childIndex = 2 * elementIndex + 1;
            return ( _heap.Count - 1 ) >= childIndex ? childIndex : -1;
        }

        private int GetRightChildIndex( int elementIndex )
        {
            int childIndex = 2 * elementIndex + 2;
            return ( _heap.Count - 1 ) >= childIndex ? childIndex : -1;
        }
    }
}
