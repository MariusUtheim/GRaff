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
			Width = 1024;
			Height = 768;
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
		public static int Speed { get { return 30; } }

		public static int Width { get; private set; }

		public static int Height { get; private set; }
	}
}
