using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker;

namespace Sandbox
{
	public class Ball : MovingObject, IGlobalMousePressListener, ICollisionListener<Paddle>, ICollisionListener<Block>, ICollisionListener<NormalBlock>
	{
		private bool _isHeld;
		private List<Point> _trail = new List<Point>();

		public Ball()
		{
			X = Room.Width / 2;
			Y = Room.Height / 2;
			Velocity = new Vector(12, GRandom.Angle(5.0 / 8 * GMath.Tau, 7.0 / 8 * GMath.Tau));
			for (int i = 0; i < 20; i++)
				_trail.Add(Location);
			Image.Blend = new Color(128, Image.Blend.R, Image.Blend.G, Image.Blend.B);
		}

		public override void Step()
		{
			if (_isHeld)
			{
				X = Instance<Paddle>.First.X;
			}
			else
			{
				base.Step();
				if (X < 0 || X > Room.Width)
				{
					X = XPrevious;
					HSpeed = -HSpeed;
				}

				if (Y < 0)
				{
					Y = YPrevious;
					VSpeed = -VSpeed;
				}
			}

			_trail.Add(Location);
			_trail.RemoveAt(0);
		}

		public void OnGlobalMousePress(MouseButton button)
		{
			_isHeld = false;
		}

		public void OnCollision(Paddle other)
		{
			VSpeed = -Math.Abs(VSpeed);
		}

		public void OnCollision(Block other)
		{
			other.Hit(this);
			VSpeed = Math.Abs(VSpeed);
		}

		public override Sprite Sprite
		{
			get { return Sprites.Ball; }
		}

		public void OnCollision(NormalBlock other)
		{
			OnCollision(other as Block);
		}

		public override void OnDraw()
		{
/*			Color c = Color.Yellow;
			for (int i = 0; i < _trail.Count - 1; i++)
			{
				c.A = i * 255 / 20;
				Draw.Line(c, _trail[i], _trail[i + 1]);
			}
			*/
			base.OnDraw();
		}
	}
}
