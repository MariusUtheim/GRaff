using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	public class AsyncOrchestrator : IDisposable
	{
		private bool _isDisposed = false;
		internal LinkedList<IAsyncTaskFactory> _program;
		private List<AsyncWorker> _workers;
		internal ConcurrentQueue<AsyncWorker> _waitingWorkers = new ConcurrentQueue<AsyncWorker>();
		private AsyncCatchContext _catcher = new AsyncCatchContext();

		public int WorkerCount { get { return _workers.Count; } }

		internal AsyncOrchestrator()
		{
			_program = new LinkedList<IAsyncTaskFactory>();
			_workers = new List<AsyncWorker>();
			
			var root = new LinkedListNode<IAsyncTaskFactory>(null);
			_program.AddFirst(root);
			_waitingWorkers.Enqueue(new AsyncWorker(this, root));
		}

		public void Abort()
		{
			Dispose();
		}

		internal void RequestContinuation(AsyncWorker worker)
		{
			lock (this)
			{
				if (_isDisposed) return;

				if (worker.CurrentNode.Next != null)
					worker.Continue();
				else
					_waitingWorkers.Enqueue(worker);
			}
		}

		internal void Register(AsyncWorker worker)
		{
			lock (_workers)
				_workers.Add(worker);
		}

		internal void Remove(AsyncWorker worker)
		{
			lock (_workers)
				_workers.Remove(worker);
		}

		internal void Catch<TException>(Action<TException> catchHandler)
			where TException : Exception
		{
			_catcher.Catch(catchHandler);
		}

		internal void ThrowException(Exception exception)
		{
			lock (this)
			{
				if (_isDisposed) return;

				if (!_catcher.TryHandle(exception))	
					Async.ThrowException(exception);

				Dispose();
			}
		}

		internal void Wait()
		{
			while (_workers.Count > _waitingWorkers.Count)
			{
				AsyncWorker[] workerArray = _workers.ToArray();
				foreach (var worker in workerArray)
					worker.Wait();
			}
		}

		private void _notifyWorkers()
		{
			AsyncWorker waitingWorker;
			while (_waitingWorkers.TryDequeue(out waitingWorker))
				waitingWorker.Continue();
		}

		private void _then(IAsyncTaskFactory nextTask)
		{
			lock (this)
			{
				if (_isDisposed) throw new ObjectDisposedException("AsyncOrchestrator");
				_program.AddLast(nextTask);
				if (_isDisposed) throw new ObjectDisposedException("AsyncOrchestrator");
				if (_waitingWorkers == null) throw new Exception();
				_notifyWorkers();
			}
		}

		/*C#6.0*/
		internal void Then<TIn, TOut>(Func<TIn, TOut> action)
		{
			_then(new SingleSerialFactory<TIn, TOut>(action));
		}

		internal void ThenAsync<TIn, TOut>(Func<TIn, Task<TOut>> action)
		{
			_then(new SingleParallelFactory<TIn, TOut>(action));
		}

		internal void ThenBranch<TIn, TOut>(Func<TIn, IEnumerable<TOut>> branchingAction)
		{
			_then(new BranchSerialFactory<TIn, TOut>(branchingAction));
		}

		internal void ThenBranchAsync<TIn, TOut>(Func<TIn, Task<IEnumerable<TOut>>> branchingAction)
		{
			_then(new BranchParallelFactory<TIn, TOut>(branchingAction));
		}

		internal void ThenMerge<TIn, TOut>(Func<IEnumerable<TIn>, TOut> mergeAction)
		{
			_then(new MergeSerialFactory<TIn, TOut>(mergeAction));
		}

		internal void ThenMergeAsync<TIn, TOut>(Func<IEnumerable<TIn>, Task<TOut>> mergeAction)
		{
			_then(new MergeParallelFactory<TIn, TOut>(mergeAction));
		}

#region Dispose handler
		~AsyncOrchestrator()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			lock (this)
			{
				if (!_isDisposed)
				{
					_isDisposed = true;

					foreach (var worker in _workers.ToArray())
						worker.Dispose();
					_catcher = null;
					_workers = null;
					_waitingWorkers = null;
				}
			}

		}
#endregion
	}

}
