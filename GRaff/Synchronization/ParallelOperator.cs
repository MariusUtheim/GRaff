using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	internal class ParallelOperator : IAsyncOperator
	{
		private Thread _thread;
		private readonly Func<object, object> _action;
		private int _awaitsResolution = 1;
		private AsyncOperationResult _result;

		public ParallelOperator(Func<object, object> action)
		{
			_action = action;
		}

		public void Cancel()
		{
			if (Interlocked.Exchange(ref _awaitsResolution, 0) == 1)
				_thread.Abort();
		}

		public void Dispatch(object arg, Action<AsyncOperationResult> callback)
		{
			var threadStart = new ThreadStart(() =>
			{
				try
				{
					var result = AsyncOperationResult.Success(_action(arg));
					if (Interlocked.Exchange(ref _awaitsResolution, 0) == 1)
						callback(result);
				}
				catch (Exception ex)
				{
					if (Interlocked.Exchange(ref _awaitsResolution, 0) == 1)
						callback(AsyncOperationResult.Failure(ex));
				}
			});

			_thread = new Thread(threadStart);
			_thread.Start();
		}

		public AsyncOperationResult DispatchSynchronously(object arg)
		{
			if (Interlocked.Exchange(ref _awaitsResolution, 0) == 1)
			{
				try
				{
					return _result = AsyncOperationResult.Success(_action(arg));
				}
				catch (Exception ex)
				{
					return _result = AsyncOperationResult.Failure(ex);
				}
			}
			else
				return _result;
		}
	}
}
