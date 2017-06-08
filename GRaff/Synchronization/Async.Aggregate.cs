using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	public static partial class Async
	{
		/// <summary>
		/// Creates a new GRaff.IAsyncOperation that will resolve when all the specified GRaff.IAsyncOperation objects have resolved.
		/// </summary>
		/// <param name="operations">An array of GRaff.IAsyncOperation objects.</param>
		/// <returns>A GRaff.IAsyncOperation that will resolve when all the specified GRaff.IAsyncOperation objects are resolved.</returns>
		/// <remarks>
		/// If all operations are accepted, the aggregated operation will be accepted. If any of the specified operations are rejected,
		/// the aggregated operation will be rejected by an AggregateException, containing an array of System.Exception objects, 
		///</remarks>
		public static IAsyncOperation All(params IAsyncOperation[] operations)
		{
			if (operations == null || operations.Length == 0)
				return Async.Operation();

			var deferred = new Deferred();
			var exceptions = new Exception[operations.Length];
			var remainingOperations = operations.Length;
			var errorsOccurred = false;
			
			for (int i = 0; i < operations.Length; i++)
			{
				int index = i;
				operations[i].Catch<Exception>(ex => {
					exceptions[index] = ex;
					errorsOccurred = true;
				}).ThenWait(() => {
					if (Interlocked.Decrement(ref remainingOperations) == 0)
					{
						if (errorsOccurred)
						{
							exceptions = exceptions.Select(ex => ex ?? new Exception(null)).ToArray();
							deferred.Reject(new AggregateException(exceptions));
						}
						else
							deferred.Accept();
					}
				});
			}

			return deferred.Operation;
		}

		/// <summary>
		/// Creates a new GRaff.IAsyncOperation`1 that will resolve when the first of the GRaff.IAsyncOperation objects resolves.
		/// The resolution value is the index of the operation that resolved in the array.
		/// </summary>
		/// <param name="operations">An array og GRaff.IAsyncOperation objects.</param>
		/// <returns>A GRaff.IAsyncOperation that will resolve when the first of the specified GRaff.IAsyncOperations is accepted.</returns>
		/// <remarks>
		/// If all operations are rejected, the aggregated operation will be rejected by an AggregateException, containing an array o
		/// System.Exception objects, where each exception corresponds respectively to each input operation.
		/// </remarks>
		public static IAsyncOperation<int> Any(params IAsyncOperation[] operations)
		{
			if (operations == null || operations.Length == 0)
				return Async.Capture(0);

			var deferred = new Deferred<int>();
			var triggeredOperation = 0;
			var remainingOperations = operations.Length;
			var exceptions = new Exception[operations.Length];

			for (int i = 0; i < operations.Length; i++)
			{
				int index = i;
				operations[i].ThenWait(() => {
					if (Interlocked.Exchange(ref triggeredOperation, 1) == 0)
						deferred.Accept(index);
				});

				operations[i].Otherwise().ThenWait(ex => {
					exceptions[index] = ex;
					if (Interlocked.Decrement(ref remainingOperations) == 0)
						deferred.Reject(new AggregateException(exceptions));
				});
			}

			return deferred.Operation;
		}

		public static IAsyncOperation Delay(double seconds)
		{
			return Async.RunAsync(async () => await Task.Delay(TimeSpan.FromSeconds(seconds)));
		}
	}
}
