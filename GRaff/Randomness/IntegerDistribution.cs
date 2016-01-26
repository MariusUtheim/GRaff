using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
	public sealed class IntegerDistribution : IDistribution<int>
	{
		private readonly Random _rnd;
		private readonly int _firstValueInclusive, _secondValueExclusive;

		public IntegerDistribution(int firstValueInclusive, int secondValueExclusive)
			: this(GRandom.Source, firstValueInclusive, secondValueExclusive) { }

		public IntegerDistribution(Random rng, int firstValueInclusive, int secondValueExclusive)
		{
			Contract.Requires<ArgumentNullException>(rng != null);
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
