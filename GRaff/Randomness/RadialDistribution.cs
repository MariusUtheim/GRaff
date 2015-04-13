using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
			Contract.Requires(rnd != null);
			Contract.Requires(innerRadius >= 0);
			Contract.Requires(outerRadius >= innerRadius);

			_rnd = rnd;
			_innerRadiusSqr = innerRadius * innerRadius;
			_radiusSquareDifference = outerRadius * outerRadius - _innerRadiusSqr;
		}

		public double Generate()
			=> GMath.Sqrt(_rnd.Double() * _radiusSquareDifference + _innerRadiusSqr);
	}
}
