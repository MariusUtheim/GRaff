using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Randomness
{
	public sealed class RectanglePointDistribution : IDistribution<Point>
	{
		Random _rnd;

		public RectanglePointDistribution(Rectangle region)
			: this(GRandom.Source, region) { }

		public RectanglePointDistribution(Random rnd, Rectangle region)
		{
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
