using System;
using System.Diagnostics.Contracts;
using GRaff.Randomness;

namespace GRaff.Graphics.Particles
{
	public class ColorBehavior : IParticleBehavior
	{
		private IDistribution<Color> _color;

		public ColorBehavior()
			: this(new RgbDistribution())
		{ }

		public ColorBehavior(Color color)
			: this(new ConstantDistribution<Color>(color))
		{ }

		public ColorBehavior(IDistribution<Color> color)
		{
			Contract.Requires<ArgumentNullException>(color != null);
			this._color = color;
		}

		public void AttachTo(Particle particle)
		{
			particle.Blend = _color.Generate();
		}
	}
}
