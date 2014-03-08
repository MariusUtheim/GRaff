using GameMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker.Motions;

namespace Boids
{
	public class Hoik : MovingObject
	{
		private const double MinSpeed = 2;
		private const double MaxSpeed = 8;
		private const double NeighbourhoodRadius = 250;

		public Hoik(Point location)
		{
			this.Location = location;
		}
		public Hoik(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public Boid[] GetNeighbourhood()
		{
			return Instance<Boid>.Where(b => (this.Location - b.Location).Magnitude < NeighbourhoodRadius).ToArray();
		}

		public override void EndStep()
		{
			var neighbourhood = GetNeighbourhood();

			this.AccelerateTowards(this.NearestInstance<Boid>().Location, 0.4);


			Velocity += 0.001 * (Room.Center - Location);
			Location += 0.001 * (Room.Center - Location);
			Speed = GMath.Median(MinSpeed, Speed, MaxSpeed);

		//	Location = Mouse.Location;
		}


		public override void OnDraw()
		{
			Draw.Line(Color.Red, Location, Location + (Velocity + Angle.FromDegrees(120)).Normal * 8);
			Draw.Line(Color.Red, Location, Location + (Velocity - Angle.FromDegrees(120)).Normal * 8);
		}
	}
}
