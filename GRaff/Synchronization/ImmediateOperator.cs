using System;

namespace GRaff.Synchronization
{
	internal class ImmediateOperator : IAsyncOperator
	{
		private Func<object, object> _action;

		public ImmediateOperator(Func<object, object> action)
		{
			this._action = action;
		}

		public void Cancel()
		{
			return;
		}

		public void Dispatch(object arg, Action<AsyncOperationResult> callback)
		{
			try
			{
				callback(AsyncOperationResult.Success(_action(arg)));
			}
			catch (Exception ex)
			{
				callback(AsyncOperationResult.Failure(ex));
			}
		}

		public AsyncOperationResult DispatchSynchronously(object arg)
		{
			try
			{
				return AsyncOperationResult.Success(_action(arg));
			}
			catch (Exception ex)
			{
				return AsyncOperationResult.Failure(ex);
			}
		}
	}
}