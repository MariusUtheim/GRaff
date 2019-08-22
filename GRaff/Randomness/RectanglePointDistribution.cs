using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
	public sealed class RectanglePointDistribution : IDistribution<Point>
	{
		private readonly Random _rnd;

		public RectanglePointDistribution(Rectangle region)
			: this(GRandom.Source, region) { }

		public RectanglePointDistribution(Random rnd, Rectangle region)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			_rnd = rnd;
			Region = region;
		}

		public Rectangle Region { get; set; }

		public Point Generate()
		{
			return new Point(_rnd.Double(Region.Left, Region.Right), _rnd.Double(Region.Top, Region.Bottom));
		}
	}
}
