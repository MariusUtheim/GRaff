using System;
using GRaff;
using GRaff.Synchronization;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace GRaff.UnitTesting
{
	[TestClass]
	public class AsyncTest
	{
		[TestMethod]
		public void Async_HandleEvents()
		{
			bool operationRan = false;
			var operation = Async.Run(() =>
			{
				operationRan = true;
			});

			Async.HandleEvents();
			Assert.IsTrue(operationRan);
			Assert.AreEqual(AsyncOperationState.Completed, operation.State);
		}

		
		[TestMethod]
		public void Async_Properties()
		{
			IAsyncOperation operation;

			operation = Async.Operation();
			Assert.AreEqual(AsyncOperationState.Completed, operation.State);

			operation = Async.Run(() => { });
			Assert.AreEqual(AsyncOperationState.Dispatched, operation.State);

			Async.HandleEvents();
			Assert.AreEqual(AsyncOperationState.Completed, operation.State);

			operation.Abort();
			Assert.AreEqual(AsyncOperationState.Aborted, operation.State);

			operation = Async.Run(() => { }).ThenQueue(() => { });
			Assert.AreEqual(AsyncOperationState.Initial, operation.State);

			operation = Async.Run(() =>  { throw new Exception("Error"); });
			Async.HandleEvents();
			Assert.AreEqual(AsyncOperationState.Failed, operation.State);
		}
		
		[TestMethod]
		public void Async_ThenSync()
		{
			IAsyncOperation operation;
			int count = 0;

			operation = Async.Operation();
			for (int i = 0; i < 10; i++)
				operation = operation.ThenQueue(() => { count++; });

			for (int i = 0; i < 10; i++)
			{
				Assert.AreEqual(i, count);
				Async.HandleEvents();
			}
		}

		[TestMethod]
		public void Async_ThenPasses()
		{
			IAsyncOperation<int> operation;

			operation = Async.Run(() => 0);
			Async.HandleEvents();

			for (int i = 0; i < 10; i++)
				operation = operation.ThenQueue(count => count + 1);

			for (int i = 0; i < 10; i++)
				Async.HandleEvents();

			//Assert.AreEqual(AsyncOperationState.Completed, operation.State);
			Assert.AreEqual(10, operation.Wait());
		}
		
		[TestMethod]
		public void Async_MultipleThen()
		{
			IAsyncOperation operation = Async.Operation();


			int count = 0;
			Func<int, Action> incrementBy = i => (() => count += i);
			for (int i = 0; i < 10; i++)
			{
				operation.ThenQueue(incrementBy(i));
				operation = operation.ThenQueue(incrementBy(i));
			}

			int expected = 0;
			for (int i = 0; i < 10; i++)
			{
				Assert.AreEqual(expected, count);

				Async.HandleEvents();
				expected += 2 * i;
			}
		}

		[TestMethod]
		public void Async_Then()
		{
			IAsyncOperation<int> operation = Async.Run(() => 0);
			Async.HandleEvents();

			for (int i = 0; i < 10; i++)
				operation = operation.ThenRun(count => Async.Run(() => count + 1));

			for (int i = 0; i < 10; i++)
				Async.HandleEvents();

			Assert.AreEqual(AsyncOperationState.Completed, operation.State);
			Assert.AreEqual(10, operation.Wait());
		}

		[TestMethod]
		public void Async_ThenWait()
		{
			int count = 0;
			Async.Run(() => { count += 1; }).ThenWait(() => { count += 1; }).Done();

			Assert.AreEqual(0, count);
			Async.HandleEvents();
			Assert.AreEqual(2, count);
		}

		//[TestMethod]
		//public void Async_Otherwise()
		//{
		//	bool thenPath = false, otherwisePath = false;
		//
		//	var operation = Async.Run(() => { throw new Exception(); });
		//	operation.ThenWait(() => thenPath = true);
		//	operation.Otherwise().ThenWait(() => otherwisePath = true);
		//
		//	Async.HandleEvents();
		//	Assert.IsFalse(thenPath);
		//	Assert.IsTrue(otherwisePath);
		//
		//	bool caughtException = thenPath = otherwisePath = false;
		//	operation = Async.Run(() => { throw new Exception(); });
		//	operation.ThenWait(() => thenPath = true);
		//	operation.Otherwise().ThenWait(() => otherwisePath = true);
		//	operation.Catch<Exception>(ex => caughtException = true);
		//
		//	Async.HandleEvents();
		//	Assert.IsTrue(thenPath);
		//	Assert.IsFalse(otherwisePath);
		//	Assert.IsTrue(caughtException);
		//}

		[TestMethod]
		public void Async_Wait()
		{
			IAsyncOperation operation = Async.Operation();
			int count = 0;
			for (int i = 0; i < 10; i++)
				operation = operation.ThenQueue(() => { count++; });
			
			operation.Wait();
			Assert.AreEqual(10, count);


			bool completed = false;
			operation = Async.Operation();

			Assert.IsFalse(completed);
			operation.ThenWait(() => completed = true);
			Assert.IsTrue(completed);

			completed = false;
			operation = Async.Run(() => { });
			operation.ThenWait(() => completed = true);
			Assert.IsFalse(completed);
			operation.Wait();
			Assert.IsTrue(completed);
		}


		[TestMethod]
		[ExpectedException(typeof(AsyncException))]
		public void Async_Done()
		{
			var operation = Async.Run(() => { throw new Exception("Error"); });

			Assert.IsFalse(operation.IsDone);
			operation.Done();
			Assert.IsTrue(operation.IsDone);
			Async.HandleEvents();

			try
			{
				Async.HandleEvents();
			}
			catch (AsyncException ex)
			{
				Assert.AreEqual(typeof(Exception), ex.InnerException.GetType());
				Assert.AreEqual("Error", ex.InnerException.Message);
				throw;
			}
		}

		[TestMethod]
		public void Async_Catch()
		{
			IAsyncOperation operation;
			bool caughtException;
			bool finished;

			// Exceptions get caught
			caughtException = finished = false;
			operation = Async
				.Run(() => { throw new Exception("Error"); })
				.Catch<Exception>(ex => { caughtException = true; })
				.ThenQueue(() => { finished = true; });
			Async.HandleEvents();
			Async.HandleEvents();
			Assert.IsTrue(caughtException);
			Assert.IsTrue(finished);

			// An early exception might skip some work before it gets caught.
			caughtException = finished = false;
			operation = Async
				.Run(() => { throw new Exception("Error"); })
				.ThenQueue(() => { finished = true; })
				.Catch<Exception>(ex => caughtException = true);
			Async.HandleEvents();
			Async.HandleEvents();
			Assert.IsTrue(caughtException);
			Assert.IsFalse(finished);

			// Subclasses of exceptions will be caught, superclasses will not.
			caughtException = finished = false;
			operation = Async
				.Run(() => { throw new ArithmeticException("Error"); })
				.Catch<DivideByZeroException>(ex => { caughtException = true; })
				.ThenQueue(() => { finished = true; })
				.Catch<Exception>(ex => { caughtException = true; });
			Async.HandleEvents();
			Async.HandleEvents();
			Assert.IsTrue(caughtException);
			Assert.IsFalse(finished);
		}

		//[TestMethod]
		//[ExpectedException(typeof(InvalidOperationException))]
		//public void Async_Deferred()
		//{
		//	var completed = false;
		//	var deferredVoid = new Deferred();
		//
		//	deferredVoid.Operation.ThenWait(() => completed = true);
		//
		//	Assert.IsFalse(completed);
		//	Assert.IsFalse(deferredVoid.IsResolved);
		//	deferredVoid.Accept();
		//	Assert.IsTrue(completed);
		//	Assert.IsTrue(deferredVoid.IsResolved);
		//
		//
		//	var deferredInt = new Deferred<int>();
		//	int result = 0;
		//	deferredInt.Operation.ThenWait(i => result = i);
		//
		//	Assert.AreEqual(0, result);
		//	Assert.IsFalse(deferredInt.IsResolved);
		//	deferredInt.Accept(10);
		//	Assert.AreEqual(10, result);
		//	Assert.IsTrue(deferredInt.IsResolved);
		//
		//
		//	deferredInt = new Deferred<int>();
		//	result = 0;
		//	deferredInt.Operation
		//		.Catch<ArithmeticException>(ex => -1)
		//		.ThenWait(i => { result = i; });
		//
		//	deferredInt.Reject(new DivideByZeroException());
		//	Assert.AreEqual(-1, result);
		//
		//
		//	deferredInt.Accept(0); // Throw InvalidOperationException
		//}

		[TestMethod]
		public void Async_All()
		{
			int completedOperations = 0;
			bool allCompleted = false;

			IAsyncOperation op1 = Async.Run(() => { completedOperations += 1; }), op2 = Async.Run(() => { completedOperations += 1; });

			IAsyncOperation all = Async.All(op1, op2);
			all.ThenWait(() => {
				allCompleted = true;
			});

			Assert.AreEqual(0, completedOperations);
			Assert.IsFalse(allCompleted);
			op1.Wait();
			Assert.AreEqual(1, completedOperations);
			Assert.IsFalse(allCompleted);
			op2.Wait();
			Assert.AreEqual(2, completedOperations);
			Assert.IsTrue(allCompleted);

			Async.HandleEvents();
			bool errorsWereHandled = false;
			all = Async.All(
				Async.Run(() => {
					throw new Exception("1");
				}),
				Async.Run(() => { }),
				Async.Run(() => { throw new Exception("3"); })
			).Catch<AggregateException>(ex => {
				Assert.AreEqual(3, ex.InnerExceptions.Count);
				Assert.AreEqual("1", ex.InnerExceptions[0].Message);
				Assert.AreEqual("", ex.InnerExceptions[1].Message);
				Assert.AreEqual("3", ex.InnerExceptions[2].Message);
				errorsWereHandled = true;
			});

			Async.HandleEvents();
			Async.HandleEvents();
			Assert.IsTrue(errorsWereHandled);
		}

		[TestMethod]
		public void Async_Any()
		{
			int completedIndex;

			completedIndex = -1;
			IAsyncOperation<int> any;
			any = Async.Any(
				Async.Run(() => { }).ThenQueue(() => { }),
				Async.Run(() => { })
			).ThenWait(index => completedIndex = index);

			Async.HandleEvents();
			Async.HandleEvents();
			Assert.AreEqual(1, completedIndex);


			completedIndex = -1;
			any = Async.Any(
				Async.Run(() => { throw new Exception("1"); }),
				Async.Run(() => { }),
				Async.Run(() => { throw new Exception("3"); })
			).ThenWait(index => completedIndex = index);

			Async.HandleEvents();
			Assert.AreEqual(1, completedIndex);


			completedIndex = -1;
			any = Async.Any(
				Async.Run(() => { throw new Exception("1"); }),
				Async.Run(() => { throw new Exception("2"); }),
				Async.Run(() => { throw new Exception("3"); })
			)
			.ThenWait(index => completedIndex = index)
			.Catch<AggregateException>(ex => {
				Assert.AreEqual(3, ex.InnerExceptions.Count);
				Assert.AreEqual("1", ex.InnerExceptions[0].Message);
				Assert.AreEqual("2", ex.InnerExceptions[1].Message);
				Assert.AreEqual("3", ex.InnerExceptions[2].Message);
				return 0;
			});

			Async.HandleEvents();
		}
	}
}
