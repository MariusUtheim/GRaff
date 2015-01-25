using System;
using System.Collections.Generic;
using System.Linq;

namespace GRaff.Synchronization
{
	public class CatchContext
	{
		List<KeyValuePair<Type, Action<Exception>>> _handledTypes = new List<KeyValuePair<Type, Action<Exception>>>();

		public void Catch<TException>(Action<TException> catchHandler) where TException : Exception
		{
			_handledTypes.Add(new KeyValuePair<Type, Action<Exception>>(typeof(TException), exception => catchHandler((TException)exception)));
		}

		public bool TryHandle(Exception exception)
		{
			var exceptionType = exception.GetType();

			var query = from pair in _handledTypes
						  where pair.Key.IsAssignableFrom(exceptionType)
						  select pair.Value;

			var handler = query.FirstOrDefault();

			if (handler == null)
				return false;
			else
			{
				handler.Invoke(exception);	 /*C#6.0*/
				return true;
			}
		}
	}

	public class CatchContext<TResult>
	{
		List<KeyValuePair<Type, Func<Exception, TResult>>> _handlers = new List<KeyValuePair<Type, Func<Exception, TResult>>>();

		public void Catch<TException>(Func<TException, TResult> catchHandler) where TException : Exception
		{
			_handlers.Add(new KeyValuePair<Type, Func<Exception, TResult>>(typeof(TException), exception => catchHandler((TException)exception)));
		}

		public bool TryHandle(Exception exception, out TResult result)
		{
			var exceptionType = exception.GetType();

			var query = from pair in _handlers
						where exceptionType.IsAssignableFrom(pair.Key)
						select pair.Value;

			var handler = query.FirstOrDefault();

			if (handler == null)
			{
				result = default(TResult);
				return false;
			}
			else
			{
				result = handler(exception);
				return true;
			}
		}

	}
}