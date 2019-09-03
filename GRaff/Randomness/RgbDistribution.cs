using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
    /// <summary>
    /// Generator for a color distribution where the red, green and blue channels are independently distributed. 
    /// </summary>
    public sealed class RgbDistribution : IDistribution<Color>
	{
		public RgbDistribution()
			: this(GRandom.Source)
        { }

		public RgbDistribution(Random rnd)
            : this(new ByteDistribution(rnd), new ByteDistribution(rnd), new ByteDistribution(rnd))
		{ }

        public RgbDistribution(IDistribution<byte> red, IDistribution<byte> green, IDistribution<byte> blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public IDistribution<byte> Red { get; set; }

        public IDistribution<byte> Green { get; set; }

        public IDistribution<byte> Blue { get; set; }


		public Color Generate() => Color.FromRgb(Red.Generate(), Green.Generate(), Blue.Generate());
	}
}
