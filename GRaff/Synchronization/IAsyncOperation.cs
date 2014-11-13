using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	public interface IAsyncOperation
	{
		IAsyncOperation Then(Action action);
		IAsyncOperation ThenAsync(Func<Task> action);
		IAsyncOperation<TPass> Then<TPass>(Func<TPass> action);
		IAsyncOperation<TPass> ThenAsync<TPass>(Func<Task<TPass>> action);
		IAsyncOperation<TPass> ThenBranch<TPass>(Func<IEnumerable<TPass>> action);
		IAsyncOperation<TPass> ThenBranchAsync<TPass>(Func<IEnumerable<Task<TPass>>> action);
		IAsyncOperation ThenMerge(Action action);
		IAsyncOperation ThenMergeAsync(Func<Task> action);
		IAsyncOperation<TPass> ThenMerge<TPass>(Func<TPass> action);
		IAsyncOperation<TPass> ThenMergeAsync<TPass>(Func<Task<TPass>> action);
	}

	public interface IAsyncOperation<TInput>
	{
		IAsyncOperation Then(Action<TInput> action);
		IAsyncOperation ThenAsync(Func<TInput, Task> action);
		IAsyncOperation<TPass> Then<TPass>(Func<TInput, TPass> action);
		IAsyncOperation<TPass> ThenAsync<TPass>(Func<TInput, Task<TPass>> action);
		IAsyncOperation<TPass> ThenBranch<TPass>(Func<TInput, IEnumerable<TPass>> action);
		IAsyncOperation<TPass> ThenBranchAsync<TPass>(Func<TInput, IEnumerable<Task<TPass>>> action);
		IAsyncOperation ThenMerge(Action<IEnumerable<TInput>> action);
		IAsyncOperation ThenMergeAsync(Func<IEnumerable<TInput>, Task> action);
		IAsyncOperation<TPass> ThenMerge<TPass>(Func<IEnumerable<TInput>, TPass> action);
		IAsyncOperation<TPass> ThenMergeAsync<TPass>(Func<IEnumerable<TInput>, Task<TPass>> action);
	}
}
