using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GRaff.Synchronization
{
#if PUBLISH
	internal static class Async
#else
#warning Missing documentation
	public static partial class Async
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

		public static IAsyncOperation Operation()
		{
			return new AsyncOperation();
		}

		public static IAsyncOperation<TPass> Operation<TPass>(TPass value)
		{
			return new AsyncOperation<TPass>(value);
		}

		public static IAsyncOperation Run(Action action)
		{
			return new AsyncOperation().Then(action);
		}

		public static IAsyncOperation<TPass> Run<TPass>(Func<TPass> action)
		{
			return new AsyncOperation().Then(action);
		}

		public static IAsyncOperation RunAsync(Func<Task> action)
		{
			return new AsyncOperation().ThenAsync(action);
		}

		public static IAsyncOperation<TPass> RunAsync<TPass>(Func<Task<TPass>> action)
		{
			return new AsyncOperation().ThenAsync(action);
		}

		public static IAsyncOperation Fail<TException>(TException ex) where TException : Exception
		{
			return new AsyncOperation(AsyncOperationResult.Failure(ex));
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
				var unhandledExceptions = new List<Exception>();
				foreach (var exception in _exceptions)
				{
					if (!_catcher.TryHandle(exception))
						unhandledExceptions.Add(exception);
				}
				_exceptions = new List<Exception>();

				if (unhandledExceptions.Count == 1)
					throw new AsyncException(unhandledExceptions[0]);
				else if (unhandledExceptions.Count >= 2)
					throw new AsyncException(new AggregateException(unhandledExceptions));

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
