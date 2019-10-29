using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace GRaff.Synchronization
{
	internal class CatchContext
	{
		private readonly List<KeyValuePair<Type, Action<Exception>>> _handledTypes = new List<KeyValuePair<Type, Action<Exception>>>();

		public void Catch<TException>(Action<TException> catchHandler) where TException : Exception
		{
			_handledTypes.Add(new KeyValuePair<Type, Action<Exception>>(typeof(TException), exception => catchHandler((TException)exception)));
		}

		public bool TryHandle(Exception exception)
		{
			var exceptionType = exception.GetType();

			var handler = _handledTypes.Where(pair => pair.Key.IsAssignableFrom(exceptionType))
									   .Select(pair => pair.Value)
									   .FirstOrDefault();

			if (handler == null)
				return false;

			try
			{
				handler?.Invoke(exception);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}

	internal class CatchContext<TResult>
	{
		private readonly List<KeyValuePair<Type, Func<Exception, TResult>>> _handlers = new List<KeyValuePair<Type, Func<Exception, TResult>>>();

		public void Catch<TException>(Func<TException, TResult> catchHandler) where TException : Exception
		{
			_handlers.Add(new KeyValuePair<Type, Func<Exception, TResult>>(typeof(TException), exception => catchHandler((TException)exception)));
		}

		public bool TryHandle(Exception exception, out TResult result)
		{
			var exceptionType = exception.GetType();

			var query = from pair in _handlers
						where pair.Key.IsAssignableFrom(exceptionType)
						select pair.Value;

			var handler = query.FirstOrDefault();

			if (handler == null)
			{
				result = default;
				return false;
			}

			try
			{
				result = handler(exception);
				return true;
			}
			catch
			{
				result = default;
				return false;
			}
		}

	}
}