using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
	public sealed class IntegerDistribution : IDistribution<int>
	{
		private readonly Random _rnd;
        private readonly int _lowerInclusive, _upperExclusive;

        public IntegerDistribution(int lowerInclusive, int upperExclusive)
			: this(GRandom.Source, lowerInclusive, upperExclusive) { }

        public IntegerDistribution(Random rnd, int lowerInclusive, int upperExclusive)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			_rnd = rnd;
			_lowerInclusive = lowerInclusive;
			_upperExclusive = upperExclusive;
		}

		public int Generate()
		{
			return _rnd.Integer(_lowerInclusive, _upperExclusive);
		}
	}
}
