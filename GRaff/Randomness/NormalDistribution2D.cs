using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Randomness
{
	public sealed class NormalDistribution2D : IDistribution<Point>
	{
		Random _rnd;
		Point _mean;
		double _std;

		public NormalDistribution2D(Point mean, double standardDeviation)
			: this(GRandom.Source, mean, standardDeviation) { }

		public NormalDistribution2D(Random rnd, Point mean, double standardDeviation)
		{
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
