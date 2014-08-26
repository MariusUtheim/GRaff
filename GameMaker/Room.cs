using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class Room
	{
		private int _width, _height;

		public Room(int width, int height, Action roomStart)
		{
			this._width = width;
			this._height = height;
			this.RoomStart = roomStart;
			this._speed = 30;
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
			foreach (var instance in Instance.All)
				instance.Destroy();
			_current = this;
			Draw.CurrentSurface = new Surface(Width, Height);
			View.RoomView = new IntRectangle(0, 0, Width, Height);
			if (RoomStart != null)
				this.RoomStart();
			this.Speed = this._speed;
		}

		private int _speed;
		public int Speed
		{
			get { return _speed; }
			set
			{
				_speed = value;
				Game.Window.TargetRenderFrequency = _speed;
				Game.Window.TargetUpdateFrequency = _speed;
			}
		}

		public Point Center { get { return new Point(Width / 2, Height / 2); } }

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
