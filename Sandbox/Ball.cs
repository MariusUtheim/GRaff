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
		private bool _isHeld = false;
		private List<Point> _trail = new List<Point>();

		public Ball()
			: base(0, 0)
		{
			Sprite = Sprites.Ball;
			X = Room.Current.Width / 2;
			Y = Room.Current.Height / 2;
			Velocity = new Vector(12, GRandom.Angle(5.0 / 8 * GMath.Tau, 7.0 / 8 * GMath.Tau));
			for (int i = 0; i < 20; i++)
				_trail.Add(Location);
		}

		public override void OnStep()
		{
			if (_isHeld)
			{
				X = Instance<Paddle>.First.X;
			}
			else
			{
				base.OnStep();
				if (X < 0 || X > Room.Current.Width)
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

			Room.Current.Views[0].Center = (IntVector)this.Location;

			if (Y > Room.Current.Width)
			{
				new Ball();
				this.Destroy();
			}
		}

		public void OnGlobalMousePress(MouseButton button)
		{
			_isHeld = false;
		}

		public void OnCollision(Paddle other)
		{
			VSpeed = -Math.Abs(VSpeed);
			HSpeed += (other.X - other.XPrevious);
		}

		public void OnCollision(Block other)
		{
			other.Hit(this);
			VSpeed = Math.Abs(VSpeed);
		}

		public void OnCollision(NormalBlock other)
		{
			OnCollision(other as Block);
		}

		public override void OnDraw()
		{
			Color col = Color.Yellow;
			
			for (int i = 0; i < _trail.Count - 1; i++)
			{
				Draw.Line(new Color(255 * i / _trail.Count, col), _trail[i], _trail[i + 1]);
			}

			base.OnDraw();
		}
	}
}
