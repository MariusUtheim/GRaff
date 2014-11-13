using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	public class AsyncOperation : AsyncOperationBase
	{
		public AsyncOperation()
			: base(new AsyncOrchestrator())
		{ }

		internal AsyncOperation(AsyncOrchestrator handler)
			: base(handler) { }

		public AsyncOperation Then(Action action)
		{
			_orchestrator.Then<object, object>(obj => { action(); return null; });
			return _pass();
		}

		public AsyncOperation ThenAsync(Func<Task> action)
		{
			_orchestrator.ThenAsync<object, object>(async obj => { await action(); return null; });
			return _pass();
		}

		public AsyncOperation<TPass> Then<TPass>(Func<TPass> action)
		{
			_orchestrator.Then<object, TPass>(obj => action());
			return _pass<TPass>();
		}

		public AsyncOperation<TPass> ThenAsync<TPass>(Func<Task<TPass>> action)
		{
			_orchestrator.ThenAsync<object, TPass>(async obj => await action());
			return _pass<TPass>();
		}

		public AsyncOperation<TPass> ThenBranch<TPass>(Func<IEnumerable<TPass>> action)
		{
			_orchestrator.ThenBranch<object, TPass>(obj => action());
			return _pass<TPass>();
		}

		public AsyncOperation<TPass> ThenBranchAsync<TPass>(Func<Task<IEnumerable<TPass>>> action)
		{
			_orchestrator.ThenBranchAsync<object, TPass>(async obj => await action());
			return _pass<TPass>();
		}

		public AsyncOperation ThenMerge(Action action)
		{
			_orchestrator.ThenMerge<object, object>(objects => { action(); return null; });
			return _pass();
		}

		public AsyncOperation ThenMergeAsync(Func<Task> action)
		{
			_orchestrator.ThenMerge<object, object>(objects => action());
			return _pass();
		}

		public AsyncOperation<TPass> ThenMerge<TPass>(Func<TPass> action)
		{
			_orchestrator.ThenMerge<object, TPass>(objects => action());
			return _pass<TPass>();
		}

		public AsyncOperation<TPass> ThenMergeAsync<TPass>(Func<Task<TPass>> action)
		{
			_orchestrator.ThenMergeAsync<object, TPass>(async objects => await action());
			return _pass<TPass>();
		}

	}


	public class AsyncOperation<TInput> : AsyncOperationBase
	{
		internal AsyncOperation(AsyncOrchestrator handler)
			: base(handler)
		{
		}

		public AsyncOperation Then(Action<TInput> action)
		{
			_orchestrator.Then<TInput, object>(obj => { action((TInput)obj); return null; });
			return _pass();
		}

		public AsyncOperation ThenAsync(Func<TInput, Task> action)
		{
			_orchestrator.ThenAsync<TInput, object>(async obj => { await action((TInput)obj); return null; });
			return _pass();
		}

		public AsyncOperation<TPass> Then<TPass>(Func<TInput, TPass> action)
		{
			_orchestrator.Then<TInput, TPass>(obj => action((TInput)obj));
			return _pass<TPass>();
		}

		public AsyncOperation<TPass> ThenAsync<TPass>(Func<TInput, Task<TPass>> action)
		{
			_orchestrator.ThenAsync<TInput, TPass>(async obj => await action((TInput)obj));
			return _pass<TPass>();
		}

		public AsyncOperation<TPass> ThenBranch<TPass>(Func<TInput, IEnumerable<TPass>> action)
		{
			_orchestrator.ThenBranch<TInput, TPass>(obj => action(obj));
			return _pass<TPass>();
		}

		public AsyncOperation<TPass> ThenBranchAsync<TPass>(Func<TInput, Task<IEnumerable<TPass>>> action)
		{
			_orchestrator.ThenBranchAsync<TInput, TPass>(async obj => await action(obj));
			return _pass<TPass>();
		}

		public AsyncOperation ThenMerge(Action<IEnumerable<TInput>> action)
		{
			_orchestrator.ThenMerge<TInput, object>(objects => { action(objects.Cast<TInput>()); return null; }); /*C#6.0*/
			return _pass();
		}

		public AsyncOperation ThenMergeAsync(Func<IEnumerable<TInput>, Task> action)
		{
			_orchestrator.ThenMergeAsync<TInput, object>(async objects => { await action(objects); return null; });
			return _pass();
		}
		
		public AsyncOperation<TPass> ThenMerge<TPass>(Func<IEnumerable<TInput>, TPass> action)
		{
			_orchestrator.ThenMerge<TInput, TPass>(objects => action(objects));
			return _pass<TPass>();
		}

		public AsyncOperation<TPass> ThenMergeAsync<TPass>(Func<IEnumerable<TInput>, Task<TPass>> action)
		{
			_orchestrator.ThenMergeAsync<TInput, TPass>(async objects => await action(objects));
			return _pass<TPass>();
		}
	}

}