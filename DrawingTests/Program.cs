using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameMaker;

namespace DrawingTests
{
	static class Program
	{
		static void Main()
		{
			Game.Run(new Room(1024, 768), 60, gameStart);
		}

		static void gameStart()
		{
			GlobalEvent.ExitOnEscape();

			new Drawer();

			Window.Title = "Hello, world!";
		}
	}
}
