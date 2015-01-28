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
		internal Room(int width, int height, Action roomStart = null)
		{
			this._width = width;
			this._height = height;
		}
		internal Action RoomStart { get; private set; }

		private static Room _current;

		public static int Width { get { return _current._width; } }
		public static int Height { get { return _current._height; } }
		public static IntVector Size
		{
			get { return new IntVector(Width, Height); }
		}
		public static Point Center { get { return new Point(Width / 2, Height / 2); } }


		internal void Enter()
		{
			foreach (var instance in Instance.All)
				instance.Destroy();
			_current = this;
			View.FocusRegion = new IntRectangle(0, 0, Width, Height);
			if (RoomStart != null)
				this.RoomStart();
		}


		internal int GetWidth()
		{
			return _width;
		}

		internal int GetHeight()
		{
			return _height;
		}
	}
}
