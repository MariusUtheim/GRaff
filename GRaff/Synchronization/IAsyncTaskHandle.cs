using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	public interface IAsyncTaskHandle
	{
		void Abort();
		void Wait();
	}
}
