using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
    /// <summary>
    /// Generator for points distributed uniformly on a rectangle.
    /// </summary>
    public sealed class PointRectDistribution : IDistribution<Point>
	{
		private readonly Random _rnd;

		public PointRectDistribution(Rectangle region)
			: this(GRandom.Source, region) { }

		public PointRectDistribution(Random rnd, Rectangle region)
		{
			_rnd = rnd;
			Region = region;
		}

		public Rectangle Region { get; set; }

		public Point Generate() => new Point(_rnd.Double(Region.Left, Region.Right), _rnd.Double(Region.Top, Region.Bottom));
	}
}
