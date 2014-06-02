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
			Ball = new Sprite("Assets/ball.png", 1, OriginMode.Center);
			Paddle = new Sprite("Assets/paddle.png", 1, OriginMode.Center);
			Block = new Sprite("Assets/block.png", 2, OriginMode.Center);
		}

		public static Sprite Ball { get; private set; }

		public static Sprite Paddle { get; set; }

		public static Sprite Block { get; set; }
	}
}
