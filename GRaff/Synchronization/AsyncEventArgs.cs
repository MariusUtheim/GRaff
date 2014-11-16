using System;
using System.Threading;

namespace GRaff.Synchronization
{
	public class AsyncEventArgs
	{
		private Action _action;
		private int _isResolved = 0;

		public AsyncEventArgs(Action action)
		{
			this._action = action;
		}

		public bool Resolve()
		{
			return Interlocked.Exchange(ref _isResolved, 1) == 0;
		}

		public Action Action
		{
			get
			{
				return _action;
			}
		}
	}
}