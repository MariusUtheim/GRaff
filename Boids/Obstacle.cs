using GameMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boids
{
	public class Obstacle : GameObject
	{
		private const double _defaultRadius = 20;

		public Obstacle(Point location, double radius)
		{
			this.Location = location;
			this.Radius = radius;
		}

		public Obstacle(double x, double y, double radius)
		{
			this.X = x;
			this.Y = y;
			this.Radius = radius;
		}

		public Obstacle(double x, double y)
			: this(x, y, _defaultRadius) { }

		public double Radius { get; set; }

		public override void OnDraw()
		{
			Draw.Circle(Color.Black, Location, Radius);
		}
	}
}
