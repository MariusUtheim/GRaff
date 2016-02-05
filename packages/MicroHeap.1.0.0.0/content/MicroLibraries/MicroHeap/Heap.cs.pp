using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace $rootnamespace$.MicroHeap
{
    internal sealed class HeapCollectionDebugView<T>
    {
        private readonly Heap<T> _Heap;

        public HeapCollectionDebugView(Heap<T> heap)
        {
            if (ReferenceEquals(null, heap))
                throw new ArgumentNullException("heap");

            _Heap = heap;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                var items = new T[_Heap.Count];
                _Heap.CopyTo(items, 0);
                return items;
            }
        }
    }

    /// <summary>
    /// This class implements a heap data structure.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements to store in the heap.
    /// </typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(HeapCollectionDebugView<>))]
    public sealed class Heap<T> : ICollection<T>, ICloneable
    {
        private readonly IComparer<T> _Comparer;
        private readonly bool _Reverse;
        private T[] _Items;
        private int _Count;
        private int _Version;

        /// <summary>
        /// This is the version of the <see cref="Heap{T}"/> class.
        /// </summary>
        public static readonly Version Version = new Version(1, 0, 0, 0);

        /// <summary>
        /// This is the default size of a new <see cref="Heap{T}"/>, unless specified
        /// with one of the constructor overloads, or given an initial collection
        /// to populate the <see cref="Heap{T}"/> with.
        /// </summary>
        public const int DefaultSize = 10;

        /// <overloads>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class.
        /// </overloads>
        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class using the specified capacity, comparer and reverse
        /// settings.
        /// </summary>
        /// <param name="capacity">
        /// The initial capacity to construct the heap with. 10 will be used if the value is less than 10.
        /// </param>
        /// <param name="comparer">
        /// The comparer object to use.
        /// </param>
        /// <param name="reverse">
        /// Wether to reverse the heap results. Set to <b>false</b> if the smallest value is to
        /// be returned by <see cref="Pop"/>, <b>true</b> if the highest value.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="capacity"/> is 0 or less than 0.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// </exception>
        public Heap(int capacity, IComparer<T> comparer, bool reverse)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException("capacity", capacity, "Invalid capacity for heap, must be at least 0");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            _Items = new T[Math.Max(capacity, 10)];
            _Comparer = comparer;
            _Reverse = reverse;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class using the specified capacity.
        /// </summary>
        /// <param name="capacity">
        /// The initial capacity to construct the heap with. 10 will be used if the value is less than 10.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="capacity"/> is 0 or less than 0.</para>
        /// </exception>
        public Heap(int capacity)
            : this(capacity, Comparer<T>.Default, false)
        {
            // Do nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class using the default capacity.
        /// </summary>
        public Heap()
            : this(DefaultSize, Comparer<T>.Default, false)
        {
            // Do nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class from the items in the collection.
        /// settings.
        /// </summary>
        /// <param name="collection">
        /// A collection holding the items to add to the heap.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        public Heap(IEnumerable<T> collection)
            : this(Comparer<T>.Default, false, (collection ?? new T[0]).ToArray())
        {
            // Do nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class from the items in the collection.
        /// settings.
        /// </summary>
        /// <param name="collection">
        /// A collection holding the items to add to the heap.
        /// </param>
        /// <param name="reverse">
        /// Wether to reverse the heap results. Set to <b>false</b> if the smallest value is to
        /// be returned by <see cref="Pop"/>, <b>true</b> if the highest value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        public Heap(IEnumerable<T> collection, bool reverse)
            : this(Comparer<T>.Default, reverse, (collection ?? new T[0]).ToArray())
        {
            // Do nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class from the specified items.
        /// settings.
        /// </summary>
        /// <param name="values">
        /// An array holding the items to add to the heap.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="values"/> is <c>null</c>.</para>
        /// </exception>
        public Heap(params T[] values)
            : this(Comparer<T>.Default, false, values)
        {
            // Do nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class with
        /// the specified comparer.
        /// </summary>
        /// <param name="comparer">
        /// The comparer object to use.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// </exception>
        public Heap(IComparer<T> comparer)
            : this(comparer, false)
        {
            // Do nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class from the items in the collection.
        /// </summary>
        /// <param name="comparer">
        /// The comparer object to use.
        /// </param>
        /// <param name="collection">
        /// A collection holding the items to add to the heap.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        public Heap(IComparer<T> comparer, IEnumerable<T> collection)
            : this(comparer, false, (collection ?? new T[0]).ToArray())
        {
            // Do nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class from the items in the collection.
        /// settings.
        /// </summary>
        /// <param name="comparer">
        /// The comparer object to use.
        /// </param>
        /// <param name="reverse">
        /// Wether to reverse the heap results. Set to <b>false</b> if the smallest value is to
        /// be returned by <see cref="Pop"/>, <b>true</b> if the highest value.
        /// </param>
        /// <param name="collection">
        /// A collection holding the items to add to the heap.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        public Heap(IComparer<T> comparer, bool reverse, IEnumerable<T> collection)
            : this(comparer, reverse, (collection ?? new T[0]).ToArray())
        {
            // Do nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class from the specified items.
        /// settings.
        /// </summary>
        /// <param name="comparer">
        /// The comparer object to use.
        /// </param>
        /// <param name="reverse">
        /// Wether to reverse the heap results. Set to <b>false</b> if the smallest value is to
        /// be returned by <see cref="Pop"/>, <b>true</b> if the highest value.
        /// </param>
        /// <param name="values">
        /// An array holding the items to add to the heap.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="values"/> is <c>null</c>.</para>
        /// </exception>
        public Heap(IComparer<T> comparer, bool reverse, params T[] values)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            if (values == null)
                throw new ArgumentNullException("values");

            _Items = new T[Math.Max(DefaultSize, values.Length)];
            values.CopyTo(_Items, 0);
            _Count = values.Length;
            _Comparer = comparer;
            _Reverse = reverse;

            for (int index = _Count / 2; index >= 0; index--)
                SiftUp(index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            int version = _Version;
            for (int index = 0; index < _Count; index++)
            {
                if (version != _Version)
                    throw new InvalidOperationException("Heap has changed, cannot continue iterating over it");

                yield return _Items[index];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds the elements of the specified collection to this <see cref="Heap{T}"/>.
        /// </summary>
        /// <param name="collection">
        /// The collection whose elements should be added to this <see cref="Heap{T}"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            foreach (var element in collection)
                Add(element);
        }

        /// <summary>
        /// Adds an item to this <see cref="Heap{T}"/>
        /// </summary>
        /// <param name="item">
        /// The item to add to this <see cref="Heap{T}"/>.
        /// </param>
        public void Add(T item)
        {
            if (_Count == _Items.Length)
                Capacity *= 2;

            _Items[_Count++] = item;
            SiftDown(0, _Count - 1);
            BreakEnumerators();
        }

        /// <summary>
        /// Sifts the value from the position down in the heap to its right spot.
        /// </summary>
        /// <param name="startPos">
        /// The starting position in the heap to move towards.
        /// </param>
        /// <param name="pos">
        /// The starting position in the heap to move from.
        /// </param>
        private void SiftDown(int startPos, int pos)
        {
            T newItem = _Items[pos];
            while (pos > startPos)
            {
                int parentPos = (pos - 1) / 2;
                T parent = _Items[parentPos];
                if (CompareElements(parent, newItem) <= 0)
                    break;
                _Items[pos] = parent;
                pos = parentPos;
            }

            _Items[pos] = newItem;
            BreakEnumerators();
        }

        /// <summary>
        /// Sifts the value from the position up in the heap to its right spot.
        /// </summary>
        /// <param name="pos">
        /// The starting position in the heap to move from.
        /// </param>
        private void SiftUp(int pos)
        {
            int endPos = _Count;
            int startPos = pos;
            T newItem = _Items[pos];

            // Bubble up the smaller child until hitting a leaf.
            int childPos = (2 * pos) + 1;

            while (childPos < endPos)
            {
                // Set childpos to index of smaller child.
                int rightPos = childPos + 1;
                if (rightPos < endPos && CompareElements(_Items[rightPos], _Items[childPos]) <= 0)
                    childPos = rightPos;

                // Move the smaller child up.
                _Items[pos] = _Items[childPos];
                pos = childPos;
                childPos = (2 * pos) + 1;
            }

            _Items[pos] = newItem;
            SiftDown(startPos, pos);
            BreakEnumerators();
        }

        /// <summary>
        /// This method increases the internal version number, to break enumerators
        /// that are currently enumerating over this <see cref="Heap{T}"/>.
        /// </summary>
        private void BreakEnumerators()
        {
            unchecked
            {
                _Version++;
            }
        }

        /// <summary>
        /// This method will set <see cref="Capacity"/> to the same value as
        /// <see cref="Count"/>, if <b>Count</b> is less than 90% of <b>Capacity</b>.
        /// </summary>
        public void TrimExcess()
        {
            if (_Count < _Items.Length * 0.9)
                Capacity = _Count;
        }

        /// <summary>
        /// Adds a single value to the heap, placing it in the heap so that the heap criteria
        /// (see <see cref="Heap{T}"/>) is satisfied.
        /// </summary>
        /// <param name="value">
        /// The value to add to the heap.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="value"/> is <c>null</c>.</para>
        /// </exception>
        public void Push(T value)
        {
            Add(value);
        }

        /// <summary>
        /// Pops off the smallest value (or largest value if this heap was constructed with
        /// the reverse flag to <b>true</b>) from the heap.
        /// </summary>
        /// <returns>
        /// The smallest value (or largest value if this heap was constructed with
        /// the reverse flag to <b>true</b>) from the heap.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// <para>The heap is empty.</para>
        /// </exception>
        public T Pop()
        {
            if (_Count == 0)
                throw new InvalidOperationException("Cannot pop from the heap, it is empty");

            T lastElement = _Items[--_Count];
            _Items[_Count] = default(T);
            T returnItem;
            if (_Count > 0)
            {
                returnItem = _Items[0];
                _Items[0] = lastElement;
                SiftUp(0);
            }
            else
                returnItem = lastElement;

            BreakEnumerators();

            return returnItem;
        }

        /// <summary>
        /// Replaces the element at the specified index with the new value, preserving
        /// heap criteria, returning the element that was previously stored in that location in the array.
        /// </summary>
        /// <param name="index">
        /// The index of the element to replace.
        /// </param>
        /// <param name="newValue">
        /// The value to replace the item at the specified <paramref name="index"/> with.
        /// </param>
        /// <returns>
        /// The value of the item at the specified <paramref name="index"/>, before it was replaced
        /// with <paramref name="newValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is less than 0 or greater than or equal to <see cref="Count"/>.</para>
        /// </exception>
        public T ReplaceAt(int index, T newValue)
        {
            if (index < 0 || index >= _Count)
                throw new ArgumentOutOfRangeException("index");

            T returnElement = _Items[index];
            if (index == 0)
            {
                _Items[0] = newValue;
                SiftUp(0);

                BreakEnumerators();
            }
            else
            {
                RemoveAt(index);
                Add(newValue);
            }

            return returnElement;
        }

        /// <summary>
        /// Removes the element at the specified index from the heap, preserving the heap criteria.
        /// </summary>
        /// <param name="index">
        /// The index of the value to remove from the heap.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is less than 0 or greater than or equal to <see cref="Count"/>.</para>
        /// </exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _Count)
                throw new ArgumentOutOfRangeException("index");

            // Copy last element over element that is to be removed
            if (index < _Count - 1)
                _Items[index] = _Items[_Count - 1];

            // Get rid of last, duplicate, element
            _Count--;
            _Items[_Count] = default(T);

            // Preserve heap criteria
            if (index < _Count)
                SiftUp(index);

            BreakEnumerators();
        }

        /// <summary>
        /// Replaces the specified element with a new value, returning whether the process
        /// succeeded or not.
        /// </summary>
        /// <param name="value">
        /// The value to replace in the heap.
        /// </param>
        /// <param name="newValue">
        /// The new value to replace it with.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="value"/> was found and replaced with
        /// <paramref name="newValue"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool Replace(T value, T newValue)
        {
            int index = IndexOf(value);
            if (index < 0)
                return false;

            ReplaceAt(index, newValue);
            return true;
        }

        /// <summary>
        /// Compares two elements and returns their order.
        /// </summary>
        /// <param name="element1">
        /// The first element to compare.
        /// </param>
        /// <param name="element2">
        /// The second element to compare.
        /// </param>
        /// <returns>
        /// <para>&lt; 0 if <paramref name="element1"/> is less than <paramref name="element2"/>;</para>
        /// <para>== 0 if <paramref name="element1"/> is equal to <paramref name="element2"/>;</para>
        /// <para>&gt; 0 if <paramref name="element1"/> is greater than <paramref name="element2"/>;</para>
        /// </returns>
        private int CompareElements(T element1, T element2)
        {
            int result = _Comparer.Compare(element1, element2);
            if (_Reverse)
                result = -result;
            return result;
        }

        /// <summary>
        /// Removes all items from this <see cref="Heap{T}"/>.
        /// </summary>
        public void Clear()
        {
            for (int index = 0; index < _Count; index++)
                _Items[index] = default(T);
            _Count = 0;
            BreakEnumerators();
        }

        /// <summary>
        /// Determines whether this <see cref="Heap{T}"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <paramref name="item"/> is found in this <see cref="Heap{T}"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <param name="item">
        /// The item to locate in this <see cref="Heap{T}"/>.
        /// </param>
        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Locates the element in the heap and returns its index.
        /// </summary>
        /// <param name="value">
        /// The value to locate in the heap.
        /// </param>
        /// <returns>
        /// The index of the element if it was located;
        /// otherwise, -1.
        /// </returns>
        public int IndexOf(T value)
        {
            for (int index = 0; index < _Count; index++)
                if (CompareElements(value, _Items[index]) == 0)
                    return index;

            return -1;
        }

        /// <summary>
        /// Returns the parent index for a given index into the heap, or -1 if the specified index
        /// is the index of the topmost/root element (always index 0).
        /// </summary>
        /// <param name="index">
        /// The index of the element to get the parent index of.
        /// </param>
        /// <returns>
        /// The index of the parent element of the element at the specified <paramref name="index"/>;
        /// or -1 if there is no parent.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is less than 0 or greater than or equal to <see cref="Count"/>.</para>
        /// </exception>
        public int ParentIndex(int index)
        {
            if (index < 0 || index >= _Count)
                throw new ArgumentOutOfRangeException("index");

            if (_Count == 0)
                return -1;
            if (index == 0)
                return -1;
            return (index - 1) / 2;
        }

        /// <summary>
        /// Returns the index of the left child of the element at the specified index in the heap,
        /// or -1 if there is no left child.
        /// </summary>
        /// <param name="index">
        /// The index of the element to get the left child index of.
        /// </param>
        /// <returns>
        /// The index of the left child element of the element at the specified <paramref name="index"/>;
        /// or -1 if there is no left child.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is less than 0 or greater than or equal to <see cref="Count"/>.</para>
        /// </exception>
        public int LeftChildIndex(int index)
        {
            if (index < 0 || index >= _Count)
                throw new ArgumentOutOfRangeException("index");

            index = (index * 2) + 1;
            if (index >= _Count)
                return -1;
            return index;
        }

        /// <summary>
        /// Returns the index of the right child of the element at the specified index in the heap,
        /// or -1 if there is no right child.
        /// </summary>
        /// <param name="index">
        /// The index of the element to get the right child index of.
        /// </param>
        /// <returns>
        /// The index of the right child element of the element at the specified <paramref name="index"/>;
        /// or -1 if there is no right child.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is less than 0 or greater than or equal to <see cref="Count"/>.</para>
        /// </exception>
        public int RightChildIndex(int index)
        {
            if (index < 0 || index >= _Count)
                throw new ArgumentOutOfRangeException("index");

            index = (index * 2) + 2;
            if (index >= _Count)
                return -1;
            return index;
        }

        /// <summary>
        /// Copies the elements of this <see cref="Heap{T}"/> to an <see cref="Array"/>,
        /// starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the elements copied
        /// from this <see cref="Heap{T}"/>. The <see cref="Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="ArgumentException">
        /// <para>The number of elements in the source <see cref="Heap{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</para>
        /// <para>- or -</para>
        /// <para>Type <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</para>
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex", arrayIndex, "arrayIndex cannot be negative");
            if (arrayIndex + Count > array.Length)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The number of elements in the heap ({0}) is greater than the available space in the array ({1}) from the given arrayIndex ({2})", Count, array.Length, arrayIndex));

            for (int index = 0; index < _Count; index++)
                array[arrayIndex + index] = _Items[index];
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from this <see cref="Heap{T}"/>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <paramref name="item"/> was successfully removed from this <see cref="Heap{T}"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <param name="item">
        /// The item to remove from this <see cref="Heap{T}"/>.
        /// </param>
        public bool Remove(T item)
        {
            if (Count == 0)
                return false;

            int index = IndexOf(item);
            if (index < 0)
                return false;

            RemoveAt(index);

            return true;
        }

        /// <summary>
        /// Gets the number of elements contained in this <see cref="Heap{T}"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in this <see cref="Heap{T}"/>.
        /// </returns>
        public int Count
        {
            get
            {
                return _Count;
            }
        }

        /// <summary>
        /// Gets the <see cref="IComparer{T}"/> that dictates the order of the elements in this
        /// <see cref="Heap{T}"/>.
        /// </summary>
        /// <remarks>
        /// Note that if <see cref="Reverse"/> is <c>true</c>, the elements are ordered in the
        /// opposite direction.
        /// </remarks>
        public IComparer<T> Comparer
        {
            get
            {
                return _Comparer;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Heap{T}"/> is ordering
        /// items in the reverse of what <see cref="Comparer"/> dictates.
        /// </summary>
        public bool Reverse
        {
            get
            {
                return _Reverse;
            }
        }

        /// <summary>
        /// Gets the element at the specified index in the heap.
        /// </summary>
        /// <param name="index">
        /// The 0-based index of the element to retrieve.
        /// </param>
        /// <returns>
        /// The object at the specified <paramref name="index"/> in the heap.
        /// </returns>
        /// <remarks>
        /// <para>Note that the heap is not kept in sorted order, but at any time the contents of the heap
        /// satisfies the heap criteria. See <see cref="Heap{T}"/> for more information about this.</para>
        /// <para>Note that to replace an element in the heap, use either <see cref="Replace"/>
        /// or <see cref="ReplaceAt"/>.</para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is less than 0.</para>
        /// <para>- or -</para>
        /// <para><paramrew name="index"/> is greater than or equal to <see cref="Count"/>.</para>
        /// </exception>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _Count)
                    throw new ArgumentOutOfRangeException("index");
                return _Items[index];
            }
        }

        /// <summary>
        /// Gets or sets the capacity of the heap.
        /// </summary>
        /// <remarks>
        /// The capacity cannot be set lower than the current number of elements in the
        /// heap, as returned by <see cref="Count"/>, or an exception will be thrown.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para>The capacity was tried set to a lower number than the number of elements in the heap.</para>
        /// </exception>
        public int Capacity
        {
            get
            {
                return _Items.Length;
            }

            set
            {
                if (value < _Count)
                    throw new ArgumentOutOfRangeException("value", value, "Heap capacity cannot be that small (" + value + "), must at least be equal to Count (" + Count + ")");

                int newSize = Math.Max(value, DefaultSize);
                if (newSize != _Items.Length)
                {
                    var newElements = new T[newSize];
                    if (_Items.Length > 0)
                        Array.Copy(_Items, 0, newElements, 0, Math.Min(_Items.Length, value));
                    _Items = newElements;
                    unchecked
                    {
                        _Version++;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Heap{T}"/> is read-only.
        /// </summary>
        /// <returns>
        /// Always <c>false</c>.
        /// </returns>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of this <see cref="Heap{T}"/>.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this <see cref="Heap{T}"/>.
        /// </returns>
        public Heap<T> Clone()
        {
            var result = new Heap<T>(_Comparer, _Reverse);
            result.Capacity = _Items.Length;
            result._Count = _Count;
            for (int index = 0; index < _Count; index++)
                result._Items[index] = _Items[index];

            result._Version = _Version;
            return result;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
