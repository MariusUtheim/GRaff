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
			var initialRoom = new Room(1024, 768, GameStart);
			Game.Run<GameMaker.Forms.FormsGraphicsEngine>(initialRoom, null);
		}

		static void GameStart()
		{
			new Paddle();
			new Ball();

			for (int i = 40; i < Room.Current.Width - 40; i += 64)
				new NormalBlock(i, 40);

			Background.Color = Color.White;

			GlobalEvent.Key += delegate(Key key)
			{
				if (key == Key.Escape)
					Game.Quit();
			};
		}
	}
}
