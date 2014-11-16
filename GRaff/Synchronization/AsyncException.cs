using System;

namespace GRaff.Synchronization
{
	public class AsyncException : Exception
	{
		public AsyncException(Exception innerException)
			: base("An asynchronous operation threw an exception. See the inner exception for more details.", innerException)
		{
		}
	}
}