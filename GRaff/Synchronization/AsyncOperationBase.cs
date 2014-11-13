using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	public abstract class AsyncOperationBase
	{
		protected AsyncOrchestrator _orchestrator;

		internal AsyncOperationBase(AsyncOrchestrator underlyingHandler)
		{
			_orchestrator = underlyingHandler;
		}

		public bool IsHandled { get; private set; } = false;

		public void Wait()
		{
			IsHandled = true;
			_orchestrator.Wait();
		}

		public void Abort()
		{
			IsHandled = true;
			_orchestrator.Abort();
		}

		protected AsyncOperation _pass()
		{
			if (IsHandled)
				throw new InvalidOperationException("Trying to add a new continuation action to a GRaff.Synchronization.AsyncOperation that already has a continuation.");

			IsHandled = true;
			return new AsyncOperation(_orchestrator);
		}

		protected AsyncOperation<TPass> _pass<TPass>()
		{
			if (IsHandled)
				throw new InvalidOperationException("Trying to add a new continuation action to a GRaff.Synchronization.AsyncOperation that already has a continuation.");

			IsHandled = true;
			return new AsyncOperation<TPass>(_orchestrator);
		}

		public AsyncOperation Catch<TException>(Action<TException> catchHandler)
			where TException : Exception
		{
			_orchestrator.Catch(catchHandler);
			return _pass();
		}
	}
}
