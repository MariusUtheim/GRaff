using System;


namespace GRaff.Randomness
{
	public sealed class UniformDoubleDistribution : IDistribution<double>
	{
		Random _rnd;
		double _firstValueInclusive, _secondValueExclusive;

		public UniformDoubleDistribution(double firstValueInclusive, double secondValueExclusive)
			: this(GRandom.Source, firstValueInclusive, secondValueExclusive) { }

		public UniformDoubleDistribution(Random rng, double firstValueInclusive, double secondValueExclusive)
		{
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