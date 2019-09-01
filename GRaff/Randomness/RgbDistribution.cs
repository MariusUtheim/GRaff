using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
    /// <summary>
    /// Generator for a color distribution where the red, green and blue channels are independently distributed. 
    /// </summary>
    #warning Add more constructors
    public sealed class RgbDistribution : IDistribution<Color>
	{
		private readonly Random _rnd;

		public RgbDistribution()
			: this(GRandom.Source) { }

		public RgbDistribution(Random rnd)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			_rnd = rnd;
		}

		public Color Generate()
		{
			return _rnd.Color();
		}
	}
}
