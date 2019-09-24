using System;
using GRaff;
using GRaff.Synchronization;
using Xunit;


namespace GRaff.UnitTesting
{
	public class AsyncTest
	{
		[Fact]
		public void Async_HandleEvents()
		{
			bool operationRan = false;
			var operation = Async.Run(() =>
			{
				operationRan = true;
			});

			Async.HandleEvents();
			Assert.True(operationRan);
			Assert.Equal(AsyncOperationState.Completed, operation.State);
		}

		
		[Fact]
		public void Async_Properties()
		{
			IAsyncOperation operation;

			operation = Async.Operation();
			Assert.Equal(AsyncOperationState.Completed, operation.State);

			operation = Async.Run(() => { });
			Assert.Equal(AsyncOperationState.Dispatched, operation.State);

			Async.HandleEvents();
			Assert.Equal(AsyncOperationState.Completed, operation.State);

			operation.Abort();
			Assert.Equal(AsyncOperationState.Aborted, operation.State);

			operation = Async.Run(() => { }).ThenQueue(() => { });
			Assert.Equal(AsyncOperationState.Initial, operation.State);

			operation = Async.Run(() =>  { throw new Exception("Error"); });
			Async.HandleEvents();
			Assert.Equal(AsyncOperationState.Failed, operation.State);
		}
		
		[Fact]
		public void Async_ThenSync()
		{
			IAsyncOperation operation;
			int count = 0;

			operation = Async.Operation();
			for (int i = 0; i < 10; i++)
				operation = operation.ThenQueue(() => { count++; });

			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(i, count);
				Async.HandleEvents();
			}
		}

		[Fact]
		public void Async_ThenPasses()
		{
			IAsyncOperation<int> operation;

			operation = Async.Run(() => 0);
			Async.HandleEvents();

			for (int i = 0; i < 10; i++)
				operation = operation.ThenQueue(count => count + 1);

			for (int i = 0; i < 10; i++)
				Async.HandleEvents();

			//Assert.Equal(AsyncOperationState.Completed, operation.State);
			Assert.Equal(10, operation.Wait());
		}
		
		[Fact]
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
				Assert.Equal(expected, count);

