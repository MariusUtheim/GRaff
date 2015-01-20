using System;

namespace GRaff.Randomness
{
	public sealed class NormalDistribution : IDistribution<double>
	{
		Random _rnd;
		double _mean, _std;

		public NormalDistribution()
			: this(GRandom.Source, 0, 1) { }

		public NormalDistribution(double mean, double standardDeviation)
			: this(GRandom.Source, mean, standardDeviation) { }

		public NormalDistribution(Random rnd)
			: this(rnd, 0, 1) { }

		public NormalDistribution(Random rnd, double mean, double standardDeviation)
		{
			if (standardDeviation < 0)
			_rnd = rnd;
			_mean = mean;
			_std = standardDeviation;
		}

		public double Generate()
		{
			return _rnd.Gaussian(_mean, _std);
		}
	}
}
