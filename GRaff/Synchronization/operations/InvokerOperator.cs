using System;

namespace GRaff.Synchronization
{
	internal class InvokerOperator : IAsyncOperator
	{
		private Func<object, IAsyncOperation> action;
		private IAsyncOperation operation;

		public InvokerOperator(Func<object, IAsyncOperation> action)
		{
			this.action = action;
		}

		public void Cancel()
		{
			if (operation == null)
				return;
			operation.Abort();
		}

		public void Dispatch(object arg, Action<AsyncOperationResult> callback)
		{
			if (operation == null)
				operation = action(arg);
			operation.Catch<Exception>(ex => callback(AsyncOperationResult.Failure(ex)));   /*C#6.0*/// Semicolon
			operation.ThenWait(() => callback(AsyncOperationResult.Success()));//operation.ThenWait(() => callback(null));
		}

		public AsyncOperationResult DispatchSynchronously(object arg)
		{
			if (operation == null)
				operation = action(arg);
			operation.Dispatch(arg);
			try
			{
				operation.Wait();
				return AsyncOperationResult.Success();
			}
			catch (Exception ex)
			{
				return AsyncOperationResult.Failure(ex);
			}
		}
	}

	internal class InvokerOperator<TPass> : IAsyncOperator
	{
		private Func<object, IAsyncOperation<TPass>> action;
		private IAsyncOperation<TPass> operation;

		public InvokerOperator(Func<object, IAsyncOperation<TPass>> action)
		{
			this.action = action;
		}

		public void Cancel()
		{
			if (operation == null)
				return;
			operation.Abort();
		}

		public void Dispatch(object arg, Action<AsyncOperationResult> callback)
		{
			if (operation == null)
				operation = action(arg);
			operation.Catch<Exception>(ex => { callback(AsyncOperationResult.Failure(ex)); return default(TPass); }); /*C#6.0*/// Semicolon
			operation.ThenWait(result => callback(AsyncOperationResult.Success(result)));
			operation.Dispatch(arg);
		}

		public AsyncOperationResult DispatchSynchronously(object arg)
		{
			if (operation == null)
				operation = action(arg);
			operation.Dispatch(arg);

			try
			{
				return AsyncOperationResult.Success(operation.Wait());
			}
			catch (Exception ex)
			{
				return AsyncOperationResult.Failure(ex);
			}
		}
	}
}