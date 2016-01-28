using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
	public class DiskPointDistribution : IDistribution<Point>
	{
		private readonly Random _rnd;
		private readonly RadialDistribution _radius;

		public DiskPointDistribution(Point center, double radius)
			: this(GRandom.Source, center, 1, radius)
		{
			Contract.Requires<ArgumentOutOfRangeException>(radius >= 0);
		}

		public DiskPointDistribution(Point center, double innerRadius, double outerRadius)
			: this(GRandom.Source, center, innerRadius, outerRadius)
		{
			Contract.Requires<ArgumentOutOfRangeException>(innerRadius >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(outerRadius >= innerRadius);
		}

		public DiskPointDistribution(Random rnd, Point center, double radius)
			: this(GRandom.Source, center, 0, radius)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			Contract.Requires<ArgumentOutOfRangeException>(radius >= 0);
		}

		public DiskPointDistribution(Random rnd, Point center, double innerRadius, double outerRadius)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			Contract.Requires<ArgumentOutOfRangeException>(innerRadius >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(outerRadius >= innerRadius);
			_radius = new RadialDistribution(innerRadius, outerRadius);
			_rnd = rnd;
			Center = center;
		}

		public Point Center { get; set; }

		public Point Generate()
		{
			return Center + new Vector(_radius.Generate(), _rnd.Angle());
		}
	}
}
