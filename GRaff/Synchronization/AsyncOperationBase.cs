using System;
using System.Collections.Concurrent;


namespace GRaff.Synchronization
{
	public abstract class AsyncOperationBase : IAsyncOperationBase
	{
		private ConcurrentQueue<AsyncOperationBase> _continuations = new ConcurrentQueue<AsyncOperationBase>();
		private AsyncOperationBase _preceedingOperation;
		private bool _hasPassedException = false;

		public AsyncOperationBase(IAsyncOperator op)
		{
			State = AsyncOperationState.Initial;
			_preceedingOperation = null;
			Operator = op;
		}

		protected AsyncOperationBase(AsyncOperationResult result)
		{
			Result = result;
			State = result.IsSuccessful ? AsyncOperationState.Completed : AsyncOperationState.Failed;
		}

		protected AsyncOperationBase(AsyncOperationBase preceeding, IAsyncOperator op)
		{
			State = AsyncOperationState.Initial;
			_preceedingOperation = preceeding;
			Operator = op;
		}

		~AsyncOperationBase()
		{
			if (!_hasPassedException && Result != null && !Result.IsSuccessful && Giraffe.IsRunning)
				Async.ThrowException(new AsyncException("An asynchronous operation threw an exception that was finalized before it was handled. See the inner exception for more details.", Result.Error));
		}

		protected IAsyncOperator Operator { get; private set; }

		protected AsyncOperationResult Result { get; private set; }

		public AsyncOperationState State { get; protected set; }

		/// <summary>
		/// Gets whether this async operation is marked as done.
		/// </summary>
		public bool IsDone { get; private set; }

		/// <summary>
		/// Marks the operation as done. When an operation is done, it is not possible to add further continuations or catch handlers.
		/// Unhandled exceptions will be thrown during the Async step. 
		/// </summary>
		public void Done()
		{
			if (Result != null && !Result.IsSuccessful)
				throw new AsyncException(Result.Error);
			IsDone = true;
			_hasPassedException = true;
		}

		public void Abort()
		{
			if (State == AsyncOperationState.Aborted)
				return;

			if (State == AsyncOperationState.Dispatched)
				Operator.Cancel();

			State = AsyncOperationState.Aborted;

			foreach (var continuation in _continuations)
				continuation.Abort();
		}

		protected void _assertState(string verb)
		{
			if (State == AsyncOperationState.Aborted) throw new InvalidOperationException("Cannot " + verb + " an aborted operation.");
			if (IsDone) throw new InvalidOperationException("Cannot " + verb + " a done operation.");
		}

		protected void Then(AsyncOperationBase continuation)
		{
			_assertState("add a continuation to");
			_hasPassedException = true;
			if (State == AsyncOperationState.Completed)
				continuation.Dispatch(Result != null ? Result.Value : null); /*C#6.0*/
			else
				_continuations.Enqueue(continuation);
		}

		public void Wait()
		{
			var result = _Wait();
			if (!result.IsSuccessful)
				throw new AsyncException(result.Error);
		}

		protected AsyncOperationResult _Wait()
		{
			switch (State)
			{
				case AsyncOperationState.Aborted:
					return AsyncOperationResult.Failure(new InvalidOperationException("Cannot wait for an operation that has been aborted."));

				case AsyncOperationState.Failed:
					return AsyncOperationResult.Failure(new AsyncException(Result.Error));

				case AsyncOperationState.Initial:
					var pass = _preceedingOperation._Wait();
					if (!pass.IsSuccessful)
						pass = Handle(pass.Error);
					if (pass.IsSuccessful)
						return Operator.DispatchSynchronously(pass.Value);
					else
						return pass;

				case AsyncOperationState.Dispatched:
					if (_preceedingOperation != null)
						return Operator.DispatchSynchronously(_preceedingOperation.Result);
					else
						return Operator.DispatchSynchronously(null);

				case AsyncOperationState.Completed:
					return Result;

				default:
					throw new NotSupportedException("Unsupported AsyncOperationState '" + Enum.GetName(typeof(AsyncOperationState), State) + "'");
			}
			
		}

		/// <summary>
		/// Is called when an operator completes. The result can be successful or failed.
		/// This should try to handle the exception, then pass the result to all continuations.
		/// </summary>
		private void _completed(AsyncOperationResult result)
		{
			Console.WriteLine("Thing completed");
			if (result.IsSuccessful)
				Accept(result.Value);
			else
				Reject(result.Error);
		}

		/// <summary>
		/// Signals that the operation has completed with the specified result.
  /// The result should be passed to all continuations.
		/// </summary>
		private void Accept(object result)
		{
			State = AsyncOperationState.Completed;
			Result = AsyncOperationResult.Success(result);
			AsyncOperationBase continuation;
			while (_continuations.TryDequeue(out continuation))
				continuation.Dispatch(Result != null ? Result.Value : null); /*C#6.0*/
		}

		/// <summary>
		/// Signals that the operation failed with the specified reason.
		/// If the reason can be handled, it will call Accept instead. Otherwise, the error
		/// will be passed to all continuations. If this operation is marked as Done, the error
		/// will be thrown on Async.
		/// </summary>
		private void Reject(Exception reason)
		{
			Result = Handle(reason);
			if (Result.IsSuccessful)
				Accept(Result.Value);
			else
			{
				State = AsyncOperationState.Failed;
				AsyncOperationBase continuation;
				while (_continuations.TryDequeue(out continuation))
					continuation._completed(Result);
				if (IsDone)
					Async.ThrowException(reason);
			}
		}

		public void Dispatch(object arg)
		{
			if (State == AsyncOperationState.Initial)
			{
				State = AsyncOperationState.Dispatched;
				Operator.Dispatch(arg, _completed);
			}
		}

		protected abstract AsyncOperationResult Handle(Exception exception);
	}
}
