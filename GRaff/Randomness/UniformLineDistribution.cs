using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Randomness
{
	public sealed class UniformLineDistribution : IDistribution<Point>
	{
		private Random _rnd;
		private Point _firstPointInclusive;
		private Point _secondPointExclusive;

		public UniformLineDistribution(Point firstPointInclusive, Point secondPointExclusive)
			: this(GRandom.Source, firstPointInclusive, secondPointExclusive)
		{ }

        public UniformLineDistribution(Line line)
			: this(GRandom.Source, line.Origin, line.Destination)
		{ }

		public UniformLineDistribution(Random rnd, Line line)
			: this(rnd, line.Origin, line.Destination)
		{ }

		public UniformLineDistribution(Random rnd, Point firstPointInclusive, Point secondPointExclusive)
		{
			this._rnd = rnd;
			this._firstPointInclusive = firstPointInclusive;
			this._secondPointExclusive = secondPointExclusive;
		}

		public Point Generate()
		{
			double t = _rnd.Double();
			return _firstPointInclusive * (1 - t) + _secondPointExclusive * t;
		}
	}
}
