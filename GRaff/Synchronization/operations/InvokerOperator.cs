using System;

namespace GRaff.Synchronization
{
	internal class InvokerOperator : IAsyncOperator
	{
		private Func<object?, IAsyncOperation> _action;
		private IAsyncOperation? _operation;

		public InvokerOperator(Func<object?, IAsyncOperation> action)
		{
			this._action = action;
		}

		public void Cancel()
		{
			if (_operation == null)
				return;
			_operation.Abort();
		}

		public void Dispatch(object? arg, Action<AsyncOperationResult> callback)
		{
			if (_operation == null)
				_operation = _action(arg);
			_operation.Catch<Exception>(ex => callback(AsyncOperationResult.Failure(ex)));
			_operation.ThenWait(() => callback(AsyncOperationResult.Success()));//operation.ThenWait(() => callback(null));
		}

		public AsyncOperationResult DispatchSynchronously(object? arg)
		{
			if (_operation == null)
				_operation = _action(arg);
			_operation.Dispatch(arg);
			try
			{
				_operation.Wait();
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
		private Func<object?, IAsyncOperation<TPass>> _action;
		private IAsyncOperation<TPass>? _operation;

		public InvokerOperator(Func<object?, IAsyncOperation<TPass>> action)
		{
			this._action = action;
		}

		public void Cancel()
		{
			if (_operation == null)
				return;
			_operation.Abort();
		}

		public void Dispatch(object? arg, Action<AsyncOperationResult> callback)
		{
			if (_operation == null)
				_operation = _action(arg);
			_operation.Catch<Exception>(ex => { callback(AsyncOperationResult.Failure(ex)); });
			_operation.ThenWait(result => callback(AsyncOperationResult.Success(result)));
			_operation.Dispatch(arg);
		}

		public AsyncOperationResult DispatchSynchronously(object? arg)
		{
			if (_operation == null)
				_operation = _action(arg);
			_operation.Dispatch(arg);

			try
			{
				return AsyncOperationResult.Success(_operation.Wait());
			}
			catch (Exception ex)
			{
				return AsyncOperationResult.Failure(ex);
			}
		}
	}
}