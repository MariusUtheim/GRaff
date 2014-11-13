

namespace GRaff.Synchronization
{
	internal interface IAsyncTaskFactory
	{
		IAsyncTaskHandle Invoke(AsyncWorker sender, object parameter);
	}

	internal interface IAsyncTaskFactory<TIn, TOut> : IAsyncTaskFactory
	{
		IAsyncTaskHandle Invoke(AsyncWorker sender, TIn parameter);
	}
}