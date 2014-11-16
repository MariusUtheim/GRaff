using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace GRaff.Synchronization
{
	public abstract class AsyncOperationBase
	{
		protected AsyncOperationBase _preceedingOperation = null;
		protected ConcurrentQueue<AsyncOperationBase> _continuations = new ConcurrentQueue<AsyncOperationBase>();
		protected AsyncEventArgs _actionHandle;
		internal AsyncOperationResult _result;	 /*C#6.0*/

		public AsyncOperationState State { get; protected set; }
		public bool IsDone { get; private set; }


		public void Abort()
		{
			if (State == AsyncOperationState.Aborted)
				return;
			if (State == AsyncOperationState.Dispatched)
				_actionHandle.Resolve();

			State = AsyncOperationState.Aborted;

			foreach (var continuation in _continuations)
				continuation.Abort();
		}

		public void Done()
		{
			if (_continuations.Count > 0) throw new InvalidOperationException("Cannot mark an operation as Done if it has continuations.");
			if (_result != null && !_result.IsSuccessful) throw new AsyncException((Exception)_result.Value);
			IsDone = true;
		}

		protected void assertState(string verb)
		{
			if (State == AsyncOperationState.Aborted) throw new InvalidOperationException("Cannot " + verb + " an aborted operation.");
			if (IsDone) throw new InvalidOperationException("Cannot " + verb + " a done operation.");
		}

		protected void then(AsyncOperationBase continuation)
		{
			assertState("add a continuation to");
			if (State == AsyncOperationState.Completed)
				continuation.Dispatch(_result);

			_continuations.Enqueue(continuation);
		}

		internal abstract void Dispatch(AsyncOperationResult result);

		protected void passToAll()
		{
			AsyncOperationBase continuation;
			while (_continuations.TryDequeue(out continuation))
				continuation.Dispatch(_result);
		}

		protected void wait()
		{
			switch (State)
			{
				case AsyncOperationState.Aborted:
					throw new InvalidOperationException("Cannot wait for an operation that has been aborted.");

				case AsyncOperationState.Failed:
					throw new AsyncException((Exception)_result.Value);

				case AsyncOperationState.Initial:
					Debug.Assert(_preceedingOperation != null);
					_preceedingOperation.wait();
					goto case AsyncOperationState.Dispatched;

				case AsyncOperationState.Dispatched:
					_actionHandle.Resolve();
					_actionHandle.Action();
					passToAll();
					goto case AsyncOperationState.Completed;

				case AsyncOperationState.Completed:
					return;

				default:
					throw new NotSupportedException("Unsupported AsyncOperationState '" + Enum.GetName(typeof(AsyncOperationState), State) + "'");
			}
		}
	}
}