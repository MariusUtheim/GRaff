using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	class SpecificThreadTaskScheduler : TaskScheduler
	{
		ConcurrentQueue<Task> _queuedTasks = new ConcurrentQueue<Task>();

		public SpecificThreadTaskScheduler(Thread target)
		{
		}

		protected override IEnumerable<Task> GetScheduledTasks()
		{
			return _queuedTasks;
		}

		protected override void QueueTask(Task task)
		{
			_queuedTasks.Enqueue(task);
		}

		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
		{
			throw new NotImplementedException();
		}

		public override int MaximumConcurrencyLevel
		{
			get
			{
				return 1;
			}
		}

	}
}
