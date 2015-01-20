using System;


namespace GRaff.Randomness
{
	public sealed class IntegerDistribution : IDistribution<int>
	{
		Random _rnd;
		int _firstValueInclusive, _secondValueExclusive;

		public IntegerDistribution(int firstValueInclusive, int secondValueExclusive)
			: this(GRandom.Source, firstValueInclusive, secondValueExclusive) { }

		public IntegerDistribution(Random rng, int firstValueInclusive, int secondValueExclusive)
		{
			_rnd = rng;
			_firstValueInclusive = firstValueInclusive;
			_secondValueExclusive = secondValueExclusive;
		}

		public int Generate()
		{
			return _rnd.Integer(_firstValueInclusive, _secondValueExclusive);
		}
	}
}
