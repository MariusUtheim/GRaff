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
			Game.Run(gameStart);
		}

		static void gameStart()
		{
			GlobalEvent.ExitOnEscape();

			Instance<Drawer>.Create();
			Instance<Block>.Create(400, 400);

			Window.Title = "Hello, world!";
		}
	}
}
