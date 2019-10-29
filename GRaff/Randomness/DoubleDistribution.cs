using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
    /// <summary>
    /// Generator for double-precision numbers according to a uniform distribution.
    /// </summary>
    public sealed class DoubleDistribution : IDistribution<double>
	{
		private readonly Random _rnd;
		private readonly double _firstValueInclusive, _secondValueExclusive;

		public DoubleDistribution(double firstValueInclusive, double secondValueExclusive)
			: this(GRandom.Source, firstValueInclusive, secondValueExclusive) { }

		public DoubleDistribution(Random rnd, double firstValueInclusive, double secondValueExclusive)
		{
			_rnd = rnd;
			_firstValueInclusive = firstValueInclusive;
			_secondValueExclusive = secondValueExclusive;
		}

		public double Generate() => _rnd.Double(_firstValueInclusive, _secondValueExclusive);
	}
}