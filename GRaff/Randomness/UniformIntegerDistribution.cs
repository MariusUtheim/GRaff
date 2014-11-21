using System;


namespace GRaff.Randomness
{
	public sealed class UniformIntegerDistribution : IDistribution<int>
	{
		Random _rnd;
		int _firstValueInclusive, _secondValueExclusive;

		public UniformIntegerDistribution(int firstValueInclusive, int secondValueExclusive)
			: this(GRandom.Source, firstValueInclusive, secondValueExclusive) { }

		public UniformIntegerDistribution(Random rng, int firstValueInclusive, int secondValueExclusive)
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
