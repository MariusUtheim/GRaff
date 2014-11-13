using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	internal class BranchSerialFactory<TIn, TOut> : IAsyncTaskFactory<TIn, IEnumerable<TOut>>
	{
		private Func<TIn, IEnumerable<TOut>> _branchingAction;

		public BranchSerialFactory(Func<TIn, IEnumerable<TOut>> branchingAction)
		{
			this._branchingAction = branchingAction;
		}

		public IAsyncTaskHandle Invoke(AsyncWorker sender, object parameter)
		{
			return Invoke(sender, (TIn)parameter);
		}

		public IAsyncTaskHandle Invoke(AsyncWorker sender, TIn parameter)
		{
			return new SerialTaskHandle<IEnumerable<TOut>>(() => _branchingAction(parameter), results => sender.Branch<TOut>(results, sender), sender.ThrowException);
		}
	}

	internal class BranchParallelFactory<TIn, TOut> : IAsyncTaskFactory<TIn, IEnumerable<TOut>>
	{
		private Func<TIn, Task<IEnumerable<TOut>>> _branchingAction;

		public BranchParallelFactory(Func<TIn, Task<IEnumerable<TOut>>> branchingAction)
		{
			this._branchingAction = branchingAction;
		}

		public IAsyncTaskHandle Invoke(AsyncWorker sender, object parameter)
		{
			return Invoke(sender, (TIn)parameter);
		}

		public IAsyncTaskHandle Invoke(AsyncWorker sender, TIn parameter)
		{
			return new ParallelTaskHandle<IEnumerable<TOut>>(() => _branchingAction(parameter), results => sender.Branch<TOut>(results, sender), sender.ThrowException);
		}
	}
}
