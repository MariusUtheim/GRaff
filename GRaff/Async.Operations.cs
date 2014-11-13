using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GRaff.Synchronization;


namespace GRaff
{
	public static partial class Async
	{
		/*C#6.0*/
		public static AsyncOperation Operation()
		{
			return new AsyncOperation();
		}

		public static AsyncOperation Run(Action action)
		{
			var newOperation = new AsyncOperation();
			return newOperation.Then(action);
		}

		public static AsyncOperation Run(Func<Task> action)
		{
			var newOperation = new AsyncOperation();
			return newOperation.ThenAsync(action);
		}

		public static AsyncOperation<TOutput> Run<TOutput>(Func<TOutput> action)
		{
			var newOperation = new AsyncOperation();
			return newOperation.Then(action);
		}

		public static AsyncOperation<TOutput> Run<TOutput>(Func<Task<TOutput>> action)
		{
			var newOperation = new AsyncOperation();
			return newOperation.ThenAsync(action);
		}


	}
}
