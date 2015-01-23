﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;
using GRaff.Synchronization;

namespace GRaff
{
#if PUBLISH
	internal static class Async
#else
#warning Missing documentation
	public static class Async
#endif
	{
		private static List<AsyncEventArgs> _queuedEvents = new List<AsyncEventArgs>();
		private static List<Exception> _exceptions = new List<Exception>();
		private static CatchContext _catcher = new CatchContext();

		public static Dispatcher MainThreadDispatcher
		{
			get; internal set;
		}

		public static void CatchException<TException>(Action<TException> exceptionHandler)
			where TException : Exception
		{
			_catcher.Catch(exceptionHandler);
		}

		public static AsyncOperation Run(Action action)
		{
			return new AsyncOperation().Then(action);
		}

		public static AsyncOperation<TPass> Run<TPass>(Func<TPass> action)
		{
			return new AsyncOperation().Then(action);
		}

		public static void ThrowException(Exception ex)
		{
			_exceptions.Add(ex);
		}

		public static void Dispatch(AsyncEventArgs e)
		{
			lock (_queuedEvents)
				_queuedEvents.Add(e);
		}

		public static void HandleEvents()
		{
			AsyncEventArgs[] processingEvents;

			lock (_exceptions)
			{
				foreach (AsyncException exception in _exceptions)
				{
					if (!_catcher.TryHandle(exception.InnerException))
						throw new AsyncException(exception);
				}

				lock (_queuedEvents)
				{
					processingEvents = _queuedEvents.ToArray();
					_queuedEvents = new List<AsyncEventArgs>();
				}
			}

			for (int i = 0; i < processingEvents.Length; i++)
			{
				if (processingEvents[i].Resolve())
				{
					try
					{
						processingEvents[i].Action();
					}
					catch (AsyncException)
					{
#warning Throwing a custom AsyncException should throw a wrapped AsyncException?
						throw;
					}
					catch (Exception ex)
					{
						if (!_catcher.TryHandle(ex))
							throw new AsyncException(ex);
					}
				}
			}
		}
	}
}
