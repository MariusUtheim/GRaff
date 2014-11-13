using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	internal class MergeSerialFactory<TIn, TOut> : IAsyncTaskFactory<IEnumerable<TIn>, TOut>
	{
		private Func<IEnumerable<TIn>, TOut> _mergeAction;

		public MergeSerialFactory(Func<IEnumerable<TIn>, TOut> mergeAction)
		{
			this._mergeAction = mergeAction;
		}

		public IAsyncTaskHandle Invoke(AsyncWorker sender, object parameter)
		{
			IEnumerable<TIn> parameters;
			if (sender.Merge<TIn>((TIn)parameter, out parameters))
				return Invoke(sender, parameters);
			else
				return null;
		}

		public IAsyncTaskHandle Invoke(AsyncWorker sender, IEnumerable<TIn> parameter)
		{
			return new SerialTaskHandle<TOut>(() => _mergeAction(parameter), sender.Pass<TOut>, sender.ThrowException);
		}
	}

	internal class MergeParallelFactory<TIn, TOut> : IAsyncTaskFactory<IEnumerable<TIn>, TOut>
	{
		private Func<IEnumerable<TIn>, Task<TOut>> _mergeAction;

		public MergeParallelFactory(Func<IEnumerable<TIn>, Task<TOut>> mergeAction)
		{
			this._mergeAction = mergeAction;
		}

		public IAsyncTaskHandle Invoke(AsyncWorker sender, object parameter)
		{
			IEnumerable<TIn> parameters;
			if (sender.Merge<TIn>((TIn)parameter, out parameters))
				return Invoke(sender, parameters);
			else
				return null;
		}

		public IAsyncTaskHandle Invoke(AsyncWorker sender, IEnumerable<TIn> parameter)
		{
			return new ParallelTaskHandle<TOut>(() => _mergeAction(parameter), sender.Pass<TOut>, sender.ThrowException);
		}
	}
}
