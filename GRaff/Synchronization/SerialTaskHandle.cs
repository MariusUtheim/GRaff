using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	internal class SerialTaskHandle<TPass> : IAsyncTaskHandle
	{
		private Action _action;
		private AsyncEventArgs _eventArgs;

		public SerialTaskHandle(Func<TPass> action, Action<TPass> completeAction, Action<Exception> exceptionHandler)
		{
			_action = delegate
			{
				try
				{
					var intermediateResult = action();
					completeAction(intermediateResult);
				}
				catch (Exception ex)
				{
					exceptionHandler(ex);
				}
			};

			_eventArgs = new AsyncEventArgs(_action);
			Async.Dispatch(_eventArgs);
		}


		public void Abort()
		{
			_eventArgs.Resolve();
		}

		public void Wait()
		{
			if (_eventArgs.Resolve())
			{
				_action.Invoke();
			}
			else
				throw new NotSupportedException("There is an unsupported case in GRaff.Synchronization.SynchronousTaskHandler");
		}
	}
}
