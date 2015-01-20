using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Randomness
{
	public class RadialDistribution : IDistribution<double>
	{
		private Random _rnd;
		private double _innerRadiusSqr, _radiusSquareDifference;

		public RadialDistribution(double radius)
			: this(GRandom.Source, 0, radius) { }

		public RadialDistribution(double innerRadius, double outerRadius)
			: this(GRandom.Source, innerRadius, outerRadius) { }

		public RadialDistribution(Random rnd, double radius)
			: this(rnd, 0, radius) { }

		public RadialDistribution(Random rnd, double innerRadius, double outerRadius)
		{
			if (innerRadius < 0) throw new ArgumentOutOfRangeException("innerRadius", "Must be greater than or equal to zero."); /*C#6.0*/
			if (outerRadius < 0) throw new ArgumentOutOfRangeException("outerRadius", "Must be greater than or equal to zero."); /*C#6.0*/
			if (innerRadius > outerRadius) throw new ArgumentException("innerRadius must be less than or equal to outerRadius.");
			_rnd = rnd;
			_innerRadiusSqr = innerRadius * innerRadius;
			_radiusSquareDifference = outerRadius * outerRadius - _innerRadiusSqr;
		}

		public double Generate()
		{
			return GMath.Sqrt(_rnd.Double() * _radiusSquareDifference + _innerRadiusSqr);
		}
	}
}
