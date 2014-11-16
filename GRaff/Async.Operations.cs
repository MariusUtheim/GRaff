using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GRaff.Synchronization;


namespace GRaff
{
	public static partial class Async
	{
		/*C#6.0*/


		public static AsyncOperation Run(Action action)
		{
			return new AsyncOperation().Then(action);
		}

		public static AsyncOperation<TPass> Run<TPass>(Func<TPass> action)
		{
			return new AsyncOperation().Then(action);
		}
	}
}
