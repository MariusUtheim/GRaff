using System;


namespace GRaff.Synchronization
{
	internal class AsyncOperationResult
	{
		internal static AsyncOperationResult Null = new AsyncOperationResult { IsSuccessful = true, Value = null };

		internal static AsyncOperationResult Success() { return new AsyncOperationResult { IsSuccessful = true, Value = null }; }

		internal static AsyncOperationResult Success(object result)
		{
			return new AsyncOperationResult { IsSuccessful = true, Value = result };
		}

		internal static AsyncOperationResult Failure(Exception exception)
		{
			return new AsyncOperationResult { IsSuccessful = false, Value = exception };
		}

		internal bool IsSuccessful;
		internal object Value;
	}
}
