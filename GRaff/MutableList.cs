using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	internal class GameElementDepthComparer : IComparer<GameElement>
	{
		public static readonly GameElementDepthComparer Instance = new GameElementDepthComparer();

		private GameElementDepthComparer() { }

		public int Compare(GameElement x, GameElement y)
		{
			return -(x.Depth - y.Depth);
		}
	}

	internal class InstanceList : MutableList<GameElement>
	{
		public void Sort()
		{
		//	_list.Sort(GameElementDepthComparer.Instance);
			_list = _list.OrderBy(instance => instance.Depth).ToList();
		}

		public override void Add(GameElement item)
		{
			_list.Add(item);
			Sort();
		}
	}

	internal class MutableList<T> : IList<T>
	{
		protected List<T> _list;
		protected List<MutableEnumerator<T>> _enumerators;

		public MutableList()
		{
			_list = new List<T>();
			_enumerators = new List<MutableEnumerator<T>>();
		}


		public void RemoveEnumerator(MutableEnumerator<T> enumerator)
		{
			_enumerators.Remove(enumerator);
		}

		public int IndexOf(T item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			throw new InvalidOperationException("Cannot insert at a specified position, as elements should be sorted.");
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		public T this[int index]
		{
			get
			{
				return _list[index];
			}
			set
			{
				_list[index] = value;
			}
		}

		public virtual void Add(T item)
		{
			_list.Add(item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(T item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(T item)
		{
			int pos = IndexOf(item);
			foreach (var enumerator in _enumerators.Where(e => e.Index >= pos))
				enumerator.MovePrevious();
			return _list.Remove(item);
		}

		public IEnumerator<T> GetEnumerator()
		{
			var enumerator = new MutableEnumerator<T>(this);
			_enumerators.Add(enumerator);
			return enumerator;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	internal class MutableEnumerator<T> : IEnumerator<T>
	{
		private MutableList<T> _list;

		public MutableEnumerator(MutableList<T> list)
		{
			this._list = list;
			Reset();
		}

		public int Index { get; private set; }

		public T Current
		{
			get { return _list[Index]; }
		}

		public void Dispose()
		{
			_list.RemoveEnumerator(this);
		}

		object System.Collections.IEnumerator.Current
		{
			get { return _list[Index]; }
		}

		public bool MoveNext()
		{
			if (++Index < _list.Count)
				return true;
			return false;
		}

		public void MovePrevious()
		{
			Index--;
		}

		public void Reset()
		{
			Index = -1;
		}
	}
}
