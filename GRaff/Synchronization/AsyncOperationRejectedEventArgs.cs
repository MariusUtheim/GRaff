using System;

namespace GRaff.Synchronization
{
	public class AsyncOperationRejectedEventArgs : EventArgs
	{
		public AsyncOperationRejectedEventArgs(Type exceptionType, Exception exception)
		{
			this.ExceptionType = exceptionType;
			this.Exception = exception;
		}

		public Exception Exception { get; private set; }
		public Type ExceptionType { get; private set; }
	}
}