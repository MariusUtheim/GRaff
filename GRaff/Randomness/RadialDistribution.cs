using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Randomness
{
    /// <summary>
    /// Generator for a double-precision number according to a radial distribution.
    /// That is, when selecting a random point uniformly on a disk, the radius of that point follows a radial distribution.
    /// </summary>
    public class RadialDistribution : IDistribution<double>
    {
        private readonly Random _rnd;
        private readonly double _innerRadiusSqr, _radiusSquareDifference;

        public RadialDistribution(double radius)
            : this(GRandom.Source, 0, radius)
        {
            Contract.Requires<ArgumentOutOfRangeException>(radius >= 0);
        }

        public RadialDistribution(double innerRadius, double outerRadius)
            : this(GRandom.Source, innerRadius, outerRadius)
        {
            Contract.Requires<ArgumentOutOfRangeException>(innerRadius >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(outerRadius >= innerRadius);
        }

        public RadialDistribution(Random rnd, double radius)
            : this(rnd, 0, radius)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            Contract.Requires<ArgumentOutOfRangeException>(radius >= 0);
        }

        public RadialDistribution(Random rnd, double innerRadius, double outerRadius)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            Contract.Requires<ArgumentOutOfRangeException>(innerRadius >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(outerRadius >= innerRadius);

            _rnd = rnd;
            _innerRadiusSqr = innerRadius * innerRadius;
            _radiusSquareDifference = outerRadius * outerRadius - _innerRadiusSqr;
        }

        public double Generate() => GMath.Sqrt(_rnd.Double() * _radiusSquareDifference + _innerRadiusSqr);
    }
}
