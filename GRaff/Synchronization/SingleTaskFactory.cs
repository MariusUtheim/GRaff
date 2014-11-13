using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	internal class SingleSerialFactory<TIn, TOut> : IAsyncTaskFactory<TIn, TOut>
	{
		private Func<TIn, TOut> _syncOperation;

		public SingleSerialFactory(Func<TIn, TOut> action)
		{
			this._syncOperation = action;
		}

		public IAsyncTaskHandle Invoke(AsyncWorker sender, object parameter)
		{
			return Invoke(sender, (TIn)parameter);
		}

		public IAsyncTaskHandle Invoke(AsyncWorker sender, TIn parameter)
		{
			return new SerialTaskHandle<TOut>(() => _syncOperation(parameter), sender.Pass<TOut>, sender.ThrowException);
		}
	}

	internal class SingleParallelFactory<TIn, TOut> : IAsyncTaskFactory<TIn, TOut>
	{
		private Func<TIn, Task<TOut>> _syncOperation;

		public SingleParallelFactory(Func<TIn, Task<TOut>> action)
		{
			this._syncOperation = action;
		}

		public IAsyncTaskHandle Invoke(AsyncWorker sender, object parameter)
		{
			return Invoke(sender, (TIn)parameter);
		}

		public IAsyncTaskHandle Invoke(AsyncWorker sender, TIn parameter)
		{
			return new ParallelTaskHandle<TOut>(() => _syncOperation(parameter), sender.Pass<TOut>, sender.ThrowException);
		}
	}

	/*
	internal class SingleParallelFactory : IAsyncTaskFactory
	{
		private Func<object, Task<object>> _syncOperation;

		public SingleParallelFactory(Func<object, Task<object>> action)
		{
			this._syncOperation = action;
		}

		public IAsyncTaskHandle Invoke(AsyncWorker sender, object parameter)
		{
			return new ParallelTaskHandle(
				action: async () => await _syncOperation(parameter),
				completeAction: sender.Pass,
				exceptionHandler: sender.ThrowException
				);
		}
	}
	*/
}
