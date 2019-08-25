using System;
using System.Diagnostics.Contracts;
using GRaff.Randomness;

namespace GRaff.Graphics.Particles
{
	public class ColorDescriptor : IParticleTypeDescriptor
	{

		public ColorDescriptor()
			: this(new RgbDistribution())
		{ }

		public ColorDescriptor(Color color)
			: this(new ConstantDistribution<Color>(color))
		{ }

		public ColorDescriptor(IDistribution<Color> colorDistribution)
		{
			this.ColorDistribution = colorDistribution;
		}

        public static ColorDescriptor Uniform() => new ColorDescriptor();

        public IDistribution<Color> ColorDistribution { get; set; }

        class ColorBehavior : IParticleBehavior

        {
            public Color Color;

            public void Initialize(Particle particle) => particle.Blend = Color;
            public void Update(Particle particle) { }
        }

        public IParticleBehavior  MakeBehavior() => new ColorBehavior { Color = ColorDistribution.Generate() };
	}
}
