using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	internal class AsyncOperation : IAsyncOperation
	{
		private CatchContext _catchHandlers = new CatchContext();
		private ConcurrentQueue<AsyncOperation> _continuations = new ConcurrentQueue<AsyncOperation>();
		private AsyncOperation _preceedingOperation;
		private bool _hasPassedException = false;
		private Deferred<Exception> _otherwiseClause = null;

		internal AsyncOperation()
			: this(false)
		{ }

		internal AsyncOperation(bool deferred)
		{
			if (deferred)
			{
				State = AsyncOperationState.Deferred;
			}
			else
			{
				Result = null;
				State = AsyncOperationState.Completed;
			}
		}

		internal AsyncOperation(AsyncOperationResult result)
		{
			Result = result;
			State = result.IsSuccessful ? AsyncOperationState.Completed : AsyncOperationState.Failed;
		}

		internal AsyncOperation(IAsyncOperator op)
			: this(null, op)
		{ }

		internal AsyncOperation(AsyncOperation preceeding, IAsyncOperator op)
		{
			State = AsyncOperationState.Initial;
			_preceedingOperation = preceeding;
			Operator = op;
		}

		~AsyncOperation()
		{
			if (!_hasPassedException && Result != null && !Result.IsSuccessful && _otherwiseClause == null && Giraffe.IsRunning)
				Async.ThrowException(new AsyncException("An asynchronous operation threw an exception that was finalized before it was handled. See the inner exception for more details.", Result.Error));
		}

		protected IAsyncOperator Operator { get; private set; }

		protected AsyncOperationResult Result { get; set; }

		public AsyncOperationState State { get; protected set; }



		protected void _assertState(string verb)
		{
			if (State == AsyncOperationState.Aborted) throw new InvalidOperationException("Cannot " + verb + " an aborted operation.");
			if (IsDone) throw new InvalidOperationException("Cannot " + verb + " a done operation.");
		}

		private void _complete(AsyncOperationResult result)
		{
			if (result.IsSuccessful)
				Accept(result.Value);
			else
				Reject(result.Error);
		}


		public void Dispatch(object arg)
		{
			if (State == AsyncOperationState.Initial)
			{
				State = AsyncOperationState.Dispatched;
				Operator.Dispatch(arg, _complete);
			}
		}

		internal virtual AsyncOperationResult Handle(Exception exception)
		{
			if (_catchHandlers.TryHandle(exception))
				return AsyncOperationResult.Success();
			else
				return AsyncOperationResult.Failure(exception);
		}

		internal void Accept(object result)
		{
			State = AsyncOperationState.Completed;
			Result = AsyncOperationResult.Success(result);
			AsyncOperation continuation;
			while (_continuations.TryDequeue(out continuation))
				continuation.Dispatch(Result?.Value);
		}

		internal void Reject(Exception reason)
		{
			Result = Handle(reason);
			if (Result.IsSuccessful)
				Accept(Result.Value);
			else
			{
				State = AsyncOperationState.Failed;
				AsyncOperation continuation;
				while (_continuations.TryDequeue(out continuation))
					continuation._complete(Result);
				if (IsDone)
					Async.ThrowException(reason);


				lock (this)
					_otherwiseClause?.Accept(reason);
			}
		}

		protected AsyncOperationResult _Wait()
		{
			switch (State)
			{
				case AsyncOperationState.Deferred:
					throw new InvalidOperationException("Cannot wait for an AsyncOperation that is Deferred");

				case AsyncOperationState.Aborted:
					return AsyncOperationResult.Failure(new InvalidOperationException("Cannot wait for an operation that has been aborted."));

				case AsyncOperationState.Failed:
					return AsyncOperationResult.Failure(new AsyncException(Result.Error));

				case AsyncOperationState.Initial:
					if (_preceedingOperation == null)
						return Operator.DispatchSynchronously(null);
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




		public void Wait()
		{
			var result = _Wait();

			if (!result.IsSuccessful)
				result = Handle(result.Error);

			if (result.IsSuccessful)
				Accept(result.Value);
			else
			{
				result = Handle(result.Error);
				State = AsyncOperationState.Failed;
				AsyncOperation continuation;
				while (_continuations.TryDequeue(out continuation))
					continuation._complete(Result);
				throw new AsyncException(result.Error);
			}
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


		protected void Then(AsyncOperation continuation)
		{
			_assertState("add a continuation to");
			_hasPassedException = true;
			if (State == AsyncOperationState.Completed)
				continuation.Dispatch(Result?.Value);
			else
				_continuations.Enqueue(continuation);
		}

		public IAsyncOperation Then(Func<IAsyncOperation> action)
		{
			var continuation = new AsyncOperation(this, new InvokerOperator(obj => action()));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation<TNext> Then<TNext>(Func<IAsyncOperation<TNext>> action)
		{
			var continuation = new AsyncOperation<TNext>(this, new InvokerOperator<TNext>(obj => action()));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation ThenWait(Action action)
		{
			var continuation = new AsyncOperation(this, new ImmediateOperator(obj => { action(); return null; }));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation<TNext> ThenWait<TNext>(Func<TNext> action)
		{
			var continuation = new AsyncOperation<TNext>(this, new ImmediateOperator(obj => action()));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation ThenSync(Action action)
		{
			var continuation = new AsyncOperation(this, new SerialOperator(obj => { action(); return null; }));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation<TNext> ThenSync<TNext>(Func<TNext> action)
		{
			var continuation = new AsyncOperation<TNext>(this, new SerialOperator(obj => action()));
			Then(continuation);
			return continuation;
		}


		public IAsyncOperation ThenAsync(Func<Task> action)
		{
			var continuation = new AsyncOperation(this, new ParallelOperator(async obj => { await action(); return default(object); }));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation<TNext> ThenAsync<TNext>(Func<Task<TNext>> action)
		{
			var continuation = new AsyncOperation<TNext>(this, new ParallelOperator(async obj => await action()));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation Catch<TException>(Action<TException> exceptionHandler) where TException : Exception
		{
			_assertState("add a catch handler to");
			_catchHandlers.Catch<TException>(exceptionHandler);
			return this;
		}

		public IAsyncOperation<Exception> Otherwise()
		{
			lock (this)
			{
				if (_otherwiseClause == null)
				{
					_otherwiseClause = new Deferred<Exception>();
					if (State == AsyncOperationState.Failed)
						_otherwiseClause.Accept(Result.Error);
				}
			}

			return _otherwiseClause.Operation;	
		}
	}
}