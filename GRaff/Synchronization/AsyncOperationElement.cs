using System;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	public abstract class AsyncOperationElement : GameElement, IAsyncOperation
	{
		public bool IsDone { get; private set; }

		public AsyncOperationState State
		{
			get; protected set;
		}

		public event EventHandler Accepted;
		public event EventHandler<AsyncOperationRejectedEventArgs> Rejected;

		public abstract void Abort();

		public IAsyncOperation Catch<TException>(Action<TException> handler) where TException : Exception
		{
			throw new NotImplementedException();
		}

		public void Dispatch(object value)
		{
			throw new NotImplementedException();
		}

		public void Done()
		{
			throw new NotImplementedException();
		}

		public IAsyncOperation<Exception> Otherwise()
		{
			throw new NotImplementedException();
		}

		public IAsyncOperation ThenQueue(Action action)
		{
			throw new NotImplementedException();
		}

		public IAsyncOperation<TNext> ThenQueue<TNext>(Func<TNext> action)
		{
			throw new NotImplementedException();
		}

		public IAsyncOperation ThenAsync(Func<Task> action)
		{
			throw new NotImplementedException();
		}

		public IAsyncOperation<TNext> ThenAsync<TNext>(Func<Task<TNext>> action)
		{
			throw new NotImplementedException();
		}

		public IAsyncOperation ThenRun(Func<IAsyncOperation> action)
		{
			throw new NotImplementedException();
		}

		public IAsyncOperation<TNext> ThenRun<TNext>(Func<IAsyncOperation<TNext>> action)
		{
			throw new NotImplementedException();
		}

		public IAsyncOperation ThenWait(Action action)
		{
			throw new NotImplementedException();
		}

		public IAsyncOperation<TNext> ThenWait<TNext>(Func<TNext> action)
		{
			throw new NotImplementedException();
		}



		public abstract void Wait();

		public IAsyncOperation ThenParallel(Action action)
		{
			throw new NotImplementedException();
		}

		public IAsyncOperation<TNext> ThenParallel<TNext>(Func<TNext> action)
		{
			throw new NotImplementedException();
		}
	}
}