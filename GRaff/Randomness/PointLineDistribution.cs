using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
    /// <summary>
    /// Generator for points distributed uniformly on a line.
    /// </summary>
    public sealed class PointLineDistribution : IDistribution<Point>
	{
		private readonly Random _rnd;

        public PointLineDistribution(Line line)
			: this(GRandom.Source, line)
		{ }

		public PointLineDistribution(Random rnd, Line line)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
            this._rnd = rnd;
            this.Line = line;
		}

        public Line Line { get; set; }

        public Point Generate() => Line.Destination + _rnd.Double() * Line.Direction;
	}
}
