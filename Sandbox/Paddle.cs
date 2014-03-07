using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker;

namespace Sandbox
{
	public class Paddle : MovingObject, IKeyListener
	{
		public Paddle()
		{
			Sprite = Sprites.Paddle;
			X = GMath.Median(Width / 2, Mouse.X, Room.Width - Width / 2);
			Y = Room.Height - 100;
		}

		public override void Step()
		{
			base.Step();
			X = Mouse.X;

			Image.Rotation = Angle.FromRadians(0.2 * Math.Sin(Environment.TickCount / 800.0));
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
