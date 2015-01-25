using System;
using GRaff;
using GRaff.Synchronization;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace GameMaker.UnitTesting
{
	[TestClass]
	public class AsyncTest
	{
		
		[TestMethod]
		public void Async_Properties()
		{
			IAsyncOperation operation;

			operation = new AsyncOperation();
			Assert.AreEqual(AsyncOperationState.Completed, operation.State);

			operation = Async.Run(() => { });
			Assert.AreEqual(AsyncOperationState.Dispatched, operation.State);

			Async.HandleEvents();
			Assert.AreEqual(AsyncOperationState.Completed, operation.State);

			operation.Abort();
			Assert.AreEqual(AsyncOperationState.Aborted, operation.State);

			operation = Async.Run(() => { }).Then(() => { });
			Assert.AreEqual(AsyncOperationState.Initial, operation.State);

			operation = Async.Run(() =>  { throw new Exception("Error"); });
			Async.HandleEvents();
			Assert.AreEqual(AsyncOperationState.Failed, operation.State);
		}
		
		[TestMethod]
		public void Async_Then()
		{
			IAsyncOperation operation;
			int count = 0;

			operation = new AsyncOperation();
			for (int i = 0; i < 10; i++)
				operation = operation.Then(() => { count++; });

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
				operation = operation.Then(count => count + 1);

			for (int i = 0; i < 10; i++)
				Async.HandleEvents();

			Assert.AreEqual(AsyncOperationState.Completed, operation.State);
			Assert.AreEqual(10, operation.Wait());
		}
		
		[TestMethod]
		public void Async_MultipleThen()
		{
			IAsyncOperation operation = new AsyncOperation();


			int count = 0;
			Func<int, Action> incrementBy = i => (() => count += i);
			for (int i = 0; i < 10; i++)
			{
				operation.Then(incrementBy(i));
				operation = operation.Then(incrementBy(i));
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
		public void Async_Wait()
		{
			IAsyncOperation operation = new AsyncOperation();

			int count = 0;
			for (int i = 0; i < 10; i++)
				operation = operation.Then(() => { count++; });
			
			operation.Wait();
			Assert.AreEqual(10, count);
		}


		[TestMethod]
		[ExpectedException(typeof(AsyncException))]
		public void Async_Done()
		{
			IAsyncOperation operation = Async.Run(() => { throw new Exception("Error"); });

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


			caughtException = finished = false;
			operation = Async
				.Run(() => { throw new Exception("Error"); })
				.Catch<Exception>(exception => { caughtException = true; })
				.Then(() => { finished = true; });
			Async.HandleEvents();
			Async.HandleEvents();
			Assert.IsTrue(caughtException);
			Assert.IsTrue(finished);


			caughtException = finished = false;
			operation = Async
				.Run(() => { throw new Exception("Error"); })
				.Catch<Exception>(exception =>
				{
					caughtException = true;
				})
				.Then(() => { finished = true; });
			Async.HandleEvents();
			Async.HandleEvents();
			Assert.IsTrue(caughtException);
			Assert.IsTrue(finished);


			caughtException = finished = false;
			operation = Async
				.Run(() => { throw new Exception("Error"); })
				.Then(() => { finished = true; })
				.Catch<Exception>(ex => caughtException = true);
			Async.HandleEvents();
			Async.HandleEvents();
			Assert.IsTrue(caughtException);
			Assert.IsFalse(finished);
		}
	}
}
