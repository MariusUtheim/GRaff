using System;

namespace GRaff.Synchronization
{
	internal class AsyncException : Exception
	{
		public AsyncException(Exception innerException)
			: base("An asynchronous operation threw an exception.", innerException)
		{
		}
	}
}