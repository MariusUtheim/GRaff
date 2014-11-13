using System;
using System.Collections.Generic;

namespace GRaff.Synchronization
{
	internal class AsyncCatchContext
	{
		private List<KeyValuePair<Type, Action<Exception>>> handledTypes = new List<KeyValuePair<Type, Action<Exception>>>();

		internal void Catch<TException>(Action<TException> catchHandler) where TException : Exception
		{
			handledTypes.Add(new KeyValuePair<Type, Action<Exception>>(typeof(TException), exception => catchHandler((TException)exception)));
		}

		internal bool TryHandle(Exception exception)
		{
			Type exceptionType = exception.GetType();

			foreach (var pair in handledTypes)
			{
				if (exceptionType.IsAssignableFrom(pair.Key))
				{
					pair.Value.Invoke(exception);
					return true;
				}
			}

			return false;
		}
	}
}