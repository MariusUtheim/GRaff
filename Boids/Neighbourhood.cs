using GameMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boids
{
	public class Neighbourhood
	{
		private const double NeighbourhoodRadius = 50;

		public Vector GetAcceleration()
		{
			Vector acceleration =
				0.03 * Center()
				+ 0.15 * Heading()
				+ 0.05 * AvoidBoids()
				+ 0.1 * AvoidObstacles()
				+ 0.1 * AvoidHoik()
				;

			return acceleration;
		}

		public Neighbourhood(Boid focus)
		{
			this.Focus = focus;
			this.Boids = Instance<Boid>.Where(b => (b.Location - focus.Location).Magnitude < NeighbourhoodRadius && b != Focus).ToArray();
			this.Hoiks = Instance<Hoik>.Where(h => (h.Location - focus.Location).Magnitude < NeighbourhoodRadius).ToArray();
			this.Obstacles = Instance<Obstacle>.Where(o => (o.Location - focus.Location).Magnitude < NeighbourhoodRadius + o.Radius).ToArray();
		}

		public Boid Focus { get; set; }

		public Boid[] Boids { get; set; }

		public Hoik[] Hoiks { get; set; }

		public Obstacle[] Obstacles { get; set; }


		public Vector Center()
		{
			if (Boids.Length == 0)
				return Vector.Zero;

			Vector center = Vector.Zero;
			
			foreach (Boid b in Boids)
				center += (b.Location - Focus.Location);

			return center / Boids.Length;
		}

		public Vector Heading()
		{
			if (Boids.Length == 0)
				return Vector.Zero;

			Vector heading = Vector.Zero;

			foreach (Boid b in Boids)
				heading += b.Velocity;

			return heading / Boids.Length;
		}

		public Vector AvoidBoids()
		{
			Vector avoid = Vector.Zero;

			foreach (Boid b in Boids)
				avoid += Vector.Polar(NeighbourhoodRadius - (Focus.Location - b.Location).Magnitude, (Focus.Location - b.Location).Direction);

			return avoid;
		}

		public Vector AvoidObstacles()
		{
			Vector avoid = Vector.Zero;

			foreach (Obstacle o in Obstacles)
				avoid += Vector.Polar(NeighbourhoodRadius + o.Radius - (Focus.Location - o.Location).Magnitude, (Focus.Location - o.Location).Direction);

			return avoid;
		}

		public Vector AvoidHoik()
		{
			Vector avoid = Vector.Zero;

			foreach (Hoik h in Hoiks)
				avoid += Vector.Polar(NeighbourhoodRadius - (Focus.Location - h.Location).Magnitude, (Focus.Location - h.Location).Direction);

			return avoid;
		}
	}
}
