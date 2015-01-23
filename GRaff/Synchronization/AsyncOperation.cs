using System;
using System.Diagnostics;

namespace GRaff.Synchronization
{
	public class AsyncOperation : AsyncOperationBase
	{
		private CatchContext _catchHandlers = new CatchContext();
		private object _input;

		public AsyncOperation()
		{
			_preceedingOperation = null;
			_result = AsyncOperationResult.Null;
			State = AsyncOperationState.Completed;
		}

		internal AsyncOperation(AsyncOperationBase preceeding, Action<object> action)
		{
			_preceedingOperation = preceeding;
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


		private void _execute(Action<object> action)
		{
			try
			{
				action(_input);
				_result = AsyncOperationResult.Success();
				State = AsyncOperationState.Completed;
				passToAll();
			}
			catch (Exception ex)
			{
				Throw(ex);
			}
		}

		public AsyncOperation Then(Action action)
		{
			var continuation = new AsyncOperation(this, obj => action());
			then(continuation);
			return continuation;
		}

		public AsyncOperation<TNext> Then<TNext>(Func<TNext> action)
		{
			var continuation = new AsyncOperation<TNext>(this, obj => action());
			then(continuation);
			return continuation;
		}


		public AsyncOperation Catch<TException>(Action<TException> exceptionHandler) where TException : Exception
		{
			assertState("add a catch handler to");
			_catchHandlers.Catch(exceptionHandler);
			return this;
		}

		public void Throw<TException>(TException exception) where TException : Exception
		{
			if (_catchHandlers.TryHandle(exception))
			{
				if (State == AsyncOperationState.Dispatched)
					_actionHandle.Resolve();
				State = AsyncOperationState.Completed;
				_result = AsyncOperationResult.Success();
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

		public void Wait()
		{
			wait();
		}
	}
}
