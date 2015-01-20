using System;

 
namespace GRaff.Randomness
{
	public sealed class DoubleDistribution : IDistribution<double>
	{
		Random _rnd;
		double _firstValueInclusive, _secondValueExclusive;

		public DoubleDistribution(double firstValueInclusive, double secondValueExclusive)
			: this(GRandom.Source, firstValueInclusive, secondValueExclusive) { }

		public DoubleDistribution(Random rng, double firstValueInclusive, double secondValueExclusive)
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