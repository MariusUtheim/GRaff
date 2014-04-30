using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public abstract class Controller
	{
		public Controller()
		{
			Instance._controllers.Add(this);
		}

		public void Destroy()
		{
			Instance._controllers.Remove(this);
		}

		public virtual void OnBeginStep() { }

		public virtual void OnEndStep() { }

		public virtual void OnBeginDraw() { }

		public virtual void OnEndDraw() { }
	}

	public static class Controller<T> where T : Controller
	{
		public static T Create()
		{
			return Activator.CreateInstance<T>();
		}
	}
}
