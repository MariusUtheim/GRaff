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
		public event EventHandler Enter;
		public event EventHandler Leave;

		protected internal Room(int width, int height)
		{
			Contract.Requires<ArgumentOutOfRangeException>(width > 0 && height > 0);
			this.Width = width;
			this.Height = height;
		}

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

		public int Width { get; }
		public int Height { get; }
		public IntVector Size => new IntVector(Width, Height);
		public Point Center => new Point(Width / 2.0, Height / 2.0);

        public IntRectangle ClientRectangle => new IntRectangle(0, 0, Width, Height);

        public virtual void OnBeginStep() { }

        public virtual void OnEndStep() { }

        public virtual void OnEnter() { Enter?.Invoke(this, new EventArgs()); }

        public virtual void OnLeave() { Leave?.Invoke(this, new EventArgs()); }

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
            View.WholeRoom().Bind();
            View.UpdateGLToRoomMatrix();

			OnEnter();
		}

	}
}
