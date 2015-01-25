using System;

namespace GRaff.Synchronization
{
	[Serializable]
	public class AsyncException : Exception
	{
		public AsyncException(Exception innerException)
			: base("An asynchronous operation threw an exception. See the inner exception for more details.", innerException)
		{
		}

		public AsyncException(string message, Exception innerException) 
			: base(message, innerException)
		{
		}
	}
}