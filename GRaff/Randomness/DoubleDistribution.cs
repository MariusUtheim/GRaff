using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
	public sealed class DoubleDistribution : IDistribution<double>
	{
		private readonly Random _rnd;
		private readonly double _firstValueInclusive, _secondValueExclusive;

		public DoubleDistribution(double firstValueInclusive, double secondValueExclusive)
			: this(GRandom.Source, firstValueInclusive, secondValueExclusive) { }

		public DoubleDistribution(Random rng, double firstValueInclusive, double secondValueExclusive)
		{
			Contract.Requires<ArgumentNullException>(rng != null);
			_rnd = rng;
			_firstValueInclusive = firstValueInclusive;
			_secondValueExclusive = secondValueExclusive;
		}

		public double Generate()
		{
			return _rnd.Double(_firstValueInclusive, _secondValueExclusive);
		}
	}
}