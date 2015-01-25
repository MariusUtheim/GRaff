using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace GRaff.Synchronization
{
	public class AsyncOperation<TPass> : AsyncOperationBase, IAsyncOperation<TPass>
	{
		private CatchContext<TPass> _catchHandlers = new CatchContext<TPass>();

		public AsyncOperation(TPass value)
			: base(AsyncOperationResult.Success(value))
		{
		}


		internal AsyncOperation(AsyncOperationBase preceeding, IAsyncOperator op)
			: base(preceeding, op)
		{ }

		public IAsyncOperation Then(Action<TPass> action)
		{
			var continuation = new AsyncOperation(this, new SerialOperator(obj => { action((TPass)obj); return null; }));
			Then(continuation);
			return continuation;
		}

		public IAsyncOperation<TNext> Then<TNext>(Func<TPass, TNext> action)
		{
			var continuation = new AsyncOperation<TNext>(this, new SerialOperator(obj => action((TPass)obj)));
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
			var result = base.Wait();
			if (result.IsSuccessful)
				return (TPass)result.Value;
			else
				throw new AsyncException(result.Error);
		}

		protected override AsyncOperationResult Handle(Exception exception)
		{
			TPass resultValue;
			if (_catchHandlers.TryHandle(exception, out resultValue))
				return AsyncOperationResult.Success(resultValue);
			else
				return AsyncOperationResult.Failure(exception);
		}
	}
}