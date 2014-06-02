using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker;
using GameMaker.Motions;

namespace Boids
{
	public class Boid : MovingObject
	{
		private const double MaxSpeed = 8;
		private const double MinSpeed = 6;

		public Boid(Point location)
		{
			this.Location = location;
		}

		public Boid(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public override void OnEndStep()
		{
			Velocity += new Neighbourhood(this).GetAcceleration();

			Velocity += 0.001 * (Room.Current.Center - Location);
			Location += 0.001 * (Room.Current.Center - Location);
			Speed = GMath.Median(MinSpeed, Speed, MaxSpeed);
		}

		public override void OnDraw()
		{
			Draw.Line(Color.Black, Location, Location + (Velocity + Angle.FromDegrees(150)).Normal * 5);
			Draw.Line(Color.Black, Location, Location + (Velocity - Angle.FromDegrees(150)).Normal * 5);

			Fill.Rectangle(Color.White, 1, 1, 2, 2);
		}
	}
}
