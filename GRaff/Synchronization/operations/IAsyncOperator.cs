using System;

namespace GRaff.Synchronization
{
	internal interface IAsyncOperator
	{
		AsyncOperationResult DispatchSynchronously(object? arg);
		void Cancel();
		void Dispatch(object? arg, Action<AsyncOperationResult> callback);
	}
}