using System;
using System.Threading;

namespace GRaff.Synchronization
{
	public class AsyncEventArgs : EventArgs
	{
		private int _isResolved = 0;

		public AsyncEventArgs(Action action)
		{
			if (action == null) throw new ArgumentNullException("action");
			Action = action;
		}

		/// <summary>
		/// Indicate this event as being resolved. If the action is already resolved somewhere else, it returns false; otherwise returns true.
		/// If true is returned, the caller of GRaff.Synchronization.AsyncEventArgs.Resolve() should perform an action.
		/// </summary>
		/// <returns>True if this event should be resolved; otherwise, false.</returns>
		/// <example>
		/// if (e.Resolve())
		///		e.Action();
		/// </example>
		public bool Resolve()
		{
			return Interlocked.Exchange(ref _isResolved, 1) == 0;
		}

		public Action Action { get; private set; }
	}
}