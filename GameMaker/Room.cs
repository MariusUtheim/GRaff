using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class Room
	{
		private Surface _surface;
		public Room(int width, int height, Action roomStart)
		{
			Width = width;
			Height = height;
			this.RoomStart = roomStart;
			this._speed = 30;
		}

		private static Room _current;
		public static Room Current
		{
			get { return _current; }
			private set { _current = value; }
		}

		public Action RoomStart { get; private set; }

		public void Enter()
		{
			foreach (var instance in Instance.All)
				instance.Destroy();
			Current = this;
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

		public int Width { get; private set; }
		public int Height { get; private set; }
		public IntVector Size
		{
			get { return new IntVector(Width, Height); }
			set { Width = value.X; Height = value.Y; }
		}
	}
}
