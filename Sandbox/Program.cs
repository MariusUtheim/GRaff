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
			Game.Run<GameMaker.Forms.FormsGraphicsEngine>(GameStart);
		}

		static void GameStart()
		{
			new Paddle();
			new Ball();
			//new Ball();

			for (int i = 40; i < Room.Width - 40; i += 64)
				new NormalBlock(i, 40);

			Window.IsBorderVisible = false;
			Background.Color = Color.DeepPink;

			GlobalEvent.Key += delegate(Key key)
			{
				if (key == Key.Escape)
					Game.Quit();
			};
		}
	}
}
