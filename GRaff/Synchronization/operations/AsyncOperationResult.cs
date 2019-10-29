using System;


namespace GRaff.Synchronization
{
	internal class AsyncOperationResult
	{
		public static AsyncOperationResult Success() { return new AsyncOperationResult { IsSuccessful = true }; }

		public static AsyncOperationResult Success(object? result)
		{
			return new AsyncOperationResult { IsSuccessful = true, Value = result };
		}

		public static AsyncOperationResult Failure(Exception exception)
		{
			return new AsyncOperationResult { IsSuccessful = false, Error = exception };
		}

		public bool IsSuccessful;
		public object? Value;
		public Exception? Error;

	}
}
