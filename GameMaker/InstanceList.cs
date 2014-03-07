using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	internal class InstanceList : IList<GameObject>
	{
		private List<GameObject> _list;
		private List<InstanceEnumerator> _enumerators;

		public InstanceList()
		{
			_list = new List<GameObject>();
			_enumerators = new List<InstanceEnumerator>();
		}

		public void RemoveEnumerator(InstanceEnumerator enumerator)
		{
			_enumerators.Remove(enumerator);
		}

		public int IndexOf(GameObject item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, GameObject item)
		{
			_list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		public GameObject this[int index]
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

		public void Add(GameObject item)
		{
			_list.Add(item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(GameObject item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(GameObject[] array, int arrayIndex)
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

		public bool Remove(GameObject item)
		{
			int pos = IndexOf(item);
			foreach (var enumerator in _enumerators.Where(e => e.Index > pos))
				enumerator.MovePrevious();
			return _list.Remove(item);
		}

		public IEnumerator<GameObject> GetEnumerator()
		{
			var enumerator = new InstanceEnumerator(this);
			_enumerators.Add(enumerator);
			return enumerator;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	internal class InstanceEnumerator : IEnumerator<GameObject>
	{
		private InstanceList _list;

		public InstanceEnumerator(InstanceList list)
		{
			this._list = list;
			Reset();
		}

		public int Index { get; private set; }

		public GameObject Current
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
