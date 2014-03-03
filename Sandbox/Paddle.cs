using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker;

namespace Sandbox
{
	public class Paddle : GameObject, IKeyListener
	{
		public Paddle()
		{
			X = GMath.Median(Width / 2, Mouse.X, Room.Width - Width / 2);
			Y = Room.Height - 100;
		}

		public override Sprite Sprite
		{
			get { return Sprites.Paddle; }
		}

		public override void Step()
		{
			base.Step();
			X = Mouse.X;
			Image.Blend = Color.Red;
		}

		public void OnKey(Key key)
		{
			if (key == Key.Left)
				X -= 12;
			if (key == Key.Right)
				X += 12;
		}
	}
}
