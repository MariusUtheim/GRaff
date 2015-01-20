using System;
 

namespace GRaff.Randomness
{
	public sealed class RgbDistribution : IDistribution<Color>
	{
		private Random _rnd;

		public RgbDistribution()
			: this(GRandom.Source) { }

		public RgbDistribution(Random rnd)
		{
			_rnd = rnd;
		}

		public Color Generate()
		{
			return _rnd.Color();
		}
	}
}
