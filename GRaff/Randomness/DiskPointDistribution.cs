using System;
 
 
namespace GRaff.Randomness
{
	public class DiskPointDistribution : IDistribution<Point>
	{
		private Random _rnd;
		private RadialDistribution _radius;

		public DiskPointDistribution(Point center, double radius)
			: this(GRandom.Source, center, 0, radius) { }

		public DiskPointDistribution(Point center, double innerRadius, double outerRadius)
			: this(GRandom.Source, center, innerRadius, outerRadius) { }

		public DiskPointDistribution(Random rnd, Point center, double radius)
			: this(GRandom.Source, center, 0, radius) { }

		public DiskPointDistribution(Random rnd, Point center, double innerRadius, double outerRadius)
		{
			_rnd = rnd;
			Center = center;
			_radius = new RadialDistribution(innerRadius, outerRadius);
		}

		public Point Center { get; set; }

		public Point Generate()
		{
			return Center + new Vector(_radius.Generate(), _rnd.Angle());
		}
	}
}
