using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public class Room
	{
		private int _width, _height;

#warning TODO: Make a Room creation class or something
		public Room(int width, int height, Action roomStart = null)
		{
			this._width = width;
			this._height = height;
			this.RoomStart = roomStart;
		}

		private static Room _current;

		public static int Width { get { return _current._width; } }
		public static int Height { get { return _current._height; } }
		public static IntVector Size
		{
			get { return new IntVector(Width, Height); }
		}

		public Action RoomStart { get; private set; }

		public void Enter()
		{
			foreach (var instance in Instance.Elements)
				instance.Destroy();
			_current = this;
			Draw.CurrentSurface = new Surface(Width, Height);
			View.RoomView = new IntRectangle(0, 0, Width, Height);
			if (RoomStart != null)
				this.RoomStart();
		}

		public static Point Center { get { return new Point(Width / 2, Height / 2); } }

		public int GetWidth()
		{
			return _width;
		}

		public int GetHeight()
		{
			return _height;
		}
	}
}
