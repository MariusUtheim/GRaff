using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class Room
	{
		static Room()
		{
			Width = 1280;
			Height = 800;
		}

		public Room(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public void Enter()
		{
			
		}

#warning GameMaker.Room.Speed is not implemented, but does not throw an exception if called.
		public static int Speed { get { return 60; } }

		public static Point Center { get { return new Point(Width / 2, Height / 2); } }

		public static int Width { get; private set; }

		public static int Height { get; private set; }
	}
}
