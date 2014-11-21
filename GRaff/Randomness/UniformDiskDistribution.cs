using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Randomness
{
	public class UniformDiskDistribution : IDistribution<Point>
	{
		Random _rnd;
		double _innerRadiusSqr, _radiusSquareDifference;

		public UniformDiskDistribution(Point center, double radius)
			: this(GRandom.Source, center, 0, radius) { }

		public UniformDiskDistribution(Point center, double innerRadius, double outerRadius)
			: this(GRandom.Source, center, innerRadius, outerRadius) { }

		public UniformDiskDistribution(Random rnd, Point center, double radius)
			: this(GRandom.Source, center, 0, radius) { }

		public UniformDiskDistribution(Random rnd, Point center, double innerRadius, double outerRadius)
		{
			if (innerRadius < 0) throw new ArgumentOutOfRangeException("innerRadius", "Must be greater than or equal to zero."); /*C#6.0*/
			if (outerRadius < 0) throw new ArgumentOutOfRangeException("outerRadius", "Must be greater than or equal to zero."); /*C#6.0*/
			if (innerRadius > outerRadius) throw new ArgumentException("innerRadius must be less than or equal to outerRadius.");
			_rnd = rnd;
			Center = center;
			_innerRadiusSqr = innerRadius * innerRadius;
			_radiusSquareDifference = outerRadius * outerRadius - _innerRadiusSqr;
		}

		public Point Center { get; set; }

		public Point Generate()
		{
			double radius = GMath.Sqrt(_rnd.Double() * _radiusSquareDifference + _innerRadiusSqr);
			return Center + new Vector(radius, _rnd.Angle());
		}
	}
}
