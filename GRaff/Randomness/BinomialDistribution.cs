using System;


namespace GRaff.Randomness
{
	public sealed class BinomialDistribution : IDistribution<int>
	{
		private Random _rnd;
		private double[] _cdf;

		public BinomialDistribution(int trials, double probability)
			: this(GRandom.Source, trials, probability) { }

		public BinomialDistribution(Random rnd, int trials, double probability)
		{
			if (probability < 0 || probability > 1)
				throw new ArgumentOutOfRangeException("probability", "Probability must be between 0 and 1 (got " + probability + ")");	 /*C#6.0*/

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
