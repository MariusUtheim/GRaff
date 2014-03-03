using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker;

namespace Sandbox
{
	public static class Sprites
	{
		static Sprites()
		{
			Ball = new Sprite("Assets/ball.png", OriginMode.Center, true);
			Paddle = new Sprite("Assets/paddle.png", OriginMode.Center, true);
			Block = new Sprite("Assets/block.png");
		}

		public static Sprite Ball { get; private set; }

		public static Sprite Paddle { get; set; }

		public static Sprite Block { get; set; }
	}
}
