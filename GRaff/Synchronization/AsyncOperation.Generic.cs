using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
    public class AsyncOperation<TPass> : AsyncOperationBase
    {
		private CatchContext<TPass> _catchHandlers = new CatchContext<TPass>();
		private object _input;


		internal AsyncOperation(AsyncOperationBase sender, Func<object, TPass> action)
		{
			_preceedingOperation = sender;
			_actionHandle = new AsyncEventArgs(() => _execute(action));
		}

		internal override void Dispatch(AsyncOperationResult result)
		{
			if (result.IsSuccessful)
			{
				State = AsyncOperationState.Dispatched;
				_input = result.Value;
				Async.Dispatch(_actionHandle);
			}
			else
			{
				Throw((Exception)result.Value);
			}
		}

		private void _execute(Func<object, TPass> action)
		{
			try
			{
				_result = AsyncOperationResult.Success(action(_input));
				State = AsyncOperationState.Completed;
				passToAll();
			}
			catch (Exception ex)
			{
				Throw(ex);
			}
		}

		public AsyncOperation Then(Action<TPass> action)
		{
			var continuation = new AsyncOperation(this, obj => action((TPass)obj));
			then(continuation);
			return continuation;
		}

		public AsyncOperation<TNext> Then<TNext>(Func<TPass, TNext> action)
		{
			var continuation = new AsyncOperation<TNext>(this, obj => action((TPass)obj));
			then(continuation);
			return continuation;
		}

		public AsyncOperation<TPass> Catch<TException>(Func<TException, TPass> exceptionHandler) where TException : Exception
		{
			assertState("add a catch handler to");
			_catchHandlers.Catch(exceptionHandler);
			return this;
		}

		public void Throw<TException>(TException exception) where TException : Exception
		{
			TPass result;
			if (_catchHandlers.TryHandle(exception, out result))
			{
				if (State == AsyncOperationState.Dispatched)
					_actionHandle.Resolve();
				State = AsyncOperationState.Completed;
				_result = AsyncOperationResult.Success(result);
				passToAll();
			}
			else
			{
				if (IsDone)
					throw new AsyncException(exception);

				State = AsyncOperationState.Failed;
				_result = AsyncOperationResult.Failure(exception);
				passToAll();
			}
		}

		public TPass Wait()
		{
			wait();
			return (TPass)_result.Value;
		}
	}
}
