using System;
using System.Diagnostics;

namespace GRaff.Synchronization
{
	internal class SerialOperator : IAsyncOperator
	{
		private AsyncEventArgs _actionHandle;
		private readonly Func<object, object> _action;
		private AsyncOperationResult _result;

		internal SerialOperator(Func<object, object> action)
		{
			_action = action;
			_actionHandle = new AsyncEventArgs(() => { });
		}

		public AsyncOperationResult DispatchSynchronously(object arg)
		{
			if (_actionHandle.Resolve())
			{
				try
				{
					return _result = AsyncOperationResult.Success(_action(arg));
				}
				catch (Exception ex)
				{
					return _result = AsyncOperationResult.Failure(ex);
				}
			}
			else
				return _result;
		}

		public void Dispatch(object arg, Action<AsyncOperationResult> callback)
		{
			_actionHandle = new AsyncEventArgs(() => 
			{
				try
				{
					_result = AsyncOperationResult.Success(_action(arg));
					callback(_result);
				}
				catch (Exception ex)
				{
					_result = AsyncOperationResult.Failure(ex);
                    callback(_result);
				}
			});
			Async.Dispatch(_actionHandle);
		}

		public void Cancel()
		{
			_actionHandle.Resolve();
		}

		public void GetResult()
		{
			Debug.Assert(_result != null);
		}
	}
}