				Async.HandleEvents();
				expected += 2 * i;
			}
		}

		[Fact]
		public void Async_Then()
		{
			IAsyncOperation<int> operation = Async.Run(() => 0);
			Async.HandleEvents();

			for (int i = 0; i < 10; i++)
				operation = operation.ThenRun(count => Async.Run(() => count + 1));

			for (int i = 0; i < 10; i++)
				Async.HandleEvents();

			Assert.Equal(AsyncOperationState.Completed, operation.State);
			Assert.Equal(10, operation.Wait());
		}

		[Fact]
		public void Async_ThenWait()
		{
			int count = 0;
			Async.Run(() => { count += 1; }).ThenWait(() => { count += 1; }).Done();

			Assert.Equal(0, count);
			Async.HandleEvents();
			Assert.Equal(2, count);
		}

		//[Fact]
		//public void Async_Otherwise()
		//{
		//	bool thenPath = false, otherwisePath = false;
		//
		//	var operation = Async.Run(() => { throw new Exception(); });
		//	operation.ThenWait(() => thenPath = true);
		//	operation.Otherwise().ThenWait(() => otherwisePath = true);
		//
		//	Async.HandleEvents();
		//	Assert.False(thenPath);
		//	Assert.True(otherwisePath);
		//
		//	bool caughtException = thenPath = otherwisePath = false;
		//	operation = Async.Run(() => { throw new Exception(); });
		//	operation.ThenWait(() => thenPath = true);
		//	operation.Otherwise().ThenWait(() => otherwisePath = true);
		//	operation.Catch<Exception>(ex => caughtException = true);
		//
		//	Async.HandleEvents();
		//	Assert.True(thenPath);
		//	Assert.False(otherwisePath);
		//	Assert.True(caughtException);
		//}

		[Fact]
		public void Async_Wait()
		{
			IAsyncOperation operation = Async.Operation();
			int count = 0;
			for (int i = 0; i < 10; i++)
				operation = operation.ThenQueue(() => { count++; });
			
			operation.Wait();
			Assert.Equal(10, count);
            

			bool completed = false;
			operation = Async.Operation();

			Assert.False(completed);
			operation.ThenWait(() => completed = true);
			Assert.True(completed);

			completed = false;
			operation = Async.Run(() => { });
			operation.ThenWait(() => completed = true);
			Assert.False(completed);
			operation.Wait();
			Assert.True(completed);
		}


		[Fact]
		public void Async_Done()
		{
			var operation = Async.Run(() => { throw new Exception("Error"); });

			Assert.False(operation.IsDone);
			operation.Done();
			Assert.True(operation.IsDone);
			Async.HandleEvents();

            Assert.Throws<AsyncException>(() =>
            {
                try
                {
                    Async.HandleEvents();
                }
                catch (AsyncException ex)
                {
                    Assert.IsType<Exception>(ex.InnerException);
                    Assert.Equal("Error", ex.InnerException.Message);
                    throw;
                }
            });
		}

		[Fact]
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
			Assert.True(caughtException);
			Assert.True(finished);

			// An early exception might skip some work before it gets caught.
			caughtException = finished = false;
			operation = Async
				.Run(() => { throw new Exception("Error"); })
				.ThenQueue(() => { finished = true; })
				.Catch<Exception>(ex => caughtException = true);
			Async.HandleEvents();
			Async.HandleEvents();
			Assert.True(caughtException);
			Assert.False(finished);

			// Subclasses of exceptions will be caught, superclasses will not.
			caughtException = finished = false;
			operation = Async
				.Run(() => { throw new ArithmeticException("Error"); })
				.Catch<DivideByZeroException>(ex => { caughtException = true; })
				.ThenQueue(() => { finished = true; })
				.Catch<Exception>(ex => { caughtException = true; });
			Async.HandleEvents();
			Async.HandleEvents();
			Assert.True(caughtException);
			Assert.False(finished);
		}

		//[Fact]
		//[ExpectedException(typeof(InvalidOperationException))]
		//public void Async_Deferred()
		//{
		//	var completed = false;
		//	var deferredVoid = new Deferred();
		//
		//	deferredVoid.Operation.ThenWait(() => completed = true);
		//
		//	Assert.False(completed);
		//	Assert.False(deferredVoid.IsResolved);
		//	deferredVoid.Accept();
		//	Assert.True(completed);
		//	Assert.True(deferredVoid.IsResolved);
		//
		//
		//	var deferredInt = new Deferred<int>();
		//	int result = 0;
		//	deferredInt.Operation.ThenWait(i => result = i);
		//
		//	Assert.Equal(0, result);
		//	Assert.False(deferredInt.IsResolved);
		//	deferredInt.Accept(10);
		//	Assert.Equal(10, result);
		//	Assert.True(deferredInt.IsResolved);
		//
		//
		//	deferredInt = new Deferred<int>();
		//	result = 0;
		//	deferredInt.Operation
		//		.Catch<ArithmeticException>(ex => -1)
		//		.ThenWait(i => { result = i; });
		//
		//	deferredInt.Reject(new DivideByZeroException());
		//	Assert.Equal(-1, result);
		//
		//
		//	deferredInt.Accept(0); // Throw InvalidOperationException
		//}

		[Fact]
		public void Async_All()
		{
			int completedOperations = 0;
			bool allCompleted = false;

			IAsyncOperation op1 = Async.Run(() => { completedOperations += 1; }), op2 = Async.Run(() => { completedOperations += 1; });

			IAsyncOperation all = Async.All(op1, op2);
			all.ThenWait(() => {
				allCompleted = true;
			});

			Assert.Equal(0, completedOperations);
			Assert.False(allCompleted);
			op1.Wait();
			Assert.Equal(1, completedOperations);
			Assert.False(allCompleted);
			op2.Wait();
			Assert.Equal(2, completedOperations);
			Assert.True(allCompleted);

			Async.HandleEvents();

			all = Async.All(
				Async.Run(() => {
					throw new Exception("1");
				}),
				Async.Run(() => { }),
				Async.Run(() => { throw new Exception("3"); })
			);

			Async.HandleEvents();
			
            Assert.Equal(AsyncOperationState.Failed, all.State);
		}

		[Fact]
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
			Assert.Equal(1, completedIndex);


			completedIndex = -1;
			any = Async.Any(
				Async.Run(() => { throw new Exception("1"); }),
				Async.Run(() => { }),
				Async.Run(() => { throw new Exception("3"); })
			).ThenWait(index => completedIndex = index);

			Async.HandleEvents();
			Assert.Equal(1, completedIndex);


			completedIndex = -1;
			any = Async.Any(
				Async.Run(() => { throw new Exception("1"); }),
				Async.Run(() => { throw new Exception("2"); }),
				Async.Run(() => { throw new Exception("3"); })
			)
			.ThenWait(index => completedIndex = index)
			.Catch<AggregateException>(ex => {
				Assert.Equal(3, ex.InnerExceptions.Count);
				Assert.Equal("1", ex.InnerExceptions[0].Message);
				Assert.Equal("2", ex.InnerExceptions[1].Message);
				Assert.Equal("3", ex.InnerExceptions[2].Message);
				return 0;
			});

			Async.HandleEvents();
		}
	}
}
