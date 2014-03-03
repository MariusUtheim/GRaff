using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameMaker;
using GameMaker.Forms;

namespace Sandbox
{
	static partial class Program
	{
		static void Main()
		{
			GameMaker.Forms.Runner.Run(GameStart);
		}

		static void GameStart()
		{
			new Ball();
			new Ball();
			new Paddle();
			for (int i = 40; i < Room.Width - 40; i+= 32)
				new NormalBlock(i, 40);

			Window.IsBorderVisible = false;

			GlobalEvent.Key += delegate(Key key) {
				if (key == Key.Escape)
					Game.Quit();
			};
		}
	}
}
