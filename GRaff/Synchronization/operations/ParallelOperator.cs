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
		private Task<AsyncOperationResult> _task;
		private readonly Func<object, Task<object>> _action;
		private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();
		private int _awaitsResolution = 1;

		public ParallelOperator(Func<object, Task<object>> action)
		{
			_action = action;
		}

		public void Cancel()
		{
			_cancellation.Cancel();
		}

		public void Dispatch(object arg, Action<AsyncOperationResult> callback)
		{
			_task = Task.Run(async () => {
				try
				{
					return AsyncOperationResult.Success(await _action(arg));
				}
				catch (Exception ex)
				{
					return AsyncOperationResult.Failure(ex);
				}
			}, _cancellation.Token);

			_task.ContinueWith(task => {
				if (Interlocked.Exchange(ref _awaitsResolution, 0) == 1)
					callback(task.Result);
			});
        }

		public AsyncOperationResult DispatchSynchronously(object arg)
		{
			if (Interlocked.Exchange(ref _awaitsResolution, 0) == 1)
			{
				if (_task == null)
				{
					try
					{
						return AsyncOperationResult.Success(_action(arg).Result);
					}
					catch (Exception ex)
					{
						return AsyncOperationResult.Failure(ex);
					}
				}
				else
					return _task.Result;
			}
			else
				throw new NotImplementedException();
        }
	}
}
