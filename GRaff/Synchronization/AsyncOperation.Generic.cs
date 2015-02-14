using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	internal class AsyncOperation<TPass> : AsyncOperation, IAsyncOperation<TPass>
	{
		private CatchContext<TPass> _catchHandlers = new CatchContext<TPass>();

		internal AsyncOperation()
		{
			State = AsyncOperationState.Deferred;
		}

		public AsyncOperation(TPass value)
			: base(AsyncOperationResult.Success(value))
		{
		}

		internal AsyncOperation(AsyncOperation preceeding, IAsyncOperator op)
			: base(preceeding, op)
		{ }

		public IAsyncOperation Then(Func<TPass, IAsyncOperation> action)
		{
			var continuation = new AsyncOperation(this, new InvokerOperator(obj => action((TPass)obj)));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation<TNext> Then<TNext>(Func<TPass, IAsyncOperation<TNext>> action)
		{
			var continuation = new AsyncOperation<TNext>(this, new InvokerOperator<TNext>(obj => action((TPass)obj)));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation ThenSync(Action<TPass> action)
		{
			var continuation = new AsyncOperation(this, new SerialOperator(obj => { action((TPass)obj); return null; }));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation<TNext> ThenSync<TNext>(Func<TPass, TNext> action)
		{
			var continuation = new AsyncOperation<TNext>(this, new SerialOperator(obj => action((TPass)obj)));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation ThenWait(Action<TPass> action)
		{
			var continuation = new AsyncOperation(this, new ImmediateOperator(obj => { action((TPass)obj); return null; }));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation<TNext> ThenWait<TNext>(Func<TPass, TNext> action)
		{
			var continuation = new AsyncOperation<TNext>(this, new ImmediateOperator(obj => action((TPass)obj)));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation ThenAsync(Func<TPass, Task> action)
		{
			var continuation = new AsyncOperation(this, new ParallelOperator(async obj => { await action((TPass)obj); return default(object); }));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation<TNext> ThenAsync<TNext>(Func<TPass, Task<TNext>> action)
		{
			var continuation = new AsyncOperation<TNext>(this, new ParallelOperator(async obj => await action((TPass)obj)));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation<TPass> Catch<TException>(Func<TException, TPass> exceptionHandler) where TException : Exception
		{
			_assertState("add a catch handler to");
			_catchHandlers.Catch(exceptionHandler);
			return this;
		}

		public new TPass Wait()
		{
			var result = base._Wait();
			if (result.IsSuccessful)
				return (TPass)result.Value;
			else
				throw new AsyncException(result.Error);
		}

		internal override AsyncOperationResult Handle(Exception exception)
		{
			TPass resultValue;
			if (_catchHandlers.TryHandle(exception, out resultValue))
				return AsyncOperationResult.Success(resultValue);
			else
				return AsyncOperationResult.Failure(exception);
		}
	}
}