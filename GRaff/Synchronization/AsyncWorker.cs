using System;
using System.Collections.Generic;
using System.Linq;

namespace GRaff.Synchronization
{
	internal class AsyncWorker : IDisposable 
	{
		internal LinkedListNode<IAsyncTaskFactory> CurrentNode;
		private AsyncOrchestrator _orchestrator;
		private IAsyncTaskHandle _taskHandle;
		internal object _intermediateResult;
		private bool _isDisposed = false;
		private MergeOrganizer _mergeOrganizer;

		public AsyncWorker(AsyncOrchestrator orchestrator, LinkedListNode<IAsyncTaskFactory> root)
		{
			orchestrator.Register(this);
			this._orchestrator = orchestrator;
			this.CurrentNode = root;
		}

		public AsyncWorker(AsyncOrchestrator orchestrator, LinkedListNode<IAsyncTaskFactory> root, MergeOrganizer organizer)
			: this(orchestrator, root)
		{
			_mergeOrganizer = organizer;
		}

		public void Continue()
		{
			CurrentNode = CurrentNode.Next;
			_taskHandle = CurrentNode.Value.Invoke(this, _intermediateResult);
		}

		public void Pass<TPass>(TPass passResult)
		{
			_taskHandle = null;
			_intermediateResult = passResult;
			_orchestrator.RequestContinuation(this);
		}

		public void Branch<TPass>(IEnumerable<TPass> results, AsyncWorker source)
		{
			MergeOrganizer organizer = new MergeOrganizer(source._mergeOrganizer, results.Count());

			foreach (var result in results)
			{
				var workerBranch = new AsyncWorker(_orchestrator, CurrentNode, organizer);
				workerBranch.Pass(result);
			}

			Dispose();
		}

		public bool Merge<TPass>(TPass partialResult, out IEnumerable<TPass> merge)
		{
			_mergeOrganizer.Merge(partialResult);
			if (_mergeOrganizer.IsComplete)
			{
				merge = _mergeOrganizer.Result<TPass>();
				return true;
			}
			else
			{
				merge = null;
				Dispose();
				return false;
			}
		}

		public object Wait()
		{
			if (_taskHandle != null)
				_taskHandle.Wait();
			return _intermediateResult;
		}

		public void ThrowException(Exception ex)
		{
			if (!_isDisposed)
				_orchestrator.ThrowException(ex);
		}

		#region System.IDisposable implementation
		~AsyncWorker()
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
			if (!_isDisposed)
			{
				_isDisposed = true;

				if (_taskHandle != null) _taskHandle.Abort(); /*C#6.0*/
				if (_orchestrator != null) _orchestrator.Remove(this);

				if (disposing)
				{
					_taskHandle = null;
					_orchestrator = null;
					CurrentNode = null;
					_intermediateResult = null;
				}
			}
		}
		#endregion
	}
}
