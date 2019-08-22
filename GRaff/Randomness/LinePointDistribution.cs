using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
	public sealed class LinePointDistribution : IDistribution<Point>
	{
		private readonly Random _rnd;
		private readonly Point _firstPointInclusive;
		private readonly Point _secondPointExclusive;

		public LinePointDistribution(Point firstPointInclusive, Point secondPointExclusive)
			: this(GRandom.Source, firstPointInclusive, secondPointExclusive)
		{ }

        public LinePointDistribution(Line line)
			: this(GRandom.Source, line.Origin, line.Destination)
		{ }

		public LinePointDistribution(Random rnd, Line line)
			: this(rnd, line.Origin, line.Destination)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
		}

		public LinePointDistribution(Random rnd, Point firstPointInclusive, Point secondPointExclusive)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
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
