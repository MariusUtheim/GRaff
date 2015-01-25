using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	public interface IAsyncOperationBase
	{
		AsyncOperationState State { get; }
		void Abort();
		void Done();
		bool IsDone { get; }
	}

	/// <summary>
 /// Represents an asynchronous operation that will not return a value.
 /// </summary>
	public interface IAsyncOperation : IAsyncOperationBase
	{
		IAsyncOperation Then(Action action);
		IAsyncOperation<TNext> Then<TNext>(Func<TNext> action);
		IAsyncOperation ThenAsync(Func<Task> action);
		IAsyncOperation<TNext> ThenAsync<TNext>(Func<Task<TNext>> action);
		IAsyncOperation Catch<TException>(Action<TException> handler) where TException : Exception;
		void Wait();
	}
	
	/// <summary>
 /// Represents an asynchronous operation that will return the specified type.
 /// </summary>
	public interface IAsyncOperation<TPass> : IAsyncOperationBase
	{
		IAsyncOperation Then(Action<TPass> action);
		IAsyncOperation<TNext> Then<TNext>(Func<TPass, TNext> action);
		IAsyncOperation<TPass> Catch<TException>(Func<TException, TPass> handler) where TException : Exception;
		TPass Wait();
	//	IAsyncOperation Otherwise<TException>(Action<TException, TPass> handler) where TException : Exception;
	//	IAsyncOperation<TNext> Otherwise<TException, TNext>(Func<TException, TPass, TNext> handler) where TException : Exception;
	}
}
