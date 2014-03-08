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
		private const double NeighbourhoodRadius = 50;
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

		public Boid[] GetNeighbourhood()
		{
			return Instance<Boid>.Where(boid => (this.Location - boid.Location).Magnitude < NeighbourhoodRadius && boid != this).ToArray();
		}

		public override void EndStep()
		{

			var neighbourhood = GetNeighbourhood();

			if (neighbourhood.Length != 0)
			{
				Vector offset = Location - neighbourhood.Aggregate(Vector.Zero, (v, b) => v + (Vector)b.Location / neighbourhood.Length);
				Vector heading = neighbourhood.Aggregate(Vector.Zero, (v, b) => v + b.Velocity / neighbourhood.Length);
				Vector avoidCollision = neighbourhood.Aggregate(Vector.Zero, (v, b) => v + Vector.FromPolar(NeighbourhoodRadius - (Location - b.Location).Magnitude, (Location - b.Location).Direction));
				Vector avoidHoik = Instance<Hoik>
					.Where(h => (this.Location - h.Location).Magnitude < NeighbourhoodRadius)
					.Aggregate(Vector.Zero, (v, h) => v += Vector.FromPolar(NeighbourhoodRadius - (Location - h.Location).Magnitude, (Location - h.Location).Direction));

				Vector acceleration =
					-0.03 * offset
					+ 0.15 * heading
					+ 0.05 * avoidCollision
					+ 0.10 * avoidHoik;
				//if (acceleration.Magnitude > MaxAcceleration)
				//	acceleration.Magnitude = MaxAcceleration;

				this.Velocity += acceleration;
			}


			Velocity += 0.001 * (Room.Center - Location);
			Location += 0.001 * (Room.Center - Location);
			Speed = GMath.Median(MinSpeed, Speed, MaxSpeed);

			Window.Title = String.Format("Number of remaining boids: {0}", Instance<Boid>.All.Count());
		}

		public override void OnDraw()
		{
			Draw.Line(Color.Black, Location, Location + (Velocity + Angle.FromDegrees(150)).Normal * 5);
			Draw.Line(Color.Black, Location, Location + (Velocity - Angle.FromDegrees(150)).Normal * 5);
		}
	}
}
