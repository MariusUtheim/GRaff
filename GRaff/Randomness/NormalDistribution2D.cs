﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Randomness
{
	public sealed class NormalDistribution2D : IDistribution<Point>
	{
		private readonly Random _rnd;
		private readonly Point _mean;
		private readonly double _std;

		public NormalDistribution2D(Point mean, double standardDeviation)
			: this(GRandom.Source, mean, standardDeviation)
		{
			Contract.Requires<ArgumentOutOfRangeException>(standardDeviation >= 0);
		}

		public NormalDistribution2D(Random rnd, Point mean, double standardDeviation)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			Contract.Requires<ArgumentOutOfRangeException>(standardDeviation >= 0);
			_rnd = rnd;
			_mean = mean;
			_std = standardDeviation;
		}

		public Point Generate()
		{
			return _mean + _rnd.Vector() * (_rnd.Gaussian() * _std);
		}
	}
}
