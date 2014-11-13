using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GRaff.Synchronization
{
	internal class MergeOrganizer
	{
		private ConcurrentBag<object> _elements = new ConcurrentBag<object>();
		private int _branchCount;

		public MergeOrganizer(MergeOrganizer previous, int branchCount)
		{
			this.Previous = previous;
			this._branchCount = branchCount;
		}

		public MergeOrganizer Previous { get; private set; }

		public void Merge<TPass>(TPass element)
		{
			_elements.Add(element);
		}

		public bool IsComplete
		{
			get
			{
				return (_elements.Count == _branchCount);
			}
		}

		public IEnumerable<TPass> Result<TPass>()
		{
			return _elements.ToArray().Cast<TPass>();
		}
	}
}