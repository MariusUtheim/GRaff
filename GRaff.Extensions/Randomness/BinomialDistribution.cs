using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
	public sealed class BinomialDistribution : IDistribution<int>
	{
		private readonly Random _rnd;
		private readonly double[] _cdf;

		public BinomialDistribution(int trials, double probability)
			: this(GRandom.Source, trials, probability)
		{
			Contract.Requires<ArgumentOutOfRangeException>(probability >= 0 && probability <= 1);
		}

		public BinomialDistribution(Random rnd, int trials, double probability)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			Contract.Requires<ArgumentOutOfRangeException>(probability >= 0 && probability <= 1);

			_rnd = rnd;

			if (probability == 0)
				_cdf = new double[0];
			else if (probability == 1)
				_cdf = new double[] { 1.0 };
			else
			{
				_cdf = new double[trials];
				double logp = GMath.Log(probability), logq = GMath.Log(1.0 - probability), logGammaN = Combinatorics.LogGamma(trials + 1);
				for (int i = 0; i < trials; i++)
				{
					_cdf[i] = GMath.Exp(logGammaN - Combinatorics.LogGamma(1 + i) - Combinatorics.LogGamma(1 + trials - i) + i * logp + (trials - i) * logq);
					if (i > 0)
						_cdf[i] += _cdf[i - 1];
				}
			}
		}

		public int Generate()
		{
			double r = _rnd.Double();
			for (int i = 0; i < _cdf.Length; i++)
			{
				if (r < _cdf[i])
					return i;
			}

			return _cdf.Length;
		}
	}
}
