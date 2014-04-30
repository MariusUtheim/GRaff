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
			: base(0, 0)
		{
			Sprite = Sprites.Paddle;
			X = GMath.Median(Width / 2, Mouse.X, Room.Current.Width - Width / 2);
			Y = Room.Current.Height - 100;
		}

		public override void OnStep()
		{
			base.OnStep();
			X = Mouse.X;

			Image.XScale = 1.5 + 0.5 * Math.Sin(Environment.TickCount / 800.0);
			Image.Rotation = Angle.FromRadians(0.1 * Math.Sin(Environment.TickCount / 1000.0));
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
