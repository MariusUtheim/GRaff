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
			Views = new[] { new View(new IntRectangle(0, 0, width, height), new IntRectangle(0, 0, width, height)) };
			this.RoomStart = roomStart;
			this.Speed = 60;
		}

		private static Room _current;
		public static Room Current
		{
			get { return _current; }
			private set
			{
				_current = value;
			}
		}

		public Action RoomStart { get; private set; }

		public void Enter()
		{
			foreach (var instance in Instance.All)
				instance.Destroy();
			Current = this;
			Draw.CurrentSurface = new Surface(Width, Height);
			if (RoomStart != null)
				this.RoomStart();
		}

		public void Redraw(Surface surface)
		{
//			this._surface = GraphicsEngine.Current.CreateSurface(Width, Height);
//			Draw.CurrentSurface = Draw.RoomSurface = this._surface;
//			Draw.DefaultSurface = surface;
			throw new NotImplementedException();
		}

		public int Speed { get; set; }

		public Point Center { get { return new Point(Width / 2, Height / 2); } }

		public int Width { get; private set; }
		public int Height { get; private set; }
		public IntVector Size
		{
			get { return new IntVector(Width, Height); }
			set { Width = value.X; Height = value.Y; }
		}

		public View[] Views { get; private set; }
	}
}
