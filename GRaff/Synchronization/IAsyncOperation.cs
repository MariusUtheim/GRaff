using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	public interface IAsyncOperation
	{
		AsyncOperationState State { get; }
		IAsyncOperation Then(Action action);
		IAsyncOperation<TNext> Then<TNext>(Func<TNext> action);
		IAsyncOperation Catch<TException>(Action<TException> handler);
		IAsyncOperation CatchThen<TException>(Action<TException> handler);
		void Abort();

		IAsyncOperation<TNext> ThenBranch<TNext>(Func<IEnumerable<TNext>> action);
	}

	public interface IAsyncOperation<out TPass>
	{
		IAsyncOperation Then(Action<TPass> action);
		IAsyncOperation<TNext> Then<TNext>(Func<TPass, TNext> action);
		IAsyncOperation<TPass> Catch<TException>(Action<TException> handler);
		IAsyncOperation<TPass> CatchThen<TException, TNext>(Func<TException, TNext> handler);
		void Abort();

		IAsyncOperation<TNext> ThenBranch<TNext>(Func<TPass, IEnumerable<TNext>> acion);

	}
}
