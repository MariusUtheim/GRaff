using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
    /// <summary>
    /// Generator for double-precision values according to a normal distribution.
    /// </summary>
    public sealed class NormalDistribution : IDistribution<double>
	{
		private readonly Random _rnd;
		private readonly double _mean, _std;

		public NormalDistribution()
			: this(GRandom.Source, 0, 1) { }

		public NormalDistribution(double mean, double standardDeviation)
			: this(GRandom.Source, mean, standardDeviation)
		{
			Contract.Requires<ArgumentOutOfRangeException>(standardDeviation >= 0);
		}

		public NormalDistribution(Random rnd)
			: this(rnd, 0, 1)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
		}

		public NormalDistribution(Random rnd, double mean, double standardDeviation)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			Contract.Requires<ArgumentOutOfRangeException>(standardDeviation >= 0);
			_rnd = rnd;
			_mean = mean;
			_std = standardDeviation;
		}

		public double Generate() => _rnd.Gaussian(_mean, _std);
	}
}
