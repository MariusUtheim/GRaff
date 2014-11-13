using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	public class ParallelTaskHandle<TPass> : IAsyncTaskHandle
	{
		Task<TPass> theTask;
		CancellationTokenSource _cancellation;
		Action<Exception> _exceptionHandler;

		public ParallelTaskHandle(Func<Task<TPass>> action, Action<TPass> completeAction, Action<Exception> exceptionHandler)
		{
			_cancellation = new CancellationTokenSource();
			_exceptionHandler = exceptionHandler;
			Console.WriteLine("[ParallelTaskHandle] <{0}>", System.Threading.Thread.CurrentThread.Name);
			theTask = new Task<TPass>(delegate
			{
				try
				{
					return action().Result;
				}
				catch (AggregateException ex)
				{
					Exception targetException = ex.InnerExceptions.First();
					exceptionHandler(targetException);
					return default(TPass);
				}
				catch (Exception ex)
				{
					Console.WriteLine("=== INNER EXCEPTION ===");
					Console.WriteLine(ex.Message);
					throw;
				}
            }, _cancellation.Token);
			
			theTask.ContinueWith(async result => { completeAction(await result); }, TaskContinuationOptions.ExecuteSynchronously);
			theTask.Start(Async.TaskScheduler);
		}

		public void Abort()
		{
			try
			{
				_cancellation.Cancel();
				theTask.Wait();
			}
			catch (AggregateException ex)
			{
				Exception targetException = ex.InnerExceptions.First();
				_exceptionHandler(targetException);
			}
		}

		public void Wait()
		{
			theTask.Wait();
		}
	}
}
