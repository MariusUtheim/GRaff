using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public class Room
	{
        
		public static Room Current { get; private set; }

		public static void Goto(Room room)
		{
			Contract.Requires<ArgumentNullException>(room != null);
			Current?._Leave();
			Current = room;
			room._Enter();
		}

		public static void Goto<TRoom>() where TRoom : Room
		{
			Goto(Activator.CreateInstance<TRoom>());
		}
        
        public virtual void OnBeginStep() { }

        public virtual void OnEndStep() { }

        public virtual void OnEnter() { }

        public virtual void OnLeave() { }

		public virtual void OnDrawBackground() { }

        public virtual void OnDrawForeground() { }


		internal void _Leave()
		{
			OnLeave();

			foreach (var instance in Instance.All)
				Instance.Remove(instance);
		}

		internal void _Enter()
		{
			Current = this;
            View.FullWindow().Bind();

			OnEnter();
		}

	}
}
