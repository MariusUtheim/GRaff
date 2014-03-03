using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public abstract class MovingObject : GameObject
	{
		public override void Step()
		{
			base.Step();

			Location += Velocity;
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

		public override void BeginStep()
		{
			base.BeginStep();
			XPrevious = X;
			YPrevious = Y;
		}
	}
}
