using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public abstract class MovingObject : GameObject
	{
		public MovingObject()
			: this(0, 0) { }

		public MovingObject(double x, double y)
			: base(x, y) { }

		public override void OnEndStep()
		{
			base.OnEndStep();

			Location += Velocity;
			if (Speed <= Friction)
				Speed = 0;
			else
				Speed -= Friction;
		}


		public double XPrevious { get; set; }
		public double YPrevious { get; set; }
		public Point PreviousLocation
		{
			get { return new Point(XPrevious, YPrevious); }
			set { XPrevious = value.X; YPrevious = value.Y; }
		}

		public Vector Velocity { get; set; }
		public double HSpeed
		{
			get { return Velocity.X; }
			set { Velocity = new Vector(value, VSpeed); }
		}
		public double VSpeed
		{
			get { return Velocity.Y; }
			set { Velocity = new Vector(HSpeed, value); }
		}
		public double Speed
		{
			get { return Velocity.Magnitude; }
			set { Velocity = new Vector(value, Direction); }
		}
		public Angle Direction
		{
			get { return Velocity.Direction; }
			set { Velocity = new Vector(Speed, value); ; }
		}

		public double Friction { get; set; }

		public override void OnBeginStep()
		{
			base.OnBeginStep();
			XPrevious = X;
			YPrevious = Y;
		}
	}
}
