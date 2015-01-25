using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	public sealed class AsyncOperation : AsyncOperationBase, IAsyncOperation
	{
		private CatchContext _catchHandlers = new CatchContext();

		public AsyncOperation()
			: base(null, null)
		{
			State = AsyncOperationState.Completed;
		}

		internal AsyncOperation(AsyncOperationBase preceeding, IAsyncOperator op)
			: base(preceeding, op)
		{ }

		public IAsyncOperation Then(Action action)
		{
			var continuation = new AsyncOperation(this, new SerialOperator(obj => { action(); return null; })); /*C#6.0*/// semicolon
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation<TNext> Then<TNext>(Func<TNext> action)
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

		public new void Wait()
		{
			var result = base.Wait();
			if (!result.IsSuccessful)
				throw new AsyncException(result.Error);
		}

		protected override AsyncOperationResult Handle(Exception exception)
		{
			if (_catchHandlers.TryHandle(exception))
				return AsyncOperationResult.Success();
			else
				return AsyncOperationResult.Failure(exception);
		}

	}
}