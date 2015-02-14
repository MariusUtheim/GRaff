using System;
using System.Threading;

namespace GRaff.Synchronization
{
	public class Deferred
	{
		private int _isResolved = 0;
		private AsyncOperation _operation;

		public Deferred()
		{
			_operation = new AsyncOperation(true);
		}

		public IAsyncOperation Operation { get { return _operation; } }

		public bool IsResolved { get { return _isResolved == 1; } }

		public void Accept()
		{
			if (Interlocked.Exchange(ref _isResolved, 1) == 0)
				_operation.Accept(null);
			else
				throw new InvalidOperationException("The GRaff.Synchronization.Deferred object has already been resolved.");
		}

		public void Reject(Exception reason)
		{
			if (Interlocked.Exchange(ref _isResolved, 1) == 0)
				_operation.Reject(reason);
			else
				throw new InvalidOperationException("The GRaff.Synchronization.Deferred object has already been resolved.");
		}
	}

	public class Deferred<TPass>
	{
		private int _isResolved = 0;
		private AsyncOperation<TPass> _operation;

		public Deferred()
		{
			_operation = new AsyncOperation<TPass>();
		}

		public IAsyncOperation<TPass> Operation { get { return _operation; } }

		public bool IsResolved { get { return _isResolved == 1; } }

		public void Accept(TPass result)
		{
			if (Interlocked.Exchange(ref _isResolved, 1) == 0)
				_operation.Accept(result);
			else
				throw new InvalidOperationException("The GRaff.Synchronization.Deferred object has already been resolved.");
		}

		public void Reject(Exception reason)
		{
			if (Interlocked.Exchange(ref _isResolved, 1) == 0)
				_operation.Reject(reason);
			else
				throw new InvalidOperationException("The GRaff.Synchronization.Deferred object has already been resolved.");
		}
	}
}
